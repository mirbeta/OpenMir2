using System;
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
        private readonly string GateAddress;
        private readonly int GatePort = 0;

        public ServerService(int i, GameGateInfo gameGate)
        {
            _clientThread = new ClientThread(i, gameGate);
            GateAddress = gameGate.sServerAdress;
            GatePort = gameGate.nGatePort;
            _serverSocket = new ISocketServer(ushort.MaxValue, 512);
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
            _logQueue.Enqueue($"网关[{GateAddress}:{GatePort}]已启动...", 1);
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Shutdown();
        }

        public (string serverIp, string serverPort, string Status, string playCount, string reviceTotal, string sendTotal) GetStatus()
        {
            return (GateAddress, $"{GatePort}", GetConnected(), _clientThread.GetSessionCount(), GetReceiveInfo(), GetSendInfo());
        }

        private string GetConnected()
        {
            return _clientThread.IsConnected ? $"[green]Connected[/]" : $"[red]Not Connected[/]";
        }

        private string GetSendInfo()
        {
            var totalStr = string.Empty;
            if (_clientThread.SendBytes > (1024 * 1000))
            {
                totalStr = $"↑{_clientThread.SendBytes / (1024 * 1000)}M";
            }
            else if (_clientThread.SendBytes > 1024)
            {
                totalStr = $"↑{_clientThread.SendBytes / 1024}K";
            }
            else
            {
                totalStr = $"↑{_clientThread.SendBytes}B";
            }
            _clientThread.SendBytes = 0;
            return totalStr;
        }

        private string GetReceiveInfo()
        {
            var totalStr = string.Empty;
            if (_clientThread.ReceiveBytes > (1024 * 1000))
            {
                totalStr = $"↓{_clientThread.ReceiveBytes / (1024 * 1000)}M";
            }
            else if (_clientThread.ReceiveBytes > 1024)
            {
                totalStr = $"↓{_clientThread.ReceiveBytes / 1024}K";
            }
            else
            {
                totalStr = $"↓{_clientThread.ReceiveBytes}B";
            }
            _clientThread.ReceiveBytes = 0;
            return totalStr;
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                _sessionManager.AddSession(userSession.SessionId, new ClientSession(userSession, clientThread));
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
                    clientThread.SessionArray[nSockIndex] = new TSessionInfo();
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