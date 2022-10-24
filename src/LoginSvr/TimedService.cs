using LoginSvr.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var builder = new StringBuilder();
            int serverListCount = _massocService.ServerList.Count;
            for (var i = 0; i < serverListCount; i++)
            {
                var msgServer = _massocService.ServerList[i];
                var sServerName = msgServer.ServerName;
                if (!string.IsNullOrEmpty(sServerName))
                {
                    builder.Append(sServerName + "/" + msgServer.ServerIndex + "/" + msgServer.OnlineCount + "/");
                    if (msgServer.ServerIndex == 99)
                    {
                        builder.Append("DB/");
                    }
                    else
                    {
                        builder.Append("Game/");
                    }
                    builder.Append($"Online:{msgServer.OnlineCount}/");
                    if ((HUtil32.GetTickCount() - msgServer.KeepAliveTick) < 30000)
                    {
                        builder.Append("正常");
                    }
                    else
                    {
                        builder.Append("超时");
                    }
                }
                else
                {
                    builder.Append("-/-/-/-;");
                }
            }
            _logger.LogDebug(builder.ToString());
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
                    string sServerName = MsgServer.ServerName;
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        var tickTime = HUtil32.GetTickCount() - MsgServer.KeepAliveTick;
                        if (tickTime <= 60000) continue;
                        MsgServer.Socket.Close();
                        if (MsgServer.ServerIndex == 99)
                        {
                            if (string.IsNullOrEmpty(sServerName))
                            {
                                _logger.Information($"数据库服务器[{MsgServer.IPaddr}]响应超时,关闭链接.");
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
                                _logger.Information($"游戏服务器[{MsgServer.IPaddr}]响应超时,关闭链接.");
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