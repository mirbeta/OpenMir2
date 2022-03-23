using LoginGate.Conf;
using System.Collections.Concurrent;
using System.Linq;
using SystemModule;

namespace LoginGate
{
    /// <summary>
    /// GameGate->GameSvr
    /// </summary>
    public class ClientManager
    {
        private readonly ConcurrentDictionary<int, ClientThread> _clientThreadMap;
        private ConfigManager _configManager => ConfigManager.Instance;
        private LogQueue _logQueue => LogQueue.Instance;
        private ServerManager serverManager => ServerManager.Instance;
        private static readonly ClientManager instance = new ClientManager();

        public ClientManager()
        {
            _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
        }
        
        public static ClientManager Instance => instance;

        public void Initialization()
        {
            for (var i = 0; i < _configManager.GateConfig.m_nGateCount; i++)
            {
                var gameGate = _configManager.GameGateList[i];
                var serverAddr = gameGate.sServerAdress;
                var serverPort = gameGate.nGatePort;
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    _logQueue.Enqueue($"角色网关配置文件服务器节点[ServerAddr{i}]配置获取失败.", 1);
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
                return _clientThreadMap.TryGetValue(connectionId, out var userClinet) ? userClinet : GetClientThread();
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

        public ClientThread GetClientThread()
        {
            if (GateShare.ServerGateList.Any())
            {
                var random = new System.Random().Next(GateShare.ServerGateList.Count);
                return GateShare.ServerGateList[random];
            }
            return null;
        }

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        /// <param name="clientThread"></param>
        public void CheckSessionStatus(ClientThread clientThread)
        {
            if (clientThread.boGateReady)
            {
                clientThread.CheckServerFailCount = 0;
                return;
            }
            if (clientThread.boCheckServerFail && clientThread.CheckServerFailCount <= 20)
            {
                clientThread.ReConnected();
                clientThread.CheckServerFailCount++;
                _logQueue.Enqueue($"重新与服务器[{clientThread.GetSocketIp()}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]", 5);
                return;
            }
            if ((HUtil32.GetTickCount() - clientThread.dwCheckServerTick) > GateShare.dwCheckServerTimeOutTime && clientThread.CheckServerFailCount <= 20)
            {
                clientThread.boCheckServerFail = true;
                clientThread.Stop();
                clientThread.CheckServerFailCount++;
                _logQueue.Enqueue($"服务器[{clientThread.GetSocketIp()}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]", 5);
                return;
            }
            if (clientThread.boCheckServerFail && clientThread.CheckServerFailCount > 20 && HUtil32.GetTickCount() - clientThread.CheckServerTick > 60 * 1000)
            {
                clientThread.CheckServerTick = HUtil32.GetTickCount();
                clientThread.ReConnected();
                clientThread.CheckServerFailCount++;
                _logQueue.Enqueue($"重新与服务器[{clientThread.GetSocketIp()}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]", 5);
            }
        }
    }
}