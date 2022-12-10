using LoginGate.Conf;
using LoginGate.Packet;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NLog;
using SystemModule.Logger;

namespace LoginGate.Services
{
    /// <summary>
    /// 网关服务管理类
    /// </summary>
    public class ServerManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<ServerService> _serverServices;
        private readonly ConfigManager _configManager;
        private readonly SessionManager _sessionManager;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 客户端登陆封包
        /// </summary>
        private readonly Channel<MessageData> _messageQueue;

        public ServerManager(IServiceProvider serviceProvider, SessionManager sessionManager, ConfigManager configManager)
        {
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
                        var userSession = _sessionManager.GetSession(message.ConnectionId);
                        if (userSession == null)
                        {
                            _logger.Warn("非法攻击: " + message.ClientIP);
                            _logger.Debug($"获取用户对应会话失败 RemoteAddr:[{message.ClientIP}] ConnectionId:[{message.ConnectionId}]");
                            continue;
                        }
                        if (!userSession.ClientThread.ConnectState)
                        {
                            _logger.Info("未就绪: " + message.ClientIP);
                            _logger.Debug($"账号服务器链接失败 Server:[{userSession.ClientThread.EndPoint}] ConnectionId:[{message.ConnectionId}]");
                            continue;
                        }
                        try
                        {
                            userSession.HandleClientPacket(message);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e);
                        }
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void Initialization()
        {
            for (var i = 0; i < _configManager.GetConfig.GateCount; i++)
            {
                var gateService = (ServerService)_serviceProvider.GetService(typeof(ServerService));
                AddServer(gateService);
            }
            _logger.Debug($"初始化网关服务完成.[{_serverServices.Count}]");
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