using SystemModule;
using SystemModule.Sockets;

namespace GameSvr
{
    public class DBService
    {
        private readonly IClientScoket _clientScoket;
        private static int packetLen = 0;
        private byte[] DBSocketRecvBuff;
        private bool DBSocketWorking = false;

        public DBService()
        {
            _clientScoket = new IClientScoket();
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.ReceivedDatagram += DBSocketRead;
            _clientScoket.OnError += DBSocketError;
            DBSocketWorking = false;
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
            DBSocketRecvBuff = null;

            var requestPacket = new RequestServerPacket();
            requestPacket.QueryId = nQueryID;
            requestPacket.Message = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(packet));
            requestPacket.Packet = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(requet));

            var s = HUtil32.MakeLong(nQueryID ^ 170, requestPacket.Message.Length + requestPacket.Packet.Length + 6);
            var nCheckCode = BitConverter.GetBytes(s);
            var codeBuff = EDcode.EncodeBuffer(nCheckCode);
            requestPacket.CheckKey = codeBuff;

            var pk = requestPacket.GetBuffer();
            _clientScoket.Send(pk);

            return true;
        }

        private void DbScoketDisconnected(object sender, DSCClientConnectedEventArgs e)
        {
            _clientScoket.IsConnected = false;
            M2Share.ErrorMessage("数据库服务器[" + e.RemoteAddress + ':' + e.RemotePort + "]断开连接...");
        }

        private void DbScoketConnected(object sender, DSCClientConnectedEventArgs e)
        {
            _clientScoket.IsConnected = true;
            M2Share.MainOutMessage("数据库服务器[" + e.RemoteAddress + ':' + e.RemotePort + "]连接成功...", messageColor: System.ConsoleColor.Green);
        }

        private void DBSocketError(object sender, DSCClientErrorEventArgs e)
        {
            _clientScoket.IsConnected = false;
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    M2Share.ErrorMessage("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    M2Share.ErrorMessage("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    M2Share.ErrorMessage("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]链接超时...");
                    break;
            }
        }

        private void DBSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                var data = e.Buff;
                if (packetLen == 0 && data[0] == (byte)'#')
                {
                    packetLen = BitConverter.ToInt32(data[1..5]);
                }
                if (DBSocketRecvBuff != null && DBSocketRecvBuff.Length > 0)
                {
                    var tempBuff = new byte[DBSocketRecvBuff.Length + e.BuffLen];
                    Buffer.BlockCopy(DBSocketRecvBuff, 0, tempBuff, 0, DBSocketRecvBuff.Length);
                    Buffer.BlockCopy(e.Buff, 0, tempBuff, DBSocketRecvBuff.Length, e.BuffLen);
                    DBSocketRecvBuff = tempBuff;
                }
                else
                {
                    DBSocketRecvBuff = e.Buff;
                }
                var len = DBSocketRecvBuff.Length - packetLen;
                if (len > 0)
                {
                    var tempBuff = new byte[len];
                    Buffer.BlockCopy(DBSocketRecvBuff, packetLen, tempBuff, 0, len);
                    data = DBSocketRecvBuff[..packetLen];
                    DBSocketRecvBuff = tempBuff;
                    DBSocketWorking = true;
                    ProcessData(data);
                    packetLen = tempBuff.Length;
                }
                else if (len == 0)
                {
                    DBSocketWorking = true;
                    ProcessData(DBSocketRecvBuff);
                    DBSocketRecvBuff = null;
                    packetLen = 0;
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
                if (!DBSocketWorking) return;
                var responsePacket = Packets.ToPacket<RequestServerPacket>(data);
                if (responsePacket != null && responsePacket.PacketLen > 0)
                {
                    var respCheckCode = responsePacket.QueryId;
                    var nLen = responsePacket.Message.Length + responsePacket.Packet.Length + 6;
                    if (nLen >= 12)
                    {
                        var nCheckCode = HUtil32.MakeLong(respCheckCode ^ 170, nLen);
                        if (nCheckCode == BitConverter.ToInt32(responsePacket.CheckKey))
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
                DBSocketWorking = false;
            }
        }
    }
}