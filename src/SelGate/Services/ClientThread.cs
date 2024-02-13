using OpenMir2;
using OpenMir2.Packets.ServerPackets;
using SelGate.Datas;
using System;
using System.Net;
using System.Threading.Tasks;
using TouchSocket.Sockets;
using TcpClient = TouchSocket.Sockets.TcpClient;

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
        private readonly TcpClient _clientSocket;
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
        public readonly SessionInfo[] SessionArray;
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

        /// <summary>
        /// 数据缓冲区
        /// </summary>
        private readonly byte[] DataBuff;
        /// <summary>
        /// 缓存缓冲长度
        /// </summary>
        private int DataLen;

        public ClientThread(int clientId, string serverAddr, int serverPort, SessionManager sessionManager)
        {
            ClientId = clientId;
            SessionArray = new SessionInfo[MaxSession];
            _sessionManager = sessionManager;
            _clientSocket = new TcpClient();
            _clientSocket.Setup(new TouchSocket.Core.TouchSocketConfig().SetRemoteIPHost(new IPHost(IPAddress.Parse(serverAddr), serverPort)));
            _clientSocket.Connected += ClientSocketConnect;
            _clientSocket.Disconnected += ClientSocketDisconnect;
            _clientSocket.Received += ClientSocketRead;
            SockThreadStutas = SockThreadStutas.Connecting;
            KeepAliveTick = HUtil32.GetTickCount();
            KeepAlive = true;
            DataBuff = new byte[2048 * 10];
        }

        public bool IsConnected => isConnected;

        public string GetEndPoint()
        {
            return _clientSocket.RemoteIPHost.ToString();
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
            _clientSocket.Close();
        }

        public SessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private Task ClientSocketConnect(ITcpClient client, ConnectedEventArgs e)
        {
            boGateReady = true;
            RestSessionArray();
            isConnected = true;
            SockThreadStutas = SockThreadStutas.Connected;
            KeepAliveTick = HUtil32.GetTickCount();
            GateShare.CheckServerTick = HUtil32.GetTickCount();
            GateShare.ServerGateList.Add(this);
            LogService.Info($"数据库服务器[{client.RemoteIPHost}]链接成功.", 1);
            LogService.Debug($"线程[{Guid.NewGuid():N}]连接 {client.RemoteIPHost} 成功...");
            return Task.CompletedTask;
        }

        private Task ClientSocketDisconnect(ITcpClientBase client, DisconnectEventArgs e)
        {
            RestSessionArray();
            GateShare.ServerGateList.Remove(this);
            LogService.Info($"数据库服务器[{client.GetIPPort()}]断开链接.", 1);
            boGateReady = false;
            isConnected = false;
            CheckServerFail = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 收到数据库服务器 直接发送给客户端
        /// </summary>
        private Task ClientSocketRead(ITcpClient client, ReceivedDataEventArgs e)
        {
            int nMsgLen = e.ByteBlock.Len;
            if (nMsgLen <= 0)
            {
                return Task.CompletedTask;
            }
            if (DataLen > 0)
            {
                MemoryCopy.BlockCopy(e.ByteBlock.Buffer, 0, DataBuff, DataLen, nMsgLen);
                ProcessServerData(DataBuff, DataLen + nMsgLen);
            }
            else
            {
                ProcessServerData(e.ByteBlock.Buffer, nMsgLen);
            }
            return Task.CompletedTask;
        }

        private void ProcessServerData(byte[] data, int nLen)
        {
            int srcOffset = 0;
            Span<byte> dataBuff = data;
            while (nLen > ServerDataPacket.FixedHeaderLen)
            {
                Span<byte> packetHead = dataBuff[..ServerDataPacket.FixedHeaderLen];
                ServerDataPacket message = SerializerUtil.Deserialize<ServerDataPacket>(packetHead);
                if (message.PacketCode != Grobal2.PacketCode)
                {
                    srcOffset++;
                    dataBuff = dataBuff.Slice(srcOffset, ServerDataPacket.FixedHeaderLen);
                    nLen -= 1;
                    LogService.Debug($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                    continue;
                }
                int nCheckMsgLen = Math.Abs(message.PacketLen + ServerDataPacket.FixedHeaderLen);
                if (nCheckMsgLen > nLen)
                {
                    break;
                }
                ServerDataMessage messageData = SerializerUtil.Deserialize<ServerDataMessage>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
                switch (messageData.Type)
                {
                    case ServerDataType.KeepAlive:
                        KeepAliveTick = HUtil32.GetTickCount();
                        CheckServerFail = false;
                        boGateReady = true;
                        isConnected = true;
                        LogService.Debug("DBSrv Heartbeat Response");
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
            for (int i = 0; i < MaxSession; i++)
            {
                if (SessionArray[i] != null)
                {
                    SessionArray[i].dwReceiveTick = HUtil32.GetTickCount();
                    SessionArray[i].SocketId = string.Empty;
                    SessionArray[i].ClientIP = string.Empty;
                }
            }
        }

        public void SendKeepAlive()
        {
            ServerDataMessage messageData = new ServerDataMessage();
            messageData.Type = ServerDataType.KeepAlive;
            SendSocket(SerializerUtil.Serialize(messageData));
            LogService.Debug("Send DBSrv Heartbeat.");
        }

        public void SendBuffer(string sendText)
        {
            SendSocket(HUtil32.GetBytes(sendText));
        }

        public void SendSocket(byte[] buffer)
        {
            if (!_clientSocket.Online)
            {
                return;
            }
            SendMessage(buffer);
        }

        private void SendMessage(byte[] sendBuffer)
        {
            ServerDataPacket serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.PacketCode,
                PacketLen = (ushort)sendBuffer.Length
            };
            byte[] dataBuff = SerializerUtil.Serialize(serverMessage);
            byte[] data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
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