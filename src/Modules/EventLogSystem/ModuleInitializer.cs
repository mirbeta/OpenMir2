using EventLogSystem.Event;
using EventLogSystem.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMir2;
using SystemModule;
using SystemModule.ModuleEvent;

namespace EventLogSystem
{
    public class ModuleInitializer : IModuleInitializer
    {
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
            LogService.Info("GameEvent(日志事件)插件启动...");
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            LogService.Info("GameEvent(日志事件)插件停止...");
        }
    }
}