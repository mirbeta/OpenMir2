using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameGate
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly ServerApp _serverApp;

        public AppService(ILogger<AppService> logger, ServerApp serverApp)
        {
            _logger = logger;
            _serverApp = serverApp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"GameGate is stopping."));
            await _serverApp.StartProcessMessageService();
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
    }
}