using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameSvr
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            var builder = new HostBuilder()
                .ConfigureLogging(logging => {
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<GameApp>();
                    services.AddHostedService<AppService>();
                });

            System.Console.CancelKeyPress += (sender, e) =>
            {
                cancellationToken.CancelAfter(2000);
            };

            await builder.RunConsoleAsync(cancellationToken.Token);
        }
    }
}