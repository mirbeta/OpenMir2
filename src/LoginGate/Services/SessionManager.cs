using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate
{
    public class SessionManager
    {
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<TMessageData> _sendQueue = null;
        private readonly ConcurrentDictionary<int, ClientSession> _sessionMap;

        public SessionManager()
        {
            _sessionMap = new ConcurrentDictionary<int, ClientSession>();
            _sendQueue = Channel.CreateUnbounded<TMessageData>();
        }

        public static SessionManager Instance { get; } = new SessionManager();

        public ChannelWriter<TMessageData> SendQueue => _sendQueue.Writer;

        /// <summary>
        /// 处理LoginSvr发送过来的消息
        /// </summary>
        public Task ProcessSendMessage()
        {
            return Task.Factory.StartNew(async () =>
             {
                 while (await _sendQueue.Reader.WaitToReadAsync())
                 {
                     while (_sendQueue.Reader.TryRead(out var message))
                     {
                         var userSession = GetSession(message.MessageId);
                         if (userSession == null)
                         {
                             continue;
                         }
                         if (message.Body[0] == (byte)'+') //收到DB服务器发过来的关闭会话请求
                         {
                             if (message.Body[1] == (byte)'-')
                             {
                                 userSession.CloseSession();
                                 Console.WriteLine("收到LoginSvr关闭会话请求");
                             }
                             else
                             {
                                 userSession.ClientThread.KeepAliveTick = HUtil32.GetTickCount();
                             }
                             return;
                         }
                         userSession.ProcessSvrData(message);
                     }
                 }
             });
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