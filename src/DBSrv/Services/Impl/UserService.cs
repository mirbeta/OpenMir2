using DBSrv.Conf;
using DBSrv.Storage;
using DBSrv.Storage.Model;
using OpenMir2.Data;
using OpenMir2.DataHandlingAdapters;
using System.Threading.Channels;

namespace DBSrv.Services.Impl
{
    /// <summary>
    /// 角色数据服务
    /// DBSrv-SelGate-Client
    /// </summary>
    public class UserService : IService
    {
        private readonly SettingsModel _setting;
        private readonly IPlayDataStorage _playDataStorage;
        private readonly IPlayRecordStorage _playRecordStorage;
        private readonly TcpService _socketServer;
        private readonly ClientSession _loginService;
        private readonly Channel<UserGateMessage> _reviceQueue;
        private readonly SelGateInfo[] _gateClients;

        public UserService(SettingsModel conf, ClientSession sessionService, IPlayRecordStorage playRecord, IPlayDataStorage playData)
        {
            _loginService = sessionService;
            _playRecordStorage = playRecord;
            _playDataStorage = playData;
            _gateClients = new SelGateInfo[20];
            _reviceQueue = Channel.CreateUnbounded<UserGateMessage>();
            _socketServer = new TcpService();
            _socketServer.Connected += Connecting;
            _socketServer.Disconnected += Disconnected;
            _socketServer.Received += Received;
            _setting = conf;
        }

        public void Initialize()
        {
            TouchSocketConfig touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts([
                new IPHost(IPAddress.Parse(_setting.GateAddr), _setting.GatePort)
            ]).SetTcpDataHandlingAdapter(() => new ServerPacketFixedHeaderDataHandlingAdapter());
            _socketServer.Setup(touchSocketConfig);
        }

        public void Start()
        {
            _socketServer.Start();
            _playRecordStorage.LoadQuickList();
            StartMessageThread(CancellationToken.None);
            LogService.Info($"玩家数据网关服务[{_setting.GateAddr}:{_setting.GatePort}]已启动.等待链接...");
        }

        public void Stop()
        {
            for (int i = 0; i < _gateClients.Length; i++)
            {
                SelGateInfo gateInfo = _gateClients[i];
                if (gateInfo != null)
                {
                    for (int ii = 0; ii < gateInfo.UserList.Count; ii++)
                    {
                        gateInfo.UserList[ii] = null;
                    }
                    gateInfo.UserList = null;
                }
            }
        }

        public IEnumerable<SelGateInfo> GetGates => _gateClients;

