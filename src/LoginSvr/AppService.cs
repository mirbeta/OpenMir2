using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LoginSvr
{
    public class AppService : BackgroundService
    {
        private readonly LogQueue _logQueue;
        private readonly AppServer _serverApp;
        private readonly MasSocService _masSocService;
        private readonly MonSocService _monSocService;
        private readonly LoginService _loginService;

        public AppService(LogQueue logQueue, AppServer serverApp, MasSocService masSocService, MonSocService monSocService,
            LoginService loginService)
        {
            _logQueue = logQueue;
            _serverApp = serverApp;
            _masSocService = masSocService;
            _monSocService = monSocService;
            _loginService = loginService;
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
