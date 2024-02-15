using GameGate.Services;
using System.Threading.Channels;

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
        /// 返回等待发送到客户端消息的消息数量
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
                    if (_sendQueue.Reader.TryRead(out SessionMessage sendPacket))
                    {
                        try
                        {
                            _ = ServerMgr.Send(sendPacket);
                        }
                        catch (Exception e)
                        {
                            LogService.Error(e.StackTrace);
                        }
                        finally
                        {
                            GateShare.BytePool.Return(sendPacket.Buffer, true);//必须要清空申请的byte数组
                        }
                    }
                }
            }, stoppingToken);
        }
    }
}