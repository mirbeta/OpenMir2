using Microsoft.Extensions.Hosting;
using System.Runtime;
using System.Text;

namespace GameSrv
{
    internal class Program {
        private static async Task Main(string[] args) {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;//强制压缩大对象堆 LOH
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
            AppServer serviceRunner = new AppServer();
            await serviceRunner.RunAsync();
        }
    }
}