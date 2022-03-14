using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace GameGate
{
    public class TimedService : BackgroundService
    {
        private readonly LogQueue _logQueue;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;
        private int _processDelayTick = 0;
        private int _processClearSessionTick = 0;

        public TimedService(LogQueue logQueue, ClientManager clientManager, SessionManager sessionManager)
        {
            _logQueue = logQueue;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
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
                var _gateClient = _clientManager.GetAllClient();
                for (var i = 0; i < _gateClient.Count; i++)
                {
                    if (_gateClient[i] == null)
                    {
                        continue;
                    }
                    if (_gateClient[i].SessionArray == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < _gateClient[i].SessionArray.Length; j++)
                    {
                        var session = _gateClient[i].SessionArray[j];
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
        /// <param name="obj"></param>
        private void ClearSession()
        {
            if (HUtil32.GetTickCount() - _processClearSessionTick > 20000)
            {
                _processClearSessionTick = HUtil32.GetTickCount();
                _logQueue.EnqueueDebugging("清理超时会话开始工作...");
                TSessionInfo UserSession;
                var clientList = _clientManager.GetAllClient();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < clientList[i].MaxSession; j++)
                    {
                        UserSession = clientList[i].SessionArray[j];
                        if (UserSession.Socket != null)
                        {
                            if ((HUtil32.GetTickCount() - UserSession.dwReceiveTick) > GateShare.dwSessionTimeOutTime)//清理超时用户会话 
                            {
                                UserSession.Socket.Close();
                                UserSession.Socket = null;
                                UserSession.SckHandle = -1;
                            }
                        }
                    }
                    GateShare.dwCheckServerTimeMin = HUtil32.GetTickCount() - GateShare.dwCheckServerTick;
                    if (GateShare.dwCheckServerTimeMax < GateShare.dwCheckServerTimeMin)
                    {
                        GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                    }
                    _clientManager.CheckSessionStatus(clientList[i]);
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