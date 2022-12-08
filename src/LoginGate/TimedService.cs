using LoginGate.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SystemModule;
using SystemModule.Logger;

namespace LoginGate
{
    public class TimedService : BackgroundService
    {
        private int _processDelayTick = 0;
        private int _heartInterval = 0;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SessionManager _sessionManager;
        private readonly ClientManager _clientManager;

        public TimedService(ClientManager clientManager, SessionManager sessionManager)
        {
            _clientManager = clientManager;
            _sessionManager = sessionManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processDelayTick = HUtil32.GetTickCount();
            _heartInterval = HUtil32.GetTickCount();
            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessDelayMsg();
                ProcessHeartbeat();
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private void ProcessHeartbeat()
        {
            if (HUtil32.GetTickCount() - _heartInterval > 10000)
            {
                _heartInterval = HUtil32.GetTickCount();
                var clientList = _clientManager.ServerGateList();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    _clientManager.ProcessClientHeart(clientList[i]);
                }
            }
        }

        private void ProcessDelayMsg()
        {
            if (HUtil32.GetTickCount() - _processDelayTick > 1000)
            {
                _processDelayTick = HUtil32.GetTickCount();
                var clientList = _clientManager.ServerGateList();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    if (clientList[i].SessionArray == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < clientList[i].SessionArray.Length; j++)
                    {
                        var session = clientList[i].SessionArray[j];
                        if (session?.Socket == null)
                        {
                            continue;
                        }
                        var userSession = _sessionManager.GetSession(session.ConnectionId);
                        if (userSession == null)
                        {
                            continue;
                        }
                        var success = false;
                        userSession.HandleDelayMsg(ref success);
                        if (success)
                        {
                            _sessionManager.CloseSession(session.ConnectionId);
                            clientList[i].SessionArray[j].Socket = null;
                            clientList[i].SessionArray[j] = null;
                        }
                    }
                }
            }
        }
    }
}