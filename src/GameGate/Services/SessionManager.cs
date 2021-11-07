using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameGate
{
    public class SessionManager
    {
        private ConcurrentDictionary<string, ClientSession> _connectionSessions;
        private ConcurrentDictionary<int, ClientSession> _idxSessions;

        public SessionManager()
        {
            _connectionSessions = new ConcurrentDictionary<string, ClientSession>();
            _idxSessions = new ConcurrentDictionary<int, ClientSession>();
        }

        public void AddSession(string sessionId,int idx, ClientSession userClientSession)
        {
            _connectionSessions.TryAdd(sessionId, userClientSession);
            _idxSessions.TryAdd(idx, userClientSession);
        }
        
        public ClientSession GetSession(string sessionId)
        {
            if (_connectionSessions.ContainsKey(sessionId))
            {
                return _connectionSessions[sessionId];
            }
            return null;
        }
        
        public ClientSession GetSession(int sessionId)
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

        public IList<ClientSession> GetAllSession()
        {
            return _connectionSessions.Values.ToList();
        }
    }
}