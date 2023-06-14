using EventLogSystem.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SystemModule;
using SystemModule.ModuleEvent;

namespace EventLogSystem
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
            serviceCollection.AddSingleton<IGameEventSource, GameEventSource>();
            serviceCollection.AddTransient<INotificationHandler<GameMessageEvent>, MessageEventHandler>();
        }

        public void Startup(CancellationToken cancellationToken = default)
        {
            logger.Info("GameEvent日志事件插件启动...");
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            logger.Info("GameEvent日志事件插件停止...");
        }
    }
}