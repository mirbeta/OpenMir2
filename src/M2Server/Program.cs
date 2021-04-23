using System;
using System.Runtime;
using System.Threading;

namespace M2Server
{
    public class Program
    {
        private static Thread serverThread = null;
        private static ServerApp serverApp = null;

        [STAThread]
        public static void Main(string[] args)
        {
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            serverThread = new Thread(Start);
            serverThread.IsBackground = true;
            serverThread.Start();

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

        static void Start(object obj)
        {
            serverApp = new ServerApp();
            serverApp.StartServer();
        }
    }
}

