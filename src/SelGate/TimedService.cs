using Microsoft.Extensions.Hosting;
using SelGate.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Logger;

namespace SelGate
{
    public class TimedService : BackgroundService
    {
        private readonly MirLogger _logQueue;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;
        private int _processClearSessionTick = 0;
        private int _lastChekSocketTick = 0;
        private int _processDelayTick = 0;
        
        public TimedService(MirLogger logQueue, ClientManager clientManager, SessionManager sessionManager)
        {
            _logQueue = logQueue;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processClearSessionTick = HUtil32.GetTickCount();
            _lastChekSocketTick = HUtil32.GetTickCount();
            _processDelayTick = HUtil32.GetTickCount();
            while (!stoppingToken.IsCancellationRequested)
            {
                CleanOutSession();
                ProcessDelayMsg();
                CheckSocketState();
                await Task.Delay(TimeSpan.FromMilliseconds(1), stoppingToken);
            }
        }

        private void ProcessDelayMsg()
        {
            if (HUtil32.GetTickCount() - _processDelayTick > 20 * 1000)
            {
                _processDelayTick = HUtil32.GetTickCount();
                var clientList = _clientManager.GetClients;
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
                        if (session == null)
                        {
                            continue;
                        }
                        if (session.Socket == null)
                        {
                            continue;
                        }
                        var userClient = _sessionManager.GetSession(session.SocketId);
                        if (userClient == null)
                        {
                            continue;
                        }
                        var success = false;
                        userClient.HandleDelayMsg(ref success);
                        if (success)
                        {
                            _sessionManager.CloseSession(session.SocketId);
                            clientList[i].SessionArray[j].Socket = null;
                            clientList[i].SessionArray[j] = null;
                        }
                    }
                }
            }
        }

        private void CleanOutSession()
        {
            if (HUtil32.GetTickCount() - _processClearSessionTick > 20 * 1000)
            {
                _processClearSessionTick = HUtil32.GetTickCount();
                var clientList = _clientManager.GetClients;
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < ClientThread.MaxSession; j++)
                    {
                        var userSession = clientList[i].SessionArray[j];
                        if (userSession?.Socket != null && userSession.Socket.Connected)
                        {
                            if ((HUtil32.GetTickCount() - userSession.dwReceiveTick) > GateShare.SessionTimeOutTime) //清理超时用户会话 
                            {
                                userSession.Socket.Close();
                                userSession.Socket = null;
                                _sessionManager.CloseSession(userSession.SocketId);
                                userSession = null;
                                _logQueue.DebugLog("清理超时会话,关闭超时Socket.");
                            }
                        }
                    }
                }
                _logQueue.DebugLog("Cleanup timeout session...");
            }
        }

        private void CheckSocketState()
        {
            if (HUtil32.GetTickCount() - _lastChekSocketTick > 10000)
            {
                _lastChekSocketTick = HUtil32.GetTickCount();
                var clientList = _clientManager.GetClients;
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    CheckSessionStatus(clientList[i]);
                }
            }
        }
        
        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        /// <param name="clientThread"></param>
        private void CheckSessionStatus(ClientThread clientThread)
        {
            if (clientThread.boGateReady)
            {
                clientThread.SendKeepAlive();
                clientThread.CheckServerFailCount = 0;
                return;
            }
            if ((HUtil32.GetTickCount() - GateShare.CheckServerTick) > GateShare.CheckServerTimeOutTime)
            {
                if (clientThread.CheckServerFail)
                {
                    clientThread.ReConnected();
                    clientThread.CheckServerFailCount++;
                    _logQueue.DebugLog($"服务器[{clientThread.GetEndPoint()}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]");
                    return;
                }
                clientThread.CheckServerFail = true;
                clientThread.Stop();
                clientThread.CheckServerFailCount++;
                _logQueue.DebugLog($"服务器[{clientThread.GetEndPoint()}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]");
            }
        }
    }
}