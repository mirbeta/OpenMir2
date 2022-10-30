using System.Collections.Generic;

namespace LoginSvr.Services
{
    public class UserManager
    {
        private readonly Dictionary<int, UserInfo> _userInfos = new Dictionary<int, UserInfo>();
        
        public UserInfo GetUser(int sessionId)
        {
            return new UserInfo();
        }

        public void AddUser(int sessionId, UserInfo userInfo)
        {
            _userInfos.Add(sessionId,userInfo);
        }
    }
}