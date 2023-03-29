using GameGate.Services;
using NLog;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GameGate
{
    public class SendQueue
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
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
                            logger.Error(e.StackTrace);
                        }
                        finally
                        {
                            GateShare.BytePool.Return(sendPacket.Buffer, true);
                        }
                    }
                }
            }, stoppingToken);
        }
    }
}