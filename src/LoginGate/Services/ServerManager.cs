using LoginGate.Conf;
using LoginGate.Packet;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate.Services
{
    /// <summary>
    /// 网关服务管理类
    /// </summary>
    public class ServerManager
    {
        private readonly MirLogger _logger;
        private readonly IList<ServerService> _serverServices;
        private readonly ConfigManager _configManager;
        private readonly SessionManager _sessionManager;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 客户端登陆封包
        /// </summary>
        private readonly Channel<MessageData> _messageQueue;

        public ServerManager(MirLogger logger, IServiceProvider serviceProvider, SessionManager sessionManager, ConfigManager configManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _sessionManager = sessionManager;
            _configManager = configManager;
            _messageQueue = Channel.CreateUnbounded<MessageData>();
            _serverServices = new List<ServerService>();
        }

        public void Start()
        {
            for (var i = 0; i < _serverServices.Count; i++)
            {
                if (_serverServices[i] == null)
                {
                    continue;
                }
                _serverServices[i].Start(_configManager.GameGates[i]);
            }
        }

        public void Stop()
        {
            for (var i = 0; i < _serverServices.Count; i++)
            {
                if (_serverServices[i] == null)
                {
                    continue;
                }
                _serverServices[i].Stop();
            }
        }

        public int ReceiveQueueCount()
        {
            return _messageQueue.Reader.Count;
        }

        /// <summary>
        /// 添加到消息队列
        /// </summary>
        /// <param name="messageData"></param>
        public void SendQueue(MessageData messageData)
        {
            _messageQueue.Writer.TryWrite(messageData);
        }

        /// <summary>
        /// 客户端登陆消息封包
        /// </summary>
        public void ProcessLoginMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _messageQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (_messageQueue.Reader.TryRead(out var message))
                    {
                        var clientSession = _sessionManager.GetSession(message.ConnectionId);
                        clientSession?.HandleClientPacket(message);
                    }
                }
            }, stoppingToken);
        }

        public void Initialization()
        {
            for (var i = 0; i < _configManager.GetConfig.GateCount; i++)
            {
                var gateService = (ServerService)_serviceProvider.GetService(typeof(ServerService));
                AddServer(gateService);
            }
            _logger.DebugLog($"初始化网关服务完成.[{_serverServices.Count}]");
        }

        private void AddServer(ServerService serverService)
        {
            _serverServices.Add(serverService);
        }

        public IList<ServerService> GetServerList()
        {
            return _serverServices;
        }
    }
}