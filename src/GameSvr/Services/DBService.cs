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
        public int DataLen { get; set; }
        private bool SocketWorking { get; set; }

        public DBService()
        {
            _clientScoket = new ClientScoket(new IPEndPoint(IPAddress.Parse(M2Share.Config.sDBAddr), M2Share.Config.nDBPort), 4096);
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.OnReceivedData += DBSocketRead;
            _clientScoket.OnError += DBSocketError;
            SocketWorking = false;
            RecvBuff = new byte[10000];
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
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                var data = e.Buff;
                if (PacketLen == 0 && data[0] == (byte)'#')
                {
                    PacketLen = BitConverter.ToInt32(data.AsSpan()[1..5]);
                }
                if (DataLen > 0)
                {
                    Buffer.BlockCopy(data, 0, RecvBuff, DataLen, data.Length);
                }
                else
                {
                    Buffer.BlockCopy(data, 0, RecvBuff, 0, data.Length);
                    DataLen += e.BuffLen;
                    return;
                }
                var len = PacketLen - DataLen;
                if (len > 0)
                {
                    data = RecvBuff[..PacketLen];
                    SocketWorking = true;
                    ProcessData(data);
                    var unData = RecvBuff[PacketLen..];
                    if (unData[0] == (byte)'#')
                    {
                        DataLen = unData.Length;
                        PacketLen = BitConverter.ToInt32(data.AsSpan()[1..5]);
                        Buffer.BlockCopy(unData, 0, RecvBuff, 0, unData.Length);
                        return;
                    }
                    else
                    {
                        PacketLen = 0;
                        DataLen = 0;
                        return;
                    }
                }
                else if (len == 0)
                {
                    SocketWorking = true;
                    ProcessData(RecvBuff);
                    PacketLen = 0;
                    DataLen = 0;
                    return;
                }
                DataLen += e.BuffLen;
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