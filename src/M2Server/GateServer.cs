using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace M2Server
{
    public class GateServer
    {
        /// <summary>
        /// 游戏网关
        /// </summary>
        private readonly ISocketServer _gateSocket = null;
        private readonly Channel<GateData> _gateChannel;
        private readonly Timer timer;

        public GateServer()
        {
            _gateSocket = new ISocketServer(20, 2048);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            _gateSocket.Init();
            _gateChannel = Channel.CreateUnbounded<GateData>();
            timer = new Timer(Test, null, 2000, 10000);
        }

        private void Test(object obj)
        {
            Console.WriteLine("待处理消息:" + _gateChannel.Reader.Count);
        }

        public void Start()
        {
            _gateSocket.Start(M2Share.g_Config.sGateAddr, M2Share.g_Config.nGatePort);
        }

        private void GateSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            //M2Share.RunSocket.CloseErrGate();
        }

        private void GateSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            M2Share.RunSocket.CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            M2Share.RunSocket.AddGate(e);
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            //M2Share.RunSocket.SocketRead(e);

            var data = new byte[e.BytesReceived];
            Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, e.BytesReceived);
            var nMsgLen = e.BytesReceived;
            if (nMsgLen <= 0)
            {
                return;
            }
            _gateChannel.Writer.TryWrite(new GateData()
            {
                ConnectionId = e.ConnectionId,
                Buffer = data
            });
        }

        public async Task StartProduct()
        {
            var gTasks = new Task[1];
            var consumer1 = new GateProduct(_gateChannel.Reader);
            var consumerTask1 = consumer1.ConsumeData();
            gTasks[0] = consumerTask1;

            await Task.WhenAll(gTasks);
        }
    }

    /// <summary>
    /// 网关消费者
    /// </summary>
    public class GateProduct
    {
        private readonly ChannelReader<GateData> _reader;

        public GateProduct(ChannelReader<GateData> reader)
        {
            _reader = reader;
        }

        public async Task ConsumeData()
        {
            Console.WriteLine($"GateProduct Starting");

            while (await _reader.WaitToReadAsync())
            {
                if (_reader.TryRead(out var token))
                {
                    M2Share.RunSocket.SocketRead(token);
                }
            }
        }
    }

    public class GateData
    {
        public string ConnectionId;
        public byte[] Buffer;
    }
}