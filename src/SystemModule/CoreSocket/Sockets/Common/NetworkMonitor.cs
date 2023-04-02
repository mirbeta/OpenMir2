using System;
using System.Net.Sockets;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 网络监听器
    /// </summary>
    public class NetworkMonitor
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPHost"></param>
        /// <param name="socket"></param>
        public NetworkMonitor(IPHost iPHost, Socket socket)
        {
            this.iPHost = iPHost;
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
        }

        private readonly IPHost iPHost;

        /// <summary>
        /// 监听地址组
        /// </summary>
        public IPHost IPHost => iPHost;

        private readonly Socket socket;

        /// <summary>
        /// Socket组件
        /// </summary>
        public Socket Socket => socket;
    }
}