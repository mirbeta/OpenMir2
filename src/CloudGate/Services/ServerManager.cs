using CloudGate.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

namespace CloudGate.Services
{
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
        private readonly IList<ServerService> _serverServices;
        /// <summary>
        /// 消息消费者
        /// </summary>
        private MessageThreadConsume[] _messageThreads;
        private int _lastMessageThreadCount;
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private readonly Channel<TMessageData> _reviceQueue = null;

        public ServerManager()
        {
            _reviceQueue = Channel.CreateUnbounded<TMessageData>();
            _serverServices = new List<ServerService>();
        }

        public void AddServer(ServerService serverService)
        {
            _serverServices.Add(serverService);
        }

        public void RemoveServer(ServerService serverService)
        {
            _serverServices.Remove(serverService);
        }

        public void Start()
        {
            _messageThreads = new MessageThreadConsume[4];
            for (int i = 0; i < Config.MessageThread; i++)
            {
                _messageThreads[i] = new MessageThreadConsume(_reviceQueue.Reader);
            }

            var serverQueueTask = new Task[_serverServices.Count];
            for (var i = 0; i < _serverServices.Count; i++)
            {
                if (_serverServices[i] == null)
                {
                    continue;
                }
                serverQueueTask[i] = _serverServices[i].Start();
            }

            Task.WhenAll(serverQueueTask);
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

        /// <summary>
        /// 添加到游戏网关消息队列
        /// </summary>
        /// <param name="messageData"></param>
        public void SendQueue(TMessageData messageData)
        {
            _reviceQueue.Writer.TryWrite(messageData);
        }

        public int MessageThreadCount => Config.MessageThread;

        /// <summary>
        /// 开启游戏网关消息消费线程
        /// </summary>
        public void StartProcessMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(() =>
            {
                if (_lastMessageThreadCount == Config.MessageThread)
                {
                    return;
                }
                switch (Config.MessageThread)
                {
                    case 1:
                        if (_messageThreads[0].ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[0]?.Start();
                        }
                        if (_messageThreads[1]?.ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[1]?.Stop();
                        }
                        if (_messageThreads[2]?.ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[2]?.Stop();
                        }
                        if (_messageThreads[3]?.ThreadState == MessageThreadState.Runing)
                        {
                            _messageThreads[3]?.Stop();
                        }
                        break;
                    case 2:
                        if (_messageThreads[0]?.ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[0]?.Start();
                        }
                        if (_messageThreads[1]?.ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[1]?.Start();
                        }
                        if (_messageThreads[2]?.ThreadState == MessageThreadState.Runing)
                        {
                            _messageThreads[2]?.Stop();
                        }
                        if (_messageThreads[3]?.ThreadState == MessageThreadState.Runing)
                        {
                            _messageThreads[3]?.Stop();
                        }
                        break;
                    case 3:
                        if (_messageThreads[0]?.ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[0]?.Start();
                        }
                        if (_messageThreads[1].ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[1]?.Start();
                        }
                        if (_messageThreads[2].ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[2]?.Start();
                        }
                        if (_messageThreads[2]?.ThreadState == MessageThreadState.Runing)
                        {
                            _messageThreads[3]?.Stop();
                        }
                        break;
                    case 4:
                        if (_messageThreads[0]?.ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[0]?.Start();
                        }
                        if (_messageThreads[1].ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[1]?.Start();
                        }
                        if (_messageThreads[2].ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[2]?.Start();
                        }
                        if (_messageThreads[3]?.ThreadState == MessageThreadState.Stop)
                        {
                            _messageThreads[3]?.Start();
                        }
                        break;
                }
                _lastMessageThreadCount = Config.MessageThread;

            }, stoppingToken);
        }

        public IList<ServerService> GetServerList()
        {
            return _serverServices;
        }

        public ClientThread GetClientThread()
        {
            return null;
        }

        private enum MessageThreadState
        {
            Runing,
            Stop
        }
        
        private class MessageThreadConsume
        {
            private readonly ManualResetEvent _resetEvent;
            private Task _messageThreads;
            private readonly string _threadId;
            private SessionManager Session => SessionManager.Instance;
            private readonly CancellationTokenSource _cts = new CancellationTokenSource();
            private static MirLog LogQueue => MirLog.Instance;
            /// <summary>
            /// 接收封包（客户端-》网关）
            /// </summary>
            private readonly ChannelReader<TMessageData> _reviceMsgList = null;
            public MessageThreadState ThreadState;

            public MessageThreadConsume(ChannelReader<TMessageData> channel)
            {
                _reviceMsgList = channel;
                _resetEvent = new ManualResetEvent(true);
                _threadId = Guid.NewGuid().ToString("N");
                ThreadState = MessageThreadState.Stop;
            }

            public void Start()
            {
                _messageThreads = Task.Factory.StartNew(async () =>
                {
                    ThreadState = MessageThreadState.Runing;
                    LogQueue.EnqueueDebugging($"消息消费线程[{_threadId}]已启动.");
                    while (await _reviceMsgList.WaitToReadAsync(_cts.Token))
                    {
                        _resetEvent.WaitOne();
                        if (_reviceMsgList.TryRead(out var message))
                        {
                            var clientSession = Session.GetSession(message.MessageId);
                            clientSession?.HandleSessionPacket(message);
                        }
                    }
                }, _cts.Token);
            }

            public void Stop()
            {
                ThreadState = MessageThreadState.Stop;
                _resetEvent.Reset();//暂停
                _cts.CancelAfter(3000);//延时3秒取消消费，防止消息丢失
                _messageThreads.Dispose();
                LogQueue.EnqueueDebugging($"消息消费线程[{_threadId}]已停止.");
            }
        }
    }
}