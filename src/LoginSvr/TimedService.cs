using LoginSvr.Services;
using Microsoft.Extensions.Hosting;
using System;
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
        private readonly DataService _massocService;
        private int _processMonSocTick;
        private int _processServerStatusTick;

        public TimedService(MirLog logger, LoginService loginService, DataService massocService, ThreadParseList threadParseList)
        {
            _logger = logger;
            _loginService = loginService;
            _massocService = massocService;
            _threadParseList = threadParseList;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processMonSocTick = HUtil32.GetTickCount();
            _processServerStatusTick = HUtil32.GetTickCount();
            while (!stoppingToken.IsCancellationRequested)
            {
                _loginService.SessionClearKick();
                _massocService.SessionClearNoPayMent();
                ProcessMonSoc();
                CheckServerStatus();
                _threadParseList.Execute();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void ProcessMonSoc()
        {
            if (HUtil32.GetTickCount() - _processMonSocTick > 20000)
            {
                _processMonSocTick = HUtil32.GetTickCount();
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
                if (builder.Length > 0)
                {
                    _logger.LogDebug(builder.ToString());
                }
            }
        }
        
        private void CheckServerStatus()
        {
            if (HUtil32.GetTickCount() - _processServerStatusTick > 20000)
            {
                _processServerStatusTick = HUtil32.GetTickCount();
                var serverList = _massocService.ServerList;
                if (!serverList.Any())
                {
                    return;
                }
                for (var i = 0; i < serverList.Count; i++)
                {
                    ServerSessionInfo msgServer = serverList[i];
                    var sServerName = msgServer.ServerName;
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        var tickTime = HUtil32.GetTickCount() - msgServer.KeepAliveTick;
                        if (tickTime <= 60000) continue;
                        msgServer.Socket.Close();
                        if (msgServer.ServerIndex == 99)
                        {
                            if (string.IsNullOrEmpty(sServerName))
                            {
                                _logger.Warn($"数据库服务器[{msgServer.IPaddr}]响应超时,关闭链接.");
                            }
                            else
                            {
                                _logger.Warn($"[{sServerName}]数据库服务器响应超时,关闭链接.");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sServerName))
                            {
                                _logger.Warn($"游戏服务器[{msgServer.IPaddr}]响应超时,关闭链接.");
                            }
                            else
                            {
                                _logger.Warn($"[{sServerName}]游戏服务器响应超时,关闭链接.");
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