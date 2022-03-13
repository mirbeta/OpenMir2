using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SelGate.Services;

namespace SelGate
{
    public class TimedService : BackgroundService
    {
        private readonly LogQueue _logQueue;
        private readonly ClientManager _clientManager;

        public TimedService(LogQueue logQueue, ClientManager clientManager)
        {
            _logQueue = logQueue;
            _clientManager = clientManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                OutMianMessage();
                _clientManager.CheckSession();
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