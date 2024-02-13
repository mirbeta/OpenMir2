/// <summary>
/// 客户端会话管理
/// </summary>
public class SessionManager
{
    private readonly ConfigManager _configManager;
    private readonly ConcurrentDictionary<string, ClientSession> _sessionMap;

    public SessionManager(ConfigManager configManager)
    {
        _configManager = configManager;
        _sessionMap = new ConcurrentDictionary<string, ClientSession>();
    }

    public void AddSession(TSessionInfo sessionInfo, ClientThread clientThread)
    {
        ClientSession userSession = new ClientSession(sessionInfo, clientThread, _configManager,
            GateShare.ServiceProvider.GetService<ServerService>());
        _sessionMap.TryAdd(sessionInfo.ConnectionId, userSession);
        userSession.UserEnter();
    }

    public ClientSession GetSession(string sessionId)
    {
        return _sessionMap.GetValueOrDefault(sessionId);
    }

    public void CloseSession(string sessionId)
    {
        if (_sessionMap.TryRemove(sessionId, out ClientSession clientSession))
        {
            clientSession.CloseSession();
        }
    }

    public bool CheckSession(string sessionId)
    {
        return _sessionMap.ContainsKey(sessionId);
    }

    public IList<ClientSession> GetAllSession()
    {
        return _sessionMap.Values.ToList();
    }
}