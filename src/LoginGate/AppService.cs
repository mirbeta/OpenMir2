using LoginGate.Conf;
using LoginGate.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SystemModule.Logger;

namespace LoginGate
{
    public class AppService : BackgroundService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConfigManager _configManager;
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;

        public AppService(ConfigManager configManager, ServerManager serverManager, ClientManager clientManager)
        {
            _configManager = configManager;
            _serverManager = serverManager;
            _clientManager = clientManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.Debug("LoginGate is stopping."));
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
            _logger.Info("服务已启动成功...", 2);
            _logger.Info("欢迎使用翎风系列游戏软件...", 0);
            _logger.Info("网站:http://www.gameofmir.com", 0);
            _logger.Info("论坛:http://bbs.gameofmir.com", 0);
            base.StartAsync(cancellationToken);
            return Task.CompletedTask;
        }

        private void Initialization()
        {
            _serverManager.Initialization();
            _clientManager.Initialization();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("LoginGate is stopping.");
            _logger.Info("正在停止服务...", 2);
            _serverManager.Stop();
            _clientManager.Stop();
            _logger.Info("服务停止成功...", 2);
            return base.StopAsync(cancellationToken);
        }
    }
}