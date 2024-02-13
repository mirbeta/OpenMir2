using Microsoft.Extensions.Hosting;
using SelGate.Conf;
using SelGate.Services;
using System.Threading;
using System.Threading.Tasks;
using OpenMir2;

namespace SelGate
{
    public class AppService : BackgroundService
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => LogService.Debug("SelGate is stopping."));
            _serverService.ProcessReviceMessage(stoppingToken);
            _sessionManager.ProcessSendMessage(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            LogService.Debug("SelGate is starting.");
            GateShare.Initialization();
            LogService.Info("正在启动服务...", 0);
            _configManager.LoadConfig();
            _serverService.Start();
            _clientManager.Initialization();
            _clientManager.Start();
            LogService.Info("服务已启动成功...", 2);
            LogService.Info("欢迎使用翎风系列游戏软件...", 0);
            LogService.Info("网站:http://www.gameofmir.com", 0);
            LogService.Info("论坛:http://bbs.gameofmir.com", 0);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            LogService.Debug("SelGate is stopping.");
            LogService.Info("正在停止服务...", 2);
            _serverService.Stop();
            _clientManager.Stop();
            LogService.Info("服务停止成功...", 2);
            return base.StopAsync(cancellationToken);
        }
    }
}