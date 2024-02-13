using Microsoft.Extensions.Hosting;
using OpenMir2;
using SelGate.Conf;
using SelGate.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SelGate
{
    public class AppService : IHostedLifecycleService
    {
        private readonly ConfigManager _configManager;
        private readonly ServerService _serverService;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;

        public AppService(ConfigManager configManager, ServerService serverService, ClientManager clientManager, SessionManager sessionManager)
        {
            _configManager = configManager;
            _serverService = serverService;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
        }

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            GateShare.Initialization();
            LogService.Info("正在启动服务...");
            _configManager.LoadConfig();
            _clientManager.Initialization();
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _serverService.Start();
            _clientManager.Start();
            return Task.CompletedTask;
        }

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            _serverService.ProcessReviceMessage(cancellationToken);
            _sessionManager.ProcessSendMessage(cancellationToken);
            LogService.Info("服务已启动成功...");
            LogService.Info("欢迎使用翎风系列游戏软件...");
            LogService.Info("网站:http://www.gameofmir.com");
            LogService.Info("论坛:http://bbs.gameofmir.com");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            LogService.Debug("SelGate is stopping.");
            LogService.Info("正在停止服务...");
            _serverService.Stop();
            _clientManager.Stop();
            LogService.Info("服务停止成功...");
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
    }
}