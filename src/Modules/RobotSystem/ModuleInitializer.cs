using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMir2;
using RobotSystem.Event;
using SystemModule;
using SystemModule.ModuleEvent;

namespace RobotSystem
{
    public class ModuleInitializer : IModuleInitializer
    {


        public void Configure(IHostEnvironment env)
        {
            throw new NotImplementedException();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<RobotProcessor>();
            serviceCollection.AddTransient<INotificationHandler<UserSelectMessageEvent>, MessageEventHandler>();
        }

        public void Startup(CancellationToken cancellationToken = default)
        {
            LogService.Info("Robot(机器人)插件启动...");
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            LogService.Info("Robot(机器人)插件停止...");
        }
    }
}