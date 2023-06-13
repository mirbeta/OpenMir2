using BotSrv.Player;
using NLog;
using System;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.SocketComponents.AsyncSocketClient;
using SystemModule.SocketComponents.Event;

namespace BotSrv.Scenes.Scene
{
    public class SelectChrScene : SceneBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ScoketClient ClientSocket;
        private int NewIndex = 0;
        private readonly SelChar[] ChrArr;

        public SelectChrScene(RobotPlayer robotClient) : base(SceneType.SelectChr, robotClient)
        {
            ChrArr = new SelChar[2];
            ChrArr[0].UserChr = new UserCharacterInfo();
            ChrArr[1].UserChr = new UserCharacterInfo();
            NewIndex = 0;
            ClientSocket = new ScoketClient();
            ClientSocket.OnConnected += CSocketConnect;
            ClientSocket.OnDisconnected += CSocketDisconnect;
            ClientSocket.OnReceivedData += CSocketRead;
            ClientSocket.OnError += CSocketError;
        }

        public override void OpenScene()
        {
            ConnectionStep = ConnectionStep.QueryChr;
            ClientSocket.Connect(MShare.SelChrAddr, MShare.SelChrPort);
        }

        public override void CloseScene()
        {
            SetNotifyEvent(CloseSocket, 1000);
        }

        private void SendSelChr(string chrname)
        {
            if (string.IsNullOrEmpty(chrname))
            {
                return;
            }
            RobotClient.ChrName = chrname;
            CommandMessage msg = Messages.MakeMessage(Messages.CM_SELCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(RobotClient.LoginId + "/" + chrname));
            logger.Info($"选择角色 {chrname}");
        }

        private void SendDelChr(string chrname)
        {
            CommandMessage msg = Messages.MakeMessage(Messages.CM_DELCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(chrname));
        }

        private void ClearChrs()
        {
            ChrArr[0].UserChr.Name = "";
            ChrArr[1].UserChr.Name = "";
        }

        private void AddChr(string uname, int job, int hair, int level, int sex)
        {
            int n;
            if (!ChrArr[0].Valid)
            {
                n = 0;
            }
            else if (!ChrArr[1].Valid)
            {
                n = 1;
            }
            else
            {
                return;
            }
            ChrArr[n].UserChr.Name = uname;
            ChrArr[n].Valid = true;
        }

        private void MakeNewChar(int index)
        {
            NewIndex = index;
            ChrArr[NewIndex].Valid = true;
        }

        public override void PlayScene()
        {
            //if (MShare.g_boOpenAutoPlay && (MShare.g_nAPReLogon == 2))
            //{
            //    if (MShare.GetTickCount() - MShare.g_nAPReLogonWaitTick > MShare.g_nAPReLogonWaitTime)
            //    {
            //        MShare.g_nAPReLogonWaitTick = MShare.GetTickCount();
            //        MShare.g_nAPReLogon = 3;
            //        SendSelChr(robotClient.m_sChrName);
            //    }
            //}
        }

        public void ClientGetReceiveChrs(string body)
        {
            string chrname = string.Empty;
            string job = string.Empty;
            string hair = string.Empty;
            string level = string.Empty;
            string sex = string.Empty;
            if (MShare.OpenAutoPlay && (MShare.g_nAPReLogon == 1))
            {
                MShare.g_nAPReLogon = 2;
                MShare.g_nAPReLogonWaitTick = MShare.GetTickCount();
                MShare.g_nAPReLogonWaitTime = 5000 + RandomNumber.GetInstance().Random(10) * 1000;
            }
            ClearChrs();
            string str = EDCode.DeCodeString(body);
            int select = 0;
            int nChrCount = 0;
            for (var i = 0; i < 1; i++)
            {
                str = HUtil32.GetValidStr3(str, ref chrname, HUtil32.Backslash);
                str = HUtil32.GetValidStr3(str, ref job, HUtil32.Backslash);
                str = HUtil32.GetValidStr3(str, ref hair, HUtil32.Backslash);
                str = HUtil32.GetValidStr3(str, ref level, HUtil32.Backslash);
                str = HUtil32.GetValidStr3(str, ref sex, HUtil32.Backslash);
                if ((!string.IsNullOrEmpty(chrname)) && (!string.IsNullOrEmpty(level)) && (!string.IsNullOrEmpty(sex)))
                {
                    if (chrname[0] == '*')
                    {
                        select = i;
                        chrname = chrname[1..];
                    }
                    AddChr(chrname, HUtil32.StrToInt(job, 0), HUtil32.StrToInt(hair, 0), HUtil32.StrToInt(level, 0), HUtil32.StrToInt(sex, 0));
                    nChrCount++;
                }
            }
            if (nChrCount > 0)
            {
                SendSelChr(ChrArr[select].UserChr.Name);
            }
            else
            {
                SetNotifyEvent(NewChr, 1000);
            }
        }

