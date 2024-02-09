using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SystemModule.Hosts
{
    public abstract class ServiceHost 
    {
        private readonly ILogger Logger;
        protected readonly IConfigurationRoot Configuration;
        protected IHost Host;
        protected readonly IHostBuilder Builder;

        protected ServiceHost()
        {
            if (File.Exists("AppSetting.json"))
            {
                Configuration = new ConfigurationBuilder().AddJsonFile("AppSetting.json", true, true).Build();
            }
            else
            {
                Configuration = new ConfigurationBuilder().Build();
            }
            LogManager.Setup()
                .SetupExtensions(ext => ext.RegisterConfigSettings(Configuration))
                .GetCurrentClassLogger();
            Logger = LogManager.GetCurrentClassLogger();
            Builder = new HostBuilder();
            Builder.UseConsoleLifetime();
            Builder.ConfigureHostOptions(options =>
            {
                options.ShutdownTimeout = TimeSpan.FromSeconds(30);
            });
            Initialize();
        }

        public IServiceProvider Services => Host.Services;

        public abstract void Initialize();

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}