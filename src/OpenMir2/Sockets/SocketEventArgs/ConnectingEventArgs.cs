using System.Net.Sockets;

namespace SystemModule.Sockets.SocketEventArgs
{
    /// <summary>
    /// 客户端连接事件。
    /// </summary>
    public class ConnectingEventArgs : OperationEventArgs
    {
        private readonly Socket socket;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socket"></param>
        public ConnectingEventArgs(Socket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// 新初始化的通信器
        /// </summary>
        public Socket Socket => socket;
    }
}