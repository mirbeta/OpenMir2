using GameGate.Conf;
using System;
using System.Net;
using System.Threading.Tasks;
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
        private readonly ClientScoket ClientSocket;
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
        /// 独立Buffer分区
        /// </summary>
        private Memory<byte> receiveBuffer = null;
        /// <summary>
        /// 上次剩下多少字节未处理
        /// </summary>
        private int buffLen = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool GateReady = false;
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool Connected = false;
        /// <summary>
        /// 历史最高在线人数
        /// </summary>
        private int Counter = 0;
        /// <summary>
        /// 发送总字节数
        /// </summary>
        private int SendBytes;
        /// <summary>
        /// 接收总字节数
        /// </summary>
        private int ReceiveBytes;
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
            ClientSocket = new ClientScoket();
            ClientSocket.Host = gameGate.ServerAdress;
            ClientSocket.Port = gameGate.ServerPort;
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
            ReceiveBytes = 0;
            SendBytes = 0;
            _gateEndPoint = endPoint;
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
            if (Connected == false)
            {
                ClientSocket.Connect();
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
            if (count > Counter)
            {
                Counter = count;
            }
            return count + "/" + Counter;
        }

        public void Stop()
        {
            ClientSocket.Disconnect();
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
            Connected = true;
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
                    if (userSession.Socket != null && userSession.Socket == e.Socket)
                    {
                        userSession.Socket.Close();
                        userSession.Socket = null;
                        userSession.SckHandle = -1;
                    }
                }
            }
            RestSessionArray();
            receiveBuffer = null;
            GateReady = false;
            LogQueue.Enqueue($"[{_gateEndPoint.ToString()}] 游戏引擎[{e.RemoteEndPoint}]断开链接.", 1);
            Connected = false;
            ClientManager.Instance.DeleteClientThread(ClientId);
        }

        /// <summary>
        /// 收到GameSvr发来的消息
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var nMsgLen = e.BuffLen;
            //Span<byte> data = stackalloc byte[e.BuffLen];
            var data = e.Buff.AsSpan()[..e.BuffLen];
            var srcOffset = 0;
            try
            {
                if (buffLen > 0)
                {
                    Span<byte> tempBuff = stackalloc byte[buffLen + nMsgLen];
                    for (var i = 0; i < receiveBuffer.Length; i++)
                    {
                        tempBuff[i] = receiveBuffer.Span[i];
                    }

                    for (var i = 0; i < data.Length; i++)
                    {
                        tempBuff[i + buffLen] = data[i];
                    }

                    receiveBuffer = tempBuff.ToArray();
                }
                else
                {
                    receiveBuffer = data.ToArray();
                }

                var nLen = buffLen + nMsgLen;
                var dataBuff = receiveBuffer;
                if (nLen >= HeaderMessageSize)
                {
                    while (true)
                    {
                        var packetHead = dataBuff.Span;
                        var PacketCode = BitConverter.ToUInt32(packetHead.Slice(0, 4));
                        var Socket = BitConverter.ToInt32(packetHead.Slice(4, 4));
                        var SessionId = BitConverter.ToUInt16(packetHead.Slice(8, 2));
                        var Ident = BitConverter.ToUInt16(packetHead.Slice(10, 2));
                        var ServerIndex = BitConverter.ToInt32(packetHead.Slice(12, 4));
                        var PackLength = BitConverter.ToInt32(packetHead.Slice(16, 4));
                        //var packetHeader = Packets.ToPacket<PacketHeader>(dataBuff.ToArray()); //这里还是有影响，看看能否直接读取内存，不做序列化操作
                        if (PacketCode == 0)
                        {
                            LogQueue.Enqueue("不应该出现这个文字", 5);
                            break;
                        }
                        if (PacketCode == Grobal2.RUNGATECODE)
                        {
                            var nCheckMsgLen = (Math.Abs(PackLength) + HeaderMessageSize);
                            if (nCheckMsgLen > nLen)
                            {
                                break;
                            }
                            switch (Ident)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    CheckServerFail = false;
                                    _checkServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    var userSession = SessionManager.GetSession(SessionId);
                                    if (userSession != null)
                                    {
                                        userSession.m_nSvrListIdx = ServerIndex;
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
                                    var message = new MessageData();
                                    message.MessageId = SessionId;
                                    if (PackLength > 0)
                                    {
                                        message.Buffer = dataBuff.Slice(20, PackLength);
                                    }
                                    else
                                    {
                                        var packetSize = dataBuff.Length - HeaderMessageSize;
                                        message.Buffer = dataBuff.Slice(20, packetSize);
                                    }
                                    message.BufferLen = PackLength;
                                    SessionManager.AddToQueue(message);
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
                            buffLen = nLen;
                            srcOffset = 0;
                        }
                        else
                        {
                            srcOffset++;
                            dataBuff = dataBuff.Slice(srcOffset, HeaderMessageSize);
                            nLen -= 1;
                            LogQueue.EnqueueDebugging("看到这行字也有点问题.");
                        }
                        if (nLen < HeaderMessageSize)
                        {
                            break;
                        }
                    }
                }
                if (nLen > 0) //有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    receiveBuffer = dataBuff[..nLen].ToArray();
                    buffLen = nLen;
                }
                else
                {
                    receiveBuffer = null;
                    buffLen = 0;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Enqueue($"[Exception] ProcReceiveBuffer BuffIndex:{srcOffset}", 5);
            }
            ReceiveBytes += e.BuffLen;
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    LogQueue.Enqueue($"游戏网关[{_gateEndPoint}]链接游戏引擎[{ClientSocket.EndPoint}]拒绝链接...", 1);
                    Connected = false;
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    LogQueue.Enqueue($"游戏引擎[{ClientSocket.EndPoint}]主动关闭连接游戏网关[{_gateEndPoint.ToString()}]...", 1);
                    Connected = false;
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    LogQueue.Enqueue($"游戏网关[{_gateEndPoint}]链接游戏引擎时[{ClientSocket.EndPoint}]超时...", 1);
                    Connected = false;
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
        public void UserEnter(ushort socketIndex, int nSocket, string Data)
        {
            SendServerMsg(Grobal2.GM_OPEN, socketIndex, nSocket, 0, Data.Length, Data);
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
            var GateMsg = new PacketHeader();
            GateMsg.PacketCode = Grobal2.RUNGATECODE;
            GateMsg.Socket = nSocket;
            GateMsg.SessionId = wSocketIndex;
            GateMsg.Ident = nIdent;
            GateMsg.ServerIndex = nUserListIndex;
            GateMsg.PackLength = nLen;
            var sendBuffer = GateMsg.GetBuffer();
            if (Data is { Length: > 0 })
            {
                var tempBuff = new byte[20 + Data.Length];
                Buffer.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
                Buffer.BlockCopy(Data, 0, tempBuff, sendBuffer.Length, Data.Length);
                SendBuffer(tempBuff);
            }
            SendBuffer(sendBuffer);
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
            if ((HUtil32.GetTickCount() - _checkServerTick) > GateShare.CheckServerTimeOutTime && CheckServerFailCount <= 20)
            {
                CheckServerFail = true;
                Stop();
                CheckServerFailCount++;
                LogQueue.EnqueueDebugging($"服务器[{GetSocketIp()}]链接超时.失败次数:[{CheckServerFailCount}]");
            }
        }

        public void CheckTimeOutSession()
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
        
        public string GetConnected()
        {
            return IsConnected ? "[green]Connected[/]" : "[red]Not Connected[/]";
        }

        public string GetSendInfo()
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

        public string GetReceiveInfo()
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

    }
}