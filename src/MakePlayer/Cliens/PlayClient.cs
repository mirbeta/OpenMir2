using System.Net;
using SystemModule;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace MakePlayer.Cliens
{
    public class PlayClient
    {
        public string SessionId;
        public int ConnectTick = 0;
        public string LoginAccount;
        public string LoginPasswd;
        public int Certification = 0;
        public string ChrName;
        /// <summary>
        /// 当前游戏网络连接步骤
        /// </summary>
        public TConnectionStep ConnectionStep;
        public TConnectionStatus ConnectionStatus;
        public string ServerName = string.Empty;
        public short MWAvailIdDay = 0;
        public short MWAvailIdHour = 0;
        public short MWAvailIpDay = 0;
        public short MWAvailIpHour = 0;
        public bool MBoDoFastFadeOut = false;
        public long MDwFirstServerTime = 0;
        public long MDwFirstClientTime = 0;
        public string MSSelChrAddr = string.Empty;
        public int MNSelChrPort = 0;
        public string MSRunServerAddr = string.Empty;
        public int MNRunServerPort = 0;
        public TSelChar[] MChrArr;
        public string MSMapTitle = string.Empty;
        public string MSMapName = string.Empty;
        public int MNMapMusic = 0;
        public bool MBoActionLock = false;
        public long MDwNotifyEventTick = 0;
        public int MNReceiveCount = 0;
        public string MSMakeNewId = string.Empty;
        public bool MBoTimerMainBusy = false;
        public bool MBoMapMovingWait = false;
        public byte MBtCode = 0;
        public bool MBoSendLogin = false;
        public bool MBoNewAccount = false;
        public int MNGold = 0;
        public byte MBtJob = 0;
        public int MNGameGold = 0;
        public Ability MAbil = null;
        public bool MBoLogin = false;
        public long MDwSayTick = 0;
        private Action? _fNotifyEvent = null;
        public readonly ScoketClient ClientSocket;
        private readonly ClientManager _clientManager;

        public PlayClient(ClientManager clientManager)
        {
            SessionId = string.Empty;
            ClientSocket = new ScoketClient();
            ClientSocket.OnConnected += SocketConnect;
            ClientSocket.OnDisconnected += SocketDisconnect;
            ClientSocket.OnReceivedData += SocketRead;
            ClientSocket.OnError += SocketError;
            MBtCode = 0;
            LoginAccount = "";
            LoginPasswd = "";
            Certification = 0;
            ChrName = "";
            ConnectionStep = TConnectionStep.cnsConnect;
            ConnectionStatus = TConnectionStatus.cns_Success;
            MBoSendLogin = false;
            MBoLogin = false;
            MBoNewAccount = false;
            ConnectTick = HUtil32.GetTickCount();
            _fNotifyEvent = null;
            MDwNotifyEventTick = HUtil32.GetTickCount();
            MChrArr = new TSelChar[2];
            _clientManager = clientManager;
        }

        private void NewAccount()
        {
            ConnectionStep = TConnectionStep.cnsNewAccount;
            SendNewAccount(LoginAccount, LoginPasswd);
        }

        private void NewChr()
        {
            ConnectionStep = TConnectionStep.cnsNewChr;
            SelectChrCreateNewChr(ChrName);
        }

        private void SocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            if (ConnectionStep == TConnectionStep.cnsConnect)
            {
                if (MBoNewAccount)
                {
                    SetNotifyEvent(NewAccount, 6000);
                }
                else
                {
                    ClientNewIdSuccess("");
                }
            }
            else if (ConnectionStep == TConnectionStep.cnsQueryChr)
            {
                // Socket.SendText('#' + '+' + '!');
                //SendQueryChr();
            }
            else if (ConnectionStep == TConnectionStep.cnsPlay)
            {
                ClientSocket.IsConnected = true;
                SendRunLogin();
            }
        }

        private void SocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MainOutMessage($"{LoginAccount}[{ClientSocket.RemoteEndPoint}] 断开链接");
        }

        private void SocketRead(object sender, DSCClientDataInEventArgs e)
        {
            if (e.BuffLen <= 0)
            {
                return;
            }
            var sData = HUtil32.GetString(e.Buff, 0, e.BuffLen);
            var nIdx = sData.IndexOf("*", StringComparison.OrdinalIgnoreCase);
            if (nIdx > 0)
            {
                var sData2 = sData.Substring(0, nIdx - 1);
                sData = sData2 + sData.Substring(nIdx, sData.Length);
                ClientSocket.SendText("*");
            }
            _clientManager.AddPacket(SessionId, e.Buff.ToArray());
        }

        private void SocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    Console.WriteLine("游戏[" + ClientSocket.RemoteEndPoint + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    Console.WriteLine("游戏[" + ClientSocket.RemoteEndPoint + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    Console.WriteLine("游戏[" + ClientSocket.RemoteEndPoint + "]链接超时...");
                    break;
            }
        }

        private void SendSocket(string sText)
        {
            if (ClientSocket.IsConnected)
            {
                var sSendText = "#" + MBtCode + sText + "!";
                ClientSocket.SendText(sSendText);
                MBtCode++;
                if (MBtCode >= 10)
                {
                    MBtCode = 1;
                }
            }
        }

        private void SendClientMessage(int nIdent, int nRecog, int nParam, int nTag, int nSeries)
        {
            var defMsg = Messages.MakeMessage(nIdent, nRecog, nParam, nTag, nSeries);
            SendSocket(EDCode.EncodeMessage(defMsg));
        }

        private void SendNewAccount(string sAccount, string sPassword)
        {
            MainOutMessage($"[{LoginAccount}] 创建帐号");
            ConnectionStep = TConnectionStep.cnsNewAccount;
            var ue = new UserEntry();
            ue.Account = sAccount;
            ue.Password = sPassword;
            ue.UserName = sAccount;
            ue.SSNo = "650101-1455111";
            ue.Quiz = sAccount;
            ue.Answer = sAccount;
            ue.Phone = "";
            ue.EMail = "";
            var ua = new UserEntryAdd();
            ua.Quiz2 = sAccount;
            ua.Answer2 = sAccount;
            ua.BirthDay = "1978/01/01";
            ua.MobilePhone = "";
            ua.Memo = "";
            ua.Memo2 = "";
            var msg = Messages.MakeMessage(Messages.CM_ADDNEWUSER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeBuffer(ue) + EDCode.EncodeBuffer(ua));
        }

        private void SelectChrCreateNewChr(string sChrName)
        {
            byte sHair = 0;
            switch (RandomNumber.GetInstance().Random(1))
            {
                case 0:
                    sHair = 2;
                    break;
                case 1:
                    switch (new Random(1).Next())
                    {
                        case 0:
                            sHair = 1;
                            break;
                        case 1:
                            sHair = 3;
                            break;
                    }
                    break;
            }
            var sJob = (byte)RandomNumber.GetInstance().Random(2);
            var sSex = (byte)RandomNumber.GetInstance().Random(1);
            SendNewChr(LoginAccount, sChrName, sHair, sJob, sSex);
        }

        private void SendSelChr(string sChrName)
        {
            MainOutMessage($"[{LoginAccount}] 选择人物：{sChrName}");
            ConnectionStep = TConnectionStep.cnsSelChr;
            ChrName = sChrName;
            var defMsg = Messages.MakeMessage(Messages.CM_SELCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(LoginAccount + "/" + sChrName));
        }

        private void SendLogin(string sAccount, string sPassword)
        {
            MainOutMessage($"[{LoginAccount}] 开始登录");
            ConnectionStep = TConnectionStep.cnsLogin;
            var defMsg = Messages.MakeMessage(Messages.CM_IDPASSWORD, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sAccount + "/" + sPassword));
            MBoSendLogin = true;
        }

        private void SendNewChr(string sAccount, string sChrName, byte sHair, byte sJob, byte sSex)
        {
            MainOutMessage($"[{LoginAccount}] 创建人物：{sChrName}");
            ConnectionStep = TConnectionStep.cnsNewChr;
            var defMsg = Messages.MakeMessage(Messages.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sAccount + "/" + sChrName + "/" + sHair + "/" + sJob + "/" + sSex));
        }

        private void SendQueryChr()
        {
            MainOutMessage($"[{LoginAccount}] 查询人物");
            ConnectionStep = TConnectionStep.cnsQueryChr;
            var defMsg = Messages.MakeMessage(Messages.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(LoginAccount + "/" + Certification));
        }

        private void SendSelectServer(string sServerName)
        {
            MainOutMessage($"[{LoginAccount}] 选择服务器：{sServerName}");
            ConnectionStep = TConnectionStep.cnsSelServer;
            var defMsg = Messages.MakeMessage(Messages.CM_SELECTSERVER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sServerName));
        }

        private void SendRunLogin()
        {
            MainOutMessage($"[{LoginAccount}] 进入游戏");
            ConnectionStep = TConnectionStep.cnsPlay;
            var sSendMsg = $"**{LoginAccount}/{ChrName}/{Certification}/{Grobal2.CLIENT_VERSION_NUMBER}/{2022080300}";
            SendSocket(EDCode.EncodeString(sSendMsg));
        }

        private void DoNotifyEvent()
        {
            if (_fNotifyEvent != null)
            {
                if (HUtil32.GetTickCount() > MDwNotifyEventTick)
                {
                    _fNotifyEvent();
                    _fNotifyEvent = null;
                }
            }
        }

        private void SetNotifyEvent(Action aNotifyEvent, int nTime)
        {
            MDwNotifyEventTick = HUtil32.GetTickCount() + nTime;
            _fNotifyEvent = aNotifyEvent;
        }

        private void ClientGetStartPlay(string sData)
        {
            var sText = EDCode.DeCodeString(sData);
            var sRunPort = HUtil32.GetValidStr3(sText, ref MSRunServerAddr, '/');
            MNRunServerPort = Convert.ToInt32(sRunPort);
            //ClientSocket.Disconnect();
            ConnectionStep = TConnectionStep.cnsPlay;
            MainOutMessage($"[{LoginAccount}] 准备进入游戏");
            //ClientSocket.ClientType = ClientSocket.ctNonBlocking;
            //ClientSocket.Close();
            ClientSocket.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(MSRunServerAddr), MNRunServerPort);
            ClientSocket.Connect();
            //ClientSocket.Active = true;
        }

        private void ClientStartPlayFail()
        {
            MainOutMessage($"[{LoginAccount}] 此服务器满员！");
        }

        private void ClientVersionFail()
        {
            // MainOutMessage(Format('[%s] 游戏程序版本不正确，请下载最新版本游戏程序！', [m_sLoginAccount]));
        }

        private void ClientGetSendNotice(string sData)
        {
            MainOutMessage($"[{LoginAccount}] 发送公告");
            SendClientMessage(Messages.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
        }

        private void ClientGetUserLogin(CommandMessage defMsg, string sData)
        {
            MBoLogin = true;
            ConnectionStep = TConnectionStep.cnsPlay;
            ConnectionStatus = TConnectionStatus.cns_Success;
            MainOutMessage($"[{LoginAccount}] 成功进入游戏");
            MainOutMessage("-----------------------------------------------");
        }

        public void ClientLoginSay(string message)
        {
            MDwSayTick = HUtil32.GetTickCount();
            var msg = Messages.MakeMessage(Messages.CM_SAY, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(message));
        }

        private void ClientGetAbility(CommandMessage defMsg, string sData)
        {
            MNGold = defMsg.Recog;
            MBtJob = (byte)defMsg.Param;
            MNGameGold = HUtil32.MakeLong(defMsg.Tag, defMsg.Series);
            var buff = EDCode.DecodeBuffer(sData);
            MAbil = ClientPacket.ToPacket<Ability>(buff);
        }

        private void ClientGetWinExp(CommandMessage defMsg)
        {
            MAbil.Exp = defMsg.Recog;
        }

        private void ClientGetLevelUp(CommandMessage defMsg)
        {
            MAbil.Level = (byte)HUtil32.MakeLong(defMsg.Param, defMsg.Tag);
        }

        private void ClientQueryChrFail(int nFailCode)
        {
            ConnectionStatus = TConnectionStatus.cns_Failure;
        }

        private void ClientNewChrFail(int nFailCode)
        {
            ConnectionStatus = TConnectionStatus.cns_Failure;
            Close();
            switch (nFailCode)
            {
                case 0:
                    MainOutMessage($"[{LoginAccount}] [错误信息] 输入的角色名称包含非法字符！ 错误代码 = 0");
                    break;
                case 2:
                    MainOutMessage($"[{LoginAccount}] [错误信息] 创建角色名称已被其他人使用！ 错误代码 = 2");
                    break;
                case 3:
                    MainOutMessage($"[{LoginAccount}] [错误信息] 您只能创建二个游戏角色！ 错误代码 = 3");
                    break;
                case 4:
                    MainOutMessage($"[{LoginAccount}] [错误信息] 创建角色时出现错误！ 错误代码 = 4");
                    break;
                default:
                    MainOutMessage($"[{LoginAccount}] [错误信息] 创建角色时出现未知错误！");
                    break;
            }
        }

        private void ClientNewIdFail(int nFailCode)
        {
            if (nFailCode != 0)
            {
                ConnectionStatus = TConnectionStatus.cns_Failure;
                Close();
            }
            switch (nFailCode)
            {
                case 0:
                    MainOutMessage($"[{LoginAccount}] 帐号已被其他的玩家使用了。请选择其它帐号名注册，尝试使用该账号登陆游戏。");
                    SendLogin(LoginAccount, LoginPasswd);
                    break;
                case 1:
                    MainOutMessage($"[{LoginAccount}] 验证码输入错误，请重新输入！！！");
                    break;
                case -2:
                    MainOutMessage($"[{LoginAccount}] 此帐号名被禁止使用！");
                    break;
                default:
                    MainOutMessage(string.Format("[{0}] 帐号创建失败，请确认帐号是否包括空格、及非法字符！Code: " + nFailCode, LoginAccount));
                    break;
            }
        }

        private void ClientGetPasswdSuccess(string sData)
        {
            var sSelChrPort = string.Empty;
            var sCertification = string.Empty;
            MainOutMessage($"[{LoginAccount}] 帐号登录成功！");
            var sText = EDCode.DeCodeString(sData);
            sText = HUtil32.GetValidStr3(sText, ref MSSelChrAddr, '/');
            sText = HUtil32.GetValidStr3(sText, ref sSelChrPort, '/');
            sText = HUtil32.GetValidStr3(sText, ref sCertification, '/');
            Certification = Convert.ToInt32(sCertification);
            MNSelChrPort = Convert.ToInt32(sSelChrPort);
            //ClientSocket.Disconnect();
            ConnectionStep = TConnectionStep.cnsQueryChr;
            //ClientSocket.Close();
            ClientSocket.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(MSSelChrAddr), MNSelChrPort);
            ClientSocket.Connect();
            //ClientSocket.Active = true;
            // ClientSocket.Socket.SendText('#' + '+' + '!');
            SendQueryChr();
        }

        private void ClientNewIdSuccess(string sData)
        {
            SendLogin(LoginAccount, LoginPasswd);
        }

        private void ClientGetReceiveChrs_AddChr(string sName, byte nJob, byte nHair, int nLevel, byte nSex)
        {
            int I;
            if (!MChrArr[0].boValid)
            {
                I = 0;
            }
            else if (!MChrArr[1].boValid)
            {
                I = 1;
            }
            else
            {
                return;
            }
            MChrArr[I].UserChr.sName = sName;
            MChrArr[I].UserChr.btJob = nJob;
            MChrArr[I].UserChr.btHair = nHair;
            MChrArr[I].UserChr.wLevel = (ushort)nLevel;
            MChrArr[I].UserChr.btSex = nSex;
            MChrArr[I].boValid = true;
        }

        public string ClientGetReceiveChrs_GetJobName(int nJob)
        {
            string result;
            switch (nJob)
            {
                case 0:
                    result = "武士";
                    break;
                case 1:
                    result = "魔法师";
                    break;
                case 2:
                    result = "道士";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        public string ClientGetReceiveChrs_GetSexName(int nSex)
        {
            string result;
            switch (nSex)
            {
                case 0:
                    result = "男";
                    break;
                case 1:
                    result = "女";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        private void ClientGetReceiveChrs(string sData)
        {
            var sName = string.Empty;
            var sJob = string.Empty;
            var sHair = string.Empty;
            var sLevel = string.Empty;
            var sSex = string.Empty;
            var sText = EDCode.DeCodeString(sData);
            var nChrCount = 0;
            var nSelect = 0;
            for (var i = 0; i < MChrArr.Length; i++)
            {
                sText = HUtil32.GetValidStr3(sText, ref sName, '/');
                sText = HUtil32.GetValidStr3(sText, ref sJob, '/');
                sText = HUtil32.GetValidStr3(sText, ref sHair, '/');
                sText = HUtil32.GetValidStr3(sText, ref sLevel, '/');
                sText = HUtil32.GetValidStr3(sText, ref sSex, '/');
                nSelect = 0;
                if ((sName != "") && (sLevel != "") && (sSex != ""))
                {
                    if (sName[0] == '*')
                    {
                        nSelect = i;
                        sName = sName.Substring(1, sName.Length - 1);
                    }
                    ClientGetReceiveChrs_AddChr(sName, Convert.ToByte(sJob), Convert.ToByte(sHair), Convert.ToInt32(sLevel), Convert.ToByte(sSex));
                    nChrCount++;
                }
                if (nSelect == 0)
                {
                    MChrArr[0].boFreezeState = false;
                    MChrArr[0].boSelected = true;
                    MChrArr[1].boFreezeState = true;
                    MChrArr[1].boSelected = false;
                }
                else
                {
                    MChrArr[0].boFreezeState = true;
                    MChrArr[0].boSelected = false;
                    MChrArr[1].boFreezeState = false;
                    MChrArr[1].boSelected = true;
                }
            }
            if (nChrCount > 0)
            {
                SendSelChr(MChrArr[nSelect].UserChr.sName);
            }
            else
            {
                SetNotifyEvent(NewChr, 3000);
            }
        }

        private void ClientLoginFail(int nFailCode)
        {
            if (nFailCode == -3)
            {
                SendLogin(LoginAccount, LoginPasswd);
                MainOutMessage($"[{LoginAccount}] 此帐号已经登录或被异常锁定，请稍候再登录！");
            }
            else
            {
                ConnectionStatus = TConnectionStatus.cns_Failure;
                switch (nFailCode)
                {
                    case -1:
                        MainOutMessage($"[{LoginAccount}] 密码错误！！");
                        break;
                    case -2:
                        MainOutMessage($"[{LoginAccount}] 密码输入错误超过3次，此帐号被暂时锁定，请稍候再登录！");
                        break;
                    case -3:
                        MainOutMessage($"[{LoginAccount}] 此帐号已经登录或被异常锁定，请稍候再登录！");
                        break;
                    case -4:
                        MainOutMessage($"[{LoginAccount}] 这个帐号访问失败！请使用其他帐号登录，或者申请付费注册。");
                        break;
                    case -5:
                        MainOutMessage($"[{LoginAccount}] 这个帐号被锁定！");
                        break;
                    default:
                        MainOutMessage($"[{LoginAccount}] 此帐号不存在或出现未知错误！！");
                        break;
                }
                MBoSendLogin = false;
                Close();
            }
        }

        private void ClientGetServerName(CommandMessage defMsg, string sBody)
        {
            var sServerName = string.Empty;
            var sServerStatus = string.Empty;
            sBody = EDCode.DeCodeString(sBody);
            var nCount = HUtil32._MIN(6, defMsg.Series);
            for (var i = 0; i < nCount; i++)
            {
                sBody = HUtil32.GetValidStr3(sBody, ref sServerName, '/');
                sBody = HUtil32.GetValidStr3(sBody, ref sServerStatus, '/');
                if (sServerName == ServerName)
                {
                    SendSelectServer(sServerName);
                    return;
                }
            }
            if (nCount == 1)
            {
                ServerName = sServerName;
                SendSelectServer(sServerName);
            }
        }

        private void ClientGetPasswordOk(string sData)
        {
            var sServerName = string.Empty;
            MainOutMessage($"[{LoginAccount}] 帐号登录成功！");
            var sText = EDCode.DeCodeString(sData);
            HUtil32.GetValidStr3(sText, ref sServerName, '/');
            SendSelectServer(sServerName);
        }

        private void Close()
        {
            //ClientSocket.Disconnect();
        }

        private void Login()
        {
            if (ConnectionStep == TConnectionStep.cnsConnect && (_fNotifyEvent == null) && !ClientSocket.IsConnected)
            {
                if ((ConnectionStatus == TConnectionStatus.cns_Success) && (HUtil32.GetTickCount() > ConnectTick))
                {
                    ConnectTick = HUtil32.GetTickCount();
                    try
                    {
                        ClientSocket.Connect();
                    }
                    catch
                    {
                        ConnectionStatus = TConnectionStatus.cns_Failure;
                    }
                }
            }
        }

        public void ProcessPacket(byte[] reviceBuffer)
        {
            MBoTimerMainBusy = true;
            try
            {
                var sockText = HUtil32.GetString(reviceBuffer, 0, reviceBuffer.Length);
                if (!string.IsNullOrEmpty(sockText))
                {
                    while (sockText.Length >= 2)
                    {
                        if (MBoMapMovingWait)
                        {
                            break;
                        }
                        if (sockText.IndexOf("!", StringComparison.OrdinalIgnoreCase) <= 0)
                        {
                            break;
                        }
                        var sData = string.Empty;
                        sockText = HUtil32.ArrestStringEx(sockText, "#", "!", ref sData);
                        if (string.IsNullOrEmpty(sData))
                        {
                            break;
                        }
                        DecodeMessagePacket(sData);
                    }
                }
            }
            finally
            {
                MBoTimerMainBusy = false;
            }
        }

        public void Run()
        {
            Login();
            DoNotifyEvent();
        }

        private void DecodeMessagePacket(string sDataBlock)
        {
            if (sDataBlock[0] == '+')
            {
                return;
            }
            if (sDataBlock.Length < Messages.DefBlockSize)
            {
                return;
            }
            var sDefMsg = sDataBlock.Substring(0, Messages.DefBlockSize);
            var sBody = sDataBlock.Substring(Messages.DefBlockSize, sDataBlock.Length - Messages.DefBlockSize);
            var defMsg = EDCode.DecodePacket(sDefMsg);
            switch (defMsg.Ident)
            {
                case Messages.SM_NEWID_SUCCESS:
                    ClientNewIdSuccess(sBody);
                    break;
                case Messages.SM_SELECTSERVER_OK:
                    ClientGetPasswdSuccess(sBody);
                    break;
                case Messages.SM_NEWID_FAIL:
                    ClientNewIdFail(defMsg.Recog);
                    break;
                case Messages.SM_PASSWD_FAIL:
                    ClientLoginFail(defMsg.Recog);
                    break;
                case Messages.SM_PASSOK_SELECTSERVER:
                    ClientGetPasswordOk(sBody);
                    break;
                case Messages.SM_QUERYCHR:
                    ClientGetReceiveChrs(sBody);
                    break;
                case Messages.SM_QUERYCHR_FAIL:
                    ClientQueryChrFail(defMsg.Recog);
                    break;
                case Messages.SM_NEWCHR_SUCCESS:
                    SendQueryChr();
                    break;
                case Messages.SM_NEWCHR_FAIL:
                    ClientNewChrFail(defMsg.Recog);
                    break;
                case Messages.SM_DELCHR_SUCCESS:
                    SendQueryChr();
                    break;
                case Messages.SM_STARTPLAY:
                    ClientGetStartPlay(sBody);
                    break;
                case Messages.SM_STARTFAIL:
                    ClientStartPlayFail();
                    break;
                case Messages.SM_VERSION_FAIL:
                    ClientVersionFail();
                    break;
                case Messages.SM_OUTOFCONNECTION:
                case Messages.SM_NEWMAP:
                case Messages.SM_RECONNECT:
                    break;
                case Messages.SM_ABILITY:
                    ClientGetAbility(defMsg, sBody);
                    break;
                case Messages.SM_WINEXP:
                    ClientGetWinExp(defMsg);
                    break;
                case Messages.SM_LEVELUP:
                    ClientGetLevelUp(defMsg);
                    break;
                case Messages.SM_SENDNOTICE:
                    ClientGetSendNotice(sBody);
                    break;
                case Messages.SM_LOGON:
                    ClientGetUserLogin(defMsg, sBody);
                    break;
                    /*default:
                        MainOutMessage($"未处理消息:[{DefMsg.Ident}]");
                        break;*/
            }
        }

        public void MainOutMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}