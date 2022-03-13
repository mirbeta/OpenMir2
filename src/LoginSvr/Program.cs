using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LoginSvr
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "Logsrv.conf")));
                    services.AddSingleton<LogQueue>();
                    services.AddSingleton<AppServer>();
                    services.AddSingleton<LoginService>();
                    services.AddSingleton<AccountDB>();
                    services.AddSingleton<MasSocService>();
                    services.AddSingleton<MonSocService>();
                    services.AddSingleton<ThreadParseList>();
                    services.AddHostedService<TimedService>();
                    services.AddHostedService<AppService>();
                });

            await builder.RunConsoleAsync();

        }
    }
}