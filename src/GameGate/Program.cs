using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace GameGate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            var builder = new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ServerApp>();
                    services.AddSingleton<ServerService>();
                    services.AddSingleton<SessionManager>();
                    services.AddTransient<ClientManager>();
                    services.AddHostedService<AppService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}