using SystemModule.CoreSocket;

namespace SystemModule.Sockets.SocketEventArgs
{
    /// <summary>
    /// 字节事件
    /// </summary>
    public class ByteBlockEventArgs : TouchSocketEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ByteBlockEventArgs(ByteBlock byteBlock)
        {
            ByteBlock = byteBlock;
        }

        /// <summary>
        /// 数据块
        /// </summary>
        public ByteBlock ByteBlock { get; private set; }
    }
}