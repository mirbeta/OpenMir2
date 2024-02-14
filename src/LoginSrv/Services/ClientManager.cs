namespace LoginSrv.Services
{
    public class ClientManager
    {
        private readonly ConcurrentDictionary<string, LoginGateInfo> _gateInfos = new ConcurrentDictionary<string, LoginGateInfo>();

        public int Gates => _gateInfos.Count;

        public LoginGateInfo GetSession(string sessionId)
        {
            if (_gateInfos.TryGetValue(sessionId, out LoginGateInfo userInfo))
            {
                return userInfo;
            }
            return null;
        }

        public bool AddSession(string socketId, LoginGateInfo userInfo)
        {
            if (_gateInfos.ContainsKey(socketId))
            {
                return false;
            }
            _gateInfos.TryAdd(socketId, userInfo);
            return true;
        }

        public void Delete(string socketId)
        {
            if (_gateInfos.ContainsKey(socketId))
            {
                if (_gateInfos.TryRemove(socketId, out LoginGateInfo gateInfo))
                {
                    if (gateInfo != null && gateInfo.UserList != null)
                    {
                        for (int j = 0; j < gateInfo.UserList.Count; j++)
                        {
                            LogService.Debug("Close: " + gateInfo.UserList[j].UserIPaddr);
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