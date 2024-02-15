using GameGate.Conf;
using GameGate.Services;
using System.Runtime;

namespace GameGate
{
    internal class Program
    {
        private static PeriodicTimer _timer;
        private static readonly CancellationTokenSource CancellationToken = new CancellationTokenSource();

        private static async Task Main(string[] args)
        {
            GCSettings.LatencyMode = GCLatencyMode.Batch;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            ThreadPool.SetMaxThreads(200, 200);
            ThreadPool.GetMinThreads(out int workThreads, out int completionPortThreads);
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
            LogService.Info($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount} Minimum work threads: {workThreads} Minimum completion port threads: {completionPortThreads}");
            await serviceRunner.StartAsync(cts.Token);
        }

        private static Task Exit()
        {
            CancellationToken.CancelAfter(3000);
            Environment.Exit(Environment.ExitCode);
            return Task.CompletedTask;
        }

        private static Task ClearConsole()
        {
            Console.Clear();
            AnsiConsole.Clear();
            return Task.CompletedTask;
        }

        private static Task ReLoadConfig()
        {
            ConfigManager.Instance.ReLoadConfig();
            ServerManager.Instance.StartClientMessageWork(CancellationToken.Token);
            Console.WriteLine("重新读取配置文件完成...");
            return Task.CompletedTask;
        }
    }
}