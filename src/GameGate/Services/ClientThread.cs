using GameGate.Conf;
using System;
using System.Net;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Packets;
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
        private IPEndPoint GateEndPoint { get; }
        /// <summary>
        /// 用户会话
        /// </summary>
        public readonly SessionInfo[] SessionArray = new SessionInfo[GateShare.MaxSession];
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        private bool CheckServerFail { get; set; }
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        private int CheckServerFailCount { get; set; }
        /// <summary>
        /// 接收GameSvr数据缓冲区
        /// </summary>
        private byte[] ReceiveBuffer { get; set; }
        /// <summary>
        /// 发送GameSvr数据缓冲区
        /// </summary>
        private byte[] SendBuffer { get; set; }
        /// <summary>
        /// 上次剩下多少字节未处理
        /// </summary>
        private int BuffLen { get; set; }
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        private bool GateReady { get; set; }
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool Connected { get; set; }
        /// <summary>
        /// 历史最高在线人数
        /// </summary>
        private int Counter { get; set; }
        /// <summary>
        /// 发送总字节数
        /// </summary>
        private int SendBytes { get; set; }
        /// <summary>
        /// 接收总字节数
        /// </summary>
        private int ReceiveBytes { get; set; }
        private int CheckRecviceTick { get; set; }
        private int CheckServerTick { get; set; }
        private int CheckServerTimeMin { get; set; }
        private int CheckServerTimeMax { get; set; }

        /// <summary>
        /// Session管理
        /// </summary>
        private static SessionManager SessionManager => SessionManager.Instance;

        /// <summary>
        /// 日志
        /// </summary>
        private static MirLog Logger => MirLog.Instance;

        private const int HeaderMessageSize = 20;

        public ClientThread(string clientId, IPEndPoint endPoint, GameGateInfo gameGate)
        {
            ClientId = clientId;
            ReceiveBytes = 0;
            SendBytes = 0;
            Connected = false;
            GateEndPoint = endPoint;
            CheckServerTick = HUtil32.GetTickCount();
            ClientSocket = new ClientScoket(new IPEndPoint(IPAddress.Parse(gameGate.ServerAdress), gameGate.ServerPort), 512);
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.OnReceivedData += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
        }

        public bool IsConnected => ClientSocket.IsConnected;

        public string EndPoint => $"{ClientSocket.RemoteEndPoint}";

        public void Start()
        {
            ClientSocket.Connect();
        }

        private void ReConnected()
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
            // ClientSocket.Disconnect();
        }

        public SessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateReady = true;
            CheckServerTick = HUtil32.GetTickCount();
            CheckRecviceTick = HUtil32.GetTickCount();
            RestSessionArray();
            CheckServerTimeMax = 0;
            CheckServerTimeMax = 0;
            Connected = true;
            ReceiveBytes = 0;
            SendBytes = 0;
            ReceiveBuffer = new byte[4096 * 10];
            SendBuffer = new byte[2048];
            Logger.Log($"[{GateEndPoint}] 游戏引擎[{e.RemoteEndPoint}]链接成功.", 1);
            Logger.DebugLog($"线程[{Guid.NewGuid():N}]连接 {e.RemoteEndPoint} 成功...");
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
            ReceiveBuffer = null;
            GateReady = false;
            Logger.Log($"[{GateEndPoint}] 游戏引擎[{e.RemoteEndPoint}]断开链接.", 1);
            Connected = false;
            CheckServerFail = true;
        }

        /// <summary>
        /// 接收GameSvr发来的封包消息
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var nMsgLen = e.BuffLen;
            if (BuffLen > 0)
            {
                MemoryCopy.BlockCopy(e.Buff, 0, ReceiveBuffer, BuffLen, nMsgLen);
                ProcessServerPacket(ReceiveBuffer, BuffLen + nMsgLen);
            }
            else
            {
                ProcessServerPacket(e.Buff, nMsgLen);
            }
            ReceiveBytes += nMsgLen;
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case SocketError.ConnectionRefused:
                    Logger.Log($"游戏网关[{GateEndPoint}]链接游戏引擎[{EndPoint}]拒绝链接...", 1);
                    Connected = false;
                    break;
                case SocketError.ConnectionReset:
                    Logger.Log($"游戏引擎[{EndPoint}]主动关闭连接游戏网关[{GateEndPoint}]...", 1);
                    Connected = false;
                    break;
                case SocketError.TimedOut:
                    Logger.Log($"游戏网关[{GateEndPoint}]链接游戏引擎时[{EndPoint}]超时...", 1);
                    Connected = false;
                    break;
            }
            GateReady = false;
            CheckServerFail = true;
        }

        private void ProcessServerPacket(byte[] buff, int buffLen)
        {
            var srcOffset = 0;
            var nLen = buffLen;
            Span<byte> dataBuff = buff;
            try
            {
                while (nLen >= ServerMessagePacket.PacketSize)
                {
                    Span<byte> packetHead = dataBuff[..20];
                    var packetCode = BitConverter.ToUInt32(packetHead[..4]);
                    if (packetCode != Grobal2.RUNGATECODE)
                    {
                        srcOffset++;
                        dataBuff = dataBuff.Slice(srcOffset, HeaderMessageSize);
                        nLen -= 1;
                        Logger.DebugLog($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
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
                            CheckServerTick = HUtil32.GetTickCount();
                            break;
                        case Grobal2.GM_SERVERUSERINDEX:
                            var userSession = SessionManager.GetSession(sessionId);
                            if (userSession != null)
                            {
                                userSession.SvrListIdx = serverIndex;
                            }
                            break;
                        case Grobal2.GM_RECEIVE_OK:
                            CheckServerTimeMin = HUtil32.GetTickCount() - CheckRecviceTick;
                            if (CheckServerTimeMin > CheckServerTimeMax)
                            {
                                CheckServerTimeMax = CheckServerTimeMin;
                            }
                            CheckRecviceTick = HUtil32.GetTickCount();
                            SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, 0, "");
                            break;
                        case Grobal2.GM_DATA:
                            var sessionPacket = new MessagePacket
                            {
                                SessionId = sessionId,
                                BufferLen = packLength
                            };
                            if (packLength > 0)
                            {
                                sessionPacket.Buffer = dataBuff.Slice(20, packLength).ToArray();
                            }
                            else
                            {
                                var packetSize = dataBuff.Length - HeaderMessageSize;
                                sessionPacket.Buffer = dataBuff.Slice(20, packetSize).ToArray();
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
                    BuffLen = nLen;
                    srcOffset = 0;
                    if (nLen < HeaderMessageSize)
                    {
                        break;
                    }
                }
                if (nLen > 0) //有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    //ReceiveBuffer = dataBuff[..nLen].ToArray();
                    MemoryCopy.BlockCopy(dataBuff, 0, ReceiveBuffer, 0, nLen);
                    BuffLen = nLen;
                }
                else
                {
                    BuffLen = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"[Exception] ProcReceiveBuffer BuffIndex:{srcOffset}", 5);
                Logger.LogError(ex);
            }
        }

        public void RestSessionArray()
        {
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].Socket = null;
                    SessionArray[i].UserListIndex = 0;
                    SessionArray[i].ReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].SckHandle = 0;
                    SessionArray[i].SessionId = 0;
                }
            }
        }

        private void SendServerMsg(ushort command, ushort socketIndex, int nSocket, ushort userIndex, int nLen,
            string data)
        {
            var gateMsg = new ServerMessagePacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = nSocket,
                SessionId = socketIndex,
                Ident = command,
                ServerIndex = userIndex,
                PackLength = nLen
            };
            var sendBuffer = ServerPackSerializer.Serialize(gateMsg);
            if (!string.IsNullOrEmpty(data))
            {
                var strBuff = HUtil32.GetBytes(data);
                var tempBuff = new byte[20 + data.Length];
                MemoryCopy.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
                MemoryCopy.BlockCopy(strBuff, 0, tempBuff, sendBuffer.Length, data.Length);
                Send(tempBuff);
            }
            else
            {
                Send(sendBuffer);
            }
        }

        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        public void UserEnter(ushort socketIndex, int socketId, string data)
        {
            SendServerMsg(Grobal2.GM_OPEN, socketIndex, socketId, 0, data.Length, data);
        }

        /// <summary>
        /// 玩家退出游戏
        /// </summary>
        public void UserLeave(int socketId)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, socketId, 0, 0, "");
        }

        /// <summary>
        /// 发送消息到GameSvr
        /// </summary>
        /// <param name="sendBuffer"></param>
        internal void Send(byte[] sendBuffer)
        {
            if (!ClientSocket.IsConnected)
            {
                return;
            }
            //MemoryCopy.BlockCopy(BitConverter.GetBytes(sendBuffLen), 0, SendBuffer, 0, 4);
            //MemoryCopy.BlockCopy(sendBuffer, 0, SendBuffer, 4, sendBuffer.Length);
            SendBytes += sendBuffer.Length;
            ClientSocket.Send(sendBuffer);
        }

        /// <summary>
        /// 处理超时或空闲会话
        /// </summary>
        public void ProcessIdleSession()
        {
            var currentTick = HUtil32.GetTickCount();
            for (var j = 0; j < SessionArray.Length; j++)
            {
                var userSession = SessionArray[j];
                if (userSession != null && userSession.Socket != null)
                {
                    if ((currentTick - userSession.ReceiveTick) > GateShare.SessionTimeOutTime) //清理超时用户会话 
                    {
                        userSession.Socket.Close();
                        userSession.SckHandle = -1;
                        userSession.Socket = null;
                    }
                }
                CheckServerTimeMin = HUtil32.GetTickCount() - CheckServerTick;
                if (CheckServerTimeMax < CheckServerTimeMin)
                {
                    CheckServerTimeMax = CheckServerTimeMin;
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
            if (CheckServerFail && CheckServerFailCount <= ushort.MaxValue)
            {
                ReConnected();
                CheckServerFailCount++;
                Logger.DebugLog($"链接服务器[{EndPoint}]失败.[{CheckServerFailCount}]");
                return;
            }
            if (CheckServerFailCount >= ushort.MaxValue)
            {
                Logger.DebugLog("超过最大重试次数，请重启程序后再次确认链接是否正常。");
                return;
            }
            CheckServerTimeOut();
        }

        private void CheckServerTimeOut()
        {
            if ((HUtil32.GetTickCount() - CheckServerTick) > GateShare.CheckServerTimeOutTime && CheckServerFailCount <= ushort.MaxValue)
            {
                CheckServerFail = true;
                Stop();
                CheckServerFailCount++;
                Logger.DebugLog($"服务器[{EndPoint}]长时间没有响应,断开链接.失败次数:[{CheckServerFailCount}]");
            }
        }

        public string GetConnected()
        {
            return IsConnected ? "[green]Connected[/]" : "[red]Not Connected[/]";
        }

        public string ShowSend()
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

        public string ShowReceive()
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

        public string TotalReceive => $"↓{HUtil32.FormatBytesValue(ClientSocket.TotalBytesRead)}";

        public string TotalSend => $"↑{HUtil32.FormatBytesValue(ClientSocket.TotalBytesWrite)}";
    }
}