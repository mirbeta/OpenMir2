using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Channels;

namespace GameSvr
{
    public class SendQueue
    {
        private readonly Channel<byte[]> _sendQueue = null;
        private readonly Socket _sendSocket;
        private readonly CancellationTokenSource _cancellation;

        public SendQueue(Socket socket)
        {
            _sendQueue = Channel.CreateUnbounded<byte[]>();
            _cancellation = new CancellationTokenSource();
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
            _cancellation.Cancel();
        }

        /// <summary>
        /// 处理队列数据并发送到GameGate
        /// GameSvr -> GameGate
        /// </summary>
        public async Task ProcessSendQueue()
        {
            while (await _sendQueue.Reader.WaitToReadAsync(_cancellation.Token))
            {
                if (_sendQueue.Reader.TryRead(out var buffer))
                {
                    if (_sendSocket.Connected)
                    {
                        var sendLen = _sendSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

                        Debug.WriteLine(sendLen);
                    }
                }
            }
        }
    }
}
