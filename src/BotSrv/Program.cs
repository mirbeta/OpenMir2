using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BotSrv
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
            AppServer serviceRunner = new AppServer();
            await serviceRunner.RunAsync();
        }
    }
    
    //internal class Program
    //{
    //    private static async Task Main(string[] args)
    //    {

    //        var config = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("AppSetting.json", true, false)
    //            .Build();

    //        var builder = new HostBuilder()
    //            .ConfigureServices((hostContext, services) =>
    //            {
    //                services.Configure<RobotOptions>(config.GetSection("BotPlay"));
    //                services.AddSingleton<ClientManager>();
    //                services.AddHostedService<AppService>();
    //            }).ConfigureLogging((context, logging) =>
    //            {
    //                logging.ClearProviders();
    //                logging.SetMinimumLevel(LogLevel.Trace);
    //                logging.AddNLog(Configuration);
    //            });

    //        await builder.RunConsoleAsync();
    //    }
    //}
}