using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

namespace BotSvr
{
    public class ClientManager
    {
        private int g_dwProcessTimeMin = 0;
        private int g_dwProcessTimeMax = 0;
        private int g_nPosition = 0;
        private int dwRunTick = 0;
        private int AutoRunTick = 0;
        private readonly ConcurrentDictionary<string, RobotClient> _clients;
        private readonly IList<RobotClient> _clientList;
        private readonly IList<AutoPlayRunTime> _autoList;
        private readonly Channel<RecvicePacket> _reviceQueue;


        public ClientManager()
        {
            _clientList = new List<RobotClient>();
            _autoList = new List<AutoPlayRunTime>();
            _clients = new ConcurrentDictionary<string, RobotClient>();
            _reviceQueue = Channel.CreateUnbounded<RecvicePacket>();
        }

        public void Start(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                await ProcessReviceMessage(stoppingToken);
            }, stoppingToken);
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
                        if (_clients.ContainsKey(message.SessionId))
                        {
                            _clients[message.SessionId].ProcessPacket(message.ReviceData);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
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

        public void AddClient(string sessionId, RobotClient objClient)
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
            _clients.TryRemove(sessionId, out RobotClient robotClient);
            _clientList.Add(robotClient);
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