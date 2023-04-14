using System.Runtime;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Threading.Tasks;

namespace SelGate
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            var serviceRunner = new AppServer();
            await serviceRunner.RunAsync();
        }
    }
}