using MakePlayer.Cliens;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace MakePlayer.Scenes.Scene
{
    public class LoginScene : SceneBase
    {
        private readonly ScoketClient _clientSocket;
        private readonly PlayClient play;
        private readonly string LoginAddr;
        private readonly int LoginPort;

        public LoginScene(PlayClient playClient, string serverAdd, int port)
        {
            play = playClient;
            LoginAddr = serverAdd;
            LoginPort = port;
            _clientSocket = new ScoketClient();
            _clientSocket.OnConnected += LoginSocketConnect;
            _clientSocket.OnDisconnected += LoginSocketDisconnect;
            _clientSocket.OnReceivedData += LoginSocketRead;
            _clientSocket.OnError += LoginSocketError;
        }

        public override void OpenScene()
        {
            ConnectionStatus = ConnectionStatus.Failure;
            _clientSocket.Connect(LoginAddr, LoginPort);
            SetNotifyEvent(Login, RandomNumber.GetInstance().Random(1000, 3000));
        }

        public override void CloseScene()
        {
            SetNotifyEvent(CloseSocket, RandomNumber.GetInstance().Random(1000, 3000));
        }

        public void PassWdFail()
        {
            play.ConnectionStep = ConnectionStep.Connect;
        }

        public override void PlayScene()
        {

        }

        internal override void ProcessPacket(CommandMessage command, string sBody)
        {
            switch (command.Ident)
            {
                case Messages.SM_NEWID_SUCCESS:
                    ClientSendLoginMessage();
                    break;
                case Messages.SM_SELECTSERVER_OK:
                    ClientGetPasswdSuccess(sBody);
                    break;
                case Messages.SM_NEWID_FAIL:
                    ClientNewIdFail(command.Recog);
                    break;
                case Messages.SM_PASSWD_FAIL:
                    ClientLoginFail(command.Recog);
                    break;
                case Messages.SM_PASSOK_SELECTSERVER:
                    ClientGetPasswordOk(command, sBody);
                    break;
            }
            base.ProcessPacket(command, sBody);
        }

        public void Login()
        {
            if (ConnectionStatus == ConnectionStatus.Failure && HUtil32.GetTickCount() > play.RunTick)
            {
                play.RunTick = HUtil32.GetTickCount();
                try
                {
                    _clientSocket.Connect();
                    ConnectionStatus = ConnectionStatus.Connect;
                }
                catch
                {
                    play.RunTick = HUtil32.GetTickCount() + 10000;
                    ConnectionStatus = ConnectionStatus.Failure;
                }
            }
        }

        private void ClientLoginFail(int nFailCode)
        {
            switch (nFailCode)
            {
                case -1:
                    MainOutMessage("密码错误！！");
                    break;
                case -2:
                    MainOutMessage("密码输入错误超过3次，此帐号被暂时锁定，请稍候再登录！");
                    break;
                case -3:
                    SendLogin(play.LoginId, play.LoginPasswd);
                    MainOutMessage("此帐号已经登录或被异常锁定，请稍候再登录！");
                    break;
                case -4:
                    MainOutMessage("这个帐号访问失败！请使用其他帐号登录，或者申请付费注册。");
                    break;
                case -5:
                    MainOutMessage("这个帐号被锁定！");
                    break;
                default:
                    MainOutMessage("此帐号不存在或出现未知错误！！");
                    break;
            }
            //Close();
        }

        /// <summary>
        /// 账号注册
        /// </summary>
        private void NewAccount()
        {
            play.ConnectionStep = ConnectionStep.NewAccount;
            SendNewAccount(play.LoginId, play.LoginPasswd);
        }

        /// <summary>
        /// 账号注册成功
        /// </summary>
        private void ClientSendLoginMessage()
        {
            SendLogin(play.LoginId, play.LoginPasswd);
        }

        private void ClientNewIdFail(int nFailCode)
        {
            if (nFailCode != 0)
            {
                //ConnectionStatus = ConnectionStatus.Failure;
                //Close();
            }
            switch (nFailCode)
            {
                case 0:
                    MainOutMessage("帐号已被其他的玩家使用了。请选择其它帐号名注册，尝试使用该账号登陆游戏。");
                    SendLogin(play.LoginId, play.LoginPasswd);
                    break;
                case 1:
                    MainOutMessage("验证码输入错误，请重新输入！！！");
                    break;
                case -2:
                    MainOutMessage("此帐号名被禁止使用！");
                    break;
                default:
                    MainOutMessage($"帐号创建失败，请确认帐号是否包括空格、及非法字符！Code: {nFailCode}");
                    break;
            }
        }

        private void SendNewAccount(string sAccount, string sPassword)
        {
            MainOutMessage("创建帐号");
            play.ConnectionStep = ConnectionStep.NewAccount;
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
            MainOutMessage("开始登陆");
            var msg = Messages.MakeMessage(Messages.CM_IDPASSWORD, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(uid + "/" + passwd));
        }

        private void ClientGetPasswordOk(CommandMessage msg, string sBody)
        {
            var g_wAvailIDDay = HUtil32.LoWord(msg.Recog);
            var g_wAvailIDHour = HUtil32.HiWord(msg.Recog);
            var g_wAvailIPDay = msg.Param;
            var g_wAvailIPHour = msg.Tag;
            if (g_wAvailIDHour % 60 > 0)
            {
                MainOutMessage("个人帐户的期限: 剩余 " + g_wAvailIDHour / 60 + " 小时 " + g_wAvailIDHour % 60 + " 分钟.");
            }
            else if (g_wAvailIDHour > 0)
            {
                MainOutMessage("个人帐户的期限: 剩余 " + g_wAvailIDHour + " 分钟.");
            }
            else
            {
                MainOutMessage("帐号登录成功！");
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
                if (sServerName == play.ServerName)
                {
                    SendSelectServer(sServerName);
                    return;
                }
            }
            if (nCount == 1)
            {
                play.ServerName = sServerName;
                SendSelectServer(sServerName);
            }
        }

        private void SendSelectServer(string svname)
        {
            MainOutMessage($"选择服务器：{svname}");
            play.ConnectionStep = ConnectionStep.SelServer;
            var defMsg = Messages.MakeMessage(Messages.CM_SELECTSERVER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(svname));
        }

        /// <summary>
        /// 登陆成功
        /// </summary>
        private void ClientGetPasswdSuccess(string body)
        {
            var runaddr = string.Empty;
            var runport = string.Empty;
            var certifystr = string.Empty;
            var str = EDCode.DeCodeString(body);
            str = HUtil32.GetValidStr3(str, ref runaddr, HUtil32.Backslash);
            str = HUtil32.GetValidStr3(str, ref runport, HUtil32.Backslash);
            str = HUtil32.GetValidStr3(str, ref certifystr, HUtil32.Backslash);
            play.Certification = HUtil32.StrToInt(certifystr, 0);
            play.SelChrAddr = runaddr;
            play.SelChrPort = HUtil32.StrToInt(runport, 0);
            MainOutMessage("帐号登录成功！");
            play.DScreen.ChangeScene(SceneType.SelectChr);
        }

        private void SendSocket(string sendstr)
        {
            if (_clientSocket.IsConnected)
            {
                _clientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                MainOutMessage($"Socket Close: {_clientSocket.RemoteEndPoint}");
            }
        }

        private void CloseSocket()
        {
            _clientSocket.Disconnect(false);//断开登录网关链接
            MainOutMessage("主动断开");
        }

        #region Socket Events

        private void LoginSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            ConnectionStatus = ConnectionStatus.Success;
            if (play.CreateAccount)
            {
                SetNotifyEvent(NewAccount, RandomNumber.GetInstance().Random(1000, 3000));
            }
            else
            {
                SetNotifyEvent(ClientSendLoginMessage, RandomNumber.GetInstance().Random(1000, 3000));
            }
            MainOutMessage($"连接登陆服务:[{e.RemoteEndPoint}]成功...");
        }

        private void LoginSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MainOutMessage($"登陆服务[{e.RemoteEndPoint}]连接已关闭...");
        }

        private void LoginSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    MainOutMessage($"登陆服务[{_clientSocket.RemoteEndPoint}]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    MainOutMessage($"登陆服务[{_clientSocket.RemoteEndPoint}]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    MainOutMessage($"登陆服务[{_clientSocket.RemoteEndPoint}]链接超时...");
                    break;
            }
        }

        private void LoginSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            if (e.BuffLen > 0)
            {
                ClientManager.AddPacket(play.SessionId, e.Buff);
            }
        }

        #endregion
    }
}