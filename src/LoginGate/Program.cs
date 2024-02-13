using System.Runtime;

namespace LoginGate;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        AppServer serviceRunner = new AppServer();
        await serviceRunner.StartAsync(CancellationToken.None);
    }
}