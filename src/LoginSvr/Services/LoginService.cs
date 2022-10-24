using LoginSvr.Conf;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ServerPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace LoginSvr.Services
{
    /// <summary>
    /// 账号服务
    /// 处理账号注册 登录 找回密码等
    /// </summary>
    public class LoginService
    {
        private readonly SocketServer _serverSocket;
        private readonly MirLog _logger;
        private readonly DataService _masSocService;
        private readonly ConfigManager _configManager;
        private readonly ClientSession _clientSession;
        private readonly Channel<byte[]> _messageQueue;

        public LoginService(DataService masSocService, MirLog logger, ConfigManager configManager, ClientSession clientSession)
        {
            _masSocService = masSocService;
            _logger = logger;
            _configManager = configManager;
            _clientSession = clientSession;
            _messageQueue = Channel.CreateUnbounded<byte[]>();
            _serverSocket = new SocketServer(short.MaxValue, 2048);
            _serverSocket.OnClientConnect += GSocketClientConnect;
            _serverSocket.OnClientDisconnect += GSocketClientDisconnect;
            _serverSocket.OnClientRead += GSocketClientRead;
            _serverSocket.OnClientError += GSocketClientError;
        }

        public void Start()
        {
            _serverSocket.Init();
            _serverSocket.Start(_configManager.Config.sGateAddr, _configManager.Config.nGatePort);
            _logger.Information($"账号登陆服务[{_configManager.Config.sGateAddr}:{_configManager.Config.nGatePort}]已启动.");
        }

        private void GSocketClientConnect(object sender, AsyncUserToken e)
        {
            var gateInfo = new GateInfo();
            gateInfo.Socket = e.Socket;
            gateInfo.sIPaddr = LsShare.GetGatePublicAddr(_configManager.Config, e.RemoteIPaddr);
            gateInfo.UserList = new List<UserInfo>();
            gateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
            LsShare.Gates.Add(gateInfo);
            _logger.Information($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]已链接.");
        }

        private void GSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < LsShare.Gates.Count; i++)
            {
                var gateInfo = LsShare.Gates[i];
                if (gateInfo.Socket == e.Socket)
                {
                    for (var j = 0; j < gateInfo.UserList.Count; j++)
                    {
                        _logger.LogDebug("Close: " + gateInfo.UserList[j].UserIPaddr);
                        gateInfo.UserList[j] = null;
                    }
                    gateInfo.UserList = null;
                    LsShare.Gates.Remove(gateInfo);
                    break;
                }
            }
            _logger.Information($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
        }

        private void GSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.LogError(e.Exception);
        }

        private void GSocketClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < LsShare.Gates.Count; i++)
            {
                if (LsShare.Gates[i].Socket == e.Socket)
                {
                    var data = new byte[e.BytesReceived];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, data.Length);
                    _messageQueue.Writer.TryWrite(data);
                    break;
                }
            }
        }

        /// <summary>
        /// 启动数据消费者线程
        /// </summary>
        /// <returns></returns>
        public void Start(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                await ProcessGateMessage(stoppingToken);
            }, stoppingToken);
            _clientSession.Start(stoppingToken);
        }

        /// <summary>
        /// 处理网关消息
        /// </summary>
        /// <returns></returns>
        private async Task ProcessGateMessage(CancellationToken stoppingToken)
        {
            while (await _messageQueue.Reader.WaitToReadAsync(stoppingToken))
            {
                while (_messageQueue.Reader.TryRead(out var message))
                {
                    try
                    {
                        var packet = Packets.ToPacket<GatePacket>(message);
                        ProcessGateData(packet);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e);
                    }
                }
            }
        }

        private void ProcessGateData(GatePacket packet)
        {
            var I = 0;
            while (true)
            {
                if (LsShare.Gates.Count <= I)
                {
                    break;
                }
                var gateInfo = LsShare.Gates[I];
                if (packet.Body != null && gateInfo.UserList != null)
                {
                    if (packet.EndChar != '$' && packet.StartChar != '%')
                    {
                        Console.WriteLine("丢弃错误的封包数据");
                        break;
                    }
                    switch (packet.Type)
                    {
                        case PacketType.KeepAlive:
                            SendKeepAlivePacket(gateInfo.Socket);
                            gateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
                            break;
                        case PacketType.Data:
                            var dataMsg = HUtil32.GetString(packet.Body, 0, packet.Body.Length);
                            ProcessUserMessage(packet.SocketId, gateInfo, dataMsg);
                            break;
                        case PacketType.Enter:
                            var endterMsg = HUtil32.GetString(packet.Body, 0, packet.Body.Length);
                            ReceiveOpenUser(packet.SocketId, endterMsg, gateInfo);
                            break;
                        case PacketType.Leave:
                            ReceiveCloseUser(packet.SocketId, gateInfo);
                            break;
                    }
                    _configManager.Config.sGateIPaddr = gateInfo.sIPaddr;
                }
                I++;
            }
        }

        private void SendKeepAlivePacket(Socket socket)
        {
            if (socket.Connected)
            {
                socket.Send(HUtil32.GetBytes("%++$"));
            }
            _logger.LogDebug($"心跳消息 链接状态:[{socket.Connected}]");
        }

        private void ReceiveCloseUser(string sSockIndex, GateInfo gateInfo)
        {
            const string sCloseMsg = "Close: {0}";
            for (var i = 0; i < gateInfo.UserList.Count; i++)
            {
                var userInfo = gateInfo.UserList[i];
                if (userInfo.SockIndex == sSockIndex)
                {
                    _logger.LogDebug(string.Format(sCloseMsg, userInfo.UserIPaddr));
                    if (!userInfo.SelServer)
                    {
                        SessionDel(_configManager.Config, userInfo.SessionID);
                    }
                    gateInfo.UserList[i] = null;
                    gateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ReceiveOpenUser(string sSockIndex, string sIPaddr, GateInfo gateInfo)
        {
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
                userInfo.nVersionDate = 0;
                userInfo.boCertificationOK = false;
                userInfo.SessionID = 0;
                userInfo.Socket = gateInfo.Socket;
                userInfo.ClientTick = HUtil32.GetTickCount();
                userInfo.Gate = gateInfo;
                gateInfo.UserList.Add(userInfo);
                _logger.LogDebug(string.Format(sOpenMsg, sUserIPaddr, sGateIPaddr));
            }
            catch (Exception ex)
            {
                _logger.Information("[Exception] LoginService.ReceiveOpenUser " + ex.Source);
            }
        }

        private void ProcessUserMessage(string sSockIndex, GateInfo gateInfo, string sData)
        {
            for (var i = 0; i < gateInfo.UserList.Count; i++)
            {
                var userInfo = gateInfo.UserList[i];
                if (userInfo.SockIndex == sSockIndex)
                {
                    _clientSession.SendToQueue(new UserSessionData()
                    {
                        UserInfo = userInfo,
                        Msg = sData
                    });
                    break;
                }
            }
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void SessionClearKick()
        {
            var config = _configManager.Config;
            for (var i = config.SessionList.Count - 1; i >= 0; i--)
            {
                var connInfo = config.SessionList[i];
                if (connInfo.boKicked && HUtil32.GetTickCount() - connInfo.dwKickTick > 5 * 1000)
                {
                    config.SessionList[i] = null;
                    config.SessionList.RemoveAt(i);
                }
            }
        }

        private void SessionDel(Config config, int nSessionId)
        {
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                if (config.SessionList[i].nSessionID == nSessionId)
                {
                    config.SessionList[i] = null;
                    config.SessionList.RemoveAt(i);
                    break;
                }
            }
        }
        
        public void LoadIPaddrCostList(Config config, AccountConst accountConst)
        {
            config.IPaddrCostList.Clear();
            config.IPaddrCostList.Add(accountConst.s1C, accountConst.nC);
        }

        public void LoadAccountCostList(Config config, AccountConst accountConst)
        {
            config.AccountCostList.Clear();
            config.AccountCostList.Add(accountConst.s1C, accountConst.nC);
        }
    }
}