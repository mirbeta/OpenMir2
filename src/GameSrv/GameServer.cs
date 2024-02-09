using GameSrv.Maps;
using M2Server;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SystemModule;

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
            _ = GameShare.GeneratorProcessor.StartAsync(stoppingToken);
            _ = GameShare.SystemProcess.StartAsync(stoppingToken);
            _ = GameShare.UserProcessor.StartAsync(stoppingToken);
            _ = GameShare.MerchantProcessor.StartAsync(stoppingToken);
            _ = GameShare.EventProcessor.StartAsync(stoppingToken);
            _ = GameShare.CharacterDataProcessor.StartAsync(stoppingToken);
            _ = GameShare.TimedRobotProcessor.StartAsync(stoppingToken);
            _ = GameShare.ActorBuffProcessor.StartAsync(stoppingToken);
            Map.StartMakeStoneThread();

            var modules = serviceProvider.GetServices<IModuleInitializer>();

            foreach (var module in modules)
            {
                module.Startup(stoppingToken);
            }

            await M2Share.NetChannel.Start(stoppingToken);

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
            await GameShare.CharacterDataProcessor.StopAsync(cancellationToken);
            await GameShare.TimedRobotProcessor.StopAsync(cancellationToken);
            await GameShare.ActorBuffProcessor.StopAsync(cancellationToken);
            await M2Share.NetChannel.StopAsync(cancellationToken);
            GameShare.DataServer.Stop();

            _logger.Info("游戏世界服务线程停止...");
        }
    }
}