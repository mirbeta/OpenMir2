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
        private static Thread serverThread = null;
        private static CancellationTokenSource _cancellation;

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            serverApp = new ServerApp();
            _cancellation = new CancellationTokenSource();
            
            serverThread = new Thread(AppStart);
            serverThread.IsBackground = true;
            serverThread.Start();

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
                    case "robot":
                        //制造一个人工智障
                        line = Console.ReadLine();
                        if (int.TryParse(line, out var count))
                        {
                            if (count == 0)
                            {
                                count = 1;
                            }
                            for (int i = 0; i < count; i++)
                            {
                                M2Share.UserEngine.AddAILogon(new TAILogon()
                                {
                                    sCharName = "玩家" + SystemModule.RandomNumber.GetInstance().Random() + "号",
                                    sMapName = "0",
                                    sConfigFileName = "",
                                    sHeroConfigFileName = "",
                                    sFilePath = M2Share.g_Config.sEnvirDir,
                                    sConfigListFileName = M2Share.g_Config.sAIConfigListFileName,
                                    sHeroConfigListFileName = M2Share.g_Config.sHeroAIConfigListFileName,
                                    nX = 285,
                                    nY = 608
                                });
                            }
                        }
                        break;
                }
            }
        }

        static void AppStart()
        {
            serverApp.StartServer(_cancellation.Token);
        }
    }
}