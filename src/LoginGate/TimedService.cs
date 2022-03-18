using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LoginGate
{
    public class TimedService : BackgroundService
    {
        private LogQueue _logQueue=>LogQueue.Instance;
        private ClientManager _clientManager=>ClientManager.Instance;

        public TimedService()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                OutMianMessage();
                _clientManager.ProcessDelayMsg();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void OutMianMessage()
        {
            while (!_logQueue.MessageLog.IsEmpty)
            {
                string message;

                if (!_logQueue.MessageLog.TryDequeue(out message)) continue;

                Console.WriteLine(message);
            }

            while (!_logQueue.DebugLog.IsEmpty)
            {
                string message;

                if (!_logQueue.DebugLog.TryDequeue(out message)) continue;

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
        

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}