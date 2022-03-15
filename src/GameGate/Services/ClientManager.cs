using System.Collections.Concurrent;
using SystemModule;

namespace GameGate
{
    /// <summary>
    /// GameGate->GameSvr
    /// </summary>
    public class ClientManager
    {
        private static readonly ClientManager instance = new ClientManager();

        public static ClientManager Instance
        {
            get { return instance; }
        }

        private ServerManager serverManager => ServerManager.Instance;
        private readonly ConcurrentDictionary<int, ClientThread> _clientThreadMap;
        
        public ClientManager()
        {
            _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
        }

        private LogQueue _logQueue => LogQueue.Instance;
        private ConfigManager _configManager => ConfigManager.Instance;

        public void Initialization()
        {
            for (var i = 0; i < _configManager.GateConfig.GateCount; i++)
            {
                var gameGate = _configManager.GameGateList[i];
                var serverAddr = gameGate.sServerAdress;
                var serverPort = gameGate.nServerPort;
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    _logQueue.Enqueue($"游戏网关配置文件服务器节点[ServerAddr{i}]配置获取失败.", 1);
                    return;
                }
                serverManager.AddServer(new ServerService(i, gameGate));
            }
        }

        /// <summary>
        /// 添加用户对饮网关
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="clientThread"></param>
        public void AddClientThread(int connectionId, ClientThread clientThread)
        {
            _clientThreadMap.TryAdd(connectionId, clientThread); //链接成功后建立对应关系
        }

        /// <summary>
        /// 获取用户链接对应网关
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public ClientThread GetClientThread(int connectionId)
        {
            if (connectionId > 0)
            {
                return _clientThreadMap.TryGetValue(connectionId, out var userClinet) ? userClinet : null;
            }
            return null;
        }

        /// <summary>
        /// 从字典删除用户和网关对应关系
        /// </summary>
        /// <param name="connectionId"></param>
        public void DeleteClientThread(int connectionId)
        {
            _clientThreadMap.TryRemove(connectionId, out var userClinet);
        }

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        /// <param name="clientThread"></param>
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
                _logQueue.EnqueueDebugging($"重新与服务器[{clientThread.GetSocketIp()}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]");
                return;
            }
            if ((HUtil32.GetTickCount() - GateShare.dwCheckServerTick) > GateShare.dwCheckServerTimeOutTime && clientThread.CheckServerFailCount <= 20)
            {
                clientThread.CheckServerFail = true;
                clientThread.Stop();
                clientThread.CheckServerFailCount++;
                _logQueue.EnqueueDebugging($"服务器[{clientThread.GetSocketIp()}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]");
                return;
            }
        }
    }
}