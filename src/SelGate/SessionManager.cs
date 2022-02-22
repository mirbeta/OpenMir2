using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelGate
{
    public class SessionManager
    {
        private readonly ConcurrentDictionary<int, TSessionObj> _sessionsMap;

        public SessionManager()
        {
            _sessionsMap = new ConcurrentDictionary<int, TSessionObj>();
        }

        public void AddSession(int sessionId, TSessionObj clientSession)
        {
            _sessionsMap.TryAdd(sessionId, clientSession);
        }

        public void DeleteSession(int sessionId)
        {
            _sessionsMap.TryRemove(sessionId, out var clientSession);
        }

        public TSessionObj GetClient(int sessionId)
        {
            TSessionObj clientSession;
            _sessionsMap.TryGetValue(sessionId, out clientSession);
            return clientSession;
        }
    }
}