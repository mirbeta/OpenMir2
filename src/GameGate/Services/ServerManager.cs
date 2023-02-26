﻿using GameGate.Conf;
using NLog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.MemoryPool;

namespace GameGate.Services
{
    public enum MessageThreadState
    {
        Runing,
        Stop
    }

    public class ServerManager
    {
        private static readonly ServerManager instance = new ServerManager();
        public static ServerManager Instance => instance;
        /// <summary>
        /// 配置文件
        /// </summary>
        private static ConfigManager ConfigManager => ConfigManager.Instance;
        /// <summary>
        /// 服务器列表
        /// </summary>
        private ServerService[] _serverServices;
        /// <summary>
        /// 消息消费者线程
        /// </summary>
        private ClientMessageWorkThread[] _messageWorkThreads;
        private int LastMessageThreadCount { get; set; }
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private readonly Channel<SessionMessage> _messageQueue;

        private ServerManager()
        {
            _messageQueue = Channel.CreateUnbounded<SessionMessage>();
        }

        public void Initialization()
        {
            _serverServices = new ServerService[ConfigManager.GateConfig.ServerWorkThread];
            for (var i = 0; i < _serverServices.Length; i++)
            {
                _serverServices[i] = new ServerService(ConfigManager.GateList[i]);
            }
        }

        public void Start(CancellationToken stoppingToken)
        {
            for (var i = 0; i < _serverServices.Length; i++)
            {
                if (_serverServices[i] == null)
                {
                    continue;
                }
                _serverServices[i].Start(stoppingToken);
            }
        }

        public void Stop()
        {
            for (var i = 0; i < _serverServices.Length; i++)
            {
                if (_serverServices[i] == null)
                {
                    continue;
                }
                _serverServices[i].Stop();
            }
        }

        /// <summary>
        /// 添加到客户端消息队列
        /// </summary>
        public void Send(ClientOutPacketData outPacket)
        {
            _serverServices[outPacket.ThreadId].Send(outPacket.ConnectId, outPacket.Buffer);
            GateShare.BytePool.Return(outPacket.Buffer, true);
        }

        /// <summary>
        /// 客户端消息添加到队列给服务端处理
        /// GameGate -> GameSrv
        /// </summary>
        public void SendMessageQueue(SessionMessage messagePacket)
        {
            _messageQueue.Writer.TryWrite(messagePacket);
        }

        /// <summary>
        /// 消息处理线程数
        /// </summary>
        public static int MessageWorkThreads => ConfigManager.GateConfig.MessageWorkThread;

        /// <summary>
        /// 开启客户端消息消费线程
        /// </summary>
        public void StartMessageWorkThread(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(() =>
            {
                if (LastMessageThreadCount == ConfigManager.GateConfig.MessageWorkThread)
                {
                    return;
                }
                if (ConfigManager.GateConfig.MessageWorkThread > LastMessageThreadCount)
                {
                    Array.Resize(ref _messageWorkThreads, ConfigManager.GateConfig.MessageWorkThread);
                    for (var i = 0; i < ConfigManager.GateConfig.MessageWorkThread; i++)
                    {
                        if (_messageWorkThreads[i] == null)
                        {
                            _messageWorkThreads[i] = new ClientMessageWorkThread(stoppingToken, _messageQueue.Reader);
                        }
                        if (_messageWorkThreads[i].ThreadState == MessageThreadState.Stop)
                        {
                            _messageWorkThreads[i]?.Start();
                        }
                    }
                }
                else
                {
                    for (var i = _messageWorkThreads.Length - 1; i >= ConfigManager.GateConfig.MessageWorkThread; i--)
                    {
                        if (_messageWorkThreads[i] == null)
                        {
                            continue;
                        }
                        if (_messageWorkThreads[i].ThreadState == MessageThreadState.Runing)
                        {
                            _messageWorkThreads[i]?.Stop();
                            _messageWorkThreads[i] = null;
                        }
                    }
                }
                LastMessageThreadCount = ConfigManager.GateConfig.MessageWorkThread;
            }, stoppingToken);
        }

        public ServerService[] GetServerList()
        {
            return _serverServices;
        }

        public ClientThread GetClientThread(out int threadId)
        {
            //TODO 根据配置文件有四种模式  默认随机
            //1.轮询分配
            //2.总是分配到最小资源 即网关在线人数最小的那个
            //3.一直分配到一个 直到当前玩家达到配置上线，则开始分配到其他可用网关
            //4.按权重分配
            threadId = -1;
            if (!_serverServices.Any())
                return null;
            if (_serverServices.Length == 1)
            {
                threadId = 0;
                return _serverServices[0].ClientThread;
            }
            var random = RandomNumber.GetInstance().Random(_serverServices.Length);
            threadId = random;
            return _serverServices[random].ClientThread;
        }

        /// <summary>
        /// 客户端消息处理工作线程
        /// </summary>
        private class ClientMessageWorkThread : IDisposable
        {
            private readonly ManualResetEvent _resetEvent;
            private string _threadId;
            private readonly CancellationTokenSource _cts;
            /// <summary>
            /// 接收封包（客户端-》网关）
            /// </summary>
            private readonly ChannelReader<SessionMessage> _messageQueue;
            public MessageThreadState ThreadState;
            private static SessionManager SessionMgr => SessionManager.Instance;
            private readonly Logger _logger = LogManager.GetCurrentClassLogger();

            public ClientMessageWorkThread(CancellationToken stoppingToken, ChannelReader<SessionMessage> channel)
            {
                _messageQueue = channel;
                _resetEvent = new ManualResetEvent(true);
                ThreadState = MessageThreadState.Stop;
                _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                _cts.Token.Register(() =>
                    _logger.Debug($"消息消费线程[{_threadId}]已停止处理.")
                );
            }

            public void Start()
            {
                Task.Factory.StartNew(async () =>
                {
                    _threadId = Guid.NewGuid().ToString("N");
                    ThreadState = MessageThreadState.Runing;
                    _logger.Debug($"消息消费线程[{_threadId}]已启动.");
                    while (await _messageQueue.WaitToReadAsync(_cts.Token))
                    {
                        _resetEvent.WaitOne();
                        if (_messageQueue.TryRead(out var message))
                        {
                            var clientSession = SessionMgr.GetSession(message.ServiceId, message.SessionId);
                            try
                            {
                                clientSession?.ProcessPacket(message);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e);
                            }
                        }
                    }
                }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            }

            public void Stop()
            {
                ThreadState = MessageThreadState.Stop;
                _resetEvent.Reset();//暂停
                if (_messageQueue.Count > 0)
                {
                    _cts.CancelAfter(_messageQueue.Count * 100);//延时取消消费，防止消息丢失
                }
                else
                {
                    _cts.Cancel();
                }
            }

            public void Dispose()
            {
                _resetEvent.Dispose();
                _cts.Dispose();
            }
        }
    }
}