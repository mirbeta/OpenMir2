using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SystemModule.Hosts;

namespace BotSrv
{
    public class AppServer : ServiceHost
    {
        public AppServer()
        {
            Builder.ConfigureLogging(ConfigureLogging);
            Builder.ConfigureServices(ConfigureServices);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RobotOptions>(Configuration.GetSection("BotPlay"));
            services.AddSingleton<ClientManager>();
            services.AddHostedService<AppService>();
        }

        private void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddNLog(Configuration);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await Builder.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Host.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
             
        }
    }
}
