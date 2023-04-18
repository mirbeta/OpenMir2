using GameSrv.Player;
using GameSrv.Services;
using GameSrv.World;
using NLog;
using System.Buffers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;

namespace GameSrv.Network
{
    public class ThreadSocket
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ThreadGateInfo _gateInfo;
        private readonly SocketSendQueue _sendQueue;
        private readonly object runSocketSection;
        private readonly CancellationTokenSource _cancellation;
        private CommandMessage mesaagePacket;
        public string ConnectionId { get;  }

        public ThreadSocket(ThreadGateInfo gateInfo)
        {
            _gateInfo = gateInfo;
            ConnectionId = _gateInfo.SocketId;
            runSocketSection = new object();
            _sendQueue = new SocketSendQueue(gateInfo);
            _cancellation = new CancellationTokenSource();
            mesaagePacket = new CommandMessage();
            Start();
        }

        public ThreadGateInfo GateInfo => _gateInfo;

        private void Start()
        {
            _sendQueue.ProcessSendQueue(_cancellation);
        }

        public void Stop()
        {
            //await _sendQueue.Stop();
            _cancellation.CancelAfter(1000);
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSrv -> GameGate
        /// </summary>
        /// <returns></returns>
        public void ProcessBufferSend(byte[] sendData)
        {
            if (!GateInfo.BoUsed && GateInfo.Socket == null)
            {
                return;
            }
            const string sExceptionMsg = "[Exception] TRunSocket::SendGateBuffers -> SendBuff";
            var dwRunTick = HUtil32.GetTickCount();
            if (GateInfo.nSendChecked > 0)// 如果网关未回复状态消息，则不再发送数据
            {
                if ((HUtil32.GetTickCount() - GateInfo.dwSendCheckTick) > M2Share.SocCheckTimeOut) // 2 * 1000
                {
                    GateInfo.nSendChecked = 0;
                    GateInfo.nSendBlockCount = 0;
                }
                return;
            }
            try
            {
                var sendBuffLen = sendData.Length;
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
                    SendCheck(Grobal2.GM_RECEIVE_OK);
                    GateInfo.nSendChecked = 1;
                    GateInfo.dwSendCheckTick = HUtil32.GetTickCount();
                }
                if (GateInfo.Socket != null && GateInfo.Socket.Connected)
                {
                    _sendQueue.SendMessage(sendData);
                    GateInfo.nSendCount++;
                    GateInfo.nSendBytesCount += sendBuffLen;
                    GateInfo.nSendBlockCount += sendBuffLen;
                    M2Share.NetworkMonitor.Send(sendBuffLen);
                }
                if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.SocLimit)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.StackTrace);
            }
        }

        internal void SendCheck(ushort nIdent)
        {
            var msgHeader = new ServerMessage
            {
                PacketCode = Grobal2.PacketCode,
                Socket = 0,
                Ident = nIdent,
                PackLength = 0
            };
            var data = SerializerUtil.Serialize(msgHeader);
            //var sendData = M2Share.BytePool.Rent(data.Length);
            //data.CopyTo(sendData.Memory);
            _sendQueue.SendMessage(data);
        }
        
        static ArraySegment<byte> GetArraySegment(ref ReadOnlySequence<byte> input)
        {
            if (input.IsSingleSegment && MemoryMarshal.TryGetArray(input.First, out var segment))
            {
                return segment;
            }

            // Should be rare
            var array = input.ToArray();
            return new ArraySegment<byte>(array);
        }

        public void ProcessBuffer(ServerMessage packetHeader, ReadOnlySpan<byte> message)
        {
            const string sExceptionMsg = "[Exception] GameGate::ProcessReceiveBuffer";
            try
            {
                if (packetHeader.PacketCode == Grobal2.PacketCode)
                {
                    if (packetHeader.PackLength > 0)
                    {
                        ExecGateBuffers(packetHeader, message, packetHeader.PackLength);
                    }
                    else
                    {
                        ExecGateBuffers(packetHeader, null, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// 执行网关封包消息
        /// </summary>
        private void ExecGateBuffers(ServerMessage msgPacket, ReadOnlySpan<byte> msgBuff, int nMsgLen)
        {
            const string sExceptionMsg = "[Exception] ThreadSocket::ExecGateMsg";
            try
            {
                int nUserIdx;
                switch (msgPacket.Ident)
                {
                    case Grobal2.GM_OPEN:
                        var sIPaddr = HUtil32.GetString(msgBuff, nMsgLen);
                        nUserIdx = OpenNewUser(msgPacket.Socket, msgPacket.SessionId, sIPaddr, GateInfo.UserList);
                        SendNewUserMsg(GateInfo.Socket, msgPacket.Socket, msgPacket.SessionId, nUserIdx + 1);
                        GateInfo.nUserCount++;
                        break;
                    case Grobal2.GM_CLOSE:
                        CloseUser(msgPacket.Socket);
                        break;
                    case Grobal2.GM_CHECKCLIENT:
                        GateInfo.boSendKeepAlive = true;
                        break;
                    case Grobal2.GM_RECEIVE_OK:
                        GateInfo.nSendChecked = 0;
                        GateInfo.nSendBlockCount = 0;
                        break;
                    case Grobal2.GM_DATA:
                        SessionUser gateUser = null;
                        if (msgPacket.SessionIndex >= 1)
                        {
                            nUserIdx = msgPacket.SessionIndex - 1;
                            if (GateInfo.UserList.Count > nUserIdx)
                            {
                                gateUser = GateInfo.UserList[nUserIdx];
                                if (gateUser != null && gateUser.nSocket != msgPacket.Socket)
                                {
                                    gateUser = null;
                                }
                            }
                        }
                        if (gateUser == null)
                        {
                            for (var i = 0; i < GateInfo.UserList.Count; i++)
                            {
                                if (GateInfo.UserList[i] == null)
                                {
                                    continue;
                                }
                                if (GateInfo.UserList[i].nSocket == msgPacket.Socket)
                                {
                                    gateUser = GateInfo.UserList[i];
                                    break;
                                }
                            }
                        }
                        if (gateUser != null)
                        {
                            if (gateUser.PlayObject != null && gateUser.WorldEngine != null)
                            {
                                if (gateUser.Certification && nMsgLen >= 12)
                                {
                                    mesaagePacket.Recog = BitConverter.ToInt32(msgBuff[..4]);
                                    mesaagePacket.Ident = BitConverter.ToUInt16(msgBuff.Slice(4, 2));
                                    mesaagePacket.Param = BitConverter.ToUInt16(msgBuff.Slice(6, 2));
                                    mesaagePacket.Tag = BitConverter.ToUInt16(msgBuff.Slice(8, 2));
                                    mesaagePacket.Series = BitConverter.ToUInt16(msgBuff.Slice(10, 2));
                                    if (nMsgLen == 12)
                                    {
                                        WorldServer.ProcessUserMessage(gateUser.PlayObject, mesaagePacket, null);
                                    }
                                    else
                                    {
                                        var sMsg = EDCode.DeCodeString(HUtil32.GetString(msgBuff, 12, msgBuff.Length - 13));
                                        WorldServer.ProcessUserMessage(gateUser.PlayObject, mesaagePacket, sMsg);
                                    }
                                }
                            }
                            else
                            {
                                var sMsg = HUtil32.SpanToStr(msgBuff);
                                DoClientCertification(gateUser, msgPacket.Socket, sMsg);
                            }
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex);
            }
        }

        private bool GetCertification(string sMsg, ref string sAccount, ref string sChrName, ref int nSessionId, ref int nVersion, ref bool boFlag, ref byte[] tHwid,ref int gateId)
        {
            var result = false;
            var sCodeStr = string.Empty;
            var sClientVersion = string.Empty;
            var sHwid = string.Empty;
            var sIdx = string.Empty;
            var sGateId = string.Empty;
            const string sExceptionMsg = "[Exception] ThreadSocket::DoClientCertification -> GetCertification";
            try
            {
                var sData = EDCode.DeCodeString(sMsg);
                if (sData.Length > 2 && sData[0] == '*' && sData[1] == '*')
                {
                    sData = sData.AsSpan()[2..sData.Length].ToString();
                    sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sChrName, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sCodeStr, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sClientVersion, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sIdx, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sHwid, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sGateId, HUtil32.Backslash);
                    nSessionId = HUtil32.StrToInt(sCodeStr, 0);
                    if (sIdx == "0")
                    {
                        boFlag = true;
                    }
                    else
                    {
                        boFlag = false;
                    }
                    if (!string.IsNullOrEmpty(sAccount) && !string.IsNullOrEmpty(sChrName) && nSessionId >= 2 && !string.IsNullOrEmpty(sHwid))
                    {
                        nVersion = HUtil32.StrToInt(sClientVersion, 0);
                        tHwid = MD5.MD5UnPrInt(sHwid);
                        result = true;
                    }
                    gateId = HUtil32.StrToInt(sGateId, -1);
                    if (gateId == -1)
                    {
                        result = false;
                    }
                    _logger.Debug($"Account:[{sAccount}] ChrName:[{sChrName}] Code:[{sCodeStr}] ClientVersion:[{sClientVersion}] HWID:[{sHwid}]");
                }
            }
            catch(Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex);
            }
            return result;
        }

        private void DoClientCertification(SessionUser gateUser, int nSocket, string sMsg)
        {
            var sAccount = string.Empty;
            var sChrName = string.Empty;
            var nSessionId = 0;
            var boFlag = false;
            var nClientVersion = 0;
            var nPayMent = 0;
            var nPayMode = 0;
            var nPlayTime = 0L;
            var hwid = MD5.EmptyDigest;
            var gateIdx = 0;
            PlayerSession sessInfo;
            const string sExceptionMsg = "[Exception] ThreadSocket::DoClientCertification";
            const string sDisable = "*disable*";
            try
            {
                if (string.IsNullOrEmpty(gateUser.Account))
                {
                    if (HUtil32.TagCount(sMsg, '!') > 0)
                    {
                        HUtil32.ArrestStringEx(sMsg, "#", "!", ref sMsg);
                        var packetMsg = sMsg.AsSpan()[1..].ToString();
                        if (GetCertification(packetMsg, ref sAccount, ref sChrName, ref nSessionId, ref nClientVersion, ref boFlag, ref hwid,ref gateIdx))
                        {
                            sessInfo = IdSrvClient.Instance.GetAdmission(sAccount, gateUser.sIPaddr, nSessionId, ref nPayMode, ref nPayMent, ref nPlayTime);
                            if (sessInfo != null && nPayMent > 0)
                            {
                                gateUser.Certification = true;
                                gateUser.Account = sAccount.Trim();
                                gateUser.sChrName = sChrName.Trim();
                                gateUser.SessionID = nSessionId;
                                gateUser.ClientVersion = nClientVersion;
                                gateUser.SessInfo = sessInfo;
                                var loadRcdInfo = new LoadDBInfo
                                {
                                    Account = sAccount,
                                    ChrName = sChrName,
                                    sIPaddr = gateUser.sIPaddr,
                                    SessionID = nSessionId,
                                    SoftVersionDate = nClientVersion,
                                    PayMent = nPayMent,
                                    PayMode = nPayMode,
                                    SocketId = nSocket,
                                    GSocketIdx = gateUser.SocketId,
                                    GateIdx = gateIdx,
                                    NewUserTick = HUtil32.GetTickCount(),
                                    PlayTime = nPlayTime
                                };
                                M2Share.FrontEngine.AddToLoadRcdList(loadRcdInfo);
                            }
                            else
                            {
                                gateUser.Account = sDisable;
                                gateUser.Certification = false;
                                CloseUser(nSocket);
                                _logger.Warn($"会话验证失败.Account:{sAccount} SessionId:{nSessionId} Address:{gateUser.sIPaddr}");
                            }
                        }
                        else
                        {
                            gateUser.Account = sDisable;
                            gateUser.Certification = false;
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
            if (GateInfo == null || GateInfo.UserList == null)
            {
                return;
            }
            if (GateInfo.UserList.Count > 0)
            {
                HUtil32.EnterCriticalSections(runSocketSection);
                try
                {
                    for (var i = 0; i < GateInfo.UserList.Count; i++)
                    {
                        if (GateInfo.UserList[i] != null)
                        {
                            var gateUser = GateInfo.UserList[i];
                            if (gateUser == null)
                            {
                                continue;
                            }
                            if (gateUser.nSocket == nSocket)
                            {
                                if (gateUser.FrontEngine != null)
                                {
                                    gateUser.FrontEngine.DeleteHuman(i, gateUser.nSocket);
                                }
                                if (gateUser.PlayObject != null)
                                {
                                    if (!gateUser.PlayObject.OffLineFlag)
                                    {
                                        gateUser.PlayObject.BoSoftClose = true;
                                    }
                                    if (gateUser.PlayObject.Ghost && !gateUser.PlayObject.BoReconnection)
                                    {
                                        IdSrvClient.Instance.SendHumanLogOutMsg(gateUser.Account, gateUser.SessionID);
                                    }
                                    if (gateUser.PlayObject.BoSoftClose && gateUser.PlayObject.BoReconnection && gateUser.PlayObject.BoEmergencyClose)
                                    {
                                        IdSrvClient.Instance.SendHumanLogOutMsg(gateUser.Account, gateUser.SessionID);
                                    }
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
                    HUtil32.LeaveCriticalSections(runSocketSection);
                }
            }
        }

        private static int OpenNewUser(int socket, ushort socketId, string sIPaddr, IList<SessionUser> userList)
        {
            var gateUser = new SessionUser
            {
                Account = string.Empty,
                sChrName = string.Empty,
                sIPaddr = sIPaddr,
                nSocket = socket,
                SocketId = socketId,
                SessionID = 0,
                WorldEngine = null,
                FrontEngine = null,
                PlayObject = null,
                dwNewUserTick = HUtil32.GetTickCount(),
                Certification = false
            };
            for (var i = 0; i < userList.Count; i++)
            {
                if (userList[i] == null)
                {
                    userList[i] = gateUser;
                    return i;
                }
            }
            userList.Add(gateUser);
            return userList.Count - 1;
        }

        private static void SendNewUserMsg(Socket socket, int nSocket, ushort socketId, int nUserIdex)
        {
            if (!socket.Connected)
            {
                return;
            }
            var msgHeader = new ServerMessage();
            msgHeader.PacketCode = Grobal2.PacketCode;
            msgHeader.Socket = nSocket;
            msgHeader.SessionId = socketId;
            msgHeader.Ident = Grobal2.GM_SERVERUSERINDEX;
            msgHeader.SessionIndex = (ushort)nUserIdex;
            msgHeader.PackLength = 0;
            var data = SerializerUtil.Serialize(msgHeader);
            socket.Send(data, 0, data.Length, SocketFlags.None);
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nSocket, PlayObject playObject)
        {
            HUtil32.EnterCriticalSection(runSocketSection);
            try
            {
                for (var i = 0; i < GateInfo.UserList.Count; i++)
                {
                    var gateUserInfo = GateInfo.UserList[i];
                    if (gateUserInfo != null && gateUserInfo.nSocket == nSocket)
                    {
                        gateUserInfo.FrontEngine = null;
                        gateUserInfo.WorldEngine = M2Share.WorldEngine;
                        gateUserInfo.PlayObject = playObject;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(runSocketSection);
            }
        }

        private class SocketSendQueue
        {
            private Channel<byte[]> SendQueue { get; }
            private string ConnectionId { get; }

            public SocketSendQueue(ThreadGateInfo gateInfo)
            {
                SendQueue = Channel.CreateUnbounded<byte[]>();
                ConnectionId = gateInfo.SocketId;
            }

            /// <summary>
            /// 获取队列消息数量
            /// </summary>
            public int GetQueueCount => SendQueue.Reader.Count;

            /// <summary>
            /// 添加到发送队列
            /// </summary>
            public void SendMessage(byte[] buffer)
            {
                SendQueue.Writer.TryWrite(buffer);
            }

            public async Task Stop()
            {
                if (SendQueue.Reader.Count > 0)
                {
                    await SendQueue.Reader.Completion;
                }
            }

            /// <summary>
            /// 处理队列数据并发送到GameGate
            /// GameSrv -> GameGate
            /// </summary>
            public void ProcessSendQueue(CancellationTokenSource cancellation)
            {
                Task.Factory.StartNew(async () =>
                {
                    while (await SendQueue.Reader.WaitToReadAsync(cancellation.Token))
                    {
                        while (SendQueue.Reader.TryRead(out var buffer))
                        {
                            try
                            {
                                M2Share.SocketMgr.Send(ConnectionId, buffer);
                            }
                            catch (Exception ex)
                            {
                                M2Share.Logger.Error(ex.StackTrace);
                            }
                            //GameGate.Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                        }
                    }
                }, cancellation.Token);
            }
        }
    }
}