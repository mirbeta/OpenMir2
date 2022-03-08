using System;
using System.Diagnostics;
using LoginGate.Package;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace LoginGate.Services
{
    /// <summary>
    /// 网关客户端(SelGate-DBSvr)
    /// </summary>
    public class ClientThread
    {
        private IClientScoket ClientSocket;
        private int nBufferOfM2Size = 0;
        private int dwProcessServerMsgTime = 0;
        /// <summary>
        /// 网关编号（初始化的时候进行分配）
        /// </summary>
        public int ClientId = 0;
        /// <summary>
        /// 最大用户数
        /// </summary>
        public int MaxSession = 2000;
        /// <summary>
        /// 用户会话
        /// </summary>
        public TSessionInfo[] SessionArray;
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        public bool boCheckServerFail = false;
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 0;
        /// <summary>
        /// 独立Buffer分区
        /// </summary>
        private byte[] SocketBuffer = null;
        /// <summary>
        /// 上次剩下多少字节未处理
        /// </summary>
        private int nBuffLen = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool boGateReady = false;
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool isConnected = false;
        private readonly SessionManager _sessionManager;

        public ClientThread(int clientId, string serverAddr, int serverPort, SessionManager sessionManager)
        {
            ClientId = clientId;
            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
            ClientSocket.Address = serverAddr;
            ClientSocket.Port = serverPort;
            SessionArray = new TSessionInfo[MaxSession];
            _sessionManager = sessionManager;
        }

        public bool IsConnected => isConnected;

        public string GetSocketIp()
        {
            return $"{ClientSocket.Address}:{ClientSocket.Port}";
        }

        public void Start()
        {
            ClientSocket.Connect();
        }

        public void ReConnected()
        {
            if (isConnected == false)
            {
                ClientSocket.Connect();
            }
        }

        public void Stop()
        {
            ClientSocket.Disconnect();
        }

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            boGateReady = true;
            GateShare.dwCheckServerTick = HUtil32.GetTickCount();
            RestSessionArray();
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.ServerGateList.Add(this);
            GateShare.AddMainLogMsg($"账号服务器[{e.RemoteAddress}:{e.RemotePort}]链接成功.", 1);
            Debug.WriteLine($"线程[{Guid.NewGuid():N}]连接 {e.RemoteAddress}:{e.RemotePort} 成功...");
            isConnected = true;
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            for (var i = 0; i < MaxSession; i++)
            {
                var userSession = SessionArray[i];
                if (userSession.Socket != null && userSession.Socket == e.socket)
                {
                    userSession.Socket.Close();
                    userSession.Socket = null;
                }
            }
            RestSessionArray();
            SocketBuffer = null;
            boGateReady = false;
            GateShare.ServerGateList.Remove(this);
            GateShare.AddMainLogMsg($"账号服务器[{e.RemoteAddress}:{e.RemotePort}]断开链接.", 1);
            isConnected = false;
        }

        /// <summary>
        /// 收到数据库服务器 直接发送给客户端
        /// todo 需要优化封包处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var sText = string.Empty;
            string sSessionId = string.Empty;
            HUtil32.ArrestStringEx(e.ReceiveText, "%", "$", ref sText);
            HUtil32.GetValidStr3(sText, ref sSessionId, new[] { "/" });
            var userData = new TMessageData();
            userData.SessionId = int.Parse(sSessionId);
            userData.Body = e.Buff;
            _sessionManager.SendQueue.TryWrite(userData);
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    GateShare.AddMainLogMsg("账号服务器[" + ClientSocket.Address + ":" + ClientSocket.Port + "]拒绝链接...", 1);
                    isConnected = false;
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    GateShare.AddMainLogMsg("账号服务器[" + ClientSocket.Address + ":" + ClientSocket.Port + "]关闭连接...", 1);
                    isConnected = false;
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    GateShare.AddMainLogMsg("账号服务器[" + ClientSocket.Address + ":" + ClientSocket.Port + "]链接超时...", 1);
                    isConnected = false;
                    break;
            }
        }

        public void RestSessionArray()
        {
            TSessionInfo tSession;
            for (var i = 0; i < MaxSession; i++)
            {
                if (SessionArray[i] == null)
                {
                    SessionArray[i] = new TSessionInfo();
                }
                tSession = SessionArray[i];
                tSession.Socket = null;
                tSession.dwReceiveTick = HUtil32.GetTickCount();
                tSession.SocketId = 0;
                tSession.ClientIP = string.Empty;
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
            var GateMsg = new MessageHeader();
            GateMsg.dwCode = Grobal2.RUNGATECODE;
            GateMsg.nSocket = nSocket;
            GateMsg.wGSocketIdx = (ushort)wSocketIndex;
            GateMsg.wIdent = nIdent;
            GateMsg.wUserListIndex = nUserListIndex;
            GateMsg.nLength = nLen;
            var sendBuffer = GateMsg.GetPacket();
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
    }
}