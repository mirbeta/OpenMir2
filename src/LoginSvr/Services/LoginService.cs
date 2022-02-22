using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packages;
using SystemModule.Packet;
using SystemModule.Sockets;

namespace LoginSvr
{
    public class LoginService : IService
    {
        private readonly ISocketServer _serverSocket;
        private readonly AccountDB _accountDB = null;
        private readonly MasSocService _masSock;
        private readonly ConfigManager _configManager;
        private const string sConfigFile = "Logsrv.conf";
        private readonly ConcurrentQueue<ReceiveData> _ReceiveQueue = null;
        private readonly ConcurrentQueue<ReceiveUserData> _ReceiveUserQueue = null;
        private readonly object _obj = new object();

        public LoginService(AccountDB accountDB, MasSocService masSock)
        {
            _accountDB = accountDB;
            _masSock = masSock;
            _serverSocket = new ISocketServer(short.MaxValue, 1024);
            _serverSocket.OnClientConnect += GSocketClientConnect;
            _serverSocket.OnClientDisconnect += GSocketClientDisconnect;
            _serverSocket.OnClientRead += GSocketClientRead;
            _serverSocket.OnClientError += GSocketClientError;
            _serverSocket.Init();
            _configManager = new ConfigManager(sConfigFile);
            _ReceiveQueue = new ConcurrentQueue<ReceiveData>();
            _ReceiveUserQueue = new ConcurrentQueue<ReceiveUserData>();
        }

        public void Start()
        {
            _serverSocket.Start(LSShare.g_Config.sGateAddr, LSShare.g_Config.nGatePort);
            LSShare.MainOutMessage($"4) 账号登陆服务[{LSShare.g_Config.sGateAddr}:{LSShare.g_Config.nGatePort}]已启动.");
        }

        private void GSocketClientConnect(object sender, AsyncUserToken e)
        {
            var Config = LSShare.g_Config;
            var GateInfo = new TGateInfo();
            GateInfo.Socket = e.Socket;
            GateInfo.sIPaddr = LSShare.GetGatePublicAddr(Config, e.RemoteIPaddr);
            GateInfo.UserList = new List<TUserInfo>();
            GateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
            Config.GateList.Add(GateInfo);
        }

        private void GSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            TGateInfo GateInfo;
            TUserInfo UserInfo;
            TConfig Config = LSShare.g_Config;
            for (var i = 0; i < Config.GateList.Count; i++)
            {
                GateInfo = Config.GateList[i];
                if (GateInfo.Socket == e.Socket)
                {
                    for (var j = 0; j < GateInfo.UserList.Count; j++)
                    {
                        UserInfo = GateInfo.UserList[j];
                        if (Config.boShowDetailMsg)
                        {
                            LSShare.MainOutMessage("Close: " + UserInfo.sUserIPaddr);
                        }
                        UserInfo = null;
                    }
                    GateInfo.UserList = null;
                    Config.GateList.Remove(GateInfo);
                    break;
                }
            }
            LSShare.MainOutMessage($"[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
        }

        private void GSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void GSocketClientRead(object sender, AsyncUserToken e)
        {
            TConfig Config = LSShare.g_Config;
            for (var i = 0; i < Config.GateList.Count; i++)
            {
                var GateInfo = Config.GateList[i];
                if (GateInfo.Socket == e.Socket)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Array.Copy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    var sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                    _ReceiveQueue.Enqueue(new ReceiveData()
                    {
                        Config = GateInfo,
                        ReceiveMsg = sReviceMsg
                    });
                    LSShare.MainOutMessage("收到数据:" + sReviceMsg);
                    break;
                }
            }
        }

        private void LoadAddrTable(TConfig Config)
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
        /// <returns></returns>
        private bool IsPayMent(TConfig Config, string sIPaddr, string sAccount)
        {
            return Config.AccountCostList.ContainsKey(sAccount) || Config.IPaddrCostList.ContainsKey(sIPaddr);
        }

        public void ProceDataTimer()
        {
            if (LSShare.bo470D20)
            {
                return;
            }
            LSShare.bo470D20 = true;
            try
            {
                HUtil32.EnterCriticalSection(_obj);
                if (_ReceiveQueue.IsEmpty)
                {
                    return;
                }
                if (_ReceiveQueue.TryDequeue(out var receiveData))
                {
                    ProcessGate(LSShare.g_Config, receiveData);
                }
                if (_ReceiveUserQueue.TryDequeue(out var userData))
                {
                    DecodeUserData(LSShare.g_Config, userData.UserInfo, userData.Msg);
                }
            }
            finally
            {
                LSShare.bo470D20 = false;
                HUtil32.LeaveCriticalSection(_obj);
            }
        }

