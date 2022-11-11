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
        private int PacketLen{ get; set; }
        private byte[] RecvBuff { get; set; }
        private bool SocketWorking { get; set; }

        public DBService()
        {
            _clientScoket = new ClientScoket(new IPEndPoint(IPAddress.Parse(M2Share.Config.sDBAddr), M2Share.Config.nDBPort), 4096);
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.OnReceivedData += DBSocketRead;
            _clientScoket.OnError += DBSocketError;
            SocketWorking = false;
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

        public bool SendRequest<T>(int nQueryID, ServerRequestMessage message, T packet) where T : RequestPacket
        {
            if (!_clientScoket.IsConnected)
            {
                return false;
            }
            RecvBuff = null;

            var requestPacket = new ServerRequestData();
            requestPacket.QueryId = nQueryID;
            requestPacket.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(message));
            requestPacket.Packet = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(packet));

            var sginId = HUtil32.MakeLong((ushort)(nQueryID ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + 6));
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

        private void DBSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            //todo 粘包处理有问题，大量数据包会导致数据解析错乱
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                var data = e.Buff;
                if (PacketLen == 0 && data[0] == (byte)'#')
                {
                    PacketLen = BitConverter.ToInt32(data.AsSpan()[1..5]);
                }
                if (RecvBuff != null && RecvBuff.Length > 0)
                {
                    var tempBuff = new byte[RecvBuff.Length + e.BuffLen];
                    Buffer.BlockCopy(RecvBuff, 0, tempBuff, 0, RecvBuff.Length);
                    Buffer.BlockCopy(data, 0, tempBuff, RecvBuff.Length, e.BuffLen);
                    RecvBuff = tempBuff;
                }
                else
                {
                    RecvBuff = data;
                }
                var len = RecvBuff.Length - PacketLen;
                if (len > 0)
                {
                    data = RecvBuff[..PacketLen];
                    SocketWorking = true;
                    ProcessData(data);
                    RecvBuff = RecvBuff.AsSpan().Slice(PacketLen, len).ToArray();
                    PacketLen = RecvBuff.Length;
                    PacketLen = 0;
                }
                else if (len == 0)
                {
                    SocketWorking = true;
                    ProcessData(RecvBuff);
                    RecvBuff = null;
                    PacketLen = 0;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
            }
        }

        private void ProcessData(byte[] data)
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