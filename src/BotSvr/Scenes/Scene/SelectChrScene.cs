using System;
using SystemModule;
using SystemModule.Packet.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace BotSvr.Scenes.Scene
{
    public class SelectChrScene : SceneBase
    {
        private readonly ClientScoket ClientSocket;
        private readonly ClientManager _clientManager;
        private int NewIndex = 0;
        private readonly SelChar[] ChrArr;

        public SelectChrScene(RobotClient robotClient, ClientManager clientManager) : base(SceneType.stSelectChr, robotClient)
        {
            _clientManager = clientManager;
            ChrArr = new SelChar[2];
            ChrArr[0].UserChr = new TUserCharacterInfo();
            ChrArr[1].UserChr = new TUserCharacterInfo();
            NewIndex = 0;
            ClientSocket = new ClientScoket();
            ClientSocket.OnConnected += CSocketConnect;
            ClientSocket.OnDisconnected += CSocketDisconnect;
            ClientSocket.ReceivedDatagram += CSocketRead;
            ClientSocket.OnError += CSocketError;
        }

        public override void OpenScene()
        {
            m_ConnectionStep = TConnectionStep.cnsQueryChr;
            ClientSocket.Connect(MShare.g_sSelChrAddr, MShare.g_nSelChrPort);
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
            robotClient.ChrName = chrname;
            ClientMesaagePacket msg = Grobal2.MakeDefaultMsg(Grobal2.CM_SELCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(robotClient.LoginID + "/" + chrname));
            MainOutMessage($"选择角色 {chrname}");
        }

        private void SendDelChr(string chrname)
        {
            ClientMesaagePacket msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DELCHR, 0, 0, 0, 0);
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
            string uname = string.Empty;
            string sjob = string.Empty;
            string shair = string.Empty;
            string slevel = string.Empty;
            string ssex = string.Empty;
            if (MShare.g_boOpenAutoPlay && (MShare.g_nAPReLogon == 1))
            {
                MShare.g_nAPReLogon = 2;
                MShare.g_nAPReLogonWaitTick = MShare.GetTickCount();
                MShare.g_nAPReLogonWaitTime = 5000 + RandomNumber.GetInstance().Random(10) * 1000;
            }
            ClearChrs();
            string Str = EDCode.DeCodeString(body);
            int select = 0;
            int nChrCount = 0;
            for (var i = 0; i < 1; i++)
            {
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref sjob, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref shair, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref slevel, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref ssex, HUtil32.Backslash);
                if ((uname != "") && (slevel != "") && (ssex != ""))
                {
                    if (uname[0] == '*')
                    {
                        select = i;
                        uname = uname.Substring(1, uname.Length - 1);
                    }
                    AddChr(uname, HUtil32.StrToInt(sjob, 0), HUtil32.StrToInt(shair, 0), HUtil32.StrToInt(slevel, 0), HUtil32.StrToInt(ssex, 0));
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
            m_ConnectionStep = TConnectionStep.cnsNewChr;
            SelectChrCreateNewChr(robotClient.ChrName);
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
            SendNewChr(robotClient.LoginID, sChrName, sHair, sJob, sSex);
            MainOutMessage($"创建角色 {sChrName}");
        }

        public void ClientGetStartPlay(string body)
        {
            MainOutMessage("准备进入游戏");
            string addr = string.Empty;
            string Str = EDCode.DeCodeString(body);
            string sport = HUtil32.GetValidStr3(Str, ref addr, HUtil32.Backslash);
            MShare.g_nRunServerPort = HUtil32.StrToInt(sport, 0);
            MShare.g_sRunServerAddr = addr;
            MShare.g_ConnectionStep = TConnectionStep.cnsPlay;
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
            //ClientSocket.Port = HUtil32.Str_ToInt(sport, 0);
            //ClientSocket.Connect();
            //robotClient.SocStr = string.Empty;
            //robotClient.BufferStr = string.Empty;
        }

        private void SendNewChr(string uid, string uname, byte shair, byte sjob, byte ssex)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(uid + "/" + uname + "/" + shair + "/" + sjob + "/" + ssex));
        }

        public void SendQueryChr()
        {
            m_ConnectionStep = TConnectionStep.cnsQueryChr;
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(robotClient.LoginID + "/" + robotClient.Certification));
            MainOutMessage("查询角色.");
        }

        private void SendSocket(string sendstr)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                MainOutMessage($"Socket Close {ClientSocket.Host}:{ClientSocket.Port}");
            }
        }

        #region Socket Events

        private void CSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.g_boServerConnected = true;
            if (m_ConnectionStep == TConnectionStep.cnsQueryChr)
            {
                SetNotifyEvent(SendQueryChr, 1000);
                m_ConnectionStep = TConnectionStep.cnsSelChr;
            }
            MainOutMessage($"连接角色网关:[{MShare.g_sSelChrAddr}:{MShare.g_nSelChrPort}]");
        }

        private void CSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.g_boServerConnected = false;
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
                    Console.WriteLine($"角色服务器[{ClientSocket.EndPoint}拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    Console.WriteLine($"角色服务器[{ClientSocket.EndPoint}关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    Console.WriteLine($"角色服务器[{ClientSocket.EndPoint}链接超时...");
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

    public struct SelChar
    {
        public bool Valid;
        public TUserCharacterInfo UserChr;
    }

    public class TUserCharacterInfo
    {
        public string Name;
    }
}