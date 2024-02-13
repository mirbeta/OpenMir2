using GameGate.Conf;
using OpenMir2.DataHandlingAdapters;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Channels;
using NetworkMonitor = OpenMir2.NetworkMonitor;

namespace GameGate.Services
{
    /// <summary>
    /// 网关客户端(GameGate-GameSrv)
    /// </summary>
    public class ClientThread
    {
        private readonly TcpClient ClientSocket;
        private readonly GameGateInfo GateInfo;
        private readonly IPEndPoint LocalEndPoint;
        /// <summary>
        /// 用户会话
        /// </summary>
        public readonly SessionInfo[] SessionArray = new SessionInfo[GateShare.MaxSession];
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        private bool CheckServerFail { get; set; }
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        private int CheckServerFailCount { get; set; }
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        private bool GateReady { get; set; }
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool Connected { get; set; }
        /// <summary>
        /// 历史最高在线人数
        /// </summary>
        private int OnlineCount { get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        private RunningState RunningState { get; set; }
        private int CheckServerTick { get; set; }
        /// <summary>
        /// Session管理
        /// </summary>
        private static SessionContainer SessionContainer => SessionContainer.Instance;
        private readonly NetworkMonitor _networkMonitor;
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<ServerSessionMessage> _messageChannel;

        public ClientThread(IPEndPoint endPoint, GameGateInfo gameGate, NetworkMonitor networkMonitor)
        {
            GateInfo = gameGate;
            RunningState = RunningState.Waiting;
            Connected = false;
            LocalEndPoint = endPoint;
            CheckServerTick = HUtil32.GetTickCount();
            _networkMonitor = networkMonitor;
            _messageChannel = Channel.CreateUnbounded<ServerSessionMessage>();
            ClientSocket = new TcpClient();
            ClientSocket.Connected += ClientSocketConnect;
            ClientSocket.Disconnected += ClientSocketDisconnect;
            ClientSocket.Received += ClientSocketRead;
        }

        public bool IsConnected => Connected;

        public string EndPoint => $"{GateInfo.ServerAdress}:{GateInfo.ServerPort}";

        public byte ThreadId => GateInfo.ServiceId;

        public RunningState Running => RunningState;

        /// <summary>
        /// 返回等待处理的消息数量
        /// </summary>
        public int QueueCount => _messageChannel.Reader.Count;

        public void Initialize()
        {
            TouchSocketConfig config = new TouchSocketConfig();
            config.SetRemoteIPHost(new IPHost(IPAddress.Parse(GateInfo.ServerAdress), GateInfo.ServerPort));
            config.SetTcpDataHandlingAdapter(() => new PacketFixedHeaderDataHandlingAdapter());
            config.ConfigurePlugins(x =>
            {
                x.UseReconnection();
            });
            ClientSocket.Setup(config);
        }

        public void Start()
        {
            try
            {
                ClientSocket.Connect();
            }
            catch (SocketException error)
            {
                ClientSocketError(null, error.SocketErrorCode);
            }
            catch (TimeoutException)
            {
                ClientSocketError(null, SocketError.TimedOut);
            }
            catch (Exception)
            {
                ClientSocketError(null, SocketError.SocketError);
            }
        }

        public void Stop()
        {
            ClientSocket.Close();
        }

        public ushort GetSessionId(string connectionId)
        {
            int length = 4;
            byte[] randomNumberBytes = new byte[length + 1];
            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumberBytes);
            }

            byte checksum = 0;
            foreach (byte randomNumberByte in randomNumberBytes)
            {
                checksum ^= randomNumberByte;
            }

            randomNumberBytes[length] = checksum;

            StringBuilder builder = new StringBuilder();
            foreach (byte randomNumberByte in randomNumberBytes)
            {
                builder.Append(randomNumberByte % 10);
            }

            if (ushort.TryParse(builder.ToString(), out ushort sessionId))
            {
                return sessionId;
            }
            return 0;
        }

        public string GetSessionCount()
        {
            int sessionCount = 0;
            for (int i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
                {
                    sessionCount++;
                }
            }
            if (sessionCount > OnlineCount)
            {
                OnlineCount = sessionCount;
            }
            return sessionCount + "/" + OnlineCount;
        }

        private Task ClientSocketConnect(IClient client, ConnectedEventArgs e)
        {
            IPHost endPoint = ((TcpClientBase)client).RemoteIPHost;
            GateReady = true;
            CheckServerTick = HUtil32.GetTickCount();
            Connected = true;
            RunningState = RunningState.Runing;
            RestSessionArray();
            LogService.Info($"游戏网关[{LocalEndPoint}] 游戏引擎[{endPoint.EndPoint}]链接成功.");
            LogService.Debug($"线程[{Guid.NewGuid():N}]连接 {endPoint} 成功...");
            return Task.CompletedTask;
        }

