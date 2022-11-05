using GameGate.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace GameGate
{
    public class TimedService : BackgroundService
    {
        private readonly ILogger<TimedService> _logger;
        private static MirLog LogQueue => MirLog.Instance;
        private static ClientManager ClientManager => ClientManager.Instance;
        private static SessionManager SessionManager => SessionManager.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;

        private int _processDelayTick = 0;
        private int _processDelayCloseTick = 0;
        private int _processClearSessionTick = 0;
        private int _checkServerConnectTick = 0;
        private int _kepAliveTick = 0;

        private readonly PeriodicTimer _periodicTimer;

        public TimedService(ILogger<TimedService> logger)
        {
            _logger = logger;
            _kepAliveTick = HUtil32.GetTickCount();
            _periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(100));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var startTick = HUtil32.GetTickCount();
            _processDelayTick = startTick;
            _processDelayCloseTick = startTick;
            _processClearSessionTick = startTick;
            _kepAliveTick = startTick;
            _checkServerConnectTick = startTick;
            while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                var currentTick = HUtil32.GetTickCount();
                OutMianMessage();
                ProcessDelayMsg(currentTick);
                ClearSession(currentTick);
                KeepAlive(currentTick);
            }
        }

        private void OutMianMessage()
        {
            if (!GateShare.ShowLog)
                return;
            
            while (!LogQueue.MessageLogQueue.IsEmpty)
            {
                string message;
                if (!LogQueue.MessageLogQueue.TryDequeue(out message)) continue;
                _logger.LogInformation(message);
            }

            while (!LogQueue.DebugLogQueue.IsEmpty)
            {
                string message;
                if (!LogQueue.DebugLogQueue.TryDequeue(out message)) continue;
                _logger.LogDebug(message);
            }
        }

        /// <summary>
        /// GameGate->GameSvr 心跳
        /// </summary>
        private void KeepAlive(int currentTick)
        {
            if (currentTick - _checkServerConnectTick > 10000)
            {
                _checkServerConnectTick = HUtil32.GetTickCount();
                IList<ClientThread> clientList = ClientManager.GetAllClient();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    clientList[i].CheckConnectedState();
                }
            }
        }

        /// <summary>
        /// 处理网关延时消息
        /// </summary>
        private void ProcessDelayMsg(int currentTick)
        {
            if (currentTick - _processDelayTick > 200)
            {
                _processDelayTick = currentTick;
                IList<ClientSession> sessionList = SessionManager.GetAllSession();
                for (var i = 0; i < sessionList.Count; i++)
                {
                    var clientSession = sessionList[i];
                    if (clientSession == null)
                    {
                        continue;
                    }
                    if (clientSession.Session?.Socket == null || !clientSession.Session.Socket.Connected)
                    {
                        continue;
                    }
                    clientSession.ProcessDelayMessage();
                }
            }
            if (currentTick - _processDelayCloseTick > 4000)
            {
                _processDelayCloseTick = HUtil32.GetTickCount();
                IList<ServerService> serverList = ServerManager.GetServerList();
                for (var i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                    serverList[i].ProcessWaitCloseList();
                }
            }
        }

        /// <summary>
        /// 清理过期会话
        /// </summary>
        private void ClearSession(int currentTick)
        {
            if (currentTick - _processClearSessionTick > 120000)
            {
                _processClearSessionTick = HUtil32.GetTickCount();
                IList<ClientThread> clientList = ClientManager.GetAllClient();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    clientList[i].ProcessIdleSession();
                }
                LogQueue.DebugLog("清理超时无效会话...");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}