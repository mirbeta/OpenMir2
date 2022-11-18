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
        public short m_wAvailIDDay = 0;
        public short m_wAvailIDHour = 0;
        public short m_wAvailIPDay = 0;
        public short m_wAvailIPHour = 0;
        public bool m_boDoFastFadeOut = false;
        public long m_dwFirstServerTime = 0;
        public long m_dwFirstClientTime = 0;
        public string m_sSelChrAddr = string.Empty;
        public int m_nSelChrPort = 0;
        public string m_sRunServerAddr = string.Empty;
        public int m_nRunServerPort = 0;
        public TSelChar[] m_ChrArr;
        public string m_sMapTitle = string.Empty;
        public string m_sMapName = string.Empty;
        public int m_nMapMusic = 0;
        public bool m_boActionLock = false;
        public long m_dwNotifyEventTick = 0;
        public int m_nReceiveCount = 0;
        public string m_sMakeNewId = string.Empty;
        public bool m_boTimerMainBusy = false;
        public bool m_boMapMovingWait = false;
        public byte m_btCode = 0;
        public bool m_boSendLogin = false;
        public bool m_boNewAccount = false;
        public int m_nGold = 0;
        public byte m_btJob = 0;
        public int m_nGameGold = 0;
        public Ability m_Abil = null;
        public bool m_boLogin = false;
        public long m_dwSayTick = 0;
        private Action? FNotifyEvent = null;
        public readonly ClientScoket ClientSocket;
        private readonly ClientManager _clientManager;

        public PlayClient(ClientManager clientManager)
        {
            SessionId = string.Empty;
            ClientSocket = new ClientScoket();
            ClientSocket.OnConnected += SocketConnect;
            ClientSocket.OnDisconnected += SocketDisconnect;
            ClientSocket.OnReceivedData += SocketRead;
            ClientSocket.OnError += SocketError;
            m_btCode = 0;
            LoginAccount = "";
            LoginPasswd = "";
            Certification = 0;
            ChrName = "";
            ConnectionStep = TConnectionStep.cnsConnect;
            ConnectionStatus = TConnectionStatus.cns_Success;
            m_boSendLogin = false;
            m_boLogin = false;
            m_boNewAccount = false;
            ConnectTick = HUtil32.GetTickCount();
            FNotifyEvent = null;
            m_dwNotifyEventTick = HUtil32.GetTickCount();
            m_ChrArr = new TSelChar[2];
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
                if (m_boNewAccount)
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
                var sSendText = "#" + m_btCode + sText + "!";
                ClientSocket.SendText(sSendText);
                m_btCode++;
                if (m_btCode >= 10)
                {
                    m_btCode = 1;
                }
            }
        }

        private void SendClientMessage(int nIdent, int nRecog, int nParam, int nTag, int nSeries)
        {
            var DefMsg = Grobal2.MakeDefaultMsg(nIdent, nRecog, nParam, nTag, nSeries);
            SendSocket(EDCode.EncodeMessage(DefMsg));
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
            var Msg = Grobal2.MakeDefaultMsg(Grobal2.CM_ADDNEWUSER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(Msg) + EDCode.EncodeBuffer(ue) + EDCode.EncodeBuffer(ua));
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
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_SELCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(LoginAccount + "/" + sChrName));
        }

        private void SendLogin(string sAccount, string sPassword)
        {
            MainOutMessage($"[{LoginAccount}] 开始登录");
            ConnectionStep = TConnectionStep.cnsLogin;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_IDPASSWORD, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(sAccount + "/" + sPassword));
            m_boSendLogin = true;
        }

        private void SendNewChr(string sAccount, string sChrName, byte sHair, byte sJob, byte sSex)
        {
            MainOutMessage($"[{LoginAccount}] 创建人物：{sChrName}");
            ConnectionStep = TConnectionStep.cnsNewChr;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(sAccount + "/" + sChrName + "/" + sHair + "/" + sJob + "/" + sSex));
        }

        private void SendQueryChr()
        {
            MainOutMessage($"[{LoginAccount}] 查询人物");
            ConnectionStep = TConnectionStep.cnsQueryChr;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(LoginAccount + "/" + Certification.ToString()));
        }

        private void SendSelectServer(string sServerName)
        {
            MainOutMessage($"[{LoginAccount}] 选择服务器：{sServerName}");
            ConnectionStep = TConnectionStep.cnsSelServer;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_SELECTSERVER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(sServerName));
        }

        private void SendRunLogin()
        {
            MainOutMessage($"[{LoginAccount}] 进入游戏");
            ConnectionStep = TConnectionStep.cnsPlay;
            var sSendMsg = string.Format("**{0}/{1}/{2}/{3}/{4}", new object[] { LoginAccount, ChrName, Certification, Grobal2.CLIENT_VERSION_NUMBER, 2022080300 });
            SendSocket(EDCode.EncodeString(sSendMsg));
        }

        private void DoNotifyEvent()
        {
            if (FNotifyEvent != null)
            {
                if (HUtil32.GetTickCount() > m_dwNotifyEventTick)
                {
                    FNotifyEvent();
                    FNotifyEvent = null;
                }
            }
        }

        private void SetNotifyEvent(Action ANotifyEvent, int nTime)
        {
            m_dwNotifyEventTick = HUtil32.GetTickCount() + nTime;
            FNotifyEvent = ANotifyEvent;
        }

        private void ClientGetStartPlay(string sData)
        {
            var sText = EDCode.DeCodeString(sData);
            var sRunPort = HUtil32.GetValidStr3(sText, ref m_sRunServerAddr, new[] { "/" });
            m_nRunServerPort = Convert.ToInt32(sRunPort);
            //ClientSocket.Disconnect();
            ConnectionStep = TConnectionStep.cnsPlay;
            MainOutMessage($"[{LoginAccount}] 准备进入游戏");
            //ClientSocket.ClientType = ClientSocket.ctNonBlocking;
            //ClientSocket.Close();
            ClientSocket.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(m_sRunServerAddr), m_nRunServerPort);
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
            SendClientMessage(Grobal2.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
        }

        private void ClientGetUserLogin(ClientMesaagePacket DefMsg, string sData)
        {
            m_boLogin = true;
            ConnectionStep = TConnectionStep.cnsPlay;
            ConnectionStatus = TConnectionStatus.cns_Success;
            MainOutMessage($"[{LoginAccount}] 成功进入游戏");
            MainOutMessage("-----------------------------------------------");
        }

        public void ClientLoginSay(string message)
        {
            m_dwSayTick = HUtil32.GetTickCount();
            var Msg = Grobal2.MakeDefaultMsg(Grobal2.CM_SAY, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(Msg) + EDCode.EncodeString(message));
        }

        private void ClientGetAbility(ClientMesaagePacket DefMsg, string sData)
        {
            m_nGold = DefMsg.Recog;
            m_btJob = (byte)DefMsg.Param;
            m_nGameGold = HUtil32.MakeLong(DefMsg.Tag, DefMsg.Series);
            var buff = EDCode.DecodeBuffer(sData);
            m_Abil = ClientPackage.ToPacket<Ability>(buff);
        }

        private void ClientGetWinExp(ClientMesaagePacket DefMsg)
        {
            m_Abil.Exp = DefMsg.Recog;
        }

        private void ClientGetLevelUp(ClientMesaagePacket DefMsg)
        {
            m_Abil.Level = (byte)HUtil32.MakeLong(DefMsg.Param, DefMsg.Tag);
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

        private void ClientNewIDFail(int nFailCode)
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
            sText = HUtil32.GetValidStr3(sText, ref m_sSelChrAddr, new[] { "/" });
            sText = HUtil32.GetValidStr3(sText, ref sSelChrPort, new[] { "/" });
            sText = HUtil32.GetValidStr3(sText, ref sCertification, new[] { "/" });
            Certification = Convert.ToInt32(sCertification);
            m_nSelChrPort = Convert.ToInt32(sSelChrPort);
            //ClientSocket.Disconnect();
            ConnectionStep = TConnectionStep.cnsQueryChr;
            //ClientSocket.Close();
            ClientSocket.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(m_sSelChrAddr), m_nSelChrPort);
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
            if (!m_ChrArr[0].boValid)
            {
                I = 0;
            }
            else if (!m_ChrArr[1].boValid)
            {
                I = 1;
            }
            else
            {
                return;
            }
            m_ChrArr[I].UserChr.sName = sName;
            m_ChrArr[I].UserChr.btJob = nJob;
            m_ChrArr[I].UserChr.btHair = nHair;
            m_ChrArr[I].UserChr.wLevel = (ushort)nLevel;
            m_ChrArr[I].UserChr.btSex = nSex;
            m_ChrArr[I].boValid = true;
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
            for (var i = 0; i < m_ChrArr.Length; i++)
            {
                sText = HUtil32.GetValidStr3(sText, ref sName, new[] { "/" });
                sText = HUtil32.GetValidStr3(sText, ref sJob, new[] { "/" });
                sText = HUtil32.GetValidStr3(sText, ref sHair, new[] { "/" });
                sText = HUtil32.GetValidStr3(sText, ref sLevel, new[] { "/" });
                sText = HUtil32.GetValidStr3(sText, ref sSex, new[] { "/" });
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
                    m_ChrArr[0].boFreezeState = false;
                    m_ChrArr[0].boSelected = true;
                    m_ChrArr[1].boFreezeState = true;
                    m_ChrArr[1].boSelected = false;
                }
                else
                {
                    m_ChrArr[0].boFreezeState = true;
                    m_ChrArr[0].boSelected = false;
                    m_ChrArr[1].boFreezeState = false;
                    m_ChrArr[1].boSelected = true;
                }
            }
            if (nChrCount > 0)
            {
                SendSelChr(m_ChrArr[nSelect].UserChr.sName);
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
                m_boSendLogin = false;
                Close();
            }
        }

        private void ClientGetServerName(ClientMesaagePacket DefMsg, string sBody)
        {
            var sServerName = string.Empty;
            var sServerStatus = string.Empty;
            sBody = EDCode.DeCodeString(sBody);
            var nCount = HUtil32._MIN(6, DefMsg.Series);
            for (var i = 0; i < nCount; i++)
            {
                sBody = HUtil32.GetValidStr3(sBody, ref sServerName, new[] { "/" });
                sBody = HUtil32.GetValidStr3(sBody, ref sServerStatus, new[] { "/" });
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

        private void ClientGetPasswordOK(string sData)
        {
            var sServerName = string.Empty;
            MainOutMessage($"[{LoginAccount}] 帐号登录成功！");
            var sText = EDCode.DeCodeString(sData);
            HUtil32.GetValidStr3(sText, ref sServerName, new[] { "/" });
            SendSelectServer(sServerName);
        }

        private void Close()
        {
            //ClientSocket.Disconnect();
        }

        private void Login()
        {
            if (ConnectionStep == TConnectionStep.cnsConnect && (FNotifyEvent == null) && !ClientSocket.IsConnected)
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
            m_boTimerMainBusy = true;
            try
            {
                var sockText = HUtil32.GetString(reviceBuffer, 0, reviceBuffer.Length);
                if (!string.IsNullOrEmpty(sockText))
                {
                    while (sockText.Length >= 2)
                    {
                        if (m_boMapMovingWait)
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
                m_boTimerMainBusy = false;
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
            if (sDataBlock.Length < Grobal2.DEFBLOCKSIZE)
            {
                return;
            }
            var sDefMsg = sDataBlock.Substring(0, Grobal2.DEFBLOCKSIZE);
            var sBody = sDataBlock.Substring(Grobal2.DEFBLOCKSIZE, sDataBlock.Length - Grobal2.DEFBLOCKSIZE);
            var DefMsg = EDCode.DecodePacket(sDefMsg);
            switch (DefMsg.Ident)
            {
                case Grobal2.SM_NEWID_SUCCESS:
                    ClientNewIdSuccess(sBody);
                    break;
                case Grobal2.SM_SELECTSERVER_OK:
                    ClientGetPasswdSuccess(sBody);
                    break;
                case Grobal2.SM_NEWID_FAIL:
                    ClientNewIDFail(DefMsg.Recog);
                    break;
                case Grobal2.SM_PASSWD_FAIL:
                    ClientLoginFail(DefMsg.Recog);
                    break;
                case Grobal2.SM_PASSOK_SELECTSERVER:
                    ClientGetPasswordOK(sBody);
                    break;
                case Grobal2.SM_QUERYCHR:
                    ClientGetReceiveChrs(sBody);
                    break;
                case Grobal2.SM_QUERYCHR_FAIL:
                    ClientQueryChrFail(DefMsg.Recog);
                    break;
                case Grobal2.SM_NEWCHR_SUCCESS:
                    SendQueryChr();
                    break;
                case Grobal2.SM_NEWCHR_FAIL:
                    ClientNewChrFail(DefMsg.Recog);
                    break;
                case Grobal2.SM_DELCHR_SUCCESS:
                    SendQueryChr();
                    break;
                case Grobal2.SM_STARTPLAY:
                    ClientGetStartPlay(sBody);
                    break;
                case Grobal2.SM_STARTFAIL:
                    ClientStartPlayFail();
                    break;
                case Grobal2.SM_VERSION_FAIL:
                    ClientVersionFail();
                    break;
                case Grobal2.SM_OUTOFCONNECTION:
                case Grobal2.SM_NEWMAP:
                case Grobal2.SM_RECONNECT:
                    break;
                case Grobal2.SM_ABILITY:
                    ClientGetAbility(DefMsg, sBody);
                    break;
                case Grobal2.SM_WINEXP:
                    ClientGetWinExp(DefMsg);
                    break;
                case Grobal2.SM_LEVELUP:
                    ClientGetLevelUp(DefMsg);
                    break;
                case Grobal2.SM_SENDNOTICE:
                    ClientGetSendNotice(sBody);
                    break;
                case Grobal2.SM_LOGON:
                    ClientGetUserLogin(DefMsg, sBody);
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