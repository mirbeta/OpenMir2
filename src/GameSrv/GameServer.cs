using GameSrv.Maps;

namespace GameSrv
{
    public class ServerBase
    {

        private readonly IServiceProvider serviceProvider;

        protected ServerBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartUp(CancellationToken stoppingToken)
        {
            _ = GameShare.GeneratorProcessor.StartAsync(stoppingToken);
            _ = GameShare.SystemProcess.StartAsync(stoppingToken);
            _ = GameShare.UserProcessor.StartAsync(stoppingToken);
            _ = GameShare.MerchantProcessor.StartAsync(stoppingToken);
            _ = GameShare.EventProcessor.StartAsync(stoppingToken);
            _ = GameShare.CharacterDataProcessor.StartAsync(stoppingToken);
            _ = GameShare.TimedRobotProcessor.StartAsync(stoppingToken);
            _ = GameShare.ActorBuffProcessor.StartAsync(stoppingToken);
            Map.StartMakeStoneThread();

            IEnumerable<IModuleInitializer> modules = serviceProvider.GetServices<IModuleInitializer>();

            foreach (IModuleInitializer module in modules)
            {
                module.Startup(stoppingToken);
            }

            await M2Share.NetChannel.Start(stoppingToken);

            LogService.Info("初始化游戏世界服务线程完成...");
        }

        public async Task Stopping(CancellationToken cancellationToken)
        {
            IEnumerable<IModuleInitializer> modules = serviceProvider.GetServices<IModuleInitializer>();
            foreach (IModuleInitializer module in modules)
            {
                module.Stopping(cancellationToken);
            }

            await GameShare.GeneratorProcessor.StopAsync(cancellationToken);
            await GameShare.SystemProcess.StopAsync(cancellationToken);
            await GameShare.UserProcessor.StopAsync(cancellationToken);
            await GameShare.MerchantProcessor.StopAsync(cancellationToken);
            await GameShare.EventProcessor.StopAsync(cancellationToken);
            await GameShare.CharacterDataProcessor.StopAsync(cancellationToken);
            await GameShare.TimedRobotProcessor.StopAsync(cancellationToken);
            await GameShare.ActorBuffProcessor.StopAsync(cancellationToken);
            await M2Share.NetChannel.StopAsync(cancellationToken);
            GameShare.DataServer.Stop();

            LogService.Info("游戏世界服务线程停止...");
        }
    }
}