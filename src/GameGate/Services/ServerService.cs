using GameGate.Conf;
using NetworkMonitor = OpenMir2.NetworkMonitor;

namespace GameGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-GameGate)
    /// </summary>
    public class ServerService
    {
        private readonly TcpService _serverSocket;
        private readonly GameGateInfo _gateInfo;
        private readonly ClientThread _clientThread;
        private readonly IPEndPoint _gateEndPoint;
        private readonly SendQueue _messageSendQueue;
        private readonly ConcurrentQueue<int> _closeSession;
        private static SessionContainer SessionContainer => SessionContainer.Instance;
        private static ServerManager ServerMgr => ServerManager.Instance;
        private readonly NetworkMonitor _networkMonitor;
        private readonly ConcurrentDictionary<string, int> sessionMap = new ConcurrentDictionary<string, int>();

        public ServerService(GameGateInfo gameGate)
        {
            _gateInfo = gameGate;
            _networkMonitor = new NetworkMonitor();
            _closeSession = new ConcurrentQueue<int>();
            _messageSendQueue = new SendQueue();
            _gateEndPoint = new IPEndPoint(IPAddress.Parse(gameGate.ServerAdress), gameGate.GatePort);
            _clientThread = new ClientThread(_gateEndPoint, gameGate, _networkMonitor);
            _serverSocket = new TcpService();
            ClientManager.Instance.AddClientThread(gameGate.ThreadId, _clientThread);
        }

        public ClientThread ClientThread => _clientThread;
        public NetworkMonitor NetworkMonitor => _networkMonitor;
        public GameGateInfo GateInfo => _gateInfo;

        public void Initialize()
        {
            _serverSocket.Setup(new TouchSocketConfig()
                .SetListenIPHosts(new IPHost(IPAddress.Parse(_gateInfo.ServerAdress), _gateInfo.GatePort))
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();
                }));
            _serverSocket.Connecting += ServerSocketClientConnect;
            _serverSocket.Disconnected += ServerSocketClientDisconnect;
            _serverSocket.Received += ServerSocketClientRead;
            _clientThread.Initialize();
        }

        public void Start(CancellationToken stoppingToken)
        {
            _clientThread.Start();
            _clientThread.RestSessionArray();
            _messageSendQueue.StartProcessQueueSend(stoppingToken);
            _serverSocket.StartAsync();
            LogService.Info($"游戏网关[{_gateEndPoint}]已启动...");
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Stop();
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
            if (_closeSession.IsEmpty)
            {
                return;
            }
            for (int i = 0; i < _closeSession.Count; i++)
            {
                _closeSession.TryDequeue(out int socketId);
                _clientThread.UserLeave(socketId); //发送消息给GameSvr断开链接
            }
        }

        public Task Send(SessionMessage sessionMessage)
        {
            _networkMonitor.Send(sessionMessage.BuffLen);
            return _serverSocket.SendAsync(sessionMessage.ConnectionId, sessionMessage.Buffer, 0, sessionMessage.BuffLen);
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        private Task ServerSocketClientConnect(IClient client, ConnectingEventArgs e)
        {
            TcpClient connectedClient = client as TcpClient;
            string sRemoteAddress = connectedClient.GetIPPort();
            string clientId = e.Id;
            LogService.Debug($"客户端 IP:{sRemoteAddress} ThreadId:{GateInfo.ServiceId} SessionId:{e.Id} RunPort:{_gateEndPoint}");
            ClientThread clientThread = ServerMgr.GetClientThread(GateInfo.ServiceId, out int threadId);
            if (clientThread == null || threadId < 0)
            {
                //todo 直接断开玩家连接，提示客户端链接失败
                LogService.Debug("获取GameSvr服务器实例失败，请确认GameGate和GameSvr是否链接正常。");
                return Task.CompletedTask;
            }
            SessionInfo userSession = null;
            for (int nIdx = 0; nIdx < clientThread.SessionArray.Length; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession == null)
                {
                    userSession = new SessionInfo();
                    userSession.Socket = e.Socket;
                    userSession.SessionIndex = 0;
                    userSession.ConnectionId = clientId;
                    userSession.ReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = e.Socket.Handle.ToInt32();
                    userSession.SessionId = (ushort)nIdx;
                    userSession.ThreadId = threadId;
                    clientThread.SessionArray[nIdx] = userSession;
                    break;
                }
            }
            if (userSession != null)
            {
                //todo:获取客户端在clientThread中的会话Id
                ushort sessionId = clientThread.GetSessionId(clientId);
                clientThread.UserEnter(sessionId, userSession.SckHandle, sRemoteAddress); //通知GameSvr有新玩家进入游戏
                SessionContainer.AddSession(GateInfo.ServiceId, sessionId, new ClientSession(GateInfo.ServiceId, userSession, clientThread, _messageSendQueue));
                sessionMap.TryAdd(clientId, sessionId);
                LogService.Info("开始连接: " + sRemoteAddress);
                LogService.Info($"ThreadId:{GateInfo.ServiceId} IP:[{sRemoteAddress}] SessionId:[{userSession.SessionId}] GameSrv:{clientThread.EndPoint}/{clientThread.ThreadId}");
            }
            else
            {
                e.Socket.Close();
                LogService.Info("禁止连接: " + sRemoteAddress);
            }
            return Task.CompletedTask;
        }

        private Task ServerSocketClientDisconnect(ISocketClient client, DisconnectEventArgs e)
        {
            if (!sessionMap.TryGetValue(client.Id, out int sessionId))
            {
                return Task.CompletedTask;
            }
            string sRemoteAddress = client.IP;
            ClientSession clientSession = SessionContainer.GetSession(GateInfo.ServiceId, sessionId);
            if (clientSession == null)
            {
                return Task.CompletedTask;
            }
            ClientThread clientThread = clientSession.ServerThread;
            if (clientThread != null)
            {
                for (int i = 0; i < clientThread.SessionArray.Length; i++)
                {
                    if (clientThread.SessionArray[i] == null)
                    {
                        continue;
                    }
                    if (clientThread.SessionArray[i].ConnectionId == client.Id)
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
                LogService.Info("断开链接: " + sRemoteAddress);
            }
            else
            {
                LogService.Info("断开链接: " + sRemoteAddress);
            }
            _closeSession.Enqueue(client.MainSocket.Handle.ToInt32()); //等待通知GameSvr断开用户会话,否则会出现退出游戏后再次登陆游戏提示账号已经登陆
            SessionContainer.CloseSession(GateInfo.ServiceId, sessionId);
            LogService.Debug($"用户断开链接 Ip:[{sRemoteAddress}] ThreadId:{_gateInfo.ServiceId} SessionId:{sessionId}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        private Task ServerSocketClientRead(ISocketClient client, ReceivedDataEventArgs e)
        {
            if (!sessionMap.TryGetValue(client.Id, out int sessionId))
            {
                return Task.CompletedTask;
            }
            string sRemoteAddress = client.IP;
            ClientSession clientSession = SessionContainer.GetSession(GateInfo.ServiceId, sessionId);
            if (clientSession != null)
            {
                //var buff = new IntPtr(NativeMemory.AllocZeroed((uint)token.BytesReceived));
                //MemoryCopy.BlockCopy(token.ReceiveBuffer, token.Offset, buff.ToPointer(), 0, token.BytesReceived);
                byte[] buff = new byte[e.ByteBlock.Len];
                MemoryCopy.BlockCopy(e.ByteBlock.Buffer, 0, buff, 0, e.ByteBlock.Len);
                ServerMgr.SendMessageQueue(new ClientPacketMessage(GateInfo.ServiceId, sessionId, buff, (ushort)buff.Length));
                _networkMonitor.Receive(e.ByteBlock.Len);
            }
            else
            {
                client.Close();
                LogService.Debug("非法攻击: " + sRemoteAddress);
            }
            return Task.CompletedTask;
        }
    }
}