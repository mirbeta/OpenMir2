using System;
using System.Net;
using System.Net.Sockets;
using GameGate.Conf;
using NLog;
using SystemModule;
using SystemModule.DataHandlingAdapters;
using SystemModule.Packets.ServerPackets;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TcpClient = TouchSocket.Sockets.TcpClient;

namespace GameGate.Services
{
    /// <summary>
    /// 网关客户端(GameGate-GameSrv)
    /// </summary>
    public class ClientThread
    {
        private readonly TcpClient ClientSocket;
        private readonly GameGateInfo GateInfo;
        private readonly IPEndPoint LocalEndPoint;
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
        private int OnlineCount { get; set; }
        /// <summary>
        /// 发送总字节数
        /// </summary>
        private long SendBytes { get; set; }
        /// <summary>
        /// 接收总字节数
        /// </summary>
        private long ReceiveBytes { get; set; }
        public long TotalBytesWrite { get; set; }
        public long TotalBytesRead { get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        private RunningState RunningState { get; set; }
        private int CheckRecviceTick { get; set; }
        private int CheckServerTick { get; set; }
        /// <summary>
        /// Session管理
        /// </summary>
        private static SessionManager SessionManager => SessionManager.Instance;
        /// <summary>
        /// 日志
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ClientThread(IPEndPoint endPoint, GameGateInfo gameGate)
        {
            GateInfo = gameGate;
            RunningState = RunningState.Waiting;
            ReceiveBytes = 0;
            SendBytes = 0;
            Connected = false;
            LocalEndPoint = endPoint;
            CheckServerTick = HUtil32.GetTickCount();
            ClientSocket = new TcpClient();
            ClientSocket.Connected  += ClientSocketConnect;
            ClientSocket.Disconnected  += ClientSocketDisconnect;
            ClientSocket.Received += ClientSocketRead;
        }

        public bool IsConnected => Connected;

        public string EndPoint => $"{GateInfo.ServerAdress}:{GateInfo.ServerPort}";
        
        public string ThreadId=> $"{GateInfo.ThreadId}";

        public RunningState Running => RunningState;

        public void Init()
        {
            var config = new TouchSocketConfig();
            config.SetRemoteIPHost(new IPHost(IPAddress.Parse(GateInfo.ServerAdress), GateInfo.ServerPort))
                .SetBufferLength(1024);
            config.SetDataHandlingAdapter(() => new PacketFixedHeaderDataHandlingAdapter());
            ClientSocket.Setup(config);
        }

        public void Start()
        {
            try
            {
                ClientSocket.Connect();
            }
            catch (SocketException e)
            {
                ClientSocketError(e.SocketErrorCode);
            }
            catch (TimeoutException e)
            {
                ClientSocketError(SocketError.TimedOut);
            }
        }

        private void ReConnected()
        {
            if (Connected == false)
            {
                Start();
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
            if (count > OnlineCount)
            {
                OnlineCount = count;
            }
            return count + "/" + OnlineCount;
        }

        public void Stop()
        {
            ClientSocket.Close();
        }

        public SessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, MsgEventArgs e)
        {
            var client = (TcpClient)sender;
            GateReady = true;
            CheckServerTick = HUtil32.GetTickCount();
            CheckRecviceTick = HUtil32.GetTickCount();
            Connected = true;
            ReceiveBytes = 0;
            SendBytes = 0;
            RunningState = RunningState.Runing;
            RestSessionArray();
            _logger.Info($"[{LocalEndPoint}] 游戏引擎[{client.MainSocket.RemoteEndPoint}]链接成功.");
            _logger.Debug($"线程[{Guid.NewGuid():N}]连接 {client.MainSocket.RemoteEndPoint} 成功...");
        }

        private void ClientSocketDisconnect(object sender, DisconnectEventArgs e)
        {
            var client = (TcpClient)sender;
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                var userSession = SessionArray[i];
                if (userSession != null)
                {
                    if (userSession.Socket != null && userSession.Socket == client.MainSocket)
                    {
                        userSession.Socket.Close();
                        userSession.Socket = null;
                        userSession.SckHandle = -1;
                    }
                }
            }
            RestSessionArray();
            GateReady = false;
            _logger.Info($"[{LocalEndPoint}] 游戏引擎[{client.MainSocket.RemoteEndPoint}]断开链接.");
            Connected = false;
            CheckServerFail = true;
        }

        /// <summary>
        /// 接收GameSvr发来的封包消息
        /// </summary>
        private void ClientSocketRead(object sender, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (requestInfo is not DataMessageFixedHeaderRequestInfo fixedHeader)
                return;
            if (fixedHeader.Header.PacketCode != Grobal2.PacketCode)
            {
                _logger.Debug("解析GameSrv消息封包错误");
                return;
            }
            var nCheckMsgLen = Math.Abs(fixedHeader.Header.PackLength) + GateShare.HeaderMessageSize;
            if (fixedHeader.BodyLength < nCheckMsgLen)
            {
                _logger.Info("封包长度不足...");
                return;
            }
            ProcessServerPacket(fixedHeader.Header, fixedHeader.Message);
            ReceiveBytes += fixedHeader.BodyLength;
        }

