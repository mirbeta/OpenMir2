using DBSvr.Conf;
using DBSvr.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace DBSvr.Services
{
    public class UsrSocMessage
    {
        public GatePacket Packet;
        public TGateInfo GateInfo;
    }

    public class UserSocService
    {
        private readonly MirLog _logger;
        private IList<TGateInfo> _gateList = null;
        private Dictionary<string, int> _mapList = null;
        private readonly IPlayDataService _playDataService;
        private readonly IPlayRecordService _playRecordService;
        private readonly SocketServer _userSocket;
        private readonly LoginSvrService _loginService;
        private readonly DBConfig _config = ConfigManager.GetConfig();
        private readonly Channel<UsrSocMessage> _reviceQueue;
        private CancellationTokenSource _cancellationToken;

        public UserSocService(MirLog logger, LoginSvrService loginService, IPlayRecordService playRecordService, IPlayDataService playDataService)
        {
            _logger = logger;
            _loginService = loginService;
            _playRecordService = playRecordService;
            _playDataService = playDataService;
            _gateList = new List<TGateInfo>();
            _mapList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _reviceQueue = Channel.CreateUnbounded<UsrSocMessage>();
            _userSocket = new SocketServer(byte.MaxValue, 1024);
            _userSocket.OnClientConnect += UserSocketClientConnect;
            _userSocket.OnClientDisconnect += UserSocketClientDisconnect;
            _userSocket.OnClientRead += UserSocketClientRead;
            _userSocket.OnClientError += UserSocketClientError;
            _userSocket.Init();
            _cancellationToken = new CancellationTokenSource();
        }

        public void Start()
        {
            LoadServerInfo();
            LoadChrNameList("DenyChrName.txt");
            LoadClearMakeIndexList("ClearMakeIndex.txt");
            _playRecordService.LoadQuickList();
            _userSocket.Start(_config.GateAddr, _config.GatePort);
            StartMessageThread();
            _logger.LogInformation($"数据库服务[{_config.GateAddr}:{_config.GatePort}]已启动.等待链接...");
        }

        public void Stop()
        {
            for (var i = 0; i < _gateList.Count; i++)
            {
                var gateInfo = _gateList[i];
                if (gateInfo != null)
                {
                    for (var ii = 0; ii < gateInfo.UserList.Count; ii++)
                    {
                        gateInfo.UserList[ii] = null;
                    }
                    gateInfo.UserList = null;
                }
                _gateList.RemoveAt(i);
            }
            _gateList = null;
            _mapList = null;
            _cancellationToken.Cancel();//停止消息处理线程
        }

        public IList<TGateInfo> GateList => _gateList;

        /// <summary>
        /// 处理客户端发过来的消息
        /// </summary>
        private void StartMessageThread()
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _reviceQueue.Reader.WaitToReadAsync(_cancellationToken.Token))
                {
                    if (_reviceQueue.Reader.TryRead(out var message))
                    {
                        ProcessGateMsg(message.GateInfo, message.Packet);
                    }
                }
            }, _cancellationToken.Token);
        }

        private void ProcessGateMsg(TGateInfo gateInfo, GatePacket packet)
        {
            var s0C = string.Empty;
            var sData = string.Empty;
            if (packet.EndChar != '$')
            {
                return;
            }
            var sText = HUtil32.GetString(packet.Body, 0, packet.BuffLen);
            HUtil32.ArrestStringEx(sText, "%", "$", ref sData);
            switch (packet.PacketType)
            {
                case PacketType.KeepAlive:
                    _logger.DebugLog("Received SelGate Heartbeat.");
                    SendKeepAlivePacket(gateInfo.Socket);
                    //dwKeepAliveTick = HUtil32.GetTickCount();
                    break;
                case PacketType.Data:
                    for (var i = 0; i < gateInfo.UserList.Count; i++)
                    {
                        var userInfo = gateInfo.UserList[i];
                        if (userInfo != null)
                        {
                            if (userInfo.sConnID == packet.SocketId)
                            {
                                userInfo.sText += sText;
                                if (sText.IndexOf("!", StringComparison.Ordinal) < 1)
                                {
                                    continue;
                                }
                                ProcessUserMsg(gateInfo, ref userInfo);
                                break;
                            }
                        }
                    }
                    break;
                case PacketType.Enter:
                    sData = HUtil32.GetValidStr3(sData, ref s0C, HUtil32.Backslash);
                    OpenUser(packet.SocketId, sData, ref gateInfo);
                    /*dwCheckUserSocTimeMin = GetTickCount - dwCheckUserSocTick;
                    if (dwCheckUserSocTimeMax < dwCheckUserSocTimeMin)
                    {
                        dwCheckUserSocTimeMax = dwCheckUserSocTimeMin;
                        dwCheckUserSocTick = HUtil32.GetTickCount();
                    }*/
                    break;
                case PacketType.Leave:
                    CloseUser(packet.SocketId, ref gateInfo);
                    /*dwCheckUserSocTimeMin = GetTickCount - dwCheckUserSocTick;
                    if (dwCheckUserSocTimeMax < dwCheckUserSocTimeMin)
                    {
                        dwCheckUserSocTimeMax = dwCheckUserSocTimeMin;
                        dwCheckUserSocTick = HUtil32.GetTickCount();
                    }*/
                    break;
            }
        }

        private void UserSocketClientConnect(object sender, AsyncUserToken e)
        {
            var sIPaddr = e.RemoteIPaddr;
            const string sGateOpen = "角色网关[{0}]({1}:{2})已打开...";
            if (!DBShare.CheckServerIP(sIPaddr))
            {
                e.Socket.Close();
                _logger.LogWarning("非法网关连接: " + sIPaddr);
                return;
            }
            var gateInfo = new TGateInfo();
            gateInfo.Socket = e.Socket;
            gateInfo.RemoteEndPoint = e.EndPoint;
            gateInfo.UserList = new List<TUserInfo>();
            gateInfo.nGateID = DBShare.GetGateID(sIPaddr);
            _gateList.Add(gateInfo);
            _logger.LogInformation(string.Format(sGateOpen, 0, e.RemoteIPaddr, e.RemotePort));
        }

        private void UserSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            const string sGateClose = "角色网关[{0}]({1}:{2})已关闭...";
            for (var i = 0; i < _gateList.Count; i++)
            {
                var gateInfo = _gateList[i];
                if (gateInfo != null && gateInfo.UserList != null)
                {
                    for (var ii = 0; ii < gateInfo.UserList.Count; ii++)
                    {
                        gateInfo.UserList[ii] = null;
                    }
                    gateInfo.UserList = null;
                }
                _logger.LogInformation(string.Format(sGateClose, i, e.RemoteIPaddr, e.RemotePort));
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
                if (_gateList[i].Socket != e.Socket)
                    continue;
                var nReviceLen = e.BytesReceived;
                var data = new byte[nReviceLen];
                Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                var packet = Packets.ToPacket<GatePacket>(data);
                if (packet == null)
                {
                    _logger.LogWarning($"错误的消息封包码:{HUtil32.GetString(data, 0, data.Length)} EndPoint:{e.EndPoint}");
                    continue;
                }
                var message = new UsrSocMessage();
                message.Packet = Packets.ToPacket<GatePacket>(data);
                message.GateInfo = _gateList[i];
                _reviceQueue.Writer.TryWrite(message);
            }
        }

        public int GetUserCount()
        {
            TGateInfo gateInfo;
            var nUserCount = 0;
            for (var i = 0; i < _gateList.Count; i++)
            {
                gateInfo = _gateList[i];
                nUserCount += gateInfo.UserList.Count;
            }
            return nUserCount;
        }

        private bool NewChrData(string sAccount, string sChrName, int nSex, int nJob, int nHair)
        {
            var result = false;
            if ((_playDataService.Index(sChrName) == -1))
            {
                var chrRecord = new THumDataInfo();
                chrRecord.Header = new TRecordHeader();
                chrRecord.Data = new THumInfoData();
                chrRecord.Data.Initialization();
                chrRecord.Header.sName = sChrName;
                chrRecord.Header.sAccount = sAccount;
                chrRecord.Data.sCharName = sChrName;
                chrRecord.Data.sAccount = sAccount;
                chrRecord.Data.btSex = (byte)nSex;
                chrRecord.Data.btJob = (byte)nJob;
                chrRecord.Data.btHair = (byte)nHair;
                _playDataService.Add(ref chrRecord);
                result = true;
            }
            return result;
        }

        private void LoadServerInfo()
        {
            var sLineText = string.Empty;
            var sSelGateIPaddr = string.Empty;
            var sGameGateIPaddr = string.Empty;
            var sGameGate = string.Empty;
            var sGameGatePort = string.Empty;
            var sMapName = string.Empty;
            var sMapInfo = string.Empty;
            var sServerIndex = string.Empty;
            var loadList = new StringList();
            if (!File.Exists(DBShare.GateConfFileName))
            {
                return;
            }
            loadList.LoadFromFile(DBShare.GateConfFileName);
            if (loadList.Count <= 0)
            {
                _logger.LogError("加载游戏服务配置文件ServerInfo.txt失败.");
                return;
            }
            var nRouteIdx = 0;
            var nGateIdx = 0;
            for (var i = 0; i < loadList.Count; i++)
            {
                sLineText = loadList[i].Trim();
                if (!string.IsNullOrEmpty(sLineText) && !sLineText.StartsWith(";"))
                {
                    sGameGate = HUtil32.GetValidStr3(sLineText, ref sSelGateIPaddr, new[] { " ", "\09" });
                    if ((sGameGate == "") || (sSelGateIPaddr == ""))
                    {
                        continue;
                    }
                    DBShare.RouteInfo[nRouteIdx] = new TRouteInfo();
                    DBShare.RouteInfo[nRouteIdx].sSelGateIP = sSelGateIPaddr.Trim();
                    DBShare.RouteInfo[nRouteIdx].nGateCount = 0;
                    nGateIdx = 0;
                    while ((sGameGate != ""))
                    {
                        sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new[] { " ", "\09" });
                        sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new[] { " ", "\09" });
                        DBShare.RouteInfo[nRouteIdx].sGameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                        DBShare.RouteInfo[nRouteIdx].nGameGatePort[nGateIdx] = HUtil32.Str_ToInt(sGameGatePort, 0);
                        nGateIdx++;
                    }
                    DBShare.RouteInfo[nRouteIdx].nGateCount = nGateIdx;
                    nRouteIdx++;
                    _logger.LogInformation($"读取网关配置信息.GameGateIP:[{sGameGateIPaddr}:{sGameGatePort}]");
                }
            }
            _logger.LogInformation($"读取网关配置信息成功.[{loadList.Count}]");
            _mapList.Clear();
            if (File.Exists(_config.MapFile))
            {
                loadList.Clear();
                loadList.LoadFromFile(_config.MapFile);
                for (var i = 0; i < loadList.Count; i++)
                {
                    sLineText = loadList[i];
                    if ((sLineText != "") && (sLineText[0] == '['))
                    {
                        sLineText = HUtil32.ArrestStringEx(sLineText, "[", "]", ref sMapName);
                        sMapInfo = HUtil32.GetValidStr3(sMapName, ref sMapName, new[] { " ", "\09" });
                        sServerIndex = HUtil32.GetValidStr3(sMapInfo, ref sMapInfo, new[] { " ", "\09" }).Trim();
                        var nServerIndex = HUtil32.Str_ToInt(sServerIndex, 0);
                        _mapList.Add(sMapName, nServerIndex);
                    }
                }
            }
            loadList = null;
        }

        private void LoadChrNameList(string sFileName)
        {
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
            }
        }

        private void LoadClearMakeIndexList(string sFileName)
        {
            if (File.Exists(sFileName))
            {
                DBShare.ClearMakeIndex.LoadFromFile(sFileName);
                var i = 0;
                while (true)
                {
                    if (DBShare.ClearMakeIndex.Count <= i)
                    {
                        break;
                    }
                    var sLineText = DBShare.ClearMakeIndex[i];
                    var nIndex = HUtil32.Str_ToInt(sLineText, -1);
                    if (nIndex < 0)
                    {
                        DBShare.ClearMakeIndex.RemoveAt(i);
                        continue;
                    }
                    DBShare.ClearMakeIndex[i] = nIndex.ToString();
                    i++;
                }
            }
        }

        private void SendKeepAlivePacket(Socket socket)
        {
            var gataPacket = new GatePacket();
            gataPacket.PacketType = PacketType.KeepAlive;
            gataPacket.SocketId = string.Empty;
            SendPacket(socket, gataPacket);
        }

        private void ProcessUserMsg(TGateInfo gateInfo, ref TUserInfo userInfo)
        {
            var sData = string.Empty;
            var nC = 0;
            if (HUtil32.TagCount(userInfo.sText, '!') <= 0)
            {
                return;
            }
            userInfo.sText = HUtil32.ArrestStringEx(userInfo.sText, "#", "!", ref sData);
            if (!string.IsNullOrEmpty(sData))
            {
                sData = sData.Substring(1, sData.Length - 1);
                if (sData.Length >= Grobal2.DEFBLOCKSIZE)
                {
                    DeCodeUserMsg(sData, gateInfo, ref userInfo);
                }
            }
            else
            {
                if (nC >= 1)
                {
                    userInfo.sText = string.Empty;
                }
                nC++;
            }
        }

        /// <summary>
        /// 打开用户会话
        /// </summary>
        private void OpenUser(string sId, string sIp, ref TGateInfo gateInfo)
        {
            var sUserIPaddr = string.Empty;
            var sGateIPaddr = HUtil32.GetValidStr3(sIp, ref sUserIPaddr, HUtil32.Backslash);
            TUserInfo userInfo;
            var success = false;
            for (var i = 0; i < gateInfo.UserList.Count; i++)
            {
                userInfo = gateInfo.UserList[i];
                if ((userInfo != null) && (userInfo.sConnID == sId))
                {
                    success = true;
                    break;
                }
            }
            if (!success)
            {
                userInfo = new TUserInfo();
                userInfo.sAccount = string.Empty;
                userInfo.sUserIPaddr = sUserIPaddr;
                userInfo.sGateIPaddr = sGateIPaddr;
                userInfo.sConnID = sId;
                userInfo.nSessionID = 0;
                userInfo.Socket = gateInfo.Socket;
                userInfo.sText = string.Empty;
                userInfo.dwTick34 = HUtil32.GetTickCount();
                userInfo.dwChrTick = HUtil32.GetTickCount();
                userInfo.boChrSelected = false;
                userInfo.boChrQueryed = false;
                userInfo.nSelGateID = gateInfo.nGateID;
                gateInfo.UserList.Add(userInfo);
            }
        }

        private void CloseUser(string connId, ref TGateInfo gateInfo)
        {
            for (var i = 0; i < gateInfo.UserList.Count; i++)
            {
                var userInfo = gateInfo.UserList[i];
                if ((userInfo != null) && (userInfo.sConnID == connId))
                {
                    if (!_loginService.GetGlobaSessionStatus(userInfo.nSessionID))
                    {
                        _loginService.SendSocketMsg(Grobal2.SS_SOFTOUTSESSION, userInfo.sAccount + "/" + userInfo.nSessionID);
                        _loginService.CloseSession(userInfo.sAccount, userInfo.nSessionID);
                    }
                    userInfo = null;
                    gateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void DeCodeUserMsg(string sData, TGateInfo gateInfo, ref TUserInfo userInfo)
        {
            var sDefMsg = sData.Substring(0, Grobal2.DEFBLOCKSIZE);
            var sText = sData.Substring(Grobal2.DEFBLOCKSIZE, sData.Length - Grobal2.DEFBLOCKSIZE);
            var clientPacket = EDcode.DecodePacket(sDefMsg);
            switch (clientPacket.Ident)
            {
                case Grobal2.CM_QUERYCHR:
                    if (!userInfo.boChrQueryed || ((HUtil32.GetTickCount() - userInfo.dwChrTick) > 200))
                    {
                        userInfo.dwChrTick = HUtil32.GetTickCount();
                        if (QueryChr(sText, ref userInfo, ref gateInfo))
                        {
                            userInfo.boChrQueryed = true;
                            _logger.DebugLog("[QueryChr] Success " + sText);
                        }
                        else
                        {
                            _logger.DebugLog("[QueryChr] Fail " + sText);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("[Hacker Attack] QueryChr " + userInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_NEWCHR:
                    if ((HUtil32.GetTickCount() - userInfo.dwChrTick) > 1000)
                    {
                        userInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((!string.IsNullOrEmpty(userInfo.sAccount)) && _loginService.CheckSession(userInfo.sAccount, userInfo.sUserIPaddr, userInfo.nSessionID))
                        {
                            NewChr(sText, ref userInfo);
                            userInfo.boChrQueryed = false;
                        }
                        else
                        {
                            OutOfConnect(userInfo);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("[Hacker Attack] NEWCHR " + userInfo.sAccount + "/" + userInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_DELCHR:
                    if ((HUtil32.GetTickCount() - userInfo.dwChrTick) > 1000)
                    {
                        userInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((userInfo.sAccount != "") && _loginService.CheckSession(userInfo.sAccount, userInfo.sUserIPaddr, userInfo.nSessionID))
                        {
                            DelChr(sText, ref userInfo);
                            userInfo.boChrQueryed = false;
                        }
                        else
                        {
                            OutOfConnect(userInfo);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("[Hacker Attack] DELCHR " + userInfo.sAccount + "/" + userInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_SELCHR:
                    if (userInfo.boChrQueryed)
                    {
                        if ((userInfo.sAccount != "") && _loginService.CheckSession(userInfo.sAccount, userInfo.sUserIPaddr, userInfo.nSessionID))
                        {
                            if (SelectChr(sText, gateInfo, ref userInfo))
                            {
                                userInfo.boChrSelected = true;
                            }
                        }
                        else
                        {
                            OutOfConnect(userInfo);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Double send SELCHR " + userInfo.sAccount + "/" + userInfo.sUserIPaddr);
                    }
                    break;
            }
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <returns></returns>
        private bool QueryChr(string sData, ref TUserInfo userInfo, ref TGateInfo curGate)
        {
            var sAccount = string.Empty;
            var sSendMsg = string.Empty;
            var result = false;
            var sSessionId = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sAccount, HUtil32.Backslash);
            var nSessionId = HUtil32.Str_ToInt(sSessionId, -2);
            var nChrCount = 0;
            if (_loginService.CheckSession(sAccount, userInfo.sUserIPaddr, nSessionId))
            {
                _loginService.SetGlobaSessionNoPlay(nSessionId);
                userInfo.sAccount = sAccount;
                userInfo.nSessionID = nSessionId;
                IList<TQuickID> chrList = new List<TQuickID>();
                if ((_playRecordService.FindByAccount(sAccount, ref chrList) >= 0))
                {
                    for (var i = 0; i < chrList.Count; i++)
                    {
                        var quickId = chrList[i];
                        if (quickId.nSelectID != userInfo.nSelGateID) // 如果选择ID不对,则跳过
                        {
                            continue;
                        }
                        var humRecord = _playRecordService.GetBy(quickId.nIndex, ref result);
                        if (result && !humRecord.boDeleted)
                        {
                            var sChrName = quickId.sChrName;
                            var nIndex = _playDataService.Index(sChrName);
                            if ((nIndex < 0) || (nChrCount >= 2))
                            {
                                continue;
                            }
                            THumDataInfo chrRecord = null;
                            if (_playDataService.Get(nIndex, ref chrRecord) >= 0)
                            {
                                var btSex = chrRecord.Data.btSex;
                                var sJob = chrRecord.Data.btJob;
                                var sHair = chrRecord.Data.btHair;
                                var sLevel = chrRecord.Data.Abil.Level;
                                if (humRecord.boSelected == 1)
                                {
                                    sSendMsg = sSendMsg + "*";
                                }
                                sSendMsg = sSendMsg + sChrName + "/" + sJob + "/" + sHair + "/" + sLevel + "/" + btSex + "/";
                                nChrCount++;
                            }
                        }
                    }
                }
                chrList = null;
                SendUserSocket(userInfo.Socket, userInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYCHR, nChrCount, 0, 1, 0)) + EDcode.EncodeString(sSendMsg));
                result = true;
            }
            else
            {
                SendUserSocket(userInfo.Socket, userInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYCHR_FAIL, nChrCount, 0, 1, 0)));
                CloseUser(userInfo.sConnID, ref curGate);
            }
            return result;
        }

        /// <summary>
        /// 会话错误
        /// </summary>
        private void OutOfConnect(TUserInfo userInfo)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var sMsg = EDcode.EncodeMessage(msg);
            SendUserSocket(userInfo.Socket, sMsg, userInfo.sConnID);
        }

        private int DelChrSnameToLevel(string sName)
        {
            THumDataInfo chrRecord = null;
            var nIndex = _playDataService.Index(sName);
            if (nIndex < 0)
                return 0;
            _playDataService.Get(nIndex, ref chrRecord);
            return chrRecord.Data.Abil.Level;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        private void DelChr(string sData, ref TUserInfo userInfo)
        {
            ClientPacket msg;
            var sChrName = EDcode.DeCodeString(sData);
            var boCheck = false;
            var nIndex = _playRecordService.Index(sChrName);
            if (nIndex >= 0)
            {
                var humRecord = _playRecordService.Get(nIndex, ref boCheck);
                if (boCheck)
                {
                    if (humRecord.sAccount == userInfo.sAccount)
                    {
                        var nLevel = DelChrSnameToLevel(sChrName);
                        if (nLevel < _config.nDELMaxLevel)
                        {
                            humRecord.boDeleted = true;
                            boCheck = _playRecordService.Update(nIndex, ref humRecord);
                        }
                    }
                }
            }
            if (boCheck)
            {
                msg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                msg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELCHR_FAIL, 0, 0, 0, 0);
            }
            var sMsg = EDcode.EncodeMessage(msg);
            SendUserSocket(userInfo.Socket, userInfo.sConnID, sMsg);
        }

        /// <summary>
        /// 新建角色
        /// </summary>
        private void NewChr(string sData, ref TUserInfo userInfo)
        {
            var sAccount = string.Empty;
            var sChrName = string.Empty;
            var sHair = string.Empty;
            var sJob = string.Empty;
            var sSex = string.Empty;
            ClientPacket msg;
            var nCode = -1;
            var data = EDcode.DeCodeString(sData);
            data = HUtil32.GetValidStr3(data, ref sAccount, HUtil32.Backslash);
            data = HUtil32.GetValidStr3(data, ref sChrName, HUtil32.Backslash);
            data = HUtil32.GetValidStr3(data, ref sHair, HUtil32.Backslash);
            data = HUtil32.GetValidStr3(data, ref sJob, HUtil32.Backslash);
            data = HUtil32.GetValidStr3(data, ref sSex, HUtil32.Backslash);
            if (!string.IsNullOrEmpty(data.Trim()))
            {
                nCode = 0;
            }
            sChrName = sChrName.Trim();
            if (sChrName.Length < 3)
            {
                nCode = 0;
            }
            if (_config.EnglishNames && !HUtil32.IsEnglishStr(sChrName))
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
                    var humRecord = new HumRecordData();
                    humRecord.sChrName = sChrName;
                    humRecord.sAccount = sAccount;
                    humRecord.boDeleted = false;
                    humRecord.Header = new TRecordHeader();
                    humRecord.Header.sName = sChrName;
                    humRecord.Header.nSelectID = userInfo.nSelGateID;
                    if (!string.IsNullOrEmpty(humRecord.Header.sName))
                    {
                        if (!_playRecordService.Add(humRecord))
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
                msg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                msg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWCHR_FAIL, nCode, 0, 0, 0);
            }
            var sMsg = EDcode.EncodeMessage(msg);
            SendUserSocket(userInfo.Socket, userInfo.sConnID, sMsg);
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        /// <returns></returns>
        private bool SelectChr(string sData, TGateInfo curGate, ref TUserInfo userInfo)
        {
            var sAccount = string.Empty;
            var sCurMap = string.Empty;
            var nRoutePort = 0;
            var result = false;
            var sChrName = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sAccount, HUtil32.Backslash);
            var boDataOk = false;
            if (userInfo.sAccount == sAccount)
            {
                int nIndex;
                IList<TQuickID> chrList = new List<TQuickID>();
                if (_playRecordService.FindByAccount(sAccount, ref chrList) >= 0)
                {
                    for (var i = 0; i < chrList.Count; i++)
                    {
                        nIndex = chrList[i].nIndex;
                        var humRecord = _playRecordService.GetBy(nIndex, ref result);
                        if (result)
                        {
                            if (humRecord.sChrName == sChrName)
                            {
                                humRecord.boSelected = 1;
                                _playRecordService.UpdateBy(nIndex, ref humRecord);
                            }
                            else
                            {
                                if (humRecord.boSelected == 1)
                                {
                                    humRecord.boSelected = 0;
                                    _playRecordService.UpdateBy(nIndex, ref humRecord);
                                }
                            }
                        }
                    }
                }
                chrList = null;
                nIndex = _playDataService.Index(sChrName);
                if (nIndex >= 0)
                {
                    THumDataInfo chrRecord = null;
                    _playDataService.Get(nIndex, ref chrRecord);
                    sCurMap = chrRecord.Data.sCurMap;
                    boDataOk = true;
                }
            }
            if (boDataOk)
            {
                var nMapIndex = GetMapIndex(sCurMap);
                var sDefMsg = EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_STARTPLAY, 0, 0, 0, 0));
                var sRouteIp = GateRouteIp(curGate.RemoteEndPoint.GetIPAddress(), ref nRoutePort);
                if (_config.DynamicIpMode)// 使用动态IP
                {
                    sRouteIp = userInfo.sGateIPaddr;
                }
                var sRouteMsg = EDcode.EncodeString(sRouteIp + "/" + (nRoutePort + nMapIndex));
                SendUserSocket(userInfo.Socket, userInfo.sConnID, sDefMsg + sRouteMsg);
                _loginService.SetGlobaSessionPlay(userInfo.nSessionID);
                result = true;
                _logger.DebugLog($"发送游戏网关信息: GameRun:{sRouteIp} Port:{nRoutePort + nMapIndex}");
            }
            else
            {
                SendUserSocket(userInfo.Socket, userInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_STARTFAIL, 0, 0, 0, 0)));
            }
            return result;
        }

        private string GateRouteIP_GetRoute(TRouteInfo routeInfo, ref int nGatePort)
        {
            var nGateIndex = RandomNumber.GetInstance().Random(routeInfo.nGateCount);
            var result = routeInfo.sGameGateIP[nGateIndex];
            nGatePort = routeInfo.nGameGatePort[nGateIndex];
            return result;
        }

        private string GateRouteIp(string sGateIp, ref int nPort)
        {
            var result = string.Empty;
            TRouteInfo routeInfo;
            nPort = 0;
            for (var i = 0; i < DBShare.RouteInfo.Length; i++)
            {
                routeInfo = DBShare.RouteInfo[i];
                if (routeInfo == null)
                {
                    continue;
                }
                if (routeInfo.sSelGateIP == sGateIp)
                {
                    result = GateRouteIP_GetRoute(routeInfo, ref nPort);
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
            return _mapList.ContainsKey(sMap) ? _mapList[sMap] : 0;
        }

        private void SendUserSocket(Socket socket, string sessionId, string sSendMsg)
        {
            var packet = new GatePacket();
            packet.SocketId = sessionId;
            packet.Body = HUtil32.GetBytes("#" + sSendMsg + "!");
            packet.BuffLen = (short)packet.Body.Length;
            packet.PacketType = PacketType.Data;
            SendPacket(socket, packet);
        }

        /// <summary>
        /// 检查是否禁止名称
        /// </summary>
        /// <param name="sChrName"></param>
        /// <returns></returns>
        private bool CheckDenyChrName(string sChrName)
        {
            var result = true;
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

        private void SendPacket(Socket socket, GatePacket packet)
        {
            if (!socket.Connected)
                return;
            packet.StartChar = '%';
            packet.EndChar = '$';
            socket.Send(packet.GetBuffer());
        }
    }
}