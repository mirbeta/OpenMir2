using System.Net;
using SystemModule.CoreSocket;

namespace SystemModule.Sockets.DataAdapter.Udp
{
    /// <summary>
    /// UdpKcpPackageAdapter
    /// </summary>
    public class UdpKcpPackageAdapter : NormalUdpDataHandlingAdapter
    {
        /// <inheritdoc/>
        protected override void PreviewReceived(EndPoint remoteEndPoint, ByteBlock byteBlock)
        {
            base.PreviewReceived(remoteEndPoint, byteBlock);
        }
    }
}