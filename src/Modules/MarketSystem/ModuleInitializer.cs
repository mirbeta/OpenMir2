using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SystemModule;

namespace MarketSystem
{
    public class ModuleInitializer : IModuleInitializer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Configure(IHostEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMarketService, MarketService>();
        }

        public void Startup(CancellationToken cancellationToken = default)
        {
            logger.Info("Marker寄售行插件启动...");
            SystemShare.ServiceProvider.GetService<IMarketService>().Start();
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            logger.Info("Marker寄售行插件停止...");
            SystemShare.ServiceProvider.GetService<IMarketService>().Stop();
        }
    }
}