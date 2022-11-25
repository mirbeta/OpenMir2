using CloudGate.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packets;

namespace CloudGate
{
    public class TimedService : BackgroundService
    {
        private readonly ILogger<TimedService> _logger;
        private static MirLog LogQueue => MirLog.Instance;
        private static SessionManager SessionManager => SessionManager.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;

        private int _processDelayTick = 0;
        private int _processClearSessionTick = 0;
        private int _kepAliveTick = 0;

        public TimedService(ILogger<TimedService> logger)
        {
            _logger = logger;
            _kepAliveTick = HUtil32.GetTickCount();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                OutMianMessage();
                ProcessDelayMsg();
                ClearSession();
                KeepAlive();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void OutMianMessage()
        {
            if (!GateShare.ShowLog)
                return;

            while (!LogQueue.MessageLog.IsEmpty)
            {
                string message;
                if (!LogQueue.MessageLog.TryDequeue(out message)) continue;
                _logger.LogInformation(message);
            }

            while (!LogQueue.DebugLog.IsEmpty)
            {
                string message;
                if (!LogQueue.DebugLog.TryDequeue(out message)) continue;
                _logger.LogDebug(message);
            }
        }

        /// <summary>
        /// GameGate->GameSvr 心跳
        /// </summary>
        private void KeepAlive()
        {
            if (HUtil32.GetTickCount() - _kepAliveTick > 10 * 10000)
            {
                _kepAliveTick = HUtil32.GetTickCount();
                IList<ServerService> serverList = ServerManager.GetServerList();
                for (int i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                    var cmdPacket = new GameServerPacket();
                    cmdPacket.PacketCode = Grobal2.RUNGATECODE;
                    cmdPacket.Socket = 0;
                    cmdPacket.Ident = Grobal2.GM_CHECKCLIENT;
                    cmdPacket.PackLength = 0;
                }
            }
        }

        /// <summary>
        /// 处理网关延时消息
        /// </summary>
        private void ProcessDelayMsg()
        {
            if (HUtil32.GetTickCount() - _processDelayTick > 100)
            {
                _processDelayTick = HUtil32.GetTickCount();
                IList<ServerService> serverList = ServerManager.GetServerList();
                for (var i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
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
                LogQueue.EnqueueDebugging("清理超时会话开始工作...");
                var serverList = ServerManager.GetServerList();
                for (var i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                }
                LogQueue.EnqueueDebugging("清理超时会话工作完成...");
            }
        }
    }
}