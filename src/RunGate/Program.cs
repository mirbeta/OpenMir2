using System;

namespace RunGate
{
    class Program
    {
        private static ServerApp _serverApp;
        
        static void Main(string[] args)
        {
            _serverApp = new ServerApp();
            _serverApp.StartService();
            Console.WriteLine("Hello World!");

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}