using GameGate.Services;
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
        private readonly ServerManager serverManager = ServerManager.Instance;

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
        public void AddToQueue(string connectionId, ReadOnlySpan<byte> buffer)
        {
            var sendPacket = new SendQueueData(connectionId, buffer);
            _sendQueue.Writer.TryWrite(sendPacket);
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
                    if (_sendQueue.Reader.TryRead(out SendQueueData sendPacket))
                    {
                        serverManager.SendClientQueue(sendPacket.ConnectId, sendPacket.PacketBuffer);
                    }
                }
            }, stoppingToken);
        }
    }
    
    public readonly struct SendQueueData
    {
        public readonly string ConnectId;
        public readonly byte[] PacketBuffer;
        
        public SendQueueData(string connectId, ReadOnlySpan<byte> buff)
        {
            this.ConnectId = connectId;
            PacketBuffer = buff.ToArray();
        }
    }
}