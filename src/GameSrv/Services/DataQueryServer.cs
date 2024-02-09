using System.Net;
using M2Server;
using NLog;
using SystemModule;
using SystemModule.Packets.ServerPackets;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TcpClient = TouchSocket.Sockets.TcpClient;

namespace GameSrv.Services
{
    /// <summary>
    /// 玩家数据读写服务
    /// </summary>
    public class DataQueryServer
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpClient _tcpClient;
        private byte[] ReceiveBuffer { get; set; }
        private int BuffLen { get; set; }
        private bool SocketWorking { get; set; }

        public DataQueryServer()
        {
            _tcpClient.Setup(new TouchSocketConfig()
                .SetRemoteIPHost(new IPHost(IPAddress.Parse(SystemShare.Config.sDBAddr), SystemShare.Config.nDBPort))
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger(); //添加一个日志注入
                }));
            
            _tcpClient.Connected = DataScoketConnected; 
            _tcpClient.Disconnected = DataScoketDisconnected;
            _tcpClient.Received = DataSocketRead;
            SocketWorking = false;
            ReceiveBuffer = new byte[10 * 2048];
        }

        public void Start()
        {
            _tcpClient.Connect();
        }

        public void Stop()
        {
            _tcpClient.Close();
        }

        public bool IsConnected => _tcpClient.Online;

        public void CheckConnected()
        {
            _tcpClient.Connect(SystemShare.Config.sDBAddr, SystemShare.Config.nDBPort);
        }

        public bool SendRequest<T>(int queryId, ServerRequestMessage message, T packet)
        {
            if (!_tcpClient.Online)
            {
                return false;
            }
            var requestPacket = new ServerRequestData();
            requestPacket.QueryId = queryId;
            requestPacket.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(message));
            requestPacket.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(packet));
            var signId = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + ServerDataPacket.FixedHeaderLen));
            requestPacket.Sign = EDCode.EncodeBuffer(BitConverter.GetBytes(signId));
            SendMessage(SerializerUtil.Serialize(requestPacket));
            return true;
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
            _tcpClient.Send(data);
        }

        private Task DataScoketDisconnected(ITcpClientBase sender, DisconnectEventArgs e)
        {
            _logger.Error("数据库服务器[" + sender.GetIPPort() + "]断开连接...");
            return Task.CompletedTask;
        }

        private Task DataScoketConnected(ITcpClient client, ConnectedEventArgs e)
        {
            _logger.Info("数据库服务器[" + client.RemoteIPHost + "]连接成功...");
            return Task.CompletedTask;
        }

        private Task DataSocketRead(TcpClient sender, ReceivedDataEventArgs e)
        {
            HUtil32.EnterCriticalSection(M2Share.UserDBCriticalSection);
            try
            {
                var nMsgLen = e.ByteBlock.Len;
                var packetData = e.ByteBlock.Buffer;
                if (BuffLen > 0)
                {
                    MemoryCopy.BlockCopy(packetData, 0, ReceiveBuffer, BuffLen, packetData.Length);
                    ProcessServerPacket(ReceiveBuffer, BuffLen + nMsgLen);
                }
                else
                {
                    ProcessServerPacket(packetData, nMsgLen);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.UserDBCriticalSection);
            }
            return Task.CompletedTask;
        }

        private void ProcessServerPacket(Span<byte> buff, int buffLen)
        {
            try
            {
                var srcOffset = 0;
                var nLen = buffLen;
                var dataBuff = buff;
                while (nLen >= ServerDataPacket.FixedHeaderLen)
                {
                    var packetHead = dataBuff[..ServerDataPacket.FixedHeaderLen];
                    var message = SerializerUtil.Deserialize<ServerDataPacket>(packetHead);
                    if (message.PacketCode != Grobal2.PacketCode)
                    {
                        srcOffset++;
                        dataBuff = dataBuff.Slice(srcOffset, ServerDataPacket.FixedHeaderLen);
                        nLen -= 1;
                        _logger.Debug($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                        continue;
                    }
                    var nCheckMsgLen = Math.Abs(message.PacketLen + ServerDataPacket.FixedHeaderLen);
                    if (nCheckMsgLen > nLen)
                    {
                        break;
                    }
                    SocketWorking = true;
                    var messageData = SerializerUtil.Deserialize<ServerRequestData>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
                    ProcessServerData(messageData);
                    nLen -= nCheckMsgLen;
                    if (nLen <= 0)
                    {
                        break;
                    }
                    dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
                    BuffLen = nLen;
                    srcOffset = 0;
                    if (nLen < ServerDataPacket.FixedHeaderLen)
                    {
                        break;
                    }
                }
                if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
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
                _logger.Error(ex);
            }
        }

        private void ProcessServerData(ServerRequestData responsePacket)
        {
            try
            {
                if (!SocketWorking) return;
                if (responsePacket != null)
                {
                    var respCheckCode = responsePacket.QueryId;
                    var nLen = responsePacket.Message.Length + responsePacket.Packet.Length + ServerDataPacket.FixedHeaderLen;
                    if (nLen >= 12)
                    {
                        var queryId = HUtil32.MakeLong((ushort)(respCheckCode ^ 170), (ushort)nLen);
                        if (queryId <= 0 || responsePacket.Sign.Length <= 0)
                        {
                            SystemShare.Config.LoadDBErrorCount++;
                            return;
                        }
                        var signatureBuff = BitConverter.GetBytes(queryId);
                        var signBuff = EDCode.DecodeBuff(responsePacket.Sign);
                        if (BitConverter.ToInt16(signatureBuff) == BitConverter.ToInt16(signBuff))
                        {
                            CharacterDataService.Enqueue(respCheckCode, responsePacket);
                        }
                        else
                        {
                            SystemShare.Config.LoadDBErrorCount++;
                        }
                    }
                }
                else
                {
                    _logger.Error("错误的封包数据");
                }
            }
            finally
            {
                SocketWorking = false;
            }
        }
    }
}