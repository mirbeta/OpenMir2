using NLog;
using SystemModule;
using SystemModule.Packet.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameSvr.Services
{
    public class DBService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ClientScoket _clientScoket;
        private static int _packetLen = 0;
        private byte[] _recvBuff;
        private bool _socketWorking = false;

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
            _clientScoket.Connect(M2Share.g_Config.sDBAddr, M2Share.g_Config.nDBPort);
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
            _clientScoket.Connect(M2Share.g_Config.sDBAddr, M2Share.g_Config.nDBPort);
        }

        public bool SendRequest<T>(int nQueryID, ServerMessagePacket packet, T requet) where T : CmdPacket
        {
            if (!_clientScoket.IsConnected)
            {
                return false;
            }
            _recvBuff = null;

            var requestPacket = new RequestServerPacket();
            requestPacket.QueryId = nQueryID;
            requestPacket.Message = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(packet));
            requestPacket.Packet = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(requet));

            var s = HUtil32.MakeLong(nQueryID ^ 170, requestPacket.Message.Length + requestPacket.Packet.Length + 6);
            requestPacket.Sgin = EDcode.EncodeBuffer(BitConverter.GetBytes(s));

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
                    _logger.Error("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logger.Error("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logger.Error("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]链接超时...");
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
                    _packetLen = BitConverter.ToInt32(data[1..5]);
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
                    ProcessData(data);
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
                var responsePacket = SystemModule.Packet.Packets.ToPacket<RequestServerPacket>(data);
                if (responsePacket != null && responsePacket.PacketLen > 0)
                {
                    var respCheckCode = responsePacket.QueryId;
                    var nLen = responsePacket.Message.Length + responsePacket.Packet.Length + 6;
                    if (nLen >= 12)
                    {
                        var nCheckCode = HUtil32.MakeLong(respCheckCode ^ 170, nLen);
                        var sginBuff = EDcode.DecodeBuff(responsePacket.Sgin);
                        if (nCheckCode == BitConverter.ToInt16(sginBuff))
                        {
                            HumDataService.AddToProcess(respCheckCode, responsePacket);
                        }
                        else
                        {
                            M2Share.g_Config.nLoadDBErrorCount++;
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