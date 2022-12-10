using LoginGate.Conf;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SystemModule.Logger;

namespace LoginGate.Services
{
    /// <summary>
    /// 客户端会话管理
    /// </summary>
    public class SessionManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConfigManager _configManager;
        private readonly ConcurrentDictionary<int, ClientSession> _sessionMap;

        public SessionManager(ConfigManager configManager)
        {
            _configManager = configManager;
            _sessionMap = new ConcurrentDictionary<int, ClientSession>();
        }

        public void AddSession(TSessionInfo sessionInfo, ClientThread clientThread)
        {
            var userSession = new ClientSession(sessionInfo, clientThread, _configManager);
            _sessionMap.TryAdd(sessionInfo.ConnectionId, userSession);
            userSession.UserEnter();
        }

        public ClientSession GetSession(int sessionId)
        {
            if (_sessionMap.ContainsKey(sessionId))
            {
                return _sessionMap[sessionId];
            }
            return null;
        }

        public void CloseSession(int sessionId)
        {
            if (_sessionMap.TryRemove(sessionId, out var clientSession))
            {
                clientSession.CloseSession();
            }
        }

        public bool CheckSession(int sessionId)
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