using LoginGate.Conf;
using System;
using SystemModule;
using SystemModule.Sockets;

namespace LoginGate
{
    /// <summary>
    /// 客户端服务端(Mir2-LoginGate)
    /// </summary>
    public class ServerService
    {
        private LogQueue _logQueue => LogQueue.Instance;
        private readonly ISocketServer _serverSocket;
        private readonly string GateAddress;
        private readonly int GatePort = 0;
        private readonly ClientThread _clientThread;
        private SessionManager _sessionManager => SessionManager.Instance;
        private ClientManager _clientManager => ClientManager.Instance;
        private ServerManager _serverManager => ServerManager.Instance;
        private readonly ConfigManager _configManager = ConfigManager.Instance;

        public ServerService(int i, GameGateInfo gameGate)
        {
            _clientThread = new ClientThread(i, gameGate);
            GateAddress = gameGate.sServerAdress;
            GatePort = gameGate.nGatePort;
            _serverSocket = new ISocketServer(ushort.MaxValue, 2048);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
        }

        public ClientThread ClientThread => _clientThread;

        public void Start()
        {
            _serverSocket.Start(GateAddress, GatePort);
            _clientThread.Start();
            _clientThread.RestSessionArray();
            _logQueue.Enqueue($"登录网关[{GateAddress}:{GatePort}]已启动...", 1);
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Shutdown();
            _logQueue.Enqueue($"登录网关[{GateAddress}:{GatePort}]停止服务...", 1);
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
                    sessionInfo = new TSessionInfo();
                    sessionInfo.Socket = e.Socket;
                    sessionInfo.SocketId = e.ConnectionId;
                    sessionInfo.dwReceiveTick = HUtil32.GetTickCount();
                    sessionInfo.ClientIP = e.RemoteIPaddr;
                    clientThread.SessionArray[nIdx] = sessionInfo;
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
            var sRemoteAddr = e.RemoteIPaddr;
            var nSockIndex = e.ConnectionId;
            var clientThread = _clientManager.GetClientThread(nSockIndex);
            if (clientThread != null && clientThread.boGateReady)
            {
                var userSession = _sessionManager.GetSession(nSockIndex);
                if (userSession != null)
                {
                    var clientSession = _sessionManager.GetSession(e.ConnectionId);
                    clientSession?.UserLeave();
                    clientSession?.CloseSession();
                    _logQueue.Enqueue("断开连接: " + sRemoteAddr, 5);
                }
                _sessionManager.CloseSession(nSockIndex);
            }
            else
            {
                _logQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
                _logQueue.EnqueueDebugging($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{e.ConnectionId}]");
            }
            _clientManager.DeleteClientThread(e.ConnectionId);
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
            Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
            var message = new TMessageData();
            message.Body = data;
            message.MessageId = connectionId;
            _serverManager.SendQueue(message);
        }
    }
}