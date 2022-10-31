using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace SelGate
{
    public class AppService : BackgroundService
    {
        private readonly MirLogger _logQueue;
        private readonly ServerApp _serverApp;

        public AppService(MirLogger logQueue, ServerApp serverApp)
        {
            _logQueue = logQueue;
            _serverApp = serverApp;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logQueue.DebugLog("GameGate is stopping."));
            _serverApp.Start(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logQueue.DebugLog("GameGate is starting.");
            _serverApp.StartService();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logQueue.DebugLog("GameGate is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }
    }
}