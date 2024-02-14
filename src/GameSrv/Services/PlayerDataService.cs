using TcpClient = TouchSocket.Sockets.TcpClient;

namespace GameSrv.Services
{
    /// <summary>
    /// 玩家数据读写服务
    /// </summary>
    public class PlayerDataService
    {
        private readonly TcpClient _tcpClient;
        private byte[] ReceiveBuffer { get; set; }
        private int BuffLen { get; set; }
        private bool SocketWorking { get; set; }

        public PlayerDataService()
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connected = DataSocketConnected;
            _tcpClient.Disconnected = DataSocketDisconnected;
            _tcpClient.Received = DataSocketRead;
            SocketWorking = false;
            ReceiveBuffer = new byte[10 * 2048];
        }

        public void Initialize()
        {
            _tcpClient.Setup(new TouchSocketConfig()
            .SetRemoteIPHost(new IPHost(IPAddress.Parse(SystemShare.Config.sDBAddr), SystemShare.Config.nDBPort))
            .ConfigureContainer(a =>
            {
                a.AddConsoleLogger();
            }).ConfigurePlugins(x =>
            {
                x.UseReconnection();
            }));
        }

        public async Task Start()
        {
            try
            {
                if (_tcpClient.Online)
                {
                    return;
                }
                await _tcpClient.ConnectAsync();
            }
            catch (TimeoutException)
            {
                LogService.Error($"链接数据库服务器[{SystemShare.Config.sDBAddr}:{SystemShare.Config.nDBPort}]超时...");
            }
            catch (Exception)
            {
                LogService.Error($"链接数据库服务器[{SystemShare.Config.sDBAddr}:{SystemShare.Config.nDBPort}]失败...");
            }
        }

        public void Stop()
        {
            _tcpClient.Close();
        }

        public bool IsConnected => _tcpClient.Online;

        public bool SendRequest<T>(int queryId, ServerRequestMessage message, T packet)
        {
            if (!_tcpClient.Online)
            {
                return false;
            }
            ServerRequestData requestPacket = new ServerRequestData();
            requestPacket.QueryId = queryId;
            requestPacket.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(message));
            requestPacket.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(packet));
            int signId = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + ServerDataPacket.FixedHeaderLen));
            requestPacket.Sign = EDCode.EncodeBuffer(BitConverter.GetBytes(signId));
            SendMessage(SerializerUtil.Serialize(requestPacket));
            return true;
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
            _tcpClient.Send(data);
        }

        private Task DataSocketDisconnected(ITcpClientBase sender, DisconnectEventArgs e)
        {
            LogService.Error("数据库服务器[" + sender.GetIPPort() + "]断开连接...");
            return Task.CompletedTask;
        }

        private Task DataSocketConnected(ITcpClient client, ConnectedEventArgs e)
        {
            LogService.Info("数据库服务器[" + client.RemoteIPHost + "]连接成功...");
            return Task.CompletedTask;
        }

        private Task DataSocketRead(TcpClient sender, ReceivedDataEventArgs e)
        {
            HUtil32.EnterCriticalSection(M2Share.UserDBCriticalSection);
            try
            {
                int nMsgLen = e.ByteBlock.Len;
                byte[] packetData = e.ByteBlock.Buffer;
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
                LogService.Error(exception);
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
                int srcOffset = 0;
                int nLen = buffLen;
                Span<byte> dataBuff = buff;
                while (nLen >= ServerDataPacket.FixedHeaderLen)
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
                    SocketWorking = true;
                    ServerRequestData messageData = SerializerUtil.Deserialize<ServerRequestData>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
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
                LogService.Error(ex);
            }
        }

        private void ProcessServerData(ServerRequestData responsePacket)
        {
            try
            {
                if (!SocketWorking)
                {
                    return;
                }
                if (responsePacket != null)
                {
                    int respCheckCode = responsePacket.QueryId;
                    int nLen = responsePacket.Message.Length + responsePacket.Packet.Length + ServerDataPacket.FixedHeaderLen;
                    if (nLen >= 12)
                    {
                        int queryId = HUtil32.MakeLong((ushort)(respCheckCode ^ 170), (ushort)nLen);
                        if (queryId <= 0 || responsePacket.Sign.Length <= 0)
                        {
                            SystemShare.Config.LoadDBErrorCount++;
                            return;
                        }
                        byte[] signatureBuff = BitConverter.GetBytes(queryId);
                        byte[] signBuff = EDCode.DecodeBuff(responsePacket.Sign);
                        if (BitConverter.ToInt16(signatureBuff) == BitConverter.ToInt16(signBuff))
                        {
                            PlayerDataHandler.Enqueue(respCheckCode, responsePacket);
                        }
                        else
                        {
                            SystemShare.Config.LoadDBErrorCount++;
                        }
                    }
                }
                else
                {
                    LogService.Error("错误的封包数据");
                }
            }
            finally
            {
                SocketWorking = false;
            }
        }
    }
}