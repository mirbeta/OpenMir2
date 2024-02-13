using BotSrv.Player;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using OpenMir2;
using SystemModule;

namespace BotSrv
{
    public class ClientManager
    {
        
        private int g_dwProcessTimeMin = 0;
        private int g_dwProcessTimeMax = 0;
        private int g_nPosition = 0;
        private int dwRunTick = 0;
        private int AutoRunTick = 0;
        private readonly ConcurrentDictionary<string, RobotPlayer> _clients;
        private readonly IList<RobotPlayer> _clientList;
        private readonly IList<AutoPlayRunTime> _autoList;
        private readonly Channel<RecvicePacket> _reviceQueue;

        public ClientManager()
        {
            _clientList = new List<RobotPlayer>();
            _autoList = new List<AutoPlayRunTime>();
            _clients = new ConcurrentDictionary<string, RobotPlayer>();
            _reviceQueue = Channel.CreateUnbounded<RecvicePacket>();
        }

        public Task Start(CancellationToken stoppingToken)
        {
            LogService.Info("消息处理线程启动...");
            return ProcessReviceMessage(stoppingToken);
        }

        public void Stop(CancellationToken stoppingToken)
        {

        }

        private async Task ProcessReviceMessage(CancellationToken stoppingToken)
        {
            while (await _reviceQueue.Reader.WaitToReadAsync(stoppingToken))
            {
                if (_reviceQueue.Reader.TryRead(out var message))
                {
                    try
                    {
                        if (_clients.TryGetValue(message.SessionId, out var client))
                        {
                            client.ProcessPacket(message.ReviceData);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogService.Error(ex);
                    }
                }
            }
        }

        public void AddPacket(string sessionId, string reviceData)
        {
            var clientPacket = new RecvicePacket();
            clientPacket.SessionId = sessionId;
            clientPacket.ReviceData = reviceData;
            _reviceQueue.Writer.TryWrite(clientPacket);
        }

        public void AddClient(string sessionId, RobotPlayer objClient)
        {
            _autoList.Add(new AutoPlayRunTime()
            {
                SessionId = sessionId,
                RunTick = HUtil32.GetTickCount()
            });
            _clients.TryAdd(sessionId, objClient);
            _clientList.Add(objClient);
        }

        public void DelClient(string sessionId)
        {
            AutoPlayRunTime findSession = null;
            foreach (var item in _autoList)
            {
                if (item.SessionId == sessionId)
                {
                    findSession = item;
                    break;
                }
            }
            if (findSession != null)
            {
                _autoList.Remove(findSession);
            }
            _clients.TryRemove(sessionId, out var robotClient);
            _clientList.Remove(robotClient);
            if (robotClient != null)
            {
                LogService.Info("机器人[{0}] 会话ID:{1}]掉线或断开链接.", robotClient.ChrName, sessionId);
            }
        }

        public void Run()
        {
            dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            for (var i = g_nPosition; i < _clientList.Count; i++)
            {
                _clientList[i].Run();
                if (((HUtil32.GetTickCount() - dwRunTick) > 20))
                {
                    g_nPosition = i;
                    boProcessLimit = true;
                    break;
                }
            }
            if (!boProcessLimit)
            {
                g_nPosition = 0;
            }
            g_dwProcessTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (g_dwProcessTimeMin > g_dwProcessTimeMax)
            {
                g_dwProcessTimeMax = g_dwProcessTimeMin;
            }
            RunAutoPlay();
        }

        private void RunAutoPlay()
        {
            AutoRunTick = HUtil32.GetTickCount();
            if (_autoList.Count > 0)
            {
                for (var i = 0; i < _autoList.Count; i++)
                {
                    if ((AutoRunTick - _autoList[i].RunTick) > 800)
                    {
                        _autoList[i].RunTick = HUtil32.GetTickCount();
                        _clientList[i].RunAutoPlay();
                    }
                }
            }
        }
    }

    public struct RecvicePacket
    {
        public string SessionId;
        public string ReviceData;
    }

    public class AutoPlayRunTime
    {
        public string SessionId;
        public int RunTick;
    }
}