using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemModule;

namespace GameGate
{
    /// <summary>
    /// GameGate->GameSvr
    /// </summary>
    public class ClientManager
    {
        private readonly IList<ClientThread> _gateClient;
        private readonly SessionManager _sessionManager;
        private readonly ConfigManager _configManager;
        private readonly ConcurrentDictionary<int, ClientThread> _clientThreadMap;
        private readonly LogQueue _logQueue;

        public ClientManager(LogQueue logQueue, ConfigManager configManager, SessionManager sessionManager)
        {
            _logQueue = logQueue;
            _sessionManager = sessionManager;
            _configManager = configManager;
            _gateClient = new List<ClientThread>();
            _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
        }

        public void Initialization()
        {
            var serverAddr = string.Empty;
            var serverPort = 0;
            for (var i = 0; i < _configManager.GateConfig.m_nGateCount; i++)
            {
                serverAddr = _configManager.m_xGameGateList[i].sServerAdress;
                serverPort = _configManager.m_xGameGateList[i].nServerPort;
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    _logQueue.Enqueue($"游戏网关配置文件服务器节点[ServerAddr{i}]配置获取失败.", 1);
                    return;
                }
                _gateClient.Add(new ClientThread(i, serverAddr, serverPort, _sessionManager, _logQueue));
            }
        }

        public void Start()
        {
            for (var i = 0; i < _gateClient.Count; i++)
            {
                if (_gateClient[i] == null)
                {
                    continue;
                }
                _gateClient[i].Start();
                _gateClient[i].RestSessionArray();
            }
        }

        public void Stop()
        {
            for (var i = 0; i < _gateClient.Count; i++)
            {
                if (_gateClient[i] == null)
                {
                    continue;
                }
                _gateClient[i].Stop();
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
            //TODO 根据配置文件有四种模式  默认随机
            //1.轮询分配
            //2.总是分配到最小资源 即网关在线人数最小的那个
            //3.一直分配到一个 直到当前玩家达到配置上线，则开始分配到其他可用网关
            //4.按权重分配
            if (GateShare.ServerGateList.Any())
            {
                var random = new System.Random().Next(GateShare.ServerGateList.Count);
                return GateShare.ServerGateList[random];
            }
            return null;
        }

        public IList<ClientThread> GetAllClient()
        {
            return _gateClient;
        }

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        /// <param name="clientThread"></param>
        public void CheckSessionStatus(ClientThread clientThread)
        {
            if (clientThread.boGateReady)
            {
                clientThread.SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, 0, "");
                clientThread.CheckServerFailCount = 0;
                return;
            }
            if (clientThread.boCheckServerFail && clientThread.CheckServerFailCount <= 20)
            {
                clientThread.ReConnected();
                clientThread.CheckServerFailCount++;
                _logQueue.EnqueueDebugging($"重新与服务器[{clientThread.GetSocketIp()}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]");
                return;
            }
            if ((HUtil32.GetTickCount() - GateShare.dwCheckServerTick) > GateShare.dwCheckServerTimeOutTime && clientThread.CheckServerFailCount <= 20)
            {
                clientThread.boCheckServerFail = true;
                clientThread.Stop();
                clientThread.CheckServerFailCount++;
                _logQueue.EnqueueDebugging($"服务器[{clientThread.GetSocketIp()}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]");
                return;
            }
        }
    }
}