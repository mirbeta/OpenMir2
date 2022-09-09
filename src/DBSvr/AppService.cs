using DBSvr.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DBSvr
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly UserSocService _userSocService;
        private readonly LoginSvrService _loginSvrService;
        private readonly HumDataService _dataService;

        public AppService(ILogger<AppService> logger, UserSocService userSoc, LoginSvrService idSoc, HumDataService dataService)
        {
            _logger = logger;
            _userSocService = userSoc;
            _loginSvrService = idSoc;
            _dataService = dataService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug("DBSvr is stopping."));
            _userSocService.Start(stoppingToken);
            _loginSvrService.Start();
            _dataService.Start();
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("DBSvr is starting.");
            DBShare.Initialization();
            DBShare.LoadConfig();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("DBSvr is stopping.");
            _userSocService.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}