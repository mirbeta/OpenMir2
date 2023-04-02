using TouchSocket.Core;

namespace TouchSocket.Sockets
{

    /// <summary>
    /// TCP端口转发服务器
    /// </summary>
    public class NATService : TcpService<NATSocketClient>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override NATSocketClient GetClientInstence()
        {
            NATSocketClient client = base.GetClientInstence();
            client.m_internalDis = OnTargetClientDisconnected;
            client.m_internalTargetClientRev = OnTargetClientReceived;
            return client;
        }

        /// <summary>
        /// 在NAT服务器收到数据时。
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        /// <returns>需要转发的数据。</returns>
        protected virtual byte[] OnNATReceived(NATSocketClient socketClient, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            return byteBlock?.ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        protected sealed override void OnReceived(NATSocketClient socketClient, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            byte[] data = OnNATReceived(socketClient, byteBlock, requestInfo);
            if (data != null)
            {
                socketClient.SendToTargetClient(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 当目标客户端断开。
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="tcpClient"></param>
        /// <param name="e"></param>
        protected virtual void OnTargetClientDisconnected(NATSocketClient socketClient, ITcpClient tcpClient, DisconnectEventArgs e)
        {
        }

        /// <summary>
        /// 在目标客户端收到数据时。
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="tcpClient"></param>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        /// <returns></returns>
        protected virtual byte[] OnTargetClientReceived(NATSocketClient socketClient, ITcpClient tcpClient, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            return byteBlock?.ToArray();
        }
    }
}