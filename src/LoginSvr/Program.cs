﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddFilter("System", LogLevel.None).AddFilter("Microsoft", LogLevel.None);
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "Logsrv.conf")));
                    services.AddSingleton<AppServer>();
                    services.AddSingleton<LoginService>();
                    services.AddSingleton<AccountDB>();
                    services.AddSingleton<MasSocService>();
                    services.AddSingleton<MonSocService>();
                    services.AddHostedService<AppService>();
                });

            await builder.RunConsoleAsync();

        }
    }
}