        private Task ClientSocketDisconnect(IClient client, DisconnectEventArgs e)
        {
            TcpClientBase socSocket = ((TcpClientBase)client);
            for (int i = 0; i < GateShare.MaxSession; i++)
            {
                SessionInfo userSession = SessionArray[i];
                if (userSession != null)
                {
                    if (userSession.Socket != null && userSession.Socket == socSocket.MainSocket)
                    {
                        userSession.Socket.Close();
                        userSession.Socket = null;
                        userSession.SckHandle = -1;
                    }
                }
            }
            RestSessionArray();
            GateReady = false;
            LogService.Info($"游戏网关[{LocalEndPoint}] 游戏引擎[{socSocket.RemoteIPHost.EndPoint}]断开链接.");
            Connected = false;
            CheckServerFail = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 接收GameSvr发来的封包消息
        /// </summary>
        private Task ClientSocketRead(IClient client, ReceivedDataEventArgs e)
        {
            try
            {
                if (e.RequestInfo is DataMessageFixedHeaderRequestInfo requestInfo)
                {
                    ProcessServerPacket(requestInfo.Header, requestInfo.Message);
                    _networkMonitor.Receive(requestInfo.BodyLength);
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message);
            }
            return Task.CompletedTask;
        }

        private void ClientSocketError(object sender, SocketError e)
        {
            switch (e)
            {
                case SocketError.ConnectionRefused:
                    LogService.Warn($"游戏网关[{LocalEndPoint}] 链接游戏引擎[{EndPoint}]拒绝链接...");
                    Connected = false;
                    break;
                case SocketError.ConnectionReset:
                    LogService.Info($"游戏引擎[{EndPoint}]主动关闭连接游戏网关[{LocalEndPoint}]...");
                    Connected = false;
                    break;
                case SocketError.TimedOut:
                    LogService.Info($"游戏网关[{LocalEndPoint}] 链接游戏引擎[{EndPoint}]超时...");
                    Connected = false;
                    break;
                default:
                    LogService.Info($"游戏网关[{LocalEndPoint}] 链接游戏引擎[{EndPoint}]失败...");
                    Connected = false;
                    break;
            }
            GateReady = false;
            CheckServerFail = true;
        }

        private void ProcessServerPacket(ServerMessage packetHeader, ReadOnlySpan<byte> data)
        {
            switch (packetHeader.Ident)
            {
                case Grobal2.GM_STOP: //游戏引擎停止服务,网关不在接收或分配用户连接到该游戏引擎
                    RunningState = RunningState.Stop; //停止分配后，10分钟内不允许尝试连接服务器
                    break;
                case Grobal2.GM_CHECKSERVER:
                    CheckServerFail = false;
                    CheckServerTick = HUtil32.GetTickCount();
                    break;
                case Grobal2.GM_SERVERUSERINDEX:
                    ClientSession userSession = SessionContainer.GetSession(ThreadId, packetHeader.SessionId);
                    if (userSession != null)
                    {
                        userSession.SvrListIdx = packetHeader.SessionIndex;
                    }
                    break;
                case Grobal2.GM_RECEIVE_OK:
                    SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, "", 0);
                    break;
                case Grobal2.GM_DATA:
                    unsafe
                    {
                        int packetLen = packetHeader.PackLength < 0 ? -packetHeader.PackLength : packetHeader.PackLength;
                        ServerSessionMessage sendMsg = new ServerSessionMessage();
                        sendMsg.SessionId = packetHeader.SessionId;
                        sendMsg.BuffLen = (short)packetHeader.PackLength;
                        //sendMsg.Buffer = GateShare.BytePool.Rent(packetLen);
                        sendMsg.Buffer = new byte[packetLen];
                        MemoryCopy.BlockCopy(data, 0, sendMsg.Buffer, 0, packetLen);
                        Enqueue(sendMsg);
                    }
                    break;
                case Messages.GM_TEST:
                    break;
            }
        }

        /// <summary>
        /// 添加到消息处理队列
        /// </summary>
        private void Enqueue(ServerSessionMessage sessionPacket)
        {
            _messageChannel.Writer.TryWrite(sessionPacket);
        }

