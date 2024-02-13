using OpenMir2;
using SelGate.Conf;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SelGate.Services
{
    /// <summary>
    /// GameGate->GameSrv
    /// </summary>
    public class ClientManager
    {

        private readonly IList<ClientThread> _clientList;
        private readonly SessionManager _sessionManager;
        private readonly ConfigManager _configManager;
        private readonly ConcurrentDictionary<string, ClientThread> _clientThreadMap;

        public ClientManager(SessionManager sessionManager, ConfigManager configManager)
        {
            _configManager = configManager;
            _sessionManager = sessionManager;
            _clientThreadMap = new ConcurrentDictionary<string, ClientThread>();
            _clientList = new List<ClientThread>();
        }

        public void Initialization()
        {
            for (int i = 0; i < _configManager.GateConfig.GateCount; i++)
            {
                string serverAddr = _configManager.m_xGameGateList[i].sServerAdress;
                int serverPort = _configManager.m_xGameGateList[i].nServerPort;
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    LogService.Debug($"角色网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
                    return;
                }
                _clientList.Add(new ClientThread(i, serverAddr, serverPort, _sessionManager));
            }
        }

        public void Start()
        {
            for (int i = 0; i < _clientList.Count; i++)
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
            for (int i = 0; i < _clientList.Count; i++)
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
        public void AddClientThread(string connectionId, ClientThread clientThread)
        {
            _clientThreadMap.TryAdd(connectionId, clientThread); //链接成功后建立对应关系
        }

        /// <summary>
        /// 获取用户链接对应网关
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public ClientThread GetClientThread(string connectionId)
        {
            return _clientThreadMap.TryGetValue(connectionId, out ClientThread userClient) ? userClient : GetClientThread();
        }

        /// <summary>
        /// 从字典删除用户和网关对应关系
        /// </summary>
        /// <param name="connectionId"></param>
        public void DeleteClientThread(string connectionId)
        {
            _clientThreadMap.TryRemove(connectionId, out ClientThread userClient);
        }

        public ClientThread GetClientThread()
        {
            if (GateShare.ServerGateList.Any())
            {
                int random = RandomNumber.GetInstance().Random(GateShare.ServerGateList.Count);
                return GateShare.ServerGateList[random];
            }
            return null;
        }
    }
}