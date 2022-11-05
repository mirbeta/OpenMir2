using GameGate.Conf;
using GameGate.Filters;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    public class AppService : BackgroundService
    {
        private readonly ServerApp _serverApp;
        private static MirLog LogQueue => MirLog.Instance;
        private static ConfigManager ConfigManager => ConfigManager.Instance;

        public AppService(ServerApp serverApp)
        {
            _serverApp = serverApp;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Debug.WriteLine($"GameGate is stopping."));
            _serverApp.StartService(stoppingToken);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            LogQueue.DebugLog("GameGate is starting.");
            LogQueue.Log("正在启动服务...", 2);
            LogQueue.Log("正在加载配置信息...", 3);
            ConfigManager.LoadConfig();
            GateShare.HardwareFilter = new HardwareFilter();
            LogQueue.Log("配置信息加载完成...", 3);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("GameGate is stopping.");
            LogQueue.Log("正在停止服务...", 2);
            _serverApp.StopService();
            LogQueue.Log("服务停止成功...", 2);
            return base.StopAsync(cancellationToken);
        }
    }
}