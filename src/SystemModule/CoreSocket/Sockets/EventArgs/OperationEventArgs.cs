using System;
using TouchSocket.Core;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// ClientOperationEventArgs
    /// </summary>
    [Obsolete("此类已被弃用，请使用OperationEventArgs代替", true)]
    public class ClientOperationEventArgs : TouchSocketEventArgs
    {

    }

    /// <summary>
    /// Client消息操作事件
    /// </summary>
    public class OperationEventArgs : TouchSocketEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationEventArgs()
        {
            IsPermitOperation = true;
        }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ID { get; set; }
    }
}