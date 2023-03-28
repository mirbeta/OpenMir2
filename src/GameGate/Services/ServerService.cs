using System;
using System.Collections.Concurrent;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using GameGate.Conf;
using NLog;
using SystemModule;
using SystemModule.Sockets;
using TouchSocket.Core;
using TouchSocket.Sockets;
using NetworkMonitor = SystemModule.NetworkMonitor;

namespace GameGate.Services
{
    /// <summary>
    /// 客户端服务端(Mir2-GameGate)
    /// </summary>
    public class ServerService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpService _serverSocket;
        private readonly GameGateInfo _gateInfo;
        private readonly ClientThread _clientThread;
        private readonly IPEndPoint _gateEndPoint;
        private readonly SendQueue messageSendQueue;
        private readonly ConcurrentQueue<int> SessionCloseQueue;
        private static SessionContainer SessionContainer => SessionContainer.Instance;
        private static ServerManager ServerMgr => ServerManager.Instance;
        private readonly NetworkMonitor networkMonitor;

        public ServerService(GameGateInfo gameGate)
        {
            _gateInfo = gameGate;
            networkMonitor = new NetworkMonitor();
            SessionCloseQueue = new ConcurrentQueue<int>();
            messageSendQueue = new SendQueue();
            _gateEndPoint = new IPEndPoint(IPAddress.Parse(gameGate.ServerAdress), gameGate.GatePort);
            _clientThread = new ClientThread(_gateEndPoint, gameGate, networkMonitor);
            _serverSocket = new TcpService();
            _serverSocket.Connected += ServerSocketClientConnect;
            _serverSocket.Disconnected += ServerSocketClientDisconnect;
            _serverSocket.Received += ServerSocketClientRead;
            ClientManager.Instance.AddClientThread(gameGate.ThreadId, _clientThread);
        }

        public ClientThread ClientThread => _clientThread;
        public NetworkMonitor NetworkMonitor => networkMonitor;
        public GameGateInfo GateInfo => _gateInfo;

        public void Initialize()
        {
            var touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts(new IPHost[1]
            {
                new IPHost(IPAddress.Parse(_gateInfo.ServerAdress), _gateInfo.GatePort)
            }).SetBufferLength(255).SetMaxCount(GateShare.MaxSession);
            _serverSocket.Setup(touchSocketConfig);
            _clientThread.Initialize();
        }

        public void Start(CancellationToken stoppingToken)
        {
            _serverSocket.Start();
            _clientThread.Start();
            _clientThread.RestSessionArray();
            messageSendQueue.StartProcessQueueSend(stoppingToken);
            _logger.Info($"游戏网关[{_gateEndPoint}]已启动...");
        }

        public void Stop()
        {
            _clientThread.Stop();
            _serverSocket.Stop();
        }

        public (string serverIp, string Status, string playCount, string reviceTotal, string sendTotal, string totalrevice, string totalSend, string queueCount, int threadCount) GetStatus()
        {
            return (_gateEndPoint.ToString(), _clientThread.GetConnected, _clientThread.GetSessionCount(), ShowReceive, ShowSend, TotalReceive, TotalSend, WaitQueueCount, ServerManager.MessageWorkThreads);
        }

        public string ShowReceive => $"↓{networkMonitor.ShowReceive()}";

        public string ShowSend => $"↑{networkMonitor.ShowSendStats()}";

        public string TotalReceive => $"↓{HUtil32.FormatBytesValue(networkMonitor.TotalBytesRecv)}";

        public string TotalSend => $"↑{HUtil32.FormatBytesValue(networkMonitor.TotalBytesSent)}";

        /// <summary>
        /// 获取队列待处理数
        /// </summary>
        /// <returns></returns>
        private string WaitQueueCount => messageSendQueue.QueueCount + "/" + SessionContainer.QueueCount;

        /// <summary>
        /// 处理会话关闭列表
        /// </summary>
        public void ProcessCloseSessionQueue()
        {
            if (SessionCloseQueue.IsEmpty)
            {
                return;
            }
            for (var i = 0; i < SessionCloseQueue.Count; i++)
            {
                SessionCloseQueue.TryDequeue(out var socketId);
                _clientThread.UserLeave(socketId); //发送消息给GameSvr断开链接
            }
        }

