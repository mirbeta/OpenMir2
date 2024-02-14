using LoginSrv.Conf;

namespace LoginSrv.Services
{
    /// <summary>
    /// 账号服务 处理来自LoginGate的客户端登陆 注册 等登陆封包消息
    /// 处理账号注册 登录 找回密码等
    /// </summary>
    public class LoginServer
    {
        private readonly TcpService _serverSocket;
        private readonly Config _config;
        private readonly ClientSession _clientSession;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;
        private readonly Channel<MessagePacket> _messageQueue;

        public LoginServer(ConfigManager configManager, ClientSession clientSession, ClientManager clientManager, SessionManager sessionManager)
        {
            _clientSession = clientSession;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
            _config = configManager.Config;
            _messageQueue = Channel.CreateUnbounded<MessagePacket>();
            _serverSocket = new TcpService();
            _serverSocket.Connected += Connecting;
            _serverSocket.Disconnected += Disconnected;
            _serverSocket.Received += Received;
        }

        public void StartServer()
        {
            TouchSocketConfig touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts(
            [
                new IPHost(IPAddress.Parse(_config.sGateAddr), _config.nGatePort)
            ]).SetTcpDataHandlingAdapter(() => new ServerPacketFixedHeaderDataHandlingAdapter());
            _serverSocket.Setup(touchSocketConfig);
            _serverSocket.Start();
            LogService.Info($"账号登陆服务[{_config.sGateAddr}:{_config.nGatePort}]已启动.");
        }

        private Task Received(SocketClient socketClient, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is not ServerDataMessageFixedHeaderRequestInfo fixedHeader)
            {
                return Task.CompletedTask;
            }
            ProcessGateData(fixedHeader.Header, fixedHeader.Message, socketClient.Id);
            return Task.CompletedTask;
        }

        private Task Connecting(object sender, TouchSocketEventArgs e)
        {
            SocketClient client = (SocketClient)sender;
            LoginGateInfo gateInfo = new LoginGateInfo();
            gateInfo.Socket = client.MainSocket;
            gateInfo.ConnectionId = client.Id;
            gateInfo.sIPaddr = LsShare.GetGatePublicAddr(_config, client.IP);//应该改为按策略获取一个对外的公开网关地址
            gateInfo.UserList = new List<UserInfo>();
            gateInfo.KeepAliveTick = HUtil32.GetTickCount();
            _clientManager.AddSession(client.Id, gateInfo);
            LogService.Info($"登录网关[{client.MainSocket.RemoteEndPoint}]已链接...");
            return Task.CompletedTask;
        }

        private Task Disconnected(object sender, DisconnectEventArgs e)
        {
            SocketClient client = (SocketClient)sender;
            _clientManager.Delete(client.Id);
            LogService.Warn($"登录网关[{client.ServiceIP}:{client.ServicePort}]断开链接.");
            return Task.CompletedTask;
        }

        private void ProcessGateData(ServerDataPacket packetHead, byte[] data, string socketId)
        {
            if (packetHead.PacketCode != Grobal2.PacketCode)
            {
                LogService.Debug($"解析登录网关封包出现异常...");
                return;
            }
            ServerDataMessage messageData = SerializerUtil.Deserialize<ServerDataMessage>(data);
            AddToQueue(socketId, messageData);
        }

        private void AddToQueue(string socketId, ServerDataMessage messageData)
        {
            _messageQueue.Writer.TryWrite(new MessagePacket()
            {
                ConnectionId = socketId,
                Pakcet = messageData
            });
        }

