using LoginGate.Conf;
using LoginGate.Packet;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate.Services
{
    /// <summary>
    /// LoginGate->LoginSvr
    /// </summary>
    public class ClientManager
    {
        private readonly Channel<TMessageData> _sendQueue;
        private readonly SessionManager _sessionManager;
        private readonly MirLog _logger;
        private readonly IList<ClientThread> _serverGateList;
        private readonly ConcurrentDictionary<int, ClientThread> _clientThreadMap;
        private readonly ConfigManager _configManager;
        private readonly ServerManager _serverManager;

        public ClientManager(MirLog logger, SessionManager sessionManager, ConfigManager configManager, ServerManager serverManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _configManager = configManager;
            _serverManager = serverManager;
            _sendQueue = Channel.CreateUnbounded<TMessageData>();
            _serverGateList = new List<ClientThread>();
            _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
        }

        public IList<ClientThread> Clients => _serverGateList;

        public void Initialization()
        {
            var serverList = _serverManager.GetServerList();
            for (var i = 0; i < serverList.Count; i++)
            {
                _serverGateList.Add(new ClientThread(_logger, this));
            }
        }

        public void Start()
        {
            for (var i = 0; i < _serverGateList.Count; i++)
            {
                _serverGateList[i].Start(_configManager.GameGates[i]);
            }
        }

        public void Stop()
        {
            for (var i = 0; i < _serverGateList.Count; i++)
            {
                _serverGateList[i].Stop();
            }
        }

        public IList<ClientThread> ServerGateList()
        {
            return _serverGateList;
        }

        private int SendQueueCount()
        {
            return _sendQueue.Reader.Count;
        }

        /// <summary>
        /// 添加到发送队列
        /// </summary>
        /// <param name="messageData"></param>
        public void SendQueue(TMessageData messageData)
        {
            _sendQueue.Writer.TryWrite(messageData);
        }

        /// <summary>
        /// LoginSvr消息封包
        /// </summary>
        public void ProcessSendMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (_sendQueue.Reader.TryRead(out var message))
                    {
                        var userSession = _sessionManager.GetSession(message.ConnectionId);
                        if (userSession == null)
                        {
                            continue;
                        }
                        if (message.Body[0] == (byte)'+')//收到LoginSvr发过来的关闭会话请求
                        {
                            if (message.Body[1] == (byte)'-')
                            {
                                userSession.CloseSession();
                                _logger.LogInformation("收到LoginSvr关闭会话请求", 1);
                            }
                            else
                            {
                                userSession.ClientThread.KeepAliveTick = HUtil32.GetTickCount();
                            }
                            continue;
                        }
                        userSession.ProcessSvrData(message);
                    }
                }
            }, stoppingToken);
        }

        /// <summary>
        /// 添加网关映射
        /// </summary>
        public void AddClientThread(int socketId, ClientThread clientThread)
        {
            _clientThreadMap.TryAdd(socketId, clientThread);
        }

        /// <summary>
        /// 获取链接映射网关
        /// </summary>
        /// <returns></returns>
        public ClientThread GetClientThread(int socketId)
        {
            if (socketId > 0)
            {
                return _clientThreadMap.TryGetValue(socketId, out var userClinet) ? userClinet : GetClientThread();
            }
            return null;
        }

        /// <summary>
        /// 从字典删除网关映射关系
        /// </summary>
        public void DeleteClientThread(int socketId)
        {
            _clientThreadMap.TryRemove(socketId, out var clientThread);
        }

        /// <summary>
        /// 随机获取一个账号服务器实例
        /// </summary>
        /// <returns></returns>
        public ClientThread GetClientThread()
        {
            if (!_serverGateList.Any())
                return null;
            ClientThread clientThread;
            if (_serverGateList.Count == 1)
            {
                clientThread = _serverGateList[0];
            }
            else
            {
                var random = RandomNumber.GetInstance().Random(_serverGateList.Count);
                clientThread = _serverGateList[random];
            }
            return !clientThread.SessionIsFull() ? clientThread : null;
        }

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        /// <param name="clientThread"></param>
        public void CheckSessionStatus(ClientThread clientThread)
        {
            if (clientThread.GateReady)
            {
                clientThread.CheckServerFailCount = 1;
                return;
            }
            if (HUtil32.GetTickCount() - clientThread.dwCheckServerTick > GateShare.CheckServerTimeOutTime)
            {
                if (clientThread.CheckServerFail)
                {
                    clientThread.ReConnected();
                    clientThread.CheckServerFailCount++;
                    _logger.LogDebug($"重新与服务器[{clientThread.EndPoint}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]");
                    return;
                }
                clientThread.CheckServerFail = true;
                clientThread.Stop();
                clientThread.CheckServerFailCount++;
                _logger.LogDebug($"服务器[{clientThread.EndPoint}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]");
            }
        }
        
        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        /// <returns></returns>
        public string GetQueueCount()
        {
            return string.Concat(SendQueueCount(), "/", _serverManager.ReceiveQueueCount());
        }
    }
}