using GameGate.Conf;
using System;
using System.Net;
using SystemModule;
using SystemModule.Packet.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameGate.Services
{
    /// <summary>
    /// 网关客户端(GameGate-GameSvr)
    /// </summary>
    public class ClientThread
    {
        private readonly AsyncClientSocket _clientSocket;
        /// <summary>
        /// 网关ID
        /// </summary>
        public readonly string ClientId;
        private readonly IPEndPoint _gateEndPoint;
        /// <summary>
        /// 用户会话
        /// </summary>
        public readonly SessionInfo[] SessionArray = new SessionInfo[GateShare.MaxSession];
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        public bool CheckServerFail = false;
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 0;
        /// <summary>
        /// Buffer
        /// </summary>
        private Memory<byte> _receiveBuffer = null;
        /// <summary>
        /// 上次剩下多少字节未处理
        /// </summary>
        private int _buffLen = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool GateReady = false;
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool _connected = false;
        /// <summary>
        /// 历史最高在线人数
        /// </summary>
        private int _counter = 0;
        /// <summary>
        /// 发送总字节数
        /// </summary>
        private int _sendBytes;
        /// <summary>
        /// 接收总字节数
        /// </summary>
        private int _receiveBytes;
        private int _checkRecviceTick = 0;
        private int _checkServerTick = 0;
        private int _checkServerTimeMin = 0;
        private int _checkServerTimeMax = 0;
        /// <summary>
        /// Session管理
        /// </summary>
        private static SessionManager SessionManager => SessionManager.Instance;
        /// <summary>
        /// 日志
        /// </summary>
        private static MirLog LogQueue => MirLog.Instance;
        private const int HeaderMessageSize = 20;

        public ClientThread(string clientId, IPEndPoint endPoint, GameGateInfo gameGate)
        {
            ClientId = clientId;
            _clientSocket = new AsyncClientSocket(gameGate.ServerAdress, gameGate.ServerPort, 512);
            _clientSocket.OnConnected += ClientSocketConnect;
            _clientSocket.OnDisconnected += ClientSocketDisconnect;
            _clientSocket.OnReceivedData += ClientSocketRead;
            _clientSocket.OnError += ClientSocketError;
            _receiveBytes = 0;
            _sendBytes = 0;
            _gateEndPoint = endPoint;
        }

        public bool IsConnected => _clientSocket.IsConnected;

        public string EndPoint => $"{_clientSocket.Host}:{_clientSocket.Port}";

        public void Start()
        {
            _clientSocket.Start();
        }

        public void ReConnected()
        {
            if (_connected == false)
            {
                _clientSocket.Start();
            }
        }

        public string GetSessionCount()
        {
            var count = 0;
            for (var i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
                {
                    count++;
                }
            }
            if (count > _counter)
            {
                _counter = count;
            }
            return count + "/" + _counter;
        }

        public void Stop()
        {
           // ClientSocket.Disconnect();
        }

        public SessionInfo[] GetSession()
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
            LogQueue.Enqueue($"[{_gateEndPoint.ToString()}] 游戏引擎[{e.RemoteEndPoint}]链接成功.", 1);
            LogQueue.EnqueueDebugging($"线程[{Guid.NewGuid():N}]连接 {e.RemoteEndPoint} 成功...");
            _connected = true;
            _receiveBytes = 0;
            _sendBytes = 0;
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                var userSession = SessionArray[i];
                if (userSession != null)
                {
                    if (userSession.Socket != null && userSession.Socket == e.Socket)
                    {
                        userSession.Socket.Close();
                        userSession.Socket = null;
                        userSession.SckHandle = -1;
                    }
                }
            }
            RestSessionArray();
            _receiveBuffer = null;
            GateReady = false;
            LogQueue.Enqueue($"[{_gateEndPoint.ToString()}] 游戏引擎[{e.RemoteEndPoint}]断开链接.", 1);
            _connected = false;
            CheckServerFail = true;
        }

        /// <summary>
        /// 接收GameSvr发来的封包消息
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var nMsgLen = e.BuffLen;
            Memory<byte> packetData = e.Buff[..nMsgLen];
            var srcOffset = 0;
            try
            {
                if (_buffLen > 0)
                {
                    Span<byte> tempBuff = stackalloc byte[_buffLen + nMsgLen];
                    //HUtil32.MemoryCopy(receiveBuffer.Span, tempBuff, 0, buffLen);
                    //HUtil32.MemoryCopy(packetData, tempBuff, buffLen, nMsgLen);
                    //receiveBuffer = tempBuff.ToArray();
                    //TODO 效率慢，直接Copy数组
                    for (int i = 0; i < _receiveBuffer.Length; i++)
                    {
                        tempBuff[i] = _receiveBuffer.Span[i];
                    }
                    for (int i = 0; i < packetData.Length; i++)
                    {
                        tempBuff[i + _buffLen] = packetData.Span[i];
                    }
                    _receiveBuffer = tempBuff.ToArray();
                }
                else
                {
                    _receiveBuffer = packetData;
                }
                var nLen = _buffLen + nMsgLen;
                Memory<byte> dataBuff = _receiveBuffer;
                while (nLen >= PacketHeader.PacketSize)
                {
                    Span<byte> packetHead = dataBuff[..20].Span;
                    var packetCode = BitConverter.ToUInt32(packetHead[..4]);
                    if (packetCode == 0 || packetCode != Grobal2.RUNGATECODE)
                    {
                        srcOffset++;
                        dataBuff = dataBuff.Slice(srcOffset, HeaderMessageSize);
                        nLen -= 1;
                        LogQueue.EnqueueDebugging($"解析封包出现异常封包，封包长度:[{dataBuff.Length}] 偏移:[{srcOffset}].");
                        continue;
                    }
                    //var Socket = BitConverter.ToInt32(packetHead.Slice(4, 4));
                    var sessionId = BitConverter.ToUInt16(packetHead.Slice(8, 2));
                    var ident = BitConverter.ToUInt16(packetHead.Slice(10, 2));
                    var serverIndex = BitConverter.ToInt32(packetHead.Slice(12, 4));
                    var packLength = BitConverter.ToInt32(packetHead.Slice(16, 4));
                    var nCheckMsgLen = Math.Abs(packLength) + HeaderMessageSize;
                    if (nCheckMsgLen > nLen)
                    {
                        break;
                    }
                    switch (ident)
                    {
                        case Grobal2.GM_CHECKSERVER:
                            CheckServerFail = false;
                            _checkServerTick = HUtil32.GetTickCount();
                            break;
                        case Grobal2.GM_SERVERUSERINDEX:
                            var userSession = SessionManager.GetSession(sessionId);
                            if (userSession != null)
                            {
                                userSession.m_nSvrListIdx = serverIndex;
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
                            var sessionPacket = new ClientSessionPacket
                            {
                                SessionId = sessionId,
                                BufferLen = packLength,
                                Buffer = dataBuff
                            };
                            //todo Memory的切片速度比Span切片速度要慢，后续看看有没有什么办法优化
                            if (packLength > 0)
                            {
                                sessionPacket.Buffer = dataBuff.Slice(20, packLength);
                            }
                            else
                            {
                                var packetSize = dataBuff.Length - HeaderMessageSize;
                                sessionPacket.Buffer = dataBuff.Slice(20, packetSize);
                            }
                            SessionManager.Enqueue(sessionPacket);
                            break;
                        case Grobal2.GM_TEST:
                            break;
                    }
                    nLen -= nCheckMsgLen;
                    if (nLen <= 0)
                    {
                        break;
                    }
                    dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
                    _buffLen = nLen;
                    srcOffset = 0;
                    if (nLen < HeaderMessageSize)
                    {
                        break;
                    }
                }
                if (nLen > 0) //有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    _receiveBuffer = dataBuff[..nLen];
                    _buffLen = nLen;
                }
                else
                {
                    _receiveBuffer = null;
                    _buffLen = 0;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Enqueue($"[Exception] ProcReceiveBuffer BuffIndex:{srcOffset}", 5);
            }
            _receiveBytes += e.BuffLen;
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    LogQueue.Enqueue($"游戏网关[{_gateEndPoint}]链接游戏引擎[{_clientSocket.EndPoint}]拒绝链接...", 1);
                    _connected = false;
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    LogQueue.Enqueue($"游戏引擎[{_clientSocket.EndPoint}]主动关闭连接游戏网关[{_gateEndPoint.ToString()}]...", 1);
                    _connected = false;
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    LogQueue.Enqueue($"游戏网关[{_gateEndPoint}]链接游戏引擎时[{_clientSocket.EndPoint}]超时...", 1);
                    _connected = false;
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
            string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var strBuff = HUtil32.GetBytes(data);
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
        public void UserEnter(ushort socketIndex, int nSocket, string data)
        {
            SendServerMsg(Grobal2.GM_OPEN, socketIndex, nSocket, 0, data.Length, data);
        }

        /// <summary>
        /// 玩家退出游戏
        /// </summary>
        public void UserLeave(int scoket)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, scoket, 0, 0, "");
        }

        private void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, ushort nUserListIndex, int nLen,
            byte[] data)
        {
            var gateMsg = new PacketHeader
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = nSocket,
                SessionId = wSocketIndex,
                Ident = nIdent,
                ServerIndex = nUserListIndex,
                PackLength = nLen
            };
            var sendBuffer = gateMsg.GetBuffer();
            if (data is { Length: > 0 })
            {
                var tempBuff = new byte[20 + data.Length];
                Buffer.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
                Buffer.BlockCopy(data, 0, tempBuff, sendBuffer.Length, data.Length);
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
            if (!_clientSocket.IsConnected)
            {
                return;
            }
            _sendBytes += sendBuffer.Length;
            _clientSocket.SendMessage(sendBuffer);
        }
        
        /// <summary>
        /// 处理超时或空闲会话
        /// </summary>
        public void ProcessIdleSession()
        {
            for (var j = 0; j < SessionArray.Length; j++)
            {
                var userSession = SessionArray[j];
                if (userSession != null && userSession.Socket != null)
                {
                    if ((HUtil32.GetTickCount() - userSession.dwReceiveTick) > GateShare.SessionTimeOutTime) //清理超时用户会话 
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

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        public void CheckConnectedState()
        {
            if (GateReady)
            {
                SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, 0, "");
                CheckServerFailCount = 0;
                return;
            }
            if (CheckServerFail && CheckServerFailCount <= 20)
            {
                ReConnected();
                CheckServerFailCount++;
                LogQueue.EnqueueDebugging($"重新与服务器[{EndPoint}]建立链接.失败次数:[{CheckServerFailCount}]");
                return;
            }
            CheckServerTimeOut();
        }

        private void CheckServerTimeOut()
        {
            if ((HUtil32.GetTickCount() - _checkServerTick) > GateShare.CheckServerTimeOutTime && CheckServerFailCount <= 20)
            {
                CheckServerFail = true;
                Stop();
                CheckServerFailCount++;
                LogQueue.EnqueueDebugging($"服务器[{EndPoint}]长时间没有回应,断开链接.失败次数:[{CheckServerFailCount}]");
            }
        }
        
        public string GetConnected()
        {
            return IsConnected ? "[green]Connected[/]" : "[red]Not Connected[/]";
        }

        public string GetSendInfo()
        {
            var sendStr = _sendBytes switch
            {
                > 1024 * 1000 => $"↑{_sendBytes / (1024 * 1000)}M",
                > 1024 => $"↑{_sendBytes / 1024}K",
                _ => $"↑{_sendBytes}B"
            };
            _sendBytes = 0;
            return sendStr;
        }

        public string GetReceiveInfo()
        {
            var receiveStr = _receiveBytes switch
            {
                > 1024 * 1000 => $"↓{_receiveBytes / (1024 * 1000)}M",
                > 1024 => $"↓{_receiveBytes / 1024}K",
                _ => $"↓{_receiveBytes}B"
            };
            _receiveBytes = 0;
            return receiveStr;
        }

    }
}