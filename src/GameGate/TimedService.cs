using GameGate.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packet.ClientPackets;

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

        private void CheckConnection()
        {
            
        }

        /// <summary>
        /// GameGate->GameSvr 心跳
        /// </summary>
        private void KeepAlive(int currentTick)
        {
            if (currentTick - _kepAliveTick > 10 * 10000)
            {
                _kepAliveTick = HUtil32.GetTickCount();
                IList<ServerService> serverList = ServerManager.GetServerList();
                for (var i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                    if (serverList[i].ClientThread == null)
                    {
                        continue;
                    }
                    if (!serverList[i].ClientThread.IsConnected)
                    {
                        continue;
                    }
                    var cmdPacket = new PacketHeader();
                    cmdPacket.PacketCode = Grobal2.RUNGATECODE;
                    cmdPacket.Socket = 0;
                    cmdPacket.Ident = Grobal2.GM_CHECKCLIENT;
                    cmdPacket.PackLength = 0;
                    serverList[i].ClientThread.SendBuffer(cmdPacket.GetBuffer());
                }
            }
        }

        /// <summary>
        /// 处理网关延时消息
        /// </summary>
        private void ProcessDelayMsg(int currentTick)
        {
            if (currentTick - _processDelayTick > 1000)
            {
                _processDelayTick = currentTick;
                IList<ServerService> serverList = ServerManager.GetServerList();
                for (var i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i] == null)
                    {
                        continue;
                    }
                    if (HUtil32.GetTickCount() - _processDelayCloseTick > 2000) //加入网关延时发送关闭消息
                    {
                        _processDelayCloseTick = HUtil32.GetTickCount();
                        serverList[i].ProcessCloseList();
                    }
                    if (serverList[i].ClientThread == null)
                    {
                        continue;
                    }
                    if (serverList[i].ClientThread.SessionArray == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < serverList[i].ClientThread.SessionArray.Length; j++)
                    {
                        var session = serverList[i].ClientThread.SessionArray[j];
                        if (session?.Socket == null)
                        {
                            continue;
                        }
                        var userClient = SessionManager.GetSession(session.SessionId);
                        userClient?.ProcessDelayMessage();
                    }
                }
            }
        }

        /// <summary>
        /// 清理过期会话
        /// </summary>
        private void ClearSession(int currentTick)
        {
            var clientList = ClientManager.GetAllClient();
            if (currentTick - _checkServerConnectTick > 5000)
            {
                _checkServerConnectTick = HUtil32.GetTickCount();
                LogQueue.EnqueueDebugging("检查链接状态...");
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    clientList[i].CheckSessionStatus();
                }
            }
            if (currentTick - _processClearSessionTick > 120000)
            {
                _processClearSessionTick = HUtil32.GetTickCount();
                LogQueue.EnqueueDebugging("清理超时会话开始...");
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    clientList[i].CheckTimeOutSession();
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}