        /// <summary>
        /// 处理客户端请求消息
        /// </summary>
        private void StartMessageThread(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _reviceQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (_reviceQueue.Reader.TryRead(out UserGateMessage message))
                    {
                        try
                        {
                            SelGateInfo selGata = _gateClients[message.ConnectionId];
                            if (selGata == null)
                            {
                                continue;
                            }
                            ProcessMessage(selGata, message.Packet);
                        }
                        catch (Exception e)
                        {
                            LogService.Error(e);
                        }
                    }
                }
            }, stoppingToken);
        }

        private void ProcessMessage(SelGateInfo gateInfo, ServerDataMessage packet)
        {
            string sTemp = string.Empty;
            string sText = HUtil32.GetString(packet.Data, 0, packet.DataLen);
            HUtil32.ArrestStringEx(sText, "%", "$", ref sTemp);
            for (int i = 0; i < gateInfo.UserList.Count; i++)
            {
                SessionUserInfo userInfo = gateInfo.UserList[i];
                if (userInfo != null)
                {
                    if (userInfo.SessionId == packet.SocketId)
                    {
                        userInfo.sText += sText;
                        if (sText.IndexOf("!", StringComparison.OrdinalIgnoreCase) < 1)
                        {
                            continue;
                        }
                        string sData = string.Empty;
                        if (HUtil32.TagCount(userInfo.sText, '!') <= 0)
                        {
                            return;
                        }
                        userInfo.sText = HUtil32.ArrestStringEx(userInfo.sText, "#", "!", ref sData);
                        if (string.IsNullOrEmpty(sData))
                        {
                            userInfo.sText = string.Empty;
                            continue;
                        }
                        sData = sData[1..];
                        if (sData.Length >= Messages.DefBlockSize)
                        {
                            DeCodeUserMsg(sData, gateInfo, ref userInfo);
                        }
                    }
                }
            }
        }

        private Task Received(ITcpClientBase socketClient, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is not ServerDataMessageFixedHeaderRequestInfo fixedHeader)
            {
                return Task.CompletedTask;
            }

            SocketClient client = (SocketClient)socketClient;
            if (int.TryParse(client.Id, out int clientId))
            {
                SelGateInfo gateClient = _gateClients[clientId - 1];
                ProcessGateData(fixedHeader.Header, fixedHeader.Message, clientId - 1, ref gateClient);
            }
            else
            {
                LogService.Info("未知客户端...");
            }
            return Task.CompletedTask;
        }

        private Task Connecting(SocketClient client, ConnectedEventArgs e)
        {
            IPEndPoint endPoint = (IPEndPoint)client.MainSocket.RemoteEndPoint;
            if (!DBShare.CheckServerIP(endPoint.Address.ToString()))
            {
                LogService.Warn("非法服务器连接: " + endPoint);
                client.Close();
                return Task.CompletedTask;
            }
            SelGateInfo selGateInfo = new SelGateInfo();
            selGateInfo.Socket = client.MainSocket;
            selGateInfo.ConnectionId = client.Id;
            selGateInfo.RemoteEndPoint = client.MainSocket.RemoteEndPoint;
            selGateInfo.UserList = new List<SessionUserInfo>();
            selGateInfo.nGateID = DBShare.GetGateID(endPoint.Address.ToString());
            _gateClients[int.Parse(client.Id) - 1] = selGateInfo;
            LogService.Info(string.Format(sGateOpen, 0, client.MainSocket.RemoteEndPoint));
            return Task.CompletedTask;
        }

        private Task Disconnected(SocketClient client, DisconnectEventArgs e)
        {
            int clientId = int.Parse(client.Id) - 1;
            SelGateInfo gateClient = _gateClients[clientId];
            if (gateClient != null && gateClient.UserList != null)
            {
                for (int ii = 0; ii < gateClient.UserList.Count; ii++)
                {
                    gateClient.UserList[ii] = null;
                }
                gateClient.UserList = null;
            }
            LogService.Info(string.Format(sGateClose, clientId, client.MainSocket.RemoteEndPoint));
            _gateClients[int.Parse(client.Id) - 1] = null;
            return Task.CompletedTask;
        }

        private const string sGateOpen = "角色网关[{0}]({1})已打开...";
        private const string sGateClose = "角色网关[{0}]({1})已关闭...";

        private void ProcessGateData(ServerDataPacket packetHead, byte[] data, int connectionId, ref SelGateInfo gateInfo)
        {
            try
            {
                if (packetHead.PacketCode != Grobal2.PacketCode)
                {
                    LogService.Debug($"解析角色网关封包出现异常...");
                    return;
                }
                ServerDataMessage messageData = SerializerUtil.Deserialize<ServerDataMessage>(data);
                switch (messageData.Type)
                {
                    case ServerDataType.KeepAlive:
                        SendKeepAlivePacket(gateInfo.ConnectionId);
                        break;
                    case ServerDataType.Enter:
                        string sData = string.Empty;
                        string sTemp = string.Empty;
                        string sText = HUtil32.GetString(messageData.Data, 0, messageData.DataLen);
                        HUtil32.ArrestStringEx(sText, "%", "$", ref sData);
                        sData = HUtil32.GetValidStr3(sData, ref sTemp, HUtil32.Backslash);
                        OpenUser(messageData.SocketId, sData, ref gateInfo);
                        break;
                    case ServerDataType.Leave:
                        CloseUser(messageData.SocketId, ref gateInfo);
                        break;
                    case ServerDataType.Data:
                        UserGateMessage userMessage = new UserGateMessage();
                        userMessage.ConnectionId = connectionId;
                        userMessage.Packet = messageData;
                        _reviceQueue.Writer.TryWrite(userMessage);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex);
            }
        }

        public int GetUserCount()
        {
            int nUserCount = 0;
            for (int i = 0; i < _gateClients.Length; i++)
            {
                SelGateInfo gateInfo = _gateClients[i];
                if (gateInfo == null)
                {
                    continue;
                }
                if (gateInfo.UserList == null)
                {
                    continue;
                }
                nUserCount += gateInfo.UserList.Count;
            }
            return nUserCount;
        }

        private bool NewChrData(string sAccount, string sChrName, int nSex, int nJob, int nHair)
        {
            if (_playDataStorage.Index(sChrName) != -1)
            {
                return false;
            }

            CharacterDataInfo chrRecord = new CharacterDataInfo();
            chrRecord.Header = new RecordHeader
            {
                Name = sChrName,
                sAccount = sAccount
            };
            chrRecord.Data = new CharacterData();
            chrRecord.Data.ChrName = sChrName;
            chrRecord.Data.Account = sAccount;
            chrRecord.Data.Sex = (byte)nSex;
            chrRecord.Data.Job = (byte)nJob;
            chrRecord.Data.Hair = (byte)nHair;
            _playDataStorage.Add(chrRecord);
            return true;
        }

        private void SendKeepAlivePacket(string connectionId)
        {
            ServerDataMessage message = new ServerDataMessage();
            message.Type = ServerDataType.KeepAlive;
            SendPacket(connectionId, message);
            LogService.Debug("响应角色网关心跳...");
        }

        /// <summary>
        /// 用户打开会话
        /// </summary>
        private static void OpenUser(string sessionId, string sIp, ref SelGateInfo gateInfo)
        {
            string sUserIPaddr = string.Empty;
            string sGateIPaddr = HUtil32.GetValidStr3(sIp, ref sUserIPaddr, HUtil32.Backslash);
            SessionUserInfo userInfo;
            bool success = false;
            for (int i = 0; i < gateInfo.UserList.Count; i++)
            {
                userInfo = gateInfo.UserList[i];
                if ((userInfo != null) && (userInfo.SessionId == sessionId))
                {
                    success = true;
                    break;
                }
            }
            if (!success)
            {
                userInfo = new SessionUserInfo();
                userInfo.sAccount = string.Empty;
                userInfo.sUserIPaddr = sUserIPaddr;
                userInfo.sGateIPaddr = sGateIPaddr;
                userInfo.SessionId = sessionId;
                userInfo.nSessionID = 0;
                userInfo.ConnectionId = gateInfo.ConnectionId;
                userInfo.sText = string.Empty;
                userInfo.dwTick34 = HUtil32.GetTickCount();
                userInfo.dwChrTick = HUtil32.GetTickCount();
                userInfo.boChrSelected = false;
                userInfo.boChrQueryed = false;
                userInfo.nSelGateID = gateInfo.nGateID;
                gateInfo.UserList.Add(userInfo);
            }
        }

        private void CloseUser(string connId, ref SelGateInfo gateInfo)
        {
            for (int i = 0; i < gateInfo.UserList.Count; i++)
            {
                SessionUserInfo userInfo = gateInfo.UserList[i];
                if ((userInfo != null) && (userInfo.SessionId == connId))
                {
                    if (!_loginService.GetGlobaSessionStatus(userInfo.nSessionID))
                    {
                        _loginService.SendSocketMsg(Messages.SS_SOFTOUTSESSION, userInfo.sAccount + "/" + userInfo.nSessionID);
                        _loginService.CloseSession(userInfo.sAccount, userInfo.nSessionID);
                    }
                    userInfo = null;
                    gateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void DeCodeUserMsg(string sData, SelGateInfo gateInfo, ref SessionUserInfo userInfo)
        {
            string sDefMsg = sData[..Messages.DefBlockSize];
            string sText = sData[Messages.DefBlockSize..];
            CommandMessage clientPacket = EDCode.DecodePacket(sDefMsg);
            switch (clientPacket.Ident)
            {
                case Messages.CM_QUERYCHR:
                    if (!userInfo.boChrQueryed || ((HUtil32.GetTickCount() - userInfo.dwChrTick) > 200))
                    {
                        userInfo.dwChrTick = HUtil32.GetTickCount();
                        if (QueryChr(sText, ref userInfo, ref gateInfo))
                        {
                            userInfo.boChrQueryed = true;
                            LogService.Debug("[QueryChr]:" + sText);
                        }
                        else
                        {
                            LogService.Debug("[QueryChr]:" + sText);
                        }
                    }
                    else
                    {
                        LogService.Warn("[Hacker Attack] QueryChr:" + userInfo.sUserIPaddr);
                    }
                    break;
                case Messages.CM_NEWCHR:
                    if ((HUtil32.GetTickCount() - userInfo.dwChrTick) > 1000)
                    {
                        userInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((!string.IsNullOrEmpty(userInfo.sAccount)) && _loginService.CheckSession(userInfo.sAccount, userInfo.sUserIPaddr, userInfo.nSessionID))
                        {
                            if (NewChr(sText, ref userInfo))
                            {
                                userInfo.boChrQueryed = false;
                            }
                        }
                        else
                        {
                            OutOfConnect(userInfo);
                        }
                    }
                    else
                    {
                        LogService.Warn("[Hacker Attack] NEWCHR " + userInfo.sAccount + "/" + userInfo.sUserIPaddr);
                    }
                    break;
                case Messages.CM_DELCHR:
                    if ((HUtil32.GetTickCount() - userInfo.dwChrTick) > 1000)
                    {
                        userInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((!string.IsNullOrEmpty(userInfo.sAccount)) && _loginService.CheckSession(userInfo.sAccount, userInfo.sUserIPaddr, userInfo.nSessionID))
                        {
                            DeleteChr(sText, ref userInfo);
                            userInfo.boChrQueryed = false;
                        }
                        else
                        {
                            OutOfConnect(userInfo);
                        }
                    }
                    else
                    {
                        LogService.Warn("[Hacker Attack] DELCHR " + userInfo.sAccount + "/" + userInfo.sUserIPaddr);
                    }
                    break;
                case Messages.CM_SELCHR:
                    if (userInfo.boChrQueryed)
                    {
                        if ((!string.IsNullOrEmpty(userInfo.sAccount)) && _loginService.CheckSession(userInfo.sAccount, userInfo.sUserIPaddr, userInfo.nSessionID))
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
                        LogService.Warn("Double send SELCHR " + userInfo.sAccount + "/" + userInfo.sUserIPaddr);
                    }
                    break;
            }
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <returns></returns>
        private bool QueryChr(string sData, ref SessionUserInfo userInfo, ref SelGateInfo curGate)
        {
            string sAccount = string.Empty;
            string sSendMsg = string.Empty;
            bool result = false;
            string sSessionId = HUtil32.GetValidStr3(EDCode.DeCodeString(sData), ref sAccount, HUtil32.Backslash);
            int nSessionId = HUtil32.StrToInt(sSessionId, -2);
            int nChrCount = 0;
            if (_loginService.CheckSession(sAccount, userInfo.sUserIPaddr, nSessionId))
            {
                _loginService.SetGlobaSessionNoPlay(nSessionId);
                userInfo.sAccount = sAccount;
                userInfo.nSessionID = nSessionId;
                IList<PlayQuick> chrList = new List<PlayQuick>();
                if ((_playRecordStorage.FindByAccount(sAccount, ref chrList) >= 0))
                {
                    for (int i = 0; i < chrList.Count; i++)
                    {
                        PlayQuick quickId = chrList[i];
                        if (quickId.SelectID != userInfo.nSelGateID) // 如果选择ID不对,则跳过
                        {
                            continue;
                        }
                        PlayerRecordData humRecord = _playRecordStorage.GetBy(quickId.Index, ref result);
                        if (result && !humRecord.Deleted)
                        {
                            string sChrName = quickId.ChrName;
                            int nIndex = _playDataStorage.Index(sChrName);
                            if ((nIndex < 0) || (nChrCount >= 2))
                            {
                                continue;
                            }
                            QueryChr chrRecord = null;
                            if (_playDataStorage.GetQryChar(nIndex, ref chrRecord))
                            {
                                if (humRecord.Selected == 1)
                                {
                                    sSendMsg = sSendMsg + "*";
                                }
                                sSendMsg = sSendMsg + sChrName + "/" + chrRecord.Job + "/" + chrRecord.Hair + "/" + chrRecord.Level + "/" + chrRecord.Sex + "/";
                                nChrCount++;
                            }
                        }
                    }
                }
                chrList = null;
                SendUserSocket(userInfo.ConnectionId, userInfo.SessionId, EDCode.EncodeMessage(Messages.MakeMessage(Messages.SM_QUERYCHR, nChrCount, 0, 1, 0)) + EDCode.EncodeString(sSendMsg));
                result = true;
            }
            else
            {
                SendUserSocket(userInfo.ConnectionId, userInfo.SessionId, EDCode.EncodeMessage(Messages.MakeMessage(Messages.SM_QUERYCHR_FAIL, nChrCount, 0, 1, 0)));
                CloseUser(userInfo.SessionId, ref curGate);
            }
            return result;
        }

        /// <summary>
        /// 会话错误
        /// </summary>
        private void OutOfConnect(SessionUserInfo userInfo)
        {
            CommandMessage msg = Messages.MakeMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            string sMsg = EDCode.EncodeMessage(msg);
            SendUserSocket(userInfo.ConnectionId, userInfo.SessionId, sMsg);
        }

        private int DelChrSnameToLevel(string sName)
        {
            QueryChr chrRecord = null;
            int nIndex = _playDataStorage.Index(sName);
            if (nIndex < 0)
            {
                return 0;
            }

            if (_playDataStorage.GetQryChar(nIndex, ref chrRecord))
            {
                return chrRecord.Level;
            }
            return 0;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        private void DeleteChr(string sData, ref SessionUserInfo userInfo)
        {
            CommandMessage msg;
            string sChrName = EDCode.DeCodeString(sData);
            bool boCheck = false;
            int nIndex = _playRecordStorage.Index(sChrName);
            if (nIndex >= 0)
            {
                PlayerRecordData humRecord = _playRecordStorage.Get(nIndex, ref boCheck);
                if (boCheck)
                {
                    if (humRecord.sAccount == userInfo.sAccount)
                    {
                        int nLevel = DelChrSnameToLevel(sChrName);
                        if (nLevel < _setting.DeleteMinLevel)
                        {
                            humRecord.Deleted = true;
                            boCheck = _playRecordStorage.Update(nIndex, ref humRecord);
                        }
                    }
                }
            }
            if (boCheck)
            {
                msg = Messages.MakeMessage(Messages.SM_DELCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                msg = Messages.MakeMessage(Messages.SM_DELCHR_FAIL, 0, 0, 0, 0);
            }
            string sMsg = EDCode.EncodeMessage(msg);
            SendUserSocket(userInfo.ConnectionId, userInfo.SessionId, sMsg);
        }

        /// <summary>
        /// 新建角色
        /// </summary>
        private bool NewChr(string sData, ref SessionUserInfo userInfo)
        {
            string sAccount = string.Empty;
            string sChrName = string.Empty;
            string sHair = string.Empty;
            string sJob = string.Empty;
            string sSex = string.Empty;
            CommandMessage msg;
            int nCode = -1;
            string data = EDCode.DeCodeString(sData);
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
            if (_setting.EnglishNames && !HUtil32.IsEnglishStr(sChrName))
            {
                nCode = 0;
            }
            if (!DBShare.CheckDenyChrName(sChrName))
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
                if (_playDataStorage.Index(sChrName) >= 0)
                {
                    nCode = 2;
                }
                if (_playRecordStorage.ChrCountOfAccount(sAccount) < 2)
                {
                    PlayerRecordData humRecord = new PlayerRecordData();
                    humRecord.sChrName = sChrName;
                    humRecord.sAccount = sAccount;
                    humRecord.Deleted = false;
                    humRecord.Header = new RecordHeader();
                    humRecord.Header.Name = sChrName;
                    humRecord.Header.SelectID = userInfo.nSelGateID;
                    if (!_playRecordStorage.Add(humRecord))
                    {
                        nCode = 2;
                    }
                }
                else
                {
                    nCode = 3;
                }
                if (nCode == -1)
                {
                    if (NewChrData(sAccount, sChrName, HUtil32.StrToInt(sSex, 0), HUtil32.StrToInt(sJob, 0), HUtil32.StrToInt(sHair, 0)))
                    {
                        nCode = 1;
                    }
                    else
                    {
                        _playRecordStorage.Delete(sChrName); //创建角色数据失败，删除索引值
                    }
                }
                else
                {
                    _playDataStorage.Delete(sChrName);//删除人物
                    nCode = 4;
                }
            }
            if (nCode == 1)
            {
                msg = Messages.MakeMessage(Messages.SM_NEWCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                msg = Messages.MakeMessage(Messages.SM_NEWCHR_FAIL, nCode, 0, 0, 0);
            }
            LogService.Info("创建角色:{0} 结果:{1}", sChrName, nCode == 1 ? "成功" : "失败");
            string sMsg = EDCode.EncodeMessage(msg);
            SendUserSocket(userInfo.ConnectionId, userInfo.SessionId, sMsg);
            return nCode == 1;
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        /// <returns></returns>
        private bool SelectChr(string sData, SelGateInfo curGate, ref SessionUserInfo userInfo)
        {
            string sAccount = string.Empty;
            string sCurMap = string.Empty;
            int nRoutePort = 0;
            bool result = false;
            string sChrName = HUtil32.GetValidStr3(EDCode.DeCodeString(sData), ref sAccount, HUtil32.Backslash);
            bool boDataOk = false;
            if (string.Compare(userInfo.sAccount, sAccount, StringComparison.OrdinalIgnoreCase) == 0)
            {
                int nIndex;
                IList<PlayQuick> chrList = new List<PlayQuick>();
                if (_playRecordStorage.FindByAccount(sAccount, ref chrList) >= 0)
                {
                    for (int i = 0; i < chrList.Count; i++)
                    {
                        nIndex = chrList[i].Index;
                        PlayerRecordData humRecord = _playRecordStorage.GetBy(nIndex, ref result);
                        if (result)
                        {
                            if (humRecord.sChrName == sChrName)
                            {
                                humRecord.Selected = 1;
                                _playRecordStorage.UpdateBy(nIndex, ref humRecord);
                            }
                            else
                            {
                                if (humRecord.Selected == 1)
                                {
                                    humRecord.Selected = 0;
                                    _playRecordStorage.UpdateBy(nIndex, ref humRecord);
                                }
                            }
                        }
                    }
                }
                chrList = null;
                nIndex = _playDataStorage.Index(sChrName);
                if (nIndex >= 0)
                {
                    CharacterData chrRecord = _playDataStorage.Query(nIndex);
                    if (!string.IsNullOrEmpty(chrRecord.ChrName))
                    {
                        sCurMap = chrRecord.CurMap;
                        boDataOk = true;
                    }
                }
            }
            if (boDataOk)
            {
                int nMapIndex = DBShare.GetMapIndex(sCurMap);
                string sDefMsg = EDCode.EncodeMessage(Messages.MakeMessage(Messages.SM_STARTPLAY, 0, 0, 0, 0));
                string sRouteIp = GateRouteIp(curGate.RemoteEndPoint.GetIP(), ref nRoutePort);
                if (_setting.DynamicIpMode)// 使用动态IP
                {
                    sRouteIp = userInfo.sGateIPaddr;
                }
                string sRouteMsg = EDCode.EncodeString(sRouteIp + "/" + (nRoutePort + nMapIndex));
                SendUserSocket(curGate.ConnectionId, userInfo.SessionId, sDefMsg + sRouteMsg);
                _loginService.SetGlobaSessionPlay(userInfo.nSessionID);
                result = true;
                LogService.Debug($"玩家获取游戏网关信息 RunGame:{sRouteIp} Port:{nRoutePort + nMapIndex}");
            }
            else
            {
                SendUserSocket(curGate.ConnectionId, userInfo.SessionId, EDCode.EncodeMessage(Messages.MakeMessage(Messages.SM_STARTFAIL, 0, 0, 0, 0)));
            }
            return result;
        }

        /// <summary>
        /// 获取游戏网关
        /// </summary>
        /// <returns></returns>
        private static string GetGameGateRoute(GateRouteInfo routeInfo, ref int nGatePort)
        {
            int nGateIndex = RandomNumber.GetInstance().Random(routeInfo.GateCount);
            string result = routeInfo.GameGateIP[nGateIndex];
            nGatePort = routeInfo.GameGatePort[nGateIndex];
            return result;
        }

        private static string GateRouteIp(string sGateIp, ref int nPort)
        {
            string result = string.Empty;
            nPort = 0;
            for (int i = 0; i < DBShare.RouteInfo.Length; i++)
            {
                GateRouteInfo routeInfo = DBShare.RouteInfo[i];
                if (routeInfo == null)
                {
                    continue;
                }
                if (routeInfo.SelGateIP == sGateIp)
                {
                    result = GetGameGateRoute(routeInfo, ref nPort);
                    break;
                }
            }
            return result;
        }

        private void SendUserSocket(string connectionId, string sessionId, string sSendMsg)
        {
            ServerDataMessage message = new ServerDataMessage();
            message.SocketId = sessionId;
            message.Data = HUtil32.GetBytes("#" + sSendMsg + "!");
            message.DataLen = (short)message.Data.Length;
            message.Type = ServerDataType.Data;
            SendPacket(connectionId, message);
        }

        private void SendPacket(string connectionId, ServerDataMessage packet)
        {
            if (!_socketServer.SocketClientExist(connectionId))
            {
                return;
            }

            SendMessage(connectionId, SerializerUtil.Serialize(packet));
        }

        private void SendMessage(string connectionId, byte[] sendBuffer)
        {
            ServerDataPacket serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.PacketCode,
                PacketLen = (ushort)sendBuffer.Length
            };
            byte[] dataBuff = SerializerUtil.Serialize(serverMessage);
            byte[] data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
            MemoryCopy.BlockCopy(dataBuff, 0, data, 0, data.Length);
            MemoryCopy.BlockCopy(sendBuffer, 0, data, dataBuff.Length, sendBuffer.Length);
            _socketServer.Send(connectionId, data);
        }
    }

    public struct UserGateMessage
    {
        public ServerDataMessage Packet;
        public int ConnectionId;
    }
}