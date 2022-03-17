using LoginGate.Conf;
using LoginGate.Package;
using System;
using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Sockets;

namespace LoginGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-LoginGate)
    /// </summary>
    public class ServerService
    {
        private readonly LogQueue _logQueue;
        private readonly ISocketServer _serverSocket;
        private readonly SessionManager _sessionManager;
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private Channel<TMessageData> _snedQueue = null;
        private readonly ClientManager _clientManager;
        private readonly ConfigManager _configManager;

        public ServerService(LogQueue logQueue, ConfigManager configManager, SessionManager sessionManager, ClientManager clientManager)
        {
            _snedQueue = Channel.CreateUnbounded<TMessageData>();
            _sessionManager = sessionManager;
            _clientManager = clientManager;
            _configManager = configManager;
            _logQueue = logQueue;
            _serverSocket = new ISocketServer(ushort.MaxValue, 1024);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
        }

        private ChannelWriter<TMessageData> SendQueue => _snedQueue.Writer;

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
            while (await _snedQueue.Reader.WaitToReadAsync())
            {
                if (_snedQueue.Reader.TryRead(out var message))
                {
                    var clientSession = _sessionManager.GetSession(message.SessionId);
                    clientSession?.HandleUserPacket(message);
                }
            }
        }

        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var clientThread = _clientManager.GetClientThread();
            if (clientThread == null)
            {
                _logQueue.EnqueueDebugging("获取服务器实例失败。");
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            _logQueue.EnqueueDebugging($"用户[{sRemoteAddress}]分配到账号服务器[{clientThread.ClientId}] Server:{clientThread.GetSocketIp()}");
            TSessionInfo sessionInfo = null;
            for (var nIdx = 0; nIdx < clientThread.MaxSession; nIdx++)
            {
                sessionInfo = clientThread.SessionArray[nIdx];
                if (sessionInfo == null)
                {
                    continue;
                }
                if (sessionInfo.Socket == null)
                {
                    sessionInfo.Socket = e.Socket;
                    sessionInfo.SocketId = e.ConnectionId;
                    sessionInfo.dwReceiveTick = HUtil32.GetTickCount();
                    sessionInfo.ClientIP = e.RemoteIPaddr;
                    break;
                }
            }
            if (sessionInfo != null)
            {
                _logQueue.Enqueue("开始连接: " + sRemoteAddress, 5);
                _clientManager.AddClientThread(e.ConnectionId, clientThread);//链接成功后建立对应关系
                var userSession = new ClientSession(_configManager, sessionInfo, clientThread);
                userSession.UserEnter();
                _sessionManager.AddSession(sessionInfo.SocketId, userSession);
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
                    var clientSession = _sessionManager.GetSession(e.ConnectionId);
                    if (clientSession != null)
                    {
                        clientSession.UserLeave();
                    }
                    _logQueue.Enqueue("断开连接: " + sRemoteAddr, 5);
                }
            }
            else
            {
                _logQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
                _logQueue.EnqueueDebugging($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{e.ConnectionId}]");
            }
            _clientManager.DeleteClientThread(e.ConnectionId);
            _sessionManager.Remove(e.ConnectionId);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logQueue.Enqueue($"客户端链接错误.[{e.Exception.ErrorCode}]", 5);
        }

        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var connectionId = token.ConnectionId;
            var userClient = _clientManager.GetClientThread(connectionId);
            var sRemoteAddress = token.RemoteIPaddr;
            if (userClient == null)
            {
                _logQueue.Enqueue("非法攻击: " + sRemoteAddress, 5);
                _logQueue.EnqueueDebugging($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddress}] ConnectionId:[{connectionId}]");
                return;
            }
            if (!userClient.boGateReady)
            {
                _logQueue.Enqueue("未就绪: " + sRemoteAddress, 5);
                _logQueue.EnqueueDebugging($"账号服务器链接失败 Server:[{userClient.GetSocketIp()}] ConnectionId:[{connectionId}]");
                return;
            }
            var data = new byte[token.BytesReceived];
            Array.Copy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
            var userData = new TMessageData();
            userData.Body = data;
            userData.SessionId = connectionId;
            SendQueue.TryWrite(userData);
        }
    }
}