using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SystemModule;

namespace RobotSystem
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
            serviceCollection.AddHostedService<RobotProcessor>();
        }

        public void Startup(CancellationToken cancellationToken = default)
        {
            logger.Info("Robot机器人插件启动...");
        }

        public void Stopping(CancellationToken cancellationToken = default)
        {
            logger.Info("Robot机器人插件停止...");
        }
    }
}