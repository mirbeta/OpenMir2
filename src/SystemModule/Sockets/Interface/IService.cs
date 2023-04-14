using System;
using SystemModule.Core.Config;
using SystemModule.Sockets.Enum;

namespace SystemModule.Sockets.Interface
{
    /// <summary>
    /// 服务器接口
    /// </summary>
    public interface IService : IDisposable
    {
        /// <summary>
        /// 服务器状态
        /// </summary>
        ServerState ServerState { get; }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        TouchSocketConfig Config { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// 配置服务器
        /// </summary>
        /// <param name="serverConfig">配置</param>
        /// <exception cref="Exception"></exception>
        /// <returns>设置的服务实例</returns>
        IService Setup(TouchSocketConfig serverConfig);

        /// <summary>
        /// 配置服务器
        /// </summary>
        /// <param name="port"></param>
        /// <exception cref="Exception"></exception>
        /// <returns>设置的服务实例</returns>
        IService Setup(int port);

        /// <summary>
        /// 启动
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        /// <returns>设置的服务实例</returns>
        IService Start();

        /// <summary>
        /// 停止
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <returns>设置的服务实例</returns>
        IService Stop();
    }
}