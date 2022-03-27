using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DBSvr
{
    public class TimedService : BackgroundService
    {
        private readonly UserSocService _userSoc;
        private readonly LoginSvrService _LoginSoc;
        private readonly HumDataService _dataService;

        public TimedService(UserSocService userSoc, LoginSvrService loginSoc, HumDataService dataService)
        {
            _userSoc = userSoc;
            _LoginSoc = loginSoc;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var userCount = _userSoc.GetUserCount();
                _LoginSoc.SendKeepAlivePacket(userCount);
                _LoginSoc.CheckConnection();
                _dataService.ClearTimeoutSession();
                await Task.Delay(TimeSpan.FromMilliseconds(5000), stoppingToken);
            }
        }
    }
}