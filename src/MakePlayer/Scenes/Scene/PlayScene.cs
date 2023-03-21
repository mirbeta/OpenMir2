using MakePlayer.Cliens;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace MakePlayer.Scenes.Scene
{
    public class PlayScene : SceneBase
    {
        public readonly ScoketClient ClientSocket;
        private readonly PlayClient _play;
        public Ability PlayAbil;
        private byte SendNum;

        public PlayScene(PlayClient playClient)
        {
            _play = playClient;
            ClientSocket = new ScoketClient();
            ClientSocket.OnConnected += SocketConnect;
            ClientSocket.OnDisconnected += SocketDisconnect;
            ClientSocket.OnReceivedData += SocketRead;
            ClientSocket.OnError += SocketError;
        }

        public void Run()
        {
            if (ConnectionStatus == ConnectionStatus.Failure && HUtil32.GetTickCount() > _play.ConnectTick)
            {
                _play.ConnectTick = HUtil32.GetTickCount();
                try
                {
                    ConnectionStatus = ConnectionStatus.Connect;
                }
                catch
                {
                    _play.ConnectTick = HUtil32.GetTickCount() + 10000;
                    ConnectionStatus = ConnectionStatus.Failure;
                }
            }
            if (ConnectionStatus == ConnectionStatus.Success)
            {
                if (_play.IsLogin && (HUtil32.GetTickCount() - _play.SayTick > 3000))
                {
                    if (PlayHelper.SayMsgList.Count > 0)
                    {
                        _play.SayTick = HUtil32.GetTickCount();
                        ClientLoginSay(PlayHelper.SayMsgList[RandomNumber.GetInstance().Random(PlayHelper.SayMsgList.Count)]);
                    }
                }
            }
        }

        public override void OpenScene()
        {
            ConnectionStatus = ConnectionStatus.Failure;
            ClientSocket.Connect(_play.RunServerAddr, _play.RunServerPort);
        }

        internal override void ProcessPacket(CommandMessage command, string sBody)
        {
            switch (command.Ident)
            {
                case Messages.SM_OUTOFCONNECTION:
                case Messages.SM_NEWMAP:
                case Messages.SM_RECONNECT:
                    break;
                case Messages.SM_ABILITY:
                    ClientGetAbility(command, sBody);
                    break;
                case Messages.SM_WINEXP:
                    ClientGetWinExp(command);
                    break;
                case Messages.SM_LEVELUP:
                    ClientGetLevelUp(command);
                    break;
                case Messages.SM_SENDNOTICE:
                    ClientGetSendNotice(sBody);
                    break;
                case Messages.SM_LOGON:
                    ClientGetUserLogin(command, sBody);
                    break;
            }
            base.ProcessPacket(command, sBody);
        }

        private void ClientGetUserLogin(CommandMessage defMsg, string sData)
        {
            _play.ConnectionStep = ConnectionStep.Play;
            MainOutMessage($"成功进入游戏");
            _play.IsLogin = true;
            MainOutMessage("-----------------------------------------------");
        }

        private void ClientGetSendNotice(string sData)
        {
            MainOutMessage("发送公告");
            SendClientMessage(Messages.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
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

        private void SendClientMessage(int nIdent, int nRecog, int nParam, int nTag, int nSeries)
        {
            var defMsg = Messages.MakeMessage(nIdent, nRecog, nParam, nTag, nSeries);
            SendSocket(EDCode.EncodeMessage(defMsg));
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

        private void SocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            ConnectionStatus = ConnectionStatus.Success;
            //if (_play.ConnectionStep == ConnectionStep.Login)
            //{
            //    //if (CreateAccount)
            //    //{
            //    //    SetNotifyEvent(NewAccount, 6000);
            //    //}
            //    //else
            //    //{
            //    //    ClientNewIdSuccess("");
            //    //}
            //}
            //else if (_play.ConnectionStep == ConnectionStep.QueryChr)
            //{
            //    // Socket.SendText('#' + '+' + '!');
            //    //SendQueryChr();
            //}
            //else if (_play.ConnectionStep == ConnectionStep.Play)
            //{
            //    ClientSocket.IsConnected = true;
            //    SendRunLogin();
            //}

            SetNotifyEvent(SendRunLogin, 500);
        }

        private void SendRunLogin()
        {
            MainOutMessage($"进入游戏");
            _play.ConnectionStep = ConnectionStep.Play;
            var sSendMsg = $"**{_play.LoginId}/{_play.ChrName}/{_play.Certification}/{Grobal2.CLIENT_VERSION_NUMBER}/{2022080300}";
            SendSocket(EDCode.EncodeString(sSendMsg));
        }

        public void ClientLoginSay(string message)
        {
            _play.SayTick = HUtil32.GetTickCount();
            var msg = Messages.MakeMessage(Messages.CM_SAY, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(message));
        }

        private void SocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            ConnectionStatus = ConnectionStatus.Failure;
            MainOutMessage($"[{ClientSocket.RemoteEndPoint}] 断开链接");
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
            ClientManager.AddPacket(_play.SessionId, e.Buff);
        }

        private void SocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    MainOutMessage("游戏[" + ClientSocket.RemoteEndPoint + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    MainOutMessage("游戏[" + ClientSocket.RemoteEndPoint + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    MainOutMessage("游戏[" + ClientSocket.RemoteEndPoint + "]链接超时...");
                    break;
            }
        }
    }
}