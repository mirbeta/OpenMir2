using LoginSrv.Conf;
using LoginSrv.Services;
using LoginSrv.Storage;

namespace LoginSrv
{
    public class AppService : IHostedLifecycleService
    {
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

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            LsShare.Initialization();
            _configManager.LoadConfig();
            _configManager.LoadAddrTable();
            return Task.CompletedTask;
        }

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            _loginService.Start(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _loginService.StartServer();
            _masSocService.StartServer();
            _accountStorage.Initialization();
            LogService.Info("服务已启动成功...");
            LogService.Info("欢迎使用翎风系列游戏软件...");
            LogService.Info("网站:http://www.gameofmir.com");
            LogService.Info("论坛:http://bbs.gameofmir.com");
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _loginService.StopServer();
            _masSocService.StopServer();
            return Task.CompletedTask;
        }
    }
}