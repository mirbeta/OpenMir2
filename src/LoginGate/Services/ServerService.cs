using LoginGate.Conf;
using LoginGate.Packet;
using NLog;
using System;
using System.Net;
using SystemModule;
using SystemModule.SocketComponents;
using SystemModule.SocketComponents.AsyncSocketServer;
using SystemModule.Sockets;

namespace LoginGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-LoginGate)
    /// </summary>
    public class ServerService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SocketServer _serverSocket;
        private readonly SessionManager _sessionManager;
        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;
        private IPEndPoint _serverEndPoint;

        public ServerService(ServerManager serverManager, ClientManager clientManager, SessionManager sessionManager)
        {
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
            _logger.Info($"登录网关[{_serverEndPoint}]已启动...");
        }

        public void Stop()
        {
            _serverSocket.Shutdown();
            _logger.Info($"登录网关[{_serverEndPoint}]停止服务...");
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
                _logger.Debug("获取登陆服务失败。");
                return;
            }
            if (!clientThread.ConnectState)
            {
                _logger.Info("未就绪: " + sRemoteAddress);
                _logger.Debug($"游戏引擎链接失败 Server:[{clientThread.EndPoint}] ConnectionId:[{e.ConnectionId}]");
                return;
            }
            _logger.Debug($"用户[{sRemoteAddress}]分配到数据库服务器[{clientThread.ClientId}] Server:{clientThread.EndPoint}");
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
                _logger.Info("开始连接: " + sRemoteAddress);
                _sessionManager.AddSession(sessionInfo, clientThread);
            }
            else
            {
                e.Socket.Close();
                _logger.Info("禁止连接: " + sRemoteAddress);
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
                _logger.Info("断开连接: " + e.RemoteIPaddr);
                _logger.Debug($"用户[{e.RemoteIPaddr}] 会话ID:[{e.SocHandle}] 断开链接.");
            }
            _sessionManager.CloseSession(e.SocHandle);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.Error($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 读取游戏客户端数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="token"></param>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var data = new byte[token.BytesReceived];
            Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
            var message = new MessageData();
            message.ClientIP = token.RemoteIPaddr;
            message.Body = data;
            message.ConnectionId = token.SocHandle;
            _serverManager.SendQueue(message);
        }
    }
}