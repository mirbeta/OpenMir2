using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packages;

namespace GameSvr
{
    public class GateService
    {
        private readonly int _gateIdx;
        private readonly TGateInfo _gateInfo;
        private readonly SendQueue _sendQueue;
        private readonly object runSocketSection;
        private byte[] GameBuffer;
        private int nBuffLen;

        public GateService(int gateIdx, TGateInfo gateInfo)
        {
            _gateIdx = gateIdx;
            _gateInfo = gateInfo;
            runSocketSection = new object();
            _sendQueue = new SendQueue();
            _sendQueue.Initialization(gateIdx, gateInfo.Socket);
        }

        public TGateInfo GateInfo => _gateInfo;

        public void StartQueueService()
        {
            Task.Run(async () =>
            {
                await _sendQueue.ProcessSendQueue();
            });
        }

        public void Stop()
        {
            _sendQueue.Stop();
        }

        /// <summary>
        /// 处理接收到的数据
        /// GameGate -> GameSvr
        /// </summary>
        /// <param name="nMsgLen"></param>
        /// <param name="data"></param>
        public void HandleReceiveBuffer(int nMsgLen,byte[] data)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::ExecGateBuffers -> pBuffer";
            const string sExceptionMsg2 = "[Exception] TRunSocket::ExecGateBuffers -> @pwork,ExecGateMsg ";
            if (nMsgLen <= 0)
            {
                return;
            }
            try
            {
                if (nBuffLen > 0)
                {
                    var buffSize = nBuffLen + nMsgLen;
                    if (GameBuffer != null && buffSize > nBuffLen)
                    {
                        var tempBuff = new byte[buffSize];
                        Buffer.BlockCopy(GameBuffer, 0, tempBuff, 0, nBuffLen);
                        Buffer.BlockCopy(data, 0, tempBuff, nBuffLen, nMsgLen);
                        GameBuffer = tempBuff;
                    }
                }
                else
                {
                    GameBuffer = data;
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg1);
            }
            var nLen = 0;
            var buffIndex = 0;
            byte[] protoBuff = GameBuffer;
            try
            {
                nLen = nBuffLen + nMsgLen;
                while (nLen >= MessageHeader.PacketSize)
                {
                    var msgHeader = new MessageHeader(protoBuff);
                    if (msgHeader.dwCode == 0)
                    {
                        return;
                    }
                    var nCheckMsgLen = Math.Abs(msgHeader.nLength) + MessageHeader.PacketSize;
                    if (msgHeader.dwCode == Grobal2.RUNGATECODE && nCheckMsgLen < 0x8000)
                    {
                        if (nLen < nCheckMsgLen)
                        {
                            break;
                        }
                        byte[] body = null;
                        if (msgHeader.nLength > 0 && protoBuff != null)
                        {
                            body = protoBuff[..nCheckMsgLen]; //获取整个封包内容,包括消息头和消息体
                            body = body[MessageHeader.PacketSize..];
                        }
                        //M2Share.GateManager.AddGameGateQueue(_gateIdx, msgHeader, body); //添加到处理队列
                        ExecGateBuffers(msgHeader, body);
                        nLen -= nCheckMsgLen;
                        if (nLen > 0 && GameBuffer != null)
                        {
                            var tempBuff = new byte[nLen];
                            Buffer.BlockCopy(GameBuffer, nCheckMsgLen, tempBuff, 0, nLen);
                            GameBuffer = tempBuff;
                            protoBuff = tempBuff;
                            nBuffLen = nLen;
                        }
                    }
                    else
                    {
                        buffIndex++;
                        var messageBuff = new byte[protoBuff.Length - 1];
                        Buffer.BlockCopy(protoBuff, buffIndex, messageBuff, 0, MessageHeader.PacketSize);
                        protoBuff = messageBuff;
                        nLen -= 1;
                        Console.WriteLine("注意看这里，看到这句话就是GameSvr封包处理出了问题.");
                    }
                    if (nLen < MessageHeader.PacketSize)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.ErrorMessage(sExceptionMsg2);
            }
            if (nLen > 0)
            {
                var tempBuff = new byte[nLen];
                Buffer.BlockCopy(protoBuff, 0, tempBuff, 0, nLen);
                GameBuffer = tempBuff;
                nBuffLen = nLen;
            }
            else
            {
                GameBuffer = null;
                nBuffLen = 0;
            }
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr -> GameGate
        /// </summary>
        /// <returns></returns>
        public void HandleSendBuffer(byte[] buffer) 
        {
            if (!GateInfo.boUsed && GateInfo.Socket == null)
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
                        nSendBuffLen = 0;
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
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.StackTrace, MessageType.Error);
            }
        }

        private void SendCheck(Socket Socket, ushort nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var msgHeader = new MessageHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = nIdent,
                nLength = 0
            };
            if (Socket.Connected)
            {
                var data = msgHeader.GetPacket();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// 执行网关封包消息
        /// </summary>
        private void ExecGateBuffers(MessageHeader packet, byte[] data)
        {
            if (packet.nLength == 0)
            {
                ExecGateMsg(_gateIdx, GateInfo, packet, null, packet.nLength);
            }
            else
            {
                ExecGateMsg(_gateIdx, GateInfo, packet, data, packet.nLength);
            }
        }

        private bool DoClientCertification_GetCertification(string sMsg, ref string sAccount, ref string sChrName, ref int nSessionID, ref int nClientVersion, ref bool boFlag, ref byte[] tHWID)
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
                M2Share.ErrorMessage(sExceptionMsg, MessageType.Error);
            }
            return result;
        }

        private void DoClientCertification(int GateIdx, TGateUserInfo GateUser, int nSocket, string sMsg)
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
                        if (DoClientCertification_GetCertification(sMsg, ref sAccount, ref sChrName, ref nSessionID, ref nClientVersion, ref boFlag, ref HWID))
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
                                    M2Share.FrontEngine.AddToLoadRcdList(sAccount, sChrName, GateUser.sIPaddr, boFlag, nSessionID, nPayMent, nPayMode, nClientVersion, nSocket, GateUser.nGSocketIdx, GateIdx);
                                }
                                catch
                                {
                                    M2Share.ErrorMessage(sExceptionMsg);
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
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public void CloseUser(int nSocket)
        {
            TGateUserInfo GateUser;
            const string sExceptionMsg0 = "[Exception] TRunSocket::CloseUser 0";
            const string sExceptionMsg1 = "[Exception] TRunSocket::CloseUser 1";
            const string sExceptionMsg2 = "[Exception] TRunSocket::CloseUser 2";
            const string sExceptionMsg3 = "[Exception] TRunSocket::CloseUser 3";
            const string sExceptionMsg4 = "[Exception] TRunSocket::CloseUser 4";
            if (GateInfo.UserList != null)
            {
                HUtil32.EnterCriticalSection(runSocketSection);
                try
                {
                    try
                    {
                        for (var i = 0; i < GateInfo.UserList.Count; i++)
                        {
                            if (GateInfo.UserList[i] != null)
                            {
                                GateUser = GateInfo.UserList[i];
                                if (GateUser.nSocket == nSocket)
                                {
                                    try
                                    {
                                        if (GateUser.FrontEngine != null)
                                        {
                                            GateUser.FrontEngine.DeleteHuman(i, GateUser.nSocket);
                                        }
                                    }
                                    catch
                                    {
                                        M2Share.ErrorMessage(sExceptionMsg1);
                                    }
                                    try
                                    {
                                        if (GateUser.PlayObject != null)
                                        {
                                            if (!GateUser.PlayObject.m_boOffLineFlag)
                                            {
                                                GateUser.PlayObject.m_boSoftClose = true;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        M2Share.ErrorMessage(sExceptionMsg2);
                                    }
                                    try
                                    {
                                        if (GateUser.PlayObject != null && GateUser.PlayObject.m_boGhost && !GateUser.PlayObject.m_boReconnection)
                                        {
                                            IdSrvClient.Instance.SendHumanLogOutMsg(GateUser.sAccount, GateUser.nSessionID);
                                        }
                                    }
                                    catch
                                    {
                                        M2Share.ErrorMessage(sExceptionMsg3);
                                    }
                                    try
                                    {
                                        GateUser = null;
                                        GateInfo.UserList[i] = null;
                                        GateInfo.nUserCount -= 1;
                                    }
                                    catch
                                    {
                                        M2Share.ErrorMessage(sExceptionMsg4);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        M2Share.ErrorMessage(sExceptionMsg0);
                    }
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(runSocketSection);
                }
            }
        }

        private int OpenNewUser(int nSocket, ushort nGSocketIdx, string sIPaddr, IList<TGateUserInfo> UserList)
        {
            int result;
            var GateUser = new TGateUserInfo
            {
                sAccount = string.Empty,
                sCharName = String.Empty,
                sIPaddr = sIPaddr,
                nSocket = nSocket,
                nGSocketIdx = nGSocketIdx,
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

        private void SendNewUserMsg(Socket Socket, int nSocket, int nSocketIndex, int nUserIdex)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var MsgHeader = new MessageHeader();
            MsgHeader.dwCode = Grobal2.RUNGATECODE;
            MsgHeader.nSocket = nSocket;
            MsgHeader.wGSocketIdx = (ushort)nSocketIndex;
            MsgHeader.wIdent = Grobal2.GM_SERVERUSERINDEX;
            MsgHeader.wUserListIndex = (ushort)nUserIdex;
            MsgHeader.nLength = 0;
            if (Socket.Connected)
            {
                var data = MsgHeader.GetPacket();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        private void ExecGateMsg(int GateIdx, TGateInfo Gate, MessageHeader MsgHeader, byte[] MsgBuff, int nMsgLen)
        {
            int nUserIdx;
            const string sExceptionMsg = "[Exception] TRunSocket::ExecGateMsg";
            try
            {
                switch (MsgHeader.wIdent)
                {
                    case Grobal2.GM_OPEN:
                        var sIPaddr = HUtil32.GetString(MsgBuff, 0, nMsgLen);
                        nUserIdx = OpenNewUser(MsgHeader.nSocket, MsgHeader.wGSocketIdx, sIPaddr, Gate.UserList);
                        SendNewUserMsg(Gate.Socket, MsgHeader.nSocket, MsgHeader.wGSocketIdx, nUserIdx + 1);
                        Gate.nUserCount++;
                        break;
                    case Grobal2.GM_CLOSE:
                        CloseUser(MsgHeader.nSocket);
                        break;
                    case Grobal2.GM_CHECKCLIENT:
                        Gate.boSendKeepAlive = true;
                        break;
                    case Grobal2.GM_RECEIVE_OK:
                        Gate.nSendChecked = 0;
                        Gate.nSendBlockCount = 0;
                        break;
                    case Grobal2.GM_DATA:
                        TGateUserInfo GateUser = null;
                        if (MsgHeader.wUserListIndex >= 1)
                        {
                            nUserIdx = MsgHeader.wUserListIndex - 1;
                            if (Gate.UserList.Count > nUserIdx)
                            {
                                GateUser = Gate.UserList[nUserIdx];
                                if (GateUser != null && GateUser.nSocket != MsgHeader.nSocket)
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
                                if (Gate.UserList[i].nSocket == MsgHeader.nSocket)
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
                                    var defMsg = new TDefaultMessage(MsgBuff);
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
                                DoClientCertification(GateIdx, GateUser, MsgHeader.nSocket, sMsg);
                            }
                        }
                        break;
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        private unsafe void SendScanMsg(TDefaultMessage DefMsg, string sMsg, int nGateIdx, int nSocket, int nGsIdx)
        {
            byte[] Buff = null;
            int nSendBytes;
            MessageHeader MsgHdr = new MessageHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = nSocket,
                wGSocketIdx = (ushort)nGsIdx,
                wIdent = Grobal2.GM_DATA,
                nLength = 12
            };
            if (DefMsg != null)
            {
                if (!string.IsNullOrEmpty(sMsg))
                {
                    MsgHdr.nLength = sMsg.Length + 12 + 1;
                    nSendBytes = MsgHdr.nLength + MessageHeader.PacketSize;
                    Buff = new byte[nSendBytes + sizeof(int)];
                }
                else
                {
                    MsgHdr.nLength = 12;
                    nSendBytes = MsgHdr.nLength + MessageHeader.PacketSize;
                    Buff = new byte[nSendBytes + sizeof(int)];
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(sMsg))
                {
                    MsgHdr.nLength = -(sMsg.Length + 1);
                    nSendBytes = Math.Abs(MsgHdr.nLength) + MessageHeader.PacketSize;
                    Buff = new byte[nSendBytes + sizeof(int)];
                    fixed (byte* pb = Buff)
                    {
                        *(int*)pb = nSendBytes;
                        //*(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                        *(char*)(pb + MessageHeader.PacketSize + sizeof(int) + sMsg.Length + 1) = sMsg[1];
                    }
                }
            }
            if (!M2Share.GateManager.AddGateBuffer(nGateIdx, Buff))
            {
                Buff = null;
            }
        }

        public void KickUser(int gateIdx, string sAccount, int nSessionID, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            try
            {
                if (GateInfo.boUsed && GateInfo.Socket != null && GateInfo.UserList != null)
                {
                    HUtil32.EnterCriticalSection(runSocketSection);
                    try
                    {
                        for (var j = 0; j < GateInfo.UserList.Count; j++)
                        {
                            var GateUserInfo = GateInfo.UserList[j];
                            if (GateUserInfo == null)
                            {
                                continue;
                            }
                            if (GateUserInfo.sAccount == sAccount || GateUserInfo.nSessionID == nSessionID)
                            {
                                if (GateUserInfo.FrontEngine != null)
                                {
                                    GateUserInfo.FrontEngine.DeleteHuman(gateIdx, GateUserInfo.nSocket);
                                }
                                if (GateUserInfo.PlayObject != null)
                                {
                                    if (payMode == 0)
                                    {
                                        GateUserInfo.PlayObject.SysMsg(sKickUserMsg, MsgColor.Red, MsgType.Hint);
                                    }
                                    else
                                    {
                                        GateUserInfo.PlayObject.SysMsg("账号付费时间已到,本机已被强行离线,请充值后再继续进行游戏!", MsgColor.Red, MsgType.Hint);
                                    }
                                    GateUserInfo.PlayObject.m_boEmergencyClose = true;
                                    GateUserInfo.PlayObject.m_boSoftClose = true;
                                }
                                GateUserInfo = null;
                                GateInfo.UserList[j] = null;
                                GateInfo.nUserCount -= 1;
                                break;
                            }
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(runSocketSection);
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message, MessageType.Error);
            }
        }
        
        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nSocket, TPlayObject PlayObject)
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
                        gateUserInfo.UserEngine = M2Share.UserEngine;
                        gateUserInfo.PlayObject = PlayObject;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(runSocketSection);
            }
        }
    }
}