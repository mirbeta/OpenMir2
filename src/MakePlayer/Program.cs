using MakePlayer.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Text;
using SystemModule;

namespace MakePlayer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
          
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSetting.json", true, false)
                .Build();
            
            var builder = new HostBuilder()
                  .ConfigureServices((hostContext, services) =>
                  {
                      services.Configure<MakePlayOptions>(config.GetSection("MakePlay"));
                      services.AddSingleton<ClientManager>();
                      services.AddHostedService<AppService>();
                  });

            await builder.RunConsoleAsync();
        }
    }
}