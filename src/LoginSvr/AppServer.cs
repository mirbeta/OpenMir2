using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Sockets.AsyncSocketServer;

namespace LoginSvr
{
    public class AppServer
    {
        private ArrayList SList_0344 = null;
        private TThreadParseList ParseList = null;
        private LoginSvrService loginSvr;
        private ISocketServer gSocket;
        private Timer _logThreadTime;
        private Timer _execThreadTimer;
        private readonly MasSocService _masSoc;

        public AppServer(LoginSvrService loginSvrService, MasSocService masSoc)
        {
            LSShare.initialization();
            TConfig Config = LSShare.g_Config;
            Config.boRemoteClose = false;
            LSShare.StringList_0 = new List<long>();
            LSShare.nSessionIdx = 1;
            Config.GateList = new List<TGateInfo>();
            Config.SessionList = new List<TConnInfo>();
            Config.ServerNameList = new List<string>();
            SList_0344 = new ArrayList();
            Config.AccountCostList = new Dictionary<string, int>();
            Config.IPaddrCostList = new Dictionary<string, int>();
            loginSvr = loginSvrService;
            _masSoc = masSoc;
            ParseList = new TThreadParseList(loginSvr);
            gSocket = new ISocketServer(ushort.MaxValue, 1024);
            gSocket.OnClientConnect += GSocketClientConnect;
            gSocket.OnClientDisconnect += GSocketClientDisconnect;
            gSocket.OnClientRead += GSocketClientRead;
            gSocket.OnClientError += GSocketClientError;
            gSocket.Init();
            _logThreadTime = new Timer(LogThreadTime, null, 1000, 3000);
        }

        private void GSocketClientConnect(object sender, SystemModule.Sockets.AsyncUserToken e)
        {
            TGateInfo GateInfo;
            TConfig Config;
            Config = LSShare.g_Config;
            //if (!ExecTimer.Enabled)
            //{
            //    Socket.Close();
            //    return;
            //}
            GateInfo = new TGateInfo();
            GateInfo.Socket = e.Socket;
            GateInfo.sIPaddr = LSShare.GetGatePublicAddr(Config, e.RemoteIPaddr);
            GateInfo.sReceiveMsg = "";
            GateInfo.UserList = new List<TUserInfo>();
            GateInfo.dwKeepAliveTick = HUtil32.GetTickCount();
            Config.GateList.Add(GateInfo);
        }

        private void GSocketClientDisconnect(object sender, SystemModule.Sockets.AsyncUserToken e)
        {
            TGateInfo GateInfo;
            TUserInfo UserInfo;
            TConfig Config = LSShare.g_Config;
            for (var i = 0; i < Config.GateList.Count; i++)
            {
                GateInfo = Config.GateList[i];
                if (GateInfo.Socket == e.Socket)
                {
                    for (var j = 0; j < GateInfo.UserList.Count; j++)
                    {
                        UserInfo = GateInfo.UserList[j];
                        if (Config.boShowDetailMsg)
                        {
                            LSShare.MainOutMessage("Close: " + UserInfo.sUserIPaddr);
                        }
                        UserInfo = null;
                    }
                    GateInfo.UserList = null;
                    GateInfo = null;
                    Config.GateList.RemoveAt(i);
                    break;
                }
            }
        }

        private void GSocketClientError(object sender, SystemModule.Sockets.AsyncSocketErrorEventArgs e)
        {

        }

        private void GSocketClientRead(object sender, SystemModule.Sockets.AsyncUserToken e)
        {
            TGateInfo GateInfo;
            TConfig Config = LSShare.g_Config;
            for (var i = 0; i < Config.GateList.Count; i++)
            {
                GateInfo = Config.GateList[i];
                if (GateInfo.Socket == e.Socket)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    var sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                    GateInfo.sReceiveMsg = GateInfo.sReceiveMsg + sReviceMsg;
                    break;
                }
            }
        }

        private void ExecTimerTimer(object obj)
        {
            if (LSShare.bo470D20 && !LSShare.g_boDataDBReady)
            {
                return;
            }
            LSShare.bo470D20 = true;
            try
            {
                TConfig Config = LSShare.g_Config;
                loginSvr.ProcessGate(Config);
            }
            finally
            {
                LSShare.bo470D20 = false;
            }
        }

        private void LogThreadTime(object obj)
        {
            TConfig Config;
            Config = LSShare.g_Config;
            //Label1.Text = (Config.dwProcessGateTime).ToString();
            //CkLogin.Checked = GSocket.Socket.Connected;
            //CkLogin.Text = "连接 (" + (GSocket.Socket.ActiveConnections).ToString() + ")";
            //LbMasCount.Text = (LSShare.nOnlineCountMin).ToString() + "/" + (LSShare.nOnlineCountMax).ToString();
            //if (Memo1.Lines.Count > 2000)
            //{
            //    Memo1.Clear();
            //}
            for (var i = 0; i < LSShare.g_MainMsgList.Count; i++)
            {
                Console.WriteLine(LSShare.g_MainMsgList[i]);
            }
            LSShare.g_MainMsgList.Clear();
            var count = 0;
            while (true)
            {
                if (LSShare.StringList_0.Count <= count)
                {
                    break;
                }
                if ((HUtil32.GetTickCount() - LSShare.StringList_0[count]) > 60000)
                {
                    LSShare.StringList_0.Remove(count);
                    continue;
                }
                count++;
            }
            loginSvr.SessionClearKick(Config);
            loginSvr.SessionClearNoPayMent(Config);
            MonitorTimer(obj);
        }

        public void Start()
        {
            TConfig Config = LSShare.g_Config;
            loginSvr.StartService(Config);
            LSShare.MainOutMessage("1) 正在启动服务器...");
            LSShare.MainOutMessage("2) 正在等待服务器连接...");
            // while (true)
            // {
            //     if (FrmMasSoc.CheckReadyServers())
            //     {
            //         break;
            //     }
            //     Thread.Sleep(1);
            // }
            gSocket.Start(Config.sGateAddr, Config.nGatePort);
            LSShare.MainOutMessage("3) 服务器启动完成...");
            _execThreadTimer = new Timer(ExecTimerTimer, null, 3000, 1);
        }

        public void CountLogTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            const string sFormatMsg = "{0}/{1}";
            TConfig Config = LSShare.g_Config;
            string sLogMsg = string.Format(sFormatMsg, LSShare.nOnlineCountMin, LSShare.nOnlineCountMax);
            loginSvr.SaveContLogMsg(Config, sLogMsg);
            LSShare.nOnlineCountMax = 0;
        }

        public void MonitorTimer(object obj)
        {
            string sServerName;
            IList<TMsgServerInfo> ServerList;
            TMsgServerInfo MsgServer;
            long TickTime;
            StringBuilder msgStr = new StringBuilder();
            try
            {
                ServerList = _masSoc.m_ServerList;
                if (!ServerList.Any())
                {
                    return;
                }
                for (var i = 0; i < ServerList.Count; i++)
                {
                    MsgServer = ServerList[i];
                    sServerName = MsgServer.sServerName;
                    if (sServerName != "")
                    {
                        msgStr.Append($"{sServerName}");
                        if (MsgServer.nServerIndex == 99)
                        {
                            msgStr.Append(" ServerIndex:[DB] ");
                        }
                        else
                        {
                            msgStr.Append($" ServerIndex:[{MsgServer.nServerIndex}] ");
                        }
                        msgStr.Append($"OnLineCount:[{MsgServer.nOnlineCount}] SelectId:[{MsgServer.nSelectID}] ");
                        TickTime = HUtil32.GetTickCount() - MsgServer.dwKeepAliveTick;
                        if (TickTime < 30000)
                        {
                            msgStr.Append("Status:[正常]");
                        }
                        else
                        {
                            msgStr.Append("Status:[超时]");
                        }
                        if (TickTime > 60000)
                        {
                            MsgServer.Socket.Close();
                        }
                    }
                }
                LSShare.MainOutMessage(msgStr.ToString());
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMain.MonitorTimerTimer");
            }
        }
    }
}