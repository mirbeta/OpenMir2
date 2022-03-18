using SystemModule;
using SystemModule.Packet;
using SystemModule.Sockets;

namespace MakePlayer
{
    public class PlayClient
    {
        public string SessionId;
        public int m_dwConnectTick = 0;
        public string m_sLoginAccount = string.Empty;
        public string m_sLoginPasswd = string.Empty;
        public int m_nCertification = 0;
        public string m_sCharName = string.Empty;
        /// <summary>
        /// 当前游戏网络连接步骤
        /// </summary>
        public TConnectionStep m_ConnectionStep;
        public TConnectionStatus m_ConnectionStatus;
        public string m_sServerName = string.Empty;
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
        /*public ArrayList m_ChangeFaceReadyList;
        public ArrayList m_FreeActorList;
        public int m_nTargetX = 0;
        public int m_nTargetY = 0;*/
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
        public TAbility m_Abil = null;
        public bool m_boLogin = false;
        public long m_dwSayTick = 0;
        private Action? FNotifyEvent = null;
        public IClientScoket ClientSocket;

        public PlayClient()
        {
            SessionId = string.Empty;
            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += SocketConnect;
            ClientSocket.OnDisconnected += SocketDisconnect;
            ClientSocket.ReceivedDatagram += SocketRead;
            ClientSocket.OnError += SocketError;
            m_btCode = 0;
            m_sLoginAccount = "";
            m_sLoginPasswd = "";
            m_nCertification = 0;
            m_sCharName = "";
            m_ConnectionStep = TConnectionStep.cnsConnect;
            m_ConnectionStatus = TConnectionStatus.cns_Success;
            m_boSendLogin = false;
            m_boLogin = false;
            m_boNewAccount = false;
            m_dwConnectTick = HUtil32.GetTickCount();
            FNotifyEvent = null;
            m_dwNotifyEventTick = HUtil32.GetTickCount();
            m_ChrArr = new TSelChar[2];
        }

        private void NewAccount()
        {
            m_ConnectionStep = TConnectionStep.cnsNewAccount;
            SendNewAccount(m_sLoginAccount, m_sLoginPasswd);
        }

        private void NewChr()
        {
            m_ConnectionStep = TConnectionStep.cnsNewChr;
            SelectChrCreateNewChr(m_sCharName);
        }

        private void SocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            if (m_ConnectionStep == TConnectionStep.cnsConnect)
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
            else if (m_ConnectionStep == TConnectionStep.cnsQueryChr)
            {
                // Socket.SendText('#' + '+' + '!');
                //SendQueryChr();
            }
            else if (m_ConnectionStep == TConnectionStep.cnsPlay)
            {
                ClientSocket.IsConnected = true;
                SendRunLogin();
            }
        }

