﻿using PluginSystem;
using System.Threading.Tasks;

namespace SocketPlugin
{
    /// <summary>
    /// 具有断开连接的插件接口
    /// </summary>
    public interface IDisconnectedPlguin : IPlugin
    {
        /// <summary>
        /// 会话断开后触发
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="e">参数</param>
        [AsyncRaiser]
        void OnDisconnected(object client, DisconnectEventArgs e);

        /// <summary>
        /// 会话断开后触发
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        Task OnDisconnectedAsync(object client, DisconnectEventArgs e);
    }
}