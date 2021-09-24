using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace LoginGate
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly ServerApp _serverApp;
        private readonly GateServer _gateServer;
        private readonly GateClient _gateClient;
        private Timer _appThreadTimer;

        public AppService(ILogger<AppService> logger, ServerApp serverApp, GateServer gateServer, GateClient gateClient)
        {
            _logger = logger;
            _serverApp = serverApp;
            _gateServer = gateServer;
            _gateClient = gateClient;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"LoginGate is stopping."));
            _serverApp.Start();
            _gateServer.Start();
            _gateClient.Start();
            _appThreadTimer = new Timer(AppThreadTimer, null, 0, 3000);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginGate is starting.");
            _serverApp.StartService();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginGate is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }

        private void AppThreadTimer(object obj)
        {
            _serverApp.SendTimerTimer();
            _gateClient.CheckConnected();
        }
    }
}
