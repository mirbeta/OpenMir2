using TouchSocket.Core;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 插件处理事件
    /// </summary>
    public class ReceivedDataEventArgs : ByteBlockEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        public ReceivedDataEventArgs(ByteBlock byteBlock, IRequestInfo requestInfo) : base(byteBlock)
        {
            RequestInfo = requestInfo;
        }

        /// <summary>
        /// 对象载体
        /// </summary>
        public IRequestInfo RequestInfo { get; }
    }
}