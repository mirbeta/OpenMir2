using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;

namespace DBSvr
{
    public class UsrSocMessage
    {
        public string Text;
        public TGateInfo GateInfo;
    }

    public class UserSocService
    {
        private IList<TGateInfo> _gateList = null;
        private Dictionary<string, int> _mapList = null;
        private readonly IPlayDataService _playDataService;
        private readonly IPlayRecordService _playRecordService;
        private readonly ISocketServer _userSocket;
        private readonly LoginSvrService _loginService;
        private readonly ConfigManager _configManager;
        private readonly Channel<UsrSocMessage> _reviceMsgList;

        public UserSocService(LoginSvrService loginService, IPlayRecordService playRecordService, IPlayDataService playDataService, ConfigManager configManager)
        {
            _loginService = loginService;
            _playRecordService = playRecordService;
            _playDataService = playDataService;
            _configManager = configManager;
            _gateList = new List<TGateInfo>();
            _mapList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _userSocket = new ISocketServer(ushort.MaxValue, 1024);
            _userSocket.OnClientConnect += UserSocketClientConnect;
            _userSocket.OnClientDisconnect += UserSocketClientDisconnect;
            _userSocket.OnClientRead += UserSocketClientRead;
            _userSocket.OnClientError += UserSocketClientError;
            _userSocket.Init();
            LoadServerInfo();
            LoadChrNameList("DenyChrName.txt");
            LoadClearMakeIndexList("ClearMakeIndex.txt");
            _reviceMsgList = Channel.CreateUnbounded<UsrSocMessage>();
        }

        public void Start()
        {
            _userSocket.Start(DBShare.g_sGateAddr, DBShare.g_nGatePort);
            _playRecordService.LoadQuickList();
            DBShare.MainOutMessage($"数据库服务[{DBShare.g_sGateAddr}:{DBShare.g_nGatePort}]已启动.等待链接...");
        }

        public async Task StartConsumer()
        {
            var gTasks = new Task[1];
            var consumerTask1 = Task.Factory.StartNew(ProcessReviceMessage);
            gTasks[0] = consumerTask1;
            /*var consumerTask2 = Task.Factory.StartNew(_sessionManager.ProcessSendMessage);
            gTasks[1] = consumerTask2;*/
            await Task.WhenAll(gTasks);
        }

        /// <summary>
        /// 处理客户端发过来的消息
        /// </summary>
        private async Task ProcessReviceMessage()
        {
            while (await _reviceMsgList.Reader.WaitToReadAsync())
            {
                if (_reviceMsgList.Reader.TryRead(out var message))
                {
                    ProcessGateMsg(message.GateInfo, message.Text);
                }
            }
        }
        
        private void ProcessGateMsg(TGateInfo GateInfo, string sText)
        {
            var s0C = string.Empty;
            var sData = string.Empty;
            char s19;
            TUserInfo UserInfo;
            while (true)
            {
                if (sText.IndexOf("$", StringComparison.Ordinal) <= 0)
                {
                    break;
                }
                sText = HUtil32.ArrestStringEx(sText, "%", "$", ref sData);
                if (sData != "")
                {
                    s19 = sData[0];
                    sData = sData.Substring(1, sData.Length - 1);
                    switch (s19)
                    {
                        case '-':
                            SendKeepAlivePacket(GateInfo.Socket);
                            //dwKeepAliveTick = HUtil32.GetTickCount();
                            break;
                        case 'A':
                            sData = HUtil32.GetValidStr3(sData, ref s0C, HUtil32.Backslash);
                            for (var i = 0; i < GateInfo.UserList.Count; i++)
                            {
                                UserInfo = GateInfo.UserList[i];
                                if (UserInfo != null)
                                {
                                    if (UserInfo.sConnID == s0C)
                                    {
                                        UserInfo.sText += sData;
                                        if (sData.IndexOf("!", StringComparison.Ordinal) < 1)
                                        {
                                            continue;
                                        }
                                        ProcessUserMsg(GateInfo, ref UserInfo);
                                        break;
                                    }
                                }
                            }
                            break;
                        case 'O':
                        case 'K':
                            sData = HUtil32.GetValidStr3(sData, ref s0C, HUtil32.Backslash);
                            OpenUser(s0C, sData, ref GateInfo);
                            /*dwCheckUserSocTimeMin = GetTickCount - dwCheckUserSocTick;
                            if (dwCheckUserSocTimeMax < dwCheckUserSocTimeMin)
                            {
                                dwCheckUserSocTimeMax = dwCheckUserSocTimeMin;
                                dwCheckUserSocTick = HUtil32.GetTickCount();
                            }*/
                            break;
                        case 'X':
                        case 'L':
                            CloseUser(sData, ref GateInfo);
                            /*dwCheckUserSocTimeMin = GetTickCount - dwCheckUserSocTick;
                            if (dwCheckUserSocTimeMax < dwCheckUserSocTimeMin)
                            {
                                dwCheckUserSocTimeMax = dwCheckUserSocTimeMin;
                                dwCheckUserSocTick = HUtil32.GetTickCount();
                            }*/
                            break;
                    }
                }
            }
        }

