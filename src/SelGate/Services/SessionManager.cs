using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packets.ServerPackets;

namespace SelGate.Services
{
    public class SessionManager
    {
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<ServerDataMessage> _sendQueue = null;
        private readonly ConcurrentDictionary<int, ClientSession> _connectionSessions;

        public SessionManager()
        {
            _connectionSessions = new ConcurrentDictionary<int, ClientSession>();
            _sendQueue = Channel.CreateUnbounded<ServerDataMessage>();
        }

        public ChannelWriter<ServerDataMessage> SendQueue => _sendQueue.Writer;

        /// <summary>
        /// 处理DBSvr发送过来的消息
        /// </summary>
        public void ProcessSendMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    while (_sendQueue.Reader.TryRead(out var message))
                    {
                        var userSession = GetSession(message.SocketId);
                        if (userSession == null)
                        {
                            continue;
                        }
                        userSession.ProcessSvrData(message.Data);
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void AddSession(int sessionId, ClientSession clientSession)
        {
            _connectionSessions.TryAdd(sessionId, clientSession);
        }

        public ClientSession GetSession(int sessionId)
        {
            if (_connectionSessions.ContainsKey(sessionId))
            {
                return _connectionSessions[sessionId];
            }
            return null;
        }

        public void CloseSession(int sessionId)
        {
            if (!_connectionSessions.TryRemove(sessionId, out var clientSession))
            {
                Console.WriteLine($"移除用户会话失败:[{sessionId}]");
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