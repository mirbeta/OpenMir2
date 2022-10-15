using GameGate.Conf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemModule;

namespace GameGate.Services
{
    /// <summary>
    /// GameGate->GameSvr
    /// </summary>
    public class ClientManager
    {
        private static readonly ClientManager instance = new ClientManager();
        public static ClientManager Instance => instance;
        private static ServerManager ServerManager => ServerManager.Instance;
        private static MirLog LogQueue => MirLog.Instance;
        private static ConfigManager ConfigManager => ConfigManager.Instance;

        private readonly ConcurrentDictionary<string, ClientThread> _clientThreadMap;

        private ClientManager()
        {
            _clientThreadMap = new ConcurrentDictionary<string, ClientThread>();
        }

        public void Initialization()
        {
            for (var i = 0; i < ConfigManager.GateConfig.GateCount; i++)
            {
                var gameGate = ConfigManager.GameGateList[i];
                var serverAddr = gameGate.ServerAdress;
                var serverPort = gameGate.ServerPort;
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    LogQueue.Enqueue($"游戏网关配置文件服务器节点[ServerAddr{i}]配置获取失败.", 1);
                    return;
                }
                ServerManager.AddServer(new ServerService(Guid.NewGuid().ToString("N"), gameGate));
            }
        }

        /// <summary>
        /// 添加用户对饮网关
        /// </summary>
        public void AddClientThread(string connectionId, ClientThread clientThread)
        {
            _clientThreadMap.TryAdd(connectionId, clientThread); //链接成功后建立对应关系
        }

        /// <summary>
        /// 获取用户链接对应网关
        /// </summary>
        /// <returns></returns>
        public ClientThread GetClientThread(string connectionId)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                return _clientThreadMap.TryGetValue(connectionId, out var userClinet) ? userClinet : null;
            }
            return null;
        }

        /// <summary>
        /// 从字典删除用户和网关对应关系
        /// </summary>
        public void DeleteClientThread(string connectionId)
        {
            _clientThreadMap.TryRemove(connectionId, out var userClinet);
        }

        public IList<ClientThread> GetAllClient()
        {
            return _clientThreadMap.Values.ToArray();
        }

    }
}