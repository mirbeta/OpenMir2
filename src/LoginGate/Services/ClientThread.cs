using LoginGate.Conf;
using System;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace LoginGate
{
    /// <summary>
    /// 网关客户端(LoginGate-Client)
    /// </summary>
    public class ClientThread
    {
        /// <summary>
        /// 最大用户数
        /// </summary>
        public int MaxSession = 10000;
        /// <summary>
        /// 用户会话
        /// </summary>
        public TSessionInfo[] SessionArray;
        /// <summary>
        /// Socket
        /// </summary>
        private IClientScoket ClientSocket;
        /// <summary>
        /// 日志
        /// </summary>
        private LogQueue _logQueue = LogQueue.Instance;
        /// <summary>
        /// 会话管理
        /// </summary>
        private SessionManager _sessionManager => SessionManager.Instance;

        public ClientThread(int clientId, GameGateInfo gateInfo)
        {
            ClientId = clientId;
            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
            ClientSocket.Host = gateInfo.sServerAdress;
            ClientSocket.Port = gateInfo.nServerPort;
            SessionArray = new TSessionInfo[MaxSession];
        }

        public bool IsConnected => ClientSocket.IsConnected;

        public string GetSocketIp()
        {
            return $"{ClientSocket.Host}:{ClientSocket.Port}";
        }

        public void Start()
        {
            ClientSocket.Connect();
        }

        public void ReConnected()
        {
            if (ClientSocket.IsConnected == false)
            {
                ClientSocket.Connect();
            }
        }

        public void Stop()
        {
            for (int i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
                {
                    SessionArray[i].Socket.Close();
                }
            }
            ClientSocket.Disconnect();
        }

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            boGateReady = true;
            RestSessionArray();
            SockThreadStutas = SockThreadStutas.Connected;
            KeepAliveTick = HUtil32.GetTickCount();
            GateShare.ServerGateList.TryAdd(ClientId, this);
            dwCheckServerTick = HUtil32.GetTickCount();
            CheckServerFailCount = 1;
            _logQueue.Enqueue($"账号服务器[{e.RemoteAddress}:{e.RemotePort}]链接成功.", 1);
            _logQueue.DebugLog($"线程[{Guid.NewGuid():N}]连接 {e.RemoteAddress}:{e.RemotePort} 成功...");
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
                if (userSession.Socket != null && userSession.Socket == e.socket)
                {
                    userSession.Socket.Close();
                    userSession.Socket = null;
                    SessionArray[i] = null;
                    _logQueue.DebugLog("账号服务器断开Socket");
                }
            }
            RestSessionArray();
            boGateReady = false;
            GateShare.ServerGateList.TryRemove(ClientId, out var client);
            _logQueue.Enqueue($"账号服务器[{ClientSocket.Host}:{ClientSocket.Port}]断开链接.", 1);
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
            var sText = string.Empty;
            int sSessionId = -1;
            var sSocketMsg = HUtil32.GetString(e.Buff, 0, e.BuffLen);
            HUtil32.ArrestStringEx(sSocketMsg, "%", "$", ref sText);
            if (sText[0] == '+' && sText[1] == '-')
            {
                var tempStr = sSocketMsg[3..7];
                if (!string.IsNullOrEmpty(tempStr))
                {
                    sSessionId = int.Parse(tempStr);
                    _sessionManager.CloseSession(sSessionId);
                    _logQueue.DebugLog($"账号服务器[{ClientSocket.Host}:{ClientSocket.Port}]主动断开链接..");
                }
                return;
            }
            HUtil32.GetValidStr3(sText, ref sSessionId, new[] { "/" });
            if (sSessionId <= 0)
            {
                return;
            }
            var userData = new TMessageData();
            userData.MessageId = sSessionId;
            userData.Body = e.Buff;
            _sessionManager.SendQueue.TryWrite(userData);
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logQueue.Enqueue($"账号服务器[{ClientSocket.Host}:{ClientSocket.Port}]拒绝链接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logQueue.Enqueue($"账号服务器[{ClientSocket.Host}:{ClientSocket.Port}]关闭连接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logQueue.Enqueue($"账号服务器[{ClientSocket.Host}:{ClientSocket.Port}]链接超时...失败[{CheckServerFailCount}]次", 1);
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

        public void SendServerMsg(ushort nIdent, int wSocketIndex, int nSocket, ushort nUserListIndex, int nLen, string Data)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var strBuff = HUtil32.GetBytes(Data);
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, strBuff);
            }
            else
            {
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, Array.Empty<byte>());
            }
        }

        private void SendServerMsg(ushort nIdent, int wSocketIndex, int nSocket, ushort nUserListIndex, int nLen, byte[] Data)
        {
            var GateMsg = new PacketHeader();
            GateMsg.PacketCode = Grobal2.RUNGATECODE;
            GateMsg.Socket = nSocket;
            GateMsg.SocketIdx = (ushort)wSocketIndex;
            GateMsg.Ident = nIdent;
            GateMsg.UserIndex = nUserListIndex;
            GateMsg.PackLength = nLen;
            var sendBuffer = GateMsg.GetBuffer();
            if (Data is { Length: > 0 })
            {
                var tempBuff = new byte[20 + Data.Length];
                Array.Copy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
                Array.Copy(Data, 0, tempBuff, sendBuffer.Length, Data.Length);
                SendSocket(tempBuff);
            }
            else
            {
                SendSocket(sendBuffer);
            }
        }

        public void SendBuffer(byte[] buffer, int buffLen = 0)
        {
            SendSocket(buffer);
        }

        private void SendSocket(byte[] sendBuffer)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.Send(sendBuffer);
            }
        }

        /// <summary>
        /// 网关编号（初始化的时候进行分配）
        /// </summary>
        public int ClientId = 0;
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        public bool boCheckServerFail = false;
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 0;
        /// <summary>
        /// 服务器之间的检查间隔
        /// </summary>
        public int CheckServerTick = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool boGateReady = false;
        /// <summary>
        /// 上次心跳链接时间
        /// </summary>
        public int KeepAliveTick;
        /// <summary>
        /// 服务器链接状态
        /// </summary>
        public SockThreadStutas SockThreadStutas;
        /// <summary>
        /// 服务器链接时间
        /// </summary>
        public int dwCheckServerTick = 0;
    }

    public enum SockThreadStutas : byte
    {
        Connecting = 0,
        Connected = 1,
        TimeOut = 2
    }
}