using LoginSrv.Conf;
using LoginSrv.Storage;
using NLog;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;
using SystemModule.SocketComponents;
using SystemModule.Sockets;

namespace LoginSrv.Services
{
    public class ClientSession
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConfigManager _configMgr;
        private readonly AccountStorage _accountStorage;
        private readonly SessionServer _sessionService;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;
        private readonly Channel<UserSessionData> _packetQueue;

        public ClientSession(AccountStorage accountStorage, ConfigManager configManager, SessionServer sessionServer, ClientManager clientManager, SessionManager sessionManager)
        {
            _configMgr = configManager;
            _accountStorage = accountStorage;
            _sessionService = sessionServer;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
            _packetQueue = Channel.CreateUnbounded<UserSessionData>();
        }

        public void Start(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                await ProcessUserMessage(stoppingToken);
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void Enqueue(UserSessionData userData)
        {
            _packetQueue.Writer.TryWrite(userData);
        }

        /// <summary>
        /// 处理登陆封包消息
        /// </summary>
        /// <returns></returns>
        private async Task ProcessUserMessage(CancellationToken stoppingToken)
        {
            while (await _packetQueue.Reader.WaitToReadAsync(stoppingToken))
            {
                while (_packetQueue.Reader.TryRead(out var message))
                {
                    try
                    {
                        var sMsg = string.Empty;
                        if (!message.Msg.EndsWith("!"))
                        {
                            return;
                        }
                        HUtil32.ArrestStringEx(message.Msg, "#", "!", ref sMsg);
                        if (string.IsNullOrEmpty(sMsg))
                            return;
                        if (sMsg.Length < Messages.DefBlockSize)
                            return;
                        sMsg = sMsg.AsSpan()[1..sMsg.Length].ToString();

                        var clientSession = _clientManager.GetSession(message.SessionId);
                        if (clientSession == null)
                        {
                            continue;
                        }
                        for (var i = 0; i < clientSession.UserList.Count; i++)
                        {
                            var userInfo = clientSession.UserList[i];
                            if (userInfo.SockIndex == message.SoketId)
                            {
                                ProcessUserMsg(clientSession, userInfo, sMsg);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] LoginService.DecodeUserData");
                        _logger.Error(ex);
                    }
                }
            }
        }

        private void ProcessUserMsg(LoginGateInfo gateInfo, UserInfo userInfo, string sMsg)
        {
            var sDefMsg = sMsg[..Messages.DefBlockSize];
            var sData = sMsg.Substring(Messages.DefBlockSize, sMsg.Length - Messages.DefBlockSize);
            var defMsg = EDCode.DecodePacket(sDefMsg);
            switch (defMsg.Ident)
            {
                case Messages.CM_SELECTSERVER:
                    if (!userInfo.SelServer)
                    {
                        AccountSelectServer(userInfo, sData);
                    }
                    break;
                case Messages.CM_PROTOCOL:
                    AccountCheckProtocol(userInfo, defMsg.Recog);
                    break;
                case Messages.CM_IDPASSWORD:
                    if (string.IsNullOrEmpty(userInfo.Account))
                    {
                        AccountLogin(userInfo, sData);
                    }
                    else
                    {
                        KickUser(gateInfo, ref userInfo);
                    }
                    break;
                case Messages.CM_ADDNEWUSER:
                    if (_configMgr.Config.EnableMakingID)
                    {
                        if (HUtil32.GetTickCount() - userInfo.LastCreateAccountTick > 1000)
                        {
                            userInfo.LastCreateAccountTick = HUtil32.GetTickCount();
                            AccountCreate(ref userInfo, sData);
                        }
                        else
                        {
                            _logger.Warn("[超速操作] 创建帐号/" + userInfo.UserIPaddr);
                        }
                    }
                    else
                    {
                        //todo Recog=99 暂未开放账号注册
                        defMsg = Messages.MakeMessage(Messages.SM_NEWID_FAIL, 99, 0, 0, 0);
                        SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
                    }
                    break;
                case Messages.CM_CHANGEPASSWORD:
                    if (string.IsNullOrEmpty(userInfo.Account))
                    {
                        if (HUtil32.GetTickCount() - userInfo.LastUpdatePwdTick > 5000)
                        {
                            userInfo.LastUpdatePwdTick = HUtil32.GetTickCount();
                            AccountChangePassword(userInfo, sData);
                        }
                        else
                        {
                            _logger.Warn("[超速操作] 修改密码 /" + userInfo.UserIPaddr);
                        }
                    }
                    else
                    {
                        userInfo.Account = string.Empty;
                    }
                    break;
                case Messages.CM_UPDATEUSER:
                    if (HUtil32.GetTickCount() - userInfo.LastUpdateAccountTick > 5000)
                    {
                        userInfo.LastUpdateAccountTick = HUtil32.GetTickCount();
                        AccountUpdateUserInfo(userInfo, sData);
                    }
                    else
                    {
                        _logger.Warn("[超速操作] 更新帐号 /" + userInfo.UserIPaddr);
                    }
                    break;
                case Messages.CM_GETBACKPASSWORD:
                    if (HUtil32.GetTickCount() - userInfo.LastGetBackPwdTick > 5000)
                    {
                        userInfo.LastGetBackPwdTick = HUtil32.GetTickCount();
                        AccountGetBackPassword(userInfo, sData);
                    }
                    else
                    {
                        _logger.Warn("[超速操作] 找回密码 /" + userInfo.UserIPaddr);
                    }
                    break;
            }
        }

        /// <summary>
        /// 账号登陆
        /// </summary>
        private void AccountLogin(UserInfo userInfo, string sData)
        {
            var sLoginId = string.Empty;
            UserEntry userEntry = null;
            AccountRecord accountRecord = null;
            try
            {
                userInfo.Seconds = 0;
                var sPassword = HUtil32.GetValidStr3(EDCode.DeCodeString(sData), ref sLoginId,'/');
                var nCode = 0;
                var boNeedUpdate = false;
                var accountIndex = _accountStorage.Index(sLoginId);
                if (accountIndex > 0 && _accountStorage.Get(accountIndex, ref accountRecord) > 0)
                {
                    if (accountRecord == null)
                    {
                        _logger.Error($"获取账号{sLoginId}资料出错");
                        _accountStorage.Get(accountIndex, ref accountRecord);
                        return;
                    }
                    if (accountRecord.ErrorCount < 5 || HUtil32.GetTickCount() - accountRecord.ActionTick > 60000)
                    {
                        if (string.Compare(accountRecord.UserEntry.Password, sPassword, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            accountRecord.ErrorCount = 0;
                            if (string.IsNullOrEmpty(accountRecord.UserEntry.UserName) || string.IsNullOrEmpty(accountRecord.UserEntryAdd.Quiz2))
                            {
                                userEntry = accountRecord.UserEntry;
                                boNeedUpdate = true;
                            }
                            nCode = 1;
                        }
                        else
                        {
                            accountRecord.ErrorCount++;
                            accountRecord.ActionTick = HUtil32.GetTickCount();
                            nCode = -1;
                        }
                    }
                    else
                    {
                        nCode = -2;
                        accountRecord.ActionTick = HUtil32.GetTickCount();
                    }
                    _accountStorage.UpdateLoginRecord(accountRecord);
                }
                if (nCode == 1 && _sessionManager.IsLogin(sLoginId))
                {
                    SessionKick(sLoginId);
                    nCode = -3;
                }
                CommandMessage defMsg;
                if (boNeedUpdate)
                {
                    defMsg = Messages.MakeMessage(Messages.SM_NEEDUPDATE_ACCOUNT, 0, 0, 0, 0);
                    SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeBuffer(userEntry));
                    userInfo.Account = sLoginId;
                    userInfo.SessionID = LsShare.GetSessionId();
                    userInfo.SelServer = false;
                    return;
                }
                if (nCode == 1 && accountRecord != null)
                {
                    userInfo.Account = sLoginId;
                    userInfo.SessionID = LsShare.GetSessionId();
                    userInfo.SelServer = false;
                    if (accountRecord.PayModel > 1 && accountRecord.PlayTime > 0) //点卡和月卡
                    {
                        userInfo.PayCost = true;
                        userInfo.Seconds = accountRecord.PlayTime;
                        userInfo.PayMode = (byte)accountRecord.PayModel;
                        AddCertUser(userInfo);
                        /*if (CheckBadAccount(userInfo.Account))
                        {
                             var szMessage = $"{st.Year}-{st.Month}-{st.Day} {st.Hour}:{st.Minute} {st.Second} {userInfo.Account} {userInfo.UserIPaddr}";
                        }*/
                        _logger.Debug($"账号[{userInfo.Account}] 登陆IP:[{userInfo.UserIPaddr}] 游戏截止时间:[{FormatSecond(userInfo.Seconds)}]");
                    }
                    else if (accountRecord.PayModel == 0)
                    {
                        userInfo.PayCost = false;
                    }
                    var sServerName = GetServerListInfo();
                    defMsg = Messages.MakeMessage(Messages.SM_PASSOK_SELECTSERVER, 0, 0, 0, _configMgr.Config.ServerNameList.Count);
                    SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sServerName));
                    SessionAdd(userInfo.Account, userInfo.UserIPaddr, userInfo.SessionID, userInfo.PayCost, accountRecord.PayModel > 0);
                }
                else
                {
                    defMsg = Messages.MakeMessage(Messages.SM_PASSWD_FAIL, nCode, 0, 0, 0);
                    SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] LoginService.LoginUser");
                _logger.Error(ex);
            }
        }

