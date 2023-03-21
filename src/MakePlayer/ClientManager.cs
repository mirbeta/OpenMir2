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

    public class ClientManager
    {
        private int g_dwProcessTimeMin = 0;
        private int g_dwProcessTimeMax = 0;
        private int g_nPosition = 0;
        private int dwRunTick = 0;
        private readonly ConcurrentDictionary<string, PlayClient> _clients;
        private readonly IList<PlayClient> _clientList;
        private readonly IList<string> _sayMsgList;
        private readonly Channel<RecvicePacket> _reviceQueue;
        private readonly CancellationTokenSource _cancellation;

        public ClientManager()
        {
            _clients = new ConcurrentDictionary<string, PlayClient>();
            _clientList = new List<PlayClient>();
            _sayMsgList = new List<string>(100);
            _reviceQueue = Channel.CreateUnbounded<RecvicePacket>();
            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "SayMessage.txt");
            if (File.Exists(filePath))
            {
                var line = string.Empty;
                var sr = new StreamReader(filePath, System.Text.Encoding.ASCII);
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    _sayMsgList.Add(line);
                }
            }
            else {
                Console.WriteLine("自动发言列表文件不存在.");
            }
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

        public void Stop()
        {
            _cancellation.Cancel();
        }

        public void AddPacket(string socHandle, byte[] reviceBuff)
        {
            var clientPacket = new RecvicePacket();
            clientPacket.SessionId = socHandle;
            clientPacket.ReviceBuffer = reviceBuff;
            _reviceQueue.Writer.TryWrite(clientPacket);
        }

        public void AddClient(string sessionId, PlayClient objClient)
        {
            _clients.TryAdd(sessionId, objClient);
            _clientList.Add(objClient);
        }

        public void DelClient(PlayClient objClient)
        {
            //_Clients.Remove(objClient);
        }

        public void Run()
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
                if (_clientList[i].IsLogin && (HUtil32.GetTickCount() - _clientList[i].SayTick > 3000))
                {
                    if (_sayMsgList.Count > 0)
                    {
                        _clientList[i].SayTick = HUtil32.GetTickCount();
                        _clientList[i].ClientLoginSay(_sayMsgList[RandomNumber.GetInstance().Random(_sayMsgList.Count)]);
                    }
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