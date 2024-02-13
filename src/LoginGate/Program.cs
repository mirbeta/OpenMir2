using Microsoft.Extensions.Hosting;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoginGate;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        var serviceRunner = new AppServer();
        await serviceRunner.StartAsync(CancellationToken.None);
    }
}