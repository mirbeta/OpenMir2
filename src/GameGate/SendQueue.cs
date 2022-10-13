using System;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
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
        public void AddToQueue(SessionInfo session, ReadOnlySpan<byte> buffer)
        {
            var sendPacket = new SendQueueData(session.Socket, buffer);
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
                    while (_sendQueue.Reader.TryRead(out SendQueueData sendPacket))
                    {
                        sendPacket.SendBuffer();
                    }
                }
            }, stoppingToken);
        }
    }
    
    public readonly struct SendQueueData
    {
        private readonly Socket _socket;
        private readonly byte[] _packetBuffer;
        
        public SendQueueData(Socket socket, ReadOnlySpan<byte> buff)
        {
            _socket = socket;
            _packetBuffer = buff.ToArray();
        }

        public unsafe int SendBuffer()
        {
            if (_socket == null || !_socket.Connected)
            {
                return 0;
            }
            return _socket.Send(_packetBuffer);
        }
    }
}