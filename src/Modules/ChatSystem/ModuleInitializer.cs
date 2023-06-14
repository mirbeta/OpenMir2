using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SystemModule;

namespace ChatSystem
{
    public class ModuleInitializer : IModuleInitializer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Configure(IHostEnvironment env)
        {
            throw new NotImplementedException();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IChatService, ChatService>();
            //serviceCollection.AddTransient<INotificationHandler<GameMessageEvent>, MessageEventHandler>();
        }

        public void Startup(CancellationToken cancellationToken = default)
        {
            logger.Info("聊天系统插件启动...");
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            logger.Info("聊天系统插件停止...");
        }
    }
}