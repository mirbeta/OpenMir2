using mSystemModule;
using NetFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace M2Server
{
    public class TRunSocket
    {
        public object m_RunSocketSection = null;
        public StringList m_RunAddrList = null;
        public int n8 = 0;
        public TIPaddr[] m_IPaddrArr;
        public int n4F8 = 0;
        public int dwSendTestMsgTick = 0;

        public void AddGate(AsyncUserToken e)
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
                    Gate.boUsed = true;
                    Gate.SocketId = e.ConnectionId;
                    Gate.Socket = e.Socket;
                    Gate.sAddr = GetGateAddr(e.EndPoint.Address.ToString());
                    Gate.nPort = e.EndPoint.Port;
                    Gate.n520 = 1;
                    Gate.UserList = new List<TGateUserInfo>();
                    Gate.nUserCount = 0;
                    Gate.Buffer = IntPtr.Zero;
                    Gate.nBuffLen = 0;
                    Gate.BufferList = new List<IntPtr>();
                    Gate.boSendKeepAlive = false;
                    Gate.nSendChecked = 0;
                    Gate.nSendBlockCount = 0;
                    Gate.dwStartTime = HUtil32.GetTickCount();
                    M2Share.MainOutMessage(string.Format(sGateOpen, i, e.EndPoint.Address, Gate.nPort));
                    break;
                }
            }
            else
            {
                M2Share.MainOutMessage(string.Format(sKickGate, new string[] { e.EndPoint.Address.ToString() }), MessageType.Error);
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
                                    ((TPlayObject)GateUser.PlayObject).m_boEmergencyClose = true;
                                    if (!((TPlayObject)GateUser.PlayObject).m_boReconnection)
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
                            Gate.Buffer = IntPtr.Zero;
                        }
                        Gate.Buffer = IntPtr.Zero;
                        Gate.nBuffLen = 0;
                        for (var i = 0; i < Gate.BufferList.Count; i++)
                        {
                            Gate.BufferList[i] = IntPtr.Zero;
                        }
                        Gate.BufferList = null;
                        Gate.boUsed = false;
                        Gate.Socket = null;
                        M2Share.MainOutMessage(string.Format(sGateClose, GateIdx, e.EndPoint.Address, e.EndPoint.Port));
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
        }

        private unsafe void ExecGateBuffers(int nGateIndex, TGateInfo Gate, byte[] Buffer, int nMsgLen)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::ExecGateBuffers -> pBuffer";
            const string sExceptionMsg2 = "[Exception] TRunSocket::ExecGateBuffers -> @pwork,ExecGateMsg ";
            const string sExceptionMsg3 = "[Exception] TRunSocket::ExecGateBuffers -> FreeMem";
            var nLen = 0;
            try
            {
                if (Buffer != null)
                {
                    IntPtr ip;
                    if (nMsgLen > 20)
                    {
                        ip = (IntPtr)(Gate.nBuffLen + nMsgLen);
                        if (Gate.Buffer != (IntPtr)0)
                        {
                            Gate.Buffer = Marshal.ReAllocHGlobal(Gate.Buffer, ip);
                        }
                        else
                        {
                            Gate.Buffer = Marshal.AllocHGlobal(ip);
                        }
                        Marshal.Copy(Buffer, 0, (IntPtr)((byte*)Gate.Buffer + Gate.nBuffLen), nMsgLen);
                    }
                    else
                    {
                        ip = (IntPtr)(Gate.nBuffLen + nMsgLen);
                        if (Gate.Buffer != (IntPtr)0)
                        {
                            Gate.Buffer = Marshal.ReAllocHGlobal(Gate.Buffer, ip);
                        }
                        else
                        {
                            Gate.Buffer = Marshal.AllocHGlobal(ip);
                        }
                        Marshal.Copy(Buffer, 0, (IntPtr)((byte*)Gate.Buffer + Gate.nBuffLen), nMsgLen);
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg1);
            }
            byte* Buff = null;
            try
            {
                nLen = Gate.nBuffLen + nMsgLen;
                Buff = (byte*)Gate.Buffer;
                if (nLen >= sizeof(TMsgHeader))
                {
                    while (true)
                    {
                        var MsgHeader = *(TMsgHeader*)Buff;
                        var nCheckMsgLen = Math.Abs(MsgHeader.nLength) + sizeof(TMsgHeader);
                        if (MsgHeader.dwCode == grobal2.RUNGATECODE && nCheckMsgLen < 0x8000)
                        {
                            if (nLen < nCheckMsgLen)
                            {
                                break;
                            }
                            var MsgBuff = Buff + sizeof(TMsgHeader);
                            // Jacky 1009 换上
                            // MsgBuff:=@Buff[SizeOf(TMsgHeader)];
                            ExecGateMsg(nGateIndex, Gate, MsgHeader, MsgBuff, MsgHeader.nLength);
                            Buff = Buff + sizeof(TMsgHeader) + MsgHeader.nLength;
                            // Jacky 1009 换上
                            // Buff:=@Buff[SizeOf(TMsgHeader) + pMsg.nLength];
                            nLen = nLen - (MsgHeader.nLength + sizeof(TMsgHeader));
                        }
                        else
                        {
                            Buff++;
                            nLen -= 1;
                        }
                        if (nLen < sizeof(TMsgHeader))
                        {
                            break;
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg2);
            }
            try
            {
                if (nLen > 0)
                {
                    var TempBuff = Marshal.AllocHGlobal(nLen);
                    HUtil32.IntPtrToIntPtr((IntPtr)Buff, 0, TempBuff, 0, nLen);
                    Marshal.FreeHGlobal(Gate.Buffer);
                    Gate.Buffer = TempBuff;
                    Gate.nBuffLen = nLen;
                }
                else
                {
                    Marshal.FreeHGlobal(Gate.Buffer);
                    Gate.Buffer = (IntPtr)0;
                    Gate.nBuffLen = 0;
                }
                //if (nLen > 0)
                //{
                //    GetMem(TempBuff, nLen);
                //    Move(Buff, TempBuff, nLen);
                //    FreeMem(Gate.Buffer);
                //    Gate.Buffer = TempBuff;
                //    Gate.nBuffLen = nLen;
                //}
                //else
                //{
                //    FreeMem(Gate.Buffer);
                //    Gate.Buffer = null;
                //    Gate.nBuffLen = 0;
                //}
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg3);
            }
        }

        public void SocketRead(AsyncUserToken e)
        {
            TGateInfo Gate;
            const string sExceptionMsg1 = "[Exception] TRunSocket::SocketRead";
            for (var GateIdx = RunSock.g_GateArr.GetLowerBound(0); GateIdx <= RunSock.g_GateArr.GetUpperBound(0); GateIdx++)
            {
                Gate = RunSock.g_GateArr[GateIdx];
                if (Gate.Socket == e.Socket)
                {
                    try
                    {
                        var data = new byte[e.BytesReceived];
                        Array.Copy(e.ReceiveBuffer, e.Offset, data, 0, e.BytesReceived);
                        var nMsgLen = e.BytesReceived;
                        if (nMsgLen <= 0)
                        {
                            break;
                        }
                        ExecGateBuffers(GateIdx, Gate, data, nMsgLen);
                    }
                    catch
                    {
                        M2Share.MainOutMessage(sExceptionMsg1);
                    }
                }
            }
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
                                if (Gate.BufferList != null)
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
                        Gate = RunSock.g_GateArr[i];
                        if (Gate.BufferList != null)
                        {
                            HUtil32.EnterCriticalSection(m_RunSocketSection);
                            try
                            {
                                Gate.nSendMsgCount = Gate.BufferList.Count;
                                if (SendGateBuffers(i, Gate, Gate.BufferList))
                                {
                                    Gate.nSendRemainCount = Gate.BufferList.Count;
                                }
                                else
                                {
                                    Gate.nSendRemainCount = Gate.BufferList.Count;
                                }
                            }
                            finally
                            {
                                HUtil32.LeaveCriticalSection(m_RunSocketSection);
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
                                SendCheck(Gate.Socket, grobal2.GM_CHECKSERVER);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    M2Share.MainOutMessage(sExceptionMsg, MessageType.Error);
                    M2Share.MainOutMessage(e.Message, MessageType.Error);
                }
            }
            M2Share.g_nSockCountMin = HUtil32.GetTickCount() - dwRunTick;
            if (M2Share.g_nSockCountMin > M2Share.g_nSockCountMax)
            {
                M2Share.g_nSockCountMax = M2Share.g_nSockCountMin;
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
                M2Share.MainOutMessage(sExceptionMsg, MessageType.Error);
            }
            return result;
        }

        private void DoClientCertification(int GateIdx, TGateUserInfo GateUser, int nSocket, string sMsg)
        {
            var nCheckCode = 0;
            var sData = string.Empty;
            var sAccount = string.Empty;
            var sChrName = string.Empty;
            var nSessionID = 0;
            var boFlag = false;
            var nClientVersion = 0;
            var nPayMent = 0;
            var nPayMode = 0;
            TSessInfo SessInfo;
            const string sExceptionMsg = "[Exception] TRunSocket::DoClientCertification CheckCode: ";
            const string sDisable = "*disable*";
            try
            {
                if (string.IsNullOrEmpty(GateUser.sAccount))
                {
                    if (HUtil32.TagCount(sMsg, '!') > 0)
                    {
                        sData = HUtil32.ArrestStringEx(sMsg, "#", "!", ref sMsg);
                        sMsg = sMsg.Substring(2 - 1, sMsg.Length - 1);
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
                                    M2Share.MainOutMessage(string.Format(sExceptionMsg, new int[] { nCheckCode }));
                                }
                            }
                            else
                            {
                                nCheckCode = 2;
                                GateUser.sAccount = sDisable;
                                GateUser.boCertification = false;
                                CloseUser(GateIdx, nSocket);
                                nCheckCode = 3;
                            }
                        }
                        else
                        {
                            nCheckCode = 4;
                            GateUser.sAccount = sDisable;
                            GateUser.boCertification = false;
                            CloseUser(GateIdx, nSocket);
                            nCheckCode = 5;
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage(string.Format(sExceptionMsg, new int[] { nCheckCode }));
            }
        }

        private unsafe bool SendGateBuffers(int GateIdx, TGateInfo Gate, IList<IntPtr> MsgList)
        {
            int I;
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
            IntPtr BufferA;
            IntPtr BufferB;
            // 004E198F
            // 将小数据合并为一个指定大小的数据
            try
            {
                I = 0;
                BufferA = MsgList[I];
                while (true)
                {
                    if (I + 1 >= MsgList.Count)
                    {
                        break;
                    }
                    BufferB = MsgList[I + 1];
                    if (BufferA == IntPtr.Zero || BufferB == IntPtr.Zero)
                    {
                        continue;
                    }
                    var nBuffALen = *(int*)BufferA;
                    //Move(BufferA, nBuffALen, sizeof(int));
                    var nBuffBLen = *(int*)BufferB;
                    //Move(BufferB, nBuffBLen, sizeof(int));
                    if (nBuffALen + nBuffBLen < M2Share.g_Config.nSendBlock)
                    {
                        MsgList.RemoveAt(I + 1);
                        var BufferC = Marshal.AllocHGlobal(nBuffALen + sizeof(int) + nBuffBLen);
                        //GetMem(BufferC, nBuffALen + sizeof(int) + nBuffBLen);
                        var nBuffCLen = nBuffALen + nBuffBLen;
                        *(int*)BufferC = nBuffCLen;
                        //Move(nBuffCLen, BufferC, sizeof(int));
                        HUtil32.IntPtrToIntPtr(BufferA, sizeof(int), BufferC, sizeof(int), nBuffALen);
                        //Move(BufferA[sizeof(int)], (BufferC + sizeof(int) as string), nBuffALen);
                        HUtil32.IntPtrToIntPtr(BufferB, sizeof(int), BufferC, nBuffALen + sizeof(int), nBuffBLen);
                        //Move(BufferB[sizeof(int)], (BufferC + nBuffALen + sizeof(int) as string), nBuffBLen);
                        Marshal.FreeHGlobal(BufferA);
                        //FreeMem(BufferA);
                        Marshal.FreeHGlobal(BufferB);
                        //FreeMem(BufferB);
                        BufferA = BufferC;
                        MsgList[I] = BufferA;
                        continue;
                        //MsgList.RemoveAt(I + 1);
                        //GetMem(BufferC, nBuffALen + sizeof(int) + nBuffBLen);
                        //nBuffCLen = nBuffALen + nBuffBLen;
                        //Move(nBuffCLen, BufferC, sizeof(int));
                        //Move(BufferA[sizeof(int)], (BufferC + sizeof(int) as string), nBuffALen);
                        //Move(BufferB[sizeof(int)], (BufferC + nBuffALen + sizeof(int) as string), nBuffBLen);
                        //FreeMem(BufferA);
                        //FreeMem(BufferB);
                        //BufferA = BufferC;
                        //MsgList[I] = BufferA;
                        //continue;
                    }
                    I++;
                    BufferA = BufferB;
                }
            }
            catch (Exception e)
            {
                M2Share.MainOutMessage(sExceptionMsg1);
                M2Share.MainOutMessage(e.Message, MessageType.Error);
            }
            try
            {
                while (MsgList.Count > 0)
                {
                    BufferA = MsgList[0];
                    if (BufferA == null)
                    {
                        MsgList.RemoveAt(0);
                        continue;
                    }
                    nSendBuffLen = *(int*)BufferA;//Move(BufferA, nSendBuffLen, sizeof(int));
                    if (Gate.nSendChecked == 0 && Gate.nSendBlockCount + nSendBuffLen >= M2Share.g_Config.nCheckBlock)
                    {
                        if (Gate.nSendBlockCount == 0 && M2Share.g_Config.nCheckBlock <= nSendBuffLen)
                        {
                            MsgList.RemoveAt(0);
                            // 如果数据大小超过指定大小则扔掉(编辑数据比较大，与此有点关系)
                            //FreeMem(BufferA);
                            Marshal.FreeHGlobal(BufferA);
                        }
                        else
                        {
                            SendCheck(Gate.Socket, grobal2.GM_RECEIVE_OK);
                            Gate.nSendChecked = 1;
                            Gate.dwSendCheckTick = HUtil32.GetTickCount();
                        }
                        break;
                    }
                    MsgList.RemoveAt(0);
                    BufferB = BufferA + sizeof(int);
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
                                        Marshal.Copy(BufferB, SendBy, 0, M2Share.g_Config.nSendBlock);
                                        Gate.Socket.Send(SendBy, 0, SendBy.Length, SocketFlags.None);
                                        //Gate.Socket.SendBuf(BufferB, M2Share.g_Config.nSendBlock);
                                    }
                                    Gate.nSendCount++;
                                    Gate.nSendBytesCount += M2Share.g_Config.nSendBlock;
                                }
                                Gate.nSendBlockCount += M2Share.g_Config.nSendBlock;
                                //BufferB = BufferB[M2Share.g_Config.nSendBlock];
                                BufferB = (IntPtr)((byte*)BufferB + M2Share.g_Config.nSendBlock);
                                nSendBuffLen -= M2Share.g_Config.nSendBlock;
                                continue;
                            }
                            if (Gate.Socket != null)
                            {
                                if (Gate.Socket.Connected)
                                {
                                    var SendBy = new byte[nSendBuffLen];
                                    Marshal.Copy(BufferB, SendBy, 0, nSendBuffLen);
                                    Gate.Socket.Send(SendBy, 0, nSendBuffLen, SocketFlags.None);
                                    //Gate.Socket.SendBuf(BufferB, nSendBuffLen);
                                }
                                Gate.nSendCount++;
                                Gate.nSendBytesCount += nSendBuffLen;
                                Gate.nSendBlockCount += nSendBuffLen;
                            }
                            nSendBuffLen = 0;
                            break;
                        }
                    }
                    BufferA = IntPtr.Zero;
                    if (HUtil32.GetTickCount() - dwRunTick > M2Share.g_dwSocLimit)
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.MainOutMessage(sExceptionMsg2);
                M2Share.MainOutMessage(e.Message, MessageType.Error);
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
                                                ((TFrontEngine)GateUser.FrontEngine).DeleteHuman(i, GateUser.nSocket);
                                            }
                                        }
                                        catch
                                        {
                                            M2Share.MainOutMessage(sExceptionMsg1);
                                        }
                                        try
                                        {
                                            if (GateUser.PlayObject != null)
                                            {
                                                if (!((TPlayObject)GateUser.PlayObject).m_boOffLineFlag)
                                                {
                                                    ((TPlayObject)GateUser.PlayObject).m_boSoftClose = true;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            M2Share.MainOutMessage(sExceptionMsg2);
                                        }
                                        try
                                        {
                                            if (GateUser.PlayObject != null && ((TPlayObject)GateUser.PlayObject).m_boGhost && !((TPlayObject)GateUser.PlayObject).m_boReconnection)
                                            {
                                                IdSrvClient.Instance.SendHumanLogOutMsg(GateUser.sAccount, GateUser.nSessionID);
                                            }
                                        }
                                        catch
                                        {
                                            M2Share.MainOutMessage(sExceptionMsg3);
                                        }
                                        try
                                        {
                                            GateUser = null;
                                            Gate.UserList[i] = null;
                                            Gate.nUserCount -= 1;
                                        }
                                        catch
                                        {
                                            M2Share.MainOutMessage(sExceptionMsg4);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            M2Share.MainOutMessage(sExceptionMsg0);
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

        private unsafe void SendNewUserMsg(Socket Socket, int nSocket, int nSocketIndex, int nUserIdex)
        {
            TMsgHeader MsgHeader;
            if (!Socket.Connected)
            {
                return;
            }
            MsgHeader = new TMsgHeader();
            MsgHeader.dwCode = grobal2.RUNGATECODE;
            MsgHeader.nSocket = nSocket;
            MsgHeader.wGSocketIdx = (short)nSocketIndex;
            MsgHeader.wIdent = grobal2.GM_SERVERUSERINDEX;
            MsgHeader.wUserListIndex = (short)nUserIdex;
            MsgHeader.nLength = 0;
            if (Socket != null && Socket.Connected)
            {
                var data = new byte[sizeof(TMsgHeader)];
                fixed (byte* pb = data)
                {
                    *(TMsgHeader*)pb = MsgHeader;
                }
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        private unsafe void ExecGateMsg(int GateIdx, TGateInfo Gate, TMsgHeader MsgHeader, byte* MsgBuff, int nMsgLen)
        {
            var nCheckCode = 0;
            int nUserIdx;
            string sIPaddr;
            TGateUserInfo GateUser;
            const string sExceptionMsg = "[Exception] TRunSocket::ExecGateMsg %d";
            try
            {
                switch (MsgHeader.wIdent)
                {
                    case grobal2.GM_OPEN:
                        nCheckCode = 1;
                        sIPaddr = HUtil32.SBytePtrToString((sbyte*)MsgBuff, 0, nMsgLen);
                        nUserIdx = OpenNewUser(MsgHeader.nSocket, MsgHeader.wGSocketIdx, sIPaddr, Gate.UserList);
                        SendNewUserMsg(Gate.Socket, MsgHeader.nSocket, MsgHeader.wGSocketIdx, nUserIdx + 1);
                        Gate.nUserCount++;
                        break;
                    case grobal2.GM_CLOSE:
                        nCheckCode = 2;
                        CloseUser(GateIdx, MsgHeader.nSocket);
                        break;
                    case grobal2.GM_CHECKCLIENT:
                        nCheckCode = 3;
                        Gate.boSendKeepAlive = true;
                        break;
                    case grobal2.GM_RECEIVE_OK:
                        nCheckCode = 4;
                        Gate.nSendChecked = 0;
                        Gate.nSendBlockCount = 0;
                        break;
                    case grobal2.GM_DATA:
                        nCheckCode = 5;
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
                        nCheckCode = 6;
                        if (GateUser != null)
                        {
                            if (GateUser.PlayObject != null && GateUser.UserEngine != null)
                            {
                                if (GateUser.boCertification && nMsgLen >= sizeof(TDefaultMessage))
                                {
                                    var DefMsg = (TDefaultMessage*)MsgBuff;
                                    if (nMsgLen == sizeof(TDefaultMessage))
                                    {
                                        M2Share.UserEngine.ProcessUserMessage((TPlayObject)GateUser.PlayObject, *DefMsg, null);
                                    }
                                    else
                                    {
                                        var sMsg = EncryptUnit.DeCodeString(HUtil32.StrPas(MsgBuff + sizeof(TDefaultMessage)), true);//解码
                                        M2Share.UserEngine.ProcessUserMessage((TPlayObject)GateUser.PlayObject, *DefMsg, sMsg);
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
                M2Share.MainOutMessage(string.Format(sExceptionMsg, new int[] { nCheckCode }));
            }
        }

        private unsafe void SendCheck(Socket Socket, int nIdent)
        {
            TMsgHeader MsgHeader;
            if (!Socket.Connected)
            {
                return;
            }
            MsgHeader = new TMsgHeader
            {
                dwCode = grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = (short)nIdent,
                nLength = 0
            };
            if (Socket != null && Socket.Connected)
            {
                var data = new byte[sizeof(TMsgHeader)];
                fixed (byte* pb = data)
                {
                    *(TMsgHeader*)pb = MsgHeader;
                }
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

        public TRunSocket()
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
                    nSendedMsgCount = 0
                };
                RunSock.g_GateArr[i] = Gate;
            }
            LoadRunAddr();
            n4F8 = 0;
        }

        public bool AddGateBuffer(int GateIdx, byte[] Buffer)
        {
            var result = false;
            TGateInfo Gate;
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                if (GateIdx < grobal2.RUNGATEMAX)
                {
                    Gate = RunSock.g_GateArr[GateIdx];
                    if (Gate.BufferList != null && Buffer != null)
                    {
                        if (Gate.boUsed && Gate.Socket != null)
                        {
                            var Ptr = Marshal.AllocHGlobal(Buffer.Length);
                            Marshal.Copy(Buffer, 0, Ptr, Buffer.Length);
                            Gate.BufferList.Add(Ptr);
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

        public unsafe void SendOutConnectMsg(int nGateIdx, int nSocket, int nGsIdx)
        {
            TDefaultMessage DefMsg;
            var MsgHeader = new TMsgHeader();
            int nLen;
            byte[] Buff;
            DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            MsgHeader.dwCode = grobal2.RUNGATECODE;
            MsgHeader.nSocket = nSocket;
            MsgHeader.wGSocketIdx = (short)nGsIdx;
            MsgHeader.wIdent = grobal2.GM_DATA;
            MsgHeader.nLength = Marshal.SizeOf(typeof(TDefaultMessage));
            nLen = MsgHeader.nLength + Marshal.SizeOf(typeof(TMsgHeader));
            Buff = new byte[nLen + sizeof(int)]; //GetMem(Buff, nLen + sizeof(int));
            fixed (byte* pb = Buff)
            {
                *(int*)pb = nLen;//Move(nLen, Buff, sizeof(int));
                *(TMsgHeader*)(pb + sizeof(int)) = MsgHeader;//Move(MsgHeader, Buff[sizeof(int)], Marshal.SizeOf(typeof(TMsgHeader)));
                *(TDefaultMessage*)(pb + sizeof(int) + sizeof(TMsgHeader)) = DefMsg; //Move(DefMsg, Buff[sizeof(int) + Marshal.SizeOf(typeof(TMsgHeader))], Marshal.SizeOf(typeof(TDefaultMessage)));
            }
            if (!AddGateBuffer(nGateIdx, Buff))
            {
                Buff = null;
            }
            //TDefaultMessage DefMsg;
            //TMsgHeader MsgHeader;
            //int nLen;
            //string Buff;
            //DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            //MsgHeader.dwCode = grobal2.RUNGATECODE;
            //MsgHeader.nSocket = nSocket;
            //MsgHeader.wGSocketIdx = (short)nGsIdx;
            //MsgHeader.wIdent = grobal2.GM_DATA;
            //MsgHeader.nLength = sizeof(TDefaultMessage);
            //nLen = MsgHeader.nLength + sizeof(TMsgHeader);
            //GetMem(Buff, nLen + sizeof(int));
            //Move(nLen, Buff, sizeof(int));
            //Move(MsgHeader, Buff[sizeof(int)], sizeof(TMsgHeader));
            //Move(DefMsg, Buff[sizeof(int) + sizeof(TMsgHeader)], sizeof(TDefaultMessage));
            //if (!AddGateBuffer(nGateIdx, Buff))
            //{
            //    FreeMem(Buff);
            //}
        }

        private unsafe void SendScanMsg(TDefaultMessage* DefMsg, string sMsg, int nGateIdx, int nSocket, int nGsIdx)
        {
            TMsgHeader MsgHdr;
            byte[] Buff = null;
            int nSendBytes;
            MsgHdr = new TMsgHeader
            {
                dwCode = grobal2.RUNGATECODE,
                nSocket = nSocket,
                wGSocketIdx = (short)nGsIdx,
                wIdent = grobal2.GM_DATA,
                nLength = sizeof(TDefaultMessage)
            };
            if (DefMsg != null)
            {
                if (sMsg != "")
                {
                    //MsgHdr.nLength = sMsg.Length + sizeof(TDefaultMessage) + 1;
                    //nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    //GetMem(Buff, nSendBytes + sizeof(int));
                    //Move(nSendBytes, Buff, sizeof(int));
                    //Move(MsgHdr, Buff[sizeof(int)], sizeof(TMsgHeader));
                    //Move(DefMsg, Buff[sizeof(TMsgHeader) + sizeof(int)], sizeof(TDefaultMessage));
                    //Move(sMsg[1], Buff[sizeof(TDefaultMessage) + sizeof(TMsgHeader) + sizeof(int)], sMsg.Length + 1);
                    MsgHdr.nLength = sMsg.Length + sizeof(TDefaultMessage) + 1;
                    nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    Buff = new byte[nSendBytes + sizeof(int)];//GetMem(Buff, nSendBytes + sizeof(int));
                    fixed (byte* pb = Buff)
                    {
                        *(int*)pb = nSendBytes;
                        *(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                        *(TDefaultMessage*)(pb + sizeof(int) + sizeof(TMsgHeader)) = *DefMsg;
                        *(char*)(pb + sizeof(TDefaultMessage) + sizeof(TMsgHeader) + sizeof(int) + sMsg.Length + 1) = sMsg[0];//sMsg[1]
                    }
                }
                else
                {
                    //MsgHdr.nLength = sizeof(TDefaultMessage);
                    //nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    //GetMem(Buff, nSendBytes + sizeof(int));
                    //Move(nSendBytes, Buff, sizeof(int));
                    //Move(MsgHdr, Buff[sizeof(int)], sizeof(TMsgHeader));
                    //Move(DefMsg, Buff[sizeof(TMsgHeader) + sizeof(int)], sizeof(TDefaultMessage));
                    MsgHdr.nLength = sizeof(TDefaultMessage);
                    nSendBytes = MsgHdr.nLength + sizeof(TMsgHeader);
                    Buff = new byte[nSendBytes + sizeof(int)];
                    fixed (byte* pb = Buff)
                    {
                        *(int*)pb = nSendBytes;
                        *(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                        *(TDefaultMessage*)(pb + sizeof(int) + sizeof(TMsgHeader)) = *DefMsg;
                    }
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
            TGateUserInfo GateUserInfo;
            TGateInfo Gate;
            if (nGateIdx > RunSock.g_GateArr.GetUpperBound(0))
            {
                return;
            }
            Gate = RunSock.g_GateArr[nGateIdx];
            if (Gate.UserList == null)
            {
                return;
            }
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                for (var I = 0; I < Gate.UserList.Count; I++)
                {
                    GateUserInfo = Gate.UserList[I];
                    if (GateUserInfo != null && GateUserInfo.nSocket == nSocket)
                    {
                        GateUserInfo.FrontEngine = null;
                        GateUserInfo.UserEngine = M2Share.UserEngine;
                        GateUserInfo.PlayObject = PlayObject;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
        }

        private unsafe void SendGateTestMsg(int nIndex)
        {
            TMsgHeader MsgHdr;
            byte[] Buff;
            int nLen;
            var DefMsg = new TDefaultMessage();
            MsgHdr = new TMsgHeader
            {
                dwCode = grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = grobal2.GM_TEST,
                nLength = 100
            };
            nLen = MsgHdr.nLength + Marshal.SizeOf(typeof(TMsgHeader));
            Buff = new byte[nLen + sizeof(int)];//GetMem(Buff, nLen + sizeof(int));
            fixed (byte* pb = Buff)
            {
                *(int*)pb = nLen;
                *(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                *(TDefaultMessage*)(pb + sizeof(int) + sizeof(TMsgHeader)) = DefMsg;
            }
            if (!AddGateBuffer(nIndex, Buff))
            {
                Buff = null;
            }
            //TMsgHeader MsgHdr;
            //string Buff;
            //int nLen;
            //TDefaultMessage DefMsg;
            //MsgHdr.dwCode = grobal2.RUNGATECODE;
            //MsgHdr.nSocket = 0;
            //MsgHdr.wIdent = grobal2.GM_TEST;
            //MsgHdr.nLength = 100;
            //nLen = MsgHdr.nLength + sizeof(TMsgHeader);
            //GetMem(Buff, nLen + sizeof(int));
            //Move(nLen, Buff, sizeof(int));
            //Move(MsgHdr, Buff[sizeof(int)], sizeof(TMsgHeader));
            //Move(DefMsg, Buff[sizeof(TMsgHeader) + sizeof(int)], sizeof(TDefaultMessage));
            //if (!AddGateBuffer(nIndex, Buff))
            //{
            //    FreeMem(Buff);
            //}
        }

        public void KickUser(string sAccount, int nSessionID)
        {
            TGateUserInfo GateUserInfo;
            TGateInfo Gate;
            var nCheckCode = 0;
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线！！！";
            try
            {
                nCheckCode = 0;
                for (var i = RunSock.g_GateArr.GetLowerBound(0); i <= RunSock.g_GateArr.GetUpperBound(0); i++)
                {
                    Gate = RunSock.g_GateArr[i];
                    nCheckCode = 1;
                    if (Gate.boUsed && Gate.Socket != null && Gate.UserList != null)
                    {
                        nCheckCode = 2;
                        HUtil32.EnterCriticalSection(m_RunSocketSection);
                        try
                        {
                            nCheckCode = 3;
                            for (var j = 0; j < Gate.UserList.Count; j++)
                            {
                                nCheckCode = 4;
                                GateUserInfo = Gate.UserList[j];
                                if (GateUserInfo == null)
                                {
                                    continue;
                                }
                                nCheckCode = 5;
                                if (GateUserInfo.sAccount == sAccount || GateUserInfo.nSessionID == nSessionID)
                                {
                                    nCheckCode = 6;
                                    if (GateUserInfo.FrontEngine != null)
                                    {
                                        nCheckCode = 7;
                                        ((TFrontEngine)GateUserInfo.FrontEngine).DeleteHuman(i, GateUserInfo.nSocket);
                                    }
                                    nCheckCode = 8;
                                    if (GateUserInfo.PlayObject != null)
                                    {
                                        nCheckCode = 9;
                                        ((TPlayObject)GateUserInfo.PlayObject).SysMsg(sKickUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        ((TPlayObject)GateUserInfo.PlayObject).m_boEmergencyClose = true;
                                        ((TPlayObject)GateUserInfo.PlayObject).m_boSoftClose = true;
                                    }
                                    nCheckCode = 10;
                                    GateUserInfo = null;
                                    nCheckCode = 11;
                                    Gate.UserList[j] = null;
                                    nCheckCode = 12;
                                    Gate.nUserCount -= 1;
                                    break;
                                }
                            }
                            nCheckCode = 13;
                        }
                        finally
                        {
                            HUtil32.LeaveCriticalSection(m_RunSocketSection);
                        }
                        nCheckCode = 14;
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.MainOutMessage(string.Format(sExceptionMsg, new int[] { nCheckCode }));
                M2Share.MainOutMessage(e.Message, MessageType.Error);
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
    }
}

namespace M2Server
{
    public class RunSock
    {
        public static TGateInfo[] g_GateArr = new TGateInfo[19 + 1];
        public static int g_nGateRecvMsgLenMin = 0;
        public static int g_nGateRecvMsgLenMax = 0;
        public static int nRunSocketRun = -1;
    }
}

