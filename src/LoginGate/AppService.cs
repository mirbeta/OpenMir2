using LoginGate.Conf;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate
{
    public class AppService : BackgroundService
    {
        private readonly ServerApp _serverApp;
        private readonly MirLogger _logQueue;
        private readonly ConfigManager _configManager;

        public AppService(MirLogger logQueue,ServerApp serverApp, ConfigManager configManager)
        {
            _serverApp = serverApp;
            _configManager = configManager;
            _logQueue = logQueue;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logQueue.DebugLog($"GameGate is stopping."));
            _serverApp.Start(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logQueue.DebugLog($"LoginGate is starting.");
            _logQueue.LogInformation("正在启动服务...", 2);
            GateShare.Initialization();
            _configManager.LoadConfig();
            _serverApp.Initialization();
            _logQueue.LogInformation("服务已启动成功...", 2);
            _logQueue.LogInformation("欢迎使用翎风系列游戏软件...", 0);
            _logQueue.LogInformation("网站:http://www.gameofmir.com", 0);
            _logQueue.LogInformation("论坛:http://bbs.gameofmir.com", 0);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logQueue.DebugLog($"LoginGate is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }
    }
}