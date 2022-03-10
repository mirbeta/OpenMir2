using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SystemModule;

namespace LoginSvr
{
    public class AppServer
    {
        private ThreadParseList parseListTimer = null;
        private LoginService _loginService;
        private readonly MasSocService _massocService;
        private readonly ConfigManager _configManager;
        private readonly ILogger<AppServer> _logger;
        private readonly Thread _appThread;

        public AppServer(ILogger<AppServer> logger, LoginService loginService, MasSocService masSocService, ConfigManager configManager)
        {
            LSShare.Initialization();
            _loginService = loginService;
            _massocService = masSocService;
            _configManager = configManager;
            _appThread = new Thread(LoginProcessThread);
            _appThread.IsBackground = true;
            _logger = logger;
            parseListTimer = new ThreadParseList(_logger, _loginService, _configManager);
        }

        public void Start()
        {
            _logger.LogInformation("正在启动服务器...");
            _logger.LogInformation("正在等待服务器连接...");
            while (true)
            {
                if (_massocService.CheckReadyServers())
                {
                    break;
                }
                Thread.Sleep(1);
            }
            parseListTimer.Start();
            _appThread.Start();
        }

        public void Stop()
        {
            
        }

        private void LoginProcessThread(object obj)
        {
            while (true)
            {
                _loginService.SessionClearKick(_configManager.Config);
                _loginService.SessionClearNoPayMent(_configManager.Config);
                Thread.Sleep(1);
            }
        }

        public void CheckServerStatus()
        {
            IList<TMsgServerInfo> ServerList = _massocService.m_ServerList;
            if (!ServerList.Any())
            {
                return;
            }
            for (var i = 0; i < ServerList.Count; i++)
            {
                TMsgServerInfo MsgServer = ServerList[i];
                string sServerName = MsgServer.sServerName;
                if (!string.IsNullOrEmpty(sServerName))
                {
                    var tickTime = HUtil32.GetTickCount() - MsgServer.dwKeepAliveTick;
                    if (tickTime <= 60000) continue;
                    MsgServer.Socket.Close();
                    if (MsgServer.nServerIndex == 99)
                    {
                        if (string.IsNullOrEmpty(sServerName))
                        {
                            _logger.LogDebug($"数据库服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                        }
                        else
                        {
                            _logger.LogDebug($"[{sServerName}]数据库服务器响应超时,关闭链接.");
                        }
                    }
                    else
                    {                    
                        if (string.IsNullOrEmpty(sServerName))
                        {
                            _logger.LogDebug($"游戏服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                        }
                        else
                        {
                            _logger.LogDebug($"[{sServerName}]游戏服务器响应超时,关闭链接.");
                        }
                    }
                }
            }
        }
    }
}