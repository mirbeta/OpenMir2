using GameGate.Services;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace GameGate
{
    public class TimedService : BackgroundService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static ClientManager ClientManager => ClientManager.Instance;
        private static SessionContainer SessionContainer => SessionContainer.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;
        private int ProcessDelayTick { get; set; }
        private int ProcessDelayCloseTick { get; set; }
        private int ProcessClearSessionTick { get; set; }
        private int CheckServerConnectTick { get; set; }
        private int KepAliveTick { get; set; }
        private int ShowMonitorTick { get; set; }

        private readonly PeriodicTimer _periodicTimer;

        public TimedService()
        {
            KepAliveTick = HUtil32.GetTickCount();
            _periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(10));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var startTick = HUtil32.GetTickCount();
            ProcessDelayTick = startTick;
            ProcessDelayCloseTick = startTick;
            ProcessClearSessionTick = startTick;
            KepAliveTick = startTick;
            CheckServerConnectTick = startTick;
            ShowMonitorTick = startTick;
            while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                var currentTick = HUtil32.GetTickCount();
                ProcessDelayMsg(currentTick);
                ClearIdleSession(currentTick);
                KeepAlive(currentTick);
                ProcessDelayClose(currentTick);
                ShowNetworkMonitor(currentTick);
            }
        }

        private void ShowNetworkMonitor(int currentTick)
        {
            if (currentTick - ShowMonitorTick > 10000)
            {
                ShowMonitorTick = HUtil32.GetTickCount();
                var serverList = ServerManager.GetServerList();
                if (serverList == null)
                {
                    return;
                }
                for (var i = 0; i < serverList.Length; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                   _logger.Info( $"{serverList[i].GateInfo.ServiceId} {serverList[i].NetworkMonitor.UpdateStatsAsync(500)}");
                }
            }
        }

        /// <summary>
        /// GameGate->GameSrv 心跳
        /// </summary>
        private void KeepAlive(int currentTick)
        {
            if (currentTick - CheckServerConnectTick > 10000)
            {
                CheckServerConnectTick = HUtil32.GetTickCount();
                var clientList = ClientManager.GetClients();
                if (clientList == null)
                {
                    return;
                }
                for (var i = 0; i < clientList.Length; i++)
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
        /// 处理会话延时消息
        /// </summary>
        private void ProcessDelayMsg(int currentTick)
        {
            if (currentTick - ProcessDelayTick > 200)
            {
                ProcessDelayTick = currentTick;
                var sessionList = SessionContainer.GetSessions();
                if (sessionList == null)
                {
                    return;
                }
                for (var i = 0; i < sessionList.Length; i++)
                {
                    var serverSession = sessionList[i];
                    if (serverSession == null)
                    {
                        continue;
                    }
                    for (int j = 0; j < serverSession.Length; j++)
                    {
                        var clientSession = serverSession[j];
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
            }
        }

        private void ProcessDelayClose(int currentTick)
        {
            if (currentTick - ProcessDelayCloseTick > 4000)
            {
                ProcessDelayCloseTick = HUtil32.GetTickCount();
                var serverList = ServerManager.GetServerList();
                if (serverList == null)
                {
                    return;
                }
                for (var i = 0; i < serverList.Length; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                    serverList[i].ProcessCloseSessionQueue();
                }
            }
        }

        /// <summary>
        /// 清理过期会话
        /// </summary>
        private void ClearIdleSession(int currentTick)
        {
            if (currentTick - ProcessClearSessionTick > 120000)
            {
                ProcessClearSessionTick = HUtil32.GetTickCount();
                var clientList = ClientManager.GetClients();
                if (clientList == null)
                {
                    return;
                }
                for (var i = 0; i < clientList.Length; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    clientList[i].ProcessIdleSession();
                }
                _logger.Debug("清理空闲或无效超时会话");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}