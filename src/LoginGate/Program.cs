using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoginGate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // services.AddSingleton<ServerApp>();
                    // services.AddSingleton<ServerService>();
                    // services.AddTransient<RunGateClient>();
                    // services.AddHostedService<AppService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}