using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DBSvr
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly ServerApp _serverApp;
        private readonly TFrmUserSoc _userSoc;
        private readonly TFrmIDSoc _idSoc;
        private readonly HumDataService _dataService;
        private Timer keepAliveTimer;

        public AppService(ILogger<AppService> logger, ServerApp serverApp, TFrmUserSoc userSoc, TFrmIDSoc idSoc, HumDataService dataService)
        {
            _logger = logger;
            _serverApp = serverApp;
            _userSoc = userSoc;
            _idSoc = idSoc;
            _dataService = dataService;
            keepAliveTimer = new Timer(KeepAliveTimer, null, 1000, 5000);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"GameGate is stopping."));
            _serverApp.Start();
            _userSoc.Start();
            _idSoc.Start();
            _dataService.Start();
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"GameGate is starting.");
            _serverApp.StartService();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"GameGate is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }

        private void KeepAliveTimer(object obj)
        {
            _idSoc.SendKeepAlivePacket(_userSoc.GetUserCount());
        }
    }
}