        private static string FormatSecond(long second)
        {
            var days = Math.Floor(second / 86400f);
            var hours = Math.Floor((second % 86400f) / 3600);
            var minutes = Math.Floor(((second % 86400f) % 3600) / 60);
            var seconds = Math.Floor(((second % 86400f) % 3600) % 60);
            return $"{days}天{hours}小时{minutes}分钟{seconds}秒";
        }

        private static bool CheckBadAccount(string account)
        {
            return true;
        }

        private static void AddCertUser(UserInfo pUser)
        {
            var pCert = new CertUser();
            pCert.LoginID = pUser.Account;
            pCert.Addr = pUser.UserIPaddr;
            pCert.IDHour = pUser.Seconds;
            pCert.IPDay = 0;
            pCert.IDDay = 0;
            pUser.AvailableType = 5;
            if ((pCert.IDHour > 0))
            {
                pUser.AvailableType = 2;
            }
            if ((pCert.IPHour > 0))
            {
                pUser.AvailableType = 4;
            }
            if ((pCert.IPDay > 0))
            {
                pUser.AvailableType = 3;
            }
            if ((pCert.IDDay > 0))
            {
                pUser.AvailableType = 1;
            }
            pCert.Certification = pUser.Certification;
            pCert.OpenTime = HUtil32.GetTickCount();
            pCert.AvailableType = pUser.AvailableType;
            pCert.Closing = false;
            LsShare.CertList.Add(pCert);
        }

