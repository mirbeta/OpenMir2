using MakePlayer.Cliens;
using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModule;

namespace MakePlayer
{
    public struct RecvicePacket
    {
        public string SessionId;
        public byte[] ReviceBuffer;
    }

    public static class ClientManager
    {
        private static int g_dwProcessTimeMin = 0;
        private static int g_dwProcessTimeMax = 0;
        private static int g_nPosition = 0;
        private static int dwRunTick = 0;
        private static readonly ConcurrentDictionary<string, PlayClient> _clients;
        private static readonly IList<PlayClient> _clientList;
        private static readonly Channel<RecvicePacket> _reviceQueue;
        private static readonly CancellationTokenSource _cancellation;

        static ClientManager()
        {
            _clients = new ConcurrentDictionary<string, PlayClient>();
            _clientList = new List<PlayClient>();
            _reviceQueue = Channel.CreateUnbounded<RecvicePacket>();
            _cancellation = new CancellationTokenSource();
        }

        public static void Start()
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _reviceQueue.Reader.WaitToReadAsync(_cancellation.Token))
                {
                    if (_reviceQueue.Reader.TryRead(out var message))
                    {
                        if (_clients.ContainsKey(message.SessionId))
                        {
                            _clients[message.SessionId].ProcessPacket(message.ReviceBuffer);
                        }
                    }
                }
            }, _cancellation.Token);
        }

        public static void Stop()
        {
            _cancellation.Cancel();
        }

        public static void AddPacket(string socHandle, byte[] reviceBuff)
        {
            var clientPacket = new RecvicePacket
            {
                SessionId = socHandle,
                ReviceBuffer = reviceBuff
            };
            _reviceQueue.Writer.TryWrite(clientPacket);
        }

        public static void AddClient(string sessionId, PlayClient objClient)
        {
            _clients.TryAdd(sessionId, objClient);
            _clientList.Add(objClient);
        }

        public static void DelClient(PlayClient objClient)
        {
            //_Clients.Remove(objClient);
        }

        public static void Run()
        {
            dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            for (var i = g_nPosition; i < _clientList.Count; i++)
            {
                _clientList[i].Run();
                if (((HUtil32.GetTickCount() - dwRunTick) > 50))
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
        }
    }
}