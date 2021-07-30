using System;
using System.Text;
using System.Threading;

namespace RunGate
{
    class Program
    {
        private static ServerApp _serverApp;
        private static Thread _appThread;
        
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _serverApp = new ServerApp();
            _appThread = new Thread(Run);
            _appThread.IsBackground = true;
            _appThread.Start();
            
            Console.WriteLine("Hello World!");

            while (true)
            {
                var line = Console.ReadLine();
                
                Console.WriteLine(line);
            }
        }

        static void Run(object obj)
        {
            _serverApp.StartService();
        }
    }
}