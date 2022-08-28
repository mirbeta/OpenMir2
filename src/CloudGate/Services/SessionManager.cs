using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CloudGate.Services
{
    public class SessionManager
    {
        private static readonly SessionManager instance = new SessionManager();
        public static SessionManager Instance => instance;

        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<TMessageData> _sendMsgList = null;
        private readonly ConcurrentDictionary<int, ClientSession> _sessionMap;
        private Task _processSendMessageTask;

        private SessionManager()
        {
            _sessionMap = new ConcurrentDictionary<int, ClientSession>();
            _sendMsgList = Channel.CreateUnbounded<TMessageData>();
        }

        /// <summary>
        /// 获取待处理的队列数量
        /// </summary>
        public int GetQueueCount => _sendMsgList.Reader.Count;

        /// <summary>
        /// 添加到消息处理队列
        /// </summary>
        /// <param name="messageData"></param>
        public void SendQueue(byte[] messageData)
        {
           // _sendMsgList.Writer.TryWrite(messageData);
        }

        /// <summary>
        /// 处理M2发过来的消息
        /// </summary>
        public void ProcessSendMessage(CancellationToken stoppingToken)
        {
            _processSendMessageTask = Task.Factory.StartNew(async () =>
            {
                while (await _sendMsgList.Reader.WaitToReadAsync(stoppingToken))
                {
                    while (_sendMsgList.Reader.TryRead(out var message))
                    {
                        try
                        {
                            var userSession = GetSession(message.MessageId);
                            if (userSession == null)
                            {
                                continue;
                            }
                            userSession.ProcessSvrData(message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }, stoppingToken);
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
            if (!_sessionMap.TryRemove(sessionId, out var clientSession))
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