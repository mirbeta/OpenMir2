using System;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 超长异常
    /// </summary>
    [Serializable]
    public class OverlengthException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OverlengthException()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public OverlengthException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public OverlengthException(string message, System.Exception inner) : base(message, inner)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected OverlengthException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            
        }
    }
}