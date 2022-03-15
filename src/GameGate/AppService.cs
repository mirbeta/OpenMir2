using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    public class AppService : BackgroundService
    {
        private readonly ServerApp _serverApp;
        private LogQueue LogQueue => LogQueue.Instance;
        private ConfigManager ConfigManager => ConfigManager.Instance;

        public AppService(ServerApp serverApp)
        {
            _serverApp = serverApp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Debug.WriteLine($"GameGate is stopping."));
            LogQueue.Enqueue("服务已启动成功...", 2);
            LogQueue.Enqueue("欢迎使用翎风系列游戏软件...", 0);
            LogQueue.Enqueue("网站:http://www.gameofmir.com", 0);
            LogQueue.Enqueue("论坛:http://bbs.gameofmir.com", 0);
            LogQueue.Enqueue("智能反外挂程序已启动...", 0);
            LogQueue.Enqueue("智能反外挂程序云端已连接...", 0);
            _serverApp.StartService();
            await _serverApp.Start();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            LogQueue.EnqueueDebugging("GameGate is starting.");
            LogQueue.Enqueue("正在启动服务...", 2);
            LogQueue.Enqueue("正在加载配置信息...", 3);
            ConfigManager.LoadConfig();
            GateShare.HWFilter = new HWIDFilter();
            LogQueue.Enqueue("配置信息加载完成...", 3);
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