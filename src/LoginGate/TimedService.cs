using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate
{
    public class TimedService : BackgroundService
    {
        private LogQueue _logQueue=>LogQueue.Instance;
        private ServerManager ServerManager => ServerManager.Instance;
        private SessionManager SessionManager => SessionManager.Instance;
        private int _processDelayTick = 0;

        public TimedService()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                OutMianMessage();
                ProcessDelayMsg();
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

        public void ProcessDelayMsg()
        {
            if (HUtil32.GetTickCount() - _processDelayTick > 200)
            {
                _processDelayTick = HUtil32.GetTickCount();
                var _clientList = ServerManager.GetServerList();
                for (var i = 0; i < _clientList.Count; i++)
                {
                    if (_clientList[i] == null)
                    {
                        continue;
                    }
                    if (_clientList[i].ClientThread == null)
                    {
                        continue;
                    }
                    if (_clientList[i].ClientThread.SessionArray == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < _clientList[i].ClientThread.SessionArray.Length; j++)
                    {
                        var session = _clientList[i].ClientThread.SessionArray[j];
                        if (session == null)
                        {
                            continue;
                        }
                        if (session.Socket == null)
                        {
                            continue;
                        }
                        var userSession = SessionManager.GetSession(session.SocketId);
                        if (userSession == null)
                        {
                            continue;
                        }
                        var success = false;
                        userSession.HandleDelayMsg(ref success);
                        if (success)
                        {
                            SessionManager.CloseSession(session.SocketId);
                            _clientList[i].ClientThread.SessionArray[j].Socket = null;
                        }
                    }
                }
            }
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}