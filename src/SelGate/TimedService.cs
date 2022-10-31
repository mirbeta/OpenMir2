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
        private readonly MirLog _logQueue;
        private readonly ClientManager _clientManager;

        public TimedService(MirLog logQueue, ClientManager clientManager)
        {
            _logQueue = logQueue;
            _clientManager = clientManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _clientManager.Process();
                await Task.Delay(TimeSpan.FromMilliseconds(1), stoppingToken);
            }
        }
    }
}