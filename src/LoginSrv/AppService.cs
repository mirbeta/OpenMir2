using LoginSrv.Conf;
using LoginSrv.Services;
using LoginSrv.Storage;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading;
using System.Threading.Tasks;

namespace LoginSrv
{
    public class AppService : BackgroundService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConfigManager _configManager;
        private readonly SessionServer _masSocService;
        private readonly LoginServer _loginService;
        private readonly AccountStorage _accountStorage;

        public AppService(SessionServer masSocService, LoginServer loginService, AccountStorage accountStorage, ConfigManager configManager)
        {
            _masSocService = masSocService;
            _loginService = loginService;
            _accountStorage = accountStorage;
            _configManager = configManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.Debug("LoginSrv is stopping."));
            _loginService.Start(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("LoginSrv is starting.");
            LsShare.Initialization();
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
            _logger.Debug("LoginSrv is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}