using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SystemModule
{
    /// <summary>
    /// Interface for module initializers.
    /// </summary>
    public interface IModuleInitializer
    {
        void ConfigureServices(IServiceCollection serviceCollection);

        void Configure(IHostEnvironment env);
    }
}