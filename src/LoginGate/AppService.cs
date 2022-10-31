using LoginGate.Conf;
using LoginGate.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate
{
    public class AppService : BackgroundService
    {
        private readonly MirLogger _logger;
        private readonly ConfigManager _configManager;
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;

        public AppService(MirLogger logger, ConfigManager configManager, ServerManager serverManager, ClientManager clientManager)
        {
            _logger = logger;
            _configManager = configManager;
            _serverManager = serverManager;
            _clientManager = clientManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.DebugLog("LoginGate is stopping."));
            _serverManager.Start();
            _clientManager.Start();
            _serverManager.ProcessLoginMessage(stoppingToken);
            _clientManager.ProcessSendMessage(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            GateShare.Initialization();
            _configManager.LoadConfig();
            Initialization();
            _serverManager.Start();
            _clientManager.Start();
            _serverManager.ProcessLoginMessage(cancellationToken);
            _clientManager.ProcessSendMessage(cancellationToken);
            _logger.LogInformation("服务已启动成功...", 2);
            _logger.LogInformation("欢迎使用翎风系列游戏软件...", 0);
            _logger.LogInformation("网站:http://www.gameofmir.com", 0);
            _logger.LogInformation("论坛:http://bbs.gameofmir.com", 0);
            return Task.CompletedTask;
        }

        private void Initialization()
        {
            _serverManager.Initialization();
            _clientManager.Initialization();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.DebugLog("LoginGate is stopping.");
            _logger.LogInformation("正在停止服务...", 2);
            _serverManager.Stop();
            _clientManager.Stop();
            _logger.LogInformation("服务停止成功...", 2);
            return base.StopAsync(cancellationToken);
        }
    }
}