        private void ProcessGate(TConfig Config, ReceiveData receiveData)
        {
            int I;
            TGateInfo GateInfo;
            HUtil32.EnterCriticalSection(Config.GateCriticalSection);
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
                    if (!string.IsNullOrEmpty(receiveData.ReceiveMsg) && GateInfo.UserList != null)
                    {
                        DecodeGateData(Config, GateInfo, receiveData.ReceiveMsg);
                        Config.sGateIPaddr = GateInfo.sIPaddr;
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
                HUtil32.LeaveCriticalSection(Config.GateCriticalSection);
            }
        }

        private void DecodeGateData(TConfig Config, TGateInfo GateInfo, string receiveMsg)
        {
            string sMsg = string.Empty;
            string sSockIndex = string.Empty;
            string sData;
            try
            {
                var nCount = 0;
                if (HUtil32.TagCount(receiveMsg, '$') <= 0)
                {
                    return;
                }
                var tempReceiveMsg = HUtil32.ArrestStringEx(receiveMsg, "%", "$", ref sMsg);
                if (!string.IsNullOrEmpty(tempReceiveMsg))
                {
                    Console.WriteLine("TempReceiveMsg不为空." + tempReceiveMsg);
                }
                if (!string.IsNullOrEmpty(sMsg))
                {
                    var Code = sMsg[0];
                    sMsg = sMsg.Substring(1, sMsg.Length - 1);
                    switch (Code)
                    {
                        case '-':
                            SendKeepAlivePacket(GateInfo.Socket);
                            GateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
                            break;
                        case 'A':
                            sData = HUtil32.GetValidStr3(sMsg, ref sSockIndex, new string[] { "/" });
                            ReceiveSendUser(sSockIndex, GateInfo, sData);
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
                    // if (nCount >= 1)
                    // {
                    //     GateInfo.sReceiveMsg = string.Empty;
                    // }
                    nCount++;
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

        private void ReceiveSendUser(string sSockIndex, TGateInfo GateInfo, string sData)
        {
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                var UserInfo = GateInfo.UserList[i];
                if (UserInfo.sSockIndex == sSockIndex)
                {
                    // if (UserInfo.sReceiveMsg.Length < 4069)
                    // {
                    //     UserInfo.sReceiveMsg = UserInfo.sReceiveMsg + sData;
                    // }
                    _ReceiveUserQueue.Enqueue(new ReceiveUserData()
                    {
                        UserInfo = UserInfo,
                        Msg = sData
                    });
                    break;
                }
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

        private void DecodeUserData(TConfig Config, TUserInfo UserInfo, string userData)
        {
            string sMsg = string.Empty;
            var nCount = 0;
            try
            {
                while (true)
                {
                    if (HUtil32.TagCount(userData, '!') <= 0)
                    {
                        break;
                    }
                    userData = HUtil32.ArrestStringEx(userData, "#", "!", ref sMsg);
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        if (sMsg.Length >= Grobal2.DEFBLOCKSIZE + 1)
                        {
                            sMsg = sMsg.Substring(1, sMsg.Length - 1);
                            ProcessUserMsg(Config, UserInfo, sMsg);
                        }
                    }
                    else
                    {
                        if (nCount >= 1)
                        {
                            userData = string.Empty;
                        }
                        nCount++;
                    }
                    if (string.IsNullOrEmpty(userData))
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
                            AccountCreate(UserInfo, sData);
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

        private void AccountCreate(TUserInfo UserInfo, string sData)
        {
            int nLen = 198;
            bool bo21 = false;
            const string sAddNewuserFail = "[新建帐号失败] {0}/{1}";
            try
            {
                int nErrCode = -1;
                var sUserEntryMsg = sData.Substring(0, nLen);
                var sUserAddEntryMsg = sData.Substring(nLen, sData.Length - nLen);
                TUserEntryAdd UserAddEntry = null;
                TUserEntry UserEntry = null;
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
                                int n10 = _accountDB.Index(UserEntry.sAccount);
                                if (n10 <= 0)
                                {
                                    TAccountDBRecord DBRecord = new TAccountDBRecord();
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
                TDefaultMessage DefMsg;
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
                LSShare.MainOutMessage("TFrmMain.AddNewUser");
            }
        }

        private void AccountChangePassword(TConfig Config, TUserInfo UserInfo, string sData)
        {
            string sLoginID = string.Empty;
            string sOldPassword = string.Empty;
            TDefaultMessage DefMsg;
            TAccountDBRecord DBRecord = null;
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
                        int n10 = _accountDB.Index(sLoginID);
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

        private void AccountCheckProtocol(TUserInfo UserInfo, int nDate)
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

        private bool KickUser(TConfig Config, TUserInfo UserInfo)
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
        private void AccountLogin(TConfig Config, TUserInfo UserInfo, string sData)
        {
            string sLoginID = string.Empty;
            TUserEntry UserEntry = null;
            int nIDCostIndex = -1;
            int nIPCostIndex = -1;
            TAccountDBRecord DBRecord = null;
            try
            {
                var sPassword = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sLoginID, new string[] { "/" });
                var nCode = 0;
                var boNeedUpdate = false;
                try
                {
                    if (_accountDB.Open())
                    {
                        var n10 = _accountDB.Index(sLoginID);
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
                TDefaultMessage DefMsg;
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
                LSShare.MainOutMessage("TFrmMain.LoginUser");
            }
        }

        /// <summary>
        /// 获取角色网关信息
        /// </summary>
        private void GetSelGateInfo(TConfig Config, string sServerName, string sIPaddr, ref string sSelGateIP, ref int nSelGatePort)
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

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <returns></returns>
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
        private void AccountSelectServer(TConfig Config, TUserInfo UserInfo, string sData)
        {
            TDefaultMessage DefMsg;
            bool boPayCost;
            int nPayMode;
            string sSelGateIP = string.Empty;
            int nSelGatePort = 0;
            const string sSelServerMsg = "Server: {0}/{1}-{2}:{3}";
            var sServerName = EDcode.DeCodeString(sData);
            if (!string.IsNullOrEmpty(UserInfo.sAccount) && !string.IsNullOrEmpty(sServerName) && IsLogin(Config, UserInfo.nSessionID))
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
                        LSShare.MainOutMessage($"同步会话消息到[{sServerName}]成功.");
                    }
                    else
                    {
                        UserInfo.boSelServer = false;
                        SessionDel(Config, UserInfo.nSessionID);
                        DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_STARTFAIL, 0, 0, 0, 0);
                        SendGateMsg(UserInfo.Socket, UserInfo.sSockIndex, EDcode.EncodeMessage(DefMsg));
                        LSShare.MainOutMessage($"同步会话消息到[{sServerName}]失败.");
                    }
                }
            }
        }

        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="UserInfo"></param>
        /// <param name="sData"></param>
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

        private void AccountGetBackPassword(TUserInfo UserInfo, string sData)
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

        private void SendGateMsg(Socket Socket, string sSockIndex, string sMsg)
        {
            if (Socket.Connected)
            {
                var sSendMsg = "%" + sSockIndex + "/#" + sMsg + "!$";
                Socket.SendText(sSendMsg);
            }
        }

        private bool IsLogin(TConfig Config, int nSessionID)
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

        private bool IsLogin(TConfig Config, string sLoginID)
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
        private void SessionKick(TConfig Config, string sLoginID)
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

        private void SessionAdd(TConfig Config, string sAccount, string sIPaddr, int nSessionID, bool boPayCost, bool bo11)
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

        private void SendGateKickMsg(Socket Socket, string sSockIndex)
        {
            var sSendMsg = "%+-" + sSockIndex + "$";
            Socket.SendText(sSendMsg);
        }

        private void SessionUpdate(TConfig Config, int nSessionID, string sServerName, bool boPayCost)
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

        private void GenServerNameList(TConfig Config)
        {
            bool boD;
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
      
        public void StartService(TConfig Config)
        {
            InitializeConfig(Config);
            _configManager.LoadConfig(Config);
            LoadAddrTable(Config);
            _accountDB.Initialization();
        }

        public void StopService(TConfig Config)
        {
            UnInitializeConfig(Config);
        }

        public void InitializeConfig(TConfig Config)
        {
            Config.GateCriticalSection = new object();
        }

        public void UnInitializeConfig(TConfig Config)
        {
            Config.GateCriticalSection = null;
        }

    }
}