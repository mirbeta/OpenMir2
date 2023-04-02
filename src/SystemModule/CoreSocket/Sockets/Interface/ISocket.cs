using System;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// Socket基接口
    /// </summary>
    public interface ISocket : IDisposable
    {
        /// <summary>
        /// 数据交互缓存池限制
        /// </summary>
        int BufferLength { get; }
    }
}