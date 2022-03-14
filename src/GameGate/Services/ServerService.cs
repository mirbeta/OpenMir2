using System;
using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Sockets;

namespace GameGate
{
    /// <summary>
    /// 客户端服务端(Mir2-GameGate)
    /// </summary>
    public class ServerService
    {
        private readonly ISocketServer _serverSocket;
        private readonly SessionManager _sessionManager;
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private Channel<TMessageData> _reviceMsgList = null;
        private readonly ClientManager _clientManager;
        private readonly ConfigManager _configManager;
        private readonly LogQueue _logQueue;
        
        public ServerService(LogQueue logQueue, ConfigManager configManager, SessionManager sessionManager, ClientManager clientManager)
        {
            _serverSocket = new ISocketServer(ushort.MaxValue, 1024);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
            _reviceMsgList = Channel.CreateUnbounded<TMessageData>();
            _sessionManager = sessionManager;
            _clientManager = clientManager;
            _configManager = configManager;
            _logQueue = logQueue;
        }

        public void Start()
        {
            _serverSocket.Start(GateShare.GateAddr, GateShare.GatePort);
        }

        public void Stop()
        {
            _serverSocket.Shutdown();
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
                    var clientSession = _sessionManager.GetSession(message.MessageId);
                    clientSession?.HandleUserPacket(message);
                }
            }
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var clientThread = _clientManager.GetClientThread();
            if (clientThread == null)
            {
                _logQueue.EnqueueDebugging("获取服务器实例失败。");
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            _logQueue.EnqueueDebugging($"用户[{sRemoteAddress}]分配到游戏数据服务器[{clientThread.ClientId}] Server:{clientThread.GetSocketIp()}");
            TSessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.MaxSession; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession.Socket == null)
                {
                    userSession.Socket = e.Socket;
                    userSession.nUserListIndex = 0;
                    userSession.SessionId = e.ConnectionId;
                    userSession.dwReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = e.SocHandle;
                    break;
                }
            }
            if (userSession != null)
            {
                _logQueue.Enqueue("开始连接: " + sRemoteAddress, 5);
                clientThread.UserEnter(userSession.SessionId, userSession.SckHandle, sRemoteAddress); //通知M2有新玩家进入游戏
                _clientManager.AddClientThread(e.ConnectionId, clientThread);
                _sessionManager.AddSession(userSession.SessionId, new ClientSession(_logQueue, _configManager, userSession, clientThread));
            }
            else
            {
                e.Socket.Close();
                _logQueue.Enqueue("禁止连接: " + sRemoteAddress, 1);
            }
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            TSessionInfo userSession;
            var sRemoteAddr = e.RemoteIPaddr;
            var nSockIndex = e.ConnectionId;
            var clientThread = _clientManager.GetClientThread(nSockIndex);
            if (clientThread != null && clientThread.boGateReady)
            {
                if (nSockIndex >= 0 && nSockIndex < clientThread.MaxSession)
                {
                    userSession = clientThread.SessionArray[nSockIndex];
                    userSession.Socket = null;
                    userSession.SckHandle = -1;
                    clientThread.UserLeave(e.SocHandle); //发送消息给M2断开链接
                    _logQueue.Enqueue("断开连接: " + sRemoteAddr, 5);
                }
            }
            else
            {
                _logQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
                _logQueue.EnqueueDebugging($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{nSockIndex}]");
            }
            _clientManager.DeleteClientThread(nSockIndex);
            _sessionManager.Remove(nSockIndex);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logQueue.EnqueueDebugging($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="token"></param>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {            
            var connectionId = token.ConnectionId;
            var userClient = _clientManager.GetClientThread(connectionId);
            var sRemoteAddress = token.RemoteIPaddr;
            if (userClient == null)
            {
                _logQueue.Enqueue("非法攻击: " + sRemoteAddress, 5);
                Debug.WriteLine($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddress}] ConnectionId:[{connectionId}]");
                return;
            }
            if (!userClient.boGateReady)
            {
                _logQueue.Enqueue("未就绪: " + sRemoteAddress, 5);
                _logQueue.EnqueueDebugging($"游戏引擎链接失败 Server:[{userClient.GetSocketIp()}] ConnectionId:[{connectionId}]");
                return;
            }
            var clientSession = _sessionManager.GetSession(connectionId);
            if (clientSession != null)
            {
                if (!clientSession.Session.Socket.Connected)
                {
                    return;
                }
                var data = new byte[token.BytesReceived];
                Array.Copy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
                var message = new TMessageData();
                message.Buffer = data;
                message.MessageId = connectionId;
                message.DataLen = data.Length;
                _reviceMsgList.Writer.TryWrite(message);
            }
            else
            {
                _logQueue.Enqueue("非法攻击: " + token.RemoteIPaddr, 5);
            }
        }
    }
}