        private static void DelCertUser(int cert)
        {
            for (var i = LsShare.CertList.Count - 1; i >= 0; i--)
            {
                if (LsShare.CertList[i].Certification == cert)
                {
                    LsShare.CertList.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 账号注册
        /// </summary>
        private void AccountCreate(ref UserInfo userInfo, string sData)
        {
            var success = false;
            const string sAddNewuserFail = "[新建帐号失败] {0}/{1}";
            try
            {
                if (string.IsNullOrEmpty(sData))
                {
                    _logger.Warn("[新建账号失败] 数据包为空或数据包长度异常");
                    return;
                }
                var accountStrSize = (byte)Math.Ceiling((decimal)(UserEntry.Size * 4) / 3);
                if (sData.Length <= accountStrSize)
                {
                    return;
                }
                var ueBuff = EDCode.DecodeBuffer(sData[..accountStrSize]);
                var uaBuff = EDCode.DecodeBuffer(sData[accountStrSize..]);
                var accountBuff = new byte[ueBuff.Length + uaBuff.Length];
                Buffer.BlockCopy(ueBuff, 0, accountBuff, 0, ueBuff.Length);
                Buffer.BlockCopy(uaBuff, 0, accountBuff, ueBuff.Length, uaBuff.Length);
                var userAccount = ClientPacket.ToPacket<UserAccountPacket>(accountBuff);
                if (userAccount == null || userAccount.UserEntry == null || userAccount.UserEntryAdd == null)
                {
                    _logger.Warn("[新建账号失败] 解析封包出现异常.");
                    return;
                }
                var nErrCode = -1;
                if (LsShare.VerifyAccountRule(userAccount.UserEntry.Account))
                {
                    success = true;
                }
                if (success)
                {
                    var n10 = _accountStorage.Index(userAccount.UserEntry.Account);
                    if (n10 <= 0)
                    {
                        var accountRecord = new AccountRecord();
                        accountRecord.UserEntry = userAccount.UserEntry;
                        accountRecord.UserEntryAdd = userAccount.UserEntryAdd;
                        if (!string.IsNullOrEmpty(userAccount.UserEntry.Account))
                        {
                            if (_accountStorage.Add(accountRecord))
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
                    _logger.Warn(string.Format(sAddNewuserFail, userAccount.UserEntry.Account, userAccount.UserEntryAdd.Quiz2));
                }
                CommandMessage defMsg;
                if (nErrCode == 1)
                {
                    defMsg = Messages.MakeMessage(Messages.SM_NEWID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Messages.MakeMessage(Messages.SM_NEWID_FAIL, nErrCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] LoginsService.AccountCreate");
                _logger.Error(ex);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void AccountChangePassword(UserInfo userInfo, string sData)
        {
            var sLoginId = string.Empty;
            var sOldPassword = string.Empty;
            AccountRecord accountRecord = null;
            try
            {
                var sMsg = EDCode.DeCodeString(sData);
                sMsg = HUtil32.GetValidStr3(sMsg, ref sLoginId, new[] { "\09", "\t" });
                var sNewPassword = HUtil32.GetValidStr3(sMsg, ref sOldPassword, new[] { "\09", "\t" });
                var nCode = 0;
                if (sNewPassword.Length >= 3)
                {
                    var n10 = _accountStorage.Index(sLoginId);
                    if (n10 >= 0 && _accountStorage.Get(n10, ref accountRecord) >= 0)
                    {
                        if (accountRecord.ErrorCount < 5 || HUtil32.GetTickCount() - accountRecord.ActionTick > 180000)
                        {
                            if (string.Compare(accountRecord.UserEntry.Password, sOldPassword, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                nCode = _accountStorage.ChanggePassword(n10, sNewPassword);
                            }
                            else
                            {
                                accountRecord.ErrorCount++;
                                accountRecord.ActionTick = HUtil32.GetTickCount();
                                nCode = -1;
                            }
                        }
                        else
                        {
                            nCode = -2;
                            if (HUtil32.GetTickCount() < accountRecord.ActionTick)
                            {
                                accountRecord.ActionTick = HUtil32.GetTickCount();
                                _accountStorage.Update(n10, ref accountRecord);
                            }
                        }
                    }
                }

                CommandMessage defMsg;
                if (nCode == 1)
                {
                    defMsg = Messages.MakeMessage(Messages.SM_CHGPASSWD_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Messages.MakeMessage(Messages.SM_CHGPASSWD_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] LoginService.ChangePassword");
                _logger.Error(ex);
            }
        }

        /// <summary>
        /// 选择服务器
        /// </summary>
        private void AccountSelectServer(UserInfo userInfo, string sData)
        {
            CommandMessage defMsg;
            var sSelGateIp = string.Empty;
            var nSelGatePort = 0;
            const string sSelServerMsg = "Server: {0}/{1}-{2}:{3}";
            var sServerName = EDCode.DeCodeString(sData);
            if (!string.IsNullOrEmpty(userInfo.Account) && !string.IsNullOrEmpty(sServerName) && _sessionManager.IsLogin(userInfo.SessionID))
            {
                GetSelGateInfo(sServerName, _configMgr.Config.sGateIPaddr, ref sSelGateIp, ref nSelGatePort);
                if (!string.IsNullOrEmpty(sSelGateIp) && nSelGatePort > 0)
                {
                    if (_configMgr.Config.DynamicIPMode)
                    {
                        sSelGateIp = userInfo.GateIPaddr;
                    }
                    _logger.Debug(string.Format(sSelServerMsg, sServerName, _configMgr.Config.sGateIPaddr, sSelGateIp, nSelGatePort));
                    userInfo.SelServer = true;
                    var nPayMode = userInfo.PayMode;
                    if (_sessionService.IsNotUserFull(sServerName))
                    {
                        _sessionManager.UpdateSession(userInfo.SessionID, sServerName, nPayMode > 0);
                        _sessionService.SendServerMsg(Messages.SS_OPENSESSION, sServerName, userInfo.Account + "/" + userInfo.SessionID + "/" + (userInfo.PayCost ? 1 : 0) + "/" + nPayMode + "/" + userInfo.UserIPaddr + "/" + userInfo.Seconds);
                        if (nPayMode > 0)
                        {
                            var playTimeSpan = DateTimeOffset.Now.AddSeconds(userInfo.Seconds) - DateTimeOffset.Now;
                            defMsg = Messages.MakeMessage(Messages.SM_SELECTSERVER_OK, (int)Math.Round(playTimeSpan.TotalSeconds, 1), 0, userInfo.PayMode, 0);
                        }
                        else
                        {
                            defMsg = Messages.MakeMessage(Messages.SM_SELECTSERVER_OK, userInfo.SessionID, 0, 0, 0);
                        }
                        SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sSelGateIp + "/" + nSelGatePort + "/" + userInfo.SessionID));
                    }
                    else
                    {
                        userInfo.SelServer = false;
                        _sessionManager.Delete(userInfo.Account, userInfo.SessionID);
                        defMsg = Messages.MakeMessage(Messages.SM_STARTFAIL, 0, 0, 0, 0);
                        SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
                    }
                }
            }
        }

        /// <summary>
        /// 更新账号信息
        /// </summary>
        private void AccountUpdateUserInfo(UserInfo userInfo, string sData)
        {
            CommandMessage defMsg;
            try
            {
                if (string.IsNullOrEmpty(sData))
                {
                    _logger.Warn("[更新账号失败] 数据包为空或数据包长度异常");
                    return;
                }
                var accountStrSize = (byte)Math.Ceiling((decimal)(UserEntry.Size * 4) / 3);
                if (sData.Length <= accountStrSize)
                {
                    return;
                }
                var ueBuff = EDCode.DecodeBuffer(sData[..accountStrSize]);
                var uaBuff = EDCode.DecodeBuffer(sData[accountStrSize..]);
                var accountBuff = new byte[ueBuff.Length + uaBuff.Length];
                Buffer.BlockCopy(ueBuff, 0, accountBuff, 0, ueBuff.Length);
                Buffer.BlockCopy(uaBuff, 0, accountBuff, ueBuff.Length, uaBuff.Length);
                var userAccount = ClientPacket.ToPacket<UserAccountPacket>(accountBuff);
                var nCode = -1;
                if (string.Compare(userInfo.Account, userAccount.UserEntry.Account, StringComparison.OrdinalIgnoreCase) == 0 && LsShare.VerifyAccountRule(userAccount.UserEntry.Account))
                {
                    var accountIndex = _accountStorage.Index(userAccount.UserEntry.Account);
                    if (accountIndex >= 0)
                    {
                        AccountRecord accountRecord = null;
                        if (_accountStorage.Get(accountIndex, ref accountRecord) >= 0)
                        {
                            accountRecord.UserEntry = userAccount.UserEntry;
                            accountRecord.UserEntryAdd = userAccount.UserEntryAdd;
                            nCode = _accountStorage.UpdateAccount(accountIndex, ref accountRecord);
                        }
                    }
                    else
                    {
                        nCode = 0;
                    }
                }
                if (nCode == 1)
                {
                    defMsg = Messages.MakeMessage(Messages.SM_UPDATEID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Messages.MakeMessage(Messages.SM_UPDATEID_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] LoginService.UpdateUserInfo");
                _logger.Error(ex);
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        private void AccountGetBackPassword(UserInfo userInfo, string sData)
        {
            var sAccount = string.Empty;
            var sQuest1 = string.Empty;
            var sAnswer1 = string.Empty;
            var sQuest2 = string.Empty;
            var sAnswer2 = string.Empty;
            var sPassword = string.Empty;
            var sBirthDay = string.Empty;
            CommandMessage defMsg;
            AccountRecord accountRecord = null;
            var sMsg = EDCode.DeCodeString(sData);
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, "\09");
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest1, "\09");
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer1, "\09");
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest2, "\09");
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer2, "\09");
            sMsg = HUtil32.GetValidStr3(sMsg, ref sBirthDay, "\09");
            var nCode = 0;
            if (!string.IsNullOrEmpty(sAccount))
            {
                var nIndex = _accountStorage.Index(sAccount);
                if (nIndex >= 0 && _accountStorage.Get(nIndex, ref accountRecord) >= 0)
                {
                    if (accountRecord.ErrorCount < 5 || HUtil32.GetTickCount() - accountRecord.ActionTick > 180000)
                    {
                        nCode = -1;
                        if (accountRecord.UserEntry.Quiz == sQuest1)
                        {
                            nCode = -3;
                            if (accountRecord.UserEntry.Answer == sAnswer1)
                            {
                                if (accountRecord.UserEntryAdd.BirthDay == sBirthDay)
                                {
                                    nCode = 1;
                                }
                            }
                        }
                        if (nCode != 1)
                        {
                            if (accountRecord.UserEntryAdd.Quiz2 == sQuest2)
                            {
                                nCode = -3;
                                if (accountRecord.UserEntryAdd.Answer2 == sAnswer2)
                                {
                                    if (accountRecord.UserEntryAdd.BirthDay == sBirthDay)
                                    {
                                        nCode = 1;
                                    }
                                }
                            }
                        }
                        if (nCode == 1)
                        {
                            sPassword = accountRecord.UserEntry.Password;
                        }
                        else
                        {
                            accountRecord.ErrorCount++;
                            accountRecord.ActionTick = HUtil32.GetTickCount();
                            _accountStorage.Update(nIndex, ref accountRecord);
                        }
                    }
                    else
                    {
                        nCode = -2;
                        if (HUtil32.GetTickCount() < accountRecord.ActionTick)
                        {
                            accountRecord.ActionTick = HUtil32.GetTickCount();
                            _accountStorage.Update(nIndex, ref accountRecord);
                        }
                    }
                }
            }
            if (nCode == 1)
            {
                defMsg = Messages.MakeMessage(Messages.SM_GETBACKPASSWD_SUCCESS, 0, 0, 0, 0);
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sPassword));
            }
            else
            {
                defMsg = Messages.MakeMessage(Messages.SM_GETBACKPASSWD_FAIL, nCode, 0, 0, 0);
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
        }

        private void AccountCheckProtocol(UserInfo userInfo, int nDate)
        {
            CommandMessage defMsg;
            if (nDate < LsShare.VersionDate)
            {
                defMsg = Messages.MakeMessage(Messages.SM_CERTIFICATION_FAIL, 0, 0, 0, 0);
            }
            else
            {
                defMsg = Messages.MakeMessage(Messages.SM_CERTIFICATION_SUCCESS, 0, 0, 0, 0);
            }
            SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
        }

        /// <summary>
        /// 剔除会话
        /// </summary>
        private void SessionKick(string sLoginId)
        {
            var connInfo = _sessionManager.GetSession(sLoginId);
            if (connInfo != null && !connInfo.Kicked)
            {
                _sessionService.SendServerMsg(Messages.SS_CLOSESESSION, connInfo.ServerName, connInfo.Account + "/" + connInfo.SessionID);
                connInfo.KickTick = HUtil32.GetTickCount();
                connInfo.Kicked = true;
                connInfo.SessionID = connInfo.SessionID;
            }
        }

        private void SessionAdd(string sAccount, string sIPaddr, int nSessionId, bool boPayCost, bool payMent)
        {
            var connInfo = new SessionConnInfo();
            connInfo.Account = sAccount;
            connInfo.IPaddr = sIPaddr;
            connInfo.SessionID = nSessionId;
            connInfo.boPayCost = boPayCost;
            connInfo.IsPayMent = payMent;
            connInfo.KickTick = HUtil32.GetTickCount();
            connInfo.StartTick = HUtil32.GetTickCount();
            connInfo.Kicked = false;
            _sessionManager.AddSession(nSessionId, connInfo);
        }

        private void SendGateMsg(Socket socket, int sSockIndex, string sMsg)
        {
            if (socket.Connected)
            {
                var packet = new ServerDataMessage();
                packet.SocketId = sSockIndex;
                packet.Data = HUtil32.GetBytes("#" + sMsg + "!$");
                packet.DataLen = (short)packet.Data.Length;
                packet.Type = ServerDataType.Data;
                var sendBuffer = SerializerUtil.Serialize(packet);
                SendMessage(socket, sendBuffer);
            }
            else
            {
                _logger.Error("登陆网关链接断开，消息发送失败");
            }
        }

        private static void SendMessage(Socket socket, byte[] sendBuffer)
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
            socket.Send(data);
        }

        private void KickUser(LoginGateInfo gateInfo, ref UserInfo userInfo)
        {
            const string sKickMsg = "Kick: {0}";
            _logger.Debug(string.Format(sKickMsg, userInfo.UserIPaddr));
            SendGateKickMsg(gateInfo.Socket, userInfo.SockIndex);
            gateInfo.UserList.Remove(userInfo);
            userInfo = null;
        }

        /// <summary>
        /// 获取角色网关信息
        /// </summary>
        private void GetSelGateInfo(string sServerName, string sIPaddr, ref string sSelGateIp, ref int nSelGatePort)
        {
            int nGateIdx;
            int nGateCount;
            int nSelIdx;
            bool boSelected;
            try
            {
                sSelGateIp = "";
                nSelGatePort = 0;
                for (var i = 0; i < _configMgr.Config.RouteCount; i++)
                {
                    if (_configMgr.Config.DynamicIPMode || (_configMgr.Config.GateRoute[i].ServerName == sServerName && _configMgr.Config.GateRoute[i].PublicAddr == sIPaddr))
                    {
                        nGateCount = 0;
                        nGateIdx = 0;
                        while (true)
                        {
                            if (!string.IsNullOrEmpty(_configMgr.Config.GateRoute[i].Gate[nGateIdx].sIPaddr) && _configMgr.Config.GateRoute[i].Gate[nGateIdx].boEnable)
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
                        nSelIdx = _configMgr.Config.GateRoute[i].nSelIdx;
                        boSelected = false;
                        for (nGateIdx = nSelIdx + 1; nGateIdx <= 9; nGateIdx++)
                        {
                            if (!string.IsNullOrEmpty(_configMgr.Config.GateRoute[i].Gate[nGateIdx].sIPaddr) && _configMgr.Config.GateRoute[i].Gate[nGateIdx].boEnable)
                            {
                                _configMgr.Config.GateRoute[i].nSelIdx = nGateIdx;
                                boSelected = true;
                                break;
                            }
                        }
                        if (!boSelected)
                        {
                            for (nGateIdx = 0; nGateIdx < nSelIdx; nGateIdx++)
                            {
                                if (!string.IsNullOrEmpty(_configMgr.Config.GateRoute[i].Gate[nGateIdx].sIPaddr) && _configMgr.Config.GateRoute[i].Gate[nGateIdx].boEnable)
                                {
                                    _configMgr.Config.GateRoute[i].nSelIdx = nGateIdx;
                                    break;
                                }
                            }
                        }
                        nSelIdx = _configMgr.Config.GateRoute[i].nSelIdx;
                        sSelGateIp = _configMgr.Config.GateRoute[i].Gate[nSelIdx].sIPaddr;
                        nSelGatePort = _configMgr.Config.GateRoute[i].Gate[nSelIdx].nPort;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] LoginService.GetSelGateInfo");
                _logger.Error(ex);
            }
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        private string GetServerListInfo()
        {
            var result = string.Empty;
            var sServerInfo = string.Empty;
            try
            {
                for (var i = 0; i < _configMgr.Config.ServerNameList.Count; i++)
                {
                    var sServerName = _configMgr.Config.ServerNameList[i];
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        sServerInfo = sServerInfo + sServerName + "/" + _sessionService.GetServerStatus(sServerName) + "/";
                    }
                }
                result = sServerInfo;
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] LoginService.GetServerListInfo");
                _logger.Error(ex);
            }
            return result;
        }

        private static void SendGateKickMsg(Socket socket, int sSockIndex)
        {
            var sSendMsg = $"%+-{sSockIndex}$";
            socket.SendText(sSendMsg);
        }
    }
}