using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GameGate.Services
{
    public class SessionManager
    {
        private static readonly SessionManager instance = new SessionManager();
        public static SessionManager Instance => instance;

        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<ClientSessionPacket> ProcessMsgQueue;
        private readonly ConcurrentDictionary<int, ClientSession> _sessionMap;

        private SessionManager()
        {
            _sessionMap = new ConcurrentDictionary<int, ClientSession>();
            ProcessMsgQueue = Channel.CreateUnbounded<ClientSessionPacket>();
        }

        /// <summary>
        /// 获取待处理的队列数量
        /// </summary>
        public int QueueCount => ProcessMsgQueue.Reader.Count;

        /// <summary>
        /// 添加到消息处理队列
        /// </summary>
        /// <param name="sessionPacket"></param>
        public void Enqueue(ClientSessionPacket sessionPacket)
        {
            ProcessMsgQueue.Writer.TryWrite(sessionPacket);
        }

        /// <summary>
        /// 转发GameSvr封包消息
        /// </summary>
        public void ProcessSendMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await ProcessMsgQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (ProcessMsgQueue.Reader.TryRead(out var message))
                    {
                        var userSession = GetSession(message.SessionId);
                        if (userSession == null)
                        {
                            continue;
                        }
                        try
                        {
                            userSession.ProcessServerPacket(message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
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
            return _sessionMap.ContainsKey(sessionId) ? _sessionMap[sessionId] : null;
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