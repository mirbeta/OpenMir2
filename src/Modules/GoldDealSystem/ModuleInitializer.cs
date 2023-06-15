using GoldDealSystem.Service;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SystemModule;
using SystemModule.ModuleEvent;

namespace GoldDealSystem
{
    public class ModuleInitializer : IModuleInitializer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Configure(IHostEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IGoldDealService, GoldDealService>();
            //serviceCollection.AddTransient<INotificationHandler<UserSelectMessageEvent>, MessageEventHandler>();
        }

        public void Startup(CancellationToken cancellationToken = default)
        {
            logger.Info("GoldDeal(元宝交易)插件启动...");
            //SystemShare.ServiceProvider.GetService<IMarketService>().Start();
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            logger.Info("GoldDeal(元宝交易)插件停止...");
            //SystemShare.ServiceProvider.GetService<IMarketService>().Stop();
        }
    }
}