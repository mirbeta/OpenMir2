using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenMir2.Hosts
{
    public abstract class ServiceHost
    {
        protected readonly IConfigurationRoot Configuration;

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
        }

        public abstract void Initialize();

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}