using GameGate.Services;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GameGate
{
    public class SendQueue
    {
        private readonly Channel<ClientPacketQueueData> _sendQueue;
        private readonly MirLog _logQueue = MirLog.Instance;
        private readonly ServerManager serverManager = ServerManager.Instance;

        public SendQueue()
        {
            _sendQueue = Channel.CreateUnbounded<ClientPacketQueueData>();
        }

        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        public int QueueCount => _sendQueue.Reader.Count;

        /// <summary>
        /// 添加到发送队列
        /// </summary>
        public void AddClientQueue(string connectionId, int threadId, Span<byte> buffer)
        {
            // var sendPacket = new ClientPacketQueueData(connectionId, threadId, buffer);
            // _sendQueue.Writer.TryWrite(sendPacket);

            serverManager.SendClientQueue(connectionId, threadId, buffer);
        }

        /// <summary>
        /// 将队列消息发送到客户端
        /// </summary>
        public void ProcessSendQueue(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (_sendQueue.Reader.TryRead(out ClientPacketQueueData sendPacket))
                    {
                        //serverManager.SendClientQueue(sendPacket.ConnectId, sendPacket.ThreadId, sendPacket.PacketBuffer);
                    }
                }
            }, stoppingToken);
        }

        private readonly struct ClientPacketQueueData
        {
            public readonly string ConnectId;
            public readonly int ThreadId;
            public readonly Memory<byte> PacketBuffer;

            public ClientPacketQueueData(string connectId,int threadId, Memory<byte> buff)
            {
                ConnectId = connectId;
                ThreadId = threadId;
                PacketBuffer = buff;
            }
        }
    }
}