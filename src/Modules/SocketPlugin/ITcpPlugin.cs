using PluginSystem;
using System.Threading.Tasks;

namespace SocketPlugin
{
    /// <summary>
    /// Tcp系插件接口
    /// </summary>
    public interface ITcpPlugin : IPlugin, IConnectingPlugin, IConnectedPlugin, IDisconnectingPlugin, IDisconnectedPlguin
    {
        /// <summary>
        /// 当Client的ID被更改后触发
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        [AsyncRaiser]
        void OnIDChanged(ITcpClientBase client, IDChangedEventArgs e);

        /// <summary>
        /// 当Client的ID被更改后触发
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        Task OnIDChangedAsync(ITcpClientBase client, IDChangedEventArgs e);

        /// <summary>
        /// 在收到数据时触发
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="e">参数</param>
        [AsyncRaiser]
        void OnReceivedData(ITcpClientBase client, ReceivedDataEventArgs e);

        /// <summary>
        /// 在收到数据时触发
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        Task OnReceivedDataAsync(ITcpClientBase client, ReceivedDataEventArgs e);

        /// <summary>
        /// 在刚收到数据时触发，即在适配器之前。
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="e">参数</param>
        [AsyncRaiser]
        void OnReceivingData(ITcpClientBase client, ByteBlockEventArgs e);

        /// <summary>
        /// 在刚收到数据时触发，即在适配器之前。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        Task OnReceivingDataAsync(ITcpClientBase client, ByteBlockEventArgs e);

        /// <summary>
        /// 当即将发送数据时，调用该方法在适配器之后，接下来即会发送数据。
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="e">参数</param>
        [AsyncRaiser]
        void OnSendingData(ITcpClientBase client, SendingEventArgs e);

        /// <summary>
        /// 当即将发送数据时，调用该方法在适配器之后，接下来即会发送数据。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        Task OnSendingDataAsync(ITcpClientBase client, SendingEventArgs e);
    }
}