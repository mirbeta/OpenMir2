using SelGate.Conf;
using SelGate.Package;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemModule;

namespace SelGate.Services
{
    /// <summary>
    /// GameGate->GameSvr
    /// </summary>
    public class ClientManager
    {
        private readonly MirLog _logQueue;
        private readonly IList<ClientThread> _clientList;
        private readonly SessionManager _sessionManager;
        private readonly ConfigManager _configManager;
        private readonly ConcurrentDictionary<string, ClientThread> _clientThreadMap;
        private int _processClearSessionTick = 0;
        private int _lastChekSocketTick = 0;
        private int _processDelayTick = 0;

        public ClientManager(MirLog logQueue, SessionManager sessionManager,ConfigManager configManager)
        {
            _logQueue = logQueue;
            _configManager = configManager;
            _sessionManager = sessionManager;
            _clientThreadMap = new ConcurrentDictionary<string, ClientThread>();
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
                    _logQueue.LogDebug($"角色网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
                    return;
                }
                _clientList.Add(new ClientThread(i, serverAddr, serverPort, _sessionManager, _logQueue));
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

        public IList<ClientThread> GetAllClient()
        {
            return _clientList;
        }

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
            if (!string.IsNullOrEmpty(connectionId))
            {
                return _clientThreadMap.TryGetValue(connectionId, out var userClinet) ? userClinet : GetClientThread();
            }
            return null;
        }

        /// <summary>
        /// 从字典删除用户和网关对应关系
        /// </summary>
        /// <param name="connectionId"></param>
        public void DeleteClientThread(string connectionId)
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

        private IList<ClientThread> GetClientThreads()
        {
            return _clientList;
        }

        public void Process()
        {
            CleanOutSession();
            ProcessDelayMsg();
            CheckSocketState();
        }

        private void ProcessDelayMsg()
        {
            if (HUtil32.GetTickCount() - _processDelayTick > 20 * 1000)
            {
                _processDelayTick = HUtil32.GetTickCount();
                for (var i = 0; i < _clientList.Count; i++)
                {
                    if (_clientList[i] == null)
                    {
                        continue;
                    }
                    if (_clientList[i].SessionArray == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < _clientList[i].SessionArray.Length; j++)
                    {
                        var session = _clientList[i].SessionArray[j];
                        if (session == null)
                        {
                            continue;
                        }
                        if (session.Socket == null)
                        {
                            continue;
                        }
                        var userClient = _sessionManager.GetSession(session.SocketId);
                        if (userClient == null)
                        {
                            continue;
                        }
                        var success = false;
                        userClient.HandleDelayMsg(ref success);
                        if (success)
                        {
                            _sessionManager.CloseSession(session.SocketId);
                            _clientList[i].SessionArray[j].Socket = null;
                            _clientList[i].SessionArray[j] = null;
                        }
                    }
                }
            }
        }

        private void CleanOutSession()
        {
            if (HUtil32.GetTickCount() - _processClearSessionTick > 20 * 1000)
            {
                _processClearSessionTick = HUtil32.GetTickCount();
                TSessionInfo UserSession;
                var clientList = GetClientThreads();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < ClientThread.MaxSession; j++)
                    {
                        UserSession = clientList[i].SessionArray[j];
                        if (UserSession == null)
                        {
                            continue;
                        }
                        if (UserSession.Socket != null)
                        {
                            if ((HUtil32.GetTickCount() - UserSession.dwReceiveTick) > GateShare.SessionTimeOutTime) //清理超时用户会话 
                            {
                                UserSession.Socket.Close();
                                UserSession.Socket = null;
                                _sessionManager.CloseSession(UserSession.SocketId);
                                UserSession = null;
                                _logQueue.LogDebug("清理超时会话,关闭超时Socket.");
                            }
                        }
                    }
                }
                _logQueue.LogDebug("Cleanup timeout session...");
            }
        }

        private void CheckSocketState()
        {
            if (HUtil32.GetTickCount() - _lastChekSocketTick > 10000)
            {
                _lastChekSocketTick = HUtil32.GetTickCount();
                var clientList = GetClientThreads();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    CheckSessionStatus(clientList[i]);
                }
            }
        }

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        /// <param name="clientThread"></param>
        private void CheckSessionStatus(ClientThread clientThread)
        {
            if (clientThread.boGateReady)
            {
                clientThread.SendKeepAlive();
                clientThread.CheckServerFailCount = 0;
                return;
            }
            if ((HUtil32.GetTickCount() - GateShare.CheckServerTick) > GateShare.CheckServerTimeOutTime)
            {
                if (clientThread.CheckServerFail)
                {
                    clientThread.ReConnected();
                    clientThread.CheckServerFailCount++;
                    _logQueue.LogDebug($"服务器[{clientThread.GetEndPoint()}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]");
                    return;
                }
                clientThread.CheckServerFail = true;
                clientThread.Stop();
                clientThread.CheckServerFailCount++;
                _logQueue.LogDebug($"服务器[{clientThread.GetEndPoint()}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]");
            }
        }
    }
}