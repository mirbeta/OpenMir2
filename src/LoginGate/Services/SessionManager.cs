using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LoginGate
{
    public class SessionManager
    {
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<TMessageData> _sendQueue = null;
        private readonly ConcurrentDictionary<int, ClientSession> _sessionMap;

        private static readonly SessionManager instance = new SessionManager();

        public static SessionManager Instance
        {
            get { return instance; }
        }

        public SessionManager()
        {
            _sessionMap = new ConcurrentDictionary<int, ClientSession>();
            _sendQueue = Channel.CreateUnbounded<TMessageData>();
        }

        public ChannelWriter<TMessageData> SendQueue => _sendQueue.Writer;

        /// <summary>
        /// 处理LoginSvr发送过来的消息
        /// </summary>
        public async Task ProcessSendMessage()
        {
            while (await _sendQueue.Reader.WaitToReadAsync())
            {
                if (_sendQueue.Reader.TryRead(out var message))
                {
                    try
                    {
                        var userSession = GetSession(message.MessageId);
                        if (userSession == null)
                        {
                            return;
                        }
                        userSession.ProcessSvrData(message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.StackTrace);
                    }
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

        public void CloseSession(int sessionId)
        {
            if (_sessionMap.TryRemove(sessionId, out var clientSession))
            {
                clientSession.Session.Socket.Close();
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