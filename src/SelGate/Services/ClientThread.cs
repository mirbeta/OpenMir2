using SelGate.Package;
using System;
using System.IO;
using System.Net;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace SelGate.Services
{
    /// <summary>
    /// 网关客户端(SelGate-DBSrv)
    /// </summary>
    public class ClientThread
    {
        /// <summary>
        /// Socket客户端
        /// </summary>
        private readonly ScoketClient _clientSocket;
        /// <summary>
        /// 网关编号（初始化的时候进行分配）
        /// </summary>
        public readonly int ClientId = 0;
        /// <summary>
        /// 最大用户数
        /// </summary>
        public const int MaxSession = 2000;
        /// <summary>
        /// 用户会话
        /// </summary>
        public readonly TSessionInfo[] SessionArray;
        /// <summary>
        ///  网关游戏服务器之间检测是否失败/超时
        /// </summary>
        public bool CheckServerFail = false;
        /// <summary>
        /// 网关游戏服务器之间检测是否失败次数
        /// </summary>
        public int CheckServerFailCount = 0;
        public bool KeepAlive;
        public int KeepAliveTick;
        public SockThreadStutas SockThreadStutas;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public bool boGateReady = false;
        /// <summary>
        /// 是否链接成功
        /// </summary>
        private bool isConnected = false;
        /// <summary>
        /// 会话管理
        /// </summary>
        private readonly SessionManager _sessionManager;
        /// <summary>
        /// Logger
        /// </summary>
        private static MirLogger _logger;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        private byte[] DataBuff;
        /// <summary>
        /// 缓存缓冲长度
        /// </summary>
        private int DataLen;
        
        public ClientThread(int clientId, string serverAddr, int serverPort, SessionManager sessionManager, MirLogger logger)
        {
            ClientId = clientId;
            _logger = logger;
            SessionArray = new TSessionInfo[MaxSession];
            _sessionManager = sessionManager;
            _clientSocket = new ScoketClient(new IPEndPoint(IPAddress.Parse(serverAddr), serverPort), 512);
            _clientSocket.OnConnected += ClientSocketConnect;
            _clientSocket.OnDisconnected += ClientSocketDisconnect;
            _clientSocket.OnReceivedData += ClientSocketRead;
            _clientSocket.OnError += ClientSocketError;
            SockThreadStutas = SockThreadStutas.Connecting;
            KeepAliveTick = HUtil32.GetTickCount();
            KeepAlive = true;
            DataBuff = new byte[2048 * 10];
        }

        public bool IsConnected => isConnected;

        public string GetEndPoint()
        {
            return _clientSocket.RemoteEndPoint.ToString();
        }

        public void Start()
        {
            _clientSocket.Connect();
        }

        public void ReConnected()
        {
            if (isConnected == false)
            {
                _clientSocket.Connect();
            }
        }

        public void Stop()
        {
            for (var i = 0; i < SessionArray.Length; i++)
            {
                if (SessionArray[i] != null && SessionArray[i].Socket != null)
                {
                    SessionArray[i].Socket.Close();
                }
            }
            _clientSocket.Disconnect();
        }

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            boGateReady = true;
            RestSessionArray();
            isConnected = true;
            SockThreadStutas = SockThreadStutas.Connected;
            KeepAliveTick = HUtil32.GetTickCount();
            GateShare.CheckServerTick = HUtil32.GetTickCount();
            GateShare.ServerGateList.Add(this);
            _logger.LogInformation($"数据库服务器[{e.RemoteEndPoint}]链接成功.", 1);
            _logger.DebugLog($"线程[{Guid.NewGuid():N}]连接 {e.RemoteEndPoint} 成功...");
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
                if (userSession.Socket != null && userSession.Socket == e.Socket)
                {
                    userSession.Socket.Close();
                    userSession.Socket = null;
                }
            }
            RestSessionArray();
            GateShare.ServerGateList.Remove(this);
            _logger.LogInformation($"数据库服务器[{e.RemoteEndPoint}]断开链接.", 1);
            boGateReady = false;
            isConnected = false;
            CheckServerFail = true;
        }

        /// <summary>
        /// 收到数据库服务器 直接发送给客户端
        /// </summary>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var nMsgLen = e.BuffLen;
            if (nMsgLen <= 0)
            {
                return;
            }
            if (DataLen > 0)
            {
                MemoryCopy.BlockCopy(e.Buff, 0, DataBuff, DataLen, nMsgLen);
                ProcessServerData(DataBuff, DataLen + nMsgLen);
            }
            else
            {
                ProcessServerData(e.Buff, nMsgLen);
            }
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logger.LogInformation($"数据库服务器[{_clientSocket.RemoteEndPoint}]拒绝链接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logger.LogInformation($"数据库服务器[{_clientSocket.RemoteEndPoint}]关闭连接...失败[{CheckServerFailCount}]次", 1);
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logger.LogInformation($"数据库服务器[{_clientSocket.RemoteEndPoint}]链接超时...失败[{CheckServerFailCount}]次", 1);
                    break;
            }
        }

        private void ProcessServerData(byte[] data, int nLen)
        {
            var srcOffset = 0;
            Span<byte> dataBuff = data;
            while (nLen > ServerDataPacket.FixedHeaderLen)
            {
                var packetHead = dataBuff[..ServerDataPacket.FixedHeaderLen];
                var message = SerializerUtil.Deserialize<ServerDataPacket>(packetHead);
                if (message.PacketCode != Grobal2.PacketCode)
                {
                    srcOffset++;
                    dataBuff = dataBuff.Slice(srcOffset, ServerDataPacket.FixedHeaderLen);
                    nLen -= 1;
                    _logger.DebugLog($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                    continue;
                }
                var nCheckMsgLen = Math.Abs(message.PacketLen + ServerDataPacket.FixedHeaderLen);
                if (nCheckMsgLen > nLen)
                {
                    break;
                } 
                var messageData = SerializerUtil.Deserialize<ServerDataMessage>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
                switch (messageData.Type)
                {
                    case ServerDataType.KeepAlive:
                        KeepAliveTick = HUtil32.GetTickCount();
                        CheckServerFail = false;
                        boGateReady = true;
                        isConnected = true;
                        _logger.DebugLog("DBSrv Heartbeat Response");
                        break;
                    case ServerDataType.Leave:
                        _sessionManager.CloseSession(messageData.SocketId);
                        /*if (message.Body[0] == (byte)'+')//收到DB服务器发过来的关闭会话请求
                        {
                            if (message.Body[1] == (byte)'-')
                            {
                                userSession.CloseSession();
                                Console.WriteLine("收到DBSvr关闭会话请求");
                            }
                            else
                            {
                                userSession.ClientThread.KeepAliveTick = HUtil32.GetTickCount();
                            }
                            continue;
                        }*/
                        break;
                    case ServerDataType.Data:
                        _sessionManager.SendQueue.TryWrite(messageData);
                        break;
                }
                nLen -= nCheckMsgLen;
                if (nLen <= 0)
                {
                    break;
                }
                dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
                DataLen = nLen;
                srcOffset = 0;
                if (nLen < ServerDataPacket.FixedHeaderLen)
                {
                    break;
                }
            }
            if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
            {
                MemoryCopy.BlockCopy(dataBuff, 0, DataBuff, 0, nLen);
                DataLen = nLen;
            }
            else
            {
                DataLen = 0;
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

        public void SendKeepAlive()
        {
            var messageData = new ServerDataMessage();
            messageData.Type = ServerDataType.KeepAlive;
            SendSocket(SerializerUtil.Serialize(messageData));
            _logger.DebugLog("Send DBSrv Heartbeat.");
        }

        public void SendBuffer(string sendText)
        {
            SendSocket(HUtil32.GetBytes(sendText));
        }

        public void SendSocket(byte[] buffer)
        {
            if (!_clientSocket.IsConnected)
            {
               return;
            }
            SendMessage(buffer);
        }
        
        private void SendMessage(byte[] sendBuffer)
        {
            var serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.PacketCode,
                PacketLen = (ushort)sendBuffer.Length
            };
            var dataBuff = SerializerUtil.Serialize(serverMessage);
            var data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
            MemoryCopy.BlockCopy(dataBuff, 0, data, 0, data.Length);
            MemoryCopy.BlockCopy(sendBuffer, 0, data, dataBuff.Length, sendBuffer.Length);
            _clientSocket.Send(data);
        }
    }

    public enum SockThreadStutas : byte
    {
        Connecting = 0,
        Connected = 1,
        TimeOut = 2
    }
}