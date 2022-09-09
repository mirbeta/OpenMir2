using DBSvr.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace DBSvr
{
    public class TimedService : BackgroundService
    {
        private readonly MirLog _logger;
        private readonly UserSocService _userSoc;
        private readonly LoginSvrService _loginSoc;
        private readonly HumDataService _dataService;
        private int _lastSocketTick;
        private int _lastKeepTick;
        private int _lastClearTick;

        public TimedService(MirLog logger, UserSocService userSoc, LoginSvrService loginSoc, HumDataService dataService)
        {
            _logger = logger;
            _userSoc = userSoc;
            _loginSoc = loginSoc;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _lastSocketTick = HUtil32.GetTickCount();
            _lastKeepTick = HUtil32.GetTickCount();
            _lastClearTick = HUtil32.GetTickCount();
            while (!stoppingToken.IsCancellationRequested)
            {
                if (HUtil32.GetTickCount() - _lastKeepTick > 7000)
                {
                    _lastKeepTick = HUtil32.GetTickCount();
                    var userCount = _userSoc.GetUserCount();
                    _loginSoc.SendKeepAlivePacket(userCount);
                    _logger.DebugLog("发送心跳检测.");
                }
                if (HUtil32.GetTickCount() - _lastSocketTick > 10000)
                {
                    _lastSocketTick = HUtil32.GetTickCount();
                    _loginSoc.CheckConnection();
                    _logger.DebugLog("检查服务链接状态.");
                }
                if (HUtil32.GetTickCount() - _lastClearTick > 10000)
                {
                    _lastClearTick = HUtil32.GetTickCount();
                    _dataService.ClearTimeoutSession();
                    _logger.DebugLog("清理过期会话.");
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
            }
        }
    }
}