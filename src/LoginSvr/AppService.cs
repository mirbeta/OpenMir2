using LoginSvr.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using LoginSvr.Conf;
using LoginSvr.Storage;

namespace LoginSvr
{
    public class AppService : BackgroundService
    {
        private readonly AppServer _serverApp;
        private readonly MirLog _logger;
        private readonly ConfigManager _configManager;
        private readonly SessionService _masSocService;
        private readonly LoginService _loginService;
        private readonly AccountStorage _accountStorage;

        public AppService(MirLog logger, AppServer appServer, SessionService masSocService, LoginService loginService, AccountStorage accountStorage, ConfigManager configManager)
        {
            _logger = logger;
            _serverApp = appServer;
            _masSocService = masSocService;
            _loginService = loginService;
            _accountStorage = accountStorage;
            _configManager = configManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"LoginSvr is stopping."));
            _loginService.Start(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginSvr is starting.");
            LsShare.Initialization();
            _serverApp.Start();
            LoadConfig();
            _loginService.StartServer();
            _masSocService.StartServer();
            _accountStorage.Initialization();
            return base.StartAsync(cancellationToken);
        }

        private void LoadConfig()
        {
            _configManager.LoadConfig();
            _configManager.LoadAddrTable();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"LoginSvr is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}