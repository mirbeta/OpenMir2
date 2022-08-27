using LoginGate.Conf;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LoginGate.Services
{
    /// <summary>
    /// MirClient会话管理
    /// </summary>
    public class SessionManager
    {
        private readonly MirLog _logger;
        private readonly ConfigManager _configManager;
        readonly ConcurrentDictionary<string, ClientSession> _sessionMap;

        public SessionManager(MirLog logger, ConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
            _sessionMap = new ConcurrentDictionary<string, ClientSession>();
        }

        public void AddSession(TSessionInfo sessionInfo, ClientThread clientThread)
        {
            var userSession = new ClientSession(_logger, sessionInfo, clientThread, _configManager);
            _sessionMap.TryAdd(sessionInfo.ConnectionId, userSession);
            userSession.UserEnter();
        }

        public ClientSession GetSession(string sessionId)
        {
            if (_sessionMap.ContainsKey(sessionId))
            {
                return _sessionMap[sessionId];
            }
            return null;
        }

        public void CloseSession(string sessionId)
        {
            if (_sessionMap.TryRemove(sessionId, out var clientSession))
            {
                clientSession.CloseSession();
            }
        }

        public bool CheckSession(string sessionId)
        {
            if (_sessionMap.ContainsKey(sessionId))
            {
                return true;
            }
            return false;
        }

        public IList<ClientSession> GetAllSession()
        {
            return _sessionMap.Values.ToList();
        }
        
    }
}