using Microsoft.Extensions.Hosting;
using System.Text;
using System.Threading.Tasks;

namespace RobotSvr
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    
                });

            await builder.RunConsoleAsync();
        }
    }
}