using LoginGate.Conf;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LoginGate
{
    public class AppService : BackgroundService
    {
        private readonly ServerApp _serverApp;
        private readonly LogQueue _logQueue = LogQueue.Instance;
        private ConfigManager ConfigManager => ConfigManager.Instance;

        public AppService(ServerApp serverApp)
        {
            _serverApp = serverApp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logQueue.EnqueueDebugging($"GameGate is stopping."));
            await _serverApp.Start();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logQueue.EnqueueDebugging($"GameGate is starting.");
            _logQueue.Enqueue("正在启动服务...", 2);
            GateShare.Initialization();
            ConfigManager.LoadConfig();
            _serverApp.StartService();
            _logQueue.Enqueue("服务已启动成功...", 2);
            _logQueue.Enqueue("欢迎使用翎风系列游戏软件...", 0);
            _logQueue.Enqueue("网站:http://www.gameofmir.com", 0);
            _logQueue.Enqueue("论坛:http://bbs.gameofmir.com", 0);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logQueue.EnqueueDebugging($"GameGate is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }
    }
}