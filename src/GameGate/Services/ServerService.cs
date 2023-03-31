using GameGate.Conf;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;
using NetworkMonitor = SystemModule.NetworkMonitor;

namespace GameGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-GameGate)
    /// </summary>
    public class ServerService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SocketServer _serverSocket;
        private readonly GameGateInfo _gateInfo;
        private readonly ClientThread _clientThread;
        private readonly IPEndPoint _gateEndPoint;
        private readonly SendQueue _messageSendQueue;
        private readonly ConcurrentQueue<int> _sessionCloseQueue;
        private static SessionContainer SessionContainer => SessionContainer.Instance;
        private static ServerManager ServerMgr => ServerManager.Instance;
        private readonly NetworkMonitor _networkMonitor;

        public ServerService(GameGateInfo gameGate)
        {
            _gateInfo = gameGate;
            _networkMonitor = new NetworkMonitor();
            _sessionCloseQueue = new ConcurrentQueue<int>();
            _messageSendQueue = new SendQueue();
            _gateEndPoint = new IPEndPoint(IPAddress.Parse(gameGate.ServerAdress), gameGate.GatePort);
            _clientThread = new ClientThread(_gateEndPoint, gameGate, _networkMonitor);
            _serverSocket = new SocketServer(GateShare.MaxSession, 256);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            ClientManager.Instance.AddClientThread(gameGate.ThreadId, _clientThread);
        }

        public ClientThread ClientThread => _clientThread;
        public NetworkMonitor NetworkMonitor => _networkMonitor;
        public GameGateInfo GateInfo => _gateInfo;

        public void Initialize()
        {
            _serverSocket.Init(20);
            _clientThread.Initialize();
        }

        public async Task Start(CancellationToken stoppingToken)
        {
            _serverSocket.Start(_gateEndPoint);
            _clientThread.RestSessionArray();
            _messageSendQueue.StartProcessQueueSend(stoppingToken);
            await _clientThread.StartMessageQueue(stoppingToken);
            _logger.Info($"游戏网关[{_gateEndPoint}]已启动...");
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Shutdown();
        }

        public (string serverIp, string Status, string playCount, string reviceTotal, string sendTotal, string totalrevice, string totalSend, string queueCount, int threadCount) GetStatus()
        {
            return (_gateEndPoint.ToString(), _clientThread.ConnectedState, _clientThread.GetSessionCount(), ShowReceive, ShowSend, TotalReceive, TotalSend, GetQueueStatus, ServerManager.MessageWorkThreads);
        }

        private string ShowReceive => $"↓{_networkMonitor.ShowReceive()}";

        private string ShowSend => $"↑{_networkMonitor.ShowSendStats()}";

        private string TotalReceive => $"↓{HUtil32.FormatBytesValue(_networkMonitor.TotalBytesRecv)}";

        private string TotalSend => $"↑{HUtil32.FormatBytesValue(_networkMonitor.TotalBytesSent)}";

        /// <summary>
        /// 获取队列待处理数
        /// </summary>
        /// <returns></returns>
        private string GetQueueStatus => _messageSendQueue.QueueCount + "/" + _clientThread.QueueCount;

        /// <summary>
        /// 处理会话关闭列表
        /// </summary>
        public void ProcessCloseSessionQueue()
        {
            if (_sessionCloseQueue.IsEmpty)
            {
                return;
            }
            for (var i = 0; i < _sessionCloseQueue.Count; i++)
            {
                _sessionCloseQueue.TryDequeue(out var socketId);
                _clientThread.UserLeave(socketId); //发送消息给GameSvr断开链接
            }
        }

        public void Send(SessionMessage sessionMessage)
        {
            _serverSocket.Send(sessionMessage.ConnectionId, sessionMessage.Buffer, sessionMessage.BuffLen);
            _networkMonitor.Send(sessionMessage.BuffLen);
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddress = e.RemoteIPaddr;
            var clientId = e.ConnectionId;
            _logger.Debug($"客户端 IP:{sRemoteAddress} ThreadId:{GateInfo.ServiceId} SessionId:{clientId} RunPort:{_gateEndPoint}");
            var clientThread = ServerMgr.GetClientThread(GateInfo.ServiceId, out var threadId);
            if (clientThread == null || threadId < 0)
            {
                //todo 直接断开玩家连接，提示客户端链接失败
                _logger.Debug("获取GameSvr服务器实例失败，请确认GameGate和GameSvr是否链接正常。");
                return;
            }
            SessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.SessionArray.Length; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession == null)
                {
                    userSession = new SessionInfo();
                    userSession.Socket = e.Socket;
                    userSession.SessionIndex = 0;
                    userSession.ConnectionId = clientId;
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
                clientThread.UserEnter((ushort)clientId, userSession.SckHandle, sRemoteAddress); //通知GameSvr有新玩家进入游戏
                SessionContainer.AddSession(GateInfo.ServiceId, clientId, new ClientSession(GateInfo.ServiceId, userSession, clientThread, _messageSendQueue));
                _logger.Trace("开始连接: " + sRemoteAddress);
                _logger.Debug($"GateId:{GateInfo.ServiceId} 用户 IP:[{sRemoteAddress}] SocketId:[{userSession.SessionId}] GameSrv:{clientThread.EndPoint}/{clientThread.ThreadId}");
            }
            else
            {
                e.Socket.Close();
                _logger.Trace("禁止连接: " + sRemoteAddress);
            }
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddress = e.RemoteIPaddr;
            var clientId = e.ConnectionId;
            var clientSession = SessionContainer.GetSession(GateInfo.ServiceId, clientId);
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
                    if (clientThread.SessionArray[i].ConnectionId == clientId)
                    {
                        clientThread.SessionArray[i].Socket?.Close();
                        clientThread.SessionArray[i].SessionIndex = 0;
                        clientThread.SessionArray[i].ReceiveTick = 0;
                        clientThread.SessionArray[i].SckHandle = 0;
                        clientThread.SessionArray[i].SessionId = 0;
                        clientThread.SessionArray[i].Socket = null;
                        clientThread.SessionArray[i] = null;
                        break;
                    }
                }
                _logger.Trace("断开链接: " + sRemoteAddress);
            }
            else
            {
                _logger.Trace("断开链接: " + sRemoteAddress);
            }
            _sessionCloseQueue.Enqueue(e.SocHandle); //等待通知GameSvr断开用户会话,否则会出现退出游戏后再次登陆游戏提示账号已经登陆
            SessionContainer.CloseSession(GateInfo.ServiceId, clientId);
            _logger.Debug($"用户断开链接 Ip:[{sRemoteAddress}] ThreadId:{_gateInfo.ServiceId} SessionId:{e.ConnectionId}");
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.Debug($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        private unsafe void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var sRemoteAddress = token.RemoteIPaddr;
            var sessionId = token.ConnectionId;
            var clientSession = SessionContainer.GetSession(GateInfo.ServiceId, sessionId);
            if (clientSession != null)
            {
                var buff = new IntPtr(NativeMemory.AllocZeroed((uint)token.BytesReceived));
                MemoryCopy.BlockCopy(token.ReceiveBuffer, token.Offset, buff.ToPointer(), 0, token.BytesReceived);
                ServerMgr.SendMessageQueue(new ClientPacketMessage(GateInfo.ServiceId, sessionId, buff, (ushort)token.BytesReceived));
                _networkMonitor.Receive(token.BytesReceived);
            }
            else
            {
                token.Socket.Close();
                _logger.Trace("非法攻击: " + sRemoteAddress);
            }
        }
    }
}