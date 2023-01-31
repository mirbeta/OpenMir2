using GameGate.Conf;
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
        /// 配置文件
        /// </summary>
        private static ConfigManager ConfigManager => ConfigManager.Instance;
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<SessionMessage> SessionMessageQueue;
        /// <summary>
        /// 客户端会话列表
        /// </summary>
        private readonly ClientSession[][] _sessionMap;

        private SessionManager()
        {
            SessionMessageQueue = Channel.CreateUnbounded<SessionMessage>();
            _sessionMap = new ClientSession[ConfigManager.GateConfig.ServerWorkThread][];
            for (var i = 0; i < _sessionMap.Length; i++)
            {
                _sessionMap[i] = new ClientSession[GateShare.MaxSession];
            }
        }

        /// <summary>
        /// 获取待处理的队列数量
        /// </summary>
        public int QueueCount => SessionMessageQueue.Reader.Count;

        /// <summary>
        /// 添加到消息处理队列
        /// </summary>
        public void Enqueue(SessionMessage sessionPacket)
        {
            SessionMessageQueue.Writer.TryWrite(sessionPacket);
        }

        /// <summary>
        /// 转发GameSvr封包消息
        /// </summary>
        public void ProcessSendMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await SessionMessageQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (SessionMessageQueue.Reader.TryRead(out var message))
                    {
                        var userSession = GetSession(message.ServiceId, message.SessionId);
                        if (userSession == null)
                        {
                            continue;
                        }
                        userSession.ProcessServerPacket(message);
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void AddSession(byte serviceId, int sessionId, ClientSession clientSession)
        {
            _sessionMap[serviceId][sessionId] = clientSession;
        }

        public ClientSession GetSession(byte serviceId, int sessionId)
        {
            return _sessionMap[serviceId][sessionId];
        }

        public void CloseSession(int sessionId)
        {
            _sessionMap[sessionId] = null;
        }

        public ClientSession[][] GetSessions()
        {
            return _sessionMap;
        }
    }
}