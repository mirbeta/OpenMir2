using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace DBSvr
{
    public class TimedService : BackgroundService
    {
        private LogQueue _logQueue => LogQueue.Instance;
        private readonly UserSocService _userSoc;
        private readonly LoginSvrService _LoginSoc;
        private readonly HumDataService _dataService;
        private int _lastSocketTick;
        private int _lastKeepTick;
        private int _lastClearTick;

        public TimedService(UserSocService userSoc, LoginSvrService loginSoc, HumDataService dataService)
        {
            _userSoc = userSoc;
            _LoginSoc = loginSoc;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                OutMianMessage();
                if (HUtil32.GetTickCount() - _lastKeepTick > 7000)
                {
                    _lastKeepTick = HUtil32.GetTickCount();
                    var userCount = _userSoc.GetUserCount();
                    _LoginSoc.SendKeepAlivePacket(userCount);
                }
                if (HUtil32.GetTickCount() - _lastSocketTick > 5000)
                {
                    _lastSocketTick = HUtil32.GetTickCount();
                    _LoginSoc.CheckConnection();
                }
                if (HUtil32.GetTickCount() - _lastClearTick > 10000)
                {
                    _lastClearTick = HUtil32.GetTickCount();
                    _dataService.ClearTimeoutSession();
                }
                await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
            }
        }

        private void OutMianMessage()
        {
            while (!_logQueue.MessageLogQueue.IsEmpty)
            {
                string message;

                if (!_logQueue.MessageLogQueue.TryDequeue(out message)) continue;

                Console.WriteLine(message);
            }

            while (!_logQueue.DebugLogQueue.IsEmpty)
            {
                string message;

                if (!_logQueue.DebugLogQueue.TryDequeue(out message)) continue;

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}