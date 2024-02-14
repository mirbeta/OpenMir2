using MakePlayer.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace MakePlayer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSetting.json", true, false)
                .Build();

            IHostBuilder builder = new HostBuilder()
                  .ConfigureServices((hostContext, services) =>
                  {
                      services.Configure<MakePlayOptions>(config.GetSection("MakePlay"));
                      services.AddHostedService<AppService>();
                  });

            await builder.RunConsoleAsync();
        }
    }
}