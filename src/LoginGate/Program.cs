using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ServerApp>();
                    services.AddSingleton<GateServer>();
                    services.AddSingleton<GateClient>();
                    services.AddHostedService<AppService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}