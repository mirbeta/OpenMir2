using LoginGate.Conf;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemModule;

namespace LoginGate.Services
{
    /// <summary>
    /// GameGate->GameSvr
    /// </summary>
    public class ClientManager
    {
        private readonly IList<ClientThread> _clientList;
        private readonly SessionManager _sessionManager;
        private readonly ConfigManager _configManager;
        private readonly ConcurrentDictionary<int, ClientThread> _clientThreadMap;
        private readonly LogQueue _logQueue;
        private int _processClearSessionTick = 0;
        private int _processDelayTick = 0;

        public ClientManager(LogQueue logQueue, ConfigManager configManager, SessionManager sessionManager)
        {
            _logQueue = logQueue;
            _sessionManager = sessionManager;
            _configManager = configManager;
            _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
            _clientList = new List<ClientThread>();
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
                    _logQueue.EnqueueDebugging($"角色网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
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

        private IList<ClientThread> GetAllClient()
        {
            return _clientList;
        }

        public void ProcessDelayMsg()
        {
            if (HUtil32.GetTickCount() - _processDelayTick > 200)
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
                            _sessionManager.Remove(session.SocketId);
                            _clientList[i].SessionArray[j].Socket = null;
                        }
                    }
                }
            }
        }

        public void CheckSession()
        {
            if (HUtil32.GetTickCount() - _processClearSessionTick > 20000)
            {
                _processClearSessionTick = HUtil32.GetTickCount();
                _logQueue.EnqueueDebugging("清理超时会话开始工作...");
                TSessionInfo UserSession;
                var clientList = GetAllClient();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < clientList[i].MaxSession; j++)
                    {
                        UserSession = clientList[i].SessionArray[j];
                        if (UserSession.Socket != null)
                        {
                            if ((HUtil32.GetTickCount() - UserSession.dwReceiveTick) > GateShare.dwSessionTimeOutTime)//清理超时用户会话 
                            {
                                UserSession.Socket.Close();
                                UserSession.Socket = null;
                                _sessionManager.Remove(UserSession.SocketId);
                                _logQueue.EnqueueDebugging("清理超时会话,关闭Socket.");
                            }
                        }
                    }
                    GateShare.dwCheckServerTimeMin = HUtil32.GetTickCount() - GateShare.dwCheckServerTick;
                    if (GateShare.dwCheckServerTimeMax < GateShare.dwCheckServerTimeMin)
                    {
                        GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                    }
                    CheckSessionStatus(clientList[i]);
                }
                _logQueue.EnqueueDebugging("清理超时会话工作完成...");
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
                //clientThread.SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, 0, "");
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
            if ((HUtil32.GetTickCount() - GateShare.dwCheckServerTick) > GateShare.dwCheckServerTimeOutTime && clientThread.CheckServerFailCount <= 20)
            {
                clientThread.boCheckServerFail = true;
                clientThread.Stop();
                clientThread.CheckServerFailCount++;
                _logQueue.Enqueue($"服务器[{clientThread.GetSocketIp()}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]", 5);
                return;
            }
        }
    }
}