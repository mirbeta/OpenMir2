using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SystemModule;

namespace SelGate
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly ServerApp _serverApp;
        private readonly GateServer _gateServer;
        private readonly GateClient _gateClient;
        private int dwReConnectServerTick = 0;
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
            stoppingToken.Register(() => _logger.LogDebug($"SelGate is stopping."));
            _serverApp.Start();
            _gateServer.Start();
            _gateClient.Start();
            _appThreadTimer = new Timer(AppThreadTimer, null, 0, 1000);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"SelGate is starting.");
            dwReConnectServerTick = HUtil32.GetTickCount() - 25 * 1000;
            _serverApp.StartService();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"SelGate is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }

        private void AppThreadTimer(object obj)
        {
            _serverApp.ShowMainLogMsg();
            _serverApp.ClearTimer();
            if (!GateShare.boGateReady && (GateShare.boServiceStart))
            {
                if ((HUtil32.GetTickCount() - dwReConnectServerTick) > 5000)// 30 * 1000
                {
                    dwReConnectServerTick = HUtil32.GetTickCount();
                    _gateClient.CheckConnected();
                }
            }
        }
    }
}