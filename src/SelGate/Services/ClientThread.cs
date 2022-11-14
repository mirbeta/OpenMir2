using SelGate.Package;
using System;
using System.Net;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packets;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace SelGate.Services
{
    /// <summary>
    /// 网关客户端(SelGate-DBSvr)
    /// </summary>
    public class ClientThread
    {
        /// <summary>
        /// Socket客户端
        /// </summary>
        private readonly ClientScoket _clientSocket;
        /// <summary>
        /// 网关编号（初始化的时候进行分配）
        /// </summary>
        public readonly int ClientId = 0;
        /// <summary>
        /// 最大用户数
        /// </summary>
        public const int MaxSession = 2000;
        /// <summary>
        /// 用户会话
        /// </summary>
        public readonly TSessionInfo[] SessionArray;
        /// <summary>
        ///  网关游戏服务器之间检测是否失败/超时
        /// </summary>
        public bool CheckServerFail = false;
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 0;
        public bool KeepAlive;
        public int KeepAliveTick;
        public SockThreadStutas SockThreadStutas;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool boGateReady = false;
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool isConnected = false;
        /// <summary>
        /// 会话管理
        /// </summary>
        private readonly SessionManager _sessionManager;
        /// <summary>
        /// Logger
        /// </summary>
        private static MirLogger _logQueue;

        public ClientThread(int clientId, string serverAddr, int serverPort, SessionManager sessionManager, MirLogger logQueue)
        {
            ClientId = clientId;
            _logQueue = logQueue;
            SessionArray = new TSessionInfo[MaxSession];
            _sessionManager = sessionManager;
            _clientSocket = new ClientScoket(new IPEndPoint(IPAddress.Parse(serverAddr), serverPort), 512);
            _clientSocket.OnConnected += ClientSocketConnect;
            _clientSocket.OnDisconnected += ClientSocketDisconnect;
            _clientSocket.OnReceivedData += ClientSocketRead;
            _clientSocket.OnError += ClientSocketError;
            SockThreadStutas = SockThreadStutas.Connecting;
            KeepAliveTick = HUtil32.GetTickCount();
            KeepAlive = true;
        }

        public bool IsConnected => isConnected;

        public string GetEndPoint()
        {
            return _clientSocket.RemoteEndPoint.ToString();
        }

        public void Start()
        {
            _clientSocket.Connect();
        }

        public void ReConnected()
        {
            if (isConnected == false)
            {
                _clientSocket.Connect();
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

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            boGateReady = true;
            RestSessionArray();
            isConnected = true;
            SockThreadStutas = SockThreadStutas.Connected;
            KeepAliveTick = HUtil32.GetTickCount();
            GateShare.CheckServerTick = HUtil32.GetTickCount();
            GateShare.ServerGateList.Add(this);
            _logQueue.LogInformation($"数据库服务器[{e.RemoteEndPoint}]链接成功.", 1);
            _logQueue.DebugLog($"线程[{Guid.NewGuid():N}]连接 {e.RemoteEndPoint} 成功...");
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            for (var i = 0; i < MaxSession; i++)
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
                }
            }
            RestSessionArray();
            GateShare.ServerGateList.Remove(this);
            _logQueue.LogInformation($"数据库服务器[{e.RemoteEndPoint}]断开链接.", 1);
            boGateReady = false;
            isConnected = false;
            CheckServerFail = true;
        }

        /// <summary>
        /// 收到数据库服务器 直接发送给客户端
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            if (e.BuffLen <= 0)
            {
                return;
            }
            var packet = Packets.ToPacket<ServerDataMessage>(e.Buff);
            if (packet == null)
            {
                return;
            }
            if (packet.Type == ServerDataType.KeepAlive)
            {
                _logQueue.DebugLog("DBSvr Heartbeat Response");
                CheckServerFail = false;
                boGateReady = true;
                isConnected = true;
                return;
            }
            var message = new TMessageData();
            message.SessionId = packet.SocketId;
            message.Body = packet.Data;
            _sessionManager.SendQueue.TryWrite(message);
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logQueue.LogInformation($"数据库服务器[{_clientSocket.RemoteEndPoint}]拒绝链接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logQueue.LogInformation($"数据库服务器[{_clientSocket.RemoteEndPoint}]关闭连接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logQueue.LogInformation($"数据库服务器[{_clientSocket.RemoteEndPoint}]链接超时...失败[{CheckServerFailCount}]次", 1);
                    break;
            }
        }

        public void RestSessionArray()
        {
            for (var i = 0; i < MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].Socket = null;
                    SessionArray[i].dwReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].SocketId = 0;
                    SessionArray[i].ClientIP = string.Empty;
                }
            }
        }

        public void SendKeepAlive()
        {
            var accountPacket = new ServerDataMessage();
            accountPacket.Data = Array.Empty<byte>();
            accountPacket.Type = ServerDataType.KeepAlive;
            accountPacket.SocketId = 0;
            accountPacket.PacketLen = accountPacket.GetPacketSize();
            SendSocket(accountPacket.GetBuffer());
            _logQueue.DebugLog("Send DBSvr Heartbeat.");
        }

        public void SendBuffer(string sendText)
        {
            SendSocket(HUtil32.GetBytes(sendText));
        }

        public void SendSocket(byte[] buffer)
        {
            if (_clientSocket.IsConnected)
            {
                _clientSocket.Send(buffer);
            }
        }
    }

    public enum SockThreadStutas : byte
    {
        Connecting = 0,
        Connected = 1,
        TimeOut = 2
    }
}