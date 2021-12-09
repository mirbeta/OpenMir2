using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GameGate
{
    public class SessionManager
    {
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        public Channel<TSendUserData> SendMsgList = null;
        private ConcurrentDictionary<int, ClientSession> _connectionSessions;

        public SessionManager()
        {
            _connectionSessions = new ConcurrentDictionary<int, ClientSession>();
            SendMsgList = Channel.CreateUnbounded<TSendUserData>();
        }
        
        /// <summary>
        /// 处理M2发过来的消息
        /// </summary>
        public async Task ProcessSendMessage()
        {
            while (await SendMsgList.Reader.WaitToReadAsync())
            {
                if (SendMsgList.Reader.TryRead(out var message))
                {
                    var userSession = GetSession(message.UserCientId);
                    if (userSession == null)
                    {
                        return;
                    }
                    userSession.ProcessSvrData(message.Buffer);
                }
            }
        }

        public void AddSession(int sessionId, ClientSession userClientSession)
        {
            _connectionSessions.TryAdd(sessionId, userClientSession);
        }

        public ClientSession GetSession(int sessionId)
        {
            if (_connectionSessions.ContainsKey(sessionId))
            {
                return _connectionSessions[sessionId];
            }
            return null;
        }
        
        public void Remove(int sessionId)
        {
            if (_connectionSessions.TryRemove(sessionId, out var clientSession))
            {
               
            }
        }

        public bool CheckSession(int sessionId)
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