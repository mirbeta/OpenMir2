using NLog;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameSvr.Services
{
    public class DBService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ClientScoket _clientScoket;
        private static int _packetLen;
        private byte[] _recvBuff;
        private bool _socketWorking;

        public DBService()
        {
            _clientScoket = new ClientScoket();
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.ReceivedDatagram += DBSocketRead;
            _clientScoket.OnError += DBSocketError;
            _socketWorking = false;
        }

        public void Start()
        {
            _clientScoket.Connect(M2Share.Config.sDBAddr, M2Share.Config.nDBPort);
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
            _recvBuff = null;

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
                if (_packetLen == 0 && data[0] == (byte)'#')
                {
                    _packetLen = BitConverter.ToInt32(data.AsSpan()[1..5]);
                }
                if (_recvBuff != null && _recvBuff.Length > 0)
                {
                    var tempBuff = new byte[_recvBuff.Length + e.BuffLen];
                    Buffer.BlockCopy(_recvBuff, 0, tempBuff, 0, _recvBuff.Length);
                    Buffer.BlockCopy(e.Buff, 0, tempBuff, _recvBuff.Length, e.BuffLen);
                    _recvBuff = tempBuff;
                }
                else
                {
                    _recvBuff = e.Buff;
                }
                var len = _recvBuff.Length - _packetLen;
                if (len > 0)
                {
                    var tempBuff = new byte[len];
                    Buffer.BlockCopy(_recvBuff, _packetLen, tempBuff, 0, len);
                    data = _recvBuff[.._packetLen];
                    _recvBuff = tempBuff;
                    _socketWorking = true;
                    ProcessData(data.ToArray());
                    _packetLen = tempBuff.Length;
                }
                else if (len == 0)
                {
                    _socketWorking = true;
                    ProcessData(_recvBuff);
                    _recvBuff = null;
                    _packetLen = 0;
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
                if (!_socketWorking) return;
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
            }
            finally
            {
                _socketWorking = false;
            }
        }
    }
}