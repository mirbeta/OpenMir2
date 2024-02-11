using NLog;
using SelGate.Conf;
using System;
using System.Net;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SelGate.Datas;
using SystemModule;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace SelGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-SelGate)
    /// </summary>
    public class ServerService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpService _serverSocket;
        private readonly SessionManager _sessionManager;
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private readonly Channel<MessageData> _sendQueue;
        private readonly ClientManager _clientManager;
        private readonly ConfigManager _configManager;

        public ServerService(SessionManager sessionManager, ClientManager clientManager, ConfigManager configManager)
        {
            _sessionManager = sessionManager;
            _clientManager = clientManager;
            _configManager = configManager;
            _sendQueue = Channel.CreateUnbounded<MessageData>();
            _serverSocket = new TcpService();
            _serverSocket.Connecting += ServerSocketClientConnect;
            _serverSocket.Disconnected += ServerSocketClientDisconnect;
            _serverSocket.Received += ServerSocketClientRead;
        }

        public void Start()
        {
            _serverSocket.Setup(new TouchSocketConfig().SetRemoteIPHost(new IPHost(IPAddress.Any,GateShare.GatePort)));
            _serverSocket.Start();
        }

        public void Stop()
        {
            _serverSocket.Stop();
        }

        /// <summary>
        /// 处理客户端发过来的消息
        /// </summary>
        public void ProcessReviceMessage(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _sendQueue.Reader.WaitToReadAsync(stoppingToken))
                {
                    while (_sendQueue.Reader.TryRead(out var message))
                    {
                        var clientSession = _sessionManager.GetSession(message.SessionId);
                        clientSession?.HandleUserPacket(message);
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void SendMessage(string connectionId, byte[] data)
        {
            _serverSocket.Send(connectionId, data);
        }

        public void SendMessage(string connectionId, byte[] data, int len)
        {
            _serverSocket.Send(connectionId, data, 0, len);
        }

        public void CloseClient(string connectionId)
        {
            if (_serverSocket.TryGetSocketClient(connectionId, out var client))
            {
                client.Close();
            }
        }

        private Task ServerSocketClientConnect(ITcpClientBase client, ConnectingEventArgs e)
        {
            var clientThread = _clientManager.GetClientThread();
            if (clientThread == null)
            {
                _logger.Info("获取服务器实例失败。");
                return Task.CompletedTask;
            }
            var sRemoteAddress = client.MainSocket.RemoteEndPoint.GetIP();
            _logger.Trace($"用户[{sRemoteAddress}]分配到数据库服务器[{clientThread.ClientId}] Server:{clientThread.GetEndPoint()}");
            SessionInfo sessionInfo = null;
            for (var nIdx = 0; nIdx < ClientThread.MaxSession; nIdx++)
            {
                sessionInfo = clientThread.SessionArray[nIdx];
                if (sessionInfo == null)
                {
                    sessionInfo = new SessionInfo();
                    sessionInfo.SocketId = ((SocketClient)client).Id;
                    sessionInfo.dwReceiveTick = HUtil32.GetTickCount();
                    sessionInfo.ClientIP = sRemoteAddress;
                    break;
                }
            }
            if (sessionInfo != null)
            {
                _logger.Info("开始连接: " + sRemoteAddress);
                _clientManager.AddClientThread(sessionInfo.SocketId, clientThread);//链接成功后建立对应关系
                var userSession = new ClientSession(_configManager, sessionInfo, clientThread,GateShare.ServiceProvider.GetService<ServerService>());
                userSession.UserEnter();
                _sessionManager.AddSession(sessionInfo.SocketId, userSession);
            }
            else
            {
                e.Socket.Close();
                _logger.Info("禁止连接: " + sRemoteAddress);
            }
            return Task.CompletedTask;
        }

        private Task ServerSocketClientDisconnect(IClient client, DisconnectEventArgs e)
        {
            var clientSoc = client as SocketClient;
            var nSockIndex = clientSoc.Id;
            var sRemoteAddr = clientSoc.MainSocket.RemoteEndPoint.GetIP();
            var clientThread = _clientManager.GetClientThread(nSockIndex);
            if (clientThread != null && clientThread.boGateReady)
            {
                var userSession = _sessionManager.GetSession(nSockIndex);
                if (userSession != null)
                {
                    userSession.UserLeave();
                    userSession.CloseSession();
                    _logger.Info("断开连接: " + sRemoteAddr);
                }
                _sessionManager.CloseSession(nSockIndex);
            }
            else
            {
                _logger.Info("断开链接: " + sRemoteAddr);
                _logger.Debug($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{clientSoc.Id}]");
            }
            _clientManager.DeleteClientThread(nSockIndex);
            return Task.CompletedTask;
        }

        private Task ServerSocketClientRead(IClient client, ReceivedDataEventArgs e)
        {
            var clientSoc = client as SocketClient;
            var connectionId = clientSoc.MainSocket.Handle.ToInt32();
            var sRemoteAddress = clientSoc.MainSocket.RemoteEndPoint.GetIP();
            var userClient = _clientManager.GetClientThread(clientSoc.Id);
            if (userClient == null)
            {
                _logger.Info("非法攻击: " + sRemoteAddress);
                _logger.Debug($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddress}] ConnectionId:[{connectionId}]");
                return Task.CompletedTask;
            }
            if (!userClient.boGateReady)
            {
                _logger.Info("未就绪: " + sRemoteAddress);
                _logger.Debug($"游戏引擎链接失败 Server:[{userClient.GetEndPoint()}] ConnectionId:[{connectionId}]");
                return Task.CompletedTask;
            }
            var data = new byte[e.ByteBlock.Len];
            Array.Copy(e.ByteBlock.Buffer, 0, data, 0, data.Length);
            var userData = new MessageData();
            userData.Body = data;
            userData.SessionId = clientSoc.Id;
            _sendQueue.Writer.TryWrite(userData);
            return Task.CompletedTask;
        }
    }
}