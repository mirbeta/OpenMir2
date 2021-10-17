using System.Collections.Concurrent;

namespace GameGate
{
    public class SessionManager
    {
        private ConcurrentDictionary<string, UserClientSession> _userSessions;

        public SessionManager()
        {
            _userSessions = new ConcurrentDictionary<string, UserClientSession>();
        }

        public void AddSession(string sessionId, UserClientSession userClientSession)
        {
            _userSessions.TryAdd(sessionId, userClientSession);
        }

        public UserClientSession GetSession(string sessionId)
        {
            if (_userSessions.ContainsKey(sessionId))
            {
                return _userSessions[sessionId];
            }
            return null;
        }

        public void Remove(string sessionId)
        {
            _userSessions.TryRemove(sessionId, out var clientSession);
        }

        public bool CheckSession(string sessionId)
        {
            if (_userSessions.ContainsKey(sessionId))
            {
                return true;
            }
            return false;
        }
    }
}