        private void SocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MainOutMessage($"{m_sLoginAccount}[{ClientSocket.Address}:{ClientSocket.Port}] 断开链接");
        }

        private void SocketRead(object sender, DSCClientDataInEventArgs e)
        {
            string sData = e.ReceiveText;
            var nIdx = sData.IndexOf("*", StringComparison.Ordinal);
            if (nIdx > 0)
            {
                var sData2 = sData.Substring(0, nIdx - 1);
                sData = sData2 + sData.Substring(nIdx, sData.Length);
                ClientSocket.SendText("*");
            }
            ClientManager.AddPacket(SessionId, e.Buff);
        }

        private void SocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    Console.WriteLine("游戏[" + ClientSocket.Address + ":" + ClientSocket.Port + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    Console.WriteLine("游戏[" + ClientSocket.Address + ":" + ClientSocket.Port + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    Console.WriteLine("游戏[" + ClientSocket.Address + ":" + ClientSocket.Port + "]链接超时...");
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
            SendSocket(EDcode.EncodeMessage(DefMsg));
        }

        private void SendNewAccount(string sAccount, string sPassword)
        {
            MainOutMessage($"[{m_sLoginAccount}] 创建帐号");
            m_ConnectionStep = TConnectionStep.cnsNewAccount;
            UserFullEntry ue = new UserFullEntry();
            ue.UserEntry.sAccount = sAccount;
            ue.UserEntry.sPassword = sPassword;
            ue.UserEntry.sUserName = sAccount;
            ue.UserEntry.sSSNo = "650101-1455111";
            ue.UserEntry.sQuiz = sAccount;
            ue.UserEntry.sAnswer = sAccount;
            ue.UserEntry.sPhone = "";
            ue.UserEntry.sEMail = "";
            ue.UserEntryAdd.sQuiz2 = sAccount;
            ue.UserEntryAdd.sAnswer2 = sAccount;
            ue.UserEntryAdd.sBirthDay = "1978/01/01";
            ue.UserEntryAdd.sMobilePhone = "";
            ue.UserEntryAdd.sMemo = "";
            ue.UserEntryAdd.sMemo2 = "";
            var Msg = Grobal2.MakeDefaultMsg(Grobal2.CM_ADDNEWUSER, 0, 0, 0, 0);
            SendSocket(EDcode.EncodeMessage(Msg) + EDcode.EncodeBuffer(ue));
        }

        private void SelectChrCreateNewChr(string sCharName)
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
            byte sJob = (byte)RandomNumber.GetInstance().Random(2);
            byte sSex = (byte)RandomNumber.GetInstance().Random(1);
            SendNewChr(m_sLoginAccount, sCharName, sHair, sJob, sSex);
        }

        private void SendSelChr(string sCharName)
        {
            MainOutMessage($"[{m_sLoginAccount}] 选择人物：{sCharName}");
            m_ConnectionStep = TConnectionStep.cnsSelChr;
            m_sCharName = sCharName;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_SELCHR, 0, 0, 0, 0);
            SendSocket(EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(m_sLoginAccount + "/" + sCharName));
        }

        private void SendLogin(string sAccount, string sPassword)
        {
            MainOutMessage($"[{m_sLoginAccount}] 开始登录");
            m_ConnectionStep = TConnectionStep.cnsLogin;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_IDPASSWORD, 0, 0, 0, 0);
            SendSocket(EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sAccount + "/" + sPassword));
            m_boSendLogin = true;
        }

        private void SendNewChr(string sAccount, string sChrName, byte sHair, byte sJob, byte sSex)
        {
            MainOutMessage($"[{m_sLoginAccount}] 创建人物：{sChrName}");
            m_ConnectionStep = TConnectionStep.cnsNewChr;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sAccount + "/" + sChrName + "/" + sHair + "/" + sJob + "/" + sSex));
        }

        private void SendQueryChr()
        {
            MainOutMessage($"[{m_sLoginAccount}] 查询人物");
            m_ConnectionStep = TConnectionStep.cnsQueryChr;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(m_sLoginAccount + "/" + m_nCertification.ToString()));
        }

        private void SendSelectServer(string sServerName)
        {
            MainOutMessage($"[{m_sLoginAccount}] 选择服务器：{sServerName}");
            m_ConnectionStep = TConnectionStep.cnsSelServer;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_SELECTSERVER, 0, 0, 0, 0);
            SendSocket(EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sServerName));
        }

        private void SendRunLogin()
        {
            MainOutMessage($"[{m_sLoginAccount}] 进入游戏");
            m_ConnectionStep = TConnectionStep.cnsPlay;
            var sSendMsg = string.Format("**{0}/{1}/{2}/{3}/{4}", new object[] { m_sLoginAccount, m_sCharName, m_nCertification, Grobal2.CLIENT_VERSION_NUMBER, 0 });
            SendSocket(EDcode.EncodeString(sSendMsg));
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
            string sText = EDcode.DeCodeString(sData);
            var sRunPort = HUtil32.GetValidStr3(sText, ref m_sRunServerAddr, new[] { "/" });
            m_nRunServerPort = Convert.ToInt32(sRunPort);
            //ClientSocket.Disconnect();
            m_ConnectionStep = TConnectionStep.cnsPlay;
            MainOutMessage($"[{m_sLoginAccount}] 准备进入游戏");
            //ClientSocket.ClientType = ClientSocket.ctNonBlocking;
            ClientSocket.Address = m_sRunServerAddr;
            ClientSocket.Port = m_nRunServerPort;
            ClientSocket.Connect();
            //ClientSocket.Active = true;
        }

        private void ClientStartPlayFail()
        {
            MainOutMessage($"[{m_sLoginAccount}] 此服务器满员！");
        }

        private void ClientVersionFail()
        {
            // MainOutMessage(Format('[%s] 游戏程序版本不正确，请下载最新版本游戏程序！', [m_sLoginAccount]));
        }

        private void ClientGetSendNotice(string sData)
        {
            MainOutMessage($"[{m_sLoginAccount}] 发送公告");
            SendClientMessage(Grobal2.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
        }

        private void ClientGetUserLogin(TCmdPack DefMsg, string sData)
        {
            m_boLogin = true;
            m_ConnectionStep = TConnectionStep.cnsPlay;
            m_ConnectionStatus = TConnectionStatus.cns_Success;
            MainOutMessage($"[{m_sLoginAccount}] 成功进入游戏");
            MainOutMessage("-----------------------------------------------");
        }

        public void ClientLoginSay()
        {
            m_dwSayTick = HUtil32.GetTickCount();
            var Msg = Grobal2.MakeDefaultMsg(Grobal2.CM_SAY, 0, 0, 0, 0);
            SendSocket(EDcode.EncodeMessage(Msg) + EDcode.EncodeString("压力测试工具."));
        }

        private void ClientGetAbility(TCmdPack DefMsg, string sData)
        {
            m_nGold = DefMsg.Recog;
            m_btJob = (byte)DefMsg.Param;
            m_nGameGold = HUtil32.MakeLong(DefMsg.Tag, DefMsg.Series);
            var buff = EDcode.DecodeBuffer(sData);
            m_Abil = new TAbility(buff);
        }

        private void ClientGetWinExp(TCmdPack DefMsg)
        {
            m_Abil.Exp = DefMsg.Recog;
        }

        private void ClientGetLevelUp(TCmdPack DefMsg)
        {
            m_Abil.Level = (ushort)HUtil32.MakeLong(DefMsg.Param, DefMsg.Tag);
        }

        private void ClientQueryChrFail(int nFailCode)
        {
            m_ConnectionStatus = TConnectionStatus.cns_Failure;
        }

        private void ClientNewChrFail(int nFailCode)
        {
            m_ConnectionStatus = TConnectionStatus.cns_Failure;
            Close();
            switch (nFailCode)
            {
                case 0:
                    MainOutMessage($"[{m_sLoginAccount}] [错误信息] 输入的角色名称包含非法字符！ 错误代码 = 0");
                    break;
                case 2:
                    MainOutMessage($"[{m_sLoginAccount}] [错误信息] 创建角色名称已被其他人使用！ 错误代码 = 2");
                    break;
                case 3:
                    MainOutMessage($"[{m_sLoginAccount}] [错误信息] 您只能创建二个游戏角色！ 错误代码 = 3");
                    break;
                case 4:
                    MainOutMessage($"[{m_sLoginAccount}] [错误信息] 创建角色时出现错误！ 错误代码 = 4");
                    break;
                default:
                    MainOutMessage($"[{m_sLoginAccount}] [错误信息] 创建角色时出现未知错误！");
                    break;
            }
        }

        private void ClientNewIDFail(int nFailCode)
        {
            if (nFailCode != 0)
            {
                m_ConnectionStatus = TConnectionStatus.cns_Failure;
                Close();
            }
            switch (nFailCode)
            {
                case 0:
                    MainOutMessage(string.Format("[{0}] 帐号已被其他的玩家使用了。请选择其它帐号名注册。", m_sLoginAccount));
                    break;
                case 1:
                    MainOutMessage($"[{m_sLoginAccount}] 验证码输入错误，请重新输入！！！");
                    break;
                case -2:
                    MainOutMessage($"[{m_sLoginAccount}] 此帐号名被禁止使用！");
                    break;
                default:
                    MainOutMessage(string.Format("[{0}] 帐号创建失败，请确认帐号是否包括空格、及非法字符！Code: " + nFailCode.ToString(), m_sLoginAccount));
                    break;
            }
        }

        private void ClientGetPasswdSuccess(string sData)
        {
            string sSelChrPort = string.Empty;
            string sCertification = string.Empty;
            MainOutMessage($"[{m_sLoginAccount}] 帐号登录成功！");
            var sText = EDcode.DeCodeString(sData);
            sText = HUtil32.GetValidStr3(sText, ref m_sSelChrAddr, new[] { "/" });
            sText = HUtil32.GetValidStr3(sText, ref sSelChrPort, new[] { "/" });
            sText = HUtil32.GetValidStr3(sText, ref sCertification, new[] { "/" });
            m_nCertification = Convert.ToInt32(sCertification);
            m_nSelChrPort = Convert.ToInt32(sSelChrPort);
            //ClientSocket.Disconnect();
            m_ConnectionStep = TConnectionStep.cnsQueryChr;
            ClientSocket.Address = m_sSelChrAddr;
            ClientSocket.Port = m_nSelChrPort;
            ClientSocket.Connect();
            //ClientSocket.Active = true;
            // ClientSocket.Socket.SendText('#' + '+' + '!');
            SendQueryChr();
        }

        private void ClientNewIdSuccess(string sData)
        {
            SendLogin(m_sLoginAccount, m_sLoginPasswd);
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
            string sName = string.Empty;
            string sJob = string.Empty;
            string sHair = string.Empty;
            string sLevel = string.Empty;
            string sSex = string.Empty;
            string sText = EDcode.DeCodeString(sData);
            int nChrCount = 0;
            int nSelect = 0;
            for (var i = m_ChrArr.GetLowerBound(0); i <= m_ChrArr.GetUpperBound(0); i++)
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
                SendLogin(m_sLoginAccount, m_sLoginPasswd);
                MainOutMessage($"[{m_sLoginAccount}] 此帐号已经登录或被异常锁定，请稍候再登录！");
            }
            else
            {
                m_ConnectionStatus = TConnectionStatus.cns_Failure;
                switch (nFailCode)
                {
                    case -1:
                        MainOutMessage($"[{m_sLoginAccount}] 密码错误！！");
                        break;
                    case -2:
                        MainOutMessage($"[{m_sLoginAccount}] 密码输入错误超过3次，此帐号被暂时锁定，请稍候再登录！");
                        break;
                    case -3:
                        MainOutMessage($"[{m_sLoginAccount}] 此帐号已经登录或被异常锁定，请稍候再登录！");
                        break;
                    case -4:
                        MainOutMessage($"[{m_sLoginAccount}] 这个帐号访问失败！请使用其他帐号登录，或者申请付费注册。");
                        break;
                    case -5:
                        MainOutMessage($"[{m_sLoginAccount}] 这个帐号被锁定！");
                        break;
                    default:
                        MainOutMessage($"[{m_sLoginAccount}] 此帐号不存在或出现未知错误！！");
                        break;
                }
                m_boSendLogin = false;
                Close();
            }
        }

        private void ClientGetServerName(TCmdPack DefMsg, string sBody)
        {
            string sServerName = string.Empty;
            string sServerStatus = string.Empty;
            sBody = EDcode.DeCodeString(sBody);
            int nCount = HUtil32._MIN(6, DefMsg.Series);
            for (var i = 0; i < nCount; i++)
            {
                sBody = HUtil32.GetValidStr3(sBody, ref sServerName, new[] { "/" });
                sBody = HUtil32.GetValidStr3(sBody, ref sServerStatus, new[] { "/" });
                if (sServerName == m_sServerName)
                {
                    SendSelectServer(sServerName);
                    return;
                }
            }
            if (nCount == 1)
            {
                m_sServerName = sServerName;
                SendSelectServer(sServerName);
            }
        }

        private void ClientGetPasswordOK(string sData)
        {
            string sServerName = string.Empty;
            MainOutMessage($"[{m_sLoginAccount}] 帐号登录成功！");
            string sText = EDcode.DeCodeString(sData);
            HUtil32.GetValidStr3(sText, ref sServerName, new[] { "/" });
            SendSelectServer(sServerName);
        }

        private void Close()
        {
            //ClientSocket.Disconnect();
        }

        private void Login()
        {
            if (m_ConnectionStep == TConnectionStep.cnsConnect && (FNotifyEvent == null) && !ClientSocket.IsConnected)
            {
                if ((m_ConnectionStatus == TConnectionStatus.cns_Success) && (HUtil32.GetTickCount() > m_dwConnectTick))
                {
                    m_dwConnectTick = HUtil32.GetTickCount();
                    try
                    {
                        ClientSocket.Connect();
                    }
                    catch
                    {
                        m_ConnectionStatus = TConnectionStatus.cns_Failure;
                    }
                }
            }
        }

        public void ProcessPacket(byte[] reviceBuffer)
        {
            m_boTimerMainBusy = true;
            try
            {
                var m_sSockText = HUtil32.GetString(reviceBuffer, 0, reviceBuffer.Length);
                if (!string.IsNullOrEmpty(m_sSockText))
                {
                    while (m_sSockText.Length >= 2)
                    {
                        if (m_boMapMovingWait)
                        {
                            break;
                        }
                        if (m_sSockText.IndexOf("!", StringComparison.Ordinal) <= 0)
                        {
                            break;
                        }
                        string sData = string.Empty;
                        m_sSockText = HUtil32.ArrestStringEx(m_sSockText, "#", "!", ref sData);
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
            string sDefMsg = sDataBlock.Substring(0, Grobal2.DEFBLOCKSIZE);
            string sBody = sDataBlock.Substring(Grobal2.DEFBLOCKSIZE, sDataBlock.Length - Grobal2.DEFBLOCKSIZE);
            var DefMsg = EDcode.DecodeMessage(sDefMsg);
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