        private void ClientSocketError(SocketError error)
        {
            switch (error)
            {
                case SocketError.ConnectionRefused:
                    _logger.Warn($"游戏网关[{LocalEndPoint}]链接游戏引擎[{EndPoint}]拒绝链接...");
                    Connected = false;
                    break;
                case SocketError.ConnectionReset:
                    _logger.Info($"游戏引擎[{EndPoint}]主动关闭连接游戏网关[{LocalEndPoint}]...");
                    Connected = false;
                    break;
                case SocketError.TimedOut:
                    _logger.Info($"游戏网关[{LocalEndPoint}]链接游戏引擎时[{EndPoint}]超时...");
                    Connected = false;
                    break;
            }
            GateReady = false;
            CheckServerFail = true;
        }

        private void ProcessServerPacket(ServerMessage packetHeader,byte[] data)
        {
            try
            {
                switch (packetHeader.Ident)
                {
                    case Grobal2.GM_STOP: //游戏引擎停止服务,网关不在接收或分配用户连接到该游戏引擎
                        RunningState = RunningState.Stop; //停止分配后，10分钟内不允许尝试连接服务器
                        break;
                    case Grobal2.GM_CHECKSERVER:
                        CheckServerFail = false;
                        CheckServerTick = HUtil32.GetTickCount();
                        break;
                    case Grobal2.GM_SERVERUSERINDEX:
                        var userSession = SessionManager.GetSession(GateInfo.ServiceId, packetHeader.SessionId);
                        if (userSession != null)
                        {
                            userSession.SvrListIdx = packetHeader.SessionIndex;
                        }
                        break;
                    case Grobal2.GM_RECEIVE_OK:
                        CheckRecviceTick = HUtil32.GetTickCount();
                        SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, "", 0);
                        break;
                    case Grobal2.GM_DATA:
                        byte[] dataMemory;
                        if (packetHeader.PackLength > 0)
                        {
                            dataMemory = data[..packetHeader.PackLength];
                        }
                        else
                        {
                            dataMemory = data[..GateShare.HeaderMessageSize];
                        }
                        var sessionPacket = new SessionMessage(packetHeader.SessionId, GateInfo.ServiceId, dataMemory, packetHeader.PackLength);
                        SessionManager.Enqueue(sessionPacket);
                        break;
                    case Messages.GM_TEST:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void RestSessionArray()
        {
            for (var i = 0; i < GateShare.MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].Socket = null;
                    SessionArray[i].SessionIndex = 0;
                    SessionArray[i].ReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].SckHandle = 0;
                    SessionArray[i].SessionId = 0;
                }
            }
        }

        private void SendServerMsg(ushort command, ushort sessionIndex, int nSocket, ushort userIndex, string data, int nLen)
        {
            var serverMessage = new ServerMessage
            {
                PacketCode = Grobal2.PacketCode,
                Socket = nSocket,
                SessionId = sessionIndex,
                Ident = command,
                SessionIndex = userIndex,
                PackLength = nLen,
            };
            var sendBuffer = SerializerUtil.Serialize(serverMessage);
            if (!string.IsNullOrEmpty(data))
            {
                var strBuff = HUtil32.GetBytes(data);
                var tempBuff = new byte[ServerMessage.PacketSize + data.Length];
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
        public void UserEnter(ushort sessionId, int socketId, string data)
        {
            SendServerMsg(Grobal2.GM_OPEN, sessionId, socketId, 0, data, data.Length);
        }

        /// <summary>
        /// 玩家退出游戏
        /// </summary>
        public void UserLeave(int socketId)
        {
            SendServerMsg(Grobal2.GM_CLOSE, 0, socketId, 0, "", 0);
        }

        /// <summary>
        /// 发送消息到GameSvr
        /// </summary>
        /// <param name="sendBuffer"></param>
        internal void Send(byte[] sendBuffer)
        {
            if (!ClientSocket.Online)
            {
                return;
            }
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
            }
        }

        /// <summary>
        /// 检查客户端和服务端之间的状态以及心跳维护
        /// </summary>
        public void CheckConnectedState()
        {
            if (Running == RunningState.Stop) //停止服务后暂时停止心跳连接检查
            {
                if ((HUtil32.GetTickCount() - CheckServerTick) > 60 * 10000) //10分钟分不允许尽兴链接服务器
                {
                    ReConnected();
                    _logger.Debug($"游戏引擎维护时间结束,重新连接游戏引擎[{EndPoint}].");
                }
                return;
            }
            if (GateReady)
            {
                SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, "", 0);
                CheckServerFailCount = 0;
                return;
            }
            if (CheckServerFail && CheckServerFailCount <= ushort.MaxValue)
            {
                ReConnected();
                CheckServerFailCount++;
                _logger.Debug($"链接服务器[{EndPoint}] 失败次数[{CheckServerFailCount}]");
                return;
            }
            if (CheckServerFailCount >= ushort.MaxValue)
            {
                _logger.Debug("超过最大重试次数，请重启程序后再次确认链接是否正常。");
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
                _logger.Debug($"服务器[{EndPoint}]长时间没有响应,断开链接.失败次数:[{CheckServerFailCount}]");
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
            TotalBytesWrite += SendBytes;
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
            TotalBytesRead += ReceiveBytes;
            ReceiveBytes = 0;
            return receiveStr;
        }

        public string TotalReceive => $"↓{HUtil32.FormatBytesValue(TotalBytesRead)}";

        public string TotalSend => $"↑{HUtil32.FormatBytesValue(TotalBytesWrite)}";
    }
}