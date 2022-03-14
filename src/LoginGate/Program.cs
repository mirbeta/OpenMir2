using LoginGate.Conf;
using LoginGate.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LoginGate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "config.conf")));
                    services.AddSingleton<LogQueue>();
                    services.AddSingleton<ServerApp>();
                    services.AddSingleton<ServerService>();
                    services.AddSingleton<SessionManager>();
                    services.AddSingleton<ClientManager>();
                    services.AddHostedService<AppService>();
                    services.AddHostedService<TimedService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}