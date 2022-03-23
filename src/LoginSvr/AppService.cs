using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LoginSvr
{
    public class AppService : BackgroundService
    {
        private readonly AppServer _serverApp;

        private LogQueue _logQueue => LogQueue.Instance;
        private MasSocService _masSocService => MasSocService.Instance;
        private MonSocService _monSocService => MonSocService.Instance;
        private LoginService _loginService => LoginService.Instance;

        public AppService(AppServer appServer)
        {
            _serverApp = appServer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logQueue.EnqueueDebugging($"LoginSvr is stopping."));
            await _loginService.StartConsumer();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logQueue.EnqueueDebugging($"LoginSvr is starting.");
            LSShare.Initialization();
            _serverApp.Start();
            _monSocService.Start();
            _loginService.LoadConfig();
            _loginService.Start();
            _masSocService.Start();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logQueue.EnqueueDebugging($"LoginSvr is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}