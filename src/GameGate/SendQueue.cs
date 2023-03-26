using GameGate.Services;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GameGate
{
    public class SendQueue
    {
        private readonly Channel<SessionMessage> _sendQueue;
        private readonly ServerManager ServerMgr = ServerManager.Instance;

        public SendQueue()
        {
            _sendQueue = Channel.CreateUnbounded<SessionMessage>();
        }

        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        public int QueueCount => _sendQueue.Reader.Count;

        /// <summary>
        /// 添加到发送队列
        /// </summary>
        public void AddClientQueue(SessionMessage sessionPacket)
        {
            _sendQueue.Writer.TryWrite(sessionPacket);
        }

        /// <summary>
        /// 消息发送队列
        /// </summary>
        public void StartProcessQueueSend(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (_sendQueue.Reader.TryRead(out var sendPacket))
                    {
                        try
                        {
                            ServerMgr.Send(sendPacket);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }

    public readonly struct ClientOutPacketData
    {
        public readonly string ConnectId;
        public readonly int ThreadId;
        public readonly byte[] Buffer;

        public ClientOutPacketData(string connectId, int threadId, byte[] buff)
        {
            ConnectId = connectId;
            ThreadId = threadId;
            Buffer = buff;
        }
    }
}