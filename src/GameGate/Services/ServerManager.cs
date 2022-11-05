using GameGate.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

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
        private static GateConfig Config => ConfigManager.Instance.GateConfig;
        /// <summary>
        /// 服务器列表
        /// </summary>
        private ServerService[] _serverServices;
        /// <summary>
        /// 消息消费者线程
        /// </summary>
        private ServerMessageWorkThread[] _messageWorkThreads;
        private int _lastMessageThreadCount;
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private readonly Channel<ClientMessagePacket> _clientPacketQueue;

        private ServerManager()
        {
            _clientPacketQueue = Channel.CreateUnbounded<ClientMessagePacket>();
        }

        public void AddServer(ServerService[] serverService)
        {
            _serverServices = serverService;
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
        public void SendClientQueue(string connectionId, int threadId, Span<byte> buffer)
        {
            _serverServices[threadId].Send(connectionId, buffer);
        }

        /// <summary>
        /// 客户端消息添加到队列给服务端处理
        /// GameGate -> GameSvr
        /// </summary>
        /// <param name="messagePacket"></param>
        public void ClientPacketQueue(ClientMessagePacket messagePacket)
        {
            _clientPacketQueue.Writer.TryWrite(messagePacket);
        }

        public static int MessageWorkThreads => Config.MessageWorkThread;

        /// <summary>
        /// 开启客户端消息消费线程
        /// </summary>
        public void StartProcessMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(() =>
            {
                if (_lastMessageThreadCount == Config.MessageWorkThread)
                {
                    return;
                }
                if (Config.MessageWorkThread > _lastMessageThreadCount)
                {
                    Array.Resize(ref _messageWorkThreads, Config.MessageWorkThread);
                    for (var i = 0; i < Config.MessageWorkThread; i++)
                    {
                        if (_messageWorkThreads[i] == null)
                        {
                            _messageWorkThreads[i] = new ServerMessageWorkThread(stoppingToken, _clientPacketQueue.Reader);
                        }
                        if (_messageWorkThreads[i].ThreadState == MessageThreadState.Stop)
                        {
                            _messageWorkThreads[i]?.Start(stoppingToken);
                        }
                    }
                }
                else
                {
                    for (var i = _messageWorkThreads.Length - 1; i >= Config.MessageWorkThread; i--)
                    {
                        if (_messageWorkThreads[i] == null)
                        {
                            continue;
                        }
                        if (_messageWorkThreads[i].ThreadState == MessageThreadState.Runing)
                        {
                            _messageWorkThreads[i]?.Stop();
                        }
                    }
                }
                _lastMessageThreadCount = Config.MessageWorkThread;
            }, stoppingToken);
        }

        public IList<ServerService> GetServerList()
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

        private class ServerMessageWorkThread
        {
            private readonly ManualResetEvent _resetEvent;
            private string _threadId;
            private readonly CancellationTokenSource _cts;
            /// <summary>
            /// 接收封包（客户端-》网关）
            /// </summary>
            private readonly ChannelReader<ClientMessagePacket> _reviceMsgQueue = null;
            public MessageThreadState ThreadState;
            private static SessionManager Session => SessionManager.Instance;
            private static MirLog LogQueue => MirLog.Instance;

            public ServerMessageWorkThread(CancellationToken stoppingToken, ChannelReader<ClientMessagePacket> channel)
            {
                _reviceMsgQueue = channel;
                _resetEvent = new ManualResetEvent(true);
                ThreadState = MessageThreadState.Stop;
                _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            }

            public void Start(CancellationToken stoppingToken)
            {
                Task.Factory.StartNew(async () =>
                {
                    _threadId = Guid.NewGuid().ToString("N");
                    ThreadState = MessageThreadState.Runing;
                    LogQueue.DebugLog($"消息消费线程[{_threadId}]已启动.");
                    while (await _reviceMsgQueue.WaitToReadAsync(_cts.Token))
                    {
                        _resetEvent.WaitOne();
                        if (_reviceMsgQueue.TryRead(out var message))
                        {
                            var clientSession = Session.GetSession(message.SessionId);
                            clientSession?.ProcessClientPacket(message);
                        }
                    }
                }, stoppingToken);
            }

            public void Stop()
            {
                ThreadState = MessageThreadState.Stop;
                _resetEvent.Reset();//暂停
                _cts.CancelAfter(3000);//延时3秒取消消费，防止消息丢失
                LogQueue.DebugLog($"消息消费线程[{_threadId}]已停止.");
            }
        }
    }
}