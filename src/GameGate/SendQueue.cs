using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GameGate
{
    public class SendQueue
    {
        private readonly Channel<SendQueueData> _sendQueue;
        private readonly MirLog _logQueue = MirLog.Instance;

        public SendQueue()
        {
            _sendQueue = Channel.CreateUnbounded<SendQueueData>();
        }

        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        public int GetQueueCount => _sendQueue.Reader.Count;

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
        /// 将队列消息发送到客户端
        /// </summary>
        public async Task ProcessSendQueue(CancellationToken stoppingToken)
        {
            while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
            {
                while (_sendQueue.Reader.TryRead(out var queueData))
                {
                    var resp = queueData.SendBuffer();
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

        public int SendBuffer()
        {
            if (Session.Socket == null || !Session.Socket.Connected)
            {
                return 0;
            }
            return Session.Socket.Send(Buffer, 0, Buffer.Length, SocketFlags.None);
        }
    }
}