        /// <summary>
        /// 启动数据消费者线程
        /// </summary>
        /// <returns></returns>
        public void Start(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _messageQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    while (_messageQueue.Reader.TryRead(out MessagePacket message))
                    {
                        try
                        {
                            LoginGateInfo gateInfo = _clientManager.GetSession(message.ConnectionId);
                            switch (message.Pakcet.Type)
                            {
                                case ServerDataType.Data:
                                    ProcessUserMessage(message.ConnectionId, message.Pakcet);
                                    break;
                                case ServerDataType.KeepAlive:
                                    SendKeepAlivePacket(gateInfo.ConnectionId);
                                    gateInfo.KeepAliveTick = HUtil32.GetTickCount();
                                    break;
                                case ServerDataType.Enter:
                                    ReceiveOpenUser(message.Pakcet, gateInfo);
                                    break;
                                case ServerDataType.Leave:
                                    ReceiveCloseUser(message.Pakcet.SocketId, gateInfo);
                                    break;
                            }
                            _config.sGateIPaddr = gateInfo.sIPaddr;
                        }
                        catch (Exception e)
                        {
                            LogService.Error(e);
                        }
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            _clientSession.Start(stoppingToken);
        }

        private void ProcessUserMessage(string sessionId, ServerDataMessage dataMessage)
        {
            string dataMsg = HUtil32.GetString(dataMessage.Data, 0, dataMessage.DataLen);
            _clientSession.Enqueue(new UserSessionData()
            {
                SessionId = sessionId,
                SoketId = dataMessage.SocketId,
                Msg = dataMsg
            });
        }

        private void SendKeepAlivePacket(string connectionId)
        {
            ServerDataMessage messagePacket = new ServerDataMessage();
            messagePacket.Type = ServerDataType.KeepAlive;
            SendMessage(connectionId, SerializerUtil.Serialize(messagePacket));
        }

        private void ReceiveCloseUser(string sSockIndex, LoginGateInfo gateInfo)
        {
            const string sCloseMsg = "Close: {0}";
            for (int i = 0; i < gateInfo.UserList.Count; i++)
            {
                UserInfo userInfo = gateInfo.UserList[i];
                if (userInfo.SockIndex == sSockIndex)
                {
                    LogService.Debug(string.Format(sCloseMsg, userInfo.UserIPaddr));
                    if (!userInfo.SelServer)
                    {
                        SessionDel(userInfo.Account, userInfo.SessionID);
                    }
                    gateInfo.UserList[i] = null;
                    gateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ReceiveOpenUser(ServerDataMessage dataMessage, LoginGateInfo gateInfo)
        {
            string sSockIndex = dataMessage.SocketId;
            string sIPaddr = HUtil32.GetString(dataMessage.Data, 0, dataMessage.Data.Length);
            UserInfo userInfo;
            string sUserIPaddr = string.Empty;
            const string sOpenMsg = "Open: {0}/{1}";
            string sGateIPaddr = HUtil32.GetValidStr3(sIPaddr, ref sUserIPaddr, '/');
            for (int i = 0; i < gateInfo.UserList.Count; i++)
            {
                userInfo = gateInfo.UserList[i];
                if (userInfo.SockIndex == sSockIndex)
                {
                    userInfo.UserIPaddr = sUserIPaddr;
                    userInfo.GateIPaddr = sGateIPaddr;
                    userInfo.Account = string.Empty;
                    userInfo.SessionID = 0;
                    userInfo.ClientTick = HUtil32.GetTickCount();
                    break;
                }
            }
            userInfo = new UserInfo();
            userInfo.Account = string.Empty;
            userInfo.UserIPaddr = sUserIPaddr;
            userInfo.GateIPaddr = sGateIPaddr;
            userInfo.SockIndex = sSockIndex;
            userInfo.SessionID = 0;
            userInfo.Socket = gateInfo.Socket;
            userInfo.ClientTick = HUtil32.GetTickCount();
            userInfo.LastCreateAccountTick = HUtil32.GetTickCount();
            userInfo.LastUpdateAccountTick = HUtil32.GetTickCount();
            userInfo.LastUpdatePwdTick = HUtil32.GetTickCount();
            userInfo.LastGetBackPwdTick = HUtil32.GetTickCount();
            gateInfo.UserList.Add(userInfo);
            LogService.Debug(string.Format(sOpenMsg, sUserIPaddr, sGateIPaddr));
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void SessionClearKick()
        {
            SessionConnInfo[] sessionList = _sessionManager.GetSessions();
            if (sessionList != null)
            {
                for (int i = sessionList.Length - 1; i >= 0; i--)
                {
                    SessionConnInfo connInfo = sessionList[i];
                    if (connInfo.Kicked && (HUtil32.GetTickCount() - connInfo.KickTick) > 5 * 1000)
                    {
                        SessionDel(connInfo.Account, connInfo.SessionID);
                        sessionList[i] = null;
                    }
                }
            }
        }

        private void SessionDel(string account, int nSessionId)
        {
            _sessionManager.Delete(account, nSessionId);
        }

        private void SendMessage(string connectionId, byte[] sendBuffer)
        {
            ServerDataPacket serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.PacketCode,
                PacketLen = (ushort)sendBuffer.Length
            };
            byte[] dataBuff = SerializerUtil.Serialize(serverMessage);
            byte[] data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
            MemoryCopy.BlockCopy(dataBuff, 0, data, 0, data.Length);
            MemoryCopy.BlockCopy(sendBuffer, 0, data, dataBuff.Length, sendBuffer.Length);
            if (_serverSocket.SocketClientExist(connectionId))
            {
                _serverSocket.Send(connectionId, data);
            }
        }

        private struct MessagePacket
        {
            public string ConnectionId;
            public ServerDataMessage Pakcet;
        }
    }
}