using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using GameGate.Conf;

namespace GameGate.Services
{
    /// <summary>
    /// 客户端会话容器
    /// </summary>
    public class SessionContainer
    {
        private static readonly SessionContainer instance = new SessionContainer();
        public static SessionContainer Instance => instance;
        /// <summary>
        /// 配置文件
        /// </summary>
        private static ConfigManager ConfigManager => ConfigManager.Instance;
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        private readonly Channel<SessionMessage> _messageChannel;
        /// <summary>
        /// 客户端会话列表
        /// </summary>
        private readonly ClientSession[][] _sessionMap;

        private SessionContainer()
        {
            _messageChannel = Channel.CreateUnbounded<SessionMessage>();
            _sessionMap = new ClientSession[ConfigManager.GateConfig.ServerWorkThread][];
            for (var i = 0; i < _sessionMap.Length; i++)
            {
                _sessionMap[i] = new ClientSession[GateShare.MaxSession];
            }
        }

        /// <summary>
        /// 获取待处理的队列数量
        /// </summary>
        public int QueueCount => _messageChannel.Reader.Count;

        /// <summary>
        /// 添加到消息处理队列
        /// </summary>
        public void Enqueue(SessionMessage sessionPacket)
        {
            _messageChannel.Writer.TryWrite(sessionPacket);
        }

        /// <summary>
        /// 转发GameSvr封包消息
        /// </summary>
        public Task ProcessSendMessage(CancellationToken stoppingToken)
        {
           return Task.Factory.StartNew(async () =>
            {
                while (await _messageChannel.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (_messageChannel.Reader.TryRead(out var message))
                    {
                        var userSession = GetSession(message.ServiceId, message.SessionId);
                        if (userSession == null)
                        {
                            continue;
                        }
                        userSession.ProcessServerPacket(message);
                    }
                }
            }, stoppingToken);
        }

        public void AddSession(byte serviceId, int sessionId, ClientSession clientSession)
        {
            _sessionMap[serviceId][sessionId] = clientSession;
        }

        public ClientSession GetSession(byte serviceId, int sessionId)
        {
            return _sessionMap[serviceId] == null ? null : _sessionMap[serviceId][sessionId];
        }

        public void CloseSession(byte serviceId, int sessionId)
        {
            if (serviceId > _sessionMap.Length)
            {
                return;
            }
            _sessionMap[serviceId][sessionId] = null;
        }

        public ClientSession[][] GetSessions()
        {
            return _sessionMap;
        }
    }
}