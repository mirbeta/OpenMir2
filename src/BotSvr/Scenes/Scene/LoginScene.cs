using System;
using SystemModule;
using SystemModule.Packet.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace BotSvr.Scenes.Scene
{
    /// <summary>
    /// 登陆场景
    /// </summary>
    public class LoginScene : SceneBase
    {
        private readonly ClientScoket _clientSocket;
        private readonly UserEntryAdd _mNewIdRetryAdd = null;
        private readonly ClientManager _clientManager;

        public LoginScene(RobotClient robotClient, ClientManager clientManager) : base(SceneType.stLogin, robotClient)
        {
            _clientManager = clientManager;
            _clientSocket = new ClientScoket();
            _clientSocket.OnConnected += CSocketConnect;
            _clientSocket.OnDisconnected += CSocketDisconnect;
            _clientSocket.ReceivedDatagram += CSocketRead;
            _clientSocket.OnError += CSocketError;
        }

        public override void OpenScene()
        {
            _clientSocket.Host = MShare.g_sGameIPaddr;
            _clientSocket.Port = MShare.g_nGamePort;
            SetNotifyEvent(Login, 1000);
        }

        public void Login()
        {
            if (m_ConnectionStep == TConnectionStep.cnsConnect)
            {
                if ((robotClient.m_ConnectionStatus == TConnectionStatus.cns_Failure) && (HUtil32.GetTickCount() > robotClient.m_dwConnectTick))
                {
                    robotClient.m_dwConnectTick = HUtil32.GetTickCount();
                    try
                    {
                        _clientSocket.Connect();
                        robotClient.m_ConnectionStatus = TConnectionStatus.cns_Connect;
                    }
                    catch
                    {
                        robotClient.m_ConnectionStatus = TConnectionStatus.cns_Failure;
                    }
                }
            }
        }

        public override void CloseScene()
        {
            SetNotifyEvent(CloseSocket, 1000);
        }

        public void PassWdFail()
        {
            m_ConnectionStep = TConnectionStep.cnsConnect;
        }

        public override void PlayScene()
        {

        }

        /// <summary>
        /// 账号注册
        /// </summary>
        private void NewAccount()
        {
            m_ConnectionStep = TConnectionStep.cnsNewAccount;
            SendNewAccount(robotClient.LoginID, robotClient.LoginPasswd);
        }

        /// <summary>
        /// 账号注册成功
        /// </summary>
        /// <param name="sData"></param>
        public void ClientNewIdSuccess()
        {
            SendLogin(robotClient.LoginID, robotClient.LoginPasswd);
        }

        private void SendNewAccount(string sAccount, string sPassword)
        {
            MainOutMessage("创建帐号");
            m_ConnectionStep = TConnectionStep.cnsNewAccount;
            var ue = new UserEntry();
            ue.sAccount = sAccount;
            ue.sPassword = sPassword;
            ue.sUserName = sAccount;
            ue.sSSNo = "650101-1455111";
            ue.sQuiz = sAccount;
            ue.sAnswer = sAccount;
            ue.sPhone = "";
            ue.sEMail = "";
            var ua = new UserEntryAdd();
            ua.sQuiz2 = sAccount;
            ua.sAnswer2 = sAccount;
            ua.sBirthDay = "1978/01/01";
            ua.sMobilePhone = "";
            ua.sMemo = "";
            ua.sMemo2 = "";
            var Msg = Grobal2.MakeDefaultMsg(Grobal2.CM_ADDNEWUSER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(Msg) + EDCode.EncodeBuffer(ue) + EDCode.EncodeBuffer(ua));
        }

        private void SendLogin(string uid, string passwd)
        {
            MainOutMessage("开始登陆");
            robotClient.LoginID = uid;
            robotClient.LoginPasswd = passwd;
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_IDPASSWORD, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(uid + "/" + passwd));
            MShare.g_boSendLogin = true;
        }

        public void ClientGetPasswordOk(ClientMesaagePacket msg, string sBody)
        {
            MShare.g_wAvailIDDay = HUtil32.LoWord(msg.Recog);
            MShare.g_wAvailIDHour = HUtil32.HiWord(msg.Recog);
            MShare.g_wAvailIPDay = msg.Param;
            MShare.g_wAvailIPHour = msg.Tag;
            if ((MShare.g_wAvailIDHour % 60) > 0)
            {
                MainOutMessage("个人帐户的期限: 剩余 " + (MShare.g_wAvailIDHour / 60) + " 小时 " + (MShare.g_wAvailIDHour % 60) + " 分钟.");
            }
            else if (MShare.g_wAvailIDHour > 0)
            {
                MainOutMessage("个人帐户的期限: 剩余 " + MShare.g_wAvailIDHour + " 分钟.");
            }
            else
            {
                MainOutMessage("帐号登录成功！");
            }
            var sServerName = string.Empty;
            var sText = EDCode.DeCodeString(sBody);
            HUtil32.GetValidStr3(sText, ref sServerName, new[] { "/" });
            ClientGetSelectServer();
            SendSelectServer(sServerName);
        }

        public void ClientGetSelectServer()
        {

        }

        private void SendSelectServer(string svname)
        {
            MainOutMessage($"选择服务器：{svname}");
            m_ConnectionStep = TConnectionStep.cnsSelServer;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_SELECTSERVER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(svname));
        }

        /// <summary>
        /// 登陆成功
        /// </summary>
        public void ClientGetPasswdSuccess(string body)
        {
            var runaddr = string.Empty;
            var runport = string.Empty;
            var certifystr = string.Empty;
            var Str = EDCode.DeCodeString(body);
            Str = HUtil32.GetValidStr3(Str, ref runaddr, HUtil32.Backslash);
            Str = HUtil32.GetValidStr3(Str, ref runport, HUtil32.Backslash);
            Str = HUtil32.GetValidStr3(Str, ref certifystr, HUtil32.Backslash);
            robotClient.Certification = HUtil32.StrToInt(certifystr, 0);
            MShare.g_sSelChrAddr = runaddr;
            MShare.g_nSelChrPort = HUtil32.StrToInt(runport, 0);
            m_ConnectionStep = TConnectionStep.cnsQueryChr;
        }

        private void SendSocket(string sendstr)
        {
            if (_clientSocket.IsConnected)
            {
                _clientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                MainOutMessage($"Socket Close: {_clientSocket.EndPoint}");
            }
        }

        private void CloseSocket()
        {
            _clientSocket.Disconnect();//断开登录网关链接
            MainOutMessage("主动断开");
        }

        #region Socket Events

        private void CSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.g_boServerConnected = true;
            if (m_ConnectionStep == TConnectionStep.cnsConnect)
            {
                if (robotClient.NewAccount)
                {
                    SetNotifyEvent(NewAccount, 3000);
                }
                else
                {
                    ClientNewIdSuccess();
                }
            }
            if (MShare.g_ConnectionStep == TConnectionStep.cnsLogin)
            {
                robotClient.DScreen.ChangeScene(SceneType.stLogin);
            }
        }

        private void CSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.g_boServerConnected = false;
            if (MShare.g_SoftClosed)
            {
                MShare.g_SoftClosed = false;
            }
            else if ((robotClient.DScreen.CurrentScene == robotClient.LoginScene) && !MShare.g_boSendLogin)
            {
                MainOutMessage("游戏连接已关闭...");
            }
        }

        private void CSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    Console.WriteLine($"游戏服务器[{_clientSocket.EndPoint}]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    Console.WriteLine($"游戏服务器[{_clientSocket.EndPoint}]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    Console.WriteLine($"游戏服务器[{_clientSocket.EndPoint}]链接超时...");
                    break;
            }
        }

        private void CSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var sData = HUtil32.GetString(e.Buff);
            if (!string.IsNullOrEmpty(sData))
            {
                _clientManager.AddPacket(robotClient.SessionId, sData);
            }
        }

        #endregion
    }
}