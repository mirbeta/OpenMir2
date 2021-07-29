using System;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace M2Server
{
    class Program
    {
        private static ServerApp? serverApp = null;
        private static CancellationTokenSource _cancellation;

        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            serverApp = new ServerApp();
            _cancellation = new CancellationTokenSource();

            await Task.Run(AppStart);

            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine($"{DateTime.Now} 后台测试服务，准备进行资源清理！");

                _cancellation.Cancel();
                // bgtask.Wait(cts.Token);

                Console.WriteLine($"{DateTime.Now} 恭喜，Test服务程序已正常退出！");
            };
            
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }

                switch (line)
                {
                    case "info":
                        Console.WriteLine("");
                        break;
                }
            }
        }

        static Task AppStart()
        {
            return serverApp.StartServer(_cancellation.Token);
        }
    }
}