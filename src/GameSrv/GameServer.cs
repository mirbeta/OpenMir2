using M2Server;
using M2Server.Maps;
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
            await GameShare.GeneratorProcessor.StartAsync(stoppingToken);
            await GameShare.SystemProcess.StartAsync(stoppingToken);
            await GameShare.UserProcessor.StartAsync(stoppingToken);
            await GameShare.RobotProcessor.StartAsync(stoppingToken);
            await GameShare.MerchantProcessor.StartAsync(stoppingToken);
            await GameShare.EventProcessor.StartAsync(stoppingToken);
            await GameShare.StorageProcessor.StartAsync(stoppingToken);
            await GameShare.TimedRobotProcessor.StartAsync(stoppingToken);
            await GameShare.SocketMgr.StartMessageThread();
            await GameShare.ChatChannel.Start();
            GameShare.DataServer.Start();
            GameShare.MarketService.Start();
            GameShare.SocketMgr.Start();
            _logger.Info("初始化游戏世界服务线程完成...");
            GameShare.StartReady = true;
            Map.StartMakeStoneThread();
        }

        public static async Task Stopping(CancellationToken cancellationToken)
        {
            await GameShare.GeneratorProcessor.StopAsync(cancellationToken);
            await GameShare.SystemProcess.StopAsync(cancellationToken);
            await GameShare.UserProcessor.StopAsync(cancellationToken);
            await GameShare.RobotProcessor.StopAsync(cancellationToken);
            await GameShare.MerchantProcessor.StopAsync(cancellationToken);
            await GameShare.EventProcessor.StopAsync(cancellationToken);
            await GameShare.StorageProcessor.StopAsync(cancellationToken);
            await GameShare.TimedRobotProcessor.StopAsync(cancellationToken);
            await GameShare.SocketMgr.StopAsync();
            GameShare.DataServer.Stop();
            GameShare.MarketService.Stop();
            await GameShare.ChatChannel.Stop();
        }

        private static void ProcessGameNotice()
        {
            if (GameShare.Config.SendOnlineCount && (HUtil32.GetTickCount() - GameShare.SendOnlineTick) > GameShare.Config.SendOnlineTime)
            {
                GameShare.SendOnlineTick = HUtil32.GetTickCount();
                string sMsg = string.Format(Settings.SendOnlineCountMsg, HUtil32.Round(M2Share.WorldEngine.OnlinePlayObject * (GameShare.Config.SendOnlineCountRate / 10.0)));
                M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
            }
        }

        public static void SaveItemNumber()
        {
            ProcessGameNotice();
            GameShare.ServerConf.SaveVariable();
        }
    }
}