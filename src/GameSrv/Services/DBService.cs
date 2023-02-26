using NLog;
using System.Net;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameSrv.Services {
    public class DBService {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ScoketClient _clientScoket;
        private byte[] ReceiveBuffer { get; set; }
        public int BuffLen { get; set; }
        private bool SocketWorking { get; set; }

        public DBService() {
            _clientScoket = new ScoketClient(new IPEndPoint(IPAddress.Parse(M2Share.Config.sDBAddr), M2Share.Config.nDBPort), 4096);
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.OnReceivedData += DBSocketRead;
            _clientScoket.OnError += DBSocketError;
            SocketWorking = false;
            ReceiveBuffer = new byte[10 * 2048];
        }

        public void Start() {
            _clientScoket.Connect();
        }

        public void Stop() {
            _clientScoket.Disconnect();
        }

        public void CheckConnected() {
            if (_clientScoket.IsConnected) {
                return;
            }
            if (_clientScoket.IsBusy) {
                return;
            }
            _clientScoket.Connect(M2Share.Config.sDBAddr, M2Share.Config.nDBPort);
        }

        public bool SendRequest<T>(int queryId, ServerRequestMessage message, T packet) {
            if (!_clientScoket.IsConnected) {
                return false;
            }
            ServerRequestData requestPacket = new ServerRequestData();
            requestPacket.QueryId = queryId;
            requestPacket.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(message));
            requestPacket.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(packet));
            int sginId = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + 6));
            requestPacket.Sgin = EDCode.EncodeBuffer(BitConverter.GetBytes(sginId));
            SendMessage(SerializerUtil.Serialize(requestPacket));
            return true;
        }

        private void SendMessage(byte[] sendBuffer) {
            using MemoryStream memoryStream = new MemoryStream();
            using BinaryWriter backingStream = new BinaryWriter(memoryStream);
            ServerDataPacket serverMessage = new ServerDataPacket {
                PacketCode = Grobal2.RunGateCode,
                PacketLen = (short)sendBuffer.Length
            };
            backingStream.Write(serverMessage.GetBuffer());
            backingStream.Write(sendBuffer);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            _clientScoket.Send(data);
        }

        private void DbScoketDisconnected(object sender, DSCClientConnectedEventArgs e) {
            _clientScoket.IsConnected = false;
            _logger.Error("数据库服务器[" + e.RemoteEndPoint + "]断开连接...");
        }

        private void DbScoketConnected(object sender, DSCClientConnectedEventArgs e) {
            _clientScoket.IsConnected = true;
            _logger.Info("数据库服务器[" + e.RemoteEndPoint + "]连接成功...");
        }

        private void DBSocketError(object sender, DSCClientErrorEventArgs e) {
            _clientScoket.IsConnected = false;
            switch (e.ErrorCode) {
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

        private void ProcessServerPacket(byte[] buff, int buffLen) {
            try {
                int srcOffset = 0;
                int nLen = buffLen;
                Span<byte> dataBuff = buff;
                while (nLen >= ServerDataPacket.FixedHeaderLen) {
                    Span<byte> packetHead = dataBuff[..ServerDataPacket.FixedHeaderLen];
                    ServerDataPacket message = ServerPacket.ToPacket<ServerDataPacket>(packetHead);
                    if (message.PacketCode != Grobal2.RunGateCode) {
                        srcOffset++;
                        dataBuff = dataBuff.Slice(srcOffset, ServerDataPacket.FixedHeaderLen);
                        nLen -= 1;
                        _logger.Debug($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                        continue;
                    }
                    int nCheckMsgLen = Math.Abs(message.PacketLen + ServerDataPacket.FixedHeaderLen);
                    if (nCheckMsgLen > nLen) {
                        break;
                    }
                    SocketWorking = true;
                    ServerRequestData messageData = SerializerUtil.Deserialize<ServerRequestData>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
                    ProcessServerData(messageData);
                    nLen -= nCheckMsgLen;
                    if (nLen <= 0) {
                        break;
                    }
                    dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
                    BuffLen = nLen;
                    srcOffset = 0;
                    if (nLen < ServerDataPacket.FixedHeaderLen) {
                        break;
                    }
                }
                if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    MemoryCopy.BlockCopy(dataBuff, 0, ReceiveBuffer, 0, nLen);
                    BuffLen = nLen;
                }
                else {
                    BuffLen = 0;
                }
            }
            catch (Exception ex) {
                _logger.Error(ex);
            }
        }

        private void DBSocketRead(object sender, DSCClientDataInEventArgs e) {
            HUtil32.EnterCriticalSection(M2Share.UserDBCriticalSection);
            try {
                int nMsgLen = e.BuffLen;
                byte[] packetData = e.Buff;
                if (BuffLen > 0) {
                    MemoryCopy.BlockCopy(packetData, 0, ReceiveBuffer, BuffLen, packetData.Length);
                    ProcessServerPacket(ReceiveBuffer, BuffLen + nMsgLen);
                }
                else {
                    ProcessServerPacket(packetData, nMsgLen);
                }
            }
            catch (Exception exception) {
                _logger.Error(exception);
            }
            finally {
                HUtil32.LeaveCriticalSection(M2Share.UserDBCriticalSection);
            }
        }

        private void ProcessServerData(ServerRequestData responsePacket) {
            try {
                if (!SocketWorking) return;
                if (responsePacket != null) {
                    int respCheckCode = responsePacket.QueryId;
                    int nLen = responsePacket.Message.Length + responsePacket.Packet.Length + 6;
                    if (nLen >= 12) {
                        int queryId = HUtil32.MakeLong((ushort)(respCheckCode ^ 170), (ushort)nLen);
                        if (queryId <= 0 || responsePacket.Sgin.Length <= 0) {
                            M2Share.Config.nLoadDBErrorCount++;
                            return;
                        }
                        byte[] signatureBuff = BitConverter.GetBytes(queryId);
                        byte[] sginBuff = EDCode.DecodeBuff(responsePacket.Sgin);
                        short signatureId = BitConverter.ToInt16(signatureBuff);
                        if (signatureId == BitConverter.ToInt16(sginBuff)) {
                            PlayerDataService.Enqueue(respCheckCode, responsePacket);
                        }
                        else {
                            M2Share.Config.nLoadDBErrorCount++;
                        }
                    }
                }
                else {
                    _logger.Error("错误的封包数据");
                }
            }
            finally {
                SocketWorking = false;
            }
        }
    }
}