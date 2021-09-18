using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<AppServer>();
                    services.AddSingleton<LoginService>();
                    services.AddHostedService<AppService>();
                    services.AddSingleton<AccountDB>();
                    services.AddSingleton<MasSocService>();
                    services.AddSingleton<MonSocService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}