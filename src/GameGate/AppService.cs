using GameGate.Conf;
using GameGate.Filters;
using GameGate.Services;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    public class AppService : BackgroundService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static ConfigManager ConfigManager => ConfigManager.Instance;
        private static SessionContainer SessionContainer => SessionContainer.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;

        public AppService()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Debug.WriteLine($"GameGate is stopping."));
            if (ConfigManager.GateConfig.UseCloudGate)
            {
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.CloudAddr) || ConfigManager.GateConfig.CloudPort <= 0)
                {
                    _logger.Info("智能防外挂云网关服务地址配置错误.请检查配置文件是否配置正确.");
                }
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.LicenseCode))
                {
                    _logger.Info("智能防外挂云网关授权码为空或配置错误,请检查配置文件是否配置正确.");
                }
                //var cloudEndpoint = new IPEndPoint(IPAddress.Parse(ConfigManager.GateConfig.CloudAddr), ConfigManager.GateConfig.CloudPort);
                //_cloudClient.Start(cloudEndpoint);
                _logger.Info("智能反外挂程序已启动...");
            }
            ServerManager.Start(stoppingToken);
            await ServerManager.StartMessageWorkThread(stoppingToken);
            await SessionContainer.ProcessSendMessage(stoppingToken);
            _logger.Info("服务已启动成功...");
            _logger.Info("欢迎使用翎风系列游戏软件...");
            _logger.Info("网站:http://www.gameofmir.com");
            _logger.Info("论坛:http://bbs.gameofmir.com");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("GameGate is starting.");
            _logger.Info("正在启动服务...");
            _logger.Info("正在加载配置信息...");
            GateShare.Initialization();
            GateShare.Load();
            ConfigManager.LoadConfig();
            ConfigManager.SaveConfig();
            ServerManager.Initialize();
            GateShare.HardwareFilter = new HardwareFilter();
            _logger.Info("配置信息加载完成...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("GameGate is stopping.");
            _logger.Info("正在停止服务...");
            ServerManager.Stop();
            _logger.Info("服务停止成功...");
            return base.StopAsync(cancellationToken);
        }
    }
}