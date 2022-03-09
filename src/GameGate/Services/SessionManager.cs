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
        private readonly Channel<TMessageData> _sendMsgList = null;
        private readonly ConcurrentDictionary<int, ClientSession> _sessionMap;

        public SessionManager()
        {
            _sessionMap = new ConcurrentDictionary<int, ClientSession>();
            _sendMsgList = Channel.CreateUnbounded<TMessageData>();
        }
        
        public ChannelWriter<TMessageData> SendQueue => _sendMsgList.Writer;
        
        /// <summary>
        /// 处理M2发过来的消息
        /// </summary>
        public async Task ProcessSendMessage()
        {
            while (await _sendMsgList.Reader.WaitToReadAsync())
            {
                if (_sendMsgList.Reader.TryRead(out var message))
                {
                    var userSession = GetSession(message.MessageId);
                    if (userSession == null)
                    {
                        return;
                    }
                    userSession.ProcessSvrData(message);
                }
            }
        }

        public void AddSession(int sessionId, ClientSession clientSession)
        {
            _sessionMap.TryAdd(sessionId, clientSession);
        }

        public ClientSession GetSession(int sessionId)
        {
            if (_sessionMap.ContainsKey(sessionId))
            {
                return _sessionMap[sessionId];
            }
            return null;
        }
        
        public void Remove(int sessionId)
        {
            if (_sessionMap.TryRemove(sessionId, out var clientSession))
            {
               
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