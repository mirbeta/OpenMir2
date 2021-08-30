using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace M2Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            var builder = new HostBuilder()
                .ConfigureLogging(logging => { logging.ClearProviders(); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MirApp>();
                    services.AddHostedService<AppService>();
                });

            cancellationToken.Token.Register(() =>
            {
                Console.WriteLine("停止服务.");
            });

            await builder.RunConsoleAsync(cancellationToken.Token);
        }
    }
}