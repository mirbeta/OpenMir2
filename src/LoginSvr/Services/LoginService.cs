using LoginSvr.Packet;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Sockets;

namespace LoginSvr
{
    /// <summary>
    /// 账号服务
    /// 处理账号注册 登录 找回密码等
    /// </summary>
    public class LoginService
    {
        private static readonly LoginService instance = new LoginService();

        public static LoginService Instance
        {
            get { return instance; }
        }
        private readonly ISocketServer _serverSocket;
        private LogQueue _logQueue => LogQueue.Instance;
        private AccountDB _accountDB = AccountDB.Instance;
        private MasSocService _masSock => MasSocService.Instance;
        private ConfigManager _configManager => ConfigManager.Instance;
        private readonly Channel<LoginPacket> _receiveQueue = null;
        private readonly Channel<ReceiveUserData> _processUserQueue = null;
        private readonly IList<TGateInfo> _gateList;

        public LoginService()
        {
            _receiveQueue = Channel.CreateUnbounded<LoginPacket>();
            _processUserQueue = Channel.CreateUnbounded<ReceiveUserData>();
            _gateList = new List<TGateInfo>();
            _serverSocket = new ISocketServer(short.MaxValue, 2048);
            _serverSocket.OnClientConnect += GSocketClientConnect;
            _serverSocket.OnClientDisconnect += GSocketClientDisconnect;
            _serverSocket.OnClientRead += GSocketClientRead;
            _serverSocket.OnClientError += GSocketClientError;
            _serverSocket.Init();
        }

        public void Start()
        {
            _serverSocket.Start(_configManager.Config.sGateAddr, _configManager.Config.nGatePort);
            _logQueue.Enqueue($"账号登陆服务[{_configManager.Config.sGateAddr}:{_configManager.Config.nGatePort}]已启动.");
        }

        private void GSocketClientConnect(object sender, AsyncUserToken e)
        {
            var GateInfo = new TGateInfo();
            GateInfo.Socket = e.Socket;
            GateInfo.sIPaddr = LSShare.GetGatePublicAddr(_configManager.Config, e.RemoteIPaddr);
            GateInfo.UserList = new List<TUserInfo>();
            GateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
            _gateList.Add(GateInfo);
            _logQueue.Enqueue($"登录网关[{e.RemoteIPaddr}:{e.RemotePort}]已链接.");
        }

        private void GSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            TGateInfo GateInfo;
            TUserInfo UserInfo;
            for (var i = 0; i < _gateList.Count; i++)
            {
                GateInfo = _gateList[i];
                if (GateInfo.Socket == e.Socket)
                {
                    for (var j = 0; j < GateInfo.UserList.Count; j++)
                    {
                        UserInfo = GateInfo.UserList[j];
                        if (_configManager.Config.ShowDetailMsg)
                        {
                            _logQueue.EnqueueDebugging("Close: " + UserInfo.sUserIPaddr);
                        }
                        GateInfo.UserList[j] = null;
                    }
                    GateInfo.UserList = null;
                    _gateList.Remove(GateInfo);
                    break;
                }
            }
            _logQueue.Enqueue($"[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
        }