        /// <summary>
        /// 转发GameSvr封包消息
        /// </summary>
        public Task StartMessageQueue(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
               {
                   while (await _messageChannel.Reader.WaitToReadAsync(stoppingToken))
                   {
                       if (_messageChannel.Reader.TryRead(out ServerSessionMessage message))
                       {
                           ClientSession userSession = SessionContainer.GetSession(ThreadId, message.SessionId);
                           if (userSession == null)
                           {
                               continue;
                           }
                           try
                           {
                               userSession.ProcessServerPacket(ThreadId, message);
                           }
                           catch (Exception ex)
                           {
                               LogService.Error(ex.Message);
                           }
                           finally
                           {
                               //GateShare.BytePool.Return(message.Buffer);
                           }
                       }
                   }
               }, stoppingToken);
        }

        public void RestSessionArray()
        {
            for (int i = 0; i < GateShare.MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].Socket = null;
                    SessionArray[i].SessionIndex = 0;
                    SessionArray[i].ReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].SckHandle = 0;
                    SessionArray[i].SessionId = 0;
                }
            }
        }

        private void SendServerMsg(ushort command, ushort sessionId, int nSocket, ushort userIndex, string data, int nLen)
        {
            ServerMessage serverMessage = new ServerMessage
            {
                PacketCode = Grobal2.PacketCode,
                Socket = nSocket,
                SessionId = sessionId,
                Ident = command,
                SessionIndex = userIndex,
                PackLength = nLen
            };
            byte[] sendBuffer = SerializerUtil.Serialize(serverMessage);
            if (!string.IsNullOrEmpty(data))
            {
                byte[] strBuff = HUtil32.GetBytes(data);
                byte[] tempBuff = new byte[ServerMessage.PacketSize + data.Length];
                MemoryCopy.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
                MemoryCopy.BlockCopy(strBuff, 0, tempBuff, sendBuffer.Length, data.Length);
                Send(tempBuff);
            }
            else
            {
                Send(sendBuffer);
            }
        }

        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        public void UserEnter(ushort sessionId, int socketId, string data)
        {
            SendServerMsg(Grobal2.GM_OPEN, sessionId, socketId, 0, data, data.Length);
        }

        /// <summary>
        /// 玩家退出游戏
        /// </summary>
        public void UserLeave(int socketId)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, socketId, 0, "", 0);
        }

        /// <summary>
        /// 发送消息到GameSvr
        /// </summary>
        /// <param name="sendBuffer"></param>
        internal void Send(byte[] sendBuffer)
        {
            if (ClientSocket.Online)
            {
                ClientSocket.Send(sendBuffer);
                _networkMonitor.Send(sendBuffer.Length);
            }
        }

        /// <summary>
        /// 处理超时或空闲会话
        /// </summary>
        public void ProcessIdleSession()
        {
            int currentTick = HUtil32.GetTickCount();
            for (int j = 0; j < SessionArray.Length; j++)
            {
                SessionInfo userSession = SessionArray[j];
                if (userSession != null && userSession.Socket != null)
                {
                    if ((currentTick - userSession.ReceiveTick) > GateShare.SessionTimeOutTime) //清理超时用户会话 
                    {
                        userSession.Socket.Close();
                        userSession.SckHandle = -1;
                        userSession.Socket = null;
                    }
                }
            }
        }

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        public void CheckConnectedState()
        {
            if (Running == RunningState.Stop) //停止服务后暂时停止心跳连接检查
            {
                if ((HUtil32.GetTickCount() - CheckServerTick) > 60 * 10000) //10分钟分不允许尽兴链接服务器
                {
                    Start();
                    LogService.Debug($"游戏引擎维护时间结束,重新连接游戏引擎[{EndPoint}].");
                }
                return;
            }
            if (GateReady)
            {
                SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, "", 0);
                CheckServerFailCount = 0;
                return;
            }
            if (CheckServerFail && CheckServerFailCount <= ushort.MaxValue)
            {
                Start();
                CheckServerFailCount++;
                LogService.Debug($"链接服务器[{EndPoint}] 失败次数[{CheckServerFailCount}]");
                return;
            }
            if (CheckServerFailCount >= ushort.MaxValue)
            {
                LogService.Debug("超过最大重试次数，请重启程序后再次确认链接是否正常。");
                return;
            }
            CheckServerTimeOut();
        }

        private void CheckServerTimeOut()
        {
            if ((HUtil32.GetTickCount() - CheckServerTick) > GateShare.CheckServerTimeOutTime && CheckServerFailCount <= ushort.MaxValue)
            {
                CheckServerFail = true;
                Stop();
                CheckServerFailCount++;
                LogService.Debug($"服务器[{EndPoint}]长时间没有响应,断开链接.失败次数:[{CheckServerFailCount}]");
            }
        }

        public string ConnectedState => IsConnected ? "[green]Connected[/]" : "[red]Not Connected[/]";
    }
}