using LoginSvr.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LoginSvr
{
    public class AppService : BackgroundService
    {
        private readonly AppServer _serverApp;

        private readonly MirLog _logger;
        private readonly MasSocService _masSocService;
        private readonly LoginService _loginService;

        public AppService(MirLog logger, AppServer appServer, MasSocService masSocService, LoginService loginService)
        {
            _logger = logger;
            _serverApp = appServer;
            _masSocService = masSocService;
            _loginService = loginService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"LoginSvr is stopping."));
            _loginService.StartThread(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginSvr is starting.");
            LsShare.Initialization();
            _serverApp.Start();
            _loginService.LoadConfig();
            _loginService.Start();
            _masSocService.Start();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginSvr is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}