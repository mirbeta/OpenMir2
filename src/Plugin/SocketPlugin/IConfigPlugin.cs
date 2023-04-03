using System.Threading.Tasks;
using SystemModule.Plugins;
using SystemModule.Sockets.SocketEventArgs;

namespace SocketPlugin
{
    /// <summary>
    /// 当配置Config时触发。
    /// </summary>
    public interface IConfigPlugin : IPlugin
    {
        /// <summary>
        /// 当载入配置时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [AsyncRaiser]
        void OnLoadingConfig(object sender, ConfigEventArgs e);

        /// <summary>
        /// 当载入配置时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        Task OnLoadingConfigAsync(object sender, ConfigEventArgs e);

        /// <summary>
        /// 当完成配置载入时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [AsyncRaiser]
        void OnLoadedConfig(object sender, ConfigEventArgs e);

        /// <summary>
        /// 当完成配置载入时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        Task OnLoadedConfigAsync(object sender, ConfigEventArgs e);
    }
}