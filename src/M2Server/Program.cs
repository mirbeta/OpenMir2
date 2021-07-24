using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace M2Server
{
    class Program
    {
        private static ServerApp? serverApp = null;

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
            
            serverApp = new ServerApp();
            var cts = new CancellationTokenSource();
            var bgtask = Task.Factory.StartNew(() => { serverApp.StartServer(cts.Token); }, cts.Token);

            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine($"{DateTime.Now} 后台测试服务，准备进行资源清理！");

                cts.Cancel();
                bgtask.Wait(cts.Token);

                Console.WriteLine($"{DateTime.Now} 恭喜，Test服务程序已正常退出！");
            };

            System.Console.ReadLine();

            // while (true)
            // {
            //     var line = Console.ReadLine();
            //     if (string.IsNullOrEmpty(line))
            //     {
            //         return;
            //     }
            //     switch (line)
            //     {
            //         case "info":
            //             Console.WriteLine("");
            //             break;
            //     }
            // }
        }
    }
}

