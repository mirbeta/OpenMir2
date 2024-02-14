using GameGate.Conf;
using GameGate.Filters;
using GameGate.Services;

namespace GameGate
{
    public class AppService : IHostedLifecycleService
    {
        private static ConfigManager ConfigManager => ConfigManager.Instance;
        private static SessionContainer SessionContainer => SessionContainer.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            LogService.Debug("GameGate is starting.");
            LogService.Info("正在启动服务...");
            LogService.Info("正在加载配置信息...");
            GateShare.Initialization();
            GateShare.Load();
            ConfigManager.LoadConfig();
            ConfigManager.SaveConfig();
            GateShare.HardwareFilter = new HardwareFilter();
            LogService.Info("配置信息加载完成...");
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ServerManager.Initialize();
            ServerManager.StartServerThreadMessageWork(cancellationToken);
            _ = ServerManager.StartClientMessageWork(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            if (ConfigManager.GateConfig.UseCloudGate)
            {
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.CloudAddr) || ConfigManager.GateConfig.CloudPort <= 0)
                {
                    LogService.Info("智能防外挂云网关服务地址配置错误.请检查配置文件是否配置正确.");
                }
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.LicenseCode))
                {
                    LogService.Info("智能防外挂云网关授权码为空或配置错误,请检查配置文件是否配置正确.");
                }
                //var cloudEndpoint = new IPEndPoint(IPAddress.Parse(ConfigManager.GateConfig.CloudAddr), ConfigManager.GateConfig.CloudPort);
                //_cloudClient.Start(cloudEndpoint);
                LogService.Info("智能反外挂程序已启动...");
            }
            LogService.Info("服务已启动成功...");
            LogService.Info("欢迎使用翎风系列游戏软件...");
            LogService.Info("网站:http://www.gameofmir.com");
            LogService.Info("论坛:http://bbs.gameofmir.com");
            ServerManager.Start(cancellationToken);
            //await SessionContainer.ProcessSendMessage(stoppingToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            LogService.Info("正在停止服务...");
            ServerManager.Stop();
            LogService.Info("服务停止成功...");
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            LogService.Info("正在停止服务...");
            return Task.CompletedTask;
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            LogService.Info("服务已停止...");
            return Task.CompletedTask;
        }
    }
}