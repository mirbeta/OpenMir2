using SelGate.Conf;
using SelGate.Package;
using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace SelGate.Services
{
    /// <summary>
    /// 客户端服务端(SystemModule-SelGate)
    /// </summary>
    public class ServerService
    {
        private readonly MirLog _logger;
        private readonly SocketServer _serverSocket;
        private readonly SessionManager _sessionManager;
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private readonly Channel<TMessageData> _sendQueue;
        private readonly ClientManager _clientManager;
        private readonly ConfigManager _configManager;

        public ServerService(MirLog mirLog, SessionManager sessionManager, ClientManager clientManager,ConfigManager configManager)
        {
            _logger = mirLog;
            _sessionManager = sessionManager;
            _clientManager = clientManager;
            _configManager = configManager;
            _sendQueue = Channel.CreateUnbounded<TMessageData>();
            _serverSocket = new SocketServer(ushort.MaxValue, 512);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
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
            while (await _sendQueue.Reader.WaitToReadAsync())
            {
                while (_sendQueue.Reader.TryRead(out var message))
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
                _logger.LogInformation("获取服务器实例失败。", 5);
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            _logger.LogDebug($"用户[{sRemoteAddress}]分配到数据库服务器[{clientThread.ClientId}] Server:{clientThread.GetEndPoint()}");
            TSessionInfo sessionInfo = null;
            for (var nIdx = 0; nIdx < ClientThread.MaxSession; nIdx++)
            {
                sessionInfo = clientThread.SessionArray[nIdx];
                if (sessionInfo == null)
                {
                    sessionInfo = new TSessionInfo();
                    sessionInfo.Socket = e.Socket;
                    sessionInfo.SocketId = e.ConnectionId;
                    sessionInfo.dwReceiveTick = HUtil32.GetTickCount();
                    sessionInfo.ClientIP = e.RemoteIPaddr;
                    break;
                }
            }
            if (sessionInfo != null)
            {
                _logger.LogInformation("开始连接: " + sRemoteAddress, 5);
                _clientManager.AddClientThread(e.ConnectionId, clientThread);//链接成功后建立对应关系
                var userSession = new ClientSession(_logger, _configManager, sessionInfo, clientThread);
                userSession.UserEnter();
                _sessionManager.AddSession(sessionInfo.SocketId, userSession);
            }
            else
            {
                e.Socket.Close();
                _logger.LogInformation("禁止连接: " + sRemoteAddress, 1);
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
                    _logger.LogInformation("断开连接: " + sRemoteAddr, 5);
                }
                _sessionManager.CloseSession(nSockIndex);
            }
            else
            {
                _logger.LogInformation("断开链接: " + sRemoteAddr, 5);
                _logger.LogDebug($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{e.ConnectionId}]");
            }
            _clientManager.DeleteClientThread(e.ConnectionId);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.LogInformation($"客户端链接错误.[{e.Exception.ErrorCode}]", 5);
        }

        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var connectionId = token.ConnectionId;
            var userClient = _clientManager.GetClientThread(connectionId);
            var sRemoteAddress = token.RemoteIPaddr;
            if (userClient == null)
            {
                _logger.LogInformation("非法攻击: " + sRemoteAddress, 5);
                _logger.LogDebug($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddress}] ConnectionId:[{connectionId}]");
                return;
            }
            if (!userClient.boGateReady)
            {
                _logger.LogInformation("未就绪: " + sRemoteAddress, 5);
                _logger.LogDebug($"游戏引擎链接失败 Server:[{userClient.GetEndPoint()}] ConnectionId:[{connectionId}]");
                return;
            }
            var data = new byte[token.BytesReceived];
            Array.Copy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
            var userData = new TMessageData();
            userData.Body = data;
            userData.SessionId = connectionId;
            _sendQueue.Writer.TryWrite(userData);
        }
    }
}