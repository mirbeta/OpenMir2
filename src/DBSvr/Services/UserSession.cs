using System.Collections.Generic;

namespace DBSvr.Services
{
    public class UserSession
    {
        private readonly Dictionary<int, SessionUserInfo> _sessionMap = new Dictionary<int, SessionUserInfo>();

        public void AddSession(int sessionId, SessionUserInfo userInfo)
        {
            _sessionMap.Add(sessionId, userInfo);
        }

        public void Delete(int sessionId)
        {
            _sessionMap.Remove(sessionId);
        }
        
    }
}