using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SystemModule.Hosts
{
    public abstract class ServiceHost : IHost
    {
        protected readonly ILogger Logger;
        protected readonly IConfigurationRoot Configuration;
        protected IHost _host;
        protected readonly IHostBuilder Builder;

        protected ServiceHost()
        {
            Configuration = new ConfigurationBuilder().Build();
            LogManager.Setup()
                .SetupExtensions(ext => ext.RegisterConfigSettings(Configuration))
                .GetCurrentClassLogger();
            Logger = LogManager.GetCurrentClassLogger();
            Builder = new HostBuilder();
        }

        public IServiceProvider Services => _host?.Services;

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        public abstract void Dispose();
    }
}