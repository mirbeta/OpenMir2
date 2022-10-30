using LoginGate.Conf;
using LoginGate.Packet;
using System;
using System.Net;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace LoginGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-LoginGate)
    /// </summary>
    public class ServerService
    {
        private readonly MirLog _logger;
        private readonly SocketServer _serverSocket;
        private readonly SessionManager _sessionManager;
        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;
        private IPEndPoint _serverEndPoint;

        public ServerService(MirLog logger, ServerManager serverManager, ClientManager clientManager, SessionManager sessionManager)
        {
            _logger = logger;
            _serverManager = serverManager;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
            _serverSocket = new SocketServer(short.MaxValue, 1024);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
        }

        public IPEndPoint EndPoint => _serverEndPoint;
        
        public void Start(GameGateInfo gateInfo)
        {
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(gateInfo.GateAddress), gateInfo.GatePort);
            _serverSocket.Init();
            _serverSocket.Start(_serverEndPoint);
            _logger.LogInformation($"登录网关[{_serverEndPoint}]已启动...", 1);
        }

        public void Stop()
        {
            _serverSocket.Shutdown();
            _logger.LogInformation($"登录网关[{_serverEndPoint}]停止服务...", 1);
        }

        /// <summary>
        /// Mir2链接
        /// </summary>
        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddress = e.RemoteIPaddr;
            var clientThread = _clientManager.GetClientThread();
            if (clientThread == null)
            {
                _logger.LogDebug("获取服务器实例失败。");
                return;
            }
            if (!clientThread.ConnectState)
            {
                _logger.LogInformation("未就绪: " + sRemoteAddress, 5);
                _logger.LogDebug($"游戏引擎链接失败 Server:[{clientThread.EndPoint}] ConnectionId:[{e.ConnectionId}]");
                return;
            }
            _logger.LogDebug($"用户[{sRemoteAddress}]分配到数据库服务器[{clientThread.ClientId}] Server:{clientThread.EndPoint}");
            TSessionInfo sessionInfo = null;
            for (var nIdx = 0; nIdx < GateShare.MaxSession; nIdx++)
            {
                sessionInfo = clientThread.SessionArray[nIdx];
                if (sessionInfo == null)
                {
                    sessionInfo = new TSessionInfo();
                    sessionInfo.Socket = e.Socket;
                    sessionInfo.ConnectionId = e.SocHandle;
                    sessionInfo.ReceiveTick = HUtil32.GetTickCount();
                    sessionInfo.ClientIP = e.RemoteIPaddr;
                    clientThread.SessionArray[nIdx] = sessionInfo;
                    break;
                }
            }
            if (sessionInfo != null)
            {
                _logger.LogInformation("开始连接: " + sRemoteAddress, 5);
                _sessionManager.AddSession(sessionInfo, clientThread);
            }
            else
            {
                e.Socket.Close();
                _logger.LogInformation("禁止连接: " + sRemoteAddress, 1);
            }
        }

        /// <summary>
        /// 游戏客户端断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            var userSession = _sessionManager.GetSession(e.SocHandle);
            if (userSession != null)
            {
                userSession.UserLeave();
                userSession.CloseSession();
                _logger.LogInformation("断开连接: " + e.RemoteIPaddr, 5);
                _logger.LogDebug($"用户[{e.RemoteIPaddr}] 会话ID:[{e.SocHandle}] 断开链接.");
            }
            _sessionManager.CloseSession(e.SocHandle);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.LogInformation($"客户端链接错误.[{e.Exception.ErrorCode}]", 5);
        }

        /// <summary>
        /// 读取游戏客户端数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="token"></param>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var connectionId = token.SocHandle;
            var sRemoteAddress = token.RemoteIPaddr;
            var userSession = _sessionManager.GetSession(connectionId);
            if (userSession == null)
            {
                _logger.LogInformation("非法攻击: " + sRemoteAddress, 5);
                _logger.LogDebug($"获取用户对应会话失败 RemoteAddr:[{sRemoteAddress}] ConnectionId:[{connectionId}]");
                return;
            }
            if (!userSession.ClientThread.ConnectState)
            {
                _logger.LogInformation("未就绪: " + sRemoteAddress, 5);
                _logger.LogDebug($"账号服务器链接失败 Server:[{userSession.ClientThread.EndPoint}] ConnectionId:[{connectionId}]");
                return;
            }
            var data = new byte[token.BytesReceived];
            Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
            var message = new MessageData();
            message.Body = data;
            message.ConnectionId = connectionId;
            _serverManager.SendQueue(message);
        }
    }
}