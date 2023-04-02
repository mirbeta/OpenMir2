using System;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 未连接异常
    /// </summary>
    [Serializable]
    public class NotConnectedException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotConnectedException()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public NotConnectedException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NotConnectedException(string message, System.Exception inner) : base(message, inner)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NotConnectedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            
        }
    }
}