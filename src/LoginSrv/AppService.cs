using LoginSrv.Conf;
using LoginSrv.Services;
using LoginSrv.Storage;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

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
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}