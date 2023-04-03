using Microsoft.Extensions.Hosting;
using NLog;
using SelGate.Conf;
using SelGate.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SelGate
{
    public class AppService : BackgroundService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.Debug("SelGate is stopping."));
            _serverService.ProcessReviceMessage(stoppingToken);
            _sessionManager.ProcessSendMessage(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("SelGate is starting.");
            GateShare.Initialization();
            _logger.Info("正在启动服务...", 0);
            _configManager.LoadConfig();
            _serverService.Start();
            _clientManager.Initialization();
            _clientManager.Start();
            _logger.Info("服务已启动成功...", 2);
            _logger.Info("欢迎使用翎风系列游戏软件...", 0);
            _logger.Info("网站:http://www.gameofmir.com", 0);
            _logger.Info("论坛:http://bbs.gameofmir.com", 0);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("SelGate is stopping.");
            _logger.Info("正在停止服务...", 2);
            _serverService.Stop();
            _clientManager.Stop();
            _logger.Info("服务停止成功...", 2);
            return base.StopAsync(cancellationToken);
        }
    }
}