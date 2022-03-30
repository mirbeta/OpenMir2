using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DBSvr
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "dbsvr.conf")));
                    services.AddSingleton<UserSocService>();
                    services.AddSingleton<LoginSvrService>();
                    services.AddSingleton<HumDataService>();
                    services.AddSingleton<IPlayRecordService, MySqlPlayRecordService>();
                    services.AddSingleton<IPlayDataService, MySqlPlayDataService>();
                    services.AddHostedService<TimedService>();
                    services.AddHostedService<AppService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}