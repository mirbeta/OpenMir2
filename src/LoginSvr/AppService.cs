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
        private readonly MasSocService _masSocService;
        private readonly MonSocService _monSocService;
        private readonly LoginService _loginService;
        private Timer _monitorTimer;

        public AppService(ILogger<AppService> logger, AppServer serverApp, MasSocService masSocService, MonSocService monSocService,
            LoginService loginService)
        {
            _logger = logger;
            _serverApp = serverApp;
            _masSocService = masSocService;
            _monSocService = monSocService;
            _loginService = loginService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"LoginSvr is stopping."));
            await _loginService.StartConsumer();
            _monitorTimer = new Timer(ShowLogTimer, null, 1000, 2000);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginSvr is starting.");
            _serverApp.Start();
            _monSocService.Start();
            _loginService.LoadConfig();
            _loginService.Start();
            _masSocService.Start();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginSvr is stopping.");
            _serverApp.Stop();
            return base.StopAsync(cancellationToken);
        }

        private void ShowLogTimer(object obj)
        {
            _serverApp.CheckServerStatus();
        }
    }
}
