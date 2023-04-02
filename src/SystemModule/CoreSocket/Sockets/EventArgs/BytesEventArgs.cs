using TouchSocket.Core;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 字节事件
    /// </summary>
    public class BytesEventArgs : TouchSocketEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public BytesEventArgs(byte[] data)
        {
            ReceivedDataBytes = data;
        }

        /// <summary>
        /// 字节数组
        /// </summary>
        public byte[] ReceivedDataBytes { get; private set; }
    }
}