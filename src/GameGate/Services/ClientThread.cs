using System;
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
        /// 用户会话
        /// </summary>
        public TSessionInfo[] SessionArray = new TSessionInfo[GateShare.MaxSession];
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
        private byte[] ReceiveBuffer = null;
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
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
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
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                var userSession = SessionArray[i];
                if (userSession != null)
                {
                    if (userSession.Socket != null && userSession.Socket == e.socket)
                    {
                        userSession.Socket.Close();
                        userSession.Socket = null;
                        userSession.SckHandle = -1;
                    }
                }
            }
            RestSessionArray();
            ReceiveBuffer = null;
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

        private const int HeaderMessageSize = 20;

        private void ProcReceiveBuffer(byte[] data, int nMsgLen)
        {
            var buffIndex = 0;
            try
            {
                if (BuffLen > 0)
                {
                    var tempBuff = new byte[BuffLen + nMsgLen];
                    Buffer.BlockCopy(ReceiveBuffer, 0, tempBuff, 0, BuffLen);
                    Buffer.BlockCopy(data, 0, tempBuff, BuffLen, data.Length);
                    ReceiveBuffer = tempBuff;
                }
                else
                {
                    ReceiveBuffer = data;
                }
                var nLen = BuffLen + nMsgLen;
                var dataBuff = ReceiveBuffer;
                if (nLen >= HeaderMessageSize)
                {
                    while (true)
                    {
                        var mesgHeader = new MessageHeader(dataBuff);
                        if (mesgHeader.PacketCode == 0)
                        {
                            _logQueue.Enqueue("不应该出现这个文字", 5);
                            break;
                        }
                        if (mesgHeader.PacketCode == Grobal2.RUNGATECODE)
                        {
                            if ((Math.Abs(mesgHeader.nLength) + HeaderMessageSize) > nLen)
                            {
                                break;
                            }
                            var nCheckMsgLen = Math.Abs(mesgHeader.nLength) + HeaderMessageSize;
                            switch (mesgHeader.wIdent)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    CheckServerFail = false;
                                    _checkServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    var userSession = _sessionManager.GetSession(mesgHeader.SocketIdx);
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
                                    var msgBuff = mesgHeader.nLength > 0 ? new byte[mesgHeader.nLength] : new byte[dataBuff.Length - HeaderMessageSize];
                                    Buffer.BlockCopy(dataBuff, HeaderMessageSize, msgBuff, 0, msgBuff.Length);
                                    var message = new TMessageData();
                                    message.MessageId = mesgHeader.SocketIdx;
                                    message.Buffer = msgBuff;
                                    message.BufferLen = mesgHeader.nLength;
                                    _sessionManager.AddToQueue(message);
                                    break;
                                case Grobal2.GM_TEST:
                                    break;
                            }
                            nLen -= nCheckMsgLen;
                            if (nLen <= 0)
                            {
                                break;
                            }
                            var tempBuff = new byte[nLen];
                            Buffer.BlockCopy(ReceiveBuffer, nCheckMsgLen, tempBuff, 0, nLen);
                            ReceiveBuffer = tempBuff;
                            dataBuff = tempBuff;
                            BuffLen = nLen;
                            buffIndex = 0;
                        }
                        else
                        {
                            buffIndex++;
                            var messageBuff = new byte[dataBuff.Length - 1];
                            Buffer.BlockCopy(dataBuff, buffIndex, messageBuff, 0, HeaderMessageSize);
                            dataBuff = messageBuff;
                            nLen -= 1;
                            _logQueue.EnqueueDebugging("看到这行字也有点问题.");
                        }
                        if (nLen < HeaderMessageSize)
                        {
                            break;
                        }
                    }
                }
                if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    var tempBuff = new byte[nLen];
                    Buffer.BlockCopy(dataBuff, 0, tempBuff, 0, nLen);
                    ReceiveBuffer = tempBuff;
                    BuffLen = nLen;
                }
                else
                {
                    ReceiveBuffer = null;
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
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].Socket = null;
                    SessionArray[i].nUserListIndex = 0;
                    SessionArray[i].dwReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].SckHandle = 0;
                    SessionArray[i].SessionId = 0;
                }
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
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, (byte[])null);
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
            GateMsg.PacketCode = Grobal2.RUNGATECODE;
            GateMsg.Socket = nSocket;
            GateMsg.SocketIdx = wSocketIndex;
            GateMsg.wIdent = nIdent;
            GateMsg.wUserListIndex = nUserListIndex;
            GateMsg.nLength = nLen;
            var sendBuffer = GateMsg.GetPacket();
            if (Data is { Length: > 0 })
            {
                var tempBuff = new byte[20 + Data.Length];
                Buffer.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
                Buffer.BlockCopy(Data, 0, tempBuff, sendBuffer.Length, Data.Length);
                SendBuffer(tempBuff);
            }
            else
            {
                SendBuffer(sendBuffer);
            }
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
            ClientSocket.Send(sendBuffer);
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
                var userSession = SessionArray[j];
                if (userSession != null && userSession.Socket != null)
                {
                    if ((HUtil32.GetTickCount() - userSession.dwReceiveTick) > GateShare.dwSessionTimeOutTime) //清理超时用户会话 
                    {
                        userSession.Socket.Close();
                        userSession.Socket = null;
                        userSession.SckHandle = -1;
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