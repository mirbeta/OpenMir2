using System.Net.Sockets;
using System.Threading.Channels;

namespace GameSvr.GameGate
{
    public class SendQueue
    {
        private readonly Channel<byte[]> _sendQueue = null;
        private readonly Socket _sendSocket;
        private Task _processSendQueueTask; 

        public SendQueue(Socket socket)
        {
            _sendQueue = Channel.CreateUnbounded<byte[]>();
            _sendSocket = socket;
        }

        /// <summary>
        /// 获取队列消息数量
        /// </summary>
        public int GetQueueCount => _sendQueue.Reader.Count;

        /// <summary>
        /// 添加到发送队列
        /// </summary>
        public void AddToQueue(byte[] buffer)
        {
            _sendQueue.Writer.TryWrite(buffer);
        }

        public void Stop()
        {
            _processSendQueueTask.Dispose();
        }

        /// <summary>
        /// 处理队列数据并发送到GameGate
        /// GameSvr -> GameGate
        /// </summary>
        public void ProcessSendQueue(CancellationTokenSource cancellation)
        {
            _processSendQueueTask = Task.Factory.StartNew(async () =>
            {
                while (await _sendQueue.Reader.WaitToReadAsync(cancellation.Token))
                {
                    while (_sendQueue.Reader.TryRead(out var buffer))
                    {
                        if (_sendSocket.Connected)
                        {
                            //todo 此处异步发送效率比同步效率要低很多,不知道为什么,暂时先保持同步发送
                            _sendSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                        }
                    }
                }
            });
        }
    }
}
