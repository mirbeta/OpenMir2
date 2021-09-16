using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace LoginSvr
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly AppServer _serverApp;
        private readonly MasSocService _masSoc;
        private readonly MonSocService _monSoc;

        public AppService(ILogger<AppService> logger, AppServer serverApp, MasSocService masSoc, MonSocService monSoc)
        {
            _logger = logger;
            _serverApp = serverApp;
            _masSoc = masSoc;
            _monSoc = monSoc;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"GameGate is stopping."));
            _serverApp.Start();
            _masSoc.Start();
            _monSoc.Start();
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"GameGate is starting.");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"GameGate is stopping.");
            //_serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }
    }
}