        public void Send(int connectionId, ReadOnlyMemory<byte> buffer)
        {
            _serverSocket.Send(Convert.ToString(connectionId), buffer);
            networkMonitor.Send(buffer.Length);
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        private void ServerSocketClientConnect(object sender, TouchSocketEventArgs e)
        {
            var client = (SocketClient)sender;
            var sRemoteAddress = client.MainSocket.RemoteEndPoint.GetIP();
            var clientId = int.Parse(client.ID);
            _logger.Debug($"客户端 IP:{sRemoteAddress} ThreadId:{GateInfo.ServiceId} SessionId:{clientId} RunPort:{_gateEndPoint}");
            var clientThread = ServerMgr.GetClientThread(GateInfo.ServiceId, out var threadId);
            if (clientThread == null || threadId < 0)
            {
                //todo 直接断开玩家连接，提示客户端链接失败
                _logger.Debug("获取GameSvr服务器实例失败，请确认GameGate和GameSvr是否链接正常。");
                return;
            }
            SessionInfo userSession = null;
            for (var nIdx = 0; nIdx < clientThread.SessionArray.Length; nIdx++)
            {
                userSession = clientThread.SessionArray[nIdx];
                if (userSession == null)
                {
                    userSession = new SessionInfo();
                    userSession.Socket = client.MainSocket;
                    userSession.SessionIndex = 0;
                    userSession.ConnectionId = clientId;
                    userSession.ReceiveTick = HUtil32.GetTickCount();
                    userSession.SckHandle = (int)client.MainSocket.Handle;
                    userSession.SessionId = (ushort)nIdx;
                    userSession.ThreadId = threadId;
                    clientThread.SessionArray[nIdx] = userSession;
                    break;
                }
            }
            if (userSession != null)
            {
                clientThread.UserEnter((ushort)clientId, userSession.SckHandle, sRemoteAddress); //通知GameSvr有新玩家进入游戏
                SessionContainer.AddSession(GateInfo.ServiceId, clientId, new ClientSession(GateInfo.ServiceId, userSession, clientThread, messageSendQueue));
                _logger.Info("开始连接: " + sRemoteAddress);
                _logger.Debug($"GateId:{GateInfo.ServiceId} 用户 IP:[{sRemoteAddress}] SocketId:[{userSession.SessionId}] GameSrv:{clientThread.EndPoint}/{clientThread.ThreadId}");
            }
            else
            {
                client.Close();
                _logger.Info("禁止连接: " + sRemoteAddress);
            }
        }

        private void ServerSocketClientDisconnect(object sender, DisconnectEventArgs e)
        {
            var client = (SocketClient)sender;
            var sRemoteAddress = client.MainSocket.RemoteEndPoint.GetIP();
            var clientId = int.Parse(client.ID);
            var clientSession = SessionContainer.GetSession(GateInfo.ServiceId, clientId);
            if (clientSession == null)
            {
                return;
            }
            var clientThread = clientSession.ServerThread;
            if (clientThread != null)
            {
                for (int i = 0; i < clientThread.SessionArray.Length; i++)
                {
                    if (clientThread.SessionArray[i] == null)
                    {
                        continue;
                    }
                    if (clientThread.SessionArray[i].ConnectionId == clientId)
                    {
                        clientThread.SessionArray[i].Socket.Close();
                        clientThread.SessionArray[i].SessionIndex = 0;
                        clientThread.SessionArray[i].ReceiveTick = 0;
                        clientThread.SessionArray[i].SckHandle = 0;
                        clientThread.SessionArray[i].SessionId = 0;
                        clientThread.SessionArray[i].Socket = null;
                        clientThread.SessionArray[i] = null;
                        break;
                    }
                }
                _logger.Info("断开链接: " + sRemoteAddress);
            }
            else
            {
                _logger.Info("断开链接: " + sRemoteAddress);
            }
            SessionCloseQueue.Enqueue((int)client.MainSocket.Handle); //等待通知GameSvr断开用户会话,否则会出现退出游戏后再次登陆游戏提示账号已经登陆
            SessionContainer.CloseSession(GateInfo.ServiceId, clientId);
            _logger.Debug($"用户断开链接 Ip:[{sRemoteAddress}] ThreadId:{_gateInfo.ServiceId} SessionId:{client.ID}");
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            _logger.Debug($"客户端链接错误.[{e.Exception.ErrorCode}]");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        private unsafe void ServerSocketClientRead(object sender, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            var client = (SocketClient)sender;
            var sRemoteAddress = client.MainSocket.RemoteEndPoint.GetIP();
            var sessionId = int.Parse(client.ID);
            var clientSession = SessionContainer.GetSession(GateInfo.ServiceId, sessionId);
            if (clientSession != null)
            {
                var buff = new IntPtr(NativeMemory.AllocZeroed((uint)byteBlock.Len));
                MemoryCopy.BlockCopy(byteBlock.Buffer, 0, buff.ToPointer(), 0, byteBlock.Len);
                ServerMgr.SendMessageQueue(new ClientPacketMessage(GateInfo.ServiceId, sessionId, buff, (ushort)byteBlock.Len));
                networkMonitor.Receive(byteBlock.Len);
            }
            else
            {
                client.Close();
                _logger.Info("非法攻击: " + sRemoteAddress);
            }
        }
    }
}