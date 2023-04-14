using NLog;
using System.Threading.Tasks;

namespace SocketPlugin.Impl
{
    /// <summary>
    /// UdpSessionPluginBase
    /// </summary>
    public class UdpSessionPluginBase : UdpSessionPluginBase<IUdpSession>
    {
    }

    /// <summary>
    /// Udp插件实现类
    /// </summary>
    public class UdpSessionPluginBase<TSession> : DisposableObject, IUdpSessionPlugin
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Logger Logger { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPluginsManager PluginsManager { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        void IUdpSessionPlugin.OnReceivedData(IUdpSession client, UdpReceivedDataEventArgs e)
        {
            OnReceivedData((TSession)client, e);
        }

        Task IUdpSessionPlugin.OnReceivedDataAsync(IUdpSession client, UdpReceivedDataEventArgs e)
        {
            return OnReceivedDataAsync((TSession)client, e);
        }

        /// <summary>
        /// 收到数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        protected virtual void OnReceivedData(TSession client, UdpReceivedDataEventArgs e)
        {
        }

        /// <summary>
        /// 在收到数据时触发
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual Task OnReceivedDataAsync(TSession client, UdpReceivedDataEventArgs e)
        {
            return EasyTask.CompletedTask;
        }
    }
}