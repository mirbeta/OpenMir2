using System;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    /// <summary>
    /// 通讯中发生的通讯相关异常
    /// </summary>
    public class AsyncSocketException : Exception
    {
        private const string m_asyncSocketException = "异步通讯中发生通讯异常.";
        private readonly AsyncSocketErrorCode m_errorCode;

        /// <summary>
        /// 使用默认错误提示构造
        /// </summary>
        public AsyncSocketException() :
            base(m_asyncSocketException)
        {
            m_errorCode = AsyncSocketErrorCode.ServerStartFailure;
        }

        public AsyncSocketException(string message, SocketException socketException) :
            base(String.Format("{0} - {1}",
            message, m_asyncSocketException), socketException)
        {
            m_errorCode = AsyncSocketErrorCode.ThrowSocketException;
        }

        /// <summary>
        /// 使用自定义错误提示信息和错误码进行构造
        /// </summary>
        /// <param name="message">自定义错误信息</param>
        /// <param name="errorCode">错误码</param>
        //public AsyncSocketException( AsyncSocketErrorCode errorCode) :
        // base(String.Format("{0} - {1}",
        //"", m_asyncSocketException))
        public AsyncSocketException(string message, AsyncSocketErrorCode errorCode) :
            base(String.Format("{0} - {1}",
          message, m_asyncSocketException))
        {
            m_errorCode = errorCode;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public AsyncSocketErrorCode ErrorCode
        {
            get
            {
                return m_errorCode;
            }
        }
    }
}