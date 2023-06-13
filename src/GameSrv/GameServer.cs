using GameSrv.Maps;
using M2Server;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SystemModule.Enums;

namespace GameSrv
{
    public class ServerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceProvider serviceProvider;

        protected ServerBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartUp(CancellationToken stoppingToken)
        {
            var modules = serviceProvider.GetServices<IModuleInitializer>();

            foreach (var module in modules)
            {
                module.Startup(stoppingToken);
            }

            await GameShare.GeneratorProcessor.StartAsync(stoppingToken);
            await GameShare.SystemProcess.StartAsync(stoppingToken);
            await GameShare.UserProcessor.StartAsync(stoppingToken);
            await GameShare.MerchantProcessor.StartAsync(stoppingToken);
            await GameShare.EventProcessor.StartAsync(stoppingToken);
            await GameShare.StorageProcessor.StartAsync(stoppingToken);
            await GameShare.TimedRobotProcessor.StartAsync(stoppingToken);
            await GameShare.SocketMgr.StartMessageThread(stoppingToken);
            Map.StartMakeStoneThread();
            _logger.Info("初始化游戏世界服务线程完成...");
        }

        public async Task Stopping(CancellationToken cancellationToken)
        {
            var modules = serviceProvider.GetServices<IModuleInitializer>();

            foreach (var module in modules)
            {
                module.Stopping(cancellationToken);
            }

            await GameShare.GeneratorProcessor.StopAsync(cancellationToken);
            await GameShare.SystemProcess.StopAsync(cancellationToken);
            await GameShare.UserProcessor.StopAsync(cancellationToken);
            await GameShare.MerchantProcessor.StopAsync(cancellationToken);
            await GameShare.EventProcessor.StopAsync(cancellationToken);
            await GameShare.StorageProcessor.StopAsync(cancellationToken);
            await GameShare.TimedRobotProcessor.StopAsync(cancellationToken);
            await GameShare.SocketMgr.StopAsync(cancellationToken);
            GameShare.DataServer.Stop();
        }

        private static void ProcessGameNotice()
        {
            if (SystemShare.Config.SendOnlineCount && (HUtil32.GetTickCount() - GameShare.SendOnlineTick) > SystemShare.Config.SendOnlineTime)
            {
                GameShare.SendOnlineTick = HUtil32.GetTickCount();
                string sMsg = string.Format(Settings.SendOnlineCountMsg, HUtil32.Round(M2Share.WorldEngine.OnlinePlayObject * (SystemShare.Config.SendOnlineCountRate / 10.0)));
                M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
            }
        }

        public static void SaveItemNumber()
        {
            ProcessGameNotice();
            SystemShare.ServerConf.SaveVariable();
        }
    }
}