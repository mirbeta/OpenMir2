using GameSrv.Player;
using GameSrv.Services;
using GameSrv.World;
using NLog;
using System.Net.Sockets;
using System.Threading.Channels;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameGate {
    public class GameGate {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly int GateIdx;
        private readonly GameGateInfo _gateInfo;
        private readonly GateSendQueue _sendQueue;
        private readonly object _runSocketSection;
        private readonly CancellationTokenSource _cancellation;
        private CommandMessage _clientMesaagePacket;
        /// <summary>
        /// 数据接收缓冲区
        /// </summary>
        public byte[] ReceiveBuffer;
        public int ReceiveLen;

        public GameGate(int gateIdx, GameGateInfo gateInfo) {
            GateIdx = gateIdx;
            _gateInfo = gateInfo;
            _runSocketSection = new object();
            _sendQueue = new GateSendQueue(gateInfo);
            _cancellation = new CancellationTokenSource();
            _clientMesaagePacket = new CommandMessage();
            ReceiveBuffer = new byte[4096];
            StartGateQueue();
        }

        public GameGateInfo GateInfo => _gateInfo;

        private void StartGateQueue() {
            _sendQueue.ProcessSendQueue(_cancellation);
        }

        public void Stop() {
            _cancellation.CancelAfter(3000);
        }

        internal void ProcessBufferReceive(byte[] packetBuff, int packetLen) {
            const string sExceptionMsg = "[Exception] GameGate::ProcessReceiveBuffer";
            int nLen = packetLen;
            int buffIndex = 0;
            int processLen = 0;
            Span<byte> processBuff = packetBuff.AsSpan();
            try {
                while (nLen >= ServerMessage.PacketSize) {
                    ServerMessage packetHeader = SerializerUtil.Deserialize<ServerMessage>(processBuff.Slice(processLen, ServerMessage.PacketSize));
                    if (packetHeader.PacketCode == Grobal2.RunGateCode) {
                        int nCheckMsgLen = Math.Abs(packetHeader.PackLength) + ServerMessage.PacketSize;
                        if (nLen < nCheckMsgLen && nCheckMsgLen < 0x8000) {
                            _logger.Warn("丢弃网关长度数据包.");
                            break;
                        }
                        if (packetHeader.PackLength > 0) {
                            Span<byte> body = processBuff.Slice(ServerMessage.PacketSize, packetHeader.PackLength);
                            ExecGateBuffers(packetHeader, packetHeader.PackLength, body);
                        }
                        else {
                            ExecGateBuffers(packetHeader, packetHeader.PackLength, null);
                        }
                        nLen -= nCheckMsgLen;
                        processLen += nCheckMsgLen;
                        if (nLen <= 0) {
                            break;
                        }
                        ReceiveLen = nLen;
                        buffIndex = 0;
                    }
                    else {
                        buffIndex++;
                        //if (buffIndex > memoryStream.Length)//异常数据，整段数据丢弃
                        //{
                        //    memoryStream.Position = 0;
                        //    _gateInfo.BuffLen = 0;
                        //    return;
                        //}
                        //memoryStream.Position = buffIndex;
                        nLen -= 1;
                        _logger.Warn("丢弃整段网关异常数据包");
                    }
                    if (nLen < ServerMessage.PacketSize) {
                        break;
                    }
                }
            }
            catch (Exception ex) {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
            if (nLen > 0) {
                MemoryCopy.BlockCopy(processBuff, 0, ReceiveBuffer, 0, nLen);
                ReceiveLen = nLen;
            }
            else {
                ReceiveLen = 0;
            }
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSrv -> GameGate
        /// </summary>
        /// <returns></returns>
        public void ProcessBufferSend(byte[] sendData) {
            if (!GateInfo.BoUsed && GateInfo.Socket == null) {
                return;
            }
            const string sExceptionMsg = "[Exception] TRunSocket::SendGateBuffers -> SendBuff";
            int dwRunTick = HUtil32.GetTickCount();
            if (GateInfo.nSendChecked > 0)// 如果网关未回复状态消息，则不再发送数据
            {
                if ((HUtil32.GetTickCount() - GateInfo.dwSendCheckTick) > M2Share.SocCheckTimeOut) // 2 * 1000
                {
                    GateInfo.nSendChecked = 0;
                    GateInfo.nSendBlockCount = 0;
                }
                return;
            }
            try {
                int sendBuffLen = sendData.Length;
                if (sendBuffLen == 0) //不发送空包
                {
                    return;
                }
                if (GateInfo.nSendChecked == 0 && GateInfo.nSendBlockCount + sendBuffLen >= M2Share.Config.CheckBlock * 10) {
                    if (GateInfo.nSendBlockCount == 0 && M2Share.Config.CheckBlock * 10 <= sendBuffLen) {
                        return;
                    }
                    SendCheck(Grobal2.GM_RECEIVE_OK);
                    GateInfo.nSendChecked = 1;
                    GateInfo.dwSendCheckTick = HUtil32.GetTickCount();
                }
                if (GateInfo.Socket != null && GateInfo.Socket.Connected) {
                    _sendQueue.SendMessage(sendData);
                    GateInfo.nSendCount++;
                    GateInfo.nSendBytesCount += sendBuffLen;
                    GateInfo.nSendBlockCount += sendBuffLen;
                    M2Share.NetworkMonitor.Receive(sendBuffLen);
                }
                if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.SocLimit) {
                    return;
                }
            }
            catch (Exception e) {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.StackTrace, MessageType.Error);
            }
        }

        internal void SendCheck(ushort nIdent) {
            ServerMessage msgHeader = new ServerMessage {
                PacketCode = Grobal2.RunGateCode,
                Socket = 0,
                Ident = nIdent,
                PackLength = 0
            };
            byte[] data = SerializerUtil.Serialize(msgHeader);
            //var sendData = M2Share.BytePool.Rent(data.Length);
            //data.CopyTo(sendData.Memory);
            _sendQueue.SendMessage(data);
        }

        /// <summary>
        /// 执行网关封包消息
        /// </summary>
        private void ExecGateBuffers(ServerMessage msgPacket, int nMsgLen, Span<byte> msgBuff) {
            const string sExceptionMsg = "[Exception] TRunSocket::ExecGateMsg";
            try {
                int nUserIdx;
                switch (msgPacket.Ident) {
                    case Grobal2.GM_OPEN:
                        string sIPaddr = HUtil32.GetString(msgBuff, nMsgLen);
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
                        GateUserInfo gateUser = null;
                        if (msgPacket.ServerIndex >= 1) {
                            nUserIdx = msgPacket.ServerIndex - 1;
                            if (GateInfo.UserList.Count > nUserIdx) {
                                gateUser = GateInfo.UserList[nUserIdx];
                                if (gateUser != null && gateUser.nSocket != msgPacket.Socket) {
                                    gateUser = null;
                                }
                            }
                        }
                        if (gateUser == null) {
                            for (int i = 0; i < GateInfo.UserList.Count; i++) {
                                if (GateInfo.UserList[i] == null) {
                                    continue;
                                }
                                if (GateInfo.UserList[i].nSocket == msgPacket.Socket) {
                                    gateUser = GateInfo.UserList[i];
                                    break;
                                }
                            }
                        }
                        if (gateUser != null) {
                            if (gateUser.PlayObject != null && gateUser.UserEngine != null) {
                                if (gateUser.Certification && nMsgLen >= 12) {
                                    _clientMesaagePacket.Recog = BitConverter.ToInt32(msgBuff[..4]);
                                    _clientMesaagePacket.Ident = BitConverter.ToUInt16(msgBuff.Slice(4, 2));
                                    _clientMesaagePacket.Param = BitConverter.ToUInt16(msgBuff.Slice(6, 2));
                                    _clientMesaagePacket.Tag = BitConverter.ToUInt16(msgBuff.Slice(8, 2));
                                    _clientMesaagePacket.Series = BitConverter.ToUInt16(msgBuff.Slice(10, 2));
                                    if (nMsgLen == 12) {
                                        WorldServer.ProcessUserMessage(gateUser.PlayObject, _clientMesaagePacket, null);
                                    }
                                    else {
                                        string sMsg = EDCode.DeCodeString(HUtil32.GetString(msgBuff, 12, msgBuff.Length - 13));
                                        WorldServer.ProcessUserMessage(gateUser.PlayObject, _clientMesaagePacket, sMsg);
                                    }
                                }
                            }
                            else {
                                string sMsg = HUtil32.StrPas(msgBuff);
                                DoClientCertification(GateIdx, gateUser, msgPacket.Socket, sMsg);
                            }
                        }
                        break;
                }
            }
            catch {
                _logger.Error(sExceptionMsg);
            }
        }

        private bool GetCertification(string sMsg, ref string sAccount, ref string sChrName, ref int nSessionId, ref int nClientVersion, ref bool boFlag, ref byte[] tHwid) {
            bool result = false;
            string sCodeStr = string.Empty;
            string sClientVersion = string.Empty;
            string sHwid = string.Empty;
            string sIdx = string.Empty;
            const string sExceptionMsg = "[Exception] TRunSocket::DoClientCertification -> GetCertification";
            try {
                string sData = EDCode.DeCodeString(sMsg);
                if (sData.Length > 2 && sData[0] == '*' && sData[1] == '*') {
                    sData = sData.AsSpan()[2..sData.Length].ToString();
                    sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sChrName, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sCodeStr, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sClientVersion, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sIdx, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sHwid, HUtil32.Backslash);
                    nSessionId = HUtil32.StrToInt(sCodeStr, 0);
                    if (sIdx == "0") {
                        boFlag = true;
                    }
                    else {
                        boFlag = false;
                    }
                    if (!string.IsNullOrEmpty(sAccount) && !string.IsNullOrEmpty(sChrName) && nSessionId >= 2 && !string.IsNullOrEmpty(sHwid)) {
                        nClientVersion = HUtil32.StrToInt(sClientVersion, 0);
                        tHwid = MD5.MD5UnPrInt(sHwid);
                        result = true;
                    }
                    M2Share.Logger.Debug($"Account:[{sAccount}] ChrName:[{sChrName}] Code:[{sCodeStr}] ClientVersion:[{sClientVersion}] HWID:[{sHwid}]");
                }
            }
            catch {
                _logger.Error(sExceptionMsg, MessageType.Error);
            }
            return result;
        }

        private void DoClientCertification(int gateIdx, GateUserInfo gateUser, int nSocket, string sMsg) {
            string sAccount = string.Empty;
            string sChrName = string.Empty;
            int nSessionId = 0;
            bool boFlag = false;
            int nClientVersion = 0;
            int nPayMent = 0;
            int nPayMode = 0;
            long nPlayTime = 0L;
            byte[] hwid = MD5.EmptyDigest;
            PlayerSession sessInfo;
            const string sExceptionMsg = "[Exception] TRunSocket::DoClientCertification";
            const string sDisable = "*disable*";
            try {
                if (string.IsNullOrEmpty(gateUser.Account)) {
                    if (HUtil32.TagCount(sMsg, '!') > 0) {
                        HUtil32.ArrestStringEx(sMsg, "#", "!", ref sMsg);
                        string packetMsg = sMsg.AsSpan()[1..].ToString();
                        if (GetCertification(packetMsg, ref sAccount, ref sChrName, ref nSessionId, ref nClientVersion, ref boFlag, ref hwid)) {
                            sessInfo = IdSrvClient.Instance.GetAdmission(sAccount, gateUser.sIPaddr, nSessionId, ref nPayMode, ref nPayMent, ref nPlayTime);
                            if (sessInfo != null && nPayMent > 0) {
                                gateUser.Certification = true;
                                gateUser.Account = sAccount.Trim();
                                gateUser.sChrName = sChrName.Trim();
                                gateUser.SessionID = nSessionId;
                                gateUser.ClientVersion = nClientVersion;
                                gateUser.SessInfo = sessInfo;
                                LoadDBInfo loadRcdInfo = new LoadDBInfo {
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
                            else {
                                gateUser.Account = sDisable;
                                gateUser.Certification = false;
                                CloseUser(nSocket);
                                _logger.Warn($"会话验证失败.Account:{sAccount} SessionId:{nSessionId} Address:{gateUser.sIPaddr}");
                            }
                        }
                        else {
                            gateUser.Account = sDisable;
                            gateUser.Certification = false;
                            CloseUser(nSocket);
                        }
                    }
                }
            }
            catch {
                _logger.Error(sExceptionMsg);
            }
        }

        public void CloseUser(int nSocket) {
            if (GateInfo == null || GateInfo.UserList == null) {
                return;
            }
            if (GateInfo.UserList.Count > 0) {
                HUtil32.EnterCriticalSections(_runSocketSection);
                try {
                    for (int i = 0; i < GateInfo.UserList.Count; i++) {
                        if (GateInfo.UserList[i] != null) {
                            GateUserInfo gateUser = GateInfo.UserList[i];
                            if (gateUser == null) {
                                continue;
                            }
                            if (gateUser.nSocket == nSocket) {
                                if (gateUser.FrontEngine != null) {
                                    gateUser.FrontEngine.DeleteHuman(i, gateUser.nSocket);
                                }
                                if (gateUser.PlayObject != null) {
                                    if (!gateUser.PlayObject.OffLineFlag) {
                                        gateUser.PlayObject.BoSoftClose = true;
                                    }
                                }
                                if (gateUser.PlayObject != null && gateUser.PlayObject.Ghost && !gateUser.PlayObject.BoReconnection) {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(gateUser.Account, gateUser.SessionID);
                                }
                                if (gateUser.PlayObject != null && gateUser.PlayObject.BoSoftClose && gateUser.PlayObject.BoReconnection && gateUser.PlayObject.BoEmergencyClose) {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(gateUser.Account, gateUser.SessionID);
                                }
                                GateInfo.UserList[i] = null;
                                GateInfo.nUserCount -= 1;
                                break;
                            }
                        }
                    }
                }
                finally {
                    HUtil32.LeaveCriticalSections(_runSocketSection);
                }
            }
        }

        private static int OpenNewUser(int socket, ushort socketId, string sIPaddr, IList<GateUserInfo> userList) {
            GateUserInfo gateUser = new GateUserInfo {
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
            for (int i = 0; i < userList.Count; i++) {
                if (userList[i] == null) {
                    userList[i] = gateUser;
                    return i;
                }
            }
            userList.Add(gateUser);
            return userList.Count - 1;
        }

        private static void SendNewUserMsg(Socket socket, int nSocket, ushort socketId, int nUserIdex) {
            if (!socket.Connected) {
                return;
            }
            ServerMessage msgHeader = new ServerMessage();
            msgHeader.PacketCode = Grobal2.RunGateCode;
            msgHeader.Socket = nSocket;
            msgHeader.SessionId = socketId;
            msgHeader.Ident = Grobal2.GM_SERVERUSERINDEX;
            msgHeader.ServerIndex = (ushort)nUserIdex;
            msgHeader.PackLength = 0;
            if (socket.Connected) {
                byte[] data = SerializerUtil.Serialize(msgHeader);
                socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nSocket, PlayObject playObject) {
            HUtil32.EnterCriticalSection(_runSocketSection);
            try {
                for (int i = 0; i < GateInfo.UserList.Count; i++) {
                    GateUserInfo gateUserInfo = GateInfo.UserList[i];
                    if (gateUserInfo != null && gateUserInfo.nSocket == nSocket) {
                        gateUserInfo.FrontEngine = null;
                        gateUserInfo.UserEngine = M2Share.WorldEngine;
                        gateUserInfo.PlayObject = playObject;
                        break;
                    }
                }
            }
            finally {
                HUtil32.LeaveCriticalSection(_runSocketSection);
            }
        }

        private class GateSendQueue {
            private Channel<byte[]> SendQueue { get; }
            private string ConnectionId { get; }
            private GameGateInfo GameGate { get; }

            public GateSendQueue(GameGateInfo gateInfo) {
                SendQueue = Channel.CreateUnbounded<byte[]>();
                ConnectionId = gateInfo.SocketId;
                GameGate = gateInfo;
            }

            /// <summary>
            /// 获取队列消息数量
            /// </summary>
            public int GetQueueCount => SendQueue.Reader.Count;

            /// <summary>
            /// 添加到发送队列
            /// </summary>
            public void SendMessage(byte[] buffer) {
                SendQueue.Writer.TryWrite(buffer);
            }

            /// <summary>
            /// 处理队列数据并发送到GameGate
            /// GameSrv -> GameGate
            /// </summary>
            public void ProcessSendQueue(CancellationTokenSource cancellation) {
                Task.Factory.StartNew(async () => {
                    while (await SendQueue.Reader.WaitToReadAsync(cancellation.Token)) {
                        while (SendQueue.Reader.TryRead(out byte[] buffer)) {
                            try {
                                M2Share.GateMgr.Send(ConnectionId, buffer);
                            }
                            catch (Exception ex) {
                                Console.WriteLine(ex.StackTrace);
                                continue;
                            }
                            //GameGate.Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                        }
                    }
                }, cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }
    }
}