        public void Stop()
        {
            TGateInfo GateInfo;
            TUserInfo UserInfo;
            for (var i = 0; i < _gateList.Count; i++)
            {
                GateInfo = _gateList[i];
                if (GateInfo != null)
                {
                    for (var ii = 0; ii < GateInfo.UserList.Count; ii++)
                    {
                        UserInfo = GateInfo.UserList[ii];
                        UserInfo = null;
                    }
                    GateInfo.UserList = null;
                }
                _gateList.RemoveAt(i);
                break;
            }
            _gateList = null;
            _mapList = null;
        }

        private void UserSocketClientConnect(object sender, AsyncUserToken e)
        {
            string sIPaddr = e.RemoteIPaddr;
            const string sGateOpen = "角色网关[{0}]({1}:{2})已打开...";
            if (!DBShare.CheckServerIP(sIPaddr))
            {
                DBShare.MainOutMessage("非法网关连接: " + sIPaddr);
                e.Socket.Close();
                return;
            }
            var GateInfo = new TGateInfo();
            GateInfo.Socket = e.Socket;
            GateInfo.sGateaddr = sIPaddr;
            GateInfo.UserList = new List<TUserInfo>();
            GateInfo.dwTick10 = HUtil32.GetTickCount();
            GateInfo.nGateID = DBShare.GetGateID(sIPaddr);
            _gateList.Add(GateInfo);
            DBShare.MainOutMessage(string.Format(sGateOpen, 0, e.RemoteIPaddr, e.RemotePort));
        }

        private void UserSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            const string sGateClose = "角色网关[{0}]({1}:{2})已关闭...";
            for (var i = 0; i < _gateList.Count; i++)
            {
                var GateInfo = _gateList[i];
                if (GateInfo != null)
                {
                    for (var ii = 0; ii < GateInfo.UserList.Count; ii++)
                    {
                        GateInfo.UserList[ii] = null;
                    }
                    GateInfo.UserList = null;
                }
                DBShare.MainOutMessage(string.Format(sGateClose, i, e.RemoteIPaddr, e.RemotePort));
                _gateList.RemoveAt(i);
                break;
            }
        }

