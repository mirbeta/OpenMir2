using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    public class AppService : BackgroundService
    {
        private readonly LogQueue _logQueue;
        private readonly ConfigManager _configManager;
        private readonly ServerApp _serverApp;

        public AppService(ServerApp serverApp, ConfigManager configManager, LogQueue logQueue)
        {
            _serverApp = serverApp;
            _configManager = configManager;
            _logQueue = logQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Debug.WriteLine($"GameGate is stopping."));
            _serverApp.StartService();
            await _serverApp.Start();
            _logQueue.Enqueue("服务已启动成功...", 2);
            _logQueue.Enqueue("欢迎使用翎风系列游戏软件...", 0);
            _logQueue.Enqueue("网站:http://www.gameofmir.com", 0);
            _logQueue.Enqueue("论坛:http://bbs.gameofmir.com", 0);
            _logQueue.Enqueue("智能反外挂程序已启动...", 0);
            _logQueue.Enqueue("智能反外挂程序云端已连接...", 0);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logQueue.EnqueueDebugging("GameGate is starting.");
            _logQueue.Enqueue("正在启动服务...", 2);
            _logQueue.Enqueue("正在加载配置信息...", 3);
            _configManager.LoadConfig();
            GateShare.HWFilter = new HWIDFilter(_configManager);
            _logQueue.Enqueue("配置信息加载完成...", 3);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("GameGate is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }
    }
}