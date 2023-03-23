using LoginSrv.Conf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.DataHandlingAdapters;
using SystemModule.Logger;
using SystemModule.Packets.ServerPackets;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace LoginSrv.Services
{
    /// <summary>
    /// 账号服务 处理来自LoginGate的客户端登陆 注册 等登陆封包消息
    /// 处理账号注册 登录 找回密码等
    /// </summary>
    public class LoginServer
    {
        private readonly TcpService _serverSocket;
        private readonly MirLogger _logger;
        private readonly Config _config;
        private readonly ClientSession _clientSession;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;
        private readonly Channel<MessagePacket> _messageQueue;
        private readonly LoginGateInfo[] loginGates = new LoginGateInfo[20];

        public LoginServer(MirLogger logger, ConfigManager configManager, ClientSession clientSession, ClientManager clientManager, SessionManager sessionManager)
        {
            _logger = logger;
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
            var touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts(new IPHost[1]
            {
                new IPHost(IPAddress.Parse(_config.sGateAddr), _config.nGatePort)
            }).SetDataHandlingAdapter(() => new ServerPacketFixedHeaderDataHandlingAdapter());
            _serverSocket.Setup(touchSocketConfig);
            _serverSocket.Start();
            _logger.LogInformation($"账号登陆服务[{_config.sGateAddr}:{_config.nGatePort}]已启动.");
        }
        
        private void Received(object sender, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (requestInfo is not ServerDataMessageFixedHeaderRequestInfo fixedHeader)
                return;
            var client = (SocketClient)sender;
            if (int.TryParse(client.ID, out var clientId))
            {
                ProcessGateData(fixedHeader.Header, fixedHeader.Message, clientId);
            }
            else
            {
                //_logger.Info("未知客户端...");
            }
        }

        private void Connecting(object sender, TouchSocketEventArgs e)
        {
            var client = (SocketClient)sender;
            var clientId = int.Parse(client.ID);
            var gateInfo = new LoginGateInfo();
            gateInfo.Socket = client.MainSocket;
            gateInfo.ConnectionId = client.ID;
            gateInfo.sIPaddr = LsShare.GetGatePublicAddr(_config, client.MainSocket.RemoteEndPoint.GetIP());//应该改为按策略获取一个对外的公开网关地址
            gateInfo.UserList = new List<UserInfo>();
            gateInfo.KeepAliveTick = HUtil32.GetTickCount();
            _clientManager.AddSession(clientId, gateInfo);
            _logger.LogInformation($"登录网关[{client.MainSocket.RemoteEndPoint}]已链接...");
            loginGates[clientId] = gateInfo;
        }

        private void Disconnected(object sender, DisconnectEventArgs e)
        {
            var client = (SocketClient)sender;
            var clientId = int.Parse(client.ID);
            loginGates[clientId] = null;
            _clientManager.Delete(clientId);
            _logger.LogWarning($"登录网关[{client.MainSocket.RemoteEndPoint}]断开链接.");
        }
        
        private void ProcessGateData(ServerDataPacket packetHead,byte[] data, int socketId)
        {
            if (packetHead.PacketCode != Grobal2.PacketCode)
            {
                _logger.DebugLog($"解析登录网关封包出现异常...");
                return;
            }
            var messageData = SerializerUtil.Deserialize<ServerDataMessage>(data);
            AddToQueue(socketId, messageData);
        }

        private void AddToQueue(int socketId, ServerDataMessage messageData)
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
                    while (_messageQueue.Reader.TryRead(out var loginPacket))
                    {
                        try
                        {
                            var gateInfo = _clientManager.GetSession(loginPacket.ConnectionId);
                            switch (loginPacket.Pakcet.Type)
                            {
                                case ServerDataType.Data:
                                    ProcessUserMessage(loginPacket.ConnectionId, loginPacket.Pakcet);
                                    break;
                                case ServerDataType.KeepAlive:
                                    SendKeepAlivePacket(gateInfo.ConnectionId);
                                    gateInfo.KeepAliveTick = HUtil32.GetTickCount();
                                    break;
                                case ServerDataType.Enter:
                                    ReceiveOpenUser(loginPacket.Pakcet, gateInfo);
                                    break;
                                case ServerDataType.Leave:
                                    ReceiveCloseUser(loginPacket.Pakcet.SocketId, gateInfo);
                                    break;
                            }
                            _config.sGateIPaddr = gateInfo.sIPaddr;
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e);
                        }
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            _clientSession.Start(stoppingToken);
        }

        private void ProcessUserMessage(int sessionId, ServerDataMessage dataMessage)
        {
            var dataMsg = HUtil32.GetString(dataMessage.Data, 0, dataMessage.DataLen);
            _clientSession.Enqueue(new UserSessionData()
            {
                SessionId = sessionId,
                SoketId = dataMessage.SocketId,
                Msg = dataMsg
            });
        }

        private void SendKeepAlivePacket(string connectionId)
        {
            var messagePacket = new ServerDataMessage();
            messagePacket.Type = ServerDataType.KeepAlive;
            SendMessage(connectionId, SerializerUtil.Serialize(messagePacket));
        }
        
        private void ReceiveCloseUser(int sSockIndex, LoginGateInfo gateInfo)
        {
            const string sCloseMsg = "Close: {0}";
            for (var i = 0; i < gateInfo.UserList.Count; i++)
            {
                var userInfo = gateInfo.UserList[i];
                if (userInfo.SockIndex == sSockIndex)
                {
                    _logger.DebugLog(string.Format(sCloseMsg, userInfo.UserIPaddr));
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
            var sSockIndex = dataMessage.SocketId;
            var sIPaddr = HUtil32.GetString(dataMessage.Data, 0, dataMessage.Data.Length);
            UserInfo userInfo;
            var sUserIPaddr = string.Empty;
            const string sOpenMsg = "Open: {0}/{1}";
            var sGateIPaddr = HUtil32.GetValidStr3(sIPaddr, ref sUserIPaddr, '/');
            for (var i = 0; i < gateInfo.UserList.Count; i++)
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
            _logger.DebugLog(string.Format(sOpenMsg, sUserIPaddr, sGateIPaddr));
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void SessionClearKick()
        {
            var sessionList = _sessionManager.GetSessions();
            if (sessionList != null)
            {
                for (var i = sessionList.Length - 1; i >= 0; i--)
                {
                    var connInfo = sessionList[i];
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
            var serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.PacketCode,
                PacketLen = (ushort)sendBuffer.Length
            };
            var dataBuff = SerializerUtil.Serialize(serverMessage);
            var data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
            MemoryCopy.BlockCopy(dataBuff, 0, data, 0, data.Length);
            MemoryCopy.BlockCopy(sendBuffer, 0, data, dataBuff.Length, sendBuffer.Length);
            if (_serverSocket.SocketClientExist(connectionId))
            {
                _serverSocket.Send(connectionId, data);
            }
        }

        private struct MessagePacket
        {
            public int ConnectionId;
            public ServerDataMessage Pakcet;
        }
    }
}