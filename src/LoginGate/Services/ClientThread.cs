using LoginGate.Conf;
using LoginGate.Packet;
using System;
using System.Net;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ServerPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace LoginGate.Services
{
    /// <summary>
    /// 网关客户端(LoginGate-Mir2)
    /// </summary>
    public class ClientThread
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly MirLogger _logger;
        /// <summary>
        /// 用户会话
        /// </summary>
        public readonly TSessionInfo[] SessionArray;
        /// <summary>
        /// Socket
        /// </summary>
        private readonly ClientScoket _clientSocket;
        /// <summary>
        /// Client管理
        /// </summary>
        private readonly ClientManager _clientManager;
        /// <summary>
        /// Session管理
        /// </summary>
        private readonly SessionManager _sessionManager;

        public ClientThread(MirLogger logger, SessionManager sessionManager, ClientManager clientManager)
        {
            _logger = logger;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
            _clientSocket = new ClientScoket();
            _clientSocket.OnConnected += ClientSocketConnect;
            _clientSocket.OnDisconnected += ClientSocketDisconnect;
            _clientSocket.ReceivedDatagram += ClientSocketRead;
            _clientSocket.OnError += ClientSocketError;
            SessionArray = new TSessionInfo[GateShare.MaxSession];
        }

        public bool IsConnected => _clientSocket.IsConnected;

        public IPEndPoint EndPoint => _clientSocket.EndPoint;

        public void Start(GameGateInfo gateInfo)
        {
            _clientSocket.Connect(gateInfo.LoginAdress, gateInfo.LoginPort);
        }

        public void ReConnected()
        {
            if (_clientSocket.IsConnected == false)
            {
                _clientSocket.Connect(EndPoint);
            }
        }

        public void Stop()
        {
            for (var i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
                {
                    SessionArray[i].Socket.Close();
                }
            }
            _clientSocket.Disconnect();
        }

        public bool SessionIsFull()
        {
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                var userSession = SessionArray[i];
                if (userSession == null)
                {
                    return false;
                }
                if (userSession.Socket == null)
                {
                    return false;
                }
            }
            return true;
        }

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            ConnectState = true;
            RestSessionArray();
            SockThreadStutas = SockThreadStutas.Connected;
            KeepAliveTick = HUtil32.GetTickCount();
            CheckServerTick = HUtil32.GetTickCount();
            CheckServerFailCount = 1;
            //_clientManager.AddClientThread(e.SocketHandle, this);
            _logger.LogInformation($"账号服务器[{EndPoint}]链接成功.", 1);
            _logger.DebugLog($"线程[{Guid.NewGuid():N}]连接 {e.RemoteEndPoint} 成功...");
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                var userSession = SessionArray[i];
                if (userSession == null)
                {
                    continue;
                }
                if (userSession.Socket != null && userSession.Socket == e.Socket)
                {
                    userSession.Socket.Close();
                    userSession.Socket = null;
                    SessionArray[i] = null;
                    _logger.DebugLog("账号服务器断开Socket");
                }
            }
            RestSessionArray();
            ConnectState = false;
            //_clientManager.DeleteClientThread(e.SocketHandle);
            _logger.LogInformation($"账号服务器[{EndPoint}]断开链接.", 1);
        }

        /// <summary>
        /// 收到登录服务器消息 直接发送给客户端
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            if (e.BuffLen <= 0)
            {
                return;
            }
            ReceiveBytes += e.BuffLen;
            if (e.Buff[1] == (byte)'+') //收到LoginSvr发过来的关闭会话请求
            {
                if (e.Buff[2] == (byte)'-')
                {
                    var sessionId = BitConverter.ToInt32(e.Buff[4..8]);
                    _sessionManager.CloseSession(sessionId);
                    _logger.LogInformation("收到LoginSvr关闭会话请求", 1);
                }
                else
                {
                    KeepAliveTick = HUtil32.GetTickCount();
                }
                return;
            }
            var loginSvrPacket = Packets.ToPacket<LoginSvrPacket>(e.Buff);
            if (loginSvrPacket != null)
            {
                var userData = new MessageData();
                userData.ConnectionId = loginSvrPacket.ConnectionId;
                userData.Body = loginSvrPacket.ClientPacket;
                _clientManager.SendQueue(userData);
            }
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case SocketError.ConnectionRefused:
                    _logger.LogInformation($"账号服务器[{EndPoint}]拒绝链接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case SocketError.ConnectionReset:
                    _logger.LogInformation($"账号服务器[{EndPoint}]关闭连接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case SocketError.TimedOut:
                    _logger.LogInformation($"账号服务器[{EndPoint}]链接超时...失败[{CheckServerFailCount}]次", 1);
                    break;
            }
        }

        private void RestSessionArray()
        {
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].Socket = null;
                    SessionArray[i].ReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].ConnectionId = 0;
                    SessionArray[i].ClientIP = string.Empty;
                }
            }
        }

        public void SendPacket(GatePacket packet)
        {
            packet.StartChar = '%';
            packet.EndChar = '$';
            SendSocket(packet.GetBuffer());
        }

        private void SendSocket(byte[] sendBuffer)
        {
            if (_clientSocket.IsConnected)
            {
                _clientSocket.Send(sendBuffer);
                SendBytes += sendBuffer.Length;
            }
        }

        private string SendStatistics()
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

        private string ReceiveStatistics()
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

        private string GetSessionCount()
        {
            var count = 0;
            for (var i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
                {
                    count++;
                }
            }
            if (count > Counter)
            {
                Counter = count;
            }
            return count + "/" + Counter;
        }

        private string GetConnected()
        {
            return IsConnected ? $"[green]Connected[/]" : $"[red]Not Connected[/]";
        }

        public (string remoteendpoint, string status, string sessionCount, string reviceTotal, string sendTotal, string threadCount) GetStatus()
        {
            return (EndPoint?.ToString(), GetConnected(), GetSessionCount(), SendStatistics(), ReceiveStatistics(), "1");
        }

        /// <summary>
        /// 网关编号（初始化的时候进行分配）
        /// </summary>
        public readonly int ClientId = 0;
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        public bool CheckServerFail = false;
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 1;
        /// <summary>
        /// 服务器之间的检查间隔
        /// </summary>
        public int CheckServerTick = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool ConnectState = false;
        /// <summary>
        /// 上次心跳链接时间
        /// </summary>
        public int KeepAliveTick;
        /// <summary>
        /// 服务器链接状态
        /// </summary>
        public SockThreadStutas SockThreadStutas;
        /// <summary>
        /// 历史最高在线人数
        /// </summary>
        private int Counter = 0;
        /// <summary>
        /// 发送总字节数
        /// </summary>
        public int SendBytes;
        /// <summary>
        /// 接收总字节数
        /// </summary>
        public int ReceiveBytes;
    }

    public enum SockThreadStutas : byte
    {
        Connecting = 0,
        Connected = 1,
        TimeOut = 2
    }
}