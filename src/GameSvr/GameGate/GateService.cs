using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace GameSvr
{
    public class GateService
    {
        /// <summary>
        /// 游戏网关
        /// </summary>
        private readonly ISocketServer _gateSocket = null;
        private readonly TGateInfo _gateInfo;
        private readonly string GateAddress;
        private readonly int GatePort;
        private readonly SendQueue sendQueue;
        private readonly object m_RunSocketSection;
        private readonly int GateIdx;

        public GateService(int gateIdx, string gateAddres, int gatePort, TGateInfo gateInfo)
        {
            GateIdx = gateIdx;
            GateAddress = gateAddres;
            GatePort = gatePort;
            _gateInfo = gateInfo;
            sendQueue = new SendQueue(1);
            m_RunSocketSection = new object();
            _gateSocket = new ISocketServer(ushort.MaxValue, 2048);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            _gateSocket.Init();
        }

        public TGateInfo GateInfo => _gateInfo;

        public Task Start()
        {
            _gateSocket.Start(GateAddress, GatePort);
            M2Share.MainOutMessage($"游戏网关[{GateAddress}:{GatePort}]已启动...");
            var gTasks = new Task[sendQueue.QueueConsumerCount];
            for (int i = 0; i < sendQueue.QueueConsumerCount; i++)
            {
                gTasks[i] = sendQueue.ProcessSendQueue();
            }
            return Task.WhenAll(gTasks);
        }

        public void Stop()
        {
            _gateSocket.Shutdown();
        }

        private void GateSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            //M2Share.RunSocket.CloseErrGate();
        }

        private void GateSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            M2Share.GateManager.CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            M2Share.GateManager.OpenGate(e);
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            var nMsgLen = e.BytesReceived;
            if (nMsgLen <= 0)
            {
                return;
            }
            if (nMsgLen > 200)
            {
                Console.WriteLine("asdasd");
            }
            var data = new byte[nMsgLen];
            Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nMsgLen);
            M2Share.GateManager.AddToQueue(new ReceiveData()
            {
                GateId = GateIdx,
                Buffer = data,
                BuffLen = nMsgLen
            });
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr->GameGate
        /// </summary>
        /// <param name="gateIdx"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool AddGateBuffer(byte[] buffer) //添加到发送队列
        {
            var result = false;
            if (GateInfo.boUsed && GateInfo.Socket != null)
            {
                sendQueue.AddToQueue(GateInfo, buffer);
                result = true;
            }
            return result;
        }

        public void ExecGateBuffers(int nGateIndex, byte[] data, int nMsgLen)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::ExecGateBuffers -> pBuffer";
            const string sExceptionMsg2 = "[Exception] TRunSocket::ExecGateBuffers -> @pwork,ExecGateMsg ";
            var nLen = 0;
            var buffIndex = 0;
            if (data is not { Length: > 0 } || nMsgLen <= 0)
            {
                return;
            }
            try
            {
                if (data.Length > 0)
                {
                    var buffSize = GateInfo.nBuffLen + nMsgLen;
                    if (GateInfo.Buffer != null && buffSize > GateInfo.nBuffLen)
                    {
                        var tempBuff = new byte[buffSize];
                        Array.Copy(GateInfo.Buffer, 0, tempBuff, 0, GateInfo.nBuffLen);
                        Array.Copy(data, 0, tempBuff, GateInfo.nBuffLen, nMsgLen);
                        GateInfo.Buffer = tempBuff;
                    }
                    else
                    {
                        GateInfo.Buffer = new byte[buffSize];
                        Array.Copy(data, 0, GateInfo.Buffer, 0, nMsgLen);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg1);
            }
            byte[] Buff = null;
            //todo 下面代码容易引起封包处理异常下，需要重构
            try
            {
                nLen = GateInfo.nBuffLen + nMsgLen;
                Buff = GateInfo.Buffer;
                if (nLen >= MessageHeader.PacketSize)
                {
                    while (true)
                    {
                        var msgHeader = new MessageHeader(Buff);
                        if (msgHeader.dwCode == 0)
                        {
                            Buff = Buff[20..];
                            buffIndex = 0;
                            nLen -= (msgHeader.nLength + MessageHeader.PacketSize);
                            Console.WriteLine("不应该出现这个文字");
                            return;
                        }
                        var nCheckMsgLen = Math.Abs(msgHeader.nLength) + MessageHeader.PacketSize;
                        if (msgHeader.dwCode == Grobal2.RUNGATECODE && nCheckMsgLen < 0x8000)
                        {
                            if (nLen < nCheckMsgLen)
                            {
                                break;
                            }
                            if (msgHeader.nLength == 0)
                            {
                                ExecGateMsg(nGateIndex, GateInfo, msgHeader, null, msgHeader.nLength);
                            }
                            else
                            {
                                byte[] msgBuff = Buff[MessageHeader.PacketSize..];//跳过消息头20字节
                                ExecGateMsg(nGateIndex, GateInfo, msgHeader, msgBuff, msgHeader.nLength);
                            }
                            nLen -= (msgHeader.nLength + MessageHeader.PacketSize);
                            if (nLen > 20)
                            {
                                var tempBuff = new byte[Buff.Length - nCheckMsgLen];
                                Array.Copy(Buff, nCheckMsgLen, tempBuff, 0, tempBuff.Length);
                                Buff = tempBuff;
                                buffIndex = 0;
                            }
                        }
                        else
                        {
                            buffIndex++;
                            var messageBuff = new byte[Buff.Length - 1];
                            Array.Copy(Buff, buffIndex, messageBuff, 0, MessageHeader.PacketSize);
                            Buff = messageBuff;
                            nLen -= 1;
                            Console.WriteLine("注意看这里，看到这句话就是GameSvr封包处理出了问题.");
                        }
                        if (nLen < 20)
                        {
                            break;
                        }
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg2);
            }
            if (nLen > 0)
            {
                var tempBuff = new byte[nLen];
                Array.Copy(Buff, 0, tempBuff, 0, nLen);
                GateInfo.Buffer = tempBuff;
                GateInfo.nBuffLen = nLen;
            }
            else
            {
                GateInfo.Buffer = null;
                GateInfo.nBuffLen = 0;
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
                                CloseUser(GateIdx, nSocket);
                            }
                        }
                        else
                        {
                            GateUser.sAccount = sDisable;
                            GateUser.boCertification = false;
                            CloseUser(GateIdx, nSocket);
                        }
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public void CloseUser(int GateIdx, int nSocket)
        {
            TGateUserInfo GateUser;
            TGateInfo Gate;
            const string sExceptionMsg0 = "[Exception] TRunSocket::CloseUser 0";
            const string sExceptionMsg1 = "[Exception] TRunSocket::CloseUser 1";
            const string sExceptionMsg2 = "[Exception] TRunSocket::CloseUser 2";
            const string sExceptionMsg3 = "[Exception] TRunSocket::CloseUser 3";
            const string sExceptionMsg4 = "[Exception] TRunSocket::CloseUser 4";
            if (GateInfo.UserList != null)
            {
                HUtil32.EnterCriticalSection(m_RunSocketSection);
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
                    HUtil32.LeaveCriticalSection(m_RunSocketSection);
                }
            }
        }

        private int OpenNewUser(int nSocket, ushort nGSocketIdx, string sIPaddr, IList<TGateUserInfo> UserList)
        {
            int result;
            var GateUser = new TGateUserInfo
            {
                sAccount = "",
                sCharName = "",
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
            string sIPaddr;
            TGateUserInfo GateUser;
            const string sExceptionMsg = "[Exception] TRunSocket::ExecGateMsg";
            try
            {
                switch (MsgHeader.wIdent)
                {
                    case Grobal2.GM_OPEN:
                        sIPaddr = HUtil32.GetString(MsgBuff, 0, nMsgLen);
                        nUserIdx = OpenNewUser(MsgHeader.nSocket, MsgHeader.wGSocketIdx, sIPaddr, Gate.UserList);
                        SendNewUserMsg(Gate.Socket, MsgHeader.nSocket, MsgHeader.wGSocketIdx, nUserIdx + 1);
                        Gate.nUserCount++;
                        break;
                    case Grobal2.GM_CLOSE:
                        CloseUser(GateIdx, MsgHeader.nSocket);
                        break;
                    case Grobal2.GM_CHECKCLIENT:
                        Gate.boSendKeepAlive = true;
                        break;
                    case Grobal2.GM_RECEIVE_OK:
                        Gate.nSendChecked = 0;
                        Gate.nSendBlockCount = 0;
                        break;
                    case Grobal2.GM_DATA:
                        GateUser = null;
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
                    HUtil32.EnterCriticalSection(m_RunSocketSection);
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
                        HUtil32.LeaveCriticalSection(m_RunSocketSection);
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message, MessageType.Error);
            }
        }
    }
}