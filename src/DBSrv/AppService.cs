using DBSrv.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DBSrv
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly GateUserService _userSocService;
        private readonly LoginSessionServer _loginSvrService;
        private readonly PlayerDataService _dataService;

        public AppService(ILogger<AppService> logger, GateUserService userSoc, LoginSessionServer idSoc, PlayerDataService dataService)
        {
            _logger = logger;
            _userSocService = userSoc;
            _loginSvrService = idSoc;
            _dataService = dataService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug("DBSrv is stopping."));
            _userSocService.Start(stoppingToken);
            _loginSvrService.Start();
            _dataService.Start();
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("DBSrv is starting.");
            DBShare.Initialization();
            DBShare.LoadConfig();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("DBSrv is stopping.");
            _userSocService.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}