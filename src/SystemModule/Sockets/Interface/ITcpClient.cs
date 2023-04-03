using System;
using System.Threading.Tasks;
using SystemModule.CoreSocket;
using SystemModule.Plugins;
using SystemModule.Sockets.Common;

namespace SystemModule.Sockets.Interface
{
    /// <summary>
    /// TCP客户端终端接口
    /// </summary>
    public interface ITcpClient : ITcpClientBase, IClientSender, IPluginObject
    {
        /// <summary>
        /// 成功连接到服务器
        /// </summary>
        MessageEventHandler<ITcpClient> Connected { get; set; }

        /// <summary>
        /// 准备连接的时候
        /// </summary>
        ConnectingEventHandler<ITcpClient> Connecting { get; set; }

        /// <summary>
        /// 远程IPHost。
        /// </summary>
        IPHost RemoteIPHost { get; }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <exception cref="TimeoutException"></exception>
        /// <exception cref="Exception"></exception>
        ITcpClient Connect(int timeout = 5000);

        /// <summary>
        /// 异步连接服务器
        /// </summary>
        /// <exception cref="TimeoutException"></exception>
        /// <exception cref="Exception"></exception>
        Task<ITcpClient> ConnectAsync(int timeout = 5000);

        /// <summary>
        /// 配置服务器
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="Exception"></exception>
        ITcpClient Setup(TouchSocketConfig config);

        /// <summary>
        /// 配置服务器
        /// </summary>
        /// <param name="ipHost"></param>
        /// <returns></returns>
        ITcpClient Setup(string ipHost);
    }
}