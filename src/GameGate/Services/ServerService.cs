using GameGate.Conf;
using NLog;
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
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SocketServer _serverSocket;
        private readonly GameGateInfo GateInfo;
        private readonly ClientThread _clientThread;
        private readonly IPEndPoint _gateEndPoint;
        private readonly SendQueue messageSendQueue;
        private readonly ConcurrentQueue<int> SessionCloseQueue;
        private static SessionManager SessionMgr => SessionManager.Instance;
        private static ServerManager ServerMgr => ServerManager.Instance;

        public ServerService(GameGateInfo gameGate)
        {
            GateInfo = gameGate;
            _serverSocket = new SocketServer(GateShare.MaxSession, 500);
            SessionCloseQueue = new ConcurrentQueue<int>();
            messageSendQueue = new SendQueue();
            _gateEndPoint = IPEndPoint.Parse(string.Concat(gameGate.ServerAdress, ":", gameGate.GatePort));
            _clientThread = new ClientThread(_gateEndPoint, gameGate);
            ClientManager.Instance.AddClientThread(gameGate.ThreadId, _clientThread);
        }

        public ClientThread ClientThread => _clientThread;

        public void Start(CancellationToken stoppingToken)
        {
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
            _serverSocket.Start(_gateEndPoint);
            _clientThread.RestSessionArray();
            _clientThread.Start();
            messageSendQueue.StartProcessQueueSend(stoppingToken);
            _logger.Info($"游戏网关[{_gateEndPoint}]已启动...", 1);
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Shutdown();
        }

        public (string serverIp, string Status, string playCount, string reviceTotal, string sendTotal, string totalrevice, string totalSend, string queueCount, int threadCount) GetStatus()
        {
            return (_gateEndPoint.ToString(), _clientThread.GetConnected(), _clientThread.GetSessionCount(),
                _clientThread.ShowReceive(), _clientThread.ShowSend(),
                _clientThread.TotalReceive, _clientThread.TotalSend, GetSendQueueCount(), ServerManager.MessageWorkThreads);
        }

        /// <summary>
        /// 获取队列待处理数
        /// </summary>
        /// <returns></returns>
        private string GetSendQueueCount()
        {
            return messageSendQueue.QueueCount + "/" + SessionMgr.QueueCount;
        }

        /// <summary>
        /// 处理会话关闭列表
        /// </summary>
        public void ProcessCloseSessionQueue()
        {
            while (!SessionCloseQueue.IsEmpty)
            {
                SessionCloseQueue.TryDequeue(out var socketId);
                _clientThread.UserLeave(socketId); //发送消息给GameSvr断开链接
            }
        }

        public void Send(string connectionId, byte[] buffer)
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
                //todo 这里应该直接断开玩家连接，并在客户端尽兴提示
                _logger.Debug("获取GameSvr服务器实例失败，请确认GameGate和GameSvr是否链接正常。");
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
                    userSession.SessionIndex = 0;
                    userSession.ConnectionId = e.ConnectionId;
                    userSession.ReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = e.SocHandle;
                    userSession.SessionId = (ushort)nIdx;
                    userSession.ThreadId = threadId;
                    clientThread.SessionArray[nIdx] = userSession;
                    break;
                }
            }
            if (userSession != null)
            {
                e.SessionId = userSession.SessionId;
                clientThread.UserEnter(userSession.SessionId, userSession.SckHandle, sRemoteAddress); //通知GameSvr有新玩家进入游戏
                SessionMgr.AddSession(GateInfo.ServiceId, userSession.SessionId, new ClientSession(GateInfo.ServiceId, userSession, clientThread, messageSendQueue));
                _logger.Info("开始连接: " + sRemoteAddress);
                _logger.Debug($"GateId:{GateInfo.ServiceId} 用户 IP:[{sRemoteAddress}] SocketId:[{userSession.SessionId}] GameSrv:{clientThread.EndPoint}-{clientThread.ThreadId}");
            }
            else
            {
                e.Socket.Close();
                _logger.Info("禁止连接: " + sRemoteAddress);
            }
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddr = e.RemoteIPaddr;
            var clientSession = SessionMgr.GetSession(GateInfo.ServiceId, e.SessionId);
            if (clientSession == null)
            {
                return;
            }
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
                        clientThread.SessionArray[i].SessionIndex = 0;
                        clientThread.SessionArray[i].ReceiveTick = 0;
                        clientThread.SessionArray[i].SckHandle = 0;
                        clientThread.SessionArray[i].SessionId = 0;
                        clientThread.SessionArray[i].Socket = null;
                        clientThread.SessionArray[i] = null;
                        break;
                    }
                }
                _logger.Info("断开链接: " + sRemoteAddr);
            }
            else
            {
                _logger.Info("断开链接: " + sRemoteAddr);
            }
            SessionCloseQueue.Enqueue(e.SocHandle); //等待通知GameSvr断开用户会话,否则会出现退出游戏后再次登陆游戏提示账号已经登陆
            SessionMgr.CloseSession(e.SessionId);
            _logger.Debug($"用户断开链接 Ip:[{sRemoteAddr}] ConnectionId:[{e.ConnectionId}] ScoketId:[{e.SocHandle}]");
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.Debug($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var clientSession = SessionMgr.GetSession(GateInfo.ServiceId, token.SessionId);
            if (clientSession != null)
            {
                if (clientSession.Session == null)
                {
                    _logger.Debug($"ConnectionId:[{token.ConnectionId}] SocketId:[{token.ConnectionId}] Session会话已经失效");
                    return;
                }
                if (clientSession.Session.Socket == null)
                {
                    _logger.Debug($"ConnectionId:[{token.ConnectionId}] SocketId:[{token.ConnectionId}] Socket已释放");
                    return;
                }
                if (!clientSession.Session.Socket.Connected)
                {
                    _logger.Debug($"ConnectionId:[{token.ConnectionId}] SocketId:[{token.ConnectionId}] Socket链接已断开");
                    return;
                }
                var data = new byte[token.BytesReceived];
                Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
                ServerMgr.SendMessageQueue(new SessionMessage(token.SessionId, GateInfo.ServiceId, data, data.Length));
            }
            else
            {
                token.Socket.Close();
                _logger.Info("非法攻击: " + token.RemoteIPaddr);
            }
        }
    }
}