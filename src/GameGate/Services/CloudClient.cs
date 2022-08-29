using GameGate.Conf;
using System;
using System.Net;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameGate.Services
{
    /// <summary>
    /// 网关客户端(GameGate-CloudGate)
    /// </summary>
    public class CloudClient
    {
        private readonly ClientScoket _clientSocket;
        private readonly IPEndPoint _gateEndPoint;
        /// <summary>
        ///  网关游戏与云网关之间检测是否失败（超时）
        /// </summary>
        public bool CheckServerFail = false;
        /// <summary>
        /// 网关游戏与云网关之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 0;
        /// <summary>
        /// 独立Buffer分区
        /// </summary>
        private byte[] _receiveBuffer = null;
        /// <summary>
        /// 上次剩下多少字节未处理
        /// </summary>
        private int _buffLen = 0;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool GateReady = false;
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

        public CloudClient(IPEndPoint endPoint, GameGateInfo gameGate)
        {
            _clientSocket = new ClientScoket();
            _clientSocket.Host = gameGate.ServerAdress;
            _clientSocket.Port = gameGate.ServerPort;
            _clientSocket.OnConnected += ClientSocketConnect;
            _clientSocket.OnDisconnected += ClientSocketDisconnect;
            _clientSocket.ReceivedDatagram += ClientSocketRead;
            _clientSocket.OnError += ClientSocketError;
            _receiveBytes = 0;
            _sendBytes = 0;
            _gateEndPoint = endPoint;
        }

        public bool IsConnected => _clientSocket.IsConnected;

        public string GetSocketIp()
        {
            return $"{_clientSocket.Host}:{_clientSocket.Port}";
        }

        public void Start()
        {
            _clientSocket.Connect();
        }

        public void ReConnected()
        {
            if (GateReady == false)
            {
                _clientSocket.Connect();
            }
        }

        public void Stop()
        {
            _clientSocket.Disconnect();
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateReady = true;
            _checkServerTick = HUtil32.GetTickCount();
            _checkRecviceTick = HUtil32.GetTickCount();
            _checkServerTimeMax = 0;
            _checkServerTimeMax = 0;
            LogQueue.Enqueue($"[{_gateEndPoint.ToString()}] 云网关智能反外挂服务器[{e.RemoteEndPoint}]链接成功.", 1);
            LogQueue.EnqueueDebugging($"线程[{Guid.NewGuid():N}]云网关智能反外挂服务器连接 {e.RemoteEndPoint} 成功...");
            LogQueue.Enqueue("智能反外挂程序云端已连接...", 0);
            _receiveBytes = 0;
            _sendBytes = 0;
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            _receiveBuffer = null;
            GateReady = false;
            LogQueue.Enqueue($"[{_gateEndPoint.ToString()}] 云网关智能反外挂服务器[{e.RemoteEndPoint}]断开链接.", 1);
        }

        /// <summary>
        /// 收到GameSvr发来的消息
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            ProcReceiveBuffer(e.Buff, e.BuffLen);
            _receiveBytes += e.BuffLen;
        }

        private const int HeaderMessageSize = 20;

        private void ProcReceiveBuffer(byte[] data, int nMsgLen)
        {
            var srcOffset = 0;
            try
            {
                if (_buffLen > 0)
                {
                    var tempBuff = new byte[_buffLen + nMsgLen];
                    Buffer.BlockCopy(_receiveBuffer, 0, tempBuff, 0, _buffLen);
                    Buffer.BlockCopy(data, 0, tempBuff, _buffLen, data.Length);
                    _receiveBuffer = tempBuff;
                }
                else
                {
                    _receiveBuffer = data;
                }
                var nLen = _buffLen + nMsgLen;
                var dataBuff = _receiveBuffer;
                if (nLen >= HeaderMessageSize)
                {
                    while (true)
                    {
                        var packetHeader = Packets.ToPacket<PacketHeader>(dataBuff);
                        if (packetHeader.PacketCode == 0)
                        {
                            LogQueue.Enqueue("不应该出现这个文字", 5);
                            break;
                        }
                        if (packetHeader.PacketCode == Grobal2.RUNGATECODE)
                        {
                            var nCheckMsgLen = (Math.Abs(packetHeader.PackLength) + HeaderMessageSize);
                            if (nCheckMsgLen > nLen)
                            {
                                break;
                            }
                            switch (packetHeader.Ident)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    CheckServerFail = false;
                                    _checkServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    var userSession = SessionManager.GetSession(packetHeader.SessionId);
                                    if (userSession != null)
                                    {
                                        userSession.m_nSvrListIdx = packetHeader.ServerIndex;
                                    }
                                    break;
                                case Grobal2.GM_RECEIVE_OK:
                                    _checkServerTimeMin = HUtil32.GetTickCount() - _checkRecviceTick;
                                    if (_checkServerTimeMin > _checkServerTimeMax)
                                    {
                                        _checkServerTimeMax = _checkServerTimeMin;
                                    }
                                    _checkRecviceTick = HUtil32.GetTickCount();
                                    SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, "", 0, 0, "");
                                    break;
                                case Grobal2.GM_DATA:
                                    var msgBuff = packetHeader.PackLength > 0 ? new byte[packetHeader.PackLength] : new byte[dataBuff.Length - HeaderMessageSize];
                                    Buffer.BlockCopy(dataBuff, HeaderMessageSize, msgBuff, 0, msgBuff.Length);
                                    //todo 根据返回云网关数据进行操作,是放行还是是否触发了策略限制
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
                            Buffer.BlockCopy(_receiveBuffer, nCheckMsgLen, tempBuff, 0, nLen);
                            _receiveBuffer = tempBuff;
                            dataBuff = tempBuff;
                            _buffLen = nLen;
                            srcOffset = 0;
                        }
                        else
                        {
                            srcOffset++;
                            var messageBuff = new byte[dataBuff.Length - 1];
                            Buffer.BlockCopy(dataBuff, srcOffset, messageBuff, 0, HeaderMessageSize);
                            dataBuff = messageBuff;
                            nLen -= 1;
                            LogQueue.EnqueueDebugging("看到这行字也有点问题.");
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
                    _receiveBuffer = tempBuff;
                    _buffLen = nLen;
                }
                else
                {
                    _receiveBuffer = null;
                    _buffLen = 0;
                }
            }
            catch (Exception)
            {
                LogQueue.Enqueue($"[Exception] ProcReceiveBuffer BuffIndex:{srcOffset}", 5);
            }
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    LogQueue.Enqueue($"游戏网关[{_gateEndPoint}]链接云网关智能反外挂[{_clientSocket.EndPoint}]服务器拒绝链接...", 1);
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    LogQueue.Enqueue($"云网关智能反外挂服务器[{_clientSocket.EndPoint}]主动关闭连接游戏网关[{_gateEndPoint}]...", 1);
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    LogQueue.Enqueue($"游戏网关[{_gateEndPoint}]链接云网关智能反外挂服务器[{_clientSocket.EndPoint}]超时...", 1);
                    break;
            }
            GateReady = false;
        }

        public void SendServerMsg(ushort nIdent, ushort wSocketIndex, string nSocket, ushort nUserListIndex, int nLen,
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
        /// 玩家开始链接
        /// </summary>
        public void UserEnter(ushort socketIndex, string sessionId, string Data)
        {
            SendServerMsg(Grobal2.GM_OPEN, socketIndex, sessionId, 0, Data.Length, Data);
        }

        /// <summary>
        /// 玩家断开游戏
        /// </summary>
        public void UserLeave(string sessionId)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, sessionId, 0, 0, "");
        }

        private void SendServerMsg(ushort nIdent, ushort wSocketIndex, string nSocket, ushort nUserListIndex, int nLen,
            byte[] Data)
        {
            //var GateMsg = new PacketHeader();
            //GateMsg.PacketCode = Grobal2.RUNGATECODE;
            //GateMsg.Socket = nSocket;
            //GateMsg.SessionId = wSocketIndex;
            //GateMsg.Ident = nIdent;
            //GateMsg.ServerIndex = nUserListIndex;
            //GateMsg.PackLength = nLen;
            //var sendBuffer = GateMsg.GetBuffer();
            //if (Data is { Length: > 0 })
            //{
            //    var tempBuff = new byte[20 + Data.Length];
            //    Buffer.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
            //    Buffer.BlockCopy(Data, 0, tempBuff, sendBuffer.Length, Data.Length);
            //    SendBuffer(tempBuff);
            //}
            //else
            //{
            //    SendBuffer(sendBuffer);
            //}
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
            _clientSocket.Send(sendBuffer);
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