        private void NewChr()
        {
            ConnectionStep = ConnectionStep.NewChr;
            SelectChrCreateNewChr(RobotClient.ChrName);
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
            SendNewChr(RobotClient.LoginId, sChrName, sHair, sJob, sSex);
            logger.Info($"创建角色 {sChrName}");
        }

        public void ClientGetStartPlay(string body)
        {
            logger.Info("准备进入游戏");
            string addr = string.Empty;
            string Str = EDCode.DeCodeString(body);
            string sport = HUtil32.GetValidStr3(Str, ref addr, HUtil32.Backslash);
            MShare.RunServerPort = HUtil32.StrToInt(sport, 0);
            MShare.RunServerAddr = addr;
            MShare.ConnectionStep = ConnectionStep.Play;
        }

        private void CloseSocket()
        {
            ClientSocket.Disconnect();
        }

        public void ClientGetReconnect(string body)
        {
            //string addr = string.Empty;
            //string sport = string.Empty;
            //string Str = EDcode.DeCodeString(body);
            //sport = HUtil32.GetValidStr3(Str, ref addr, HUtil32.Backslash);
            //MShare.g_boServerChanging = true;
            //MShare.g_ConnectionStep = TConnectionStep.cnsPlay;
            //CloseSocket();//断开游戏网关链接
            //ClientSocket.Host = addr;
            //ClientSocket.Port = HUtil32.StrToInt(sport, 0);
            //ClientSocket.Connect();
            //robotClient.SocStr = string.Empty;
            //robotClient.BufferStr = string.Empty;
        }

        private void SendNewChr(string uid, string uname, byte shair, byte sjob, byte ssex)
        {
            var msg = Messages.MakeMessage(Messages.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(uid + "/" + uname + "/" + shair + "/" + sjob + "/" + ssex));
        }

        public void SendQueryChr()
        {
            ConnectionStep = ConnectionStep.QueryChr;
            var DefMsg = Messages.MakeMessage(Messages.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(RobotClient.LoginId + "/" + RobotClient.Certification));
            logger.Info("查询角色.");
        }

        private void SendSocket(string sendstr)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                logger.Warn($"Socket Close {ClientSocket.RemoteEndPoint}");
            }
        }

        #region Socket Events

        private void CSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.ServerConnected = true;
            if (ConnectionStep == ConnectionStep.QueryChr)
            {
                SetNotifyEvent(SendQueryChr, 1000);
                ConnectionStep = ConnectionStep.SelChr;
            }
            logger.Info($"连接角色网关:[{MShare.SelChrAddr}:{MShare.SelChrPort}]");
        }

        private void CSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.ServerConnected = false;
            if (MShare.g_SoftClosed)
            {
                MShare.g_SoftClosed = false;
            }
        }

        private void CSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    logger.Warn($"角色服务器[{ClientSocket.RemoteEndPoint}拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    logger.Warn($"角色服务器[{ClientSocket.RemoteEndPoint}关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    logger.Warn($"角色服务器[{ClientSocket.RemoteEndPoint}链接超时...");
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

    public struct SelChar
    {
        public bool Valid;
        public UserCharacterInfo UserChr;
    }

    public class UserCharacterInfo
    {
        public string Name;
    }
}