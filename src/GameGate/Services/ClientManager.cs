using GameGate.Conf;
using System;
using System.Collections.Concurrent;
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

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        public void CheckSessionStatus(ClientThread clientThread)
        {
            if (clientThread.GateReady)
            {
                clientThread.SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, 0, "");
                clientThread.CheckServerFailCount = 0;
                return;
            }
            if (clientThread.CheckServerFail && clientThread.CheckServerFailCount <= 20)
            {
                clientThread.ReConnected();
                clientThread.CheckServerFailCount++;
                LogQueue.EnqueueDebugging($"重新与服务器[{clientThread.GetSocketIp()}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]");
                return;
            }
            clientThread.CheckServerIsTimeOut();
        }
    }
}