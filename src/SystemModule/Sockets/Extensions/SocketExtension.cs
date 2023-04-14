using System;
using System.Net.Sockets;

namespace SystemModule.Sockets.Extensions
{
    /// <summary>
    /// SocketExtension
    /// </summary>
    public static class SocketExtension
    {
        /// <summary>
        /// 会使用同步锁，保证所有数据上缓存区。
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public static void AbsoluteSend(this Socket socket, byte[] buffer, int offset, int length)
        {
            lock (socket)
            {
                while (length > 0)
                {
                    int r = socket.Send(buffer, offset, length, SocketFlags.None);
                    if (r == 0 && length > 0)
                    {
                        throw new Exception("发送数据不完全");
                    }
                    offset += r;
                    length -= r;
                }
            }
        }

        /// <summary>
        /// 会使用同步锁，保证所有数据上缓存区。
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public static void AbsoluteSend(this Socket socket, ReadOnlyMemory<byte> buffer, int offset, int length)
        {
            lock (socket)
            {
                while (length > 0)
                {
                    int r = socket.Send(buffer.Span, SocketFlags.None);
                    if (r == 0 && length > 0)
                    {
                        throw new Exception("发送数据不完全");
                    }
                    offset += r;
                    length -= r;
                }
            }
        }
    }
}