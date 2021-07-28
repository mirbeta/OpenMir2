using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;

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
                    Gate.Buffer = null;
                    Gate.nBuffLen = 0;
                    Gate.BufferList = new List<byte[]>();
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
                        for (var i = 0; i < Gate.BufferList.Count; i++)
                        {
                            Gate.BufferList[i] = null;
                        }
                        Gate.BufferList = null;
                        Gate.boUsed = false;
                        Gate.Socket = null;
                        M2Share.ErrorMessage(string.Format(sGateClose, GateIdx, e.EndPoint.Address, e.EndPoint.Port));
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
        }

        public static byte[] Redim (byte[] origArray, int desiredSize)
        {
            byte[] newArray = new byte[desiredSize];
            Array.Copy (origArray, 0, newArray, 0, Math.Min (origArray.Length, desiredSize));
            return newArray;
        }
        
        private void ExecGateBuffers(int nGateIndex, TGateInfo GameGate, byte[] data, int nMsgLen)
        {
            const string sExceptionMsg1 = "[Exception] TRunSocket::ExecGateBuffers -> pBuffer";
            const string sExceptionMsg2 = "[Exception] TRunSocket::ExecGateBuffers -> @pwork,ExecGateMsg ";
            const string sExceptionMsg3 = "[Exception] TRunSocket::ExecGateBuffers -> FreeMem";
            var nLen = 0;
            try
            {
                if (data != null && data.Length > 0)
                {
                    int buffSize = GameGate.nBuffLen + nMsgLen;
                    if (GameGate.Buffer != null && buffSize > GameGate.nBuffLen)
                    {
                        Array.Resize(ref GameGate.Buffer, buffSize);
                    }
                    else
                    {
                        GameGate.Buffer = new byte[buffSize];
                    }
                    Buffer.BlockCopy(data, 0, GameGate.Buffer, 0, nMsgLen);
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
                        if (msgHeader.dwCode == grobal2.RUNGATECODE && nCheckMsgLen < 0x8000)
                        {
                            if (nLen < nCheckMsgLen)
                            {
                                break;
                            }
                            var msgBuff = new byte[msgHeader.nLength];
                            Buffer.BlockCopy(Buff, 20, msgBuff, 0, msgHeader.nLength);
                            ExecGateMsg(nGateIndex, GameGate, msgHeader, msgBuff, msgHeader.nLength);
                            nLen = nLen - (msgHeader.nLength + 20);
                        }
                        else
                        {
                            //Buff++; //buff大小-1
                            nLen -= 1;
                            Console.WriteLine("看这里，看这里。。。");
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
                /*if (nLen > 0)
                {
                    var tempBuff = Marshal.AllocHGlobal(nLen);
                    HUtil32.IntPtrToIntPtr((IntPtr)Buff, 0, tempBuff, 0, nLen);
                    Marshal.FreeHGlobal(GameGate.Buffer);
                    GameGate.Buffer = tempBuff;
                    GameGate.nBuffLen = nLen;
                }
                else
                {
                    Marshal.FreeHGlobal(GameGate.Buffer);
                    GameGate.Buffer = IntPtr.Zero;
                    GameGate.nBuffLen = 0;
                }*/
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg3);
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
                                    M2Share.ErrorMessage(string.Format(sExceptionMsg));
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
        
        public static unsafe string StrPas(byte* Buff)
        {
            var nLen = 0;
            var pb = Buff;
            while (*pb++ != 0)
            {
                nLen++;
            }
            var ret = new string('\0', nLen);
            var sb = new StringBuilder(ret);
            pb = Buff;
            for (var i = 0; i < nLen; i++)
            {
                sb[i] = (char)*pb++;
            }
            return sb.ToString();
        }

        public unsafe byte[] PointToBytes(byte* Buff)
        {
            var nLen = 0;
            var pb = Buff;
            while (*pb++ != 0) nLen++;
            var sb = new byte[nLen];
            for (var i = 0; i < nLen; i++)
            {
                sb[i] = *pb++;
            }
            return sb;
        }
        
        public unsafe void Move(IntPtr Src, int SrcIndex, IntPtr Dest, int DestIndex, int nLen)
        {
            byte* pSrc = (byte*)Src + SrcIndex;
            byte* pDest = (byte*)Dest + DestIndex;
            if (pDest > pSrc)
            {
                pDest = pDest + (nLen - 1);
                pSrc = pSrc + (nLen - 1);
                for (int i = 0; i < nLen; i++)
                    *pDest-- = *pSrc--;
            }
            else
            {
                for (int i = 0; i < nLen; i++)
                    *pDest++ = *pSrc++;
            }
            var ccc = StrPas(pDest);
            var aaaa = PointToBytes(pSrc);
            var bbbb = PointToBytes(pDest);
        }
        
        private unsafe bool SendGateBuffers(int GateIdx, TGateInfo Gate, IList<byte[]> MsgList)
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
                BufferA = MsgList[msgIdx];//得到第一个消息
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
                    //Move(BufferA, nBuffALen, sizeof(int));
                    var nBuffBLen = BufferB.Length;
                    //Move(BufferB, nBuffBLen, sizeof(int));
                    if (nBuffALen + nBuffBLen < M2Share.g_Config.nSendBlock)
                    {
                        MsgList.RemoveAt(msgIdx + 1);
                        //var BufferC = Marshal.AllocHGlobal(nBuffALen + sizeof(int) + nBuffBLen);
                        //var nBuffCLen = nBuffALen + nBuffBLen;
                        //*(int*)BufferC = nBuffCLen;

                        //var BufferC = new byte[nBuffCLen];
                        
                        //HUtil32.IntPtrToIntPtr(BufferA, sizeof(int), BufferC, sizeof(int), nBuffALen);
                        //HUtil32.IntPtrToIntPtr(BufferB, sizeof(int), BufferC, nBuffALen + sizeof(int), nBuffBLen);
                        
                        
                        //byte[] managedArraya = new byte[nBuffALen];
                        //Marshal.Copy(BufferA, managedArraya, 0, nBuffALen);
                        //byte[] managedArrayb = new byte[nBuffBLen];
                        //Marshal.Copy(BufferB, managedArrayb, 0, nBuffBLen);

                        using var memoryStream = new MemoryStream();
                        var backingStream = new BinaryWriter(memoryStream);
                        backingStream.Write(BufferA);
                        backingStream.Write(BufferB);
                        var stream = backingStream.BaseStream as MemoryStream;
                        var BufferC = stream.ToArray();
                        
                        //Move(BufferA, sizeof(int), BufferC, sizeof(int), nBuffALen); //Move(BufferA[sizeof(int)], BufferC[sizeof(int)], nBuffALen);
                        //Move(BufferB, sizeof(int), BufferC, nBuffALen + sizeof(int), nBuffBLen); //Move(BufferB[sizeof(int)], BufferC[nBuffALen + sizeof(int)], nBuffBLen);
                        
                        //Marshal.FreeHGlobal(BufferA);
                        //Marshal.FreeHGlobal(BufferB);
                        BufferA = BufferC;
                        MsgList[msgIdx] = BufferA;
                        
                        //byte[] oldBuff = new byte[nBuffCLen];
                        //Marshal.Copy(BufferA, oldBuff, 0, nBuffCLen);

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
                //这里要优化，这里无法多线程发送数据，优化这里会对游戏体验提升比较大
                //当只有一个网关或多个网关的时候当MsgList内容数据比较多的时候会进入当线程处理，
                //会导致其他网关或者用户收到的消息会需要等待当前处理完才能处理其他玩家数据,正确的做法应该是一个网关对应一个MsgList
                while (MsgList.Count > 0) 
                {
                    BufferA = MsgList[0];
                    if (BufferA == null)
                    {
                        MsgList.RemoveAt(0);
                        continue;
                    }
                    nSendBuffLen = BufferA.Length;//Move(BufferA, nSendBuffLen, sizeof(int));
                    if (Gate.nSendChecked == 0 && Gate.nSendBlockCount + nSendBuffLen >= M2Share.g_Config.nCheckBlock)
                    {
                        if (Gate.nSendBlockCount == 0 && M2Share.g_Config.nCheckBlock <= nSendBuffLen)
                        {
                            MsgList.RemoveAt(0);
                            //Marshal.FreeHGlobal(BufferA);
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
                    //BufferB = BufferA + sizeof(int);
                    //BufferB = BufferA;
                    BufferB = new byte[BufferA.Length + 4];
                    Buffer.BlockCopy(BufferA, 0, BufferB, 0, BufferA.Length);
                    //var ddddddd = BufferA.Length;
                    //byte[] managedArrayb = new byte[ddddddd + 4];
                    //Marshal.Copy(BufferB, managedArrayb, 0, ddddddd + 4);
                    
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
                                //BufferB = BufferB[M2Share.g_Config.nSendBlock];
                                //BufferB = (IntPtr)((byte*)BufferB + M2Share.g_Config.nSendBlock);
                                Console.WriteLine("来看这里，这里有问题");
                                nSendBuffLen -= M2Share.g_Config.nSendBlock;
                                continue;
                            }
                            if (Gate.Socket != null)
                            {
                                if (Gate.Socket.Connected)
                                {
                                    var SendBy = new byte[nSendBuffLen];
                                    Buffer.BlockCopy(BufferB, 0, SendBy, 0, nSendBuffLen);
                                    Gate.Socket.Send(SendBy, 0, nSendBuffLen, SocketFlags.None);
                                }
                                Gate.nSendCount++;
                                Gate.nSendBytesCount += nSendBuffLen;
                                Gate.nSendBlockCount += nSendBuffLen;
                            }
                            nSendBuffLen = 0;
                            break;
                        }
                    }
                    BufferA = null;
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
                M2Share.ErrorMessage(e.Message, MessageType.Error);
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
                                            M2Share.ErrorMessage(sExceptionMsg1);
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
                                            M2Share.ErrorMessage(sExceptionMsg2);
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
            MsgHeader.dwCode = grobal2.RUNGATECODE;
            MsgHeader.nSocket = nSocket;
            MsgHeader.wGSocketIdx = (ushort)nSocketIndex;
            MsgHeader.wIdent = grobal2.GM_SERVERUSERINDEX;
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
            const string sExceptionMsg = "[Exception] TRunSocket::ExecGateMsg %d";
            try
            {
                switch (MsgHeader.wIdent)
                {
                    case grobal2.GM_OPEN:
                        sIPaddr = HUtil32.GetString(MsgBuff, 0, nMsgLen);
                        nUserIdx = OpenNewUser(MsgHeader.nSocket, MsgHeader.wGSocketIdx, sIPaddr, Gate.UserList);
                        SendNewUserMsg(Gate.Socket, MsgHeader.nSocket, MsgHeader.wGSocketIdx, nUserIdx + 1);
                        Gate.nUserCount++;
                        break;
                    case grobal2.GM_CLOSE:
                        CloseUser(GateIdx, MsgHeader.nSocket);
                        break;
                    case grobal2.GM_CHECKCLIENT:
                        Gate.boSendKeepAlive = true;
                        break;
                    case grobal2.GM_RECEIVE_OK:
                        Gate.nSendChecked = 0;
                        Gate.nSendBlockCount = 0;
                        break;
                    case grobal2.GM_DATA:
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
                                if (GateUser.boCertification && nMsgLen >= 12)//sizeof(TDefaultMessage)
                                {
                                    var defMsg = new TDefaultMessage(MsgBuff);
                                    if (nMsgLen == 12) 
                                    {
                                        M2Share.UserEngine.ProcessUserMessage(GateUser.PlayObject, defMsg, null);
                                    }
                                    else
                                    {
                                        var sMsg = EDcode.DeCodeString(HUtil32.GetString(MsgBuff, 12, MsgBuff.Length-13), true);
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
                dwCode = grobal2.RUNGATECODE,
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
                            Gate.BufferList.Add(Buffer);
                            result = true;
                            //Marshal.FreeHGlobal(Ptr);
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
            var defMsg = grobal2.MakeDefaultMsg(grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new TMsgHeader();
            msgHeader.dwCode = grobal2.RUNGATECODE;
            msgHeader.nSocket = nSocket;
            msgHeader.wGSocketIdx = (ushort)nGsIdx;
            msgHeader.wIdent = grobal2.GM_DATA;
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
                dwCode = grobal2.RUNGATECODE,
                nSocket = nSocket,
                wGSocketIdx = (ushort)nGsIdx,
                wIdent = grobal2.GM_DATA,
                nLength = 12
            };
            if (DefMsg != null)
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
                for (var i = 0; i < Gate.UserList.Count; i++)
                {
                    GateUserInfo = Gate.UserList[i];
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

        private void SendGateTestMsg(int nIndex)
        {
            var DefMsg = new TDefaultMessage();
            var MsgHdr = new TMsgHeader
            {
                dwCode = grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = grobal2.GM_TEST,
                nLength = 100
            };
            var nLen = MsgHdr.nLength + Marshal.SizeOf(typeof(TMsgHeader));
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(MsgHdr.ToByte());
            backingStream.Write(DefMsg.ToByte());
            var stream = backingStream.BaseStream as MemoryStream;
            var buff = stream.ToArray();
            if (!AddGateBuffer(nIndex, buff))
            {
                buff = null;
            }
        }

        public void KickUser(string sAccount, int nSessionID)
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
                                        ((TFrontEngine)GateUserInfo.FrontEngine).DeleteHuman(i, GateUserInfo.nSocket);
                                    }
                                    if (GateUserInfo.PlayObject != null)
                                    {
                                        ((TPlayObject)GateUserInfo.PlayObject).SysMsg(sKickUserMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                        ((TPlayObject)GateUserInfo.PlayObject).m_boEmergencyClose = true;
                                        ((TPlayObject)GateUserInfo.PlayObject).m_boSoftClose = true;
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
    }
}

namespace M2Server
{
    public class RunSock
    {
        public static TGateInfo[] g_GateArr = new TGateInfo[20];
    }
}