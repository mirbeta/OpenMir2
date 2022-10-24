using LoginSvr.Conf;
using LoginSvr.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;
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
        private readonly AccountStorage _accountStorage;
        private readonly MasSocService _masSocService;
        private readonly ConfigManager _configManager;
        private readonly Channel<GatePacket> _messageQueue;
        private readonly Channel<ReceiveUserData> _userMessageQueue;
        private readonly IList<GateInfo> _gateList;

        public LoginService(MasSocService masSocService, MirLog logger, AccountStorage accountStorage, ConfigManager configManager)
        {
            _masSocService = masSocService;
            _logger = logger;
            _accountStorage = accountStorage;
            _configManager = configManager;
            _messageQueue = Channel.CreateUnbounded<GatePacket>();
            _userMessageQueue = Channel.CreateUnbounded<ReceiveUserData>();
            _gateList = new List<GateInfo>();
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
            gateInfo.UserList = new List<TUserInfo>();
            gateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
            _gateList.Add(gateInfo);
            _logger.Information($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]已链接.");
        }

        private void GSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _gateList.Count; i++)
            {
                var gateInfo = _gateList[i];
                if (gateInfo.Socket == e.Socket)
                {
                    for (var j = 0; j < gateInfo.UserList.Count; j++)
                    {
                        _logger.LogDebug("Close: " + gateInfo.UserList[j].sUserIPaddr);
                        gateInfo.UserList[j] = null;
                    }
                    gateInfo.UserList = null;
                    _gateList.Remove(gateInfo);
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
            for (var i = 0; i < _gateList.Count; i++)
            {
                if (_gateList[i].Socket == e.Socket)
                {
                    var data = new byte[e.BytesReceived];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, data.Length);
                    var packet = Packets.ToPacket<GatePacket>(data);
                    _messageQueue.Writer.TryWrite(packet);
                    break;
                }
            }
        }

        /// <summary>
        /// 是否付费账号
        /// </summary>
        /// <returns></returns>
        private bool IsPayMent(Config config, string sIPaddr, string sAccount)
        {
            return config.AccountCostList.ContainsKey(sAccount) || config.IPaddrCostList.ContainsKey(sIPaddr);
        }

        /// <summary>
        /// 启动数据消费者线程
        /// </summary>
        /// <returns></returns>
        public void StartThread(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () => { await ProcessGateMessage(stoppingToken); }, stoppingToken);
            Task.Factory.StartNew(async () => { await ProcessUserMessage(stoppingToken); }, stoppingToken);
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
                    ProcessGateData(message);
                }
            }
        }

        /// <summary>
        /// 处理封包消息
        /// </summary>
        /// <returns></returns>
        private async Task ProcessUserMessage(CancellationToken stoppingToken)
        {
            while (await _userMessageQueue.Reader.WaitToReadAsync(stoppingToken))
            {
                while (_userMessageQueue.Reader.TryRead(out var message))
                {
                    DecodeUserData(message.UserInfo, message.Msg);
                }
            }
        }

        private void DecodeUserData(TUserInfo userInfo, string userData)
        {
            var sMsg = string.Empty;
            try
            {
                if (!userData.EndsWith("!"))
                {
                    return;
                }
                HUtil32.ArrestStringEx(userData, "#", "!", ref sMsg);
                if (string.IsNullOrEmpty(sMsg))
                    return;
                if (sMsg.Length < Grobal2.DEFBLOCKSIZE)
                    return;
                sMsg = sMsg.Substring(1, sMsg.Length - 1);
                ProcessUserMsg(userInfo, sMsg);
            }
            catch (Exception ex)
            {
                _logger.Information("[Exception] LoginService.DecodeUserData");
                _logger.Information(ex.StackTrace);
            }
        }

        private void ProcessGateData(GatePacket packet)
        {
            var I = 0;
            while (true)
            {
                if (_gateList.Count <= I)
                {
                    break;
                }
                var gateInfo = _gateList[I];
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
                            ReceiveSendUser(packet.SocketId, gateInfo, dataMsg);
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
                socket.Send(new LoginSvrPacket()
                {
                    ConnectionId = socket.Handle.ToString(),
                    PackLen = 0,
                    ClientPacket = Array.Empty<byte>()
                }.GetBuffer());
            }
            _logger.Information($"心跳消息 链接状态:[{socket.Connected}]");
        }

        private void ReceiveCloseUser(string sSockIndex, GateInfo gateInfo)
        {
            const string sCloseMsg = "Close: {0}";
            for (var i = 0; i < gateInfo.UserList.Count; i++)
            {
                var userInfo = gateInfo.UserList[i];
                if (userInfo.sSockIndex == sSockIndex)
                {
                    _logger.LogDebug(string.Format(sCloseMsg, userInfo.sUserIPaddr));
                    if (!userInfo.boSelServer)
                    {
                        SessionDel(_configManager.Config, userInfo.nSessionID);
                    }
                    gateInfo.UserList[i] = null;
                    gateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ReceiveOpenUser(string sSockIndex, string sIPaddr, GateInfo gateInfo)
        {
            TUserInfo userInfo;
            var sUserIPaddr = string.Empty;
            const string sOpenMsg = "Open: {0}/{1}";
            var sGateIPaddr = HUtil32.GetValidStr3(sIPaddr, ref sUserIPaddr, new[] { "/" });
            try
            {
                for (var i = 0; i < gateInfo.UserList.Count; i++)
                {
                    userInfo = gateInfo.UserList[i];
                    if (userInfo.sSockIndex == sSockIndex)
                    {
                        userInfo.sUserIPaddr = sUserIPaddr;
                        userInfo.sGateIPaddr = sGateIPaddr;
                        userInfo.sAccount = string.Empty;
                        userInfo.nSessionID = 0;
                        userInfo.dwClientTick = HUtil32.GetTickCount();
                        break;
                    }
                }
                userInfo = new TUserInfo();
                userInfo.sAccount = string.Empty;
                userInfo.sUserIPaddr = sUserIPaddr;
                userInfo.sGateIPaddr = sGateIPaddr;
                userInfo.sSockIndex = sSockIndex;
                userInfo.nVersionDate = 0;
                userInfo.boCertificationOK = false;
                userInfo.nSessionID = 0;
                userInfo.Socket = gateInfo.Socket;
                userInfo.dwClientTick = HUtil32.GetTickCount();
                userInfo.Gate = gateInfo;
                gateInfo.UserList.Add(userInfo);
                _logger.LogDebug(string.Format(sOpenMsg, sUserIPaddr, sGateIPaddr));
            }
            catch (Exception ex)
            {
                _logger.Information("[Exception] LoginService.ReceiveOpenUser " + ex.Source);
            }
        }

        private void ReceiveSendUser(string sSockIndex, GateInfo gateInfo, string sData)
        {
            for (var i = 0; i < gateInfo.UserList.Count; i++)
            {
                var userInfo = gateInfo.UserList[i];
                if (userInfo.sSockIndex == sSockIndex)
                {
                    _userMessageQueue.Writer.TryWrite(new ReceiveUserData()
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

        private void ProcessUserMsg(TUserInfo userInfo, string sMsg)
        {
            var sDefMsg = sMsg.Substring(0, Grobal2.DEFBLOCKSIZE);
            var sData = sMsg.Substring(Grobal2.DEFBLOCKSIZE, sMsg.Length - Grobal2.DEFBLOCKSIZE);
            var defMsg = EDCode.DecodePacket(sDefMsg);
            switch (defMsg.Ident)
            {
                case Grobal2.CM_SELECTSERVER:
                    if (!userInfo.boSelServer)
                    {
                        AccountSelectServer(_configManager.Config, userInfo, sData);
                    }
                    break;
                case Grobal2.CM_PROTOCOL:
                    AccountCheckProtocol(userInfo, defMsg.Recog);
                    break;
                case Grobal2.CM_IDPASSWORD:
                    if (string.IsNullOrEmpty(userInfo.sAccount))
                    {
                        AccountLogin(_configManager.Config, userInfo, sData);
                    }
                    else
                    {
                        KickUser(_configManager.Config, ref userInfo);
                    }
                    break;
                case Grobal2.CM_ADDNEWUSER:
                    if (_configManager.Config.boEnableMakingID)
                    {
                        if (HUtil32.GetTickCount() - userInfo.dwClientTick > 5000)
                        {
                            AccountCreate(ref userInfo, sData);
                        }
                        else
                        {
                            _logger.Information("[超速操作] 创建帐号/" + userInfo.sUserIPaddr);
                        }
                    }
                    break;
                case Grobal2.CM_CHANGEPASSWORD:
                    if (string.IsNullOrEmpty(userInfo.sAccount))
                    {
                        if (HUtil32.GetTickCount() - userInfo.dwClientTick > 5000)
                        {
                            userInfo.dwClientTick = HUtil32.GetTickCount();
                            AccountChangePassword(userInfo, sData);
                        }
                        else
                        {
                            _logger.Information("[超速操作] 修改密码 /" + userInfo.sUserIPaddr);
                        }
                    }
                    else
                    {
                        userInfo.sAccount = string.Empty;
                    }
                    break;
                case Grobal2.CM_UPDATEUSER:
                    if (HUtil32.GetTickCount() - userInfo.dwClientTick > 5000)
                    {
                        userInfo.dwClientTick = HUtil32.GetTickCount();
                        AccountUpdateUserInfo(userInfo, sData);
                    }
                    else
                    {
                        _logger.Information("[超速操作] 更新帐号 /" + userInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_GETBACKPASSWORD:
                    if (HUtil32.GetTickCount() - userInfo.dwClientTick > 5000)
                    {
                        userInfo.dwClientTick = HUtil32.GetTickCount();
                        AccountGetBackPassword(userInfo, sData);
                    }
                    else
                    {
                        _logger.Information("[超速操作] 找回密码 /" + userInfo.sUserIPaddr);
                    }
                    break;
            }
        }

        /// <summary>
        /// 账号注册
        /// </summary>
        private void AccountCreate(ref TUserInfo userInfo, string sData)
        {
            var bo21 = false;
            const string sAddNewuserFail = "[新建帐号失败] {0}/{1}";
            try
            {
                if (string.IsNullOrEmpty(sData) || sData.Length < 333)
                {
                    _logger.Information("[新建账号失败] 数据包为空或数据包长度异常");
                    return;
                }
                var accountStrSize = (byte)Math.Ceiling((decimal)(UserEntry.Size * 4) / 3);
                var ueBuff = EDCode.DecodeBuffer(sData[..accountStrSize]);
                var uaBuff = EDCode.DecodeBuffer(sData[accountStrSize..]);
                var accountBuff = new byte[ueBuff.Length + uaBuff.Length];
                Buffer.BlockCopy(ueBuff, 0, accountBuff, 0, ueBuff.Length);
                Buffer.BlockCopy(uaBuff, 0, accountBuff, ueBuff.Length, uaBuff.Length);
                var userFullEntry = Packets.ToPacket<UserFullEntry>(accountBuff);
                var nErrCode = -1;
                if (LsShare.CheckAccountName(userFullEntry.UserEntry.sAccount))
                {
                    bo21 = true;
                }
                if (bo21)
                {
                    var n10 = _accountStorage.Index(userFullEntry.UserEntry.sAccount);
                    if (n10 <= 0)
                    {
                        var dbRecord = new AccountRecord();
                        dbRecord.UserEntry = userFullEntry.UserEntry;
                        dbRecord.UserEntryAdd = userFullEntry.UserEntryAdd;
                        if (!string.IsNullOrEmpty(userFullEntry.UserEntry.sAccount))
                        {
                            if (_accountStorage.Add(ref dbRecord))
                            {
                                nErrCode = 1;
                            }
                        }
                    }
                    else
                    {
                        nErrCode = 0;
                    }
                }
                else
                {
                    _logger.Information(string.Format(sAddNewuserFail, userFullEntry.UserEntry.sAccount, userFullEntry.UserEntryAdd.sQuiz2));
                }
                ClientMesaagePacket defMsg;
                if (nErrCode == 1)
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWID_FAIL, nErrCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.LogDebug("[Exception] LoginsService.AccountCreate");
                _logger.Information(ex.StackTrace);
            }
            finally
            {
                userInfo.dwClientTick = HUtil32.GetTickCount();
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void AccountChangePassword(TUserInfo userInfo, string sData)
        {
            var sLoginId = string.Empty;
            var sOldPassword = string.Empty;
            ClientMesaagePacket defMsg;
            AccountRecord dbRecord = null;
            try
            {
                var sMsg = EDCode.DeCodeString(sData);
                sMsg = HUtil32.GetValidStr3(sMsg, ref sLoginId, new[] { "\09","\t" });
                var sNewPassword = HUtil32.GetValidStr3(sMsg, ref sOldPassword, new[] { "\09","\t" });
                var nCode = 0;
                if (sNewPassword.Length >= 3)
                {
                    var n10 = _accountStorage.Index(sLoginId);
                    if (n10 >= 0 && _accountStorage.Get(n10, ref dbRecord) >= 0)
                    {
                        if (dbRecord.nErrorCount < 5 || HUtil32.GetTickCount() - dbRecord.dwActionTick > 180000)
                        {
                            if (dbRecord.UserEntry.sPassword == sOldPassword)
                            {
                                dbRecord.nErrorCount = 0;
                                dbRecord.UserEntry.sPassword = sNewPassword;
                                nCode = 1;
                            }
                            else
                            {
                                dbRecord.nErrorCount++;
                                dbRecord.dwActionTick = HUtil32.GetTickCount();
                                nCode = -1;
                            }
                            _accountStorage.Update(n10, ref dbRecord);
                        }
                        else
                        {
                            nCode = -2;
                            if (HUtil32.GetTickCount() < dbRecord.dwActionTick)
                            {
                                dbRecord.dwActionTick = HUtil32.GetTickCount();
                                _accountStorage.Update(n10, ref dbRecord);
                            }
                        }
                    }
                }
                if (nCode == 1)
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.Information("[Exception] LoginService.ChangePassword");
                _logger.Information(ex.StackTrace);
            }
        }

        private void AccountCheckProtocol(TUserInfo userInfo, int nDate)
        {
            ClientMesaagePacket defMsg;
            if (nDate < LsShare.nVersionDate)
            {
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CERTIFICATION_FAIL, 0, 0, 0, 0);
            }
            else
            {
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CERTIFICATION_SUCCESS, 0, 0, 0, 0);
                userInfo.nVersionDate = nDate;
                userInfo.boCertificationOK = true;
            }
            SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg));
        }

        private void KickUser(Config config, ref TUserInfo userInfo)
        {
            const string sKickMsg = "Kick: {0}";
            for (var i = 0; i < _gateList.Count; i++)
            {
                var gateInfo = _gateList[i];
                for (var j = 0; j < gateInfo.UserList.Count; j++)
                {
                    var user = gateInfo.UserList[j];
                    if (user == userInfo)
                    {
                        _logger.LogDebug(string.Format(sKickMsg, userInfo.sUserIPaddr));
                        SendGateKickMsg(gateInfo.Socket, userInfo.sSockIndex);
                        userInfo = null;
                        gateInfo.UserList.RemoveAt(j);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 账号登陆
        /// </summary>
        private void AccountLogin(Config config, TUserInfo userInfo, string sData)
        {
            var sLoginId = string.Empty;
            UserEntry userEntry = null;
            var nIdCostIndex = -1;
            var nIpCostIndex = -1;
            AccountRecord dbRecord = null;
            try
            {
                var sPassword = HUtil32.GetValidStr3(EDCode.DeCodeString(sData), ref sLoginId, new[] { "/" });
                var nCode = 0;
                var boNeedUpdate = false;
                var n10 = _accountStorage.Index(sLoginId);
                if (n10 >= 0 && _accountStorage.Get(n10, ref dbRecord) >= 0)
                {
                    if (dbRecord.nErrorCount < 5 || HUtil32.GetTickCount() - dbRecord.dwActionTick > 60000)
                    {
                        if (dbRecord.UserEntry.sPassword == sPassword)
                        {
                            dbRecord.nErrorCount = 0;
                            if (dbRecord.UserEntry.sUserName == "" || dbRecord.UserEntryAdd.sQuiz2 == "")
                            {
                                userEntry = dbRecord.UserEntry;
                                boNeedUpdate = true;
                            }
                            dbRecord.Header.CreateDate = userInfo.dtDateTime;
                            nCode = 1;
                        }
                        else
                        {
                            dbRecord.nErrorCount++;
                            dbRecord.dwActionTick = HUtil32.GetTickCount();
                            nCode = -1;
                        }
                        _accountStorage.Update(n10, ref dbRecord);
                    }
                    else
                    {
                        nCode = -2;
                        dbRecord.dwActionTick = HUtil32.GetTickCount();
                        _accountStorage.Update(n10, ref dbRecord);
                    }
                }
                if (nCode == 1 && IsLogin(config, sLoginId))
                {
                    SessionKick(config, sLoginId);
                    nCode = -3;
                }
                ClientMesaagePacket defMsg;
                if (boNeedUpdate)
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEEDUPDATE_ACCOUNT, 0, 0, 0, 0);
                    SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeBuffer(userEntry));
                }
                if (nCode == 1)
                {
                    userInfo.sAccount = sLoginId;
                    userInfo.nSessionID = LsShare.GetSessionId();
                    userInfo.boSelServer = false;
                    if (config.AccountCostList.ContainsKey(userInfo.sAccount))
                    {
                        nIdCostIndex = config.AccountCostList[userInfo.sAccount];
                    }
                    if (config.IPaddrCostList.ContainsKey(userInfo.sUserIPaddr))
                    {
                        nIpCostIndex = config.IPaddrCostList[userInfo.sUserIPaddr];
                    }
                    var nIdCost = 0;
                    var nIpCost = 0;
                    if (nIdCostIndex >= 0)
                    {
                        nIdCost = nIdCostIndex;//Config.AccountCostList[nIDCostIndex];
                    }
                    if (nIpCostIndex >= 0)
                    {
                        nIpCost = nIpCostIndex;//Config.IPaddrCostList[nIPCostIndex];
                    }
                    if (nIdCost >= 0 || nIpCost >= 0)
                    {
                        userInfo.boPayCost = true;
                    }
                    else
                    {
                        userInfo.boPayCost = false;
                    }
                    userInfo.nIDDay = HUtil32.LoWord(nIdCost);
                    userInfo.nIDHour = HUtil32.HiWord(nIdCost);
                    userInfo.nIPDay = HUtil32.LoWord(nIpCost);
                    userInfo.nIPHour = HUtil32.HiWord(nIpCost);
                    if (!userInfo.boPayCost)
                    {
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSOK_SELECTSERVER, 0, 0, 0, config.ServerNameList.Count);
                    }
                    else
                    {
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSOK_SELECTSERVER, nIdCost, HUtil32.LoWord(nIpCost), HUtil32.HiWord(nIpCost), config.ServerNameList.Count);
                    }
                    var sServerName = GetServerListInfo();
                    SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sServerName));
                    SessionAdd(config, userInfo.sAccount, userInfo.sUserIPaddr, userInfo.nSessionID, userInfo.boPayCost, false);
                }
                else
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSWD_FAIL, nCode, 0, 0, 0);
                    SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg));
                }
            }
            catch (Exception ex)
            {
                _logger.Information("[Exception] LoginService.LoginUser");
                _logger.Information(ex.StackTrace);
            }
        }

        /// <summary>
        /// 获取角色网关信息
        /// </summary>
        private void GetSelGateInfo(Config config, string sServerName, string sIPaddr, ref string sSelGateIp, ref int nSelGatePort)
        {
            int nGateIdx;
            int nGateCount;
            int nSelIdx;
            bool boSelected;
            try
            {
                sSelGateIp = "";
                nSelGatePort = 0;
                for (var i = 0; i < config.nRouteCount; i++)
                {
                    if (config.boDynamicIPMode || (config.GateRoute[i].sServerName == sServerName && config.GateRoute[i].sPublicAddr == sIPaddr))
                    {
                        nGateCount = 0;
                        nGateIdx = 0;
                        while (true)
                        {
                            if (config.GateRoute[i].Gate[nGateIdx].sIPaddr != "" && config.GateRoute[i].Gate[nGateIdx].boEnable)
                            {
                                nGateCount++;
                            }
                            nGateIdx++;
                            if (nGateIdx >= 10)
                            {
                                break;
                            }
                        }
                        if (nGateCount <= 0)
                        {
                            break;
                        }
                        nSelIdx = config.GateRoute[i].nSelIdx;
                        boSelected = false;
                        for (nGateIdx = nSelIdx + 1; nGateIdx <= 9; nGateIdx++)
                        {
                            if (config.GateRoute[i].Gate[nGateIdx].sIPaddr != "" && config.GateRoute[i].Gate[nGateIdx].boEnable)
                            {
                                config.GateRoute[i].nSelIdx = nGateIdx;
                                boSelected = true;
                                break;
                            }
                        }
                        if (!boSelected)
                        {
                            for (nGateIdx = 0; nGateIdx < nSelIdx; nGateIdx++)
                            {
                                if (config.GateRoute[i].Gate[nGateIdx].sIPaddr != "" && config.GateRoute[i].Gate[nGateIdx].boEnable)
                                {
                                    config.GateRoute[i].nSelIdx = nGateIdx;
                                    break;
                                }
                            }
                        }
                        nSelIdx = config.GateRoute[i].nSelIdx;
                        sSelGateIp = config.GateRoute[i].Gate[nSelIdx].sIPaddr;
                        nSelGatePort = config.GateRoute[i].Gate[nSelIdx].nPort;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Information("[Exception] LoginService.GetSelGateInfo");
                _logger.Information(ex.StackTrace);
            }
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <returns></returns>
        private string GetServerListInfo()
        {
            var result = string.Empty;
            var sServerInfo = string.Empty;
            var config = _configManager.Config;
            try
            {
                for (var i = 0; i < config.ServerNameList.Count; i++)
                {
                    var sServerName = config.ServerNameList[i];
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        sServerInfo = sServerInfo + sServerName + "/" + _masSocService.GetServerStatus(sServerName) + "/";
                    }
                }
                result = sServerInfo;
            }
            catch
            {
                _logger.Information("[Exception] LoginService.GetServerListInfo");
            }
            return result;
        }

        /// <summary>
        /// 选择服务器
        /// </summary>
        private void AccountSelectServer(Config config, TUserInfo userInfo, string sData)
        {
            ClientMesaagePacket defMsg;
            bool boPayCost;
            var sSelGateIp = string.Empty;
            var nSelGatePort = 0;
            const string sSelServerMsg = "Server: {0}/{1}-{2}:{3}";
            var sServerName = EDCode.DeCodeString(sData);
            if (!string.IsNullOrEmpty(userInfo.sAccount) && !string.IsNullOrEmpty(sServerName) && IsLogin(config, userInfo.nSessionID))
            {
                GetSelGateInfo(config, sServerName, config.sGateIPaddr, ref sSelGateIp, ref nSelGatePort);
                if (sSelGateIp != "" && nSelGatePort > 0)
                {
                    if (config.boDynamicIPMode)
                    {
                        sSelGateIp = userInfo.sGateIPaddr;
                    }
                    _logger.LogDebug(string.Format(sSelServerMsg, sServerName, config.sGateIPaddr, sSelGateIp, nSelGatePort));
                    userInfo.boSelServer = true;
                    boPayCost = false;
                    var nPayMode = 5;
                    if (userInfo.nIDHour > 0)
                    {
                        nPayMode = 2;
                    }
                    if (userInfo.nIPHour > 0)
                    {
                        nPayMode = 4;
                    }
                    if (userInfo.nIPDay > 0)
                    {
                        nPayMode = 3;
                    }
                    if (userInfo.nIDDay > 0)
                    {
                        nPayMode = 1;
                    }
                    if (_masSocService.IsNotUserFull(sServerName))
                    {
                        SessionUpdate(config, userInfo.nSessionID, sServerName, boPayCost);
                        _masSocService.SendServerMsg(Grobal2.SS_OPENSESSION, sServerName, userInfo.sAccount + "/" + userInfo.nSessionID + "/" + (userInfo.boPayCost ? 1 : 0) + "/" + nPayMode + "/" + userInfo.sUserIPaddr);
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELECTSERVER_OK, userInfo.nSessionID, 0, 0, 0);
                        SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sSelGateIp + "/" + nSelGatePort + "/" + userInfo.nSessionID));
                    }
                    else
                    {
                        userInfo.boSelServer = false;
                        SessionDel(config, userInfo.nSessionID);
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_STARTFAIL, 0, 0, 0, 0);
                        SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg));
                    }
                }
            }
        }

        /// <summary>
        /// 更新账号信息
        /// </summary>
        private void AccountUpdateUserInfo(TUserInfo userInfo, string sData)
        {
            AccountRecord dbRecord = null;
            UserFullEntry userFullEntry = null;
            ClientMesaagePacket defMsg;
            try
            {
                if (string.IsNullOrEmpty(sData))
                {
                    _logger.Information("[新建账号失败,数据包为空].");
                    return;
                }
                var deBuffer = EDCode.DecodeBuffer(sData);
                userFullEntry = Packets.ToPacket<UserFullEntry>(deBuffer);
                var nCode = -1;
                if (userInfo.sAccount == userFullEntry.UserEntry.sAccount && LsShare.CheckAccountName(userFullEntry.UserEntry.sAccount))
                {
                    var n10 = _accountStorage.Index(userFullEntry.UserEntry.sAccount);
                    if (n10 >= 0)
                    {
                        if (_accountStorage.Get(n10, ref dbRecord) >= 0)
                        {
                            dbRecord.UserEntry = userFullEntry.UserEntry;
                            dbRecord.UserEntryAdd = userFullEntry.UserEntryAdd;
                            _accountStorage.Update(n10, ref dbRecord);
                            nCode = 1;
                        }
                    }
                    else
                    {
                        nCode = 0;
                    }
                }
                if (nCode == 1)
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.Information("[Exception] LoginService.UpdateUserInfo");
                _logger.Information(ex.StackTrace);
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        private void AccountGetBackPassword(TUserInfo userInfo, string sData)
        {
            var sAccount = string.Empty;
            var sQuest1 = string.Empty;
            var sAnswer1 = string.Empty;
            var sQuest2 = string.Empty;
            var sAnswer2 = string.Empty;
            var sPassword = string.Empty;
            var sBirthDay = string.Empty;
            ClientMesaagePacket defMsg;
            AccountRecord dbRecord = null;
            var sMsg = EDCode.DeCodeString(sData);
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest1, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer1, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest2, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer2, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sBirthDay, new[] { "\09" });
            var nCode = 0;
            if (!string.IsNullOrEmpty(sAccount))
            {
                var nIndex = _accountStorage.Index(sAccount);
                if (nIndex >= 0 && _accountStorage.Get(nIndex, ref dbRecord) >= 0)
                {
                    if (dbRecord.nErrorCount < 5 || HUtil32.GetTickCount() - dbRecord.dwActionTick > 180000)
                    {
                        nCode = -1;
                        if (dbRecord.UserEntry.sQuiz == sQuest1)
                        {
                            nCode = -3;
                            if (dbRecord.UserEntry.sAnswer == sAnswer1)
                            {
                                if (dbRecord.UserEntryAdd.sBirthDay == sBirthDay)
                                {
                                    nCode = 1;
                                }
                            }
                        }
                        if (nCode != 1)
                        {
                            if (dbRecord.UserEntryAdd.sQuiz2 == sQuest2)
                            {
                                nCode = -3;
                                if (dbRecord.UserEntryAdd.sAnswer2 == sAnswer2)
                                {
                                    if (dbRecord.UserEntryAdd.sBirthDay == sBirthDay)
                                    {
                                        nCode = 1;
                                    }
                                }
                            }
                        }
                        if (nCode == 1)
                        {
                            sPassword = dbRecord.UserEntry.sPassword;
                        }
                        else
                        {
                            dbRecord.nErrorCount++;
                            dbRecord.dwActionTick = HUtil32.GetTickCount();
                            _accountStorage.Update(nIndex, ref dbRecord);
                        }
                    }
                    else
                    {
                        nCode = -2;
                        if (HUtil32.GetTickCount() < dbRecord.dwActionTick)
                        {
                            dbRecord.dwActionTick = HUtil32.GetTickCount();
                            _accountStorage.Update(nIndex, ref dbRecord);
                        }
                    }
                }
            }
            if (nCode == 1)
            {
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GETBACKPASSWD_SUCCESS, 0, 0, 0, 0);
                SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sPassword));
            }
            else
            {
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GETBACKPASSWD_FAIL, nCode, 0, 0, 0);
                SendGateMsg(userInfo.Socket, userInfo.sSockIndex, EDCode.EncodeMessage(defMsg));
            }
        }

        private void SendGateMsg(Socket socket, string sSockIndex, string sMsg)
        {
            if (socket.Connected)
            {
                //var sSendMsg = "%" + sSockIndex + "/#" + sMsg + "!$";
                //Socket.SendText(sSendMsg);
                var packet = new LoginSvrPacket();
                packet.ConnectionId = sSockIndex;
                packet.ClientPacket = HUtil32.GetBytes("#" + sMsg + "!$");
                socket.SendBuffer(packet.GetBuffer());
            }
        }

        private bool IsLogin(Config config, int nSessionId)
        {
            var result = false;
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                if (config.SessionList[i].nSessionID == nSessionId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsLogin(Config config, string sLoginId)
        {
            var result = false;
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                if (config.SessionList[i].sAccount == sLoginId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 剔除会话
        /// </summary>
        private void SessionKick(Config config, string sLoginId)
        {
            TConnInfo connInfo;
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                connInfo = config.SessionList[i];
                if (connInfo.sAccount == sLoginId && !connInfo.boKicked)
                {
                    _masSocService.SendServerMsg(Grobal2.SS_CLOSESESSION, connInfo.sServerName, connInfo.sAccount + "/" + connInfo.nSessionID);
                    connInfo.dwKickTick = HUtil32.GetTickCount();
                    connInfo.boKicked = true;
                }
            }
        }

        private void SessionAdd(Config config, string sAccount, string sIPaddr, int nSessionId, bool boPayCost, bool bo11)
        {
            var connInfo = new TConnInfo();
            connInfo.sAccount = sAccount;
            connInfo.sIPaddr = sIPaddr;
            connInfo.nSessionID = nSessionId;
            connInfo.boPayCost = boPayCost;
            connInfo.bo11 = bo11;
            connInfo.dwKickTick = HUtil32.GetTickCount();
            connInfo.dwStartTick = HUtil32.GetTickCount();
            connInfo.boKicked = false;
            config.SessionList.Add(connInfo);
        }

        private void SendGateKickMsg(Socket socket, string sSockIndex)
        {
            var sSendMsg = $"%+-{sSockIndex}$";
            socket.SendText(sSendMsg);
        }

        private void SessionUpdate(Config config, int nSessionId, string sServerName, bool boPayCost)
        {
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                var connInfo = config.SessionList[i];
                if (connInfo.nSessionID == nSessionId)
                {
                    connInfo.sServerName = sServerName;
                    connInfo.bo11 = boPayCost;
                    break;
                }
            }
        }

        /// <summary>
        /// 清理没有付费的账号
        /// </summary>
        public void SessionClearNoPayMent()
        {
            var config = _configManager.Config;
            for (var i = config.SessionList.Count - 1; i >= 0; i--)
            {
                var connInfo = config.SessionList[i];
                if (!connInfo.boKicked && !config.TestServer && !connInfo.bo11)
                {
                    if (HUtil32.GetTickCount() - connInfo.dwStartTick > 60 * 60 * 1000)
                    {
                        connInfo.dwStartTick = HUtil32.GetTickCount();
                        if (!IsPayMent(config, connInfo.sIPaddr, connInfo.sAccount))
                        {
                            _masSocService.SendServerMsg(Grobal2.SS_KICKUSER, connInfo.sServerName, connInfo.sAccount + "/" + connInfo.nSessionID);
                            config.SessionList[i] = null;
                            config.SessionList.RemoveAt(i);
                        }
                    }
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

        public void LoadConfig()
        {
            _configManager.LoadConfig();
            _configManager.LoadAddrTable();
            _accountStorage.Initialization();
        }
    }
}