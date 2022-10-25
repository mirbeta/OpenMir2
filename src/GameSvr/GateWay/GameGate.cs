using GameSvr.Player;
using GameSvr.Services;
using NLog;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Channels;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.GateWay
{
    public class GameGate
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly int _gateIdx;
        private readonly GameGateInfo _gateInfo;
        private readonly GateSendQueue _sendQueue;
        private readonly object _runSocketSection;
        private byte[] _gateBuff;
        private int _buffLen;
        private readonly CancellationTokenSource _cancellation;
        private readonly PacketHeader packetHeader;
        private readonly ClientMesaagePacket clientMesaagePacket;

        public GameGate(int gateIdx, GameGateInfo gateInfo)
        {
            _gateIdx = gateIdx;
            _gateInfo = gateInfo;
            _runSocketSection = new object();
            _sendQueue = new GateSendQueue(gateInfo);
            _cancellation = new CancellationTokenSource();
            packetHeader = new PacketHeader();
            clientMesaagePacket = new ClientMesaagePacket();
        }

        public GameGateInfo GateInfo => _gateInfo;

        public void StartGateQueue()
        {
            _sendQueue.ProcessSendQueue(_cancellation);
        }

        public void Stop()
        {
            _cancellation.CancelAfter(3000);
        }

        /// <summary>
        /// 处理接收到的数据
        /// GameGate -> GameSvr
        /// </summary>
        public void ProcessReceiveBuffer(int nMsgLen, Span<byte> data)
        {
            const string sExceptionMsg = "[Exception] GameGate::ProcessReceiveBuffer";
            try
            {
                if (_buffLen > 0)
                {
                    var buffSize = _buffLen + nMsgLen;
                    if (buffSize > _buffLen)
                    {
                        Span<byte> tempBuff = stackalloc byte[buffSize];
                        MemoryCopy.BlockCopy(_gateBuff, 0, tempBuff, 0, _buffLen);
                        MemoryCopy.BlockCopy(data, 0, tempBuff, _buffLen, nMsgLen);
                        ProcessBuffer(tempBuff, buffSize);
                    }
                }
                else
                {
                    ProcessBuffer(data, nMsgLen);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
        }

        private void ProcessBuffer(Span<byte> packetBuff,int packetLen)
        {
            const string sExceptionMsg = "[Exception] GameGate::ProcessReceiveBuffer -> ExecGateMsg";
            var nLen = packetLen;
            var buffIndex = 0;
            Span<byte> protoBuff = packetBuff;
            try
            {
                while (nLen >= PacketHeader.PacketSize)
                {
                    Span<byte> packetHead = protoBuff[..20];
                    if (packetHead.Length < 20)
                    {
                        break;
                    }
                    packetHeader.PacketCode = BitConverter.ToUInt32(packetHead[..4]);
                    packetHeader.PackLength = BitConverter.ToInt32(packetHead.Slice(16, 4));
                    var nCheckMsgLen = Math.Abs(packetHeader.PackLength) + PacketHeader.PacketSize;
                    if (packetHeader.PacketCode == Grobal2.RUNGATECODE && nCheckMsgLen < 0x8000)
                    {
                        if (nLen < nCheckMsgLen)
                        {
                            break;
                        }
                        packetHeader.Socket = BitConverter.ToInt32(packetHead.Slice(4, 4));
                        packetHeader.SessionId = BitConverter.ToUInt16(packetHead.Slice(8, 2));
                        packetHeader.ServerIndex = BitConverter.ToInt32(packetHead.Slice(12, 4));
                        packetHeader.Ident = BitConverter.ToUInt16(packetHead.Slice(10, 2));
                        Span<byte> body = null;
                        if (packetHeader.PackLength > 0)
                        {
                            body = protoBuff[..nCheckMsgLen]; //获取整个封包内容,包括消息头和消息体
                            body = body[PacketHeader.PacketSize..];
                        }
                        //M2Share.GateMgr.AddGameGateQueue(_gateIdx, msgHeader, body); //添加到处理队列
                        ExecGateBuffers(packetHeader, body);
                        nLen -= nCheckMsgLen;
                        if (nLen <= 0)
                        {
                            break;
                        }
                        protoBuff = protoBuff.Slice(nCheckMsgLen, nLen);
                        _buffLen = nLen;
                        buffIndex = 0;
                    }
                    else
                    {
                        _logger.Error($"{sExceptionMsg} Offset:{buffIndex} DataLen:{protoBuff.Length}");
                        buffIndex++;
                        if (buffIndex > protoBuff.Length) //异常数据，直接丢弃
                        {
                            _gateBuff = null;
                            _buffLen = 0;
                            break;
                        }
                        protoBuff = protoBuff.Slice(buffIndex, PacketHeader.PacketSize);
                        nLen -= 1;
                    }
                    if (nLen < PacketHeader.PacketSize)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
            if (nLen > 0)
            {
                _gateBuff = protoBuff[..nLen].ToArray();
                _buffLen = nLen;
            }
            else
            {
                _gateBuff = null;
                _buffLen = 0;
            }
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr -> GameGate
        /// </summary>
        /// <returns></returns>
        public void HandleSendBuffer(byte[] buffer)
        {
            if (!GateInfo.BoUsed && GateInfo.Socket == null)
            {
                return;
            }
            const string sExceptionMsg = "[Exception] TRunSocket::SendGateBuffers -> SendBuff";
            var dwRunTick = HUtil32.GetTickCount();
            if (GateInfo.nSendChecked > 0)// 如果网关未回复状态消息，则不再发送数据
            {
                if ((HUtil32.GetTickCount() - GateInfo.dwSendCheckTick) > M2Share.g_dwSocCheckTimeOut) // 2 * 1000
                {
                    GateInfo.nSendChecked = 0;
                    GateInfo.nSendBlockCount = 0;
                }
                return;
            }
            try
            {
                var nSendBuffLen = buffer.Length;
                if (GateInfo.nSendChecked == 0 && GateInfo.nSendBlockCount + nSendBuffLen >= M2Share.Config.CheckBlock * 10)
                {
                    if (GateInfo.nSendBlockCount == 0 && M2Share.Config.CheckBlock * 10 <= nSendBuffLen)
                    {
                        return;
                    }
                    SendCheck(GateInfo.Socket, Grobal2.GM_RECEIVE_OK);
                    GateInfo.nSendChecked = 1;
                    GateInfo.dwSendCheckTick = HUtil32.GetTickCount();
                }
                var sendBuffer = new byte[buffer.Length - 4];
                Buffer.BlockCopy(buffer, 4, sendBuffer, 0, sendBuffer.Length);
                nSendBuffLen = sendBuffer.Length;
                if (nSendBuffLen > 0)
                {
                    while (true)
                    {
                        if (M2Share.Config.SendBlock <= nSendBuffLen)
                        {
                            if (GateInfo.Socket != null)
                            {
                                var sendBuff = new byte[M2Share.Config.SendBlock];
                                Buffer.BlockCopy(sendBuffer, 0, sendBuff, 0, M2Share.Config.SendBlock);
                                _sendQueue.AddToQueue(sendBuff);
                                GateInfo.nSendCount++;
                                GateInfo.nSendBytesCount += M2Share.Config.SendBlock;
                            }
                            GateInfo.nSendBlockCount += M2Share.Config.SendBlock;
                            nSendBuffLen -= M2Share.Config.SendBlock;
                            var tempBuff = new byte[nSendBuffLen];
                            Buffer.BlockCopy(sendBuffer, M2Share.Config.SendBlock, tempBuff, 0, nSendBuffLen);
                            sendBuffer = tempBuff;
                            continue;
                        }
                        if (GateInfo.Socket != null)
                        {
                            _sendQueue.AddToQueue(sendBuffer);
                            GateInfo.nSendCount++;
                            GateInfo.nSendBytesCount += nSendBuffLen;
                            GateInfo.nSendBlockCount += nSendBuffLen;
                        }
                        break;
                    }
                }
                if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.SocLimit)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.StackTrace, MessageType.Error);
            }
        }

        private void SendCheck(Socket Socket, ushort nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var msgHeader = new PacketHeader
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = nIdent,
                PackLength = 0
            };
            if (Socket.Connected)
            {
                var data = msgHeader.GetBuffer();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// 执行网关封包消息
        /// </summary>
        private void ExecGateBuffers(PacketHeader packet, Span<byte> data)
        {
            if (packet.PackLength == 0)
            {
                ExecGateMessage(_gateIdx, GateInfo, packet, null, packet.PackLength);
            }
            else
            {
                ExecGateMessage(_gateIdx, GateInfo, packet, data, packet.PackLength);
            }
        }

        private bool GetCertification(string sMsg, ref string sAccount, ref string sChrName, ref int nSessionID, ref int nClientVersion, ref bool boFlag, ref byte[] tHWID)
        {
            var result = false;
            var sCodeStr = string.Empty;
            var sClientVersion = string.Empty;
            var sHWID = string.Empty;
            var sIdx = string.Empty;
            const string sExceptionMsg = "[Exception] TRunSocket::DoClientCertification -> GetCertification";
            try
            {
                var sData = EDCode.DeCodeString(sMsg);
                if (sData.Length > 2 && sData[0] == '*' && sData[1] == '*')
                {
                    sData = sData.AsSpan().Slice(2, sData.Length - 2).ToString();
                    sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sChrName, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sCodeStr, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sClientVersion, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sIdx, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sHWID, HUtil32.Backslash);
                    nSessionID = HUtil32.StrToInt(sCodeStr, 0);
                    if (sIdx == "0")
                    {
                        boFlag = true;
                    }
                    else
                    {
                        boFlag = false;
                    }
                    if (!string.IsNullOrEmpty(sAccount) && !string.IsNullOrEmpty(sChrName) && nSessionID >= 2 && !string.IsNullOrEmpty(sHWID))
                    {
                        nClientVersion = HUtil32.StrToInt(sClientVersion, 0);
                        tHWID = MD5.MD5UnPrInt(sHWID);
                        result = true;
                    }
                    M2Share.Log.Debug($"Account:[{sAccount}] ChrName:[{sChrName}] Code:[{sCodeStr}] ClientVersion:[{sClientVersion}] HWID:[{sHWID}]");
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg, MessageType.Error);
            }
            return result;
        }

        private void DoClientCertification(int GateIdx, GateUserInfo GateUser, int nSocket, string sMsg)
        {
            var sAccount = string.Empty;
            var sChrName = string.Empty;
            var nSessionID = 0;
            var boFlag = false;
            var nClientVersion = 0;
            var nPayMent = 0;
            var nPayMode = 0;
            byte[] HWID = MD5.g_MD5EmptyDigest;
            TSessInfo SessInfo;
            const string sExceptionMsg = "[Exception] TRunSocket::DoClientCertification";
            const string sDisable = "*disable*";
            try
            {
                if (string.IsNullOrEmpty(GateUser.sAccount))
                {
                    if (HUtil32.TagCount(sMsg, '!') > 0)
                    {
                        HUtil32.ArrestStringEx(sMsg, "#", "!", ref sMsg);
                        var packetMsg = sMsg.AsSpan()[1..].ToString();
                        if (GetCertification(packetMsg, ref sAccount, ref sChrName, ref nSessionID, ref nClientVersion, ref boFlag, ref HWID))
                        {
                            SessInfo = IdSrvClient.Instance.GetAdmission(sAccount, GateUser.sIPaddr, nSessionID, ref nPayMode, ref nPayMent);
                            if (SessInfo != null && nPayMent > 0)
                            {
                                GateUser.boCertification = true;
                                GateUser.sAccount = sAccount.Trim();
                                GateUser.sChrName = sChrName.Trim();
                                GateUser.nSessionID = nSessionID;
                                GateUser.nClientVersion = nClientVersion;
                                GateUser.SessInfo = SessInfo;
                                try
                                {
                                    M2Share.FrontEngine.AddToLoadRcdList(sAccount, sChrName, GateUser.sIPaddr, boFlag, nSessionID, nPayMent, nPayMode, nClientVersion, nSocket, GateUser.SocketId, GateIdx);
                                }
                                catch
                                {
                                    _logger.Error(sExceptionMsg);
                                }
                            }
                            else
                            {
                                GateUser.sAccount = sDisable;
                                GateUser.boCertification = false;
                                CloseUser(nSocket);
                                _logger.Warn($"会话验证失败.Account:{sAccount} SessionId:{nSessionID} Address:{GateUser.sIPaddr}");
                            }
                        }
                        else
                        {
                            GateUser.sAccount = sDisable;
                            GateUser.boCertification = false;
                            CloseUser(nSocket);
                        }
                    }
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        public void CloseUser(int nSocket)
        {
            GateUserInfo GateUser;
            if (GateInfo.UserList.Count > 0)
            {
                HUtil32.EnterCriticalSections(_runSocketSection);
                try
                {
                    for (var i = 0; i < GateInfo.UserList.Count; i++)
                    {
                        if (GateInfo.UserList[i] != null)
                        {
                            GateUser = GateInfo.UserList[i];
                            if (GateUser == null)
                            {
                                continue;
                            }
                            if (GateUser.nSocket == nSocket)
                            {
                                if (GateUser.FrontEngine != null)
                                {
                                    GateUser.FrontEngine.DeleteHuman(i, GateUser.nSocket);
                                }
                                if (GateUser.PlayObject != null)
                                {
                                    if (!GateUser.PlayObject.OffLineFlag)
                                    {
                                        GateUser.PlayObject.m_boSoftClose = true;
                                    }
                                }
                                if (GateUser.PlayObject != null && GateUser.PlayObject.Ghost && !GateUser.PlayObject.m_boReconnection)
                                {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(GateUser.sAccount, GateUser.nSessionID);
                                }
                                if (GateUser.PlayObject != null && GateUser.PlayObject.m_boSoftClose && GateUser.PlayObject.m_boReconnection && GateUser.PlayObject.m_boEmergencyClose)
                                {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(GateUser.sAccount, GateUser.nSessionID);
                                }
                                GateInfo.UserList[i] = null;
                                GateInfo.nUserCount -= 1;
                                break;
                            }
                        }
                    }
                }
                finally
                {
                    HUtil32.LeaveCriticalSections(_runSocketSection);
                }
            }
        }

        private int OpenNewUser(int socket, ushort socketId, string sIPaddr, IList<GateUserInfo> UserList)
        {
            int result;
            var GateUser = new GateUserInfo
            {
                sAccount = string.Empty,
                sChrName = string.Empty,
                sIPaddr = sIPaddr,
                nSocket = socket,
                SocketId = socketId,
                nSessionID = 0,
                UserEngine = null,
                FrontEngine = null,
                PlayObject = null,
                dwNewUserTick = HUtil32.GetTickCount(),
                boCertification = false
            };
            for (var i = 0; i < UserList.Count; i++)
            {
                if (UserList[i] == null)
                {
                    UserList[i] = GateUser;
                    result = i;
                    return result;
                }
            }
            UserList.Add(GateUser);
            result = UserList.Count - 1;
            return result;
        }

        private void SendNewUserMsg(Socket Socket, int nSocket, ushort socketId, int nUserIdex)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var MsgHeader = new PacketHeader();
            MsgHeader.PacketCode = Grobal2.RUNGATECODE;
            MsgHeader.Socket = nSocket;
            MsgHeader.SessionId = socketId;
            MsgHeader.Ident = Grobal2.GM_SERVERUSERINDEX;
            MsgHeader.ServerIndex = (ushort)nUserIdex;
            MsgHeader.PackLength = 0;
            if (Socket.Connected)
            {
                var data = MsgHeader.GetBuffer();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        private void ExecGateMessage(int GateIdx, GameGateInfo Gate, PacketHeader MsgHeader, Span<byte> MsgBuff, int nMsgLen)
        {
            int nUserIdx;
            const string sExceptionMsg = "[Exception] TRunSocket::ExecGateMsg";
            try
            {
                switch (MsgHeader.Ident)
                {
                    case Grobal2.GM_OPEN:
                        var sIPaddr = HUtil32.GetString(MsgBuff, nMsgLen);
                        nUserIdx = OpenNewUser(MsgHeader.Socket, MsgHeader.SessionId, sIPaddr, Gate.UserList);
                        SendNewUserMsg(Gate.Socket, MsgHeader.Socket, MsgHeader.SessionId, nUserIdx + 1);
                        Gate.nUserCount++;
                        break;
                    case Grobal2.GM_CLOSE:
                        CloseUser(MsgHeader.Socket);
                        break;
                    case Grobal2.GM_CHECKCLIENT:
                        Gate.boSendKeepAlive = true;
                        break;
                    case Grobal2.GM_RECEIVE_OK:
                        Gate.nSendChecked = 0;
                        Gate.nSendBlockCount = 0;
                        break;
                    case Grobal2.GM_DATA:
                        GateUserInfo GateUser = null;
                        if (MsgHeader.ServerIndex >= 1)
                        {
                            nUserIdx = MsgHeader.ServerIndex - 1;
                            if (Gate.UserList.Count > nUserIdx)
                            {
                                GateUser = Gate.UserList[nUserIdx];
                                if (GateUser != null && GateUser.nSocket != MsgHeader.Socket)
                                {
                                    GateUser = null;
                                }
                            }
                        }
                        if (GateUser == null)
                        {
                            for (var i = 0; i < Gate.UserList.Count; i++)
                            {
                                if (Gate.UserList[i] == null)
                                {
                                    continue;
                                }
                                if (Gate.UserList[i].nSocket == MsgHeader.Socket)
                                {
                                    GateUser = Gate.UserList[i];
                                    break;
                                }
                            }
                        }
                        if (GateUser != null)
                        {
                            if (GateUser.PlayObject != null && GateUser.UserEngine != null)
                            {
                                if (GateUser.boCertification && nMsgLen >= 12)
                                {
                                    clientMesaagePacket.Recog = BitConverter.ToInt32(MsgBuff[..4]);
                                    clientMesaagePacket.Ident = BitConverter.ToUInt16(MsgBuff.Slice(4, 2));
                                    clientMesaagePacket.Param = BitConverter.ToUInt16(MsgBuff.Slice(6, 2));
                                    clientMesaagePacket.Tag = BitConverter.ToUInt16(MsgBuff.Slice(8, 2));
                                    clientMesaagePacket.Series = BitConverter.ToUInt16(MsgBuff.Slice(10, 2));
                                    //var defMsg = Packets.ToPacket<ClientMesaagePacket>(MsgBuff);
                                    if (nMsgLen == 12)
                                    {
                                        M2Share.WorldEngine.ProcessUserMessage(GateUser.PlayObject, clientMesaagePacket, null);
                                    }
                                    else
                                    {
                                        var sMsg = EDCode.DeCodeString(HUtil32.GetString(MsgBuff, 12, MsgBuff.Length - 13));
                                        M2Share.WorldEngine.ProcessUserMessage(GateUser.PlayObject, clientMesaagePacket, sMsg);
                                    }
                                }
                            }
                            else
                            {
                                var sMsg = HUtil32.StrPas(MsgBuff);
                                DoClientCertification(GateIdx, GateUser, MsgHeader.Socket, sMsg);
                            }
                        }
                        break;
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nSocket, PlayObject PlayObject)
        {
            HUtil32.EnterCriticalSection(_runSocketSection);
            try
            {
                for (var i = 0; i < GateInfo.UserList.Count; i++)
                {
                    var gateUserInfo = GateInfo.UserList[i];
                    if (gateUserInfo != null && gateUserInfo.nSocket == nSocket)
                    {
                        gateUserInfo.FrontEngine = null;
                        gateUserInfo.UserEngine = M2Share.WorldEngine;
                        gateUserInfo.PlayObject = PlayObject;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(_runSocketSection);
            }
        }

        private class GateSendQueue
        {
            private readonly Channel<byte[]> _sendQueue;
            private readonly Socket _sendSocket;
            private readonly GameGateInfo _gameGate;

            public GateSendQueue(GameGateInfo gameGate)
            {
                _sendQueue = Channel.CreateUnbounded<byte[]>();
                _gameGate = gameGate;
                _sendSocket = gameGate.Socket;
            }

            /// <summary>
            /// 获取队列消息数量
            /// </summary>
            public int GetQueueCount => _sendQueue.Reader.Count;

            /// <summary>
            /// 添加到发送队列
            /// </summary>
            public void AddToQueue(byte[] buffer)
            {
                _sendQueue.Writer.TryWrite(buffer);
            }

            /// <summary>
            /// 处理队列数据并发送到GameGate
            /// GameSvr -> GameGate
            /// </summary>
            public void ProcessSendQueue(CancellationTokenSource cancellation)
            {
                Task.Factory.StartNew(async () =>
                {
                    while (await _sendQueue.Reader.WaitToReadAsync(cancellation.Token))
                    {
                        while (_sendQueue.Reader.TryRead(out var buffer))
                        {
                            if (_sendSocket.Connected)
                            {
                                //todo 此处异步发送效率比同步效率要低很多,不知道为什么,暂时先保持同步发送
                                _sendSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                            }
                        }
                    }
                }, cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }
    }
}