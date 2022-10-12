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
    /// 客户端服务端(MirClient-GameGate)
    /// </summary>
    public class ServerService
    {
        private readonly SocketServer _serverSocket;
        private readonly ClientThread _clientThread;
        private readonly IPEndPoint _gateEndPoint;
        private readonly SendQueue _sendQueue;
        private readonly ConcurrentQueue<int> _waitCloseQueue;
        private static MirLog LogQueue => MirLog.Instance;
        private static SessionManager SessionManager => SessionManager.Instance;
        private static ClientManager ClientManager => ClientManager.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;

        public ServerService(string clientId, GameGateInfo gameGate)
        {
            _waitCloseQueue = new ConcurrentQueue<int>();
            _serverSocket = new SocketServer(ushort.MaxValue, 500);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
            _sendQueue = new SendQueue();
            _gateEndPoint = IPEndPoint.Parse(string.Concat(gameGate.ServerAdress, ":", gameGate.GatePort));
            _clientThread = new ClientThread(clientId, _gateEndPoint, gameGate);
        }

        public ClientThread ClientThread => _clientThread;

        public void Start(CancellationToken stoppingToken)
        {
            _serverSocket.Start(_gateEndPoint);
            _clientThread.Start();
            _clientThread.RestSessionArray();
            _sendQueue.ProcessSendQueue(stoppingToken);
            LogQueue.Enqueue($"网关[{_gateEndPoint}]已启动...", 1);
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Shutdown();
        }

        public (string serverIp,  string Status, string playCount, string reviceTotal, string sendTotal, string queueCount, int threadCount) GetStatus()
        {
            return (_gateEndPoint.ToString(), _clientThread.GetConnected(), _clientThread.GetSessionCount(), _clientThread.GetReceiveInfo(), _clientThread.GetSendInfo(), GetSendQueueCount(), GetThreadCount());
        }

        /// <summary>
        /// 获取消息处理线程数量,最少1个
        /// </summary>
        /// <returns></returns>
        private int GetThreadCount()
        {
            return ServerManager.MessageThreadCount;
        }

        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        /// <returns></returns>
        private string GetSendQueueCount()
        {
            return string.Concat(_sendQueue.GetQueueCount, "/", SessionManager.GetQueueCount);
        }

        /// <summary>
        /// 处理等待关闭链接列表
        /// </summary>
        public void ProcessCloseList()
        {
            while (!_waitCloseQueue.IsEmpty)
            {
                _waitCloseQueue.TryDequeue(out int socket);
                _clientThread.UserLeave(socket); //发送消息给M2断开链接
            }
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var clientThread = ServerManager.GetClientThread();
            if (clientThread == null)
            {
                LogQueue.EnqueueDebugging("获取服务器实例失败。");
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            LogQueue.EnqueueDebugging($"用户[{sRemoteAddress}]分配到游戏数据服务器[{clientThread.ClientId}] Server:{clientThread.GetSocketIp()}");
            TSessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.SessionArray.Length; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession == null)
                {
                    userSession = new TSessionInfo();
                    userSession.Socket = e.Socket;
                    userSession.nUserListIndex = 0;
                    userSession.ConnectionId = e.ConnectionId;
                    userSession.dwReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = e.SocHandle;
                    userSession.SessionId = (ushort)e.SocHandle;
                    clientThread.SessionArray[nIdx] = userSession;
                    break;
                }
            }
            if (userSession != null)
            {
                LogQueue.Enqueue("开始连接: " + sRemoteAddress, 5);
                clientThread.UserEnter(userSession.SessionId, userSession.SckHandle, sRemoteAddress); //通知M2有新玩家进入游戏
                SessionManager.AddSession(userSession.SessionId, new ClientSession(userSession, clientThread, _sendQueue));
                ClientManager.AddClientThread(userSession.ConnectionId, clientThread);
            }
            else
            {
                e.Socket.Close();
                LogQueue.Enqueue("禁止连接: " + sRemoteAddress, 1);
            }
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddr = e.RemoteIPaddr;
            var connectionId = e.ConnectionId;
            var clientThread = ClientManager.GetClientThread(connectionId);
            if (clientThread != null && clientThread.GateReady)
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
                        clientThread.SessionArray[i].dwReceiveTick = HUtil32.GetTickCount();
                        clientThread.SessionArray[i].SckHandle = 0;
                        clientThread.SessionArray[i].SessionId = 0;
                        clientThread.SessionArray[i] = null;
                        break;
                    }
                }
                _waitCloseQueue.Enqueue(e.SocHandle); //等待100ms才能发送给M2
                LogQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
            }
            else
            {
                LogQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
                LogQueue.EnqueueDebugging($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{connectionId}]");
            }
            ClientManager.DeleteClientThread(connectionId);
            SessionManager.CloseSession(e.SocHandle);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            LogQueue.EnqueueDebugging($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var connectionId = token.SocHandle;
            var clientSession = SessionManager.GetSession(connectionId);
            if (clientSession != null)
            {
                if (clientSession.Session == null)
                {
                    LogQueue.Enqueue($"[{connectionId}] Session会话已经失效", 5);
                    return;
                }
                if (clientSession.Session.Socket == null)
                {
                    LogQueue.Enqueue($"[{connectionId}] Socket已释放", 5);
                    return;
                }
                if (!clientSession.Session.Socket.Connected)
                {
                    return;
                }
                var data = new byte[token.BytesReceived];
                Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
                var message = new TMessageData();
                message.Buffer = data;
                message.MessageId = connectionId;
                message.BufferLen = data.Length;
                ServerManager.SendQueue(message);
            }
            else
            {
                token.Socket.Close();
                LogQueue.Enqueue("非法攻击: " + token.RemoteIPaddr, 5);
            }
        }
    }
}