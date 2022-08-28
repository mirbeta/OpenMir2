using CloudGate.Conf;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace CloudGate.Services
{
    /// <summary>
    /// CloudGate服务端
    /// </summary>
    public class ServerService
    {
        private readonly SocketServer _serverSocket;
        private readonly IPEndPoint _gateEndPoint;
        private readonly SendQueue _sendQueue;
        private readonly ConcurrentQueue<int> _waitCloseQueue;
        private static MirLog LogQueue => MirLog.Instance;
        private static SessionManager SessionManager => SessionManager.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;
        /// <summary>
        /// 发送总字节数
        /// </summary>
        public int SendBytes;
        /// <summary>
        /// 接收总字节数
        /// </summary>
        public int ReceiveBytes;
        
        public ServerService(string clientId, GameGateInfo gameGate)
        {
            _waitCloseQueue = new ConcurrentQueue<int>();
            _serverSocket = new SocketServer(ushort.MaxValue, 255);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _serverSocket.Init();
            _sendQueue = new SendQueue();
            _gateEndPoint = IPEndPoint.Parse(string.Concat(gameGate.ServerAdress, ":", gameGate.GatePort));
        }

        public Task Start()
        {
            _serverSocket.Start(_gateEndPoint);
            LogQueue.Enqueue($"Cloud智能防外挂网关[{_gateEndPoint}]已启动...", 1);
            return _sendQueue.ProcessSendQueue();
        }

        public void Stop()
        {
            _serverSocket.Shutdown();
        }

        public (string serverIp,  string Status, string playCount, string reviceTotal, string sendTotal, string queueCount, int threadCount) GetStatus()
        {
            return (_gateEndPoint.ToString(), GetConnected(), "0", GetReceiveInfo(), GetSendInfo(), GetSendQueueCount(), GetThreadCount());
        }

        /// <summary>
        /// 获取消息处理线程数量,最少1个
        /// </summary>
        /// <returns></returns>
        private int GetThreadCount()
        {
            return ServerManager.MessageThreadCount;
        }

        /// <summary>
        /// 获取待发送队列数量
        /// </summary>
        /// <returns></returns>
        private string GetSendQueueCount()
        {
            return string.Concat(_sendQueue.GetQueueCount, "/", SessionManager.GetQueueCount);
        }

        private string GetConnected()
        {
            return true ? $"[green]Connected[/]" : $"[red]Not Connected[/]";
        }

        private string GetSendInfo()
        {
            var sendStr = SendBytes switch
            {
                > 1024 * 1000 => $"↑{SendBytes / (1024 * 1000)}M",
                > 1024 => $"↑{SendBytes / 1024}K",
                _ => $"↑{SendBytes}B"
            };
            SendBytes = 0;
            return sendStr;
        }

        private string GetReceiveInfo()
        {
            var receiveStr = ReceiveBytes switch
            {
                > 1024 * 1000 => $"↓{ReceiveBytes / (1024 * 1000)}M",
                > 1024 => $"↓{ReceiveBytes / 1024}K",
                _ => $"↓{ReceiveBytes}B"
            };
            ReceiveBytes = 0;
            return receiveStr;
        }

        /// <summary>
        /// 游戏网关链接成功
        /// </summary>
        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            var clientThread = ServerManager.GetClientThread();
            if (clientThread == null)
            {
                LogQueue.EnqueueDebugging("获取服务器实例失败。");
                return;
            }
            var sRemoteAddress = e.RemoteIPaddr;
            LogQueue.EnqueueDebugging($"用户[{sRemoteAddress}]分配到游戏数据服务器[{clientThread.ClientId}]");
            TSessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.SessionArray.Length; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession == null)
                {
                    userSession = new TSessionInfo();
                    userSession.Socket = e.Socket;
                    userSession.nUserListIndex = 0;
                    userSession.ConnectionId = e.ConnectionId;
                    userSession.dwReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = e.SocHandle;
                    userSession.SessionId = (ushort)e.SocHandle;
                    clientThread.SessionArray[nIdx] = userSession;
                    break;
                }
            }
            if (userSession != null)
            {
                LogQueue.Enqueue("开始连接: " + sRemoteAddress, 5);
                clientThread.UserEnter(userSession.SessionId, userSession.SckHandle, sRemoteAddress); //通知M2有新玩家进入游戏
                SessionManager.AddSession(userSession.SessionId, new ClientSession(userSession, SessionManager, _sendQueue));
            }
            else
            {
                e.Socket.Close();
                LogQueue.Enqueue("禁止连接: " + sRemoteAddress, 1);
            }
        }

        /// <summary>
        /// 游戏网关断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddr = e.RemoteIPaddr;
            var connectionId = e.ConnectionId;
            //var clientThread = ClientManager.GetClientThread(connectionId);
            //if (clientThread != null && clientThread.GateReady)
            //{
            //    for (int i = 0; i < clientThread.SessionArray.Length; i++)
            //    {
            //        if (clientThread.SessionArray[i] == null)
            //        {
            //            continue;
            //        }
            //        if (clientThread.SessionArray[i].SckHandle == e.SocHandle)
            //        {
            //            clientThread.SessionArray[i].Socket.Close();
            //            clientThread.SessionArray[i].Socket = null;
            //            clientThread.SessionArray[i].nUserListIndex = 0;
            //            clientThread.SessionArray[i].dwReceiveTick = HUtil32.GetTickCount();
            //            clientThread.SessionArray[i].SckHandle = 0;
            //            clientThread.SessionArray[i].SessionId = 0;
            //            clientThread.SessionArray[i] = null;
            //            break;
            //        }
            //    }
            //    _waitCloseQueue.Enqueue(e.SocHandle); //等待100ms才能发送给M2
            //    LogQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
            //}
            //else
            //{
            //    LogQueue.Enqueue("断开链接: " + sRemoteAddr, 5);
            //    LogQueue.EnqueueDebugging($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{connectionId}]");
            //}
            //ClientManager.DeleteClientThread(connectionId);
            SessionManager.CloseSession(e.SocHandle);
        }

        /// <summary>
        /// 游戏网关链接异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            LogQueue.EnqueueDebugging($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到游戏网关消息
        /// </summary>
        private void ServerSocketClientRead(object sender, AsyncUserToken token)
        {
            var connectionId = token.SocHandle;
            var clientSession = SessionManager.GetSession(connectionId);
            if (clientSession != null)
            {
                if (clientSession.Session == null)
                {
                    LogQueue.Enqueue($"[{connectionId}] Session会话已经失效", 5);
                    return;
                }
                if (clientSession.Session.Socket == null)
                {
                    LogQueue.Enqueue($"[{connectionId}] Socket已释放", 5);
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
                ServerManager.SendQueue(message);
            }
            else
            {
                token.Socket.Close();
                LogQueue.Enqueue("非法攻击: " + token.RemoteIPaddr, 5);
            }
        }
    }
}