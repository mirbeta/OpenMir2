using System;
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
        public void AddToQueue(TSessionInfo session, ReadOnlySpan<byte> buffer)
        {
            var sendPacket = new SendQueueData()
            {
                Socket = session.Socket,
                Buffer = buffer.ToArray()
            };
            _sendQueue.Writer.TryWrite(sendPacket);
        }

        /// <summary>
        /// 将队列消息发送到客户端
        /// </summary>
        public async Task ProcessSendQueue(CancellationToken stoppingToken)
        {
            while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
            {
                SendQueueData sendPacket;
                while (_sendQueue.Reader.TryRead(out sendPacket))
                {
                    var resp = sendPacket.SendBuffer();
                    if (resp != sendPacket.Buffer.Length)
                    {
                        _logQueue.Enqueue("向客户端发送数据包失败", 5);
                    }
                }
            }
        }
    }

    public struct SendQueueData
    {
        public Socket Socket;
        public ReadOnlyMemory<byte> Buffer;

        public int SendBuffer()
        {
            if (Socket == null || !Socket.Connected)
            {
                return 0;
            }
            return Socket.Send(Buffer.Span);
        }
    }
}