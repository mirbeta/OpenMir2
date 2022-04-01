using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModule;

namespace MakePlayer
{
    public class RecvicePacket
    {
        public string SessionId;
        public byte[] ReviceBuffer;
    }

    public static class ClientManager
    {
        private static ConcurrentDictionary<string, PlayClient> _Clients;
        private static int g_dwProcessTimeMin = 0;
        private static int g_dwProcessTimeMax = 0;
        private static int g_nPosition = 0;
        private static int dwRunTick = 0;
        private static Channel<RecvicePacket> _reviceMsgList;

        static ClientManager()
        {
            _Clients = new ConcurrentDictionary<string, PlayClient>();
            _reviceMsgList = Channel.CreateUnbounded<RecvicePacket>();
        }

        public static async void Start()
        {
            var gTasks = new Task[1];
            var consumerTask1 = Task.Factory.StartNew(ProcessReviceMessage);
            gTasks[0] = consumerTask1;
            await Task.WhenAll(gTasks);
        }

        private static async Task ProcessReviceMessage()
        {
            while (await _reviceMsgList.Reader.WaitToReadAsync())
            {
                if (_reviceMsgList.Reader.TryRead(out var message))
                {
                    if (_Clients.ContainsKey(message.SessionId))
                    {
                        _Clients[message.SessionId].ProcessPacket(message.ReviceBuffer);
                    }
                }
            }
        }

        public static void AddPacket(string socHandle, byte[] reviceBuff)
        {
            var clientPacket = new RecvicePacket();
            clientPacket.SessionId = socHandle;
            clientPacket.ReviceBuffer = reviceBuff;
            _reviceMsgList.Writer.TryWrite(clientPacket);
        }

        public static void AddClient(string sessionId, PlayClient objClient)
        {
            _Clients.TryAdd(sessionId, objClient);
        }

        public static void DelClient(PlayClient objClient)
        {
            //_Clients.Remove(objClient);
        }

        public static void Run()
        {
            dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            var clientList = _Clients.Values.ToList();
            for (var i = g_nPosition; i < _Clients.Count; i++)
            {
                clientList[i].Run();
                if (((HUtil32.GetTickCount() - dwRunTick) > 20))
                {
                    g_nPosition = i;
                    boProcessLimit = true;
                    break;
                }
                if (clientList[i].m_boLogin && (HUtil32.GetTickCount() - clientList[i].m_dwSayTick > 3000))
                {
                    clientList[i].m_dwSayTick = HUtil32.GetTickCount();
                    clientList[i].ClientLoginSay();
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