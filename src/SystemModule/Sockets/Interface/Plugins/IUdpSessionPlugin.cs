using System.Threading.Tasks;
using SystemModule.Plugins;
using SystemModule.Sockets.SocketEventArgs;

namespace SystemModule.Sockets.Interface.Plugins
{
    /// <summary>
    /// Udp会话插件
    /// </summary>
    public interface IUdpSessionPlugin : IPlugin
    {
        /// <summary>
        /// 在收到数据时触发
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="e">参数</param>
        [AsyncRaiser]
        void OnReceivedData(IUdpSession client, UdpReceivedDataEventArgs e);

        /// <summary>
        /// 在收到数据时触发
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        Task OnReceivedDataAsync(IUdpSession client, UdpReceivedDataEventArgs e);
    }
}