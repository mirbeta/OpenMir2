using System;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SystemModule;

namespace M2Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            var builder = new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MirApp>();
                    services.AddHostedService<AppService>();
                });

            await builder.RunConsoleAsync();
            
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine($"{DateTime.Now} 后台测试服务，准备进行资源清理！");

                Console.WriteLine($"{DateTime.Now} 恭喜，Test服务程序已正常退出！");
            };
            
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }
                switch (line)
                {
                    case "info":
                        Console.WriteLine("");
                        break;
                }
            }
        }
    }
}