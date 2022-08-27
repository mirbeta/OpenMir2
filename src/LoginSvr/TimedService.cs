using LoginSvr.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace LoginSvr
{
    public class TimedService : BackgroundService
    {
        private readonly MirLog _logger;
        private readonly LoginService _loginService;
        private readonly ThreadParseList _threadParseList;
        private readonly MasSocService _massocService;
        private int _processMonSocTick = 0;
        private int _processServerStatuTick = 0;

        public TimedService(MirLog logger, LoginService loginService, MasSocService massocService, ThreadParseList threadParseList)
        {
            _logger = logger;
            _loginService = loginService;
            _massocService = massocService;
            _threadParseList = threadParseList;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                LoginProcess();
                ProcessMonSoc();
                CheckServerStatus();
                _threadParseList.Execute();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void LoginProcess()
        {
            _loginService.SessionClearKick();
            _loginService.SessionClearNoPayMent();
        }

        private void ProcessMonSoc()
        {
            if (HUtil32.GetTickCount() - _processMonSocTick > 20000)
            {
                _processMonSocTick = HUtil32.GetTickCount();
                ProcessCleanSession();
            }
        }

        private void ProcessCleanSession()
        {
            string sMsg = string.Empty;
            int nC = _massocService.ServerList.Count;
            for (var i = 0; i < _massocService.ServerList.Count; i++)
            {
                var msgServer = _massocService.ServerList[i];
                var sServerName = msgServer.sServerName;
                if (!string.IsNullOrEmpty(sServerName))
                {
                    sMsg = sMsg + sServerName + "/" + msgServer.nServerIndex + "/" + msgServer.nOnlineCount + "/";
                    if ((HUtil32.GetTickCount() - msgServer.dwKeepAliveTick) < 30000)
                    {
                        sMsg = sMsg + "正常 ";
                    }
                    else
                    {
                        sMsg = sMsg + "超时 ";
                    }
                }
                else
                {
                    sMsg = "-/-/-/-;";
                }
            }
            _logger.LogDebug(sMsg);
        }
        
        private void CheckServerStatus()
        {
            if (HUtil32.GetTickCount() - _processServerStatuTick > 20000)
            {
                _processServerStatuTick = HUtil32.GetTickCount();
                IList<MessageServerInfo> ServerList = _massocService.ServerList;
                if (!ServerList.Any())
                {
                    return;
                }
                for (var i = 0; i < ServerList.Count; i++)
                {
                    MessageServerInfo MsgServer = ServerList[i];
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
                                _logger.Information($"数据库服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                            }
                            else
                            {
                                _logger.Information($"[{sServerName}]数据库服务器响应超时,关闭链接.");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sServerName))
                            {
                                _logger.Information($"游戏服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                            }
                            else
                            {
                                _logger.Information($"[{sServerName}]游戏服务器响应超时,关闭链接.");
                            }
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}