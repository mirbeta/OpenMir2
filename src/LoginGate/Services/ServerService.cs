using LoginGate.Conf;
using LoginGate.Packet;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using SystemModule;
using TouchSocket.Sockets;

namespace LoginGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-LoginGate)
    /// </summary>
    public class ServerService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpService _serverSocket;
        private readonly SessionManager _sessionManager;
        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;

        public ServerService(ServerManager serverManager, ClientManager clientManager, SessionManager sessionManager)
        {
            _serverManager = serverManager;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
            _serverSocket = new TcpService();
            _serverSocket.Connected += ServerSocketClientConnect;
            _serverSocket.Disconnected += ServerSocketClientDisconnect;
            _serverSocket.Received += ServerSocketClientRead;
        }

        public void Start(GameGateInfo gateInfo)
        {
            _serverSocket.Setup(new TouchSocket.Core.TouchSocketConfig().SetListenIPHosts(new IPHost(IPAddress.Parse(gateInfo.GateAddress), gateInfo.GatePort)));
            _serverSocket.Start();
            _logger.Info($"登录网关[{_serverSocket.ServerName}]已启动...");
        }

        public void Stop()
        {
            _serverSocket.Stop();
            _logger.Info($"登录网关[{_serverSocket.ServerName}]停止服务...");
        }

        public string GetEndPoint()
        { 
            return _serverSocket.ServerName;
        }

        /// <summary>
        /// Mir2链接
        /// </summary>
        private Task ServerSocketClientConnect(ITcpClientBase client, ConnectedEventArgs e)
        {
            var sRemoteAddress = client.GetIPPort();
            var clientThread = _clientManager.GetClientThread();
            if (clientThread == null)
            {
                _logger.Debug("获取登陆服务失败。");
                return Task.CompletedTask;
            }
            if (!clientThread.ConnectState)
            {
                _logger.Info("未就绪: " + sRemoteAddress);
                _logger.Debug($"游戏引擎链接失败 Server:[{clientThread.EndPoint}] Ip:[{client.IP}]");
                return Task.CompletedTask;
            }
            _logger.Debug($"用户[{sRemoteAddress}]分配到数据库服务器[{clientThread.ClientId}] Server:{clientThread.EndPoint}");
            TSessionInfo sessionInfo = null;
            for (var nIdx = 0; nIdx < GateShare.MaxSession; nIdx++)
            {
                sessionInfo = clientThread.SessionArray[nIdx];
                if (sessionInfo == null)
                {
                    sessionInfo = new TSessionInfo();
                    sessionInfo.Socket = client.MainSocket;
                    sessionInfo.ConnectionId = client.MainSocket.Handle.ToInt32();
                    sessionInfo.ReceiveTick = HUtil32.GetTickCount();
                    sessionInfo.ClientIP = client.IP;
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
                client.Close();
                _logger.Info("禁止连接: " + sRemoteAddress);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 游戏客户端断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Task ServerSocketClientDisconnect(ITcpClientBase client, DisconnectEventArgs e)
        {
            var userSession = _sessionManager.GetSession(client.MainSocket.Handle.ToInt32());
            if (userSession != null)
            {
                userSession.UserLeave();
                userSession.CloseSession();
                _logger.Info("断开连接: " + client.IP);
                _logger.Debug($"用户[{client.IP}] 会话ID:[{client.MainSocket.Handle.ToInt32()}] 断开链接.");
            }
            _sessionManager.CloseSession(client.MainSocket.Handle.ToInt32());
            return Task.CompletedTask;
        }

        /// <summary>
        /// 读取游戏客户端数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="token"></param>
        private Task ServerSocketClientRead(SocketClient client, ReceivedDataEventArgs e)
        {
            var data = new byte[e.ByteBlock.Len];
            Buffer.BlockCopy(e.ByteBlock.Buffer, 0, data, 0, data.Length);
            var message = new MessageData();
            message.ClientIP = client.IP;
            message.Body = data;
            message.ConnectionId = client.MainSocket.Handle.ToInt32();
            _serverManager.SendQueue(message);
            return Task.CompletedTask;
        }
    }
}