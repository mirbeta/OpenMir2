using System;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace LoginSvr
{
    public class LoginService
    {
        private readonly AccountDB _accountDB = null;
        private readonly MasSocService _masSock;

        public LoginService(AccountDB accountDB, MasSocService masSock)
        {
            _accountDB = accountDB;
            _masSock = masSock;
        }

        public void LoadAddrTable(TConfig Config)
        {
            int nRouteIdx;
            int nSelGateIdx;
            string sLineText = string.Empty;
            string sTitle = string.Empty;
            string sServerName = string.Empty;
            string sGate = string.Empty;
            string sRemote = string.Empty;
            string sPublic = string.Empty;
            string sGatePort = string.Empty;
            string sFileName = "!AddrTable.txt";
            StringList LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                LoadList.LoadFromFile(sFileName);
                nRouteIdx = 0;
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if ((sLineText != "") && (sLineText[0] != ';'))
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sServerName, new string[] { " " });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sTitle, new string[] { " " });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sRemote, new string[] { " " });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sPublic, new string[] { " " });
                        sLineText = sLineText.Trim();
                        if ((sTitle != "") && (sRemote != "") && (sPublic != "") && (nRouteIdx < 60))
                        {
                            Config.GateRoute[nRouteIdx] = new TGateRoute();
                            Config.GateRoute[nRouteIdx].sServerName = sServerName;
                            Config.GateRoute[nRouteIdx].sTitle = sTitle;
                            Config.GateRoute[nRouteIdx].sRemoteAddr = sRemote;
                            Config.GateRoute[nRouteIdx].sPublicAddr = sPublic;
                            nSelGateIdx = 0;
                            while ((sLineText != ""))
                            {
                                if (nSelGateIdx > 9)
                                {
                                    break;
                                }
                                sLineText = HUtil32.GetValidStr3(sLineText, ref sGate, new string[] { " " });
                                if (sGate != "")
                                {
                                    if (sGate[0] == '*')
                                    {
                                        sGate = sGate.Substring(1, sGate.Length - 1);
                                        Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].boEnable = false;
                                    }
                                    else
                                    {
                                        Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].boEnable = true;
                                    }
                                    sGatePort = HUtil32.GetValidStr3(sGate, ref sGate, new string[] { ":" });
                                    Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].sIPaddr = sGate;
                                    Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].nPort = HUtil32.Str_ToInt(sGatePort, 0);
                                    Config.GateRoute[nRouteIdx].nSelIdx = 0;
                                    nSelGateIdx++;
                                }
                                sLineText = sLineText.Trim();
                            }
                            nRouteIdx++;
                        }
                    }
                }
                Config.nRouteCount = nRouteIdx;
            }
            LoadList = null;
            GenServerNameList(Config);
        }

        /// <summary>
        /// 是否付费账号
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="sIPaddr"></param>
        /// <param name="sAccount"></param>
        /// <returns></returns>
        public bool IsPayMent(TConfig Config, string sIPaddr, string sAccount)
        {
            bool result = false;
            if ((Config.AccountCostList.ContainsKey(sAccount)) || (Config.IPaddrCostList.ContainsKey(sIPaddr)))
            {
                result = true;
            }
            return result;
        }

        public void CloseUser(TConfig Config, string sAccount, int nSessionID)
        {
            TConnInfo ConnInfo;
            for (var i = Config.SessionList.Count - 1; i >= 0; i--)
            {
                ConnInfo = Config.SessionList[i];
                if ((ConnInfo.sAccount == sAccount) || (ConnInfo.nSessionID == nSessionID))
                {
                    _masSock.SendServerMsg(Grobal2.SS_CLOSESESSION, ConnInfo.sServerName, ConnInfo.sAccount + "/" + (ConnInfo.nSessionID).ToString());
                    ConnInfo = null;
                    Config.SessionList.RemoveAt(i);
                }
            }
        }

        public void ProcessGate(TConfig Config)
        {
            int I;
            int II;
            TGateInfo GateInfo;
            TUserInfo UserInfo;
            //EnterCriticalSection(Config.GateCriticalSection);
            try
            {
                Config.dwProcessGateTick = HUtil32.GetTickCount();
                I = 0;
                while (true)
                {
                    if (Config.GateList.Count <= I)
                    {
                        break;
                    }
                    GateInfo = Config.GateList[I];
                    if (GateInfo.sReceiveMsg != "" && GateInfo.UserList != null)
                    {
                        DecodeGateData(Config, GateInfo);
                        Config.sGateIPaddr = GateInfo.sIPaddr;
                        II = 0;
                        while (true)
                        {
                            if (GateInfo.UserList.Count <= II)
                            {
                                break;
                            }
                            UserInfo = GateInfo.UserList[II];
                            if (UserInfo.sReceiveMsg != "")
                            {
                                DecodeUserData(Config, UserInfo);
                            }
                            II++;
                        }
                    }
                    I++;
                }
                if (Config.dwProcessGateTime < Config.dwProcessGateTick)
                {
                    Config.dwProcessGateTime = HUtil32.GetTickCount() - Config.dwProcessGateTick;
                }
                if (Config.dwProcessGateTime > 100)
                {
                    Config.dwProcessGateTime -= 100;
                }
            }
            finally
            {
                // LeaveCriticalSection(Config.GateCriticalSection);
            }
        }

        private void DecodeGateData(TConfig Config, TGateInfo GateInfo)
        {
            int nCount;
            string sMsg = string.Empty;
            string sSockIndex = string.Empty;
            string sData;
            char Code;
            try
            {
                nCount = 0;
                while (true)
                {
                    if (HUtil32.TagCount(GateInfo.sReceiveMsg, '$') <= 0)
                    {
                        break;
                    }
                    GateInfo.sReceiveMsg = HUtil32.ArrestStringEx(GateInfo.sReceiveMsg, "%", "$", ref sMsg);
                    if (sMsg != "")
                    {
                        Code = sMsg[0];
                        sMsg = sMsg.Substring(1, sMsg.Length - 1);
                        switch (Code)
                        {
                            case '-':
                                SendKeepAlivePacket(GateInfo.Socket);
                                GateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
                                break;
                            case 'A':
                                sData = HUtil32.GetValidStr3(sMsg, ref sSockIndex, new string[] { "/" });
                                ReceiveSendUser(Config, sSockIndex, GateInfo, sData);
                                break;
                            case 'O':
                                sData = HUtil32.GetValidStr3(sMsg, ref sSockIndex, new string[] { "/" });
                                ReceiveOpenUser(Config, sSockIndex, sData, GateInfo);
                                break;
                            case 'X':
                                sSockIndex = sMsg;
                                ReceiveCloseUser(Config, sSockIndex, GateInfo);
                                break;
                        }
                    }
                    else
                    {
                        if (nCount >= 1)
                        {
                            GateInfo.sReceiveMsg = "";
                        }
                        nCount++;
                    }
                }
            }
            catch
            {
                LSShare.MainOutMessage("[Exception] TFrmMain.DecodeGateData");
            }
        }

        private void SendKeepAlivePacket(Socket Socket)
        {
            if (Socket.Connected)
            {
                Socket.SendText("%++$");
            }
        }

        private void ReceiveCloseUser(TConfig Config, string sSockIndex, TGateInfo GateInfo)
        {
            TUserInfo UserInfo;
            const string sCloseMsg = "Close: {0}";
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                UserInfo = GateInfo.UserList[i];
                if (UserInfo.sSockIndex == sSockIndex)
                {
                    if (Config.boShowDetailMsg)
                    {
                        LSShare.MainOutMessage(string.Format(sCloseMsg, UserInfo.sUserIPaddr));
                    }
                    if (!UserInfo.boSelServer)
                    {
                        SessionDel(Config, UserInfo.nSessionID);
                    }
                    UserInfo = null;
                    GateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ReceiveOpenUser(TConfig Config, string sSockIndex, string sIPaddr, TGateInfo GateInfo)
        {
            TUserInfo UserInfo;
            string sUserIPaddr = string.Empty;
            const string sOpenMsg = "Open: {0}/{1}";
            var sGateIPaddr = HUtil32.GetValidStr3(sIPaddr, ref sUserIPaddr, new string[] { "/" });
            try
            {
                for (var i = 0; i < GateInfo.UserList.Count; i++)
                {
                    UserInfo = GateInfo.UserList[i];
                    if (UserInfo.sSockIndex == sSockIndex)
                    {
                        UserInfo.sUserIPaddr = sUserIPaddr;
                        UserInfo.sGateIPaddr = sGateIPaddr;
                        UserInfo.sAccount = "";
                        UserInfo.nSessionID = 0;
                        UserInfo.sReceiveMsg = "";
                        UserInfo.dwTime5C = HUtil32.GetTickCount();
                        UserInfo.dwClientTick = HUtil32.GetTickCount();
                        return;
                    }
                }
                UserInfo = new TUserInfo();
                UserInfo.sAccount = "";
                UserInfo.sUserIPaddr = sUserIPaddr;
                UserInfo.sGateIPaddr = sGateIPaddr;
                UserInfo.sSockIndex = sSockIndex;
                UserInfo.nVersionDate = 0;
                UserInfo.boCertificationOK = false;
                UserInfo.nSessionID = 0;
                UserInfo.bo51 = false;
                UserInfo.Socket = GateInfo.Socket;
                UserInfo.sReceiveMsg = "";
                UserInfo.dwTime5C = HUtil32.GetTickCount();
                UserInfo.dwClientTick = HUtil32.GetTickCount();
                UserInfo.bo60 = false;
                UserInfo.Gate = GateInfo;
                GateInfo.UserList.Add(UserInfo);
                if (Config.boShowDetailMsg)
                {
                    LSShare.MainOutMessage(string.Format(sOpenMsg, sUserIPaddr, sGateIPaddr));
                }
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMain.ReceiveOpenUser");
            }
        }

        private void ReceiveSendUser(TConfig Config, string sSockIndex, TGateInfo GateInfo, string sData)
        {
            TUserInfo UserInfo;
            try
            {
                for (var i = 0; i < GateInfo.UserList.Count; i++)
                {
                    UserInfo = GateInfo.UserList[i];
                    if (UserInfo.sSockIndex == sSockIndex)
                    {
                        if (UserInfo.sReceiveMsg.Length < 4069)
                        {
                            UserInfo.sReceiveMsg = UserInfo.sReceiveMsg + sData;
                        }
                        break;
                    }
                }
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMain.ReceiveSendUser");
            }
        }

        public void SessionClearKick(TConfig Config)
        {
            TConnInfo ConnInfo;
            for (var i = Config.SessionList.Count - 1; i >= 0; i--)
            {
                ConnInfo = Config.SessionList[i];
                if (ConnInfo.boKicked && ((HUtil32.GetTickCount() - ConnInfo.dwKickTick) > 5 * 1000))
                {
                    ConnInfo = null;
                    Config.SessionList.RemoveAt(i);
                }
            }
        }

        private void DecodeUserData(TConfig Config, TUserInfo UserInfo)
        {
            string sMsg = string.Empty;
            var nCount = 0;
            try
            {
                while (true)
                {
                    if (HUtil32.TagCount(UserInfo.sReceiveMsg, '!') <= 0)
                    {
                        break;
                    }
                    UserInfo.sReceiveMsg = HUtil32.ArrestStringEx(UserInfo.sReceiveMsg, "#", "!", ref sMsg);
                    if (sMsg != "")
                    {
                        if (sMsg.Length >= Grobal2.DEFBLOCKSIZE + 1)
                        {
                            sMsg = sMsg.Substring(2 - 1, sMsg.Length - 1);
                            ProcessUserMsg(Config, UserInfo, sMsg);
                        }
                    }
                    else
                    {
                        if (nCount >= 1)
                        {
                            UserInfo.sReceiveMsg = "";
                        }
                        nCount++;
                    }
                    if (UserInfo.sReceiveMsg == "")
                    {
                        break;
                    }
                }
            }
            catch
            {
                LSShare.MainOutMessage("[Exception] TFrmMain.DecodeUserData ");
            }
        }

        private void SessionDel(TConfig Config, int nSessionID)
        {
            TConnInfo ConnInfo;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                ConnInfo = Config.SessionList[i];
                if (ConnInfo.nSessionID == nSessionID)
                {
                    ConnInfo = null;
                    Config.SessionList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ProcessUserMsg(TConfig Config, TUserInfo UserInfo, string sMsg)
        {
            string sDefMsg = sMsg.Substring(0, Grobal2.DEFBLOCKSIZE);
            string sData = sMsg.Substring(Grobal2.DEFBLOCKSIZE, sMsg.Length - Grobal2.DEFBLOCKSIZE);
            TDefaultMessage DefMsg = EDcode.DecodeMessage(sDefMsg);
            switch (DefMsg.Ident)
            {
                case Grobal2.CM_SELECTSERVER:
                    if (!UserInfo.boSelServer)
                    {
                        AccountSelectServer(Config, UserInfo, sData);
                    }
                    break;
                case Grobal2.CM_PROTOCOL:
                    AccountCheckProtocol(UserInfo, DefMsg.Recog);
                    break;
                case Grobal2.CM_IDPASSWORD:
                    if (UserInfo.sAccount == "")
                    {
                        AccountLogin(Config, UserInfo, sData);
                    }
                    else
                    {
                        KickUser(Config, UserInfo);
                    }
                    break;
                case Grobal2.CM_ADDNEWUSER:
                    if (Config.boEnableMakingID)
                    {
                        if ((HUtil32.GetTickCount() - UserInfo.dwClientTick) > 5000)
                        {
                            UserInfo.dwClientTick = HUtil32.GetTickCount();
                            AccountCreate(Config, UserInfo, sData);
                        }
                        else
                        {
                            LSShare.MainOutMessage("[超速操作] 创建帐号 /" + UserInfo.sUserIPaddr);
                        }
                    }
                    break;
                case Grobal2.CM_CHANGEPASSWORD:
                    if (UserInfo.sAccount == "")
                    {
                        if ((HUtil32.GetTickCount() - UserInfo.dwClientTick) > 5000)
                        {
                            UserInfo.dwClientTick = HUtil32.GetTickCount();
                            AccountChangePassword(Config, UserInfo, sData);
                        }
                        else
                        {
                            LSShare.MainOutMessage("[超速操作] 修改密码 /" + UserInfo.sUserIPaddr);
                        }
                    }
                    else
                    {
                        UserInfo.sAccount = "";
                    }
                    break;
                case Grobal2.CM_UPDATEUSER:
                    if ((HUtil32.GetTickCount() - UserInfo.dwClientTick) > 5000)
                    {
                        UserInfo.dwClientTick = HUtil32.GetTickCount();
                        AccountUpdateUserInfo(Config, UserInfo, sData);
                    }
                    else
                    {
                        LSShare.MainOutMessage("[超速操作] 更新帐号 /" + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_GETBACKPASSWORD:
                    if ((HUtil32.GetTickCount() - UserInfo.dwClientTick) > 5000)
                    {
                        UserInfo.dwClientTick = HUtil32.GetTickCount();
                        AccountGetBackPassword(UserInfo, sData);
                    }
                    else
                    {
                        LSShare.MainOutMessage("[超速操作] 找回密码 /" + UserInfo.sUserIPaddr);
                    }
                    break;
            }
        }

        private void AccountCreate(TConfig Config, TUserInfo UserInfo, string sData)
        {
            TUserEntry UserEntry = null;
            TUserEntryAdd UserAddEntry = null;
            TAccountDBRecord DBRecord = null;
            int nLen = 0;
            string sUserEntryMsg;
            string sUserAddEntryMsg;
            int nErrCode;
            TDefaultMessage DefMsg;
            bool bo21;
            int n10;
            const string sAddNewuserFail = "[新建帐号失败] {0}/{1}";
            const string sLogFlag = "new";
            try
            {
                nErrCode = -1;
                nLen = 198;
                bo21 = false;
                sUserEntryMsg = sData.Substring(0, nLen);
                sUserAddEntryMsg = sData.Substring(nLen, sData.Length - nLen);
                if ((sUserEntryMsg != "") && (sUserAddEntryMsg != ""))
                {
                    UserEntry = new TUserEntry(EDcode.DecodeBuffer(sUserEntryMsg, sUserEntryMsg.Length));
                    UserAddEntry = new TUserEntryAdd(EDcode.DecodeBuffer(sUserAddEntryMsg, sUserAddEntryMsg.Length));
                    if (LSShare.CheckAccountName(UserEntry.sAccount))
                    {
                        bo21 = true;
                    }
                    if (bo21)
                    {
                        try
                        {
                            if (_accountDB.Open())
                            {
                                n10 = _accountDB.Index(UserEntry.sAccount);
                                if (n10 <= 0)
                                {
                                    DBRecord = new TAccountDBRecord();
                                    DBRecord.UserEntry = UserEntry;
                                    DBRecord.UserEntryAdd = UserAddEntry;
                                    if (UserEntry.sAccount != "")
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
                        }
                        finally
                        {
                            _accountDB.Close();
                        }
                    }
                    else
                    {
                        LSShare.MainOutMessage(string.Format(sAddNewuserFail, UserEntry.sAccount, UserAddEntry.sQuiz2));
                    }
                }
                if (nErrCode == 1)
                {
                    WriteLogMsg(Config, sLogFlag, ref UserEntry, ref UserAddEntry);
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
                LSShare.MainOutMessage("TFrmMain.AddNewUser");
            }
        }

        private void AccountChangePassword(TConfig Config, TUserInfo UserInfo, string sData)
        {
            string sLoginID = string.Empty;
            string sOldPassword = string.Empty;
            TDefaultMessage DefMsg;
            int n10;
            TAccountDBRecord DBRecord = null;
            const string sChgMsg = "chg";
            try
            {
                string sMsg = EDcode.DeCodeString(sData);
                sMsg = HUtil32.GetValidStr3(sMsg, ref sLoginID, new string[] { "\09" });
                string sNewPassword = HUtil32.GetValidStr3(sMsg, ref sOldPassword, new string[] { "\09" });
                int nCode = 0;
                try
                {
                    if (_accountDB.Open() && (sNewPassword.Length >= 3))
                    {
                        n10 = _accountDB.Index(sLoginID);
                        if ((n10 >= 0) && (_accountDB.Get(n10, ref DBRecord) >= 0))
                        {
                            if ((DBRecord.nErrorCount < 5) || ((HUtil32.GetTickCount() - DBRecord.dwActionTick) > 180000))
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
                }
                finally
                {
                    _accountDB.Close();
                }
                if (nCode == 1)
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_SUCCESS, 0, 0, 0, 0);
                    WriteLogMsg(Config, sChgMsg, ref DBRecord.UserEntry, ref DBRecord.UserEntryAdd);
                }
                else
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_CHGPASSWD_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMain.ChangePassword");
            }
        }

        public void AccountCheckProtocol(TUserInfo UserInfo, int nDate)
        {
            TDefaultMessage DefMsg;
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

        public bool KickUser(TConfig Config, TUserInfo UserInfo)
        {
            TGateInfo GateInfo;
            TUserInfo User;
            const string sKickMsg = "Kick: {0}";
            var result = false;
            for (var i = 0; i < Config.GateList.Count; i++)
            {
                GateInfo = Config.GateList[i];
                for (var j = 0; j < GateInfo.UserList.Count; j++)
                {
                    User = GateInfo.UserList[j];
                    if (User == UserInfo)
                    {
                        if (Config.boShowDetailMsg)
                        {
                            LSShare.MainOutMessage(string.Format(sKickMsg, UserInfo.sUserIPaddr));
                        }
                        SendGateKickMsg(GateInfo.Socket, UserInfo.sSockIndex);
                        UserInfo = null;
                        GateInfo.UserList.RemoveAt(j);
                        result = true;
                        return result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 账号登陆
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="UserInfo"></param>
        /// <param name="sData"></param>
        private void AccountLogin(TConfig Config, TUserInfo UserInfo, string sData)
        {
            string sLoginID = string.Empty;
            string sPassword;
            int nCode;
            bool boNeedUpdate;
            TDefaultMessage DefMsg;
            TUserEntry UserEntry = null;
            int nIDCost;
            int nIPCost;
            int nIDCostIndex = -1;
            int nIPCostIndex = -1;
            TAccountDBRecord DBRecord = null;
            int n10;
            bool boPayCost;
            string sServerName;
            try
            {
                sPassword = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sLoginID, new string[] { "/" });
                nCode = 0;
                boNeedUpdate = false;
                try
                {
                    if (_accountDB.Open())
                    {
                        n10 = _accountDB.Index(sLoginID);
                        if ((n10 >= 0) && (_accountDB.Get(n10, ref DBRecord) >= 0))
                        {
                            if ((DBRecord.nErrorCount < 5) || ((HUtil32.GetTickCount() - DBRecord.dwActionTick) > 60000))
                            {
                                if (DBRecord.UserEntry.sPassword == sPassword)
                                {
                                    DBRecord.nErrorCount = 0;
                                    if ((DBRecord.UserEntry.sUserName == "") || (DBRecord.UserEntryAdd.sQuiz2 == ""))
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
                    }
                }
                finally
                {
                    _accountDB.Close();
                }
                if ((nCode == 1) && IsLogin(Config, sLoginID))
                {
                    SessionKick(Config, sLoginID);
                    nCode = -3;
                }
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
                    nIDCost = 0;
                    nIPCost = 0;
                    boPayCost = false;
                    if (nIDCostIndex >= 0)
                    {
                        nIDCost = nIDCostIndex;//Config.AccountCostList[nIDCostIndex];
                    }
                    if (nIPCostIndex >= 0)
                    {
                        nIPCost = nIPCostIndex;//Config.IPaddrCostList[nIPCostIndex];
                        boPayCost = true;
                    }
                    if ((nIDCost >= 0) || (nIPCost >= 0))
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
                    sServerName = GetServerListInfo();
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
                LSShare.MainOutMessage("TFrmMain.LoginUser");
            }
        }

        /// <summary>
        /// 获取角色网关信息
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="sServerName"></param>
        /// <param name="sIPaddr"></param>
        /// <param name="sSelGateIP"></param>
        /// <param name="nSelGatePort"></param>
        public void GetSelGateInfo(TConfig Config, string sServerName, string sIPaddr, ref string sSelGateIP, ref int nSelGatePort)
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
                    if (Config.boDynamicIPMode || ((Config.GateRoute[i].sServerName == sServerName) && (Config.GateRoute[i].sPublicAddr == sIPaddr)))
                    {
                        nGateCount = 0;
                        nGateIdx = 0;
                        while (true)
                        {
                            if ((Config.GateRoute[i].Gate[nGateIdx].sIPaddr != "") && (Config.GateRoute[i].Gate[nGateIdx].boEnable))
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
                            if ((Config.GateRoute[i].Gate[nGateIdx].sIPaddr != "") && (Config.GateRoute[i].Gate[nGateIdx].boEnable))
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
                                if ((Config.GateRoute[i].Gate[nGateIdx].sIPaddr != "") && (Config.GateRoute[i].Gate[nGateIdx].boEnable))
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
            catch
            {
                LSShare.MainOutMessage("TFrmMain.GetSelGateInfo");
            }
        }

        private string GetServerListInfo()
        {
            var result = string.Empty;
            var sServerInfo = string.Empty;
            var Config = LSShare.g_Config;
            try
            {
                for (var i = 0; i < Config.ServerNameList.Count; i++)
                {
                    string sServerName = Config.ServerNameList[i];
                    if (sServerName != "")
                    {
                        sServerInfo = sServerInfo + sServerName + "/" + _masSock.ServerStatus(sServerName) + "/";
                    }
                }
                result = sServerInfo;
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMain.GetServerListInfo");
            }
            return result;
        }

        /// <summary>
        /// 选择服务器
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="UserInfo"></param>
        /// <param name="sData"></param>
        public void AccountSelectServer(TConfig Config, TUserInfo UserInfo, string sData)
        {
            TDefaultMessage DefMsg;
            bool boPayCost;
            int nPayMode;
            string sSelGateIP = string.Empty;
            int nSelGatePort = 0;
            const string sSelServerMsg = "Server: {0}/{1}-{2}:{3}";
            var sServerName = EDcode.DeCodeString(sData);
            if ((UserInfo.sAccount != "") && (sServerName != "") && IsLogin(Config, UserInfo.nSessionID))
            {
                GetSelGateInfo(Config, sServerName, Config.sGateIPaddr, ref sSelGateIP, ref nSelGatePort);
                if ((sSelGateIP != "") && (nSelGatePort > 0))
                {
                    if (Config.boDynamicIPMode)
                    {
                        sSelGateIP = UserInfo.sGateIPaddr;
                    }
                    if (Config.boShowDetailMsg)
                    {
                        LSShare.MainOutMessage(string.Format(sSelServerMsg, sServerName, Config.sGateIPaddr, sSelGateIP, nSelGatePort));
                    }
                    UserInfo.boSelServer = true;
                    boPayCost = false;
                    nPayMode = 5;
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
                        _masSock.SendServerMsg(Grobal2.SS_OPENSESSION, sServerName, UserInfo.sAccount + "/" + (UserInfo.nSessionID).ToString() + "/" + ((UserInfo.boPayCost == true ? 1 : 0)).ToString() + "/" + (nPayMode).ToString() + "/" + UserInfo.sUserIPaddr);
                        DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SELECTSERVER_OK, UserInfo.nSessionID, 0, 0, 0);
                        SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sSelGateIP + "/" + (nSelGatePort).ToString() + "/" + (UserInfo.nSessionID).ToString()));
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

        private void AccountUpdateUserInfo(TConfig Config, TUserInfo UserInfo, string sData)
        {
            TUserEntry UserEntry = null;
            TUserEntryAdd UserAddEntry = null;
            TAccountDBRecord DBRecord = null;
            int nLen = 0;
            string sUserEntryMsg;
            string sUserAddEntryMsg;
            TDefaultMessage DefMsg;
            try
            {
                //nLen = LSShare.GetCodeMsgSize(sizeof(TUserEntry) * 4 / 3);
                sUserEntryMsg = sData.Substring(01, nLen);
                sUserAddEntryMsg = sData.Substring(nLen, sData.Length - nLen);
                UserEntry = new TUserEntry(EDcode.DecodeBuffer(sUserEntryMsg));
                UserAddEntry = new TUserEntryAdd(EDcode.DecodeBuffer(sUserAddEntryMsg));
                int nCode = -1;
                if ((UserInfo.sAccount == UserEntry.sAccount) && LSShare.CheckAccountName(UserEntry.sAccount))
                {
                    try
                    {
                        if (_accountDB.Open())
                        {
                            int n10 = _accountDB.Index(UserEntry.sAccount);
                            if ((n10 >= 0))
                            {
                                if ((_accountDB.Get(n10, ref DBRecord) >= 0))
                                {
                                    DBRecord.UserEntry = UserEntry;
                                    DBRecord.UserEntryAdd = UserAddEntry;
                                    _accountDB.Update(n10, ref DBRecord);
                                    nCode = 1;
                                }
                            }
                            else
                            {
                                nCode = 0;
                            }
                        }
                    }
                    finally
                    {
                        _accountDB.Close();
                    }
                }
                if (nCode == 1)
                {
                    WriteLogMsg(Config, "upg", ref UserEntry, ref UserAddEntry);
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_SUCCESS, 0, 0, 0, 0);
                }
                else
                {
                    DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEID_FAIL, nCode, 0, 0, 0);
                }
                SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMain.UpdateUserInfo");
            }
        }

        public void AccountGetBackPassword(TUserInfo UserInfo, string sData)
        {
            string sAccount = string.Empty;
            string sQuest1 = string.Empty;
            string sAnswer1 = string.Empty;
            string sQuest2 = string.Empty;
            string sAnswer2 = string.Empty;
            string sPassword = string.Empty;
            string sBirthDay = string.Empty;
            int nIndex;
            TDefaultMessage DefMsg;
            TAccountDBRecord DBRecord = null;
            string sMsg = EDcode.DeCodeString(sData);
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new string[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest1, new string[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer1, new string[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sQuest2, new string[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sAnswer2, new string[] { "\09" });
            sMsg = HUtil32.GetValidStr3(sMsg, ref sBirthDay, new string[] { "\09" });
            int nCode = 0;
            try
            {
                if ((sAccount != "") && _accountDB.Open())
                {
                    nIndex = _accountDB.Index(sAccount);
                    if ((nIndex >= 0) && (_accountDB.Get(nIndex, ref DBRecord) >= 0))
                    {
                        if ((DBRecord.nErrorCount < 5) || ((HUtil32.GetTickCount() - DBRecord.dwActionTick) > 180000))
                        {
                            nCode = -1;
                            if ((DBRecord.UserEntry.sQuiz == sQuest1))
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
                                if ((DBRecord.UserEntryAdd.sQuiz2 == sQuest2))
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
            }
            finally
            {
                _accountDB.Close();
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

        public void SendGateMsg(Socket Socket, string sSockIndex, string sMsg)
        {
            var sSendMsg = "%" + sSockIndex + "/#" + sMsg + "!$";
            Socket.SendText(sSendMsg);
        }

        public bool IsLogin(TConfig Config, int nSessionID)
        {
            bool result = false;
            TConnInfo ConnInfo;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                ConnInfo = Config.SessionList[i];
                if ((ConnInfo.nSessionID == nSessionID))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool IsLogin(TConfig Config, string sLoginID)
        {
            bool result = false;
            TConnInfo ConnInfo;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                ConnInfo = Config.SessionList[i];
                if ((ConnInfo.sAccount == sLoginID))
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
        /// <param name="Config"></param>
        /// <param name="sLoginID"></param>
        public void SessionKick(TConfig Config, string sLoginID)
        {
            TConnInfo ConnInfo;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                ConnInfo = Config.SessionList[i];
                if ((ConnInfo.sAccount == sLoginID) && !ConnInfo.boKicked)
                {
                    _masSock.SendServerMsg(Grobal2.SS_CLOSESESSION, ConnInfo.sServerName, ConnInfo.sAccount + "/" + ConnInfo.nSessionID);
                    ConnInfo.dwKickTick = HUtil32.GetTickCount();
                    ConnInfo.boKicked = true;
                }
            }
        }

        public void SessionAdd(TConfig Config, string sAccount, string sIPaddr, int nSessionID, bool boPayCost, bool bo11)
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

        public void SendGateKickMsg(Socket Socket, string sSockIndex)
        {
            var sSendMsg = "%+-" + sSockIndex + "$";
            Socket.SendText(sSendMsg);
        }

        public void SessionUpdate(TConfig Config, int nSessionID, string sServerName, bool boPayCost)
        {
            TConnInfo ConnInfo;
            for (var i = 0; i < Config.SessionList.Count; i++)
            {
                ConnInfo = Config.SessionList[i];
                if ((ConnInfo.nSessionID == nSessionID))
                {
                    ConnInfo.sServerName = sServerName;
                    ConnInfo.bo11 = boPayCost;
                    break;
                }
            }
        }

        public void GenServerNameList(TConfig Config)
        {
            bool boD;
            try
            {
                Config.ServerNameList.Clear();
                for (var i = 0; i < Config.nRouteCount; i++)
                {
                    boD = true;
                    for (var j = 0; j < Config.ServerNameList.Count; j++)
                    {
                        if (Config.ServerNameList[j] == Config.GateRoute[i].sServerName)
                        {
                            boD = false;
                        }
                    }
                    if (boD)
                    {
                        Config.ServerNameList.Add(Config.GateRoute[i].sServerName);
                    }
                }
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMain.GenServerNameList");
            }
        }

        public void SessionClearNoPayMent(TConfig Config)
        {
            TConnInfo ConnInfo;
            for (var i = Config.SessionList.Count - 1; i >= 0; i--)
            {
                ConnInfo = Config.SessionList[i];
                if (!ConnInfo.boKicked && !Config.boTestServer && !ConnInfo.bo11)
                {
                    if ((HUtil32.GetTickCount() - ConnInfo.dwStartTick) > 60 * 60 * 1000)
                    {
                        ConnInfo.dwStartTick = HUtil32.GetTickCount();
                        if (!IsPayMent(Config, ConnInfo.sIPaddr, ConnInfo.sAccount))
                        {
                            _masSock.SendServerMsg(Grobal2.SS_KICKUSER, ConnInfo.sServerName, ConnInfo.sAccount + "/" + ConnInfo.nSessionID);
                            ConnInfo = null;
                            Config.SessionList.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public void LoadIPaddrCostList(TConfig Config, AccountConst accountConst)
        {
            Config.IPaddrCostList.Clear();
            Config.IPaddrCostList.Add(accountConst.s1C, accountConst.nC);
        }

        public void LoadAccountCostList(TConfig Config, AccountConst accountConst)
        {
            Config.AccountCostList.Clear();
            Config.AccountCostList.Add(accountConst.s1C, accountConst.nC);
        }

        public void SaveContLogMsg(TConfig Config, string sLogMsg)
        {
            // int Year = 0;
            // int Month = 0;
            // int Day = 0;
            // int Hour = 0;
            // int Min = 0;
            // int Sec = 0;
            // int MSec = 0;
            // string sLogDir;
            // string sLogFileName;
            // System.IO.FileInfo LogFile;
            // if (sLogMsg == "")
            // {
            //     return;
            // }
            // Year = DateTime.Today.Year;
            // Month = DateTime.Today.Month;
            // Day = DateTime.Today.Day;
            // Hour = DateTime.Now.Hour;
            // Min = DateTime.Now.Minute;
            // Sec = DateTime.Now.Second;
            // MSec = DateTime.Now.Millisecond;
            // if (!Directory.Exists(Config.sCountLogDir))
            // {
            //     Directory.CreateDirectory(Config.sCountLogDir);
            // }
            // sLogDir = Config.sCountLogDir + (Year).ToString() + "-" + HUtil32.IntToStr2(Month);
            // if (!Directory.Exists(sLogDir))
            // {
            //     CreateDirectory((sLogDir as string), null);
            // }
            // sLogFileName = sLogDir + "\\" + (Year).ToString() + "-" + HUtil32.IntToStr2(Month) + "-" + HUtil32.IntToStr2(Day) + ".txt";
            // LogFile = new FileInfo(sLogFileName);
            // if (!File.Exists(sLogFileName))
            // {
            //     StreamWriter _W_0 = LogFile.CreateText();
            // }
            // else
            // {
            //     _W_0 = LogFile.AppendText();
            // }
            // sLogMsg = sLogMsg + "\09" + DateTime.Now.ToString();
            // _W_0.WriteLine(sLogMsg);
            // _W_0.Close();
        }

        public void WriteLogMsg(TConfig Config, string sType, ref TUserEntry UserEntry, ref TUserEntryAdd UserAddEntry)
        {
            // short Year;
            // short Month;
            // short Day;
            // string sLogDir;
            // string sLogFileName;
            // System.IO.FileInfo LogFile;
            // string sLogFormat;
            // string sLogMsg;
            // Year = DateTime.Today.Year;
            // Month = DateTime.Today.Month;
            // Day = DateTime.Today.Day;
            // if (!Directory.Exists(Config.sChrLogDir))
            // {
            //     Directory.CreateDirectory(Config.sChrLogDir);
            // }
            // sLogDir = Config.sChrLogDir + (Year).ToString() + "-" + HUtil32.IntToStr2(Month);
            // if (!Directory.Exists(sLogDir))
            // {
            //     CreateDirectory((sLogDir as string), null);
            // }
            // sLogFileName = sLogDir + "\\Id_" + HUtil32.IntToStr2(Day) + ".log";
            // LogFile = new FileInfo(sLogFileName);
            // if (!File.Exists(sLogFileName))
            // {
            //     _W_0 = LogFile.CreateText();
            // }
            // else
            // {
            //     _W_0 = LogFile.AppendText();
            // }
            // sLogFormat = "*%s*\09%s\09\"%s\"\09%s\09%s\09%s\09%s\09%s\09%s\09%s\09%s\09%s\09[%s]";
            // sLogMsg = format(sLogFormat, new string[] { sType, UserEntry.sAccount, UserEntry.sPassword, UserEntry.sUserName, UserEntry.sSSNo, UserEntry.sQuiz, UserEntry.sAnswer, UserEntry.sEMail, UserAddEntry.sQuiz2, UserAddEntry.sAnswer2, UserAddEntry.sBirthDay, UserAddEntry.sMobilePhone, DateTime.Now.ToString() });
            // _W_0.WriteLine(sLogMsg);
            // _W_0.Close();
        }

        public void StartService(TConfig Config)
        {
            InitializeConfig(Config);
            LoadConfig(Config);
            _accountDB.Initialization();
        }

        public void StopService(TConfig Config)
        {
            UnInitializeConfig(Config);
        }

        public void InitializeConfig(TConfig Config)
        {
            const string sConfigFile = ".\\Logsrv.ini";
            Config.IniConf = new IniFile(sConfigFile);
            Config.GateCriticalSection = new object();
        }

        public void UnInitializeConfig(TConfig Config)
        {
            Config.IniConf = null;
            Config.GateCriticalSection = null;
        }

        public string LoadConfig_LoadConfigString(string sSection, string sIdent, string sDefault)
        {
            string result;
            string sString = LSShare.g_Config.IniConf.ReadString(sSection, sIdent, "");
            if (sString == "")
            {
                LSShare.g_Config.IniConf.WriteString(sSection, sIdent, sDefault);
                result = sDefault;
            }
            else
            {
                result = sString;
            }
            return result;
        }

        public int LoadConfig_LoadConfigInteger(string sSection, string sIdent, int nDefault)
        {
            int result;
            int nLoadInteger;
            nLoadInteger = LSShare.g_Config.IniConf.ReadInteger(sSection, sIdent, -1);
            if (nLoadInteger < 0)
            {
                LSShare.g_Config.IniConf.WriteInteger(sSection, sIdent, nDefault);
                result = nDefault;
            }
            else
            {
                result = nLoadInteger;
            }
            return result;
        }

        public bool LoadConfig_LoadConfigBoolean(string sSection, string sIdent, bool boDefault)
        {
            bool result;
            int nLoadInteger;
            nLoadInteger = LSShare.g_Config.IniConf.ReadInteger(sSection, sIdent, -1);
            if (nLoadInteger < 0)
            {
                LSShare.g_Config.IniConf.WriteBool(sSection, sIdent, boDefault);
                result = boDefault;
            }
            else
            {
                result = nLoadInteger == 1;
            }
            return result;
        }

        public void LoadConfig(TConfig Config)
        {
            const string sSectionServer = "Server";
            const string sSectionDB = "DB";
            const string sIdentDBServer = "DBServer";
            const string sIdentFeeServer = "FeeServer";
            const string sIdentLogServer = "LogServer";
            const string sIdentGateAddr = "GateAddr";
            const string sIdentGatePort = "GatePort";
            const string sIdentServerAddr = "ServerAddr";
            const string sIdentServerPort = "ServerPort";
            const string sIdentMonAddr = "MonAddr";
            const string sIdentMonPort = "MonPort";
            const string sIdentDBSPort = "DBSPort";
            const string sIdentFeePort = "FeePort";
            const string sIdentLogPort = "LogPort";
            const string sIdentReadyServers = "ReadyServers";
            const string sIdentTestServer = "TestServer";
            const string sIdentDynamicIPMode = "DynamicIPMode";
            const string sIdentIdDir = "IdDir";
            const string sIdentWebLogDir = "WebLogDir";
            const string sIdentCountLogDir = "CountLogDir";
            const string sIdentFeedIDList = "FeedIDList";
            const string sIdentFeedIPList = "FeedIPList";
            Config.sDBServer = LoadConfig_LoadConfigString(sSectionServer, sIdentDBServer, Config.sDBServer);
            Config.sFeeServer = LoadConfig_LoadConfigString(sSectionServer, sIdentFeeServer, Config.sFeeServer);
            Config.sLogServer = LoadConfig_LoadConfigString(sSectionServer, sIdentLogServer, Config.sLogServer);
            Config.sGateAddr = LoadConfig_LoadConfigString(sSectionServer, sIdentGateAddr, Config.sGateAddr);
            Config.nGatePort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentGatePort, Config.nGatePort);
            Config.sServerAddr = LoadConfig_LoadConfigString(sSectionServer, sIdentServerAddr, Config.sServerAddr);
            Config.nServerPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentServerPort, Config.nServerPort);
            Config.sMonAddr = LoadConfig_LoadConfigString(sSectionServer, sIdentMonAddr, Config.sMonAddr);
            Config.nMonPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentMonPort, Config.nMonPort);
            Config.nDBSPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentDBSPort, Config.nDBSPort);
            Config.nFeePort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentFeePort, Config.nFeePort);
            Config.nLogPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentLogPort, Config.nLogPort);
            Config.nReadyServers = LoadConfig_LoadConfigInteger(sSectionServer, sIdentReadyServers, Config.nReadyServers);
            Config.boEnableMakingID = LoadConfig_LoadConfigBoolean(sSectionServer, sIdentTestServer, Config.boEnableMakingID);
            Config.boDynamicIPMode = LoadConfig_LoadConfigBoolean(sSectionServer, sIdentDynamicIPMode, Config.boDynamicIPMode);
            Config.sIdDir = LoadConfig_LoadConfigString(sSectionDB, sIdentIdDir, Config.sIdDir);
            Config.sWebLogDir = LoadConfig_LoadConfigString(sSectionDB, sIdentWebLogDir, Config.sWebLogDir);
            Config.sCountLogDir = LoadConfig_LoadConfigString(sSectionDB, sIdentCountLogDir, Config.sCountLogDir);
            Config.sFeedIDList = LoadConfig_LoadConfigString(sSectionDB, sIdentFeedIDList, Config.sFeedIDList);
            Config.sFeedIPList = LoadConfig_LoadConfigString(sSectionDB, sIdentFeedIPList, Config.sFeedIPList);
            LoadAddrTable(Config);
        }
    }
}