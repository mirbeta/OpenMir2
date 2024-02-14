using MakePlayer.Cliens;
using OpenMir2;
using OpenMir2.Packets.ClientPackets;
using System.Net;
using System.Net.Sockets;
using TouchSocket.Sockets;
using TcpClient = TouchSocket.Sockets.TcpClient;

namespace MakePlayer.Scenes.Scene
{
    public class GameScene : SceneBase
    {
        private readonly TcpClient _clientSocket;
        private readonly PlayClient _play;
        private Ability PlayAbil = default;
        private byte SendNum;

        public GameScene(PlayClient playClient)
        {
            _play = playClient;
            _clientSocket = new TcpClient();
            _clientSocket.Connected += SocketConnect;
            _clientSocket.Disconnected += SocketDisconnect;
            _clientSocket.Received += SocketRead;
        }

        public override void OpenScene()
        {
            ConnectionStatus = ConnectionStatus.Failure;
            _clientSocket.Connect(new IPHost(IPAddress.Parse(_play.RunServerAddr), _play.RunServerPort));
        }

        public override void PlayScene()
        {
            if (ConnectionStatus == ConnectionStatus.Failure && HUtil32.GetTickCount() > _play.RunTick)
            {
                _play.RunTick = HUtil32.GetTickCount();
                try
                {
                    ConnectionStatus = ConnectionStatus.Connect;
                }
                catch
                {
                    _play.RunTick = HUtil32.GetTickCount() + 10000;
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
            _play.IsLogin = true;
            _play.ConnectionStep = ConnectionStep.Play;
            MainOutMessage("成功进入游戏");
            MainOutMessage("-----------------------------------------------");
        }

        private void ClientGetSendNotice(string sData)
        {
            MainOutMessage("发送公告");
            SendClientMessage(Messages.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
        }

        private void ClientGetAbility(CommandMessage defMsg, string sData)
        {
            int gold = defMsg.Recog;
            byte job = (byte)defMsg.Param;
            int gameGold = HUtil32.MakeLong(defMsg.Tag, defMsg.Series);
            byte[] buff = EDCode.DecodeBuffer(sData);
            PlayAbil = SerializerUtil.Deserialize<Ability>(buff);
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
            CommandMessage defMsg = Messages.MakeMessage(nIdent, nRecog, nParam, nTag, nSeries);
            SendSocket(EDCode.EncodeMessage(defMsg));
        }

        private void SendSocket(string sText)
        {
            if (_clientSocket.Online)
            {
                string sSendText = "#" + SendNum + sText + "!";
                _clientSocket.Send(HUtil32.GetBytes(sSendText));
                SendNum++;
                if (SendNum >= 10)
                {
                    SendNum = 1;
                }
            }
        }

        private void SendRunLogin()
        {
            MainOutMessage($"进入游戏");
            _play.ConnectionStep = ConnectionStep.Play;
            string sSendMsg = $"**{_play.LoginId}/{_play.ChrName}/{_play.Certification}/{Grobal2.ClientVersionNumber}/{2022080300}";
            SendSocket(EDCode.EncodeString(sSendMsg));
        }

        private void ClientLoginSay(string message)
        {
            _play.SayTick = HUtil32.GetTickCount();
            CommandMessage msg = Messages.MakeMessage(Messages.CM_SAY, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(message));
        }

        private Task SocketConnect(ITcpClientBase client, ConnectedEventArgs e)
        {
            ConnectionStatus = ConnectionStatus.Success;
            SetNotifyEvent(SendRunLogin, RandomNumber.GetInstance().Random(300, 5000));
            MainOutMessage($"连接游戏服务:[{client.IP}:{client.Port}]成功...");
            return Task.CompletedTask;
        }

        private Task SocketDisconnect(ITcpClientBase client, DisconnectEventArgs e)
        {
            ConnectionStatus = ConnectionStatus.Failure;
            MainOutMessage($"[{client.IP}:{client.Port}] 断开链接");
            return Task.CompletedTask;
        }

        private Task SocketRead(TcpClient client, ReceivedDataEventArgs e)
        {
            if (e.ByteBlock.Len <= 0)
            {
                return Task.CompletedTask;
            }
            var data = new byte[e.ByteBlock.Len];
            Array.Copy(e.ByteBlock.Buffer, 0, data, 0, data.Length);
            string sData = HUtil32.GetString(data, 0, data.Length);
            int nIdx = sData.IndexOf("*", StringComparison.OrdinalIgnoreCase);
            if (nIdx > 0)
            {
                //var sData2 = sData[..(nIdx - 1)];
                //sData = sData2 + sData.Substring(nIdx, sData.Length);
                _clientSocket.Send(HUtil32.GetBytes("*"));
            }
            ClientManager.AddPacket(_play.SessionId, data);
            return Task.CompletedTask;
        }

        private void SocketError(SocketError e)
        {
            switch (e)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    MainOutMessage("游戏服务[" + _clientSocket.GetIPPort() + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    MainOutMessage("游戏服务[" + _clientSocket.GetIPPort() + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    MainOutMessage("游戏服务[" + _clientSocket.GetIPPort() + "]链接超时...");
                    break;
            }
        }
    }
}