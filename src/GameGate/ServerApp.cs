using GameGate.Conf;
using GameGate.Services;
using System.Net;
using System.Threading;

namespace GameGate
{
    public class ServerApp
    {
        private static MirLog LogQueue => MirLog.Instance;
        private static ClientManager ClientManager => ClientManager.Instance;
        private static SessionManager SessionManager => SessionManager.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;
        private static ConfigManager ConfigManager => ConfigManager.Instance;

        public ServerApp()
        {

        }

        public void StartService(CancellationToken stoppingToken)
        {
            LogQueue.Log("服务已启动成功...", 2);
            LogQueue.Log("欢迎使用翎风系列游戏软件...", 0);
            LogQueue.Log("网站:http://www.gameofmir.com", 0);
            LogQueue.Log("论坛:http://bbs.gameofmir.com", 0);
            GateShare.Initialization();
            ClientManager.Initialization();
            ServerManager.Start(stoppingToken);
            if (ConfigManager.GateConfig.UseCloudGate)
            {
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.CloudAddr) || ConfigManager.GateConfig.CloudPort <= 0)
                {
                    LogQueue.Log("智能防外挂云网关服务地址配置错误.请检查配置文件是否配置正确.", 0);
                    return;
                }
                if (string.IsNullOrEmpty(ConfigManager.GateConfig.LicenseCode))
                {
                    LogQueue.Log("智能防外挂云网关授权码为空或配置错误,请检查配置文件是否配置正确.", 0);
                    return;
                }
                var cloudEndpoint = new IPEndPoint(IPAddress.Parse(ConfigManager.GateConfig.CloudAddr), ConfigManager.GateConfig.CloudPort);
                //_cloudClient.Start(cloudEndpoint);
                LogQueue.Log("智能反外挂程序已启动...", 0);
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