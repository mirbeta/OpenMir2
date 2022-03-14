using System;
using System.Diagnostics;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace GameGate
{
    /// <summary>
    /// 网关客户端(GameGate-GameSvr)
    /// </summary>
    public class ClientThread
    {
        private IClientScoket ClientSocket;
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
        /// <summary>
        /// 发送总字节数
        /// </summary>
        public int SendBytes;
        /// <summary>
        /// 接受总字节数
        /// </summary>
        public int ReceiveBytes;
        /// <summary>
        /// Session管理
        /// </summary>
        private readonly SessionManager _sessionManager;
        private readonly LogQueue _logQueue;

        public ClientThread(int clientId, string serverAddr, int serverPort, SessionManager sessionManager, LogQueue logQueue)
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
            _logQueue = logQueue;
        }

        public bool IsConnected => isConnected;

        public string GetSocketIp()
        {
            return $"{ClientSocket.Address}:{ClientSocket.Port}";
        }

        private string GetSocketPort()
        {
            return $"{ClientSocket.Port}";
        }

        private string GetSocketAddress()
        {
            return $"{ClientSocket.Address}";
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

        public string GetSessionCount()
        {
            var count = 0;
            for (int i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i].Socket != null)
                {
                    count++;
                }
            }
            return count + "/" + count;
        }

        public (string serverIp, string serverPort, string Status, string playCount, string reviceTotal, string sendTotal) GetStatus()
        {
            return (GetSocketAddress(), GetSocketPort(), GetConnected(), GetSessionCount(), GetSendInfo(), GetReceiveInfo());
        }

        internal string GetConnected() {
            return IsConnected ? $"[green]已连接[/]" : $"[red]Not Connected[/]";
        }

        public string GetSendInfo()
        {
            var totalStr = string.Empty;
            if (SendBytes > (1024 * 1000))
            {
                totalStr= $"↑{SendBytes / (1024 * 1000)}M";
            }
            else if (SendBytes > 1024)
            {
                totalStr = $"↑{SendBytes / 1024}K";
            }
            else
            {
                totalStr = $"↑{SendBytes}B";
            }
            SendBytes = 0;
            return totalStr;
        }

        public string GetReceiveInfo()
        {
            var totalStr = string.Empty;
            if (ReceiveBytes > (1024 * 1000))
            {
                totalStr = $"↓{ReceiveBytes / (1024 * 1000)}M";
            }
            else if (ReceiveBytes > 1024)
            {
                totalStr = $"↓{ReceiveBytes / 1024}K";
            }
            else
            {
                totalStr = $"↓{ReceiveBytes}B";
            }
            ReceiveBytes = 0;
            return totalStr;
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
            GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
            RestSessionArray();
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.ServerGateList.Add(this);
            _logQueue.Enqueue($"游戏引擎[{e.RemoteAddress}:{e.RemotePort}]链接成功.", 1);
            Debug.WriteLine($"线程[{Guid.NewGuid():N}]连接 {e.RemoteAddress}:{e.RemotePort} 成功...");
            isConnected = true;
            ReceiveBytes = 0;
            SendBytes = 0;
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
                    userSession.SckHandle = -1;
                }
            }
            RestSessionArray();
            SocketBuffer = null;
            boGateReady = false;
            GateShare.ServerGateList.Remove(this);
            _logQueue.Enqueue($"游戏引擎[{e.RemoteAddress}:{e.RemotePort}]断开链接.", 1);
            isConnected = false;
        }

        /// <summary>
        /// 收到GameSvr发来的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            ProcReceiveBuffer(e.Buff, e.BuffLen);
            ReceiveBytes += e.BuffLen;
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logQueue.Enqueue("游戏引擎[" + ClientSocket.Address + ":" + ClientSocket.Port + "]拒绝链接...", 1);
                    isConnected = false;
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logQueue.Enqueue("游戏引擎[" + ClientSocket.Address + ":" + ClientSocket.Port + "]关闭连接...", 1);
                    isConnected = false;
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logQueue.Enqueue("游戏引擎[" + ClientSocket.Address + ":" + ClientSocket.Port + "]链接超时...", 1);
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
                tSession.nUserListIndex = 0;
                tSession.dwReceiveTick = HUtil32.GetTickCount();
                tSession.SckHandle = 0;
                tSession.SessionId = 0;
            }
        }

        private void SendServerMsg(ForwardMessage message)
        {
            SendServerMsg(message.nIdent, message.wSocketIndex, message.nSocket, message.nUserListIndex, message.nLen, message.Data);
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

        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        public void UserEnter(int wSocketIndex, int nSocket, string Data)
        {
            SendServerMsg(Grobal2.GM_OPEN,  wSocketIndex, nSocket, 0, Data.Length, Data);
        }

        /// <summary>
        /// 玩家退出游戏
        /// </summary>
        public void UserLeave(int scoket)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, scoket, 0, 0, ""); 
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

        private void ProcReceiveBuffer(byte[] tBuffer, int nMsgLen)
        {
            MessageHeader mesgHeader;
            var BuffIndex = 0;
            const int HeaderMessageSize = 20;
            try
            {
                if (nBuffLen > 0) //有未处理完成的buff
                {
                    var tempBuff = new byte[nBuffLen + nMsgLen];
                    Array.Copy(SocketBuffer, 0, tempBuff, 0, nBuffLen);
                    Array.Copy(tBuffer, 0, tempBuff, nBuffLen, tBuffer.Length);
                    SocketBuffer = tempBuff;
                }
                else
                {
                    SocketBuffer = tBuffer;
                }
                var nLen = nBuffLen + nMsgLen;
                var Buff = SocketBuffer;
                if (nLen >= HeaderMessageSize)
                {
                    while (true)
                    {
                        mesgHeader = new MessageHeader(Buff);
                        if (mesgHeader.dwCode == 0)
                        {
                            break;
                        }
                        if (mesgHeader.dwCode == Grobal2.RUNGATECODE)
                        {
                            if ((Math.Abs(mesgHeader.nLength) + HeaderMessageSize) > nLen)
                            {
                                break;
                            }
                            switch (mesgHeader.wIdent)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    boCheckServerFail = false;
                                    GateShare.dwCheckServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    var userSession = _sessionManager.GetSession(mesgHeader.wGSocketIdx);
                                    if (userSession != null)
                                    {
                                        userSession.m_nSvrListIdx = mesgHeader.wUserListIndex;
                                    }
                                    break;
                                case Grobal2.GM_RECEIVE_OK:
                                    GateShare.dwCheckServerTimeMin = HUtil32.GetTickCount() - GateShare.dwCheckRecviceTick;
                                    if (GateShare.dwCheckServerTimeMin > GateShare.dwCheckServerTimeMax)
                                    {
                                        GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                                    }
                                    GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
                                    SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, 0, "");
                                    break;
                                case Grobal2.GM_DATA:
                                    var msgBuff = mesgHeader.nLength > 0 ? new byte[mesgHeader.nLength] : new byte[Buff.Length - 20];
                                    Array.Copy(Buff, HeaderMessageSize, msgBuff, 0, msgBuff.Length);
                                    var message = new TMessageData();
                                    message.MessageId = mesgHeader.wGSocketIdx;
                                    message.Buffer = msgBuff;
                                    message.DataLen = mesgHeader.nLength;
                                    _sessionManager.SendQueue.TryWrite(message);
                                    break;
                                case Grobal2.GM_TEST:
                                    break;
                            }
                            var newLen = HeaderMessageSize + Math.Abs(mesgHeader.nLength);
                            var tempBuff = new byte[Buff.Length - newLen];
                            Array.Copy(Buff, newLen, tempBuff, 0, tempBuff.Length);
                            Buff = tempBuff;
                            BuffIndex = 0;
                            nLen -= Math.Abs(mesgHeader.nLength) + HeaderMessageSize;
                        }
                        else
                        {
                            BuffIndex++;
                            var messageBuff = new byte[Buff.Length - 1];
                            Array.Copy(Buff, BuffIndex, messageBuff, 0, HeaderMessageSize);
                            Buff = messageBuff;
                            nLen -= 1;
                        }
                        if (nLen < HeaderMessageSize)
                        {
                            break;
                        }
                    }
                }
                if (nLen > 0)
                {
                    var tempBuff = new byte[nLen];
                    Array.Copy(Buff, 0, tempBuff, 0, nLen);
                    SocketBuffer = tempBuff;
                    nBuffLen = nLen;
                }
                else
                {
                    SocketBuffer = null;
                    nBuffLen = 0;
                }
            }
            catch (Exception E)
            {
                _logQueue.Enqueue($"[Exception] ProcReceiveBuffer BuffIndex:{BuffIndex}", 1);
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
                SendBytes += sendBuffer.Length;
            }
        }
    }

    public class ForwardMessage
    {
        public ushort nIdent;
        public ushort wSocketIndex;
        public int nSocket;
        public ushort nUserListIndex;
        public int nLen;
        public byte[] Data;
    }
}