using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameGate
{
    public class SessionManager
    {
        private ConcurrentDictionary<string, UserClientSession> _connectionSessions;
        private ConcurrentDictionary<int, UserClientSession> _idxSessions;

        public SessionManager()
        {
            _connectionSessions = new ConcurrentDictionary<string, UserClientSession>();
            _idxSessions = new ConcurrentDictionary<int, UserClientSession>();
        }

        public void AddSession(string sessionId,int idx, UserClientSession userClientSession)
        {
            _connectionSessions.TryAdd(sessionId, userClientSession);
            _idxSessions.TryAdd(idx, userClientSession);
        }
        
        public UserClientSession GetSession(string sessionId)
        {
            if (_connectionSessions.ContainsKey(sessionId))
            {
                return _connectionSessions[sessionId];
            }
            return null;
        }
        
        public UserClientSession GetSession(int sessionId)
        {
            if (_idxSessions.ContainsKey(sessionId))
            {
                return _idxSessions[sessionId];
            }
            return null;
        }

        public void Remove(string sessionId)
        {
            if (_connectionSessions.TryRemove(sessionId, out var clientSession))
            {
                _idxSessions.TryRemove(clientSession.SessionId, out clientSession);
            }
        }

        public bool CheckSession(string sessionId)
        {
            if (_connectionSessions.ContainsKey(sessionId))
            {
                return true;
            }
            return false;
        }

        public IList<UserClientSession> GetAllSession()
        {
            return _connectionSessions.Values.ToList();
        }
    }
}