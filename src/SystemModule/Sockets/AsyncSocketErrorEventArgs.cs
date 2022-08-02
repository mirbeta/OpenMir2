using System;

namespace SystemModule.Sockets
{
    /// <summary>
    /// 异步Socket错误事件参数类
    /// </summary>
    public class AsyncSocketErrorEventArgs : EventArgs
    {
        private AsyncSocketException _exception;

        /// <summary>
        /// 使用SocketException参数进行构造
        /// </summary>
        /// <param name="exception"></param>
        public AsyncSocketErrorEventArgs(AsyncSocketException exception)
        {
            this._exception = exception;
        }

        public AsyncSocketException Exception
        {
            get { return _exception; }
        }
    }
}