        private void GSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void GSocketClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _gateList.Count; i++)
            {
                if (_gateList[i].Socket == e.Socket)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    _receiveQueue.Writer.TryWrite(new LoginPacket(data));
                    break;
                }
            }
        }

        /// <summary>
        /// 是否付费账号
        /// </summary>
        /// <returns></returns>
        private bool IsPayMent(Config Config, string sIPaddr, string sAccount)
        {
            return Config.AccountCostList.ContainsKey(sAccount) || Config.IPaddrCostList.ContainsKey(sIPaddr);
        }

        /// <summary>
        /// 启动数据消费者
        /// </summary>
        /// <returns></returns>
        public async Task StartConsumer()
        {
            await Task.Run(() =>
            {
                Task.Factory.StartNew(async () => { await ProcessReviceMessage(); });
                Task.Factory.StartNew(async () => { await ProcessUserMessage(); });
            });
        }

        /// <summary>
        /// 处理网关消息
        /// </summary>
        /// <returns></returns>
        private async Task ProcessReviceMessage()
        {
            while (await _receiveQueue.Reader.WaitToReadAsync())
            {
                if (_receiveQueue.Reader.TryRead(out var message))
                {
                    ProcessGateData(message);
                }
            }
        }

        /// <summary>
        /// 处理封包消息
        /// </summary>
        /// <returns></returns>
        private async Task ProcessUserMessage()
        {
            while (await _processUserQueue.Reader.WaitToReadAsync())
            {
                if (_processUserQueue.Reader.TryRead(out var message))
                {
                    DecodeUserData(message.UserInfo, message.Msg);
                }
            }
        }

        private void DecodeUserData(TUserInfo UserInfo, string userData)
        {
            string sMsg = string.Empty;
            try
            {
                if (HUtil32.TagCount(userData, '!') <= 0)
                {
                    return;
                }
                HUtil32.ArrestStringEx(userData, "#", "!", ref sMsg);
                if (!string.IsNullOrEmpty(sMsg))
                {
                    if (sMsg.Length >= Grobal2.DEFBLOCKSIZE + 1)
                    {
                        sMsg = sMsg.Substring(1, sMsg.Length - 1);
                        ProcessUserMsg(UserInfo, sMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue("[Exception] LoginService.DecodeUserData");
                _logQueue.Enqueue(ex.StackTrace);
            }
        }

        private void ProcessGateData(LoginPacket packet)
        {
            int I = 0;
            while (true)
            {
                if (_gateList.Count <= I)
                {
                    break;
                }
                TGateInfo GateInfo = _gateList[I];
                if (packet.Body != null && GateInfo.UserList != null)
                {
                    if (packet.EndChar != '$' && packet.StartChar != '%')
                    {
                        Console.WriteLine("丢弃错误的封包数据");
                        return;
                    }
                    string sMsg = HUtil32.GetString(packet.Body, 0, packet.Body.Length);
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        switch (packet.PacketType)
                        {
                            case '-':
                                SendKeepAlivePacket(GateInfo.Socket);
                                GateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
                                break;
                            case 'A':
                            case 'D':
                                ReceiveSendUser(packet.SocketId, GateInfo, sMsg);
                                break;
                            case 'O':
                            case 'L':
                                ReceiveOpenUser(packet.SocketId, sMsg, GateInfo);
                                break;
                            case 'X':
                                ReceiveCloseUser(packet.SocketId, GateInfo);
                                break;
                        }
                    }
                    _configManager.Config.sGateIPaddr = GateInfo.sIPaddr;
                }
                I++;
            }
        }

        private void SendKeepAlivePacket(Socket Socket)
        {
            if (Socket.Connected)
            {
                Socket.SendText("%++$");
            }
        }

        private void ReceiveCloseUser(int sSockIndex, TGateInfo GateInfo)
        {
            const string sCloseMsg = "Close: {0}";
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                var UserInfo = GateInfo.UserList[i];
                if (UserInfo.sSockIndex == sSockIndex)
                {
                    if (_configManager.Config.ShowDetailMsg)
                    {
                        _logQueue.EnqueueDebugging(string.Format(sCloseMsg, UserInfo.sUserIPaddr));
                    }
                    if (!UserInfo.boSelServer)
                    {
                        SessionDel(_configManager.Config, UserInfo.nSessionID);
                    }
                    GateInfo.UserList[i] = null;
                    GateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ReceiveOpenUser(int sSockIndex, string sIPaddr, TGateInfo GateInfo)
        {
            TUserInfo UserInfo;
            string sUserIPaddr = string.Empty;
            const string sOpenMsg = "Open: {0}/{1}";
            var sGateIPaddr = HUtil32.GetValidStr3(sIPaddr, ref sUserIPaddr, new[] { "/" });
            try
            {
                for (var i = 0; i < GateInfo.UserList.Count; i++)
                {
                    UserInfo = GateInfo.UserList[i];
                    if (UserInfo.sSockIndex == sSockIndex)
                    {
                        UserInfo.sUserIPaddr = sUserIPaddr;
                        UserInfo.sGateIPaddr = sGateIPaddr;
                        UserInfo.sAccount = string.Empty;
                        UserInfo.nSessionID = 0;
                        UserInfo.dwClientTick = HUtil32.GetTickCount();
                        return;
                    }
                }
                UserInfo = new TUserInfo();
                UserInfo.sAccount = string.Empty;
                UserInfo.sUserIPaddr = sUserIPaddr;
                UserInfo.sGateIPaddr = sGateIPaddr;
                UserInfo.sSockIndex = sSockIndex;
                UserInfo.nVersionDate = 0;
                UserInfo.boCertificationOK = false;
                UserInfo.nSessionID = 0;
                UserInfo.Socket = GateInfo.Socket;
                UserInfo.dwClientTick = HUtil32.GetTickCount();
                UserInfo.Gate = GateInfo;
                GateInfo.UserList.Add(UserInfo);
                if (_configManager.Config.ShowDetailMsg)
                {
                    _logQueue.EnqueueDebugging(string.Format(sOpenMsg, sUserIPaddr, sGateIPaddr));
                }
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue("[Exception] LoginService.ReceiveOpenUser " + ex.Source);
            }
        }

        private void ReceiveSendUser(int sSockIndex, TGateInfo GateInfo, string sData)
        {
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                var UserInfo = GateInfo.UserList[i];
                if (UserInfo.sSockIndex == sSockIndex)
                {
                    _processUserQueue.Writer.TryWrite(new ReceiveUserData()
                    {
                        UserInfo = UserInfo,
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
            var Config = _configManager.Config;
            for (var i = Config.SessionList.Count - 1; i >= 0; i--)
            {
                TConnInfo ConnInfo = Config.SessionList[i];
                if (ConnInfo.boKicked && HUtil32.GetTickCount() - ConnInfo.dwKickTick > 5 * 1000)
                {
                    Config.SessionList[i] = null;
                    Config.SessionList.RemoveAt(i);
                }
            }
        }

        private void SessionDel(Config Config, int nSessionID)
        {
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                if (Config.SessionList[i].nSessionID == nSessionID)
                {
                    Config.SessionList[i] = null;
                    Config.SessionList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ProcessUserMsg(TUserInfo UserInfo, string sMsg)
        {
            string sDefMsg = sMsg.Substring(0, Grobal2.DEFBLOCKSIZE);
            string sData = sMsg.Substring(Grobal2.DEFBLOCKSIZE, sMsg.Length - Grobal2.DEFBLOCKSIZE);
            ClientPacket DefMsg = EDcode.DecodePacket(sDefMsg);
            switch (DefMsg.Ident)
            {
                case Grobal2.CM_SELECTSERVER:
                    if (!UserInfo.boSelServer)
                    {
                        AccountSelectServer(_configManager.Config, UserInfo, sData);
                    }
                    break;
                case Grobal2.CM_PROTOCOL:
                    AccountCheckProtocol(UserInfo, DefMsg.Recog);
                    break;
                case Grobal2.CM_IDPASSWORD:
                    if (string.IsNullOrEmpty(UserInfo.sAccount))
                    {
                        AccountLogin(_configManager.Config, UserInfo, sData);
                    }
                    else
                    {
                        KickUser(_configManager.Config, ref UserInfo);
                    }
                    break;
                case Grobal2.CM_ADDNEWUSER:
                    if (_configManager.Config.boEnableMakingID)
                    {
                        if (HUtil32.GetTickCount() - UserInfo.dwClientTick > 5000)
                        {
                            AccountCreate(ref UserInfo, sData);
                        }
                        else
                        {
                            _logQueue.Enqueue("[超速操作] 创建帐号 /" + UserInfo.sUserIPaddr);
                        }
                    }
                    break;
                case Grobal2.CM_CHANGEPASSWORD:
                    if (string.IsNullOrEmpty(UserInfo.sAccount))
                    {
                        if (HUtil32.GetTickCount() - UserInfo.dwClientTick > 5000)
                        {
                            UserInfo.dwClientTick = HUtil32.GetTickCount();
                            AccountChangePassword(UserInfo, sData);
                        }
                        else
                        {
                            _logQueue.Enqueue("[超速操作] 修改密码 /" + UserInfo.sUserIPaddr);
                        }
                    }
                    else
                    {
                        UserInfo.sAccount = string.Empty;
                    }
                    break;
                case Grobal2.CM_UPDATEUSER:
                    if (HUtil32.GetTickCount() - UserInfo.dwClientTick > 5000)
                    {
                        UserInfo.dwClientTick = HUtil32.GetTickCount();
                        AccountUpdateUserInfo(UserInfo, sData);
                    }
                    else
                    {
                        _logQueue.Enqueue("[超速操作] 更新帐号 /" + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_GETBACKPASSWORD:
                    if (HUtil32.GetTickCount() - UserInfo.dwClientTick > 5000)
                    {
                        UserInfo.dwClientTick = HUtil32.GetTickCount();
                        AccountGetBackPassword(UserInfo, sData);
                    }
                    else
                    {
                        _logQueue.Enqueue("[超速操作] 找回密码 /" + UserInfo.sUserIPaddr);
                    }
                    break;
            }
        }

        private void AccountCreate(ref TUserInfo UserInfo, string sData)
        {
            bool bo21 = false;
            const string sAddNewuserFail = "[新建帐号失败] {0}/{1}";
            UserFullEntry userFullEntry = null;
            try
            {
                if (string.IsNullOrEmpty(sData))
                {
                    _logQueue.Enqueue("[新建账号失败] 数据包为空.");
                    return;
                }
                var deBuffer = EDcode.DecodeBuffer(sData);
                userFullEntry = Packets.ToPacket<UserFullEntry>(deBuffer);
                int nErrCode = -1;
                if (LSShare.CheckAccountName(userFullEntry.UserEntry.sAccount))
                {
                    bo21 = true;
                }
                if (bo21)
                {
                    int n10 = _accountDB.Index(userFullEntry.UserEntry.sAccount);
                    if (n10 <= 0)
                    {
                        TAccountDBRecord DBRecord = new TAccountDBRecord();
                        DBRecord.UserEntry = userFullEntry.UserEntry;
                        DBRecord.UserEntryAdd = userFullEntry.UserEntryAdd;
                        if (!string.IsNullOrEmpty(userFullEntry.UserEntry.sAccount))
                        {
                            if (_accountDB.Add(ref DBRecord))
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
                    _logQueue.Enqueue(string.Format(sAddNewuserFail, userFullEntry.UserEntry.sAccount, userFullEntry.UserEntryAdd.sQuiz2));
                }
                ClientPacket DefMsg;
                if (nErrCode == 1)
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWID_FAIL, nErrCode, 0, 0, 0);
                }
                SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
            }
            catch (Exception ex)
            {
                _logQueue.EnqueueDebugging("[Exception] LoginsService.AccountCreate");
                _logQueue.Enqueue(ex.StackTrace);
            }
            finally
            {
                UserInfo.dwClientTick = HUtil32.GetTickCount();
            }
        }

        private void AccountChangePassword(TUserInfo UserInfo, string sData)
        {
            string sLoginID = string.Empty;
            string sOldPassword = string.Empty;
            ClientPacket DefMsg;
            TAccountDBRecord DBRecord = null;
            try
            {
                string sMsg = EDcode.DeCodeString(sData);
                sMsg = HUtil32.GetValidStr3(sMsg, ref sLoginID, new[] { "\09" });
                string sNewPassword = HUtil32.GetValidStr3(sMsg, ref sOldPassword, new[] { "\09" });
                int nCode = 0;
                if (sNewPassword.Length >= 3)
                {
                    int n10 = _accountDB.Index(sLoginID);
                    if (n10 >= 0 && _accountDB.Get(n10, ref DBRecord) >= 0)
                    {
                        if (DBRecord.nErrorCount < 5 || HUtil32.GetTickCount() - DBRecord.dwActionTick > 180000)
                        {
                            if (DBRecord.UserEntry.sPassword == sOldPassword)
                            {
                                DBRecord.nErrorCount = 0;
                                DBRecord.UserEntry.sPassword = sNewPassword;
                                nCode = 1;
                            }
                            else
                            {
                                DBRecord.nErrorCount++;
                                DBRecord.dwActionTick = HUtil32.GetTickCount();
                                nCode = -1;
                            }
                            _accountDB.Update(n10, ref DBRecord);
                        }
                        else
                        {
                            nCode = -2;
                            if (HUtil32.GetTickCount() < DBRecord.dwActionTick)
                            {
                                DBRecord.dwActionTick = HUtil32.GetTickCount();
                                _accountDB.Update(n10, ref DBRecord);
                            }
                        }
                    }
                }
                if (nCode == 1)
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue("[Exception] LoginService.ChangePassword");
                _logQueue.Enqueue(ex.StackTrace);
            }
        }

        private void AccountCheckProtocol(TUserInfo UserInfo, int nDate)
        {
            ClientPacket DefMsg;
            if (nDate < LSShare.nVersionDate)
            {
                DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CERTIFICATION_FAIL, 0, 0, 0, 0);
            }
            else
            {
                DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CERTIFICATION_SUCCESS, 0, 0, 0, 0);
                UserInfo.nVersionDate = nDate;
                UserInfo.boCertificationOK = true;
            }
            SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
        }

        private void KickUser(Config Config, ref TUserInfo UserInfo)
        {
            const string sKickMsg = "Kick: {0}";
            for (var i = 0; i < _gateList.Count; i++)
            {
                var GateInfo = _gateList[i];
                for (var j = 0; j < GateInfo.UserList.Count; j++)
                {
                    var User = GateInfo.UserList[j];
                    if (User == UserInfo)
                    {
                        if (Config.ShowDetailMsg)
                        {
                            _logQueue.EnqueueDebugging(string.Format(sKickMsg, UserInfo.sUserIPaddr));
                        }
                        SendGateKickMsg(GateInfo.Socket, UserInfo.sSockIndex);
                        UserInfo = null;
                        GateInfo.UserList.RemoveAt(j);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 账号登陆
        /// </summary>
        private void AccountLogin(Config Config, TUserInfo UserInfo, string sData)
        {
            string sLoginID = string.Empty;
            TUserEntry UserEntry = null;
            int nIDCostIndex = -1;
            int nIPCostIndex = -1;
            TAccountDBRecord DBRecord = null;
            try
            {
                var sPassword = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sLoginID, new[] { "/" });
                var nCode = 0;
                var boNeedUpdate = false;
                var n10 = _accountDB.Index(sLoginID);
                if (n10 >= 0 && _accountDB.Get(n10, ref DBRecord) >= 0)
                {
                    if (DBRecord.nErrorCount < 5 || HUtil32.GetTickCount() - DBRecord.dwActionTick > 60000)
                    {
                        if (DBRecord.UserEntry.sPassword == sPassword)
                        {
                            DBRecord.nErrorCount = 0;
                            if (DBRecord.UserEntry.sUserName == "" || DBRecord.UserEntryAdd.sQuiz2 == "")
                            {
                                UserEntry = DBRecord.UserEntry;
                                boNeedUpdate = true;
                            }
                            DBRecord.Header.CreateDate = UserInfo.dtDateTime;
                            nCode = 1;
                        }
                        else
                        {
                            DBRecord.nErrorCount++;
                            DBRecord.dwActionTick = HUtil32.GetTickCount();
                            nCode = -1;
                        }
                        _accountDB.Update(n10, ref DBRecord);
                    }
                    else
                    {
                        nCode = -2;
                        DBRecord.dwActionTick = HUtil32.GetTickCount();
                        _accountDB.Update(n10, ref DBRecord);
                    }
                }
                if (nCode == 1 && IsLogin(Config, sLoginID))
                {
                    SessionKick(Config, sLoginID);
                    nCode = -3;
                }
                ClientPacket DefMsg;
                if (boNeedUpdate)
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEEDUPDATE_ACCOUNT, 0, 0, 0, 0);
                    SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg) + EDcode.EncodeBuffer(UserEntry));
                }
                if (nCode == 1)
                {
                    UserInfo.sAccount = sLoginID;
                    UserInfo.nSessionID = LSShare.GetSessionID();
                    UserInfo.boSelServer = false;
                    if (Config.AccountCostList.ContainsKey(UserInfo.sAccount))
                    {
                        nIDCostIndex = Config.AccountCostList[UserInfo.sAccount];
                    }
                    if (Config.IPaddrCostList.ContainsKey(UserInfo.sUserIPaddr))
                    {
                        nIPCostIndex = Config.IPaddrCostList[UserInfo.sUserIPaddr];
                    }
                    var nIDCost = 0;
                    var nIPCost = 0;
                    if (nIDCostIndex >= 0)
                    {
                        nIDCost = nIDCostIndex;//Config.AccountCostList[nIDCostIndex];
                    }
                    if (nIPCostIndex >= 0)
                    {
                        nIPCost = nIPCostIndex;//Config.IPaddrCostList[nIPCostIndex];
                    }
                    if (nIDCost >= 0 || nIPCost >= 0)
                    {
                        UserInfo.boPayCost = true;
                    }
                    else
                    {
                        UserInfo.boPayCost = false;
                    }
                    UserInfo.nIDDay = HUtil32.LoWord(nIDCost);
                    UserInfo.nIDHour = HUtil32.HiWord(nIDCost);
                    UserInfo.nIPDay = HUtil32.LoWord(nIPCost);
                    UserInfo.nIPHour = HUtil32.HiWord(nIPCost);
                    if (!UserInfo.boPayCost)
                    {
                        DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSOK_SELECTSERVER, 0, 0, 0, Config.ServerNameList.Count);
                    }
                    else
                    {
                        DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSOK_SELECTSERVER, nIDCost, HUtil32.LoWord(nIPCost), HUtil32.HiWord(nIPCost), Config.ServerNameList.Count);
                    }
                    var sServerName = GetServerListInfo();
                    SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sServerName));
                    SessionAdd(Config, UserInfo.sAccount, UserInfo.sUserIPaddr, UserInfo.nSessionID, UserInfo.boPayCost, false);
                }
                else
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSWD_FAIL, nCode, 0, 0, 0);
                    SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
                }
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue("[Exception] LoginService.LoginUser");
                _logQueue.Enqueue(ex.StackTrace);
            }
        }

        /// <summary>
        /// 获取角色网关信息
        /// </summary>
        private void GetSelGateInfo(Config Config, string sServerName, string sIPaddr, ref string sSelGateIP, ref int nSelGatePort)
        {
            int nGateIdx;
            int nGateCount;
            int nSelIdx;
            bool boSelected;
            try
            {
                sSelGateIP = "";
                nSelGatePort = 0;
                for (var i = 0; i < Config.nRouteCount; i++)
                {
                    if (Config.boDynamicIPMode || (Config.GateRoute[i].sServerName == sServerName && Config.GateRoute[i].sPublicAddr == sIPaddr))
                    {
                        nGateCount = 0;
                        nGateIdx = 0;
                        while (true)
                        {
                            if (Config.GateRoute[i].Gate[nGateIdx].sIPaddr != "" && Config.GateRoute[i].Gate[nGateIdx].boEnable)
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
                        nSelIdx = Config.GateRoute[i].nSelIdx;
                        boSelected = false;
                        for (nGateIdx = nSelIdx + 1; nGateIdx <= 9; nGateIdx++)
                        {
                            if (Config.GateRoute[i].Gate[nGateIdx].sIPaddr != "" && Config.GateRoute[i].Gate[nGateIdx].boEnable)
                            {
                                Config.GateRoute[i].nSelIdx = nGateIdx;
                                boSelected = true;
                                break;
                            }
                        }
                        if (!boSelected)
                        {
                            for (nGateIdx = 0; nGateIdx < nSelIdx; nGateIdx++)
                            {
                                if (Config.GateRoute[i].Gate[nGateIdx].sIPaddr != "" && Config.GateRoute[i].Gate[nGateIdx].boEnable)
                                {
                                    Config.GateRoute[i].nSelIdx = nGateIdx;
                                    break;
                                }
                            }
                        }
                        nSelIdx = Config.GateRoute[i].nSelIdx;
                        sSelGateIP = Config.GateRoute[i].Gate[nSelIdx].sIPaddr;
                        nSelGatePort = Config.GateRoute[i].Gate[nSelIdx].nPort;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue("[Exception] LoginService.GetSelGateInfo");
                _logQueue.Enqueue(ex.StackTrace);
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
            var Config = _configManager.Config;
            try
            {
                for (var i = 0; i < Config.ServerNameList.Count; i++)
                {
                    string sServerName = Config.ServerNameList[i];
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        sServerInfo = sServerInfo + sServerName + "/" + _masSock.ServerStatus(sServerName) + "/";
                    }
                }
                result = sServerInfo;
            }
            catch
            {
                _logQueue.Enqueue("[Exception] LoginService.GetServerListInfo");
            }
            return result;
        }

        /// <summary>
        /// 选择服务器
        /// </summary>
        private void AccountSelectServer(Config Config, TUserInfo UserInfo, string sData)
        {
            ClientPacket DefMsg;
            bool boPayCost;
            string sSelGateIP = string.Empty;
            int nSelGatePort = 0;
            const string sSelServerMsg = "Server: {0}/{1}-{2}:{3}";
            var sServerName = EDcode.DeCodeString(sData);
            if (!string.IsNullOrEmpty(UserInfo.sAccount) && !string.IsNullOrEmpty(sServerName) && IsLogin(Config, UserInfo.nSessionID))
            {
                GetSelGateInfo(Config, sServerName, Config.sGateIPaddr, ref sSelGateIP, ref nSelGatePort);
                if (sSelGateIP != "" && nSelGatePort > 0)
                {
                    if (Config.boDynamicIPMode)
                    {
                        sSelGateIP = UserInfo.sGateIPaddr;
                    }
                    if (Config.ShowDetailMsg)
                    {
                        _logQueue.EnqueueDebugging(string.Format(sSelServerMsg, sServerName, Config.sGateIPaddr, sSelGateIP, nSelGatePort));
                    }
                    UserInfo.boSelServer = true;
                    boPayCost = false;
                    var nPayMode = 5;
                    if (UserInfo.nIDHour > 0)
                    {
                        nPayMode = 2;
                    }
                    if (UserInfo.nIPHour > 0)
                    {
                        nPayMode = 4;
                    }
                    if (UserInfo.nIPDay > 0)
                    {
                        nPayMode = 3;
                    }
                    if (UserInfo.nIDDay > 0)
                    {
                        nPayMode = 1;
                    }
                    if (_masSock.IsNotUserFull(sServerName))
                    {
                        SessionUpdate(Config, UserInfo.nSessionID, sServerName, boPayCost);
                        //向DBServer握手
                        _masSock.SendServerMsg(Grobal2.SS_OPENSESSION, sServerName, UserInfo.sAccount + "/" + UserInfo.nSessionID + "/" + (UserInfo.boPayCost ? 1 : 0) + "/" + nPayMode + "/" + UserInfo.sUserIPaddr);
                        DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELECTSERVER_OK, UserInfo.nSessionID, 0, 0, 0);
                        SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sSelGateIP + "/" + nSelGatePort + "/" + UserInfo.nSessionID));
                    }
                    else
                    {
                        UserInfo.boSelServer = false;
                        SessionDel(Config, UserInfo.nSessionID);
                        DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_STARTFAIL, 0, 0, 0, 0);
                        SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
                    }
                }
            }
        }

        /// <summary>
        /// 更新账号信息
        /// </summary>
        private void AccountUpdateUserInfo(TUserInfo UserInfo, string sData)
        {
            TAccountDBRecord DBRecord = null;
            UserFullEntry userFullEntry = null;
            ClientPacket DefMsg;
            try
            {
                if (string.IsNullOrEmpty(sData))
                {
                    _logQueue.Enqueue("[新建账号失败,数据包为空].");
                    return;
                }
                var deBuffer = EDcode.DecodeBuffer(sData);
                userFullEntry = Packets.ToPacket<UserFullEntry>(deBuffer);
                int nCode = -1;
                if (UserInfo.sAccount == userFullEntry.UserEntry.sAccount && LSShare.CheckAccountName(userFullEntry.UserEntry.sAccount))
                {
                    int n10 = _accountDB.Index(userFullEntry.UserEntry.sAccount);
                    if (n10 >= 0)
                    {
                        if (_accountDB.Get(n10, ref DBRecord) >= 0)
                        {
                            DBRecord.UserEntry = userFullEntry.UserEntry;
                            DBRecord.UserEntryAdd = userFullEntry.UserEntryAdd;
                            _accountDB.Update(n10, ref DBRecord);
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
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue("[Exception] LoginService.UpdateUserInfo");
                _logQueue.Enqueue(ex.StackTrace);
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        private void AccountGetBackPassword(TUserInfo UserInfo, string sData)
        {
            string sAccount = string.Empty;
            string sQuest1 = string.Empty;
            string sAnswer1 = string.Empty;
            string sQuest2 = string.Empty;
            string sAnswer2 = string.Empty;
            string sPassword = string.Empty;
            string sBirthDay = string.Empty;
            ClientPacket DefMsg;
            TAccountDBRecord DBRecord = null;
            string sMsg = EDcode.DeCodeString(sData);
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest1, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer1, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest2, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer2, new[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sBirthDay, new[] { "\09" });
            int nCode = 0;
            if (!string.IsNullOrEmpty(sAccount))
            {
                var nIndex = _accountDB.Index(sAccount);
                if (nIndex >= 0 && _accountDB.Get(nIndex, ref DBRecord) >= 0)
                {
                    if (DBRecord.nErrorCount < 5 || HUtil32.GetTickCount() - DBRecord.dwActionTick > 180000)
                    {
                        nCode = -1;
                        if (DBRecord.UserEntry.sQuiz == sQuest1)
                        {
                            nCode = -3;
                            if (DBRecord.UserEntry.sAnswer == sAnswer1)
                            {
                                if (DBRecord.UserEntryAdd.sBirthDay == sBirthDay)
                                {
                                    nCode = 1;
                                }
                            }
                        }
                        if (nCode != 1)
                        {
                            if (DBRecord.UserEntryAdd.sQuiz2 == sQuest2)
                            {
                                nCode = -3;
                                if (DBRecord.UserEntryAdd.sAnswer2 == sAnswer2)
                                {
                                    if (DBRecord.UserEntryAdd.sBirthDay == sBirthDay)
                                    {
                                        nCode = 1;
                                    }
                                }
                            }
                        }
                        if (nCode == 1)
                        {
                            sPassword = DBRecord.UserEntry.sPassword;
                        }
                        else
                        {
                            DBRecord.nErrorCount++;
                            DBRecord.dwActionTick = HUtil32.GetTickCount();
                            _accountDB.Update(nIndex, ref DBRecord);
                        }
                    }
                    else
                    {
                        nCode = -2;
                        if (HUtil32.GetTickCount() < DBRecord.dwActionTick)
                        {
                            DBRecord.dwActionTick = HUtil32.GetTickCount();
                            _accountDB.Update(nIndex, ref DBRecord);
                        }
                    }
                }
            }
            if (nCode == 1)
            {
                DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GETBACKPASSWD_SUCCESS, 0, 0, 0, 0);
                SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sPassword));
            }
            else
            {
                DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GETBACKPASSWD_FAIL, nCode, 0, 0, 0);
                SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
            }
        }

        private void SendGateMsg(Socket Socket, int sSockIndex, string sMsg)
        {
            if (Socket.Connected)
            {
                var sSendMsg = "%" + sSockIndex + "/#" + sMsg + "!$";
                Socket.SendText(sSendMsg);
            }
        }

        private bool IsLogin(Config Config, int nSessionID)
        {
            bool result = false;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                if (Config.SessionList[i].nSessionID == nSessionID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsLogin(Config Config, string sLoginID)
        {
            bool result = false;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                if (Config.SessionList[i].sAccount == sLoginID)
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
        private void SessionKick(Config Config, string sLoginID)
        {
            TConnInfo ConnInfo;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                ConnInfo = Config.SessionList[i];
                if (ConnInfo.sAccount == sLoginID && !ConnInfo.boKicked)
                {
                    _masSock.SendServerMsg(Grobal2.SS_CLOSESESSION, ConnInfo.sServerName, ConnInfo.sAccount + "/" + ConnInfo.nSessionID);
                    ConnInfo.dwKickTick = HUtil32.GetTickCount();
                    ConnInfo.boKicked = true;
                }
            }
        }

        private void SessionAdd(Config Config, string sAccount, string sIPaddr, int nSessionID, bool boPayCost, bool bo11)
        {
            TConnInfo ConnInfo = new TConnInfo();
            ConnInfo.sAccount = sAccount;
            ConnInfo.sIPaddr = sIPaddr;
            ConnInfo.nSessionID = nSessionID;
            ConnInfo.boPayCost = boPayCost;
            ConnInfo.bo11 = bo11;
            ConnInfo.dwKickTick = HUtil32.GetTickCount();
            ConnInfo.dwStartTick = HUtil32.GetTickCount();
            ConnInfo.boKicked = false;
            Config.SessionList.Add(ConnInfo);
        }

        private void SendGateKickMsg(Socket Socket, int sSockIndex)
        {
            var sSendMsg = $"%+-{sSockIndex}$";
            Socket.SendText(sSendMsg);
        }

        private void SessionUpdate(Config Config, int nSessionID, string sServerName, bool boPayCost)
        {
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                var ConnInfo = Config.SessionList[i];
                if (ConnInfo.nSessionID == nSessionID)
                {
                    ConnInfo.sServerName = sServerName;
                    ConnInfo.bo11 = boPayCost;
                    break;
                }
            }
        }

        /// <summary>
        /// 清理没有付费的账号
        /// </summary>
        public void SessionClearNoPayMent()
        {
            Config Config = _configManager.Config;
            for (var i = Config.SessionList.Count - 1; i >= 0; i--)
            {
                var ConnInfo = Config.SessionList[i];
                if (!ConnInfo.boKicked && !Config.TestServer && !ConnInfo.bo11)
                {
                    if (HUtil32.GetTickCount() - ConnInfo.dwStartTick > 60 * 60 * 1000)
                    {
                        ConnInfo.dwStartTick = HUtil32.GetTickCount();
                        if (!IsPayMent(Config, ConnInfo.sIPaddr, ConnInfo.sAccount))
                        {
                            _masSock.SendServerMsg(Grobal2.SS_KICKUSER, ConnInfo.sServerName, ConnInfo.sAccount + "/" + ConnInfo.nSessionID);
                            Config.SessionList[i] = null;
                            Config.SessionList.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public void LoadIPaddrCostList(Config Config, AccountConst accountConst)
        {
            Config.IPaddrCostList.Clear();
            Config.IPaddrCostList.Add(accountConst.s1C, accountConst.nC);
        }

        public void LoadAccountCostList(Config Config, AccountConst accountConst)
        {
            Config.AccountCostList.Clear();
            Config.AccountCostList.Add(accountConst.s1C, accountConst.nC);
        }

        public void LoadConfig()
        {
            _configManager.LoadConfig();
            _configManager.LoadAddrTable();
            _accountDB.Initialization();
        }
    }
}