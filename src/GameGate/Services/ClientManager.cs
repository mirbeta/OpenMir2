using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameGate
{
    /// <summary>
    /// GameGate->GameSvr
    /// </summary>
    public class ClientManager
    {
        /// <summary>
        /// 点击最多链接10个客户端
        /// </summary>
        private readonly ClientThread[] _gateClient = new ClientThread[10];
        private readonly SessionManager _sessionManager;
        private readonly ConfigManager _configManager;
        private Thread _delayThread;
        private static ConcurrentDictionary<int, ClientThread> _clientThreadMap;
        
        public ClientManager(SessionManager sessionManager,ConfigManager configManager)
        {
            _sessionManager = sessionManager;
            _configManager = configManager;
            _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
        }

        public void LoadConfig()
        {
            var serverCount =_configManager.GateConfig.m_nGateCount;
            var serverAddr = string.Empty;
            var serverPort = 0;
            for (var i = 0; i < serverCount; i++)
            {
                serverAddr = _configManager.m_xGameGateList[i].sServerAdress;
                serverPort = _configManager.m_xGameGateList[i].nServerPort;
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    Console.WriteLine($"网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
                    return;
                }
                _gateClient[i] = new ClientThread(serverAddr, serverPort, _sessionManager);
                _gateClient[i].GateIdx = i;
            }
        }

        public void Start()
        {
            for (var i = 0; i < _gateClient.Length; i++)
            {
                if (_gateClient[i] == null)
                {
                    continue;
                }
                _gateClient[i].Start();
                _gateClient[i].RestSessionArray();
            }

            _delayThread = new Thread(ProcessDelayMsg);
            _delayThread.IsBackground = true;
            _delayThread.Start();
        }

        public void Stop()
        {
            for (var i = 0; i < _gateClient.Length; i++)
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
            var userList = new List<ClientThread>(GateShare.ServerGateList.Count);
            if (userList.Any())
            {
                var random = new System.Random().Next(userList.Count);
                return userList[random];
            }
            return null;
        }
        
        public IList<ClientThread> GetAllClient()
        {
            return _gateClient;
        }

        private void ProcessDelayMsg(object obj)
        {
            while (true)
            {
                for (var i = 0; i < _gateClient.Length; i++)
                {
                    if (_gateClient[i] == null)
                    {
                        continue;
                    }
                    if (_gateClient[i].SessionArray == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < _gateClient[i].SessionArray.Length; j++)
                    {
                        var session = _gateClient[i].SessionArray[j];
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
                        userClient.HandleDelayMsg();
                        //todo 清理超时会话
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}