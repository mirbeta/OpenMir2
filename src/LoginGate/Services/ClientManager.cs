/// <summary>
/// LoginGate->LoginSrv
/// </summary>
public class ClientManager
{
    private readonly Channel<ServerDataMessage> _sendQueue;
    private readonly SessionManager _sessionManager;
    private readonly IList<ClientThread> _serverGateList;
    private readonly ConcurrentDictionary<int, ClientThread> _clientThreadMap;
    private readonly ConfigManager _configManager;
    private readonly ServerManager _serverManager;

    public ClientManager(SessionManager sessionManager, ConfigManager configManager, ServerManager serverManager)
    {
        _sessionManager = sessionManager;
        _configManager = configManager;
        _serverManager = serverManager;
        _sendQueue = Channel.CreateUnbounded<ServerDataMessage>();
        _serverGateList = new List<ClientThread>();
        _clientThreadMap = new ConcurrentDictionary<int, ClientThread>();
    }

    public IList<ClientThread> Clients => _serverGateList;

    public void Initialization()
    {
        var serverList = _serverManager.GetServerList();
        for (var i = 0; i < serverList.Count; i++) _serverGateList.Add(new ClientThread(this, _sessionManager));
    }

    public void Start()
    {
        for (var i = 0; i < _serverGateList.Count; i++) _serverGateList[i].Start(_configManager.GameGates[i]);
    }

    public void Stop()
    {
        for (var i = 0; i < _serverGateList.Count; i++) _serverGateList[i].Stop();
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
    /// <param name="message"></param>
    public void SendQueue(ServerDataMessage message)
    {
        _sendQueue.Writer.TryWrite(message);
    }

    /// <summary>
    /// LoginSvr消息封包
    /// </summary>
    public void ProcessSendMessage(CancellationToken stoppingToken)
    {
        Task.Factory.StartNew(async () =>
        {
            while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
                if (_sendQueue.Reader.TryRead(out var message))
                {
                    var userSession = _sessionManager.GetSession(message.SocketId);
                    if (userSession == null) continue;
                    userSession.ProcessSvrData(message.Data);
                }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
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
            return _clientThreadMap.TryGetValue(socketId, out var userClinet) ? userClinet : GetClientThread();
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
    public void ProcessClientHeart(ClientThread clientThread)
    {
        if (clientThread.ConnectState)
        {
            clientThread.SendClientPacket(new ServerDataMessage()
            {
                Type = ServerDataType.KeepAlive,
                SocketId = string.Empty
            });
            clientThread.CheckServerFailCount = 1;
            if (HUtil32.GetTickCount() - clientThread.KeepAliveTick >
                GateShare.KeepAliveTickTimeOut) //30s没有LoginSvr服务器心跳回应则超时
            {
                LogService.Warn("账号服务器长时间没有响应，断开链接。");
                clientThread.ConnectState = false;
                clientThread.Stop();
            }

            return;
        }

        if (HUtil32.GetTickCount() - clientThread.CheckServerTick > GateShare.CheckServerTimeOutTime)
        {
            clientThread.CheckServerTick = HUtil32.GetTickCount();
            if (clientThread.CheckServerFail)
            {
                clientThread.ReConnected();
                clientThread.CheckServerFailCount++;
                LogService.Debug($"重新与服务器[{clientThread.EndPoint}]建立链接.失败次数:[{clientThread.CheckServerFailCount}]");
                return;
            }

            clientThread.Stop();
            clientThread.CheckServerFailCount++;
            LogService.Debug($"服务器[{clientThread.EndPoint}]链接超时.失败次数:[{clientThread.CheckServerFailCount}]");
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