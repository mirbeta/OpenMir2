using System.Runtime;

namespace GameSrv
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;//强制压缩大对象堆 LOH
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            AppServer serviceRunner = new AppServer();
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Token.Register(() => _ = serviceRunner.StopAsync(cts.Token));
            // 监听 Ctrl+C 事件
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Ctrl+C pressed");
                cts.Cancel();
                // 阻止其他处理程序处理此事件，以及默认的操作（终止程序）
                e.Cancel = true;
            };
            await serviceRunner.StartAsync(cts.Token);
        }
    }
}