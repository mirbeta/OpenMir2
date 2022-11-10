using LoginSvr.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packet;
using SystemModule.Packet.ServerPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace LoginSvr.Services
{
    public struct LoginPacket
    {
        public int SocketId;
        public byte[] Pakcet;
    }

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
        private readonly Channel<LoginPacket> _messageQueue;

        public LoginServer(MirLogger logger, ConfigManager configManager, ClientSession clientSession, ClientManager clientManager, SessionManager sessionManager)
        {
            _logger = logger;
            _clientSession = clientSession;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
            _config = configManager.Config;
            _messageQueue = Channel.CreateUnbounded<LoginPacket>();
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
            var gateInfo = new GateInfo();
            gateInfo.Socket = e.Socket;
            gateInfo.sIPaddr = LsShare.GetGatePublicAddr(_config, e.RemoteIPaddr);//应该改为按策略获取一个对外的公开网关地址
            gateInfo.UserList = new List<UserInfo>();
            gateInfo.KeepAliveTick = HUtil32.GetTickCount();
            _clientManager.AddSession(e.SocHandle, gateInfo);
            _logger.LogInformation($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]已链接.");
        }

        private void GSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            _clientManager.Delete(e.SocHandle);
            _logger.LogWarning($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
        }

        private void GSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.LogError(e.Exception);
        }

        private void GSocketClientRead(object sender, AsyncUserToken e)
        {
            var data = new byte[e.BytesReceived];
            Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, data.Length);
            _messageQueue.Writer.TryWrite(new LoginPacket()
            {
                SocketId = e.SocHandle,
                Pakcet = data
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
                    while (_messageQueue.Reader.TryRead(out var clientPacket))
                    {
                        try
                        {
                            var packet = Packets.ToPacket<ServerDataMessage>(clientPacket.Pakcet);
                            if (packet == null)
                            {
                                continue;
                            }
                            var gateInfo = _clientManager.GetSession(clientPacket.SocketId);
                            if (packet.Body != null)
                            {
                                if (packet.EndChar != '$' && packet.StartChar != '%')
                                {
                                    _logger.LogWarning("丢弃错误的封包数据");
                                    return;
                                }
                                switch (packet.Type)
                                {
                                    case ServerDataType.KeepAlive:
                                        SendKeepAlivePacket(gateInfo.Socket);
                                        gateInfo.KeepAliveTick = HUtil32.GetTickCount();
                                        break;
                                    case ServerDataType.Data:
                                        ProcessUserMessage(packet.SocketId, packet.Body);
                                        break;
                                    case ServerDataType.Enter:
                                        ReceiveOpenUser(packet.SocketId, packet.Body, gateInfo);
                                        break;
                                    case ServerDataType.Leave:
                                        ReceiveCloseUser(packet.SocketId, gateInfo);
                                        break;
                                }
                                _config.sGateIPaddr = gateInfo.sIPaddr;
                            }
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

        private void ProcessUserMessage(int sSockIndex, byte[] data)
        {
            var dataMsg = HUtil32.GetString(data, 0, data.Length);
            _clientSession.Enqueue(new UserSessionData()
            {
                SoketId = sSockIndex,
                Msg = dataMsg
            });
        }

        private void SendKeepAlivePacket(Socket socket)
        {
            if (socket.Connected)
            {
                socket.Send(HUtil32.GetBytes("%++$"));
            }
            _logger.DebugLog($"心跳消息 链接状态:[{socket.Connected}]");
        }

        private void ReceiveCloseUser(int sSockIndex, GateInfo gateInfo)
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
                        SessionDel(userInfo.SessionID);
                    }
                    gateInfo.UserList[i] = null;
                    gateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ReceiveOpenUser(int sSockIndex, byte[] data, GateInfo gateInfo)
        {
            if (data == null || data.Length <= 0)
            {
                return;
            }
            var sIPaddr = HUtil32.GetString(data, 0, data.Length);
            UserInfo userInfo;
            var sUserIPaddr = string.Empty;
            const string sOpenMsg = "Open: {0}/{1}";
            var sGateIPaddr = HUtil32.GetValidStr3(sIPaddr, ref sUserIPaddr, new[] { "/" });
            try
            {
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
                //_clientManager.AddSession(sSockIndex, gateInfo);
                _logger.DebugLog(string.Format(sOpenMsg, sUserIPaddr, sGateIPaddr));
            }
            catch (Exception ex)
            {
                _logger.LogError("[Exception] LoginService.ReceiveOpenUser ");
                _logger.LogError(ex);
            }
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void SessionClearKick()
        {
            var sessionList = _sessionManager.GetSessions.ToArray();
            for (var i = sessionList.Length - 1; i >= 0; i--)
            {
                var connInfo = sessionList[i];
                if (connInfo.Kicked && HUtil32.GetTickCount() - connInfo.dwKickTick > 5 * 1000)
                {
                    sessionList[i] = null;
                }
            }
        }

        private void SessionDel(int nSessionId)
        {
            _sessionManager.Delete(nSessionId);
            /*for (var i = 0; i < _config.SessionList.Count; i++)
            {
                if (_config.SessionList[i].SessionID == nSessionId)
                {
                    _config.SessionList[i] = null;
                    _config.SessionList.RemoveAt(i);
                    break;
                }
            }*/
        }
    }
}