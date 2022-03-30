using Microsoft.Extensions.Hosting;
using SelGate.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

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
                //KeepAlive();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void KeepAlive()
        {
            var clientList = _clientManager.GetALlClient();
            foreach (var client in clientList)
            {
                if (client.IsConnected)
                {
                    client.SendData("%--$");
                }
                if (client.IsConnected && client.KeepAlive)
                {
                    if (HUtil32.GetTickCount() - client.KeepAliveTick > 25 * 1000)
                    {
                        client.KeepAliveTick = HUtil32.GetTickCount();
                        client.Stop();
                        client.SockThreadStutas = SockThreadStutas.TimeOut;
                    }
                }
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