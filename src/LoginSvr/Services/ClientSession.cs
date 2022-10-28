using LoginSvr.Conf;
using LoginSvr.Storage;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;
using SystemModule.Sockets;

namespace LoginSvr.Services
{
    public class ClientSession
    {
        private readonly MirLog _logger;
        private readonly AccountStorage _accountStorage;
        private readonly SessionService _masSocService;
        private readonly ConfigManager _configManager;
        private readonly Channel<UserSessionData> _userMessageQueue;

        public ClientSession(MirLog logger, AccountStorage accountStorage, ConfigManager configManager, SessionService masSocService)
        {
            _logger = logger;
            _accountStorage = accountStorage;
            _configManager = configManager;
            _masSocService = masSocService;
            _userMessageQueue = Channel.CreateUnbounded<UserSessionData>();
        }

        public void Start(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                await ProcessUserMessage(stoppingToken);
            }, stoppingToken);
        }

        public void SendToQueue(UserSessionData userData)
        {
            _userMessageQueue.Writer.TryWrite(userData);
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

        private void DecodeUserData(UserInfo userInfo, string userData)
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
                _logger.LogError("[Exception] LoginService.DecodeUserData");
                _logger.LogError(ex);
            }
        }

        private void ProcessUserMsg(UserInfo userInfo, string sMsg)
        {
            var sDefMsg = sMsg.Substring(0, Grobal2.DEFBLOCKSIZE);
            var sData = sMsg.Substring(Grobal2.DEFBLOCKSIZE, sMsg.Length - Grobal2.DEFBLOCKSIZE);
            var defMsg = EDCode.DecodePacket(sDefMsg);
            switch (defMsg.Ident)
            {
                case Grobal2.CM_SELECTSERVER:
                    if (!userInfo.SelServer)
                    {
                        AccountSelectServer(_configManager.Config, userInfo, sData);
                    }
                    break;
                case Grobal2.CM_PROTOCOL:
                    AccountCheckProtocol(userInfo, defMsg.Recog);
                    break;
                case Grobal2.CM_IDPASSWORD:
                    if (string.IsNullOrEmpty(userInfo.Account))
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
                        if (HUtil32.GetTickCount() - userInfo.ClientTick > 5000)
                        {
                            AccountCreate(ref userInfo, sData);
                        }
                        else
                        {
                            _logger.Warn("[超速操作] 创建帐号/" + userInfo.UserIPaddr);
                        }
                    }
                    break;
                case Grobal2.CM_CHANGEPASSWORD:
                    if (string.IsNullOrEmpty(userInfo.Account))
                    {
                        if (HUtil32.GetTickCount() - userInfo.ClientTick > 5000)
                        {
                            userInfo.ClientTick = HUtil32.GetTickCount();
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
                case Grobal2.CM_UPDATEUSER:
                    if (HUtil32.GetTickCount() - userInfo.ClientTick > 5000)
                    {
                        userInfo.ClientTick = HUtil32.GetTickCount();
                        AccountUpdateUserInfo(userInfo, sData);
                    }
                    else
                    {
                        _logger.Warn("[超速操作] 更新帐号 /" + userInfo.UserIPaddr);
                    }
                    break;
                case Grobal2.CM_GETBACKPASSWORD:
                    if (HUtil32.GetTickCount() - userInfo.ClientTick > 5000)
                    {
                        userInfo.ClientTick = HUtil32.GetTickCount();
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
        private void AccountLogin(Config config, UserInfo userInfo, string sData)
        {
            var sLoginId = string.Empty;
            UserEntry userEntry = null;
            var nIdCostIndex = -1;
            var nIpCostIndex = -1;
            AccountRecord accountRecord = null;
            try
            {
                userInfo.dwValidFrom = 0;
                userInfo.dwValidUntil = 0;
                userInfo.Seconds = 0;
                userInfo.dwStopUntil = 0;
                userInfo.dwIpValidFrom = 0;
                userInfo.dwIpValidUntil = 0;
                userInfo.dwIpSeconds = 0;
                var sPassword = HUtil32.GetValidStr3(EDCode.DeCodeString(sData), ref sLoginId, new[] { "/" });
                var nCode = 0;
                var boNeedUpdate = false;
                var accountIndex = _accountStorage.Index(sLoginId);
                if (accountIndex > 0 && _accountStorage.Get(accountIndex, ref accountRecord) > 0)
                {
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
                if (nCode == 1 && IsLogin(config, sLoginId))
                {
                    SessionKick(config, sLoginId);
                    nCode = -3;
                }
                ClientMesaagePacket defMsg;
                if (boNeedUpdate)
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEEDUPDATE_ACCOUNT, 0, 0, 0, 0);
                    SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeBuffer(userEntry));
                    userInfo.Account = sLoginId;
                    userInfo.SessionID = LsShare.GetSessionId();
                    userInfo.SelServer = false;
                    return;
                }
                if (nCode == 1)
                {
                    userInfo.Account = sLoginId;
                    userInfo.SessionID = LsShare.GetSessionId();
                    userInfo.SelServer = false;
                    if (config.AccountCostList.ContainsKey(userInfo.Account))
                    {
                        nIdCostIndex = config.AccountCostList[userInfo.Account];
                    }
                    if (config.IPaddrCostList.ContainsKey(userInfo.UserIPaddr))
                    {
                        nIpCostIndex = config.IPaddrCostList[userInfo.UserIPaddr];
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
                        userInfo.PayCost = true;
                    }
                    else
                    {
                        userInfo.PayCost = false;
                    }
                    userInfo.Seconds = accountRecord.PlayTime;
                    var st = DateTime.Now;
                    var nCurrentTime = GetDay(st.Year, st.Month, st.Day);
                    if ((nCurrentTime <= userInfo.dwValidUntil) || (nCurrentTime <= userInfo.dwIpValidUntil) || (userInfo.Seconds > 0) || (userInfo.dwIpSeconds > 0))
                    {
                        userInfo.PayMode = 1;
                        
                        long RemainDays = 0;
                        long RemainHours = 0;
                        var playSpan = DateTimeOffset.Now.AddMilliseconds(userInfo.Seconds) - DateTimeOffset.Now;

                        var playModel = 0;
                        RemainDays = (int)Math.Round(playSpan.TotalDays, 1);
                        RemainHours = (int)Math.Ceiling(playSpan.TotalHours);
                        if (RemainDays < 0)
                            RemainDays = 0;
                        if (RemainHours < 0)
                            RemainHours = 0;

                        userInfo.IDDay = HUtil32.LoWord(nIdCost);
                        userInfo.IDHour = HUtil32.HiWord(nIdCost);
                        userInfo.IPDay = HUtil32.LoWord(nIpCost);
                        userInfo.IPHour = HUtil32.HiWord(nIpCost);
           
                        AddCertUser(userInfo);

                        if (CheckBadAccount(userInfo.Account))
                        {
                           // var szMessage = $"{st.Year}-{st.Month}-{st.Day} {st.Hour}:{st.Minute} {st.Second} {userInfo.Account} {userInfo.UserIPaddr}";
                        }
                        var playTime = DateTimeOffset.Now.AddMilliseconds(userInfo.Seconds);
                        _logger.LogDebug($"账号[{userInfo.Account}] 登陆IP:[{userInfo.UserIPaddr}] 游戏到期时间:[{playTime.Year}-{playTime.Month}-{playTime.Day}-{playTime.Hour}-{playTime.Minute}-{playTime.Second}]");
                        var playHour = HUtil32.MakeLong((short)RemainDays, (short)RemainHours);
                        var playMinute = HUtil32.MakeLong((short)RemainHours, (short)Math.Ceiling(playSpan.TotalMinutes));
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSOK_SELECTSERVER, playModel, playHour, playMinute, config.ServerNameList.Count);
                    }
                    else
                    {
                        userInfo.PayMode = 0;
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSOK_SELECTSERVER, 0, 0, 0, config.ServerNameList.Count);
                    }
                    var sServerName = GetServerListInfo();
                    SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sServerName));
                    SessionAdd(config, userInfo.Account, userInfo.UserIPaddr, userInfo.SessionID, userInfo.PayCost, false);
                }
                else
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_PASSWD_FAIL, nCode, 0, 0, 0);
                    SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Exception] LoginService.LoginUser");
                _logger.LogError(ex);
            }
        }

        private bool CheckBadAccount(string account)
        {
            return true;
        }

        private void AddCertUser(UserInfo pUser)
        {
            var st = DateTime.Now;
            var nCurrentTime = GetDay(st.Year, st.Month, st.Day);
            var pCert = new CertUser();
            pCert.LoginID = pUser.Account;
            pCert.Addr = pUser.UserIPaddr;
            var remaindId = pUser.dwValidUntil - nCurrentTime;
            var remaindIp = pUser.dwIpValidUntil - nCurrentTime;
            pCert.IDHour = pUser.Seconds;
            pCert.IPHour = pUser.dwIpSeconds;
            pCert.IPDay = (remaindIp + 1);
            pCert.IDDay = (remaindId + 1);
            pUser.nAvailableType = 5;
            if ((pCert.IDHour > 0))
            {
                pUser.nAvailableType = 2;
            }
            if ((pCert.IPHour > 0))
            {
                pUser.nAvailableType = 4;
            }
            if ((pCert.IPDay > 0))
            {
                pUser.nAvailableType = 3;
            }
            if ((pCert.IDDay > 0))
            {
                pUser.nAvailableType = 1;
            }
            pCert.Certification = pUser.nCertification;
            pCert.OpenTime = HUtil32.GetTickCount();
            pCert.AvailableType = pUser.nAvailableType;
            pCert.Closing = false;
            pUser.dwOpenTime = pCert.OpenTime;
            LsShare.CertList.Add(pCert);
        }

        private void DelCertUser(int cert)
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

        private long GetDay(int iYear, int iMonth, int iDay)
        {
            var MONTH_DAY = new[] { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            const int DAYTOYEAR1999 = 730119;
            if (iYear > 0)
            {
                int iTotalDay;
                int iStartYear;
                if (iYear > 1999)
                {
                    iStartYear = 2000;
                    iTotalDay = DAYTOYEAR1999;
                }
                else
                {
                    iStartYear = 1;
                    iTotalDay = 0;
                }

                for (int i = iStartYear; i < iYear; i++)
                {
                    int iTemp;
                    if (i % 400 == 0)
                    {
                        iTemp = 366;
                    }
                    else if (i % 100 == 0)
                    {
                        iTemp = 365;
                    }
                    else if (i % 4 == 0)
                    {
                        iTemp = 366;
                    }
                    else
                    {
                        iTemp = 365;
                    }
                    iTotalDay += iTemp;
                }

                bool bYun;
                if (iYear % 400 == 0)
                {
                    bYun = true;
                }
                else if (iYear % 100 == 0)
                {
                    bYun = false;
                }
                else if (iYear % 4 == 0)
                {
                    bYun = true;
                }
                else
                {
                    bYun = false;
                }

                if (iMonth >= 1 && iMonth <= 12)
                {
                    for (int i = 1; i < iMonth; i++)
                    {
                        iTotalDay += MONTH_DAY[i];
                        if (i == 2 && !bYun)
                        {
                            iTotalDay = iTotalDay - 1;
                        }
                    }
                }

                int MaxDay = MONTH_DAY[iMonth];
                if (iMonth == 2 && !bYun)
                {
                    MaxDay = 28;
                }
                if (iDay >= 1 && iDay <= MaxDay)
                {
                    iTotalDay = iTotalDay + iDay;
                }

                return iTotalDay;
            }

            return 0;
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
                if (string.IsNullOrEmpty(sData) || sData.Length < 333)
                {
                    _logger.Warn("[新建账号失败] 数据包为空或数据包长度异常");
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
                if (LsShare.CheckAccountName(userFullEntry.UserEntry.Account))
                {
                    success = true;
                }
                if (success)
                {
                    var n10 = _accountStorage.Index(userFullEntry.UserEntry.Account);
                    if (n10 <= 0)
                    {
                        var accountRecord = new AccountRecord();
                        accountRecord.UserEntry = userFullEntry.UserEntry;
                        accountRecord.UserEntryAdd = userFullEntry.UserEntryAdd;
                        if (!string.IsNullOrEmpty(userFullEntry.UserEntry.Account))
                        {
                            if (_accountStorage.Add(ref accountRecord))
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
                    _logger.Warn(string.Format(sAddNewuserFail, userFullEntry.UserEntry.Account, userFullEntry.UserEntryAdd.Quiz2));
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
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError("[Exception] LoginsService.AccountCreate");
                _logger.LogError(ex);
            }
            finally
            {
                userInfo.ClientTick = HUtil32.GetTickCount();
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void AccountChangePassword(UserInfo userInfo, string sData)
        {
            var sLoginId = string.Empty;
            var sOldPassword = string.Empty;
            ClientMesaagePacket defMsg;
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
                if (nCode == 1)
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError("[Exception] LoginService.ChangePassword");
                _logger.LogError(ex);
            }
        }

        /// <summary>
        /// 选择服务器
        /// </summary>
        private void AccountSelectServer(Config config, UserInfo userInfo, string sData)
        {
            ClientMesaagePacket defMsg;
            bool boPayCost;
            var sSelGateIp = string.Empty;
            var nSelGatePort = 0;
            const string sSelServerMsg = "Server: {0}/{1}-{2}:{3}";
            var sServerName = EDCode.DeCodeString(sData);
            if (!string.IsNullOrEmpty(userInfo.Account) && !string.IsNullOrEmpty(sServerName) && IsLogin(config, userInfo.SessionID))
            {
                GetSelGateInfo(config, sServerName, config.sGateIPaddr, ref sSelGateIp, ref nSelGatePort);
                if (!string.IsNullOrEmpty(sSelGateIp) && nSelGatePort > 0)
                {
                    if (config.boDynamicIPMode)
                    {
                        sSelGateIp = userInfo.GateIPaddr;
                    }
                    _logger.LogDebug(string.Format(sSelServerMsg, sServerName, config.sGateIPaddr, sSelGateIp, nSelGatePort));
                    userInfo.SelServer = true;
                    boPayCost = false;
                    var nPayMode = 5;
                    if (userInfo.IDHour > 0)
                    {
                        nPayMode = 2;
                    }
                    if (userInfo.IPHour > 0)
                    {
                        nPayMode = 4;
                    }
                    if (userInfo.IPDay > 0)
                    {
                        nPayMode = 3;
                    }
                    if (userInfo.IDDay > 0)
                    {
                        nPayMode = 1;
                    }
                    if (_masSocService.IsNotUserFull(sServerName))
                    {
                        SessionUpdate(config, userInfo.SessionID, sServerName, boPayCost);
                        _masSocService.SendServerMsg(Grobal2.SS_OPENSESSION, sServerName, userInfo.Account + "/" + userInfo.SessionID + "/" + (userInfo.PayCost ? 1 : 0) + "/" + nPayMode + "/" + userInfo.UserIPaddr + "/" + userInfo.Seconds);
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELECTSERVER_OK, userInfo.SessionID, 0, 0, 0);
                        SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sSelGateIp + "/" + nSelGatePort + "/" + userInfo.SessionID));
                    }
                    else
                    {
                        userInfo.SelServer = false;
                        SessionDel(config, userInfo.SessionID);
                        defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_STARTFAIL, 0, 0, 0, 0);
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
            AccountRecord accountRecord = null;
            ClientMesaagePacket defMsg;
            try
            {
                if (string.IsNullOrEmpty(sData))
                {
                    _logger.Warn("[更新账号失败,数据包为空].");
                    return;
                }
                if (string.IsNullOrEmpty(sData) || sData.Length < 333)
                {
                    _logger.Warn("[更新账号失败] 数据包为空或数据包长度异常");
                    return;
                }
                var accountStrSize = (byte)Math.Ceiling((decimal)(UserEntry.Size * 4) / 3);
                var ueBuff = EDCode.DecodeBuffer(sData[..accountStrSize]);
                var uaBuff = EDCode.DecodeBuffer(sData[accountStrSize..]);
                var accountBuff = new byte[ueBuff.Length + uaBuff.Length];
                Buffer.BlockCopy(ueBuff, 0, accountBuff, 0, ueBuff.Length);
                Buffer.BlockCopy(uaBuff, 0, accountBuff, ueBuff.Length, uaBuff.Length);
                var userFullEntry = Packets.ToPacket<UserFullEntry>(accountBuff);
                var nCode = -1;
                if (string.Compare(userInfo.Account, userFullEntry.UserEntry.Account, StringComparison.OrdinalIgnoreCase) == 0 && LsShare.CheckAccountName(userFullEntry.UserEntry.Account))
                {
                    var accountIndex = _accountStorage.Index(userFullEntry.UserEntry.Account);
                    if (accountIndex >= 0)
                    {
                        if (_accountStorage.Get(accountIndex, ref accountRecord) >= 0)
                        {
                            accountRecord.UserEntry = userFullEntry.UserEntry;
                            accountRecord.UserEntryAdd = userFullEntry.UserEntryAdd;
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
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError("[Exception] LoginService.UpdateUserInfo");
                _logger.LogError(ex);
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
            ClientMesaagePacket defMsg;
            AccountRecord accountRecord = null;
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
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GETBACKPASSWD_SUCCESS, 0, 0, 0, 0);
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sPassword));
            }
            else
            {
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_GETBACKPASSWD_FAIL, nCode, 0, 0, 0);
                SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
            }
        }

        private void AccountCheckProtocol(UserInfo userInfo, int nDate)
        {
            ClientMesaagePacket defMsg;
            if (nDate < LsShare.VersionDate)
            {
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CERTIFICATION_FAIL, 0, 0, 0, 0);
            }
            else
            {
                defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CERTIFICATION_SUCCESS, 0, 0, 0, 0);
            }
            SendGateMsg(userInfo.Socket, userInfo.SockIndex, EDCode.EncodeMessage(defMsg));
        }

        private bool IsLogin(Config config, int nSessionId)
        {
            var result = false;
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                if (config.SessionList[i].SessionID == nSessionId)
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
                if (config.SessionList[i].Account == sLoginId)
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
                if (connInfo.Account == sLoginId && !connInfo.boKicked)
                {
                    _masSocService.SendServerMsg(Grobal2.SS_CLOSESESSION, connInfo.ServerName, connInfo.Account + "/" + connInfo.SessionID);
                    connInfo.dwKickTick = HUtil32.GetTickCount();
                    connInfo.boKicked = true;
                }
            }
        }

        private void SessionUpdate(Config config, int nSessionId, string sServerName, bool boPayCost)
        {
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                var connInfo = config.SessionList[i];
                if (connInfo.SessionID == nSessionId)
                {
                    connInfo.ServerName = sServerName;
                    connInfo.bo11 = boPayCost;
                    break;
                }
            }
        }

        private void SessionAdd(Config config, string sAccount, string sIPaddr, int nSessionId, bool boPayCost, bool bo11)
        {
            var connInfo = new TConnInfo();
            connInfo.Account = sAccount;
            connInfo.IPaddr = sIPaddr;
            connInfo.SessionID = nSessionId;
            connInfo.boPayCost = boPayCost;
            connInfo.bo11 = bo11;
            connInfo.dwKickTick = HUtil32.GetTickCount();
            connInfo.dwStartTick = HUtil32.GetTickCount();
            connInfo.boKicked = false;
            config.SessionList.Add(connInfo);
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

        private void KickUser(Config config, ref UserInfo userInfo)
        {
            const string sKickMsg = "Kick: {0}";
            for (var i = 0; i < LsShare.Gates.Count; i++)
            {
                var gateInfo = LsShare.Gates[i];
                for (var j = 0; j < gateInfo.UserList.Count; j++)
                {
                    var user = gateInfo.UserList[j];
                    if (user == userInfo)
                    {
                        _logger.LogDebug(string.Format(sKickMsg, userInfo.UserIPaddr));
                        SendGateKickMsg(gateInfo.Socket, userInfo.SockIndex);
                        userInfo = null;
                        gateInfo.UserList.RemoveAt(j);
                        return;
                    }
                }
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
                _logger.LogError("[Exception] LoginService.GetSelGateInfo");
                _logger.LogError(ex);
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
            catch(Exception ex)
            {
                _logger.LogError("[Exception] LoginService.GetServerListInfo");
                _logger.LogError(ex);
            }
            return result;
        }

        private void SendGateKickMsg(Socket socket, string sSockIndex)
        {
            var sSendMsg = $"%+-{sSockIndex}$";
            socket.SendText(sSendMsg);
        }

        private void SessionDel(Config config, int nSessionId)
        {
            for (var i = 0; i < config.SessionList.Count; i++)
            {
                if (config.SessionList[i].SessionID == nSessionId)
                {
                    config.SessionList[i] = null;
                    config.SessionList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}