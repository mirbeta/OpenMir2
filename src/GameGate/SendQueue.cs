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

        private readonly Channel<QueueData> _sendMsgList = null;
        private readonly LogQueue _logQueue = LogQueue.Instance;

        public SendQueue()
        {
            _sendMsgList = Channel.CreateUnbounded<QueueData>();
        }

        /// <summary>
        /// 添加到发送队列
        /// </summary>
        /// <param name="queueData"></param>
        public void AddToQueue(QueueData queueData)
        {
            _sendMsgList.Writer.TryWrite(queueData);
        }
        
        /// <summary>
        /// 处理M2发过来的消息
        /// </summary>
        public async Task ProcessSendQueue()
        {
            while (await _sendMsgList.Reader.WaitToReadAsync())
            {
                if (_sendMsgList.Reader.TryRead(out var sendData))
                {
                    if (sendData.Socket == null || !sendData.Socket.Connected) continue;
                    var resp = await sendData.Socket.SendAsync(sendData.Buffer, SocketFlags.None);
                    if (resp != sendData.Buffer.Length)
                    {
                        _logQueue.Enqueue("向客户端发送数据包失败", 5);
                    }
                }
            }
        }
    }

    public class QueueData
    {
        public Socket Socket;
        public byte[] Buffer;
    }
}