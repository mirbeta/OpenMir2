using System;
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
        private LogQueue _logQueue => LogQueue.Instance;
        private SessionManager _sessionManager => SessionManager.Instance;
        private ClientManager _clientManager => ClientManager.Instance;
        private ServerManager _serverManager => ServerManager.Instance;
        private readonly ISocketServer _serverSocket;
        private readonly ClientThread _clientThread;
        private readonly string _gateAddress;
        private readonly int _gatePort = 0;
        private readonly SendQueue _sendQueue;

        public ServerService(int i, GameGateInfo gameGate)
        {
            _sendQueue = new SendQueue();
            _clientThread = new ClientThread(i, gameGate);
            _gateAddress = gameGate.sServerAdress;
            _gatePort = gameGate.nGatePort;
            _serverSocket = new ISocketServer(ushort.MaxValue, 512);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
        }

        public ClientThread ClientThread => _clientThread;

        public Task Start()
        {
            _serverSocket.Start(_gateAddress, _gatePort);
            _clientThread.Start();
            _clientThread.RestSessionArray();
            _logQueue.Enqueue($"网关[{_gateAddress}:{_gatePort}]已启动...", 1);
            return _sendQueue.ProcessSendQueue();
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Shutdown();
        }

        public (string serverIp, string serverPort, string Status, string playCount, string reviceTotal, string sendTotal, string queueCount) GetStatus()
        {
            return (_gateAddress, $"{_gatePort}", GetConnected(), _clientThread.GetSessionCount(), GetReceiveInfo(), GetSendInfo(), GetSendQueueCount());
        }

        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        /// <returns></returns>
        private string GetSendQueueCount()
        {
            return $"{_sendQueue.GetQueueCount}";
        }

        private string GetConnected()
        {
            return _clientThread.IsConnected ? $"[green]Connected[/]" : $"[red]Not Connected[/]";
        }

        private string GetSendInfo()
        {
            var sendStr = _clientThread.SendBytes switch
            {
                > 1024 * 1000 => $"↑{_clientThread.SendBytes / (1024 * 1000)}M",
                > 1024 => $"↑{_clientThread.SendBytes / 1024}K",
                _ => $"↑{_clientThread.SendBytes}B"
            };
            _clientThread.SendBytes = 0;
            return sendStr;
        }

        private string GetReceiveInfo()
        {
            var receiveStr = _clientThread.ReceiveBytes switch
            {
                > 1024 * 1000 => $"↓{_clientThread.ReceiveBytes / (1024 * 1000)}M",
                > 1024 => $"↓{_clientThread.ReceiveBytes / 1024}K",
                _ => $"↓{_clientThread.ReceiveBytes}B"
            };
            _clientThread.ReceiveBytes = 0;
            return receiveStr;
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var clientThread = _serverManager.GetClientThread();
            if (clientThread == null)
            {
                _logQueue.EnqueueDebugging("获取服务器实例失败。");
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            _logQueue.EnqueueDebugging($"用户[{sRemoteAddress}]分配到游戏数据服务器[{clientThread.ClientId}] Server:{clientThread.GetSocketIp()}");
            TSessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.SessionArray.Length; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession == null)
                {
                    userSession = new TSessionInfo();
                    userSession.Socket = e.Socket;
                    userSession.nUserListIndex = 0;
                    userSession.SessionId = e.ConnectionId;
                    userSession.dwReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = e.SocHandle;
                    clientThread.SessionArray[nIdx] = userSession;
                    break;
                }
            }
            if (userSession != null)
            {
                _logQueue.Enqueue("开始连接: " + sRemoteAddress, 5);
                clientThread.UserEnter((ushort)userSession.SessionId, userSession.SckHandle, sRemoteAddress); //通知M2有新玩家进入游戏
                _sessionManager.AddSession(userSession.SessionId, new ClientSession(userSession, clientThread, _sendQueue));
                _clientManager.AddClientThread(userSession.SessionId, clientThread);
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
            if (clientThread != null && clientThread.GateReady)
            {
                if (nSockIndex >= 0 && nSockIndex < clientThread.SessionArray.Length)
                {
                    clientThread.SessionArray[nSockIndex] = null;
                    clientThread.UserLeave(e.SocHandle); //发送消息给M2断开链接
                    _logQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
                }
            }
            else
            {
                _logQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
                _logQueue.EnqueueDebugging($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{nSockIndex}]");
            }
            _clientManager.DeleteClientThread(nSockIndex);
            _sessionManager.CloseSession(nSockIndex);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logQueue.EnqueueDebugging($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var connectionId = token.ConnectionId;
            var clientSession = _sessionManager.GetSession(connectionId);
            if (clientSession != null)
            {
                if (clientSession.Session == null)
                {
                    _logQueue.Enqueue($"[{connectionId}] Session会话已经失效", 5);
                    return;
                }
                if (clientSession.Session.Socket == null)
                {
                    _logQueue.Enqueue($"[{connectionId}] Socket已释放", 5);
                    return;
                }
                if (!clientSession.Session.Socket.Connected)
                {
                    return;
                }
                var data = new byte[token.BytesReceived];
                Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, data.Length);
                var message = new TMessageData();
                message.Buffer = data;
                message.MessageId = connectionId;
                message.BufferLen = data.Length;
                _serverManager.SendQueue(message);
            }
            else
            {
                token.Socket.Close();
                _logQueue.Enqueue("非法攻击: " + token.RemoteIPaddr, 5);
            }
        }
    }
}