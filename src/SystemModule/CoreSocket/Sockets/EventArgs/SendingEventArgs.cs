using SystemModule.CoreSocket;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 发送事件
    /// </summary>
    public class SendingEventArgs : TouchSocketEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public SendingEventArgs(byte[] buffer, int offset, int length)
        {
            Buffer = buffer;
            Offset = offset;
            Length = length;
            IsPermitOperation = true;
        }

        /// <summary>
        /// 数据缓存区，该属性获取来自于内存池，所以最好不要引用该对象，可以同步使用该对象
        /// </summary>
        public byte[] Buffer { get; }

        /// <summary>
        /// 缓存偏移
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length { get; }
    }
}