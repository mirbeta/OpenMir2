using System;
using System.Threading.Tasks;
using SystemModule.Extensions;
using TouchSocket.Core;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 端口转发辅助
    /// </summary>
    public class NATSocketClient : SocketClient
    {
        internal Action<NATSocketClient, ITcpClient, DisconnectEventArgs> m_internalDis;
        internal Func<NATSocketClient, ITcpClient, ByteBlock, IRequestInfo, byte[]> m_internalTargetClientRev;
        private readonly ConcurrentList<ITcpClient> m_targetClients = new ConcurrentList<ITcpClient>();

        /// <summary>
        /// 添加转发客户端。
        /// </summary>
        /// <param name="config">配置文件</param>
        /// <param name="setupAction">当完成配置，但是还未连接时回调。</param>
        /// <returns></returns>
        public ITcpClient AddTargetClient(TouchSocketConfig config, Action<ITcpClient> setupAction = default)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Disconnected += TcpClient_Disconnected;
            tcpClient.Received += TcpClient_Received;
            tcpClient.Setup(config);
            setupAction?.Invoke(tcpClient);
            tcpClient.Connect();

            m_targetClients.Add(tcpClient);
            return tcpClient;
        }

        /// <summary>
        /// 添加转发客户端。
        /// </summary>
        /// <param name="config">配置文件</param>
        /// <param name="setupAction">当完成配置，但是还未连接时回调。</param>
        /// <returns></returns>
        public Task<ITcpClient> AddTargetClientAsync(TouchSocketConfig config, Action<ITcpClient> setupAction = default)
        {
            return EasyTask.Run(() =>
            {
                return AddTargetClient(config, setupAction);
            });
        }

        /// <summary>
        /// 获取所有目标客户端
        /// </summary>
        /// <returns></returns>
        public ITcpClient[] GetTargetClients()
        {
            return m_targetClients.ToArray();
        }

        /// <summary>
        /// 发送数据到全部转发端。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void SendToTargetClient(byte[] buffer, int offset, int length)
        {
            foreach (ITcpClient socket in m_targetClients)
            {
                try
                {
                    socket.Send(buffer, offset, length);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDisconnected(DisconnectEventArgs e)
        {
            foreach (ITcpClient client in m_targetClients)
            {
                client.TryShutdown();
                client.SafeDispose();
            }
            base.OnDisconnected(e);
        }

        private void TcpClient_Disconnected(ITcpClientBase client, DisconnectEventArgs e)
        {
            client.Dispose();
            m_targetClients.Remove((ITcpClient)client);
            m_internalDis?.Invoke(this, (ITcpClient)client, e);
        }

        private void TcpClient_Received(TcpClient client, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (DisposedValue)
            {
                return;
            }

            try
            {
                byte[] data = m_internalTargetClientRev?.Invoke(this, client, byteBlock, requestInfo);
                if (data != null)
                {
                    if (Online)
                    {
                        this.Send(data);
                    }
                }
            }
            catch
            {
            }
        }
    }
}