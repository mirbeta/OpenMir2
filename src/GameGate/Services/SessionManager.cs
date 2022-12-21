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
        private static MirLog Logger => MirLog.Instance;

        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private Channel<MessagePacket> ProcessMsgQueue { get; }

        private readonly ConcurrentDictionary<int, ClientSession> _sessionMap;

        private SessionManager()
        {
            _sessionMap = new ConcurrentDictionary<int, ClientSession>();
            ProcessMsgQueue = Channel.CreateUnbounded<MessagePacket>();
        }

        /// <summary>
        /// 获取待处理的队列数量
        /// </summary>
        public int QueueCount => ProcessMsgQueue.Reader.Count;

        /// <summary>
        /// 加入会话让会话自身处理
        /// </summary>
        public void EnqueueSession(MessagePacket sessionPacket)
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
                            Logger.DebugLog("异常会话");
                            continue;
                        }
                        try
                        {
                            userSession.ProcessServerPacket(message);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(e);
                        }
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void AddSession(int sessionId, ClientSession clientSession)
        {
            _sessionMap.TryAdd(sessionId, clientSession);
        }

        public ClientSession GetSession(int sessionId)
        {
            return _sessionMap.TryGetValue(sessionId, out var clientSession) ? clientSession : null;
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

        public ClientSession[] GetSessions()
        {
            return _sessionMap.IsEmpty ? null : _sessionMap.Values.ToArray();
        }
    }
}