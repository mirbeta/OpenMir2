using System;
using System.Net;
using BotSrv.Player;
using NLog;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace BotSrv.Scenes.Scene
{
    /// <summary>
    /// 登陆场景
    /// </summary>
    public class LoginScene : SceneBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ScoketClient _clientSocket;

        public LoginScene(RobotPlayer robotClient) : base(SceneType.Login, robotClient)
        {
            _clientSocket = new ScoketClient();
            _clientSocket.OnConnected += CSocketConnect;
            _clientSocket.OnDisconnected += CSocketDisconnect;
            _clientSocket.OnReceivedData += CSocketRead;
            _clientSocket.OnError += CSocketError;
        }

        public override void OpenScene()
        {
            _clientSocket.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(MShare.g_sGameIPaddr), MShare.g_nGamePort);
            SetNotifyEvent(Login, 1000);
        }

        public void Login()
        {
            if (ConnectionStep == ConnectionStep.Connect)
            {
                if ((RobotClient.ConnectionStatus == ConnectionStatus.Failure) && (HUtil32.GetTickCount() > RobotClient.ConnectTick))
                {
                    RobotClient.ConnectTick = HUtil32.GetTickCount();
                    try
                    {
                        _clientSocket.Connect();
                        RobotClient.ConnectionStatus = ConnectionStatus.Connect;
                    }
                    catch
                    {
                        RobotClient.ConnectionStatus = ConnectionStatus.Failure;
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
            ConnectionStep = ConnectionStep.Connect;
        }

        public override void PlayScene()
        {

        }

        /// <summary>
        /// 账号注册
        /// </summary>
        private void NewAccount()
        {
            ConnectionStep = ConnectionStep.NewAccount;
            SendNewAccount(RobotClient.LoginId, RobotClient.LoginPasswd);
        }

        /// <summary>
        /// 账号注册成功
        /// </summary>
        public void ClientNewIdSuccess()
        {
            SendLogin(RobotClient.LoginId, RobotClient.LoginPasswd);
        }

        private void SendNewAccount(string sAccount, string sPassword)
        {
            logger.Info("创建帐号");
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

        private void SendLogin(string uid, string passwd)
        {
            logger.Info("开始登陆");
            RobotClient.LoginId = uid;
            RobotClient.LoginPasswd = passwd;
            var msg = Messages.MakeMessage(Messages.CM_IDPASSWORD, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(uid + "/" + passwd));
            MShare.SendLogin = true;
        }

        public void ClientGetPasswordOk(CommandMessage msg, string sBody)
        {
            MShare.g_wAvailIDDay = HUtil32.LoWord(msg.Recog);
            MShare.g_wAvailIDHour = HUtil32.HiWord(msg.Recog);
            MShare.g_wAvailIPDay = msg.Param;
            MShare.g_wAvailIPHour = msg.Tag;
            if ((MShare.g_wAvailIDHour % 60) > 0)
            {
                logger.Info("个人帐户的期限: 剩余 " + (MShare.g_wAvailIDHour / 60) + " 小时 " + (MShare.g_wAvailIDHour % 60) + " 分钟.");
            }
            else if (MShare.g_wAvailIDHour > 0)
            {
                logger.Info("个人帐户的期限: 剩余 " + MShare.g_wAvailIDHour + " 分钟.");
            }
            else
            {
                logger.Info("帐号登录成功！");
            }
            var sServerName = string.Empty;
            var sText = EDCode.DeCodeString(sBody);
            HUtil32.GetValidStr3(sText, ref sServerName, "/");
            ClientGetSelectServer();
            SendSelectServer(sServerName);
        }

        public void ClientGetSelectServer()
        {

        }

        private void SendSelectServer(string svname)
        {
            logger.Info($"选择服务器：{svname}");
            ConnectionStep = ConnectionStep.SelServer;
            var defMsg = Messages.MakeMessage(Messages.CM_SELECTSERVER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(svname));
        }

        /// <summary>
        /// 登陆成功
        /// </summary>
        public void ClientGetPasswdSuccess(string body)
        {
            var runaddr = string.Empty;
            var runport = string.Empty;
            var certifystr = string.Empty;
            var str = EDCode.DeCodeString(body);
            str = HUtil32.GetValidStr3(str, ref runaddr, HUtil32.Backslash);
            str = HUtil32.GetValidStr3(str, ref runport, HUtil32.Backslash);
            str = HUtil32.GetValidStr3(str, ref certifystr, HUtil32.Backslash);
            RobotClient.Certification = HUtil32.StrToInt(certifystr, 0);
            MShare.SelChrAddr = runaddr;
            MShare.SelChrPort = HUtil32.StrToInt(runport, 0);
            ConnectionStep = ConnectionStep.QueryChr;
        }

        private void SendSocket(string sendstr)
        {
            if (_clientSocket.IsConnected)
            {
                _clientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                logger.Warn($"Socket Close: {_clientSocket.RemoteEndPoint}");
            }
        }

        private void CloseSocket()
        {
            _clientSocket.Disconnect();//断开登录网关链接
            logger.Info("主动断开");
        }

        #region Socket Events

        private void CSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.ServerConnected = true;
            if (ConnectionStep == ConnectionStep.Connect)
            {
                if (RobotClient.NewAccount)
                {
                    SetNotifyEvent(NewAccount, 3000);
                }
                else
                {
                    ClientNewIdSuccess();
                }
            }
            if (MShare.ConnectionStep == ConnectionStep.Login)
            {
                RobotClient.DScreen.ChangeScene(SceneType.Login);
            }
        }

        private void CSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.ServerConnected = false;
            if (MShare.g_SoftClosed)
            {
                MShare.g_SoftClosed = false;
            }
            else if ((RobotClient.DScreen.CurrentScene == RobotClient.LoginScene) && !MShare.SendLogin)
            {
                logger.Info("游戏连接已关闭...");
            }
        }

        private void CSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    logger.Warn($"游戏服务器[{_clientSocket.RemoteEndPoint}]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    logger.Warn($"游戏服务器[{_clientSocket.RemoteEndPoint}]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    logger.Warn($"游戏服务器[{_clientSocket.RemoteEndPoint}]链接超时...");
                    break;
            }
        }

        private void CSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var sData = HUtil32.GetString(e.Buff, 0, e.BuffLen);
            if (!string.IsNullOrEmpty(sData))
            {
                BotShare.ClientMgr.AddPacket(RobotClient.SessionId, sData);
            }
        }

        #endregion
    }
}