        private void UserSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void UserSocketClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _gateList.Count; i++)
            {
                if (_gateList[i].Socket == e.Socket)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    var sText = HUtil32.GetString(data, 0, data.Length);
                    if (sText.Length < 81920 && sText.IndexOf("$", StringComparison.Ordinal) > 1)
                    {
                        var message = new UsrSocMessage();
                        message.Text = sText;
                        message.GateInfo = _gateList[i];
                        _reviceMsgList.Writer.TryWrite(message);
                    }
                }
            }
        }

        public int GetUserCount()
        {
            TGateInfo GateInfo;
            int nUserCount = 0;
            for (var i = 0; i < _gateList.Count; i++)
            {
                GateInfo = _gateList[i];
                nUserCount += GateInfo.UserList.Count;
            }
            return nUserCount;
        }

        private bool NewChrData(string sAccount, string sChrName, int nSex, int nJob, int nHair)
        {
            bool result = false;
            THumDataInfo ChrRecord;
            try
            {
                if ((_playDataService.Index(sChrName) == -1))
                {
                    ChrRecord = new THumDataInfo();
                    ChrRecord.Header = new TRecordHeader();
                    ChrRecord.Data = new THumInfoData();
                    ChrRecord.Data.Initialization();
                    ChrRecord.Header.sName = sChrName;
                    ChrRecord.Header.sAccount = sAccount;
                    ChrRecord.Data.sCharName = sChrName;
                    ChrRecord.Data.sAccount = sAccount;
                    ChrRecord.Data.btSex = (byte)nSex;
                    ChrRecord.Data.btJob = (byte)nJob;
                    ChrRecord.Data.btHair = (byte)nHair;
                    _playDataService.Add(ref ChrRecord);
                    result = true;
                }
            }
            finally
            {
                
            }
            return result;
        }

        private void LoadServerInfo()
        {
            string sLineText = string.Empty;
            string sSelGateIPaddr = string.Empty;
            string sGameGateIPaddr = string.Empty;
            string sGameGate = string.Empty;
            string sGameGatePort = string.Empty;
            string sMapName = string.Empty;
            string sMapInfo = string.Empty;
            string sServerIndex = string.Empty;
            var LoadList = new StringList();
            if (!File.Exists(DBShare.sGateConfFileName))
            {
                return;
            }
            LoadList.LoadFromFile(DBShare.sGateConfFileName);
            if (LoadList.Count <= 0)
            {
                DBShare.MainOutMessage("加载游戏服务配置文件!ServerInfo.txt失败.");
                return;
            }
            int nRouteIdx = 0;
            int nGateIdx = 0;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i].Trim();
                if (!string.IsNullOrEmpty(sLineText) && !sLineText.StartsWith(";"))
                {
                    sGameGate = HUtil32.GetValidStr3(sLineText, ref sSelGateIPaddr, new[] { " ", "\09" });
                    if ((sGameGate == "") || (sSelGateIPaddr == ""))
                    {
                        continue;
                    }
                    DBShare.g_RouteInfo[nRouteIdx] = new TRouteInfo();
                    DBShare.g_RouteInfo[nRouteIdx].sSelGateIP = sSelGateIPaddr.Trim();
                    DBShare.g_RouteInfo[nRouteIdx].nGateCount = 0;
                    nGateIdx = 0;
                    while ((sGameGate != ""))
                    {
                        sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new[] { " ", "\09" });
                        sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new[] { " ", "\09" });
                        DBShare.g_RouteInfo[nRouteIdx].sGameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                        DBShare.g_RouteInfo[nRouteIdx].nGameGatePort[nGateIdx] = HUtil32.Str_ToInt(sGameGatePort, 0);
                        nGateIdx++;
                    }
                    DBShare.g_RouteInfo[nRouteIdx].nGateCount = nGateIdx;
                    nRouteIdx++;
                }
            }
            DBShare.sMapFile = _configManager.ReadString("Setup", "MapFile", DBShare.sMapFile);
            _mapList.Clear();
            if (File.Exists(DBShare.sMapFile))
            {
                LoadList.Clear();
                LoadList.LoadFromFile(DBShare.sMapFile);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if ((sLineText != "") && (sLineText[0] == '['))
                    {
                        sLineText = HUtil32.ArrestStringEx(sLineText, "[", "]", ref sMapName);
                        sMapInfo = HUtil32.GetValidStr3(sMapName, ref sMapName, new[] { " ", "\09" });
                        sServerIndex = HUtil32.GetValidStr3(sMapInfo, ref sMapInfo, new[] { " ", "\09" }).Trim();
                        int nServerIndex = HUtil32.Str_ToInt(sServerIndex, 0);
                        _mapList.Add(sMapName, nServerIndex);
                    }
                }
            }
            LoadList = null;
        }

        private bool LoadChrNameList(string sFileName)
        {
            bool result = false;
            int i;
            if (File.Exists(sFileName))
            {
                DBShare.DenyChrNameList.LoadFromFile(sFileName);
                i = 0;
                while (true)
                {
                    if (DBShare.DenyChrNameList.Count <= i)
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(DBShare.DenyChrNameList[i].Trim()))
                    {
                        DBShare.DenyChrNameList.RemoveAt(i);
                        continue;
                    }
                    i++;
                }
                result = true;
            }
            return result;
        }

        private bool LoadClearMakeIndexList(string sFileName)
        {
            bool result = false;
            if (File.Exists(sFileName))
            {
                DBShare.g_ClearMakeIndex.LoadFromFile(sFileName);
                var i = 0;
                while (true)
                {
                    if (DBShare.g_ClearMakeIndex.Count <= i)
                    {
                        break;
                    }
                    var sLineText = DBShare.g_ClearMakeIndex[i];
                    var nIndex = HUtil32.Str_ToInt(sLineText, -1);
                    if (nIndex < 0)
                    {
                        DBShare.g_ClearMakeIndex.RemoveAt(i);
                        continue;
                    }
                    DBShare.g_ClearMakeIndex[i] = nIndex.ToString();
                    i++;
                }
                result = true;
            }
            return result;
        }
        
        private void SendKeepAlivePacket(Socket Socket)
        {
            if (Socket.Connected)
            {
                Socket.SendText("%++$");
            }
        }

        private void ProcessUserMsg(TGateInfo GateInfo, ref TUserInfo UserInfo)
        {
            string sData = string.Empty;
            int nC = 0;
            if (HUtil32.TagCount(UserInfo.sText, '!') <= 0)
            {
                return;
            }
            UserInfo.sText = HUtil32.ArrestStringEx(UserInfo.sText, "#", "!", ref sData);
            if (!string.IsNullOrEmpty(sData))
            {
                sData = sData.Substring(1, sData.Length - 1);
                if (sData.Length >= Grobal2.DEFBLOCKSIZE)
                {
                    DeCodeUserMsg(sData, GateInfo, ref UserInfo);
                }
            }
            else
            {
                if (nC >= 1)
                {
                    UserInfo.sText = string.Empty;
                }
                nC++;
            }
        }

        /// <summary>
        /// 打开用户会话
        /// </summary>
        private void OpenUser(string sID, string sIP, ref TGateInfo GateInfo)
        {
            string sUserIPaddr = string.Empty;
            string sGateIPaddr = HUtil32.GetValidStr3(sIP, ref sUserIPaddr, HUtil32.Backslash);
            TUserInfo UserInfo;
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                UserInfo = GateInfo.UserList[i];
                if ((UserInfo != null) && (UserInfo.sConnID == sID))
                {
                    return;
                }
            }
            UserInfo = new TUserInfo();
            UserInfo.sAccount = string.Empty;
            UserInfo.sUserIPaddr = sUserIPaddr;
            UserInfo.sGateIPaddr = sGateIPaddr;
            UserInfo.sConnID = sID;
            UserInfo.nSessionID = 0;
            UserInfo.Socket = GateInfo.Socket;
            UserInfo.sText = string.Empty;
            UserInfo.dwTick34 = HUtil32.GetTickCount();
            UserInfo.dwChrTick = HUtil32.GetTickCount();
            UserInfo.boChrSelected = false;
            UserInfo.boChrQueryed = false;
            UserInfo.nSelGateID = GateInfo.nGateID;
            GateInfo.UserList.Add(UserInfo);
        }

        private void CloseUser(string sID, ref TGateInfo GateInfo)
        {
            TUserInfo UserInfo;
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                UserInfo = GateInfo.UserList[i];
                if ((UserInfo != null) && (UserInfo.sConnID == sID))
                {
                    if (!_loginService.GetGlobaSessionStatus(UserInfo.nSessionID))
                    {
                        _loginService.SendSocketMsg(Grobal2.SS_SOFTOUTSESSION, UserInfo.sAccount + "/" + UserInfo.nSessionID);
                        _loginService.CloseSession(UserInfo.sAccount, UserInfo.nSessionID);
                    }
                    UserInfo = null;
                    GateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void DeCodeUserMsg(string sData, TGateInfo gateInfo, ref TUserInfo UserInfo)
        {
            var sDefMsg = sData.Substring(0, Grobal2.DEFBLOCKSIZE);
            var sText = sData.Substring(Grobal2.DEFBLOCKSIZE, sData.Length - Grobal2.DEFBLOCKSIZE);
            var clientPacket = EDcode.DecodePacket(sDefMsg);
            switch (clientPacket.Ident)
            {
                case Grobal2.CM_QUERYCHR:
                    if (!UserInfo.boChrQueryed || ((HUtil32.GetTickCount() - UserInfo.dwChrTick) > 200))
                    {
                        UserInfo.dwChrTick = HUtil32.GetTickCount();
                        if (QueryChr(sText, ref UserInfo, ref gateInfo))
                        {
                            UserInfo.boChrQueryed = true;
                        }
                    }
                    else
                    {
                        DBShare.MainOutMessage("[Hacker Attack] QUERYCHR " + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_NEWCHR:
                    if ((HUtil32.GetTickCount() - UserInfo.dwChrTick) > 1000)
                    {
                        UserInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((!string.IsNullOrEmpty(UserInfo.sAccount)) && _loginService.CheckSession(UserInfo.sAccount, UserInfo.sUserIPaddr, UserInfo.nSessionID))
                        {
                            NewChr(sText, ref UserInfo);
                            UserInfo.boChrQueryed = false;
                        }
                        else
                        {
                            OutOfConnect(UserInfo);
                        }
                    }
                    else
                    {
                        DBShare.MainOutMessage("[Hacker Attack] NEWCHR " + UserInfo.sAccount + "/" + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_DELCHR:
                    if ((HUtil32.GetTickCount() - UserInfo.dwChrTick) > 1000)
                    {
                        UserInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((UserInfo.sAccount != "") && _loginService.CheckSession(UserInfo.sAccount, UserInfo.sUserIPaddr, UserInfo.nSessionID))
                        {
                            DelChr(sText, ref UserInfo);
                            UserInfo.boChrQueryed = false;
                        }
                        else
                        {
                            OutOfConnect(UserInfo);
                        }
                    }
                    else
                    {
                        DBShare.MainOutMessage("[Hacker Attack] DELCHR " + UserInfo.sAccount + "/" + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_SELCHR:
                    if (UserInfo.boChrQueryed)
                    {
                        if ((UserInfo.sAccount != "") && _loginService.CheckSession(UserInfo.sAccount, UserInfo.sUserIPaddr, UserInfo.nSessionID))
                        {
                            if (SelectChr(sText, gateInfo, ref UserInfo))
                            {
                                UserInfo.boChrSelected = true;
                            }
                        }
                        else
                        {
                            OutOfConnect(UserInfo);
                        }
                    }
                    else
                    {
                        DBShare.MainOutMessage("Double send SELCHR " + UserInfo.sAccount + "/" + UserInfo.sUserIPaddr);
                    }
                    break;
            }
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <returns></returns>
        private bool QueryChr(string sData, ref TUserInfo UserInfo,ref TGateInfo CurGate)
        {
            string sAccount = string.Empty;
            string sSendMsg = string.Empty;
            bool result = false;
            string sSessionID = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sAccount, HUtil32.Backslash);
            int nSessionID = HUtil32.Str_ToInt(sSessionID, -2);
            int nChrCount = 0;
            if (_loginService.CheckSession(sAccount, UserInfo.sUserIPaddr, nSessionID))
            {
                _loginService.SetGlobaSessionNoPlay(nSessionID);
                UserInfo.sAccount = sAccount;
                UserInfo.nSessionID = nSessionID;
                IList<TQuickID> ChrList = new List<TQuickID>();
                if ((_playRecordService.FindByAccount(sAccount, ref ChrList) >= 0))
                {
                    for (var i = 0; i < ChrList.Count; i++)
                    {
                        var quickId = ChrList[i];
                        if (quickId.nSelectID != UserInfo.nSelGateID) // 如果选择ID不对,则跳过
                        {
                            continue;
                        }
                        HumRecordData HumRecord = _playRecordService.GetBy(quickId.nIndex, ref result);
                        if (result && !HumRecord.boDeleted)
                        {
                            string sChrName = quickId.sChrName;
                            var nIndex = _playDataService.Index(sChrName);
                            if ((nIndex < 0) || (nChrCount >= 2))
                            {
                                continue;
                            }
                            THumDataInfo ChrRecord = null;
                            if (_playDataService.Get(nIndex, ref ChrRecord) >= 0)
                            {
                                var btSex = ChrRecord.Data.btSex;
                                var sJob = ChrRecord.Data.btJob;
                                var sHair = ChrRecord.Data.btHair;
                                var sLevel = ChrRecord.Data.Abil.Level;
                                if (HumRecord.boSelected == 1)
                                {
                                    sSendMsg = sSendMsg + "*";
                                }
                                sSendMsg = sSendMsg + sChrName + "/" + sJob + "/" + sHair + "/" + sLevel + "/" + btSex + "/";
                                nChrCount++;
                            }
                        }
                    }
                }
                ChrList = null;
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYCHR, nChrCount, 0, 1, 0)) + EDcode.EncodeString(sSendMsg));
                result = true;
            }
            else
            {
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYCHR_FAIL, nChrCount, 0, 1, 0)));
                CloseUser(UserInfo.sConnID, ref CurGate);
            }
            return result;
        }

        /// <summary>
        /// 会话错误
        /// </summary>
        private void OutOfConnect(TUserInfo UserInfo)
        {
            ClientPacket Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            string sMsg = EDcode.EncodeMessage(Msg);
            SendUserSocket(UserInfo.Socket, sMsg, UserInfo.sConnID);
        }

        private int DelChrSnameToLevel(string sName)
        {
            THumDataInfo ChrRecord = null;
            int result = 0;
            try
            {
                int nIndex = _playDataService.Index(sName);
                if (nIndex >= 0)
                {
                    _playDataService.Get(nIndex, ref ChrRecord);
                    result = ChrRecord.Data.Abil.Level;
                }
            }
            finally
            {
                 
            }
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        private void DelChr(string sData, ref TUserInfo UserInfo)
        {
            ClientPacket Msg;
            var sChrName = EDcode.DeCodeString(sData);
            var boCheck = false;
            var nIndex = _playRecordService.Index(sChrName);
            if (nIndex >= 0)
            {
                var HumRecord = _playRecordService.Get(nIndex, ref boCheck);
                if (boCheck)
                {
                    if (HumRecord.sAccount == UserInfo.sAccount)
                    {
                        int nLevel = DelChrSnameToLevel(sChrName);
                        if (nLevel < DBShare.nDELMaxLevel)
                        {
                            HumRecord.boDeleted = true;
                            boCheck = _playRecordService.Update(nIndex, ref HumRecord);
                        }
                    }
                }
            }
            if (boCheck)
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELCHR_FAIL, 0, 0, 0, 0);
            }
            string sMsg = EDcode.EncodeMessage(Msg);
            SendUserSocket(UserInfo.Socket, UserInfo.sConnID, sMsg);
        }

        /// <summary>
        /// 新建角色
        /// </summary>
        private void NewChr(string sData, ref TUserInfo UserInfo)
        {
            string sAccount = string.Empty;
            string sChrName = string.Empty;
            string sHair = string.Empty;
            string sJob = string.Empty;
            string sSex = string.Empty;
            ClientPacket Msg;
            var nCode = -1;
            string Data = EDcode.DeCodeString(sData);
            Data = HUtil32.GetValidStr3(Data, ref sAccount, HUtil32.Backslash);
            Data = HUtil32.GetValidStr3(Data, ref sChrName, HUtil32.Backslash);
            Data = HUtil32.GetValidStr3(Data, ref sHair, HUtil32.Backslash);
            Data = HUtil32.GetValidStr3(Data, ref sJob, HUtil32.Backslash);
            Data = HUtil32.GetValidStr3(Data, ref sSex, HUtil32.Backslash);
            if (!string.IsNullOrEmpty(Data.Trim()))
            {
                nCode = 0;
            }
            sChrName = sChrName.Trim();
            if (sChrName.Length < 3)
            {
                nCode = 0;
            }
            if (DBShare.g_boEnglishNames && !HUtil32.IsEnglishStr(sChrName))
            {
                nCode = 0;
            }
            if (!CheckDenyChrName(sChrName))
            {
                nCode = 2;
            }
            /*if (!DBShare.boDenyChrName)
            {
                if (!DBShare.CheckChrName(sChrName))
                {
                    nCode = 0;
                }
                for (var i = 0; i < sChrName.Length; i++)
                {
                    if ((sChrName[i] == '?') || (sChrName[i] == ' ') || (sChrName[i] == '/') || (sChrName[i] == '@') || (sChrName[i] == '?') || (sChrName[i] == '\'') ||
                        (sChrName[i] == '\'') || (sChrName[i] == '\\') || (sChrName[i] == '.') || (sChrName[i] == ',') || (sChrName[i] == ':') || (sChrName[i] == ';') ||
                        (sChrName[i] == '`') || (sChrName[i] == '~') || (sChrName[i] == '!') || (sChrName[i] == '#') || (sChrName[i] == '$') || (sChrName[i] == '%') ||
                        (sChrName[i] == '^') || (sChrName[i] == '&') || (sChrName[i] == '*') || (sChrName[i] == '(') || (sChrName[i] == ')') || (sChrName[i] == '-') ||
                        (sChrName[i] == '_') || (sChrName[i] == '+') || (sChrName[i] == '=') || (sChrName[i] == '|') || (sChrName[i] == '[') || (sChrName[i] == '{') ||
                        (sChrName[i] == ']') || (sChrName[i] == '}'))
                    {
                        nCode = 0;
                    }
                }
            }*/
            if (nCode == -1)
            {
                if (_playDataService.Index(sChrName) >= 0)
                {
                    nCode = 2;
                }
                if (_playRecordService.ChrCountOfAccount(sAccount) < 2)
                {
                    HumRecordData HumRecord = new HumRecordData();
                    HumRecord.sChrName = sChrName;
                    HumRecord.sAccount = sAccount;
                    HumRecord.boDeleted = false;
                    HumRecord.Header = new TRecordHeader();
                    HumRecord.Header.sName = sChrName;
                    HumRecord.Header.nSelectID = UserInfo.nSelGateID;
                    if (!string.IsNullOrEmpty(HumRecord.Header.sName))
                    {
                        if (!_playRecordService.Add(HumRecord))
                        {
                            nCode = 2;
                        }
                    }
                }
                else
                {
                    nCode = 3;
                }
                if (nCode == -1)
                {
                    if (NewChrData(sAccount, sChrName, HUtil32.Str_ToInt(sSex, 0), HUtil32.Str_ToInt(sJob, 0), HUtil32.Str_ToInt(sHair, 0)))
                    {
                        nCode = 1;
                    }
                }
                else
                {
                    _playDataService.Delete(sChrName);//删除人物
                    nCode = 4;
                }
            }
            if (nCode == 1)
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWCHR_FAIL, nCode, 0, 0, 0);
            }
            string sMsg = EDcode.EncodeMessage(Msg);
            SendUserSocket(UserInfo.Socket, UserInfo.sConnID, sMsg);
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        /// <returns></returns>
        private bool SelectChr(string sData, TGateInfo CurGate, ref TUserInfo UserInfo)
        {
            string sAccount = string.Empty;
            THumDataInfo ChrRecord = null;
            string sCurMap = string.Empty;
            int nRoutePort = 0;
            var result = false;
            var sChrName = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sAccount, HUtil32.Backslash);
            var boDataOK = false;
            if (UserInfo.sAccount == sAccount)
            {
                int nIndex;
                IList<TQuickID> ChrList = new List<TQuickID>();
                if (_playRecordService.FindByAccount(sAccount, ref ChrList) >= 0)
                {
                    for (var i = 0; i < ChrList.Count; i++)
                    {
                        nIndex = ChrList[i].nIndex;
                        HumRecordData HumRecord = _playRecordService.GetBy(nIndex, ref result);
                        if (result)
                        {
                            if (HumRecord.sChrName == sChrName)
                            {
                                HumRecord.boSelected = 1;
                                _playRecordService.UpdateBy(nIndex, ref HumRecord);
                            }
                            else
                            {
                                if (HumRecord.boSelected == 1)
                                {
                                    HumRecord.boSelected = 0;
                                    _playRecordService.UpdateBy(nIndex, ref HumRecord);
                                }
                            }
                        }
                    }
                }
                ChrList = null;
                nIndex = _playDataService.Index(sChrName);
                if (nIndex >= 0)
                {
                    _playDataService.Get(nIndex, ref ChrRecord);
                    sCurMap = ChrRecord.Data.sCurMap;
                    boDataOK = true;
                }
            }
            if (boDataOK)
            {
                int nMapIndex = GetMapIndex(sCurMap);
                string sDefMsg = EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_STARTPLAY, 0, 0, 0, 0));
                string sRouteIP = GateRouteIP(CurGate.sGateaddr, ref nRoutePort);
                if (DBShare.g_boDynamicIPMode)// 使用动态IP
                {
                    sRouteIP = UserInfo.sGateIPaddr;
                }
                string sRouteMsg = EDcode.EncodeString(sRouteIP + "/" + (nRoutePort + nMapIndex));
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, sDefMsg + sRouteMsg);
                _loginService.SetGlobaSessionPlay(UserInfo.nSessionID);
                result = true;
            }
            else
            {
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_STARTFAIL, 0, 0, 0, 0)));
            }
            return result;
        }

        private int GateRoutePort(string sGateIP)
        {
            return 7200;
        }

        private string GateRouteIP_GetRoute(TRouteInfo RouteInfo, ref int nGatePort)
        {
            var nGateIndex = RandomNumber.GetInstance().Random(RouteInfo.nGateCount);
            var result = RouteInfo.sGameGateIP[nGateIndex];
            nGatePort = RouteInfo.nGameGatePort[nGateIndex];
            return result;
        }

        private string GateRouteIP(string sGateIP, ref int nPort)
        {
            string result = string.Empty;
            TRouteInfo RouteInfo;
            nPort = 0;
            for (var i = DBShare.g_RouteInfo.GetLowerBound(0); i <= DBShare.g_RouteInfo.GetUpperBound(0); i++)
            {
                RouteInfo = DBShare.g_RouteInfo[i];
                if (RouteInfo == null)
                {
                    continue;
                }
                if (RouteInfo.sSelGateIP == sGateIP)
                {
                    result = GateRouteIP_GetRoute(RouteInfo, ref nPort);
                    break;
                }
            }
            return result;
        }

        private int GetMapIndex(string sMap)
        {
            if (string.IsNullOrEmpty(sMap))
            {
                return 0;
            }
            if (_mapList.ContainsKey(sMap))
            {
                return _mapList[sMap];
            }
            return 0;
        }

        private void SendUserSocket(Socket Socket, string sSessionID, string sSendMsg)
        {
            if (Socket.Connected)
            {
                Socket.SendText("%" + sSessionID + "/#" + sSendMsg + "!$");
            }
        }

        /// <summary>
        /// 检查是否禁止名称
        /// </summary>
        /// <param name="sChrName"></param>
        /// <returns></returns>
        private bool CheckDenyChrName(string sChrName)
        {
            bool result = true;
            for (var i = 0; i < DBShare.DenyChrNameList.Count; i++)
            {
                if (string.Compare(sChrName, DBShare.DenyChrNameList[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}