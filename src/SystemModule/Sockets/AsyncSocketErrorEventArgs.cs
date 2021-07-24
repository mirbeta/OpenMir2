using System;

namespace NetFramework
{
    /// <summary>
    /// 异步Socket错误事件参数类
    /// </summary>
    public class AsyncSocketErrorEventArgs : EventArgs
    {
        public AsyncSocketException exception;

        /// <summary>
        /// 使用SocketException参数进行构造
        /// </summary>
        /// <param name="e">SocketException</param>
        public AsyncSocketErrorEventArgs(AsyncSocketException exception)
        {
            this.exception = exception;
        }
    }
}