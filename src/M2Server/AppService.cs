using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace M2Server
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly MirApp _mirApp;
        
        public AppService(ILogger<AppService> logger, MirApp serverApp)
        {
            _logger = logger;
            _mirApp = serverApp;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"M2Server is stopping."));
            Task.Run(async () =>
            {
                await M2Share.RunSocket.StartConsumer();
            }, stoppingToken);
            return Task.CompletedTask;
        }
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"M2Server is starting.");
            _mirApp.StartServer(cancellationToken);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"M2Server is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}