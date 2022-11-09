using NLog;
using System.Collections.Concurrent;

namespace LoginSvr.Services
{
    public class ClientManager
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentDictionary<int, GateInfo> _gateInfos = new ConcurrentDictionary<int, GateInfo>();

        public int Gates => _gateInfos.Count;

        public GateInfo GetSession(int sessionId)
        {
            if (_gateInfos.TryGetValue(sessionId, out var userInfo))
            {
                return userInfo;
            }
            return null;
        }

        public bool AddSession(int socketId, GateInfo userInfo)
        {
            if (_gateInfos.ContainsKey(socketId))
            {
                return false;
            }
            _gateInfos.TryAdd(socketId, userInfo);
            return true;
        }

        public void Delete(int socketId)
        {
            if (_gateInfos.ContainsKey(socketId))
            {
                if (_gateInfos.TryRemove(socketId, out var gateInfo))
                {
                    if (gateInfo != null && gateInfo.UserList != null)
                    {
                        for (var j = 0; j < gateInfo.UserList.Count; j++)
                        {
                            _logger.Debug("Close: " + gateInfo.UserList[j].UserIPaddr);
                            gateInfo.UserList[j].Socket.Close();
                            gateInfo.UserList[j] = null;
                        }
                        gateInfo.UserList = null;
                    }
                }
            }
        }
    }
}