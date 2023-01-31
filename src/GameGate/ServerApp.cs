using GameGate.Conf;
using GameGate.Services;
using NLog;
using System.Threading;

namespace GameGate
{
    public class ServerApp
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static SessionManager SessionManager => SessionManager.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;
        private static ConfigManager ConfigManager => ConfigManager.Instance;

        public ServerApp()
        {

        }

        public void StartService(CancellationToken stoppingToken)
        {
            _logger.Info("服务已启动成功...");
            _logger.Info("欢迎使用翎风系列游戏软件...");
            _logger.Info("网站:http://www.gameofmir.com");
            _logger.Info("论坛:http://bbs.gameofmir.com");
            GateShare.Initialization();
            GateShare.Load();
            ServerManager.Initialization();
            ServerManager.Start(stoppingToken);
            if (ConfigManager.GateConfig.UseCloudGate)
            {
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.CloudAddr) || ConfigManager.GateConfig.CloudPort <= 0)
                {
                    _logger.Info("智能防外挂云网关服务地址配置错误.请检查配置文件是否配置正确.");
                    return;
                }
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.LicenseCode))
                {
                    _logger.Info("智能防外挂云网关授权码为空或配置错误,请检查配置文件是否配置正确.");
                    return;
                }
                //var cloudEndpoint = new IPEndPoint(IPAddress.Parse(ConfigManager.GateConfig.CloudAddr), ConfigManager.GateConfig.CloudPort);
                //_cloudClient.Start(cloudEndpoint);
                _logger.Info("智能反外挂程序已启动...");
            }
            ServerManager.StartMessageWorkThread(stoppingToken);
            SessionManager.ProcessSendMessage(stoppingToken);
        }

        public void StopService()
        {
            ServerManager.Stop();
        }
    }
}