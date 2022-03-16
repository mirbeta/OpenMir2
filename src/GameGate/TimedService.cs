using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace GameGate
{
    public class TimedService : BackgroundService
    {
        private LogQueue _logQueue => LogQueue.Instance;
        private ClientManager _clientManager => ClientManager.Instance;
        private SessionManager _sessionManager => SessionManager.Instance;
        private ServerManager _serverManager => ServerManager.Instance;

        private int _processDelayTick = 0;
        private int _processClearSessionTick = 0;

        public TimedService()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                OutMianMessage();
                ProcessDelayMsg();
                ClearSession();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void OutMianMessage()
        {
            if (GateShare.ShowLog)
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
        }

        /// <summary>
        /// 处理网关延时消息
        /// </summary>
        private void ProcessDelayMsg()
        {
            if (HUtil32.GetTickCount() - _processDelayTick > 500)
            {
                _processDelayTick = HUtil32.GetTickCount();
                var _gateServer = _serverManager.GetServerList();
                for (var i = 0; i < _gateServer.Count; i++)
                {
                    if (_gateServer[i] == null)
                    {
                        continue;
                    }
                    if (_gateServer[i].ClientThread == null)
                    {
                        continue;
                    }
                    if (_gateServer[i].ClientThread.SessionArray == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < _gateServer[i].ClientThread.SessionArray.Length; j++)
                    {
                        var session = _gateServer[i].ClientThread.SessionArray[j];
                        if (session == null)
                        {
                            continue;
                        }
                        if (session.Socket == null)
                        {
                            continue;
                        }
                        var userClient = _sessionManager.GetSession(session.SessionId);
                        if (userClient == null)
                        {
                            continue;
                        }
                        userClient.HandleDelayMsg();
                    }
                }
            }
        }

        /// <summary>
        /// 清理过期会话
        /// </summary>
        private void ClearSession()
        {
            if (HUtil32.GetTickCount() - _processClearSessionTick > 20000)
            {
                _processClearSessionTick = HUtil32.GetTickCount();
                _logQueue.EnqueueDebugging("清理超时会话开始工作...");
                var serverList = _serverManager.GetServerList();
                for (var i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                    if (serverList[i].ClientThread != null)
                    {
                        continue;
                    }
                    ClientThread clientThread = serverList[i].ClientThread;
                    if (clientThread == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < clientThread.SessionArray.Length; j++)
                    {
                        var UserSession = clientThread.SessionArray[j];
                        if (UserSession.Socket != null)
                        {
                            if ((HUtil32.GetTickCount() - UserSession.dwReceiveTick) > GateShare.dwSessionTimeOutTime)//清理超时用户会话 
                            {
                                UserSession.Socket.Close();
                                UserSession.Socket = null;
                                UserSession.SckHandle = -1;
                            }
                        }
                        clientThread.dwCheckServerTimeMin = HUtil32.GetTickCount() - clientThread.dwCheckServerTick;
                        if (clientThread.dwCheckServerTimeMax < clientThread.dwCheckServerTimeMin)
                        {
                            clientThread.dwCheckServerTimeMax = clientThread.dwCheckServerTimeMin;
                        }
                    }
                    _clientManager.CheckSessionStatus(serverList[i].ClientThread);
                }
                _logQueue.EnqueueDebugging("清理超时会话工作完成...");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}