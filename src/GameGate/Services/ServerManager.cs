using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

namespace GameGate
{
    public class ServerManager
    {
        private static readonly ServerManager instance = new ServerManager();

        public static ServerManager Instance
        {
            get { return instance; }
        }

        private readonly IList<ServerService> _serverServices;

        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private Channel<TMessageData> _reviceMsgList = null;

        public ServerManager()
        {
            _reviceMsgList = Channel.CreateUnbounded<TMessageData>();
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
        /// 客户端消息添加到队列给服务端处理
        /// GameGate -> GameSvr
        /// </summary>
        /// <param name="messageData"></param>
        public void SendQueue(TMessageData messageData)
        {
            _reviceMsgList.Writer.TryWrite(messageData);
        }

        /// <summary>
        /// 处理客户端发过来的消息
        /// </summary>
        public async Task ProcessReviceMessage()
        {
            while (await _reviceMsgList.Reader.WaitToReadAsync())
            {
                if (_reviceMsgList.Reader.TryRead(out var message))
                {
                    var clientSession = SessionManager.Instance.GetSession(message.MessageId);
                    clientSession?.HandleSessionPacket(message);
                }
            }
        }

        public IList<ServerService> GetServerList()
        {
            return _serverServices;
        }

        public ClientThread GetClientThread()
        {
            //TODO 根据配置文件有四种模式  默认随机
            //1.轮询分配
            //2.总是分配到最小资源 即网关在线人数最小的那个
            //3.一直分配到一个 直到当前玩家达到配置上线，则开始分配到其他可用网关
            //4.按权重分配
            if (_serverServices.Any())
            {
                var random = RandomNumber.GetInstance().Random(_serverServices.Count);
                return _serverServices[random].ClientThread;
            }
            return null;
        }
    }
}