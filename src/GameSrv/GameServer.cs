using GameSrv.Maps;
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

        public async Task StartUp(CancellationToken stoppingToken)
        {
            await M2Share.GeneratorProcessor.StartAsync(stoppingToken);
            await M2Share.SystemProcess.StartAsync(stoppingToken);
            await M2Share.UserProcessor.StartAsync(stoppingToken);
            await M2Share.RobotProcessor.StartAsync(stoppingToken);
            await M2Share.MerchantProcessor.StartAsync(stoppingToken);
            await M2Share.EventProcessor.StartAsync(stoppingToken);
            await M2Share.StorageProcessor.StartAsync(stoppingToken);
            await M2Share.TimedRobotProcessor.StartAsync(stoppingToken);
            await M2Share.SocketMgr.StartMessageThread();
            await M2Share.ChatChannel.Start();
            M2Share.DataServer.Start();
            M2Share.MarketService.Start();
            M2Share.SocketMgr.Start();
            _logger.Info("初始化游戏世界服务线程完成...");
            M2Share.StartReady = true;
            Map.StartMakeStoneThread();
        }

        public static async Task Stopping(CancellationToken cancellationToken)
        {
            await M2Share.GeneratorProcessor.StopAsync(cancellationToken);
            await M2Share.SystemProcess.StopAsync(cancellationToken);
            await M2Share.UserProcessor.StopAsync(cancellationToken);
            await M2Share.RobotProcessor.StopAsync(cancellationToken);
            await M2Share.MerchantProcessor.StopAsync(cancellationToken);
            await M2Share.EventProcessor.StopAsync(cancellationToken);
            await M2Share.StorageProcessor.StopAsync(cancellationToken);
            await M2Share.TimedRobotProcessor.StopAsync(cancellationToken);
            await M2Share.SocketMgr.StopAsync();
            M2Share.DataServer.Stop();
            M2Share.MarketService.Stop();
            await M2Share.ChatChannel.Stop();
        }

        private static void ProcessGameNotice()
        {
            if (M2Share.Config.SendOnlineCount && (HUtil32.GetTickCount() - M2Share.SendOnlineTick) > M2Share.Config.SendOnlineTime)
            {
                M2Share.SendOnlineTick = HUtil32.GetTickCount();
                string sMsg = string.Format(Settings.SendOnlineCountMsg, HUtil32.Round(M2Share.WorldEngine.OnlinePlayObject * (M2Share.Config.SendOnlineCountRate / 10.0)));
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