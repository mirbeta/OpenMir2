using System;
using SystemModule;
using SystemModule.Packages;
using SystemModule.ProtobuffPacket;
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
        private const int MaxSession = 5000;
        /// <summary>
        /// 用户会话
        /// </summary>
        public TSessionInfo[] SessionArray = new TSessionInfo[MaxSession];
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        public bool CheckServerFail = false;
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
        private int BuffLen = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool GateReady = false;
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool isConnected = false;
        /// <summary>
        /// 发送总字节数
        /// </summary>
        public int SendBytes;
        /// <summary>
        /// 接收总字节数
        /// </summary>
        public int ReceiveBytes;
        private int _checkRecviceTick = 0;
        private int _checkServerTick = 0;
        private int _checkServerTimeMin = 0;
        private int _checkServerTimeMax = 0;
        /// <summary>
        /// Session管理
        /// </summary>
        private SessionManager _sessionManager => SessionManager.Instance;
        /// <summary>
        /// 日志
        /// </summary>
        private LogQueue _logQueue => LogQueue.Instance;

        public ClientThread(int clientId, GameGateInfo gameGate)
        {
            ClientId = clientId;
            ClientSocket = new IClientScoket();
            ClientSocket.Host = gameGate.sServerAdress;
            ClientSocket.Port = gameGate.nServerPort;
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
            ReceiveBytes = 0;
            SendBytes = 0;
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
            GateReady = true;
            _checkServerTick = HUtil32.GetTickCount();
            _checkRecviceTick = HUtil32.GetTickCount();
            RestSessionArray();
            _checkServerTimeMax = 0;
            _checkServerTimeMax = 0;
            _logQueue.Enqueue($"[{ClientId}] 游戏引擎[{e.RemoteAddress}:{e.RemotePort}]链接成功.", 1);
            _logQueue.EnqueueDebugging($"线程[{Guid.NewGuid():N}]连接 {e.RemoteAddress}:{e.RemotePort} 成功...");
            isConnected = true;
            ReceiveBytes = 0;
            SendBytes = 0;
            ClientManager.Instance.AddClientThread(ClientId, this);
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
            GateReady = false;
            _logQueue.Enqueue($"[{ClientId}] 游戏引擎[{e.RemoteAddress}:{e.RemotePort}]断开链接.", 1);
            isConnected = false;
            ClientManager.Instance.DeleteClientThread(ClientId);
        }

        /// <summary>
        /// 收到GameSvr发来的消息
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            ProcReceiveBuffer(e.Buff, e.BuffLen);
            ReceiveBytes += e.BuffLen;
        }
        
        const int headerMessageSize = 20;
        
        private void ProcReceiveBuffer(byte[] Buffer, int nMsgLen)
        {
            MessageHeader mesgHeader;
            var buffIndex = 0;
            try
            {
                if (BuffLen > 0) //有未处理完成的buff
                {
                    var tempBuff = new byte[BuffLen + nMsgLen];
                    Array.Copy(SocketBuffer, 0, tempBuff, 0, BuffLen);
                    Array.Copy(Buffer, 0, tempBuff, BuffLen, Buffer.Length);
                    SocketBuffer = tempBuff;
                }
                else
                {
                    SocketBuffer = Buffer;
                }
                var nLen = BuffLen + nMsgLen;
                var Buff = SocketBuffer;
                if (nLen >= headerMessageSize)
                {
                    while (true)
                    {
                        mesgHeader = new MessageHeader(Buff);
                        if (mesgHeader.dwCode == 0)
                        {
                            Buff = Buff[20..];
                            buffIndex = 0;
                            nLen -= Math.Abs(mesgHeader.nLength) + headerMessageSize;
                            _logQueue.Enqueue("不应该出现这个文字", 5);
                            break;
                        }
                        if (mesgHeader.dwCode == Grobal2.RUNGATECODE)
                        {
                            if ((Math.Abs(mesgHeader.nLength) + headerMessageSize) > nLen)
                            {
                                break;
                            }
                            switch (mesgHeader.wIdent)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    CheckServerFail = false;
                                    _checkServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    var userSession = _sessionManager.GetSession(mesgHeader.wGSocketIdx);
                                    if (userSession != null)
                                    {
                                        userSession.m_nSvrListIdx = mesgHeader.wUserListIndex;
                                    }
                                    break;
                                case Grobal2.GM_RECEIVE_OK:
                                    _checkServerTimeMin = HUtil32.GetTickCount() - _checkRecviceTick;
                                    if (_checkServerTimeMin > _checkServerTimeMax)
                                    {
                                        _checkServerTimeMax = _checkServerTimeMin;
                                    }
                                    _checkRecviceTick = HUtil32.GetTickCount();
                                    SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, 0, "");
                                    break;
                                case Grobal2.GM_DATA:
                                    var msgBuff = mesgHeader.nLength > 0 ? new byte[mesgHeader.nLength] : new byte[Buff.Length - 20];
                                    Array.Copy(Buff, headerMessageSize, msgBuff, 0, msgBuff.Length);
                                    var message = new TMessageData();
                                    message.MessageId = mesgHeader.wGSocketIdx;
                                    message.Buffer = msgBuff;
                                    message.BufferLen = mesgHeader.nLength;
                                    _sessionManager.AddToQueue(message);
                                    break;
                                case Grobal2.GM_TEST:
                                    break;
                            }
                            nLen -= Math.Abs(mesgHeader.nLength) + headerMessageSize;
                            if (nLen <= 0)
                            {
                                break;
                            }
                            var newLen = headerMessageSize + Math.Abs(mesgHeader.nLength);
                            Buff = Buff[newLen..];
                            buffIndex = 0;
                        }
                        else
                        {
                            buffIndex++;
                            var messageBuff = new byte[Buff.Length - 1];
                            Array.Copy(Buff, buffIndex, messageBuff, 0, headerMessageSize);
                            Buff = messageBuff;
                            nLen -= 1;
                        }
                        if (nLen < headerMessageSize)
                        {
                            break;
                        }
                    }
                }
                if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    var tempBuff = new byte[nLen];
                    Array.Copy(Buff, 0, tempBuff, 0, nLen);
                    SocketBuffer = tempBuff;
                    BuffLen = nLen;
                }
                else
                {
                    SocketBuffer = null;
                    BuffLen = 0;
                }
            }
            catch (Exception E)
            {
                _logQueue.Enqueue($"[Exception] ProcReceiveBuffer BuffIndex:{buffIndex}", 5);
            }
        }
        
        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logQueue.Enqueue($"[{ClientId}] 游戏引擎[" + ClientSocket.Host + ":" + ClientSocket.Port + "]拒绝链接...", 1);
                    isConnected = false;
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logQueue.Enqueue($"[{ClientId}] 游戏引擎[" + ClientSocket.Host + ":" + ClientSocket.Port + "]关闭连接...", 1);
                    isConnected = false;
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logQueue.Enqueue($"[{ClientId}] 游戏引擎[" + ClientSocket.Host + ":" + ClientSocket.Port + "]链接超时...", 1);
                    isConnected = false;
                    break;
            }
            GateReady = false;
        }

        public void RestSessionArray()
        {
            for (var i = 0; i < MaxSession; i++)
            {
                if (SessionArray[i] == null)
                {
                    SessionArray[i] = new TSessionInfo();
                }
                var tSession = SessionArray[i];
                tSession.Socket = null;
                tSession.nUserListIndex = 0;
                tSession.dwReceiveTick = HUtil32.GetTickCount();
                tSession.SckHandle = 0;
                tSession.SessionId = 0;
            }
        }

        public void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, ushort nUserListIndex, int nLen,
            string Data)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var strBuff = HUtil32.GetBytes(Data);
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, strBuff);
            }
            else
            {
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, (byte[]) null);
            }
        }

        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        public void UserEnter(ushort wSocketIndex, int nSocket, string Data)
        {
            SendServerMsg(Grobal2.GM_OPEN, wSocketIndex, nSocket, 0, Data.Length, Data);
        }

        /// <summary>
        /// 玩家退出游戏
        /// </summary>
        public void UserLeave(int scoket)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, scoket, 0, 0, "");
        }

        private void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, ushort nUserListIndex, int nLen,
            byte[] Data)
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
                SendBuffer(tempBuff);
            }
            else
            {
                SendBuffer(sendBuffer);
            }
            /*var PactetMessage = new GateMessage();
            PactetMessage.dwCode = Grobal2.RUNGATECODE;
            PactetMessage.nSocket = nSocket;
            PactetMessage.wGSocketIdx = wSocketIndex;
            PactetMessage.wIdent = nIdent;
            PactetMessage.wUserListIndex = nUserListIndex;
            PactetMessage.nLength = nLen;
            if (Data is {Length: > 0})
            {
                PactetMessage.Data = Data;
                SendBuffer(PactetMessage);
            }
            else
            {
                SendBuffer(PactetMessage);
            }*/
        }

        /// <summary>
        /// 发送消息到GameSvr
        /// </summary>
        /// <param name="sendBuffer"></param>
        public void SendBuffer(byte[] sendBuffer)
        {
            if (!ClientSocket.IsConnected)
            {
                return;
            }
            SendBytes += sendBuffer.Length;
            ClientSocket.SendBuff(sendBuffer);
        }

        public void SendBuffer(IProtoBuff protoBuff)
        {
            if (!ClientSocket.IsConnected) {
                return;
            }
            var bodyBuffer = ProtobuffHelp.Serialize(protoBuff);
            SendBytes += bodyBuffer.Length;
            ClientSocket.SendBuff(bodyBuffer);
        }

        public void CheckServerIsTimeOut()
        {
            if ((HUtil32.GetTickCount() - _checkServerTick) > GateShare.dwCheckServerTimeOutTime && CheckServerFailCount <= 20)
            {
                CheckServerFail = true;
                Stop();
                CheckServerFailCount++;
                _logQueue.EnqueueDebugging($"服务器[{GetSocketIp()}]链接超时.失败次数:[{CheckServerFailCount}]");
            }
        }

        public void CheckTimeOutSession()
        {
            for (var j = 0; j < SessionArray.Length; j++)
            {
                var UserSession = SessionArray[j];
                if (UserSession.Socket != null)
                {
                    if ((HUtil32.GetTickCount() - UserSession.dwReceiveTick) > GateShare.dwSessionTimeOutTime)//清理超时用户会话 
                    {
                        UserSession.Socket.Close();
                        UserSession.Socket = null;
                        UserSession.SckHandle = -1;
                    }
                }
                _checkServerTimeMin = HUtil32.GetTickCount() - _checkServerTick;
                if (_checkServerTimeMax < _checkServerTimeMin)
                {
                    _checkServerTimeMax = _checkServerTimeMin;
                }
            }
        }
    }
}