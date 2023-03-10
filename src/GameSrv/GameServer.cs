using NLog;
using SystemModule.Enums;

namespace GameSrv
{
    public class ServerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ServerBase()
        {
            
        }

        public async Task StartWorld(CancellationToken stoppingToken)
        {
            await M2Share.SystemProcess.StartAsync(stoppingToken);
            await M2Share.UserProcessor.StartAsync(stoppingToken);
            await M2Share.RobotProcessor.StartAsync(stoppingToken);
            await M2Share.MerchantProcessor.StartAsync(stoppingToken);
            await M2Share.GeneratorProcessor.StartAsync(stoppingToken);
            await M2Share.EventProcessor.StartAsync(stoppingToken);
            M2Share.RobotMgr.Start();
            M2Share.DataServer.Start();
            M2Share.GateMgr.Start(stoppingToken);
            M2Share.UsrRotCountTick = HUtil32.GetTickCount();
            _logger.Info("启动游戏世界和环境服务线程...");
        }

        public static void Stop(CancellationToken cancellationToken)
        {
            M2Share.DataServer.Stop();
            M2Share.GateMgr.Stop();
            M2Share.RobotMgr.Stop();
            M2Share.GeneratorProcessor.StopAsync(cancellationToken);
            M2Share.SystemProcess.StopAsync(cancellationToken);
            M2Share.UserProcessor.StopAsync(cancellationToken);
            M2Share.RobotProcessor.StopAsync(cancellationToken);
            M2Share.MerchantProcessor.StopAsync(cancellationToken);
            M2Share.EventProcessor.StopAsync(cancellationToken);
        }

        private static void ProcessGameNotice()
        {
            if (M2Share.Config.SendOnlineCount && (HUtil32.GetTickCount() - M2Share.SendOnlineTick) > M2Share.Config.SendOnlineTime)
            {
                M2Share.SendOnlineTick = HUtil32.GetTickCount();
                string sMsg = string.Format(Settings.SendOnlineCountMsg, HUtil32.Round(M2Share.WorldEngine.OnlinePlayObject * (M2Share.Config.SendOnlineCountRate / 10)));
                M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
            }
        }

        public static void SaveItemNumber()
        {
            ProcessGameNotice();
            M2Share.ServerConf.SaveVariable();
        }
    }
}