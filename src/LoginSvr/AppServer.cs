using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using SystemModule;
using SystemModule.Sockets;

namespace LoginSvr
{
    public class AppServer
    {
        private ThreadParseList parseListTimer = null;
        private LoginService _loginService;
        private ISocketServer _serverSocket;
        private Timer _logThreadTime;
        private Timer _execThreadTimer;
        private readonly MasSocService _massocService;

        public AppServer(LoginService loginService, MasSocService masSocService)
        {
            LSShare.Initialization();
            TConfig Config = LSShare.g_Config;
            Config.boRemoteClose = false;
            Config.GateList = new List<TGateInfo>();
            Config.SessionList = new List<TConnInfo>();
            Config.ServerNameList = new List<string>();
            Config.AccountCostList = new Dictionary<string, int>();
            Config.IPaddrCostList = new Dictionary<string, int>();
            LSShare.nSessionIdx = 1;
            _loginService = loginService;
            _massocService = masSocService;
            _logThreadTime = new Timer(LogThreadTime, null, 1000, 3000);
            _serverSocket = new ISocketServer(ushort.MaxValue, 1024);
            _serverSocket.OnClientConnect += GSocketClientConnect;
            _serverSocket.OnClientDisconnect += GSocketClientDisconnect;
            _serverSocket.OnClientRead += GSocketClientRead;
            _serverSocket.OnClientError += GSocketClientError;
            _serverSocket.Init();
        }

        private void GSocketClientConnect(object sender, AsyncUserToken e)
        {
            TGateInfo GateInfo;
            var Config = LSShare.g_Config;
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

        private void GSocketClientDisconnect(object sender, AsyncUserToken e)
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

        private void GSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void GSocketClientRead(object sender, AsyncUserToken e)
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
                _loginService.ProcessGate(Config);
            }
            finally
            {
                LSShare.bo470D20 = false;
            }
        }

        private void LogThreadTime(object obj)
        {
            TConfig Config = LSShare.g_Config;
            //Label1.Text = (Config.dwProcessGateTime).ToString();
            //CkLogin.Checked = GSocket.Socket.Connected;
            //CkLogin.Text = "连接 (" + (GSocket.Socket.ActiveConnections).ToString() + ")";
            //LbMasCount.Text = (LSShare.nOnlineCountMin).ToString() + "/" + (LSShare.nOnlineCountMax).ToString();
            for (var i = 0; i < LSShare.g_MainMsgList.Count; i++)
            {
                Console.WriteLine(LSShare.g_MainMsgList[i]);
            }
            LSShare.g_MainMsgList.Clear();
            _loginService.SessionClearKick(Config);
            _loginService.SessionClearNoPayMent(Config);
            MonitorTimer(obj);
        }

        public void Start()
        {
            TConfig Config = LSShare.g_Config;
            _loginService.StartService(Config);
            LSShare.MainOutMessage("1) 正在启动服务器...");
            LSShare.MainOutMessage("2) 正在等待服务器连接...");
            while (true)
            {
                if (_massocService.CheckReadyServers())
                {
                    break;
                }
                Thread.Sleep(1);
            }
            _serverSocket.Start(Config.sGateAddr, Config.nGatePort);
            LSShare.MainOutMessage("3) 服务器启动完成...");
            _execThreadTimer = new Timer(ExecTimerTimer, null, 3000, 1);
            parseListTimer = new ThreadParseList(_loginService);
        }

        public void CountLogTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            const string sFormatMsg = "{0}/{1}";
            TConfig Config = LSShare.g_Config;
            string sLogMsg = string.Format(sFormatMsg, LSShare.nOnlineCountMin, LSShare.nOnlineCountMax);
            _loginService.SaveContLogMsg(Config, sLogMsg);
            LSShare.nOnlineCountMax = 0;
        }

        public void MonitorTimer(object obj)
        {
            string sServerName;
            TMsgServerInfo MsgServer;
            IList<TMsgServerInfo> ServerList = _massocService.m_ServerList;
            if (!ServerList.Any())
            {
                return;
            }
            StringBuilder msgStr = new StringBuilder();
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
                    var tickTime = HUtil32.GetTickCount() - MsgServer.dwKeepAliveTick;
                    if (tickTime < 30000)
                    {
                        msgStr.Append("Status:[正常]");
                    }
                    else
                    {
                        msgStr.Append("Status:[超时]");
                    }
                    if (tickTime <= 60000) continue;
                    MsgServer.Socket.Close();
                    if (string.IsNullOrEmpty(sServerName))
                    {
                        LSShare.MainOutMessage($"数据库服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                    }
                    else
                    {
                        LSShare.MainOutMessage($"[{sServerName}]数据库服务器响应超时,关闭链接.");
                    }
                }
            }
            Debug.WriteLine(msgStr.ToString());
        }
    }
}