using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SystemModule;

namespace LoginSvr
{
    public class AppServer
    {
        private ThreadParseList parseListTimer = null;
        private LoginService _loginService;
        private readonly MasSocService _massocService;

        public AppServer(LoginService loginService, MasSocService masSocService)
        {
            LSShare.Initialization();
            _loginService = loginService;
            _massocService = masSocService;
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
            _loginService.Start();
            parseListTimer = new ThreadParseList(_loginService);
        }

        public void CountLogTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            const string sFormatMsg = "{0}/{1}";
            TConfig Config = LSShare.g_Config;
            string sLogMsg = string.Format(sFormatMsg, LSShare.nOnlineCountMin, LSShare.nOnlineCountMax);
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
                if (!string.IsNullOrEmpty(sServerName))
                {
                    msgStr.Append($"{sServerName} ");
                    if (MsgServer.nServerIndex == 99)
                    {
                        msgStr.Append("ServerIndex:[DB] ");
                    }
                    else
                    {
                        msgStr.Append($"ServerIndex:[{MsgServer.nServerIndex}] ");
                    }
                    msgStr.Append($"OnLineCount:[{MsgServer.nOnlineCount}] SelectId:[{MsgServer.nSelectID}] ");
                    var tickTime = HUtil32.GetTickCount() - MsgServer.dwKeepAliveTick;
                    if (tickTime < 30000)
                    {
                        msgStr.Append("Status:[Success] ");
                    }
                    else
                    {
                        msgStr.Append("Status:[TimeOut] ");
                    }
                    if (tickTime <= 60000) continue;
                    MsgServer.Socket.Close();
                    if (MsgServer.nServerIndex == 99)
                    {
                        if (string.IsNullOrEmpty(sServerName))
                        {
                            LSShare.MainOutMessage($"数据库服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                        }
                        else
                        {
                            LSShare.MainOutMessage($"[{sServerName}]数据库服务器响应超时,关闭链接.");
                        }
                    }
                    else
                    {                    
                        if (string.IsNullOrEmpty(sServerName))
                        {
                            LSShare.MainOutMessage($"游戏服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                        }
                        else
                        {
                            LSShare.MainOutMessage($"[{sServerName}]游戏服务器响应超时,关闭链接.");
                        }
                    }
                }
            }
            Console.WriteLine(msgStr.ToString());
        }
    }
}