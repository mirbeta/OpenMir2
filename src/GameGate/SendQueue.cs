using System.Net.Sockets;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GameGate
{
    public class SendQueue
    {
        private static readonly SendQueue instance = new SendQueue();

        public static SendQueue Instance
        {
            get { return instance; }
        }

        private readonly Channel<SendQueueData> _sendQueue = null;
        private readonly LogQueue _logQueue = LogQueue.Instance;

        public SendQueue()
        {
            _sendQueue = Channel.CreateUnbounded<SendQueueData>();
        }

        /// <summary>
        /// 添加到发送队列
        /// </summary>
        public void AddToQueue(TSessionInfo session, byte[] buffer)
        {
            _sendQueue.Writer.TryWrite(new SendQueueData()
            {
                Session = session,
                Buffer = buffer
            });
        }

        /// <summary>
        /// 处理M2发过来的消息
        /// </summary>
        public async Task ProcessSendQueue()
        {
            while (await _sendQueue.Reader.WaitToReadAsync())
            {
                if (_sendQueue.Reader.TryRead(out var queueData))
                {
                    var resp = await queueData.SendBuffer();
                    if (resp != queueData.Buffer.Length)
                    {
                        _logQueue.Enqueue("向客户端发送数据包失败", 5);
                    }
                }
            }
        }
    }

    public struct SendQueueData
    {
        public TSessionInfo Session;
        public byte[] Buffer;

        public async Task<int> SendBuffer()
        {
            if (Session.Socket == null || !Session.Socket.Connected)
            {
                return 0;
            }
            return await Session.Socket.SendAsync(Buffer, SocketFlags.None);
        }
    }
}