using GameGate.Conf;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using OpenMir2;

namespace GameGate.Services
{
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
        /// <summary>
        /// 运行消息消费线程数
        /// </summary>
        private int RunMessageThreadCount { get; set; }
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private readonly Channel<ClientPacketMessage> _messageQueue;

        private ServerManager()
        {
            _messageQueue = Channel.CreateUnbounded<ClientPacketMessage>();
        }

        public void Initialize()
        {
            _serverServices = new ServerService[ConfigManager.GateConfig.ServerWorkThread];
            for (var i = 0; i < _serverServices.Length; i++)
            {
                _serverServices[i] = new ServerService(ConfigManager.GateList[i]);
                _serverServices[i].Initialize();
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
        public void Send(SessionMessage sendPacket)
        {
            _serverServices[sendPacket.ServiceId].Send(sendPacket);
        }

        /// <summary>
        /// 客户端消息添加到队列给服务端处理
        /// GameGate -> GameSrv
        /// </summary>
        public void SendMessageQueue(ClientPacketMessage messagePacket)
        {
            _messageQueue.Writer.TryWrite(messagePacket);
        }

        /// <summary>
        /// 消息处理线程数
        /// </summary>
        public static int MessageWorkThreads => ConfigManager.GateConfig.MessageWorkThread;

        /// <summary>
        /// 开启服务消息消费线程
        /// </summary>
        /// <param name="cancellationToken"></param>
        public void StartServerThreadMessageWork(CancellationToken cancellationToken)
        {
            Task[] tasks = new Task[_serverServices.Length];
            for (int i = 0; i < _serverServices.Length; i++)
            {
                tasks[i] = _serverServices[i].ClientThread.StartMessageQueue(cancellationToken);
            }
            Task.WaitAll(tasks, cancellationToken);
        }

        /// <summary>
        /// 开启客户端消息消费线程
        /// </summary>
        public Task StartClientMessageWork(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(() =>
            {
                if (RunMessageThreadCount == ConfigManager.GateConfig.MessageWorkThread)
                {
                    return;
                }
                if (ConfigManager.GateConfig.MessageWorkThread > RunMessageThreadCount)
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
                RunMessageThreadCount = ConfigManager.GateConfig.MessageWorkThread;
            }, stoppingToken);
        }

        public ServerService[] GetServerList()
        {
            return _serverServices;
        }

        public ClientThread GetClientThread(byte serviceId, out int threadId)
        {
            //TODO 根据配置文件有四种模式  默认随机
            //1.轮询分配
            //2.总是分配到最小资源 即网关在线人数最小的那个
            //3.一直分配到一个 直到当前玩家达到配置上线，则开始分配到其他可用网关
            //4.按权重分配
            threadId = -1;
            if (!_serverServices.Any())
                return null;
            var availableList = _serverServices.Where(x => x.ClientThread.Running == RunningState.Runing).ToArray();//允许分配玩家连接
            if (availableList.Length == 1)
            {
                threadId = 1;
                return availableList[0].ClientThread;
            }
            if (serviceId > 0)
            {
                threadId = serviceId;
                return availableList[serviceId].ClientThread;
            }
            var random = RandomNumber.GetInstance().Random(availableList.Length);
            threadId = random;
            return availableList[random].ClientThread;
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
            private readonly ChannelReader<ClientPacketMessage> _messageQueue;
            public MessageThreadState ThreadState;
            private static SessionContainer SessionContainer => SessionContainer.Instance;

            public ClientMessageWorkThread(CancellationToken stoppingToken, ChannelReader<ClientPacketMessage> channel)
            {
                _messageQueue = channel;
                _resetEvent = new ManualResetEvent(true);
                ThreadState = MessageThreadState.Stop;
                _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                _cts.Token.Register(() =>
                    LogService.Debug($"消息消费线程[{_threadId}]已停止处理.")
                );
            }

            public void Start()
            {
                Task.Factory.StartNew(async () =>
                {
                    _threadId = Guid.NewGuid().ToString("N");
                    ThreadState = MessageThreadState.Runing;
                    LogService.Debug($"消息消费线程[{_threadId}]已启动.");
                    while (await _messageQueue.WaitToReadAsync(_cts.Token))
                    {
                        _resetEvent.WaitOne();
                        if (_messageQueue.TryRead(out var message))
                        {
                            var clientSession = SessionContainer.GetSession(message.ServiceId, message.SessionId);
                            if (clientSession == null)
                            {
                                LogService.Debug($"ServiceId:[{message.ServiceId}] SocketId:[{message.SessionId}] Session会话不存在");
                                return;
                            }
                            if (clientSession.Session == null)
                            {
                                LogService.Debug($"ServiceId:[{message.ServiceId}] SocketId:[{message.SessionId}] Session会话已经失效");
                                return;
                            }
                            try
                            {
                                clientSession.ProcessSessionPacket(message);
                            }
                            catch (Exception e)
                            {
                                LogService.Error(e.Message);
                            }
                        }
                    }
                }, _cts.Token);
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