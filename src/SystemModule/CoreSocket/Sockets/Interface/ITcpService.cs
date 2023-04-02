using System;
using SystemModule.CoreSocket.Common;
using SystemModule.Plugins;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// TCP系列服务器接口
    /// </summary>
    public interface ITcpService<TClient> : ITcpService where TClient : ISocketClient
    {
        /// <summary>
        /// 用户连接完成
        /// </summary>
        TouchSocketEventHandler<TClient> Connected { get; set; }

        /// <summary>
        /// 有用户连接的时候
        /// </summary>
        OperationEventHandler<TClient> Connecting { get; set; }

        /// <summary>
        /// 有用户断开连接
        /// </summary>
        DisconnectEventHandler<TClient> Disconnected { get; set; }

        /// <summary>
        /// 尝试获取TClient
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="socketClient">TClient</param>
        /// <returns></returns>
        bool TryGetSocketClient(string id, out TClient socketClient);
    }

    /// <summary>
    /// TCP服务器接口
    /// </summary>
    public interface ITcpService : IService, IIDSender, IIDRequsetInfoSender, IPluginObject
    {
        /// <summary>
        /// 当前在线客户端数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 获取默认新ID。
        /// </summary>
        Func<string> GetDefaultNewID { get; }

        /// <summary>
        /// 获取最大可连接数
        /// </summary>
        int MaxCount { get; }

        /// <summary>
        /// 网络监听集合
        /// </summary>
        Common.NetworkMonitor[] Monitors { get; }

        /// <summary>
        /// 获取当前连接的所有客户端
        /// </summary>
        SocketClientCollection SocketClients { get; }

        /// <summary>
        /// 使用Ssl加密
        /// </summary>
        bool UseSsl { get; }

        /// <summary>
        /// 清理当前已连接的所有客户端
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取当前在线的所有ID集合
        /// </summary>
        /// <returns></returns>
        string[] GetIDs();

        /// <summary>
        /// 重置ID
        /// </summary>
        /// <param name="oldID"></param>
        /// <param name="newID"></param>
        /// <exception cref="ClientNotFindException"></exception>
        /// <exception cref="Exception"></exception>
        void ResetID(string oldID, string newID);

        /// <summary>
        /// 根据ID判断SocketClient是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SocketClientExist(string id);
    }
}