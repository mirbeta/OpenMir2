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
            await GameShare.GeneratorProcessor.StartAsync(stoppingToken);
            await GameShare.SystemProcess.StartAsync(stoppingToken);
            await GameShare.UserProcessor.StartAsync(stoppingToken);
            await GameShare.MerchantProcessor.StartAsync(stoppingToken);
            await GameShare.EventProcessor.StartAsync(stoppingToken);
            await GameShare.StorageProcessor.StartAsync(stoppingToken);
            await GameShare.TimedRobotProcessor.StartAsync(stoppingToken);
            await GameShare.ActorBuffProcessor.StopAsync(stoppingToken);
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
            await GameShare.StorageProcessor.StopAsync(cancellationToken);
            await GameShare.TimedRobotProcessor.StopAsync(cancellationToken);
            await GameShare.ActorBuffProcessor.StopAsync(cancellationToken);
            await M2Share.NetChannel.StopAsync(cancellationToken);
            GameShare.DataServer.Stop();

            _logger.Info("游戏世界服务线程停止...");
        }
    }
}