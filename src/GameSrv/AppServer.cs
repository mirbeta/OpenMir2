using GameSrv.Module;
using MediatR;

namespace GameSrv
{
    public class AppServer
    {
        private readonly ServerHost _serverHost;

        public AppServer()
        {
            _serverHost = new ServerHost();
            _serverHost.ConfigureServices(service =>
            {
                service.AddModules();
                service.AddSingleton<GameApp>();
                service.AddHostedService<AppService>();
                service.AddHostedService<TimedService>();
                service.AddScoped<IMediator, Mediator>();
                foreach (ModuleInfo module in GameShare.Modules)
                {
                    Type moduleInitializerType = module.Assembly.GetTypes().FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                    if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModuleInitializer)))
                    {
                        IModuleInitializer moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                        service.AddSingleton(typeof(IModuleInitializer), moduleInitializer);
                        moduleInitializer.ConfigureServices(service);
                    }
                }
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _serverHost.BuildHost();
            await _serverHost.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serverHost.StopAsync(cancellationToken);
        }

        private static void Stop()
        {
            AnsiConsole.Status().Start("Disconnecting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
            });
        }
    }
}