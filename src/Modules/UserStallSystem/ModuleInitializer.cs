using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMir2;
using SystemModule;

namespace UserStallSystem
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Configure(IHostEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddSingleton<IMarketService, MarketService>();
            //serviceCollection.AddTransient<INotificationHandler<UserSelectMessageEvent>, MessageEventHandler>();
        }

        public void Startup(CancellationToken cancellationToken = default)
        {
            LogService.Info("UserStall(摆摊)插件启动...");
            //SystemShare.ServiceProvider.GetService<IMarketService>().Start();
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            LogService.Info("UserStall(摆摊)插件停止...");
            //SystemShare.ServiceProvider.GetService<IMarketService>().Stop();
        }
    }
}