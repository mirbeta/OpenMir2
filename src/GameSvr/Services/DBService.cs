using NLog;
using System.Net;
using SystemModule;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameSvr.Services
{
    public class DBService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ClientScoket _clientScoket;
        private byte[] ReceiveBuffer { get; set; }
        public int BuffLen { get; set; }
        private bool SocketWorking { get; set; }

        public DBService()
        {
            _clientScoket = new ClientScoket(new IPEndPoint(IPAddress.Parse(M2Share.Config.sDBAddr), M2Share.Config.nDBPort), 4096);
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.OnReceivedData += DBSocketRead;
            _clientScoket.OnError += DBSocketError;
            SocketWorking = false;
            ReceiveBuffer = new byte[10000];
        }

        public void Start()
        {
            _clientScoket.Connect();
        }

        public void Stop()
        {
            _clientScoket.Disconnect();
        }

        public void CheckConnected()
        {
            if (_clientScoket.IsConnected)
            {
                return;
            }
            if (_clientScoket.IsBusy)
            {
                return;
            }
            _clientScoket.Connect(M2Share.Config.sDBAddr, M2Share.Config.nDBPort);
        }

        public bool SendRequest<T>(int queryId, ServerRequestMessage message, T packet) where T : RequestPacket
        {
            if (!_clientScoket.IsConnected)
            {
                return false;
            }
            var requestPacket = new ServerRequestData();
            requestPacket.PacketCode = Grobal2.RUNGATECODE;
            requestPacket.QueryId = queryId;
            requestPacket.Message = EDCode.EncodeBuffer(ServerPackSerializer.Serialize(message));
            requestPacket.Packet = EDCode.EncodeBuffer(ServerPackSerializer.Serialize(packet));
            var sginId = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + 6));
            requestPacket.Sgin = EDCode.EncodeBuffer(BitConverter.GetBytes(sginId));
            _clientScoket.Send(requestPacket.GetBuffer());
            return true;
        }

        private void DbScoketDisconnected(object sender, DSCClientConnectedEventArgs e)
        {
            _clientScoket.IsConnected = false;
            _logger.Error("数据库服务器[" + e.RemoteEndPoint + "]断开连接...");
        }

        private void DbScoketConnected(object sender, DSCClientConnectedEventArgs e)
        {
            _clientScoket.IsConnected = true;
            _logger.Info("数据库服务器[" + e.RemoteEndPoint + "]连接成功...");
        }

        private void DBSocketError(object sender, DSCClientErrorEventArgs e)
        {
            _clientScoket.IsConnected = false;
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logger.Error("数据库服务器[" + M2Share.Config.sDBAddr + ":" + M2Share.Config.nDBPort + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logger.Error("数据库服务器[" + M2Share.Config.sDBAddr + ":" + M2Share.Config.nDBPort + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logger.Error("数据库服务器[" + M2Share.Config.sDBAddr + ":" + M2Share.Config.nDBPort + "]链接超时...");
                    break;
            }
        }

        private void ProcessServerPacket(byte[] buff, int buffLen)
        {
            try
            {   
                var srcOffset = 0;
                var nLen = buffLen;
                Span<byte> dataBuff = buff;
                while (nLen >= ServerRequestData.HeaderMessageSize)
                {
                    Span<byte> packetHead = dataBuff[..ServerRequestData.HeaderMessageSize];
                    var packetCode = BitConverter.ToUInt32(packetHead[..4]);
                    if (packetCode != Grobal2.RUNGATECODE)
                    {
                        srcOffset++;
                        dataBuff = dataBuff.Slice(srcOffset, ServerRequestData.HeaderMessageSize);
                        nLen -= 1;
                        _logger.Debug($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                        continue;
                    }
                    var queryId = BitConverter.ToUInt32(packetHead.Slice(4, 4));
                    var messageLen = BitConverter.ToInt16(packetHead.Slice(8, 2));
                    var nCheckMsgLen = Math.Abs(messageLen);
                    if (nCheckMsgLen > nLen)
                    {
                        break;
                    }
                    SocketWorking = true;
                    ProcessData(dataBuff[..messageLen]);
                    nLen -= nCheckMsgLen;
                    if (nLen <= 0)
                    {
                        break;
                    }
                    dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
                    BuffLen = nLen;
                    srcOffset = 0;
                    if (nLen < ServerRequestData.HeaderMessageSize)
                    {
                        break;
                    }
                }
                if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    MemoryCopy.BlockCopy(dataBuff, 0, ReceiveBuffer, 0, nLen);
                    //ReceiveBuffer = dataBuff[..nLen].ToArray();
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

        private void DBSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                var nMsgLen = e.BuffLen;
                var packetData = e.Buff[..nMsgLen];
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
                HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
            }
        }

        private void ProcessData(Span<byte> data)
        {
            try
            {
                if (!SocketWorking) return;
                var responsePacket = Packets.ToPacket<ServerRequestData>(data);
                if (responsePacket != null && responsePacket.PacketLen > 0)
                {
                    var respCheckCode = responsePacket.QueryId;
                    var nLen = responsePacket.Message.Length + responsePacket.Packet.Length + 6;
                    if (nLen >= 12)
                    {
                        var queryId = HUtil32.MakeLong((ushort)(respCheckCode ^ 170), (ushort)nLen);
                        if (queryId <= 0 || responsePacket.Sgin.Length <= 0)
                        {
                            M2Share.Config.nLoadDBErrorCount++;
                            return;
                        }
                        var signatureBuff = BitConverter.GetBytes(queryId);
                        var sginBuff = EDCode.DecodeBuff(responsePacket.Sgin);
                        var signatureId = BitConverter.ToInt16(signatureBuff);
                        if (signatureId == BitConverter.ToInt16(sginBuff))
                        {
                            PlayerDataService.Enqueue(respCheckCode, responsePacket);
                            _logger.Info($"[{respCheckCode}]读取成功");
                        }
                        else
                        {
                            M2Share.Config.nLoadDBErrorCount++;
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