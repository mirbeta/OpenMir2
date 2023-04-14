using NLog;
using SelGate.Conf;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemModule;

namespace SelGate.Services
{
    /// <summary>
    /// GameGate->GameSrv
    /// </summary>
    public class ClientManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<ClientThread> _clientList;
        private readonly SessionManager _sessionManager;
        private readonly ConfigManager _configManager;
        private readonly ConcurrentDictionary<int, ClientThread> _clientThreadMap;

        public ClientManager(SessionManager sessionManager, ConfigManager configManager)
        {
            _configManager = configManager;
            _sessionManager = sessionManager;
            _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
            _clientList = new List<ClientThread>();
        }

        public void Initialization()
        {
            for (var i = 0; i < _configManager.GateConfig.m_nGateCount; i++)
            {
                var serverAddr = _configManager.m_xGameGateList[i].sServerAdress;
                var serverPort = _configManager.m_xGameGateList[i].nServerPort;
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    _logger.Debug($"角色网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
                    return;
                }
                _clientList.Add(new ClientThread(i, serverAddr, serverPort, _sessionManager));
            }
        }

        public void Start()
        {
            for (var i = 0; i < _clientList.Count; i++)
            {
                if (_clientList[i] == null)
                {
                    continue;
                }
                _clientList[i].Start();
                _clientList[i].RestSessionArray();
            }
        }

        public void Stop()
        {
            for (var i = 0; i < _clientList.Count; i++)
            {
                if (_clientList[i] == null)
                {
                    continue;
                }
                _clientList[i].Stop();
            }
        }

        public IList<ClientThread> GetClients => _clientList;

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
                var random = RandomNumber.GetInstance().Random(GateShare.ServerGateList.Count);
                return GateShare.ServerGateList[random];
            }
            return null;
        }
    }
}