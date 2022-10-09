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
        private int _makeDBTick;

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
            _makeDBTick = HUtil32.GetTickCount();
            while (!stoppingToken.IsCancellationRequested)
            {
                if (HUtil32.GetTickCount() - _lastKeepTick > 7000)
                {
                    _lastKeepTick = HUtil32.GetTickCount();
                    var userCount = _userSoc.GetUserCount();
                    _loginSoc.SendKeepAlivePacket(userCount);
                }
                if (HUtil32.GetTickCount() - _lastSocketTick > 10000)
                {
                    _lastSocketTick = HUtil32.GetTickCount();
                    _loginSoc.CheckConnection();
                }
                if (HUtil32.GetTickCount() - _lastClearTick > 10000)
                {
                    _lastClearTick = HUtil32.GetTickCount();
                    _dataService.ClearTimeoutSession();
                }
                if (HUtil32.GetTickCount() - _makeDBTick > 60 * 1000)
                {
                    ProcessMemeryStorage();
                    _makeDBTick = HUtil32.GetTickCount();
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
            }
        }

        private void ProcessMemeryStorage()
        {
            //todo 从内存获取数据，刷新到数据库，减少数据库压力，和防止大量数据保存超时
            
        }
    }
}