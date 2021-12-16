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
        public int NReviceMsgSize = 0;
        private long _dwProcessClientMsgTime = 0;
        private readonly ISocketServer _serverSocket;
        private readonly SessionManager _sessionManager;
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        private Channel<TSendUserData> _reviceMsgList = null;
        private readonly ClientManager _clientManager;
        private readonly ConfigManager _configManager;

        public ServerService(ConfigManager configManager, SessionManager sessionManager, ClientManager clientManager)
        {
            _serverSocket = new ISocketServer(2000, 1024);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
            _reviceMsgList = Channel.CreateUnbounded<TSendUserData>();
            _sessionManager = sessionManager;
            _clientManager = clientManager;
            _configManager = configManager;
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
                    var clientSession = _sessionManager.GetSession(message.UserCientId);
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
                Console.WriteLine("获取服务器实例失败。");
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            Console.WriteLine($"用户[{sRemoteAddress}]分配到游戏数据服务器[{clientThread.ClientId}] Server:{clientThread.GetSocketIp()}");
            TSessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.MaxSession; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession.Socket == null)
                {
                    userSession.Socket = e.Socket;
                    userSession.nUserListIndex = 0;
                    userSession.SocketId = e.ConnectionId;
                    userSession.dwReceiveTick = HUtil32.GetTickCount();
                    userSession.nSckHandle = (int)e.Socket.Handle;
                    break;
                }
            }
            if (userSession != null)
            {
                clientThread.UserEnter(userSession.SocketId, (int)e.Socket.Handle, sRemoteAddress); //通知M2有新玩家进入游戏
                GateShare.AddMainLogMsg("开始连接: " + sRemoteAddress, 5);
                _clientManager.AddClientThread(e.ConnectionId, clientThread);//链接成功后建立对应关系
                _sessionManager.AddSession(userSession.SocketId, new ClientSession(_configManager, userSession, clientThread));
            }
            else
            {
                e.Socket.Close();
                GateShare.AddMainLogMsg("禁止连接: " + sRemoteAddress, 1);
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
                    userSession.nSckHandle = -1;
                    clientThread.UserLeave((int)e.Socket.Handle); //发送消息给M2断开链接
                    GateShare.AddMainLogMsg("断开连接: " + sRemoteAddr, 5);
                }
            }
            else
            {
                GateShare.AddMainLogMsg("断开链接: " + sRemoteAddr, 5);
                Debug.WriteLine($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{e.ConnectionId}]");
            }
            _clientManager.DeleteClientThread(e.ConnectionId);
            _sessionManager.Remove(e.ConnectionId);
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            Console.WriteLine($"客户端链接错误.[{e.Exception.ErrorCode}]");
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
                var userClient = _clientManager.GetClientThread(connectionId);
                var sRemoteAddress = token.RemoteIPaddr;
                if (userClient == null)
                {
                    GateShare.AddMainLogMsg("非法攻击: " + sRemoteAddress, 5);
                    Debug.WriteLine($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddress}] ConnectionId:[{connectionId}]");
                    return;
                }
                if (!userClient.boGateReady)
                {
                    GateShare.AddMainLogMsg("未就绪: " + sRemoteAddress, 5);
                    Debug.WriteLine($"服务端未就绪 Server:[{userClient.GetSocketIp()}] ConnectionId:[{connectionId}]");
                    return;
                }
                if (!userClient.SessionArray[userClient.ClientId].Socket.Connected)
                {
                    return;
                }
                var dwProcessMsgTick = HUtil32.GetTickCount();
                var nReviceLen = token.BytesReceived;
                var data = new byte[nReviceLen];
                Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, nReviceLen);
                var nSocketIndex = token.ConnectionId;
                if (nSocketIndex >= 0 && nSocketIndex < userClient.MaxSession)
                {
                    GateShare.NReviceMsgSize += data.Length;
                    var userData = new TSendUserData();
                    userData.Buffer = data;
                    userData.BufferLen = data.Length;
                    userData.UserCientId = token.ConnectionId;
                    _reviceMsgList.Writer.TryWrite(userData);
                }
                var dwProcessMsgTime = HUtil32.GetTickCount() - dwProcessMsgTick;
                if (dwProcessMsgTime > _dwProcessClientMsgTime)
                {
                    _dwProcessClientMsgTime = dwProcessMsgTime;
                }
            }
            else
            {
                GateShare.AddMainLogMsg("非法攻击: " + token.RemoteIPaddr, 5);
            }
        }
    }
}