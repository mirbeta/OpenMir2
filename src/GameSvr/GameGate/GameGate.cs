using GameSvr.Player;
using GameSvr.Services;
using GameSvr.World;
using NLog;
using System.Net.Sockets;
using System.Threading.Channels;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameSvr.GameGate
{
    public class GameGate
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly int GateIdx;
        private readonly GameGateInfo _gateInfo;
        private readonly GateSendQueue _sendQueue;
        private readonly object _runSocketSection;
        private readonly CancellationTokenSource _cancellation;
        private ClientCommandPacket clientMesaagePacket;
        /// <summary>
        /// 发送缓冲区
        /// </summary>
        private byte[] SendBuffer;
        private int SendLen;
        /// <summary>
        /// 数据接收缓冲区
        /// </summary>
        public byte[] ReceiveBuffer;
        public int ReceiveLen;
        
        public GameGate(int gateIdx, GameGateInfo gateInfo)
        {
            
            GateIdx = gateIdx;
            _gateInfo = gateInfo;
            _runSocketSection = new object();
            _sendQueue = new GateSendQueue(gateInfo);
            _cancellation = new CancellationTokenSource();
            clientMesaagePacket = new ClientCommandPacket();
            SendBuffer = new byte[1024 * 10];
            ReceiveBuffer = new byte[4096];
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

        internal void ProcessBuffer(byte[] packetBuff, int packetLen)
        {
            const string sExceptionMsg = "[Exception] GameGate::ProcessReceiveBuffer";
            var nLen = packetLen;
            var buffIndex = 0;
            try
            {
                while (nLen >= ServerMessagePacket.PacketSize)
                {
                    var packetHeader = ServerPackSerializer.Deserialize<ServerMessagePacket>(packetBuff[..ServerMessagePacket.PacketSize]);
                    if (packetHeader.PacketCode == Grobal2.RUNGATECODE)
                    {
                        var nCheckMsgLen = Math.Abs(packetHeader.PackLength) + ServerMessagePacket.PacketSize;
                        if (nLen < nCheckMsgLen && nCheckMsgLen < 0x8000)
                        {
                            break;
                        }
                        if (packetHeader.PackLength > 0)
                        {
                            var body = packetBuff[nCheckMsgLen..];
                            ExecGateBuffers(GateIdx, GateInfo, packetHeader, body, packetHeader.PackLength);
                        }
                        else
                        {
                            ExecGateBuffers(GateIdx, GateInfo, packetHeader, null, packetHeader.PackLength);
                        }
                        nLen -= nCheckMsgLen;
                        if (nLen <= 0)
                        {
                            break;
                        }
                        ReceiveLen = nLen;
                        buffIndex = 0;
                    }
                    else
                    {
                        buffIndex++;
                        //if (buffIndex > memoryStream.Length)//异常数据，整段数据丢弃
                        //{
                        //    memoryStream.Position = 0;
                        //    _gateInfo.BuffLen = 0;
                        //    return;
                        //}
                        //memoryStream.Position = buffIndex;
                        nLen -= 1;
                    }
                    if (nLen < ServerMessagePacket.PacketSize)
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
                MemoryCopy.BlockCopy(packetBuff, 0, ReceiveBuffer, 0, nLen);
                ReceiveLen = nLen;
            }
            else
            {
                ReceiveLen = 0;
            }
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr -> GameGate
        /// </summary>
        /// <returns></returns>
        public void HandleSendBuffer(int sendBuffLen,ReadOnlySpan<byte> buffer, byte[] msgBuffer)
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
                if (sendBuffLen == 0) //不发送空包
                {
                    return;
                }
                if (GateInfo.nSendChecked == 0 && GateInfo.nSendBlockCount + sendBuffLen >= M2Share.Config.CheckBlock * 10)
                {
                    if (GateInfo.nSendBlockCount == 0 && M2Share.Config.CheckBlock * 10 <= sendBuffLen)
                    {
                        return;
                    }
                    SendCheck(GateInfo.Socket, Grobal2.GM_RECEIVE_OK);
                    GateInfo.nSendChecked = 1;
                    GateInfo.dwSendCheckTick = HUtil32.GetTickCount();
                }
                MemoryCopy.BlockCopy(BitConverter.GetBytes(sendBuffLen), 0, SendBuffer, 0, 4);
                MemoryCopy.BlockCopy(buffer, 0, SendBuffer, 4, buffer.Length);
                if (msgBuffer != null && msgBuffer.Length > 0)
                {
                    MemoryCopy.BlockCopy(msgBuffer, 0, SendBuffer, buffer.Length + 4, msgBuffer.Length);
                }
                if (GateInfo.Socket != null && GateInfo.Socket.Connected)
                {
                    _sendQueue.AddToQueue(SendBuffer[..sendBuffLen]);
                    GateInfo.nSendCount++;
                    GateInfo.nSendBytesCount += sendBuffLen;
                    GateInfo.nSendBlockCount += sendBuffLen;
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
            var msgHeader = new ServerMessagePacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = nIdent,
                PackLength = 0
            };
            if (Socket.Connected)
            {
                var data = ServerPackSerializer.Serialize(msgHeader);
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// 执行网关封包消息
        /// </summary>
        private void ExecGateBuffers(int GateIdx, GameGateInfo Gate, ServerMessagePacket MsgHeader, Span<byte> MsgBuff, int nMsgLen)
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
                                if (GateUser.Certification && nMsgLen >= 12)
                                {
                                    clientMesaagePacket.Recog = BitConverter.ToInt32(MsgBuff[..4]);
                                    clientMesaagePacket.Ident = BitConverter.ToUInt16(MsgBuff.Slice(4, 2));
                                    clientMesaagePacket.Param = BitConverter.ToUInt16(MsgBuff.Slice(6, 2));
                                    clientMesaagePacket.Tag = BitConverter.ToUInt16(MsgBuff.Slice(8, 2));
                                    clientMesaagePacket.Series = BitConverter.ToUInt16(MsgBuff.Slice(10, 2));
                                    //var defMsg = Packets.ToPacket<ClientMesaagePacket>(MsgBuff);
                                    if (nMsgLen == 12)
                                    {
                                        WorldServer.ProcessUserMessage(GateUser.PlayObject, clientMesaagePacket, null);
                                    }
                                    else
                                    {
                                        var sMsg = EDCode.DeCodeString(HUtil32.GetString(MsgBuff, 12, MsgBuff.Length - 13));
                                        WorldServer.ProcessUserMessage(GateUser.PlayObject, clientMesaagePacket, sMsg);
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
            var nPlayTime = 0L;
            byte[] HWID = MD5.EmptyDigest;
            TSessInfo SessInfo;
            const string sExceptionMsg = "[Exception] TRunSocket::DoClientCertification";
            const string sDisable = "*disable*";
            try
            {
                if (string.IsNullOrEmpty(GateUser.Account))
                {
                    if (HUtil32.TagCount(sMsg, '!') > 0)
                    {
                        HUtil32.ArrestStringEx(sMsg, "#", "!", ref sMsg);
                        var packetMsg = sMsg.AsSpan()[1..].ToString();
                        if (GetCertification(packetMsg, ref sAccount, ref sChrName, ref nSessionID, ref nClientVersion, ref boFlag, ref HWID))
                        {
                            SessInfo = IdSrvClient.Instance.GetAdmission(sAccount, GateUser.sIPaddr, nSessionID, ref nPayMode, ref nPayMent, ref nPlayTime);
                            if (SessInfo != null && nPayMent > 0)
                            {
                                GateUser.Certification = true;
                                GateUser.Account = sAccount.Trim();
                                GateUser.sChrName = sChrName.Trim();
                                GateUser.SessionID = nSessionID;
                                GateUser.ClientVersion = nClientVersion;
                                GateUser.SessInfo = SessInfo;
                                var loadRcdInfo = new LoadDBInfo
                                {
                                    Account = sAccount,
                                    ChrName = sChrName,
                                    sIPaddr = GateUser.sIPaddr,
                                    SessionID = nSessionID,
                                    SoftVersionDate = nClientVersion,
                                    PayMent = nPayMent,
                                    PayMode = nPayMode,
                                    SocketId = nSocket,
                                    GSocketIdx = GateUser.SocketId,
                                    GateIdx = GateIdx,
                                    NewUserTick = HUtil32.GetTickCount(),
                                    PlayTime = nPlayTime
                                };
                                M2Share.FrontEngine.AddToLoadRcdList(loadRcdInfo);
                            }
                            else
                            {
                                GateUser.Account = sDisable;
                                GateUser.Certification = false;
                                CloseUser(nSocket);
                                _logger.Warn($"会话验证失败.Account:{sAccount} SessionId:{nSessionID} Address:{GateUser.sIPaddr}");
                            }
                        }
                        else
                        {
                            GateUser.Account = sDisable;
                            GateUser.Certification = false;
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
                                        GateUser.PlayObject.BoSoftClose = true;
                                    }
                                }
                                if (GateUser.PlayObject != null && GateUser.PlayObject.Ghost && !GateUser.PlayObject.BoReconnection)
                                {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(GateUser.Account, GateUser.SessionID);
                                }
                                if (GateUser.PlayObject != null && GateUser.PlayObject.BoSoftClose && GateUser.PlayObject.BoReconnection && GateUser.PlayObject.BoEmergencyClose)
                                {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(GateUser.Account, GateUser.SessionID);
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
                Account = string.Empty,
                sChrName = string.Empty,
                sIPaddr = sIPaddr,
                nSocket = socket,
                SocketId = socketId,
                SessionID = 0,
                UserEngine = null,
                FrontEngine = null,
                PlayObject = null,
                dwNewUserTick = HUtil32.GetTickCount(),
                Certification = false
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
            var MsgHeader = new ServerMessagePacket();
            MsgHeader.PacketCode = Grobal2.RUNGATECODE;
            MsgHeader.Socket = nSocket;
            MsgHeader.SessionId = socketId;
            MsgHeader.Ident = Grobal2.GM_SERVERUSERINDEX;
            MsgHeader.ServerIndex = (ushort)nUserIdex;
            MsgHeader.PackLength = 0;
            if (Socket.Connected)
            {
                var data = ServerPackSerializer.Serialize(MsgHeader);
                Socket.Send(data, 0, data.Length, SocketFlags.None);
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