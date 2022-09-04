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
        private byte[] _gameBuffer;
        private int _buffLen;
        private readonly CancellationTokenSource _cancellation;

        public GameGate(int gateIdx, GameGateInfo gateInfo)
        {
            _gateIdx = gateIdx;
            _gateInfo = gateInfo;
            _runSocketSection = new object();
            _sendQueue = new GateSendQueue(gateInfo);
            _cancellation = new CancellationTokenSource();
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
        public void HandleReceiveBuffer(int nMsgLen, byte[] data)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::ExecGateBuffers -> pBuffer";
            const string sExceptionMsg2 = "[Exception] TRunSocket::ExecGateBuffers -> @pwork,ExecGateMsg ";
            if (nMsgLen <= 0)
            {
                return;
            }
            try
            {
                if (_buffLen > 0)
                {
                    var buffSize = _buffLen + nMsgLen;
                    if (_gameBuffer != null && buffSize > _buffLen)
                    {
                        var tempBuff = new byte[buffSize];
                        Buffer.BlockCopy(_gameBuffer, 0, tempBuff, 0, _buffLen);
                        Buffer.BlockCopy(data, 0, tempBuff, _buffLen, nMsgLen);
                        _gameBuffer = tempBuff;
                    }
                }
                else
                {
                    _gameBuffer = data;
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg1);
            }
            var nLen = 0;
            var buffIndex = 0;
            byte[] protoBuff = _gameBuffer;
            try
            {
                nLen = _buffLen + nMsgLen;
                while (nLen >= PacketHeader.PacketSize)
                {
                    var msgHeader = Packets.ToPacket<PacketHeader>(protoBuff);
                    if (msgHeader.PacketCode == 0)
                    {
                        return;
                    }
                    var nCheckMsgLen = Math.Abs(msgHeader.PackLength) + PacketHeader.PacketSize;
                    if (msgHeader.PacketCode == Grobal2.RUNGATECODE && nCheckMsgLen < 0x8000)
                    {
                        if (nLen < nCheckMsgLen)
                        {
                            break;
                        }
                        byte[] body = null;
                        if (msgHeader.PackLength > 0 && protoBuff != null)
                        {
                            body = protoBuff[..nCheckMsgLen]; //获取整个封包内容,包括消息头和消息体
                            body = body[PacketHeader.PacketSize..];
                        }
                        //M2Share.GateManager.AddGameGateQueue(_gateIdx, msgHeader, body); //添加到处理队列
                        ExecGateBuffers(msgHeader, body);
                        nLen -= nCheckMsgLen;
                        if (nLen > 0 && _gameBuffer != null)
                        {
                            var tempBuff = new byte[nLen];
                            Buffer.BlockCopy(_gameBuffer, nCheckMsgLen, tempBuff, 0, nLen);
                            _gameBuffer = tempBuff;
                            protoBuff = tempBuff;
                            _buffLen = nLen;
                            buffIndex = 0;
                        }
                    }
                    else
                    {
                        buffIndex++;
                        var messageBuff = new byte[protoBuff.Length - 1];
                        Buffer.BlockCopy(protoBuff, buffIndex, messageBuff, 0, PacketHeader.PacketSize);
                        protoBuff = messageBuff;
                        nLen -= 1;
                        _logger.Error("注意看这里，看到这句话就是GameSvr封包处理出了问题.");
                    }
                    if (nLen < PacketHeader.PacketSize)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                _logger.Error(sExceptionMsg2);
            }
            if (nLen > 0)
            {
                var tempBuff = new byte[nLen];
                Buffer.BlockCopy(protoBuff, 0, tempBuff, 0, nLen);
                _gameBuffer = tempBuff;
                _buffLen = nLen;
            }
            else
            {
                _gameBuffer = null;
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
                if (GateInfo.nSendChecked == 0 && GateInfo.nSendBlockCount + nSendBuffLen >= M2Share.g_Config.nCheckBlock * 10)
                {
                    if (GateInfo.nSendBlockCount == 0 && M2Share.g_Config.nCheckBlock * 10 <= nSendBuffLen)
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
                        if (M2Share.g_Config.nSendBlock <= nSendBuffLen)
                        {
                            if (GateInfo.Socket != null)
                            {
                                var sendBuff = new byte[M2Share.g_Config.nSendBlock];
                                Buffer.BlockCopy(sendBuffer, 0, sendBuff, 0, M2Share.g_Config.nSendBlock);
                                _sendQueue.AddToQueue(sendBuff);
                                GateInfo.nSendCount++;
                                GateInfo.nSendBytesCount += M2Share.g_Config.nSendBlock;
                            }
                            GateInfo.nSendBlockCount += M2Share.g_Config.nSendBlock;
                            nSendBuffLen -= M2Share.g_Config.nSendBlock;
                            var tempBuff = new byte[nSendBuffLen];
                            Buffer.BlockCopy(sendBuffer, M2Share.g_Config.nSendBlock, tempBuff, 0, nSendBuffLen);
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
                if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.g_dwSocLimit)
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
        private void ExecGateBuffers(PacketHeader packet, byte[] data)
        {
            if (packet.PackLength == 0)
            {
                ExecGateMsg(_gateIdx, GateInfo, packet, null, packet.PackLength);
            }
            else
            {
                ExecGateMsg(_gateIdx, GateInfo, packet, data, packet.PackLength);
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
                var sData = EDcode.DeCodeString(sMsg);
                if (sData.Length > 2 && sData[0] == '*' && sData[1] == '*')
                {
                    sData = sData.Substring(2, sData.Length - 2);
                    sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sChrName, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sCodeStr, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sClientVersion, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sIdx, HUtil32.Backslash);
                    sData = HUtil32.GetValidStr3(sData, ref sHWID, HUtil32.Backslash);
                    nSessionID = HUtil32.Str_ToInt(sCodeStr, 0);
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
                        nClientVersion = HUtil32.Str_ToInt(sClientVersion, 0);
                        tHWID = MD5.MD5UnPrInt(sHWID);
                        result = true;
                    }
                    Debug.WriteLine($"Account:[{sAccount}] ChrName:[{sChrName}] Code:[{sCodeStr}] ClientVersion:[{sClientVersion}] HWID:[{sHWID}]");
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
            var sData = string.Empty;
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
                        sData = HUtil32.ArrestStringEx(sMsg, "#", "!", ref sMsg);
                        sMsg = sMsg.Substring(1, sMsg.Length - 1);
                        if (GetCertification(sMsg, ref sAccount, ref sChrName, ref nSessionID, ref nClientVersion, ref boFlag, ref HWID))
                        {
                            SessInfo = IdSrvClient.Instance.GetAdmission(sAccount, GateUser.sIPaddr, nSessionID, ref nPayMode, ref nPayMent);
                            if (SessInfo != null && nPayMent > 0)
                            {
                                GateUser.boCertification = true;
                                GateUser.sAccount = sAccount.Trim();
                                GateUser.sCharName = sChrName.Trim();
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
                sCharName = String.Empty,
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

        private void ExecGateMsg(int GateIdx, GameGateInfo Gate, PacketHeader MsgHeader, byte[] MsgBuff, int nMsgLen)
        {
            int nUserIdx;
            const string sExceptionMsg = "[Exception] TRunSocket::ExecGateMsg";
            try
            {
                switch (MsgHeader.Ident)
                {
                    case Grobal2.GM_OPEN:
                        var sIPaddr = HUtil32.GetString(MsgBuff, 0, nMsgLen);
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
                                    var defMsg = Packets.ToPacket<ClientPacket>(MsgBuff);
                                    if (nMsgLen == 12)
                                    {
                                        M2Share.UserEngine.ProcessUserMessage(GateUser.PlayObject, defMsg, null);
                                    }
                                    else
                                    {
                                        var sMsg = EDcode.DeCodeString(HUtil32.GetString(MsgBuff, 12, MsgBuff.Length - 13));
                                        M2Share.UserEngine.ProcessUserMessage(GateUser.PlayObject, defMsg, sMsg);
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
                        gateUserInfo.UserEngine = M2Share.UserEngine;
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
            private readonly Channel<byte[]> _sendQueue = null;
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
                });
            }
        }
    }
}