using LoginSvr.Conf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace LoginSvr.Services
{
    /// <summary>
    /// 账号服务 处理来自LoginGate的客户端登陆 注册 等登陆封包消息
    /// 处理账号注册 登录 找回密码等
    /// </summary>
    public class LoginServer
    {
        private readonly SocketServer _serverSocket;
        private readonly MirLogger _logger;
        private readonly Config _config;
        private readonly ClientSession _clientSession;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;
        private readonly Channel<MessagePacket> _messageQueue;
        private readonly Dictionary<string, LoginGateInfo> _loginGateMap = new Dictionary<string, LoginGateInfo>();

        public LoginServer(MirLogger logger, ConfigManager configManager, ClientSession clientSession, ClientManager clientManager, SessionManager sessionManager)
        {
            _logger = logger;
            _clientSession = clientSession;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
            _config = configManager.Config;
            _messageQueue = Channel.CreateUnbounded<MessagePacket>();
            _serverSocket = new SocketServer(short.MaxValue, 2048);
            _serverSocket.OnClientConnect += GSocketClientConnect;
            _serverSocket.OnClientDisconnect += GSocketClientDisconnect;
            _serverSocket.OnClientRead += GSocketClientRead;
            _serverSocket.OnClientError += GSocketClientError;
        }

        public void StartServer()
        {
            _serverSocket.Init();
            _serverSocket.Start(_config.sGateAddr, _config.nGatePort);
            _logger.LogInformation($"账号登陆服务[{_config.sGateAddr}:{_config.nGatePort}]已启动.");
        }

        private void GSocketClientConnect(object sender, AsyncUserToken e)
        {
            var gateInfo = new LoginGateInfo();
            gateInfo.Socket = e.Socket;
            gateInfo.ConnectionId = e.ConnectionId;
            gateInfo.sIPaddr = LsShare.GetGatePublicAddr(_config, e.RemoteIPaddr);//应该改为按策略获取一个对外的公开网关地址
            gateInfo.UserList = new List<UserInfo>();
            gateInfo.KeepAliveTick = HUtil32.GetTickCount();
            _clientManager.AddSession(e.SocHandle, gateInfo);
            _logger.LogInformation($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]已链接.");
            _loginGateMap.Add(e.ConnectionId, gateInfo);
        }

        private void GSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            _loginGateMap.Remove(e.ConnectionId);
            _clientManager.Delete(e.SocHandle);
            _logger.LogWarning($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
        }

        private void GSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.LogError(e.Exception);
        }

        private void GSocketClientRead(object sender, AsyncUserToken e)
        {
            try
            {
                if (_loginGateMap.TryGetValue(e.ConnectionId, out var gateInfo))
                {
                    var nMsgLen = e.BytesReceived;
                    if (gateInfo.DataLen > 0)
                    {
                        var packetData = new byte[e.BytesReceived];
                        Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, packetData, 0, nMsgLen);
                        MemoryCopy.BlockCopy(packetData, 0, gateInfo.Data, gateInfo.DataLen, packetData.Length);
                        ProcessGateData(gateInfo.Data, gateInfo.DataLen + nMsgLen, e.SocHandle, ref gateInfo);
                    }
                    else
                    {
                        Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, gateInfo.Data, 0, nMsgLen);
                        ProcessGateData(gateInfo.Data, nMsgLen, e.SocHandle, ref gateInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        private void ProcessGateData(byte[] data, int nLen, int socketId, ref LoginGateInfo gateInfo)
        {
            var srcOffset = 0;
            Span<byte> dataBuff = data;
            while (nLen > ServerDataPacket.FixedHeaderLen)
            {
                var packetHead = dataBuff[..ServerDataPacket.FixedHeaderLen];
                var message = ServerPacket.ToPacket<ServerDataPacket>(packetHead);
                if (message.PacketCode != Grobal2.RUNGATECODE)
                {
                    srcOffset++;
                    dataBuff = dataBuff.Slice(srcOffset, ServerDataPacket.FixedHeaderLen);
                    nLen -= 1;
                    _logger.DebugLog($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                    continue;
                }
                var nCheckMsgLen = Math.Abs(message.PacketLen + ServerDataPacket.FixedHeaderLen);
                if (nCheckMsgLen > nLen)
                {
                    break;
                } 
                var messageData = SerializerUtil.Deserialize<ServerDataMessage>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
                AddToQueue(socketId, messageData);
                nLen -= nCheckMsgLen;
                if (nLen <= 0)
                {
                    break;
                }
                dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
                gateInfo.DataLen = nLen;
                srcOffset = 0;
                if (nLen < ServerDataPacket.FixedHeaderLen)
                {
                    break;
                }
            }
            if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
            {
                MemoryCopy.BlockCopy(dataBuff, 0, gateInfo.Data, 0, nLen);
                gateInfo.DataLen = nLen;
            }
            else
            {
                gateInfo.DataLen = 0;
            }
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
            using var memoryStream = new MemoryStream();
            using var backingStream = new BinaryWriter(memoryStream);
            var serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                PacketLen = (short)sendBuffer.Length
            };
            backingStream.Write(serverMessage.GetBuffer());
            backingStream.Write(sendBuffer);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            if (_serverSocket.IsOnline(connectionId))
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