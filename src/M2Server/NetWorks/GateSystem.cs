using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packages;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace M2Server
{
    public class GateSystem
    {
        /// <summary>
        /// 游戏网关
        /// </summary>
        private readonly ISocketServer _gateSocket = null;
        
        public object m_RunSocketSection = null;
        public StringList m_RunAddrList = null;
        public int n8 = 0;
        public TIPaddr[] m_IPaddrArr;
        public int n4F8 = 0;
        public int dwSendTestMsgTick = 0;

        private void AddGate(AsyncUserToken e)
        {
            const string sGateOpen = "游戏网关[{0}]({1}:{2})已打开...";
            const string sKickGate = "服务器未就绪: {0}";
            if (e.EndPoint == null)
            {
                return;
            }
            if (M2Share.boStartReady)
            {
                for (var i = RunSock.g_GateArr.GetLowerBound(0); i <= RunSock.g_GateArr.GetUpperBound(0); i++)
                {
                    var Gate = RunSock.g_GateArr[i];
                    if (Gate.boUsed)
                    {
                        continue;
                    }
                    Gate.GateIndex = i;
                    Gate.boUsed = true;
                    Gate.SocketId = e.ConnectionId;
                    Gate.Socket = e.Socket;
                    Gate.sAddr = GetGateAddr(e.EndPoint.Address.ToString());
                    Gate.nPort = e.EndPoint.Port;
                    Gate.UserList = new List<TGateUserInfo>();
                    Gate.nUserCount = 0;
                    Gate.Buffer = null;
                    Gate.nBuffLen = 0;
                    Gate.boSendKeepAlive = false;
                    Gate.nSendChecked = 0;
                    Gate.nSendBlockCount = 0;
                    RunSock.GataSocket.TryAdd(e.ConnectionId, Gate);
                    M2Share.MainOutMessage(string.Format(sGateOpen, i, e.EndPoint.Address, Gate.nPort));
                    break;
                }
            }
            else
            {
                M2Share.ErrorMessage(string.Format(sKickGate, new object?[] { e.EndPoint.Address.ToString() }), MessageType.Error);
                e.Socket.Close();
            }
        }

        public void CloseAllGate()
        {
            TGateInfo Gate;
            for (var GateIdx = RunSock.g_GateArr.GetLowerBound(0); GateIdx <= RunSock.g_GateArr.GetUpperBound(0); GateIdx++)
            {
                Gate = RunSock.g_GateArr[GateIdx];
                if (Gate.Socket != null)
                {
                    Gate.Socket.Close();
                }
            }
        }

        public void CloseErrGate(Socket Socket)
        {
            if (Socket.Connected)
            {
                Socket.Close();
            }
        }

        public void CloseGate(AsyncUserToken e)
        {
            TGateUserInfo GateUser;
            IList<TGateUserInfo> UserList;
            TGateInfo Gate;
            const string sGateClose = "游戏网关[{0}]({1}:{2})已关闭...";
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                for (var GateIdx = RunSock.g_GateArr.GetLowerBound(0); GateIdx <= RunSock.g_GateArr.GetUpperBound(0); GateIdx++)
                {
                    Gate = RunSock.g_GateArr[GateIdx];
                    if (Gate.Socket == null)
                    {
                        continue;
                    }
                    if (Gate.SocketId.Equals(e.ConnectionId))
                    {
                        UserList = Gate.UserList;
                        for (var i = 0; i < UserList.Count; i++)
                        {
                            GateUser = UserList[i];
                            if (GateUser != null)
                            {
                                if (GateUser.PlayObject != null)
                                {
                                    GateUser.PlayObject.m_boEmergencyClose = true;
                                    if (!GateUser.PlayObject.m_boReconnection)
                                    {
                                        IdSrvClient.Instance.SendHumanLogOutMsg(GateUser.sAccount, GateUser.nSessionID);
                                    }
                                }
                                GateUser = null;
                                UserList[i] = null;
                            }
                        }
                        Gate.UserList = null;
                        if (Gate.Buffer != null)
                        {
                            Gate.Buffer = null;
                        }
                        Gate.Buffer = null;
                        Gate.nBuffLen = 0;
                        Gate.boUsed = false;
                        Gate.Socket = null;
                        M2Share.ErrorMessage(string.Format(sGateClose, GateIdx, e.EndPoint.Address, e.EndPoint.Port));
                        break;
                    }
                }
                RunSock.GataSocket.TryRemove(e.ConnectionId, out Gate);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
        }
   
        private void ExecGateBuffers(int nGateIndex, TGateInfo GameGate, byte[] data, int nMsgLen)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::ExecGateBuffers -> pBuffer";
            const string sExceptionMsg2 = "[Exception] TRunSocket::ExecGateBuffers -> @pwork,ExecGateMsg ";
            const string sExceptionMsg3 = "[Exception] TRunSocket::ExecGateBuffers -> FreeMem";
            var nLen = 0;
            var buffIndex = 0;
            if (data is not {Length: > 0} || nMsgLen <= 0)
            {
                return;
            }
            try
            {
                if (data.Length > 0)
                {
                    int buffSize = GameGate.nBuffLen + nMsgLen;
                    if (GameGate.Buffer != null && buffSize > GameGate.nBuffLen)
                    {
                        var tempBuff = new byte[buffSize];
                        Buffer.BlockCopy(GameGate.Buffer, 0, tempBuff, 0, GameGate.nBuffLen);
                        Buffer.BlockCopy(data, 0, tempBuff, GameGate.nBuffLen, nMsgLen);
                        GameGate.Buffer = tempBuff;
                    }
                    else
                    {
                        GameGate.Buffer = new byte[buffSize];
                        Buffer.BlockCopy(data, 0, GameGate.Buffer, 0, nMsgLen);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg1);
            }
            byte[] Buff = null;
            try
            {
                nLen = GameGate.nBuffLen + nMsgLen;
                Buff = GameGate.Buffer;
                if (nLen >= 20)//sizeof(TMsgHeader)
                {
                    while (true)
                    {
                        var msgHeader = new TMsgHeader(Buff);
                        var nCheckMsgLen = Math.Abs(msgHeader.nLength) + 20;
                        if (msgHeader.dwCode == Grobal2.RUNGATECODE && nCheckMsgLen < 0x8000)
                        {
                            if (nLen < nCheckMsgLen)
                            {
                                break;
                            }
                            if (msgHeader.nLength == 0)
                            {
                                ExecGateMsg(nGateIndex, GameGate, msgHeader, null, msgHeader.nLength);
                            }
                            else
                            {
                                byte[] msgBuff = new byte[msgHeader.nLength];
                                Buffer.BlockCopy(Buff, 20, msgBuff, 0, msgBuff.Length);//跳过消息头20字节
                                ExecGateMsg(nGateIndex, GameGate, msgHeader, msgBuff, msgHeader.nLength);
                            }
                            var newLen = 20 + msgHeader.nLength;
                            var tempBuff = new byte[Buff.Length - newLen];
                            Buffer.BlockCopy(Buff, newLen, tempBuff, 0, tempBuff.Length);
                            Buff = tempBuff;
                            buffIndex = 0;
                            nLen -= (msgHeader.nLength + 20);
                        }
                        else
                        {
                            buffIndex++;
                            var messageBuff = new byte[Buff.Length - 1];
                            Array.Copy(Buff, buffIndex, messageBuff, 0, 20);
                            Buff = messageBuff;
                            nLen -= 1;
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
            try
            {
                if (nLen > 0)
                {
                    var tempBuff = new byte[nLen];
                    Buffer.BlockCopy(Buff, 0, tempBuff, 0, nLen);
                    GameGate.Buffer = tempBuff;
                    GameGate.nBuffLen = nLen;
                }
                else
                {
                    GameGate.Buffer = null;
                    GameGate.nBuffLen = 0;
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg3);
            }
        }

        private void SocketRead(string connectionId,byte[] buffer)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::SocketRead";
            if (RunSock.GataSocket.TryGetValue(connectionId, out var gate))
            {
                try
                {
                    ExecGateBuffers(gate.GateIndex, gate, buffer, buffer.Length);
                }
                catch
                {
                    M2Share.MainOutMessage(sExceptionMsg1);
                }
            }
        }
        
        private bool DoClientCertification_GetCertification(string sMsg, ref string sAccount, ref string sChrName, ref int nSessionID, ref int nClientVersion, ref bool boFlag)
        {
            var result = false;
            var sCodeStr = string.Empty;
            var sClientVersion = string.Empty;
            const string sExceptionMsg = "[Exception] TRunSocket::DoClientCertification -> GetCertification";
            try
            {
                var sData = EDcode.DeCodeString(sMsg, true);
                if (sData.Length > 2 && sData[0] == '*' && sData[1] == '*')
                {
                    sData = sData.Substring(2, sData.Length - 2);
                    sData = HUtil32.GetValidStr3(sData, ref sAccount, "/");
                    sData = HUtil32.GetValidStr3(sData, ref sChrName, "/");
                    sData = HUtil32.GetValidStr3(sData, ref sCodeStr, "/");
                    sData = HUtil32.GetValidStr3(sData, ref sClientVersion, "/");
                    var sIdx = sData;
                    nSessionID = HUtil32.Str_ToInt(sCodeStr, 0);
                    if (sIdx == "0")
                    {
                        boFlag = true;
                    }
                    else
                    {
                        boFlag = false;
                    }
                    if (sAccount != "" && sChrName != "" && nSessionID >= 2)
                    {
                        nClientVersion = HUtil32.Str_ToInt(sClientVersion, 0);
                        result = true;
                    }
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
                        if (DoClientCertification_GetCertification(sMsg, ref sAccount, ref sChrName, ref nSessionID, ref nClientVersion, ref boFlag))
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
        
        private bool SendGateBuffers(int GateIdx, TGateInfo Gate, IList<byte[]> MsgList)
        {
            var nSendBuffLen = 0;
            const string sExceptionMsg1 = "[Exception] TRunSocket::SendGateBuffers -> ProcessBuff";
            const string sExceptionMsg2 = "[Exception] TRunSocket::SendGateBuffers -> SendBuff";
            var result = true;
            if (MsgList.Count == 0)
            {
                return result;
            }
            var dwRunTick = HUtil32.GetTickCount();
            // 如果网关未回复状态消息，则不再发送数据
            if (Gate.nSendChecked > 0)
            {
                if (HUtil32.GetTickCount() - Gate.dwSendCheckTick > M2Share.g_dwSocCheckTimeOut) // 2 * 1000
                {
                    Gate.nSendChecked = 0;
                    Gate.nSendBlockCount = 0;
                }
                return result;
            }
            byte[] BufferA;
            byte[] BufferB;
            
            // 将小数据合并为一个指定大小的数据
            try
            {
                var msgIdx = 0;
                BufferA = MsgList[msgIdx];// 
                while (true)
                {
                    if (msgIdx + 1 >= MsgList.Count)
                    {
                        break;
                    }
                    BufferB = MsgList[msgIdx + 1];//取得下一个消息
                    if (BufferA ==null || BufferB == null)
                    {
                        continue;
                    }
                    var nBuffALen = BufferA.Length;
                    var nBuffBLen = BufferB.Length;
                    if (nBuffALen + nBuffBLen < M2Share.g_Config.nSendBlock)
                    {
                        MsgList.RemoveAt(msgIdx + 1);
                        var newBuffer = new byte[nBuffALen + nBuffBLen];
                        Buffer.BlockCopy(BufferA, 0, newBuffer, 0, BufferA.Length);
                        Buffer.BlockCopy(BufferB, 0, newBuffer, BufferA.Length, BufferB.Length);
                        BufferA = newBuffer;
                        MsgList[msgIdx] = BufferA;
                        continue;
                    }
                    msgIdx++;
                    BufferA = BufferB;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg1);
                M2Share.ErrorMessage(e.Message, MessageType.Error);
            }
            try
            {
                //todo 需要优化发送逻辑
                while (MsgList.Count > 0) 
                {
                    BufferA = MsgList[0];
                    if (BufferA == null)
                    {
                        MsgList.RemoveAt(0);
                        continue;
                    }
                    nSendBuffLen = BufferA.Length;//Move(BufferA, nSendBuffLen, sizeof(int));
                    if (Gate.nSendChecked == 0 && Gate.nSendBlockCount + nSendBuffLen >= M2Share.g_Config.nCheckBlock * 10)
                    {
                        //Move(DefMsg, BufferA[SizeOf(Integer) + SizeOf(TMsgHeader)], SizeOf(TDefaultMessage));
                        var DefMsg = new TDefaultMessage(BufferA);
                        M2Share.MainOutMessage(string.Format("消息 Ident:{0} Ident:{1}", DefMsg.Ident, DefMsg.Ident));
                        M2Share.MainOutMessage(string.Format("数据大小 Block:{0} sMsg:{1} {2}",M2Share.g_Config.nCheckBlock, Gate.nSendBlockCount + nSendBuffLen, nSendBuffLen));
                        
                        if (Gate.nSendBlockCount == 0 && M2Share.g_Config.nCheckBlock * 10 <= nSendBuffLen)
                        {
                            MsgList.RemoveAt(0); //如果数据大小超过指定大小则扔掉(编辑数据比较大，与此有点关系)
                            BufferA = null;
                        }
                        else
                        {
                            SendCheck(Gate.Socket, Grobal2.GM_RECEIVE_OK);
                            Gate.nSendChecked = 1;
                            Gate.dwSendCheckTick = HUtil32.GetTickCount();
                        }
                        break;
                    }
                    MsgList.RemoveAt(0);
                    BufferB = new byte[BufferA.Length + 4];
                     if (BufferA.Length > BufferB.Length)
                     {
                         Buffer.BlockCopy(BufferA, 0, BufferB, 0, BufferB.Length);
                     }
                     else
                     {
                         Buffer.BlockCopy(BufferA, 0, BufferB, 0, BufferA.Length);
                     }
                     if (nSendBuffLen > 0)
                     {
                         while (true)
                         {
                             if (M2Share.g_Config.nSendBlock <= nSendBuffLen)
                             {
                                 if (Gate.Socket != null)
                                 {
                                     if (Gate.Socket.Connected)
                                     {
                                         var SendBy = new byte[M2Share.g_Config.nSendBlock];
                                         Buffer.BlockCopy(BufferB, 0, SendBy, 0, M2Share.g_Config.nSendBlock);
                                         Gate.Socket.Send(SendBy, 0, SendBy.Length, SocketFlags.None);
                                     }
                                     Gate.nSendCount++;
                                     Gate.nSendBytesCount += M2Share.g_Config.nSendBlock;
                                 }
                                 Gate.nSendBlockCount += M2Share.g_Config.nSendBlock;
                                 Array.Resize(ref BufferB, BufferB.Length + M2Share.g_Config.nSendBlock);
                                 nSendBuffLen -= M2Share.g_Config.nSendBlock;
                                 continue;
                             }
                             if (Gate.Socket != null)
                             {
                                 if (Gate.Socket.Connected)
                                 {
                                     var sendBy = new byte[nSendBuffLen];
                                     Buffer.BlockCopy(BufferB, 0, sendBy, 0, nSendBuffLen);
                                     Gate.Socket.Send(sendBy, 0, nSendBuffLen, SocketFlags.None);
                                 }
                                 Gate.nSendCount++;
                                 Gate.nSendBytesCount += nSendBuffLen;
                                 Gate.nSendBlockCount += nSendBuffLen;
                             }
                             nSendBuffLen = 0;
                             break;
                         }
                     }
                     if (HUtil32.GetTickCount() - dwRunTick > M2Share.g_dwSocLimit)
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg2);
                M2Share.ErrorMessage(e.StackTrace, MessageType.Error);
            }
            return result;
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
            if (GateIdx <= RunSock.g_GateArr.GetUpperBound(0))
            {
                Gate = RunSock.g_GateArr[GateIdx];
                if (Gate.UserList != null)
                {
                    HUtil32.EnterCriticalSection(m_RunSocketSection);
                    try
                    {
                        try
                        {
                            for (var i = 0; i < Gate.UserList.Count; i++)
                            {
                                if (Gate.UserList[i] != null)
                                {
                                    GateUser = Gate.UserList[i];
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
                                            Gate.UserList[i] = null;
                                            Gate.nUserCount -= 1;
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
        }

        private int OpenNewUser(int nSocket, int nGSocketIdx, string sIPaddr, IList<TGateUserInfo> UserList)
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
            var MsgHeader = new TMsgHeader();
            MsgHeader.dwCode = Grobal2.RUNGATECODE;
            MsgHeader.nSocket = nSocket;
            MsgHeader.wGSocketIdx = (ushort)nSocketIndex;
            MsgHeader.wIdent = Grobal2.GM_SERVERUSERINDEX;
            MsgHeader.wUserListIndex = (ushort)nUserIdex;
            MsgHeader.nLength = 0;
            if (Socket.Connected)
            {
                var data = MsgHeader.ToByte();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        private void ExecGateMsg(int GateIdx, TGateInfo Gate, TMsgHeader MsgHeader, byte[] MsgBuff, int nMsgLen)
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
                                        var sMsg = EDcode.DeCodeString(HUtil32.GetString(MsgBuff, 12, MsgBuff.Length - 13), true);
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

        private void SendCheck(Socket Socket, int nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var MsgHeader = new TMsgHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = (ushort)nIdent,
                nLength = 0
            };
            if (Socket.Connected)
            {
                var data = MsgHeader.ToByte();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        private void LoadRunAddr()
        {
            var sFileName = ".\\RunAddr.txt";
            if (File.Exists(sFileName))
            {
                m_RunAddrList.LoadFromFile(sFileName);
                M2Share.TrimStringList(m_RunAddrList);
            }
        }

        public GateSystem()
        {
            TGateInfo Gate;
            m_RunAddrList = new StringList();
            m_RunSocketSection = new object();
            for (var i = RunSock.g_GateArr.GetLowerBound(0); i <= RunSock.g_GateArr.GetUpperBound(0); i++)
            {
                Gate = new TGateInfo
                {
                    boUsed = false,
                    Socket = null,
                    boSendKeepAlive = false,
                    nSendMsgCount = 0,
                    nSendRemainCount = 0,
                    dwSendTick = HUtil32.GetTickCount(),
                    nSendMsgBytes = 0,
                    nSendedMsgCount = 0,
                    BufferChannel = Channel.CreateUnbounded<byte[]>()
                };
                RunSock.g_GateArr[i] = Gate;
            }
            LoadRunAddr();
            n4F8 = 0;
            
            _gateSocket = new ISocketServer(20, 2048);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            _gateSocket.Init();
        }

        public bool AddGateBuffer(int gateIdx, byte[] buffer)
        {
            var result = false;
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                if (gateIdx < Grobal2.RUNGATEMAX)
                {
                    var gameGate = RunSock.g_GateArr[gateIdx];
                    if (gameGate.BufferChannel != null && buffer != null)
                    {
                        if (gameGate.boUsed && gameGate.Socket != null)
                        {
                            gameGate.BufferChannel.Writer.TryWrite(buffer);
                            result = true;
                        }
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
            return result;
        }

        public void SendOutConnectMsg(int nGateIdx, int nSocket, int nGsIdx)
        {
            var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new TMsgHeader();
            msgHeader.dwCode = Grobal2.RUNGATECODE;
            msgHeader.nSocket = nSocket;
            msgHeader.wGSocketIdx = (ushort)nGsIdx;
            msgHeader.wIdent = Grobal2.GM_DATA;
            msgHeader.nLength = Marshal.SizeOf(typeof(TDefaultMessage));
            var nLen = msgHeader.nLength + 20;
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(msgHeader.ToByte());
            backingStream.Write(defMsg.ToByte());
            var stream = backingStream.BaseStream as MemoryStream;
            var buff = stream.ToArray();
            if (!AddGateBuffer(nGateIdx, buff))
            {
                buff = null;
            }
        }

        private unsafe void SendScanMsg(TDefaultMessage DefMsg, string sMsg, int nGateIdx, int nSocket, int nGsIdx)
        {
            byte[] Buff = null;
            int nSendBytes;
            TMsgHeader  MsgHdr = new TMsgHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = nSocket,
                wGSocketIdx = (ushort)nGsIdx,
                wIdent = Grobal2.GM_DATA,
                nLength = 12
            };
            TDefaultMessage? sendDefMsg = DefMsg;
            if (sendDefMsg != null)
            {
                if (!string.IsNullOrEmpty(sMsg))
                {
                    //MsgHdr.nLength = sMsg.Length + sizeof(TDefaultMessage) + 1;
                    //nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    //GetMem(Buff, nSendBytes + sizeof(int));
                    //Move(nSendBytes, Buff, sizeof(int));
                    //Move(MsgHdr, Buff[sizeof(int)], sizeof(TMsgHeader));
                    //Move(DefMsg, Buff[sizeof(TMsgHeader) + sizeof(int)], sizeof(TDefaultMessage));
                    //Move(sMsg[1], Buff[sizeof(TDefaultMessage) + sizeof(TMsgHeader) + sizeof(int)], sMsg.Length + 1);
                    MsgHdr.nLength = sMsg.Length + 12 + 1;
                    nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    Buff = new byte[nSendBytes + sizeof(int)];//GetMem(Buff, nSendBytes + sizeof(int));
                    
                    // fixed (byte* pb = Buff)
                    // {
                    //     *(int*)pb = nSendBytes;
                    //     *(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                    //     *(TDefaultMessage*)(pb + sizeof(int) + sizeof(TMsgHeader)) = *DefMsg;
                    //     *(char*)(pb + sizeof(TDefaultMessage) + sizeof(TMsgHeader) + sizeof(int) + sMsg.Length + 1) = sMsg[0];//sMsg[1]
                    // }
                }
                else
                {
                    //MsgHdr.nLength = sizeof(TDefaultMessage);
                    //nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    //GetMem(Buff, nSendBytes + sizeof(int));
                    //Move(nSendBytes, Buff, sizeof(int));
                    //Move(MsgHdr, Buff[sizeof(int)], sizeof(TMsgHeader));
                    //Move(DefMsg, Buff[sizeof(TMsgHeader) + sizeof(int)], sizeof(TDefaultMessage));
                    MsgHdr.nLength = 12;
                    nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    Buff = new byte[nSendBytes + sizeof(int)];
                    // fixed (byte* pb = Buff)
                    // {
                    //     *(int*)pb = nSendBytes;
                    //     *(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                    //     *(TDefaultMessage*)(pb + sizeof(int) + sizeof(TMsgHeader)) = *DefMsg;
                    // }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(sMsg))
                {
                    MsgHdr.nLength = -(sMsg.Length + 1);
                    nSendBytes = Math.Abs(MsgHdr.nLength) + sizeof(TMsgHeader);
                    Buff = new byte[nSendBytes + sizeof(int)];
                    fixed (byte* pb = Buff)
                    {
                        *(int*)pb = nSendBytes;
                        *(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                        *(char*)(pb + sizeof(TMsgHeader) + sizeof(int) + sMsg.Length + 1) = sMsg[1];
                    }
                    //Move(sMsg[1], Buff[sizeof(TMsgHeader) + sizeof(int)], sMsg.Length + 1);
                }
                //if (sMsg != "")
                //{
                //    MsgHdr.nLength = -(sMsg.Length + 1);
                //    nSendBytes = Math.Abs(MsgHdr.nLength) + sizeof(TMsgHeader);
                //    GetMem(Buff, nSendBytes + sizeof(int));
                //    Move(nSendBytes, Buff, sizeof(int));
                //    Move(MsgHdr, Buff[sizeof(int)], sizeof(TMsgHeader));
                //    Move(sMsg[1], Buff[sizeof(TMsgHeader) + sizeof(int)], sMsg.Length + 1);
                //}
            }
            if (!M2Share.RunSocket.AddGateBuffer(nGateIdx, Buff))
            {
                Buff = null;
            }
        }

        public void SetGateUserList(int nGateIdx, int nSocket, TPlayObject PlayObject)
        {
            if (nGateIdx > RunSock.g_GateArr.GetUpperBound(0))
            {
                return;
            }
            var Gate = RunSock.g_GateArr[nGateIdx];
            if (Gate.UserList == null)
            {
                return;
            }
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                for (var i = 0; i < Gate.UserList.Count; i++)
                {
                    var gateUserInfo = Gate.UserList[i];
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
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
        }

        private void SendGateTestMsg(int nIndex)
        {
            var defMsg = new TDefaultMessage();
            var msgHdr = new TMsgHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = Grobal2.GM_TEST,
                nLength = 100
            };
            var nLen = msgHdr.nLength + Marshal.SizeOf(typeof(TMsgHeader));
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(msgHdr.ToByte());
            backingStream.Write(defMsg.ToByte());
            var stream = backingStream.BaseStream as MemoryStream;
            var buff = stream.ToArray();
            if (!AddGateBuffer(nIndex, buff))
            {
                buff = null;
            }
        }

        public void KickUser(string sAccount, int nSessionID,int payMode)
        {
            TGateUserInfo GateUserInfo;
            TGateInfo Gate;
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线！！！";
            try
            {
                for (var i = RunSock.g_GateArr.GetLowerBound(0); i <= RunSock.g_GateArr.GetUpperBound(0); i++)
                {
                    Gate = RunSock.g_GateArr[i];
                    if (Gate.boUsed && Gate.Socket != null && Gate.UserList != null)
                    {
                        HUtil32.EnterCriticalSection(m_RunSocketSection);
                        try
                        {
                            for (var j = 0; j < Gate.UserList.Count; j++)
                            {
                                GateUserInfo = Gate.UserList[j];
                                if (GateUserInfo == null)
                                {
                                    continue;
                                }
                                if (GateUserInfo.sAccount == sAccount || GateUserInfo.nSessionID == nSessionID)
                                {
                                    if (GateUserInfo.FrontEngine != null)
                                    {
                                        GateUserInfo.FrontEngine.DeleteHuman(i, GateUserInfo.nSocket);
                                    }
                                    if (GateUserInfo.PlayObject != null)
                                    {
                                        if (payMode == 0)
                                        {
                                            GateUserInfo.PlayObject.SysMsg(sKickUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        }
                                        else
                                        {
                                            GateUserInfo.PlayObject.SysMsg("账号付费时间已到,本机已被强行离线,请充值后再继续进行游戏！", TMsgColor.c_Red, TMsgType.t_Hint);
                                        }
                                        GateUserInfo.PlayObject.m_boEmergencyClose = true;
                                        GateUserInfo.PlayObject.m_boSoftClose = true;
                                    }
                                    GateUserInfo = null;
                                    Gate.UserList[j] = null;
                                    Gate.nUserCount -= 1;
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
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message, MessageType.Error);
            }
        }

        private string GetGateAddr(string sIPaddr)
        {
            var result = sIPaddr;
            for (var i = 0; i < n8; i++)
            {
                if (m_IPaddrArr[i].sIpaddr == sIPaddr)
                {
                    result = m_IPaddrArr[i].dIPaddr;
                    break;
                }
            }
            return result;
        }
        
        public void Run()
        {
            TGateInfo Gate;
            const string sExceptionMsg = "[Exception] TRunSocket::Run ";
            var dwRunTick = HUtil32.GetTickCount();
            if (M2Share.boStartReady)
            {
                try
                {
                    if (M2Share.g_Config.nGateLoad > 0)
                    {
                        if (HUtil32.GetTickCount() - dwSendTestMsgTick >= 100)
                        {
                            dwSendTestMsgTick = HUtil32.GetTickCount();
                            for (var i = RunSock.g_GateArr.GetLowerBound(0); i <= RunSock.g_GateArr.GetUpperBound(0); i++)
                            {
                                Gate = RunSock.g_GateArr[i];
                                if (Gate.BufferChannel != null)
                                {
                                    for (var nG = 0; nG < M2Share.g_Config.nGateLoad; nG++)
                                    {
                                        SendGateTestMsg(i);
                                    }
                                }
                            }
                        }
                    }
                    for (var i = RunSock.g_GateArr.GetLowerBound(0); i <= RunSock.g_GateArr.GetUpperBound(0); i++)
                    {
                        if (RunSock.g_GateArr[i].Socket != null)
                        {
                            Gate = RunSock.g_GateArr[i];
                            if (HUtil32.GetTickCount() - Gate.dwSendTick >= 1000)
                            {
                                Gate.dwSendTick = HUtil32.GetTickCount();
                                Gate.nSendMsgBytes = Gate.nSendBytesCount;
                                Gate.nSendedMsgCount = Gate.nSendCount;
                                Gate.nSendBytesCount = 0;
                                Gate.nSendCount = 0;
                            }
                            if (Gate.boSendKeepAlive)
                            {
                                Gate.boSendKeepAlive = false;
                                SendCheck(Gate.Socket, Grobal2.GM_CHECKSERVER);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    M2Share.ErrorMessage(sExceptionMsg, MessageType.Error);
                    M2Share.ErrorMessage(e.Message, MessageType.Error);
                }
            }
            M2Share.g_nSockCountMin = HUtil32.GetTickCount() - dwRunTick;
            if (M2Share.g_nSockCountMin > M2Share.g_nSockCountMax)
            {
                M2Share.g_nSockCountMax = M2Share.g_nSockCountMin;
            }
        }
        
        public void Start()
        {
            _gateSocket.Start(M2Share.g_Config.sGateAddr, M2Share.g_Config.nGatePort);
        }

        private void GateSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            //M2Share.RunSocket.CloseErrGate();
        }

        private void GateSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
           AddGate(e);
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            var data = new byte[e.BytesReceived];
            Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, e.BytesReceived);
            var nMsgLen = e.BytesReceived;
            if (nMsgLen <= 0)
            {
                return;
            }
            SocketRead(e.ConnectionId, data);
        }
        
        public async Task StartConsumer()
        {
            var gTasks = new Task[RunSock.g_GateArr.Length];
            for (var i = RunSock.g_GateArr.GetLowerBound(0); i <= RunSock.g_GateArr.GetUpperBound(0); i++)
            {
                var consumer = new GateConsumer(RunSock.g_GateArr[i], i);
                var consumerTask = consumer.ConsumeData();
                gTasks[i] = consumerTask;
            }
            await Task.WhenAll(gTasks);
        }
    }

    /// <summary>
    /// 网关消费者
    /// </summary>
    public class GateConsumer
    {
        private readonly ChannelReader<byte[]> _reader;
        private readonly int _identifier;
        private readonly TGateInfo _gate;

        public GateConsumer(TGateInfo gate,int identifier)
        {
            _reader = gate.BufferChannel.Reader;
            _identifier = identifier;
            _gate = gate;
        }
        
        public async Task ConsumeData()
        {
            Console.WriteLine($"RunGate Consumer ({_identifier}): Starting");
            while (await _reader.WaitToReadAsync())
            {
                if (_reader.TryRead(out var buff))
                {
                    SendGateBuffers(buff);
                }
            }
        }
        
        private void SendGateBuffers(byte[] buffer)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::SendGateBuffers -> ProcessBuff";
            const string sExceptionMsg2 = "[Exception] TRunSocket::SendGateBuffers -> SendBuff";
            var dwRunTick = HUtil32.GetTickCount();
            if (_gate.nSendChecked > 0)// 如果网关未回复状态消息，则不再发送数据
            {
                if (HUtil32.GetTickCount() - _gate.dwSendCheckTick > M2Share.g_dwSocCheckTimeOut) // 2 * 1000
                {
                    _gate.nSendChecked = 0;
                    _gate.nSendBlockCount = 0;
                }
                return;
            }
            try
            {
                var nSendBuffLen = buffer.Length; 
                if (_gate.nSendChecked == 0 && _gate.nSendBlockCount + nSendBuffLen >= M2Share.g_Config.nCheckBlock * 10)
                {
                    if (_gate.nSendBlockCount == 0 && M2Share.g_Config.nCheckBlock * 10 <= nSendBuffLen)
                    {
                        return;
                    }
                    SendCheck(_gate.Socket, Grobal2.GM_RECEIVE_OK);
                    _gate.nSendChecked = 1;
                    _gate.dwSendCheckTick = HUtil32.GetTickCount();
                }
                var sendBuffer = new byte[buffer.Length - 4];//todo 这里为啥要减4?
                Buffer.BlockCopy(buffer, 4, sendBuffer, 0, sendBuffer.Length);
                nSendBuffLen = sendBuffer.Length;
                if (nSendBuffLen > 0)
                {
                    while (true)
                    {
                        if (M2Share.g_Config.nSendBlock <= nSendBuffLen)
                        {
                            if (_gate.Socket != null)
                            {
                                if (_gate.Socket.Connected)
                                {
                                    var sendBuff = new byte[M2Share.g_Config.nSendBlock];
                                    Buffer.BlockCopy(sendBuffer, 0, sendBuff, 0, M2Share.g_Config.nSendBlock);
                                    _gate.Socket.Send(sendBuff, 0, sendBuff.Length, SocketFlags.None);
                                }
                                _gate.nSendCount++;
                                _gate.nSendBytesCount += M2Share.g_Config.nSendBlock;
                            }
                            _gate.nSendBlockCount += M2Share.g_Config.nSendBlock;
                            nSendBuffLen -= M2Share.g_Config.nSendBlock;
                            var tempBuff = new byte[nSendBuffLen];
                            Array.Copy(sendBuffer, M2Share.g_Config.nSendBlock, tempBuff, 0, nSendBuffLen);
                            sendBuffer = tempBuff;
                            continue;
                        }
                        if (_gate.Socket != null)
                        {
                            if (_gate.Socket.Connected) //发送剩下的
                            {
                                _gate.Socket.Send(sendBuffer, 0, nSendBuffLen, SocketFlags.None);
                            }
                            _gate.nSendCount++;
                            _gate.nSendBytesCount += nSendBuffLen;
                            _gate.nSendBlockCount += nSendBuffLen;
                        }
                        nSendBuffLen = 0;
                        break;
                    }
                }
                if (HUtil32.GetTickCount() - dwRunTick > M2Share.g_dwSocLimit)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg2);
                M2Share.ErrorMessage(e.StackTrace, MessageType.Error);
            }
        }
        
        private void SendCheck(Socket Socket, int nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var MsgHeader = new TMsgHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = (ushort)nIdent,
                nLength = 0
            };
            if (Socket.Connected)
            {
                var data = MsgHeader.ToByte();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }
    }
}

namespace M2Server
{
    public class RunSock
    {
        /// <summary>
        /// 最大6个游戏网关
        /// </summary>
        public static TGateInfo[] g_GateArr = new TGateInfo[6];

        public static ConcurrentDictionary<string, TGateInfo> GataSocket = new ConcurrentDictionary<string, TGateInfo>();
    }
}