using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using SelGate.Conf;
using SelGate.Services;
using SystemModule;
using SystemModule.Logger;

namespace SelGate
{
    public class AppService : BackgroundService
    {
        private readonly MirLogger _logger;
        private readonly ConfigManager _configManager;
        private readonly ServerService _serverService;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;

        public AppService(MirLogger logQueue, ConfigManager configManager, ServerService serverService, ClientManager clientManager, SessionManager sessionManager)
        {
            _logger = logQueue;
            _configManager = configManager;
            _serverService = serverService;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.DebugLog("SelGate is stopping."));
            _serverService.ProcessReviceMessage(stoppingToken);
            _sessionManager.ProcessSendMessage(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.DebugLog("SelGate is starting.");
            GateShare.Initialization();
            _logger.LogInformation("正在启动服务...", 0);
            _configManager.LoadConfig();
            _serverService.Start();
            _clientManager.Initialization();
            _clientManager.Start();
            _logger.LogInformation("服务已启动成功...", 2);
            _logger.LogInformation("欢迎使用翎风系列游戏软件...", 0);
            _logger.LogInformation("网站:http://www.gameofmir.com", 0);
            _logger.LogInformation("论坛:http://bbs.gameofmir.com", 0);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.DebugLog("SelGate is stopping.");
            _logger.LogInformation("正在停止服务...", 2);
            _serverService.Stop();
            _clientManager.Stop();
            _logger.LogInformation("服务停止成功...", 2);
            return base.StopAsync(cancellationToken);
        }
    }
}