using System.Net;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace MakePlayer.Cliens
{
    public class PlayClient
    {
        public string SessionId;
        public int ConnectTick;
        public string LoginAccount;
        public string LoginPasswd;
        public int Certification;
        public string ChrName;
        /// <summary>
        /// 当前游戏网络连接步骤
        /// </summary>
        public ConnectionStep ConnectionStep;
        public ConnectionStatus ConnectionStatus;
        public string ServerName = string.Empty;
        public SelChar[] ChrArr;
        public long NotifyEventTick;
        public byte SendNum;
        public bool CreateAccount;
        public Ability PlayAbil;
        public bool IsLogin;
        public long SayTick;
        private Action? _notifyEvent;
        public readonly ScoketClient ClientSocket;

        public PlayClient()
        {
            SessionId = string.Empty;
            ClientSocket = new ScoketClient();
            ClientSocket.OnConnected += SocketConnect;
            ClientSocket.OnDisconnected += SocketDisconnect;
            ClientSocket.OnReceivedData += SocketRead;
            ClientSocket.OnError += SocketError;
            SendNum = 0;
            Certification = 0;
            ConnectionStep = ConnectionStep.Connect;
            ConnectionStatus = ConnectionStatus.Success;
            IsLogin = false;
            CreateAccount = false;
            ConnectTick = HUtil32.GetTickCount();
            _notifyEvent = null;
            NotifyEventTick = HUtil32.GetTickCount();
            ChrArr = new SelChar[2];
        }

        private void NewAccount()
        {
            ConnectionStep = ConnectionStep.NewAccount;
            SendNewAccount(LoginAccount, LoginPasswd);
        }
        
        private void SocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            if (ConnectionStep == ConnectionStep.Connect)
            {
                if (CreateAccount)
                {
                    SetNotifyEvent(NewAccount, 6000);
                }
                else
                {
                    ClientNewIdSuccess("");
                }
            }
            else if (ConnectionStep == ConnectionStep.QueryChr)
            {
                // Socket.SendText('#' + '+' + '!');
                //SendQueryChr();
            }
            else if (ConnectionStep == ConnectionStep.Play)
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
                var sData2 = sData[..(nIdx - 1)];
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
                var sSendText = "#" + SendNum + sText + "!";
                ClientSocket.SendText(sSendText);
                SendNum++;
                if (SendNum >= 10)
                {
                    SendNum = 1;
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
            ConnectionStep = ConnectionStep.NewAccount;
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
        
        private void SendLogin(string sAccount, string sPassword)
        {
            MainOutMessage($"[{LoginAccount}] 开始登录");
            ConnectionStep = ConnectionStep.Login;
            var defMsg = Messages.MakeMessage(Messages.CM_IDPASSWORD, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sAccount + "/" + sPassword));
        }

        private void SendNewChr(string sAccount, string sChrName, byte sHair, byte sJob, byte sSex)
        {
            MainOutMessage($"[{LoginAccount}] 创建人物：{sChrName}");
            ConnectionStep = ConnectionStep.NewChr;
            var defMsg = Messages.MakeMessage(Messages.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sAccount + "/" + sChrName + "/" + sHair + "/" + sJob + "/" + sSex));
        }

        private void SendQueryChr()
        {
            MainOutMessage($"[{LoginAccount}] 查询人物");
            ConnectionStep = ConnectionStep.QueryChr;
            var defMsg = Messages.MakeMessage(Messages.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(LoginAccount + "/" + Certification));
        }

        private void SendSelectServer(string sServerName)
        {
            MainOutMessage($"[{LoginAccount}] 选择服务器：{sServerName}");
            ConnectionStep = ConnectionStep.SelServer;
            var defMsg = Messages.MakeMessage(Messages.CM_SELECTSERVER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sServerName));
        }

        private void SendRunLogin()
        {
            MainOutMessage($"[{LoginAccount}] 进入游戏");
            ConnectionStep = ConnectionStep.Play;
            var sSendMsg = $"**{LoginAccount}/{ChrName}/{Certification}/{Grobal2.CLIENT_VERSION_NUMBER}/{2022080300}";
            SendSocket(EDCode.EncodeString(sSendMsg));
        }

        private void DoNotifyEvent()
        {
            if (_notifyEvent != null)
            {
                if (HUtil32.GetTickCount() > NotifyEventTick)
                {
                    _notifyEvent();
                    _notifyEvent = null;
                }
            }
        }

        private void SetNotifyEvent(Action aNotifyEvent, int nTime)
        {
            NotifyEventTick = HUtil32.GetTickCount() + nTime;
            _notifyEvent = aNotifyEvent;
        }

        private void ClientGetStartPlay(string sData)
        {
            var runServerAddr = string.Empty;
            var runServerPort = 0;
            var sText = EDCode.DeCodeString(sData);
            var sRunPort = HUtil32.GetValidStr3(sText, ref runServerAddr, '/');
            runServerPort = Convert.ToInt32(sRunPort);
            ConnectionStep = ConnectionStep.Play;
            MainOutMessage($"[{LoginAccount}] 准备进入游戏");
            ClientSocket.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(runServerAddr), runServerPort);
            ClientSocket.Connect();
        }
        
        private void ClientGetSendNotice(string sData)
        {
            MainOutMessage($"[{LoginAccount}] 发送公告");
            SendClientMessage(Messages.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
        }

        private void ClientGetUserLogin(CommandMessage defMsg, string sData)
        {
            IsLogin = true;
            ConnectionStep = ConnectionStep.Play;
            ConnectionStatus = ConnectionStatus.Success;
            MainOutMessage($"[{LoginAccount}] 成功进入游戏");
            MainOutMessage("-----------------------------------------------");
        }

        public void ClientLoginSay(string message)
        {
            SayTick = HUtil32.GetTickCount();
            var msg = Messages.MakeMessage(Messages.CM_SAY, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(message));
        }

        private void ClientGetAbility(CommandMessage defMsg, string sData)
        {
            var gold = defMsg.Recog;
            var job = (byte)defMsg.Param;
            var gameGold = HUtil32.MakeLong(defMsg.Tag, defMsg.Series);
            var buff = EDCode.DecodeBuffer(sData);
            PlayAbil = ClientPacket.ToPacket<Ability>(buff);
        }

        private void ClientGetWinExp(CommandMessage defMsg)
        {
            PlayAbil.Exp = defMsg.Recog;
        }

        private void ClientGetLevelUp(CommandMessage defMsg)
        {
            PlayAbil.Level = (byte)HUtil32.MakeLong(defMsg.Param, defMsg.Tag);
        }
        
        private void ClientNewIdSuccess(string sData)
        {
            SendLogin(LoginAccount, LoginPasswd);
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
            if (ConnectionStep == ConnectionStep.Connect && (_notifyEvent == null) && !ClientSocket.IsConnected)
            {
                if ((ConnectionStatus == ConnectionStatus.Success) && (HUtil32.GetTickCount() > ConnectTick))
                {
                    ConnectTick = HUtil32.GetTickCount();
                    try
                    {
                        ClientSocket.Connect();
                    }
                    catch
                    {
                        ConnectionStatus = ConnectionStatus.Failure;
                    }
                }
            }
        }

        public void ProcessPacket(byte[] reviceBuffer)
        {
            var sockText = HUtil32.GetString(reviceBuffer, 0, reviceBuffer.Length);
            if (!string.IsNullOrEmpty(sockText))
            {
                while (sockText.Length >= 2)
                {
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
            var sDefMsg = sDataBlock[..Messages.DefBlockSize];
            var sBody = sDataBlock.Substring(Messages.DefBlockSize, sDataBlock.Length - Messages.DefBlockSize);
            var defMsg = EDCode.DecodePacket(sDefMsg);
            switch (defMsg.Ident)
            {
             
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
            }
        }

        public void MainOutMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}