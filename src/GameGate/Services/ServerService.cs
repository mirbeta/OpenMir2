using GameGate.Conf;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace GameGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-GameGate)
    /// </summary>
    public class ServerService
    {
        private readonly SocketServer _serverSocket;
        private readonly ClientThread _clientThread;
        private readonly IPEndPoint _gateEndPoint;
        private readonly SendQueue _sendQueue;
        private readonly ConcurrentQueue<int> _waitCloseQueue;
        private static MirLog LogQueue => MirLog.Instance;
        private static SessionManager SessionMgr => SessionManager.Instance;
        private static ServerManager ServerMgr => ServerManager.Instance;

        public ServerService(GameGateInfo gameGate)
        {
            var clientId = Guid.NewGuid().ToString("N");
            _waitCloseQueue = new ConcurrentQueue<int>();
            _serverSocket = new SocketServer(GateShare.MaxSession, 500);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
            _sendQueue = new SendQueue();
            _gateEndPoint = IPEndPoint.Parse(string.Concat(gameGate.ServerAdress, ":", gameGate.GatePort));
            _clientThread = new ClientThread(clientId, _gateEndPoint, gameGate);
            ClientManager.Instance.AddClientThread(clientId, _clientThread);
        }

        public ClientThread ClientThread => _clientThread;

        public void Start(CancellationToken stoppingToken)
        {
            _serverSocket.Start(_gateEndPoint);
            _clientThread.Start();
            _clientThread.RestSessionArray();
            _sendQueue.ProcessSendQueue(stoppingToken);
            LogQueue.Log($"游戏网关[{_gateEndPoint}]已启动...", 1);
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Shutdown();
        }

        public (string serverIp,  string Status, string playCount, string reviceTotal, string sendTotal, string queueCount, int threadCount) GetStatus()
        {
            return (_gateEndPoint.ToString(), _clientThread.GetConnected(), _clientThread.GetSessionCount(), _clientThread.GetReceiveInfo(), _clientThread.GetSendInfo(), GetSendQueueCount(), GetMessageWorkThreads());
        }

        /// <summary>
        /// 获取消息处理线程数量,最少1个
        /// </summary>
        /// <returns></returns>
        private static int GetMessageWorkThreads()
        {
            return ServerManager.MessageWorkThreads;
        }

        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        /// <returns></returns>
        private string GetSendQueueCount()
        {
            return string.Concat(_sendQueue.QueueCount, "/", SessionMgr.QueueCount);
        }

        /// <summary>
        /// 关闭等待链接列表
        /// </summary>
        public void CloseWaitList()
        {
            while (!_waitCloseQueue.IsEmpty)
            {
                _waitCloseQueue.TryDequeue(out int socket);
                _clientThread.UserLeave(socket); //发送消息给GameSvr断开链接
            }
        }

        public void Send(string connectionId, Span<byte> buffer)
        {
            _serverSocket.Send(connectionId, buffer);
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var threadId = -1;
            var clientThread = ServerMgr.GetClientThread(out threadId);
            if (clientThread == null || threadId < 0)
            {
                LogQueue.DebugLog("获取GameSvr服务器实例失败，请确认GameGate和GameSvr是否链接正常。");
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            SessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.SessionArray.Length; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession == null)
                {
                    userSession = new SessionInfo();
                    userSession.Socket = e.Socket;
                    userSession.nUserListIndex = 0;
                    userSession.ConnectionId = e.ConnectionId;
                    userSession.dwReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = e.SocHandle;
                    userSession.SessionId = (ushort)e.SocHandle;
                    userSession.ThreadId = threadId;
                    clientThread.SessionArray[nIdx] = userSession;
                    break;
                }
            }
            if (userSession != null)
            {
                clientThread.UserEnter(userSession.SessionId, userSession.SckHandle, sRemoteAddress); //通知GameSvr有新玩家进入游戏
                SessionMgr.AddSession(userSession.SessionId, new ClientSession(userSession, clientThread, _sendQueue));
                LogQueue.Log("开始连接: " + sRemoteAddress, 5);
                LogQueue.DebugLog($"新用户 IP:[{sRemoteAddress}] SocketId:[{userSession.SessionId}]分配到游戏数据服务器[{clientThread.ClientId}] Server:{clientThread.EndPoint}");
            }
            else
            {
                e.Socket.Close();
                LogQueue.Log("禁止连接: " + sRemoteAddress, 1);
            }
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddr = e.RemoteIPaddr;
            var clientSession = SessionMgr.GetSession(e.SocHandle);
            var clientThread = clientSession.ServerThread;
            if (clientThread != null)
            {
                for (int i = 0; i < clientThread.SessionArray.Length; i++)
                {
                    if (clientThread.SessionArray[i] == null)
                    {
                        continue;
                    }
                    if (clientThread.SessionArray[i].SckHandle == e.SocHandle)
                    {
                        clientThread.SessionArray[i].Socket.Close();
                        clientThread.SessionArray[i].Socket = null;
                        clientThread.SessionArray[i].nUserListIndex = 0;
                        clientThread.SessionArray[i].dwReceiveTick = 0;
                        clientThread.SessionArray[i].SckHandle = 0;
                        clientThread.SessionArray[i].SessionId = 0;
                        clientThread.SessionArray[i] = null;
                        break;
                    }
                }
                LogQueue.Log("断开链接: " + sRemoteAddr, 5);
            }
            else
            {
                LogQueue.Log("断开链接: " + sRemoteAddr, 5);
            }
            _waitCloseQueue.Enqueue(e.SocHandle); //等待100ms才通知GameSvr断开用户会话,否则会出现退出游戏后再次登陆游戏提示账号已经登陆
            SessionMgr.CloseSession(e.SocHandle);
            LogQueue.DebugLog($"用户断开链接 Ip:[{sRemoteAddr}] ConnectionId:[{e.ConnectionId}] ScoketId:[{e.SocHandle}]");
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            LogQueue.DebugLog($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var clientSession = SessionMgr.GetSession(token.SocHandle);
            if (clientSession != null)
            {
                if (clientSession.Session == null)
                {
                    LogQueue.Log($"ConnectionId:[{token.ConnectionId}] SocketId:[{token.ConnectionId}] Session会话已经失效", 5);
                    return;
                }
                if (clientSession.Session.Socket == null)
                {
                    LogQueue.Log($"ConnectionId:[{token.ConnectionId}] SocketId:[{token.ConnectionId}] Socket已释放", 5);
                    return;
                }
                if (!clientSession.Session.Socket.Connected)
                {
                    LogQueue.Log($"ConnectionId:[{token.ConnectionId}] SocketId:[{token.ConnectionId}] Socket链接已断开", 5);
                    return;
                }
                var data = new byte[token.BytesReceived];
                Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
                var message = new MessagePacket
                {
                    Buffer = data,
                    SessionId = token.SocHandle,
                    BufferLen = data.Length
                };
                ServerMgr.SendMessageQueue(message);
            }
            else
            {
                token.Socket.Close();
                LogQueue.Log("非法攻击: " + token.RemoteIPaddr, 5);
            }
        }
    }
}