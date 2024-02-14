using MakePlayer.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMir2;
using Serilog;
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

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            LogService.Logger = Log.Logger;

            await builder.RunConsoleAsync();
        }
    }
}