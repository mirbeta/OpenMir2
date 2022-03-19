using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace GameSvr
{
    public class GameGate
    {
        /// <summary>
        /// 游戏网关
        /// </summary>
        private readonly ISocketServer _gateSocket = null;
        private object m_RunSocketSection = null;
        private StringList m_RunAddrList = null;
        private int n8 = 0;
        private TIPaddr[] m_IPaddrArr;
        private int SendTestMsgTick = 0;
        /// <summary>
        /// 最大6个游戏网关
        /// </summary>
        private TGateInfo[] g_GateArr = new TGateInfo[6];
        private ConcurrentDictionary<int, TGateInfo> GataSocket = new ConcurrentDictionary<int, TGateInfo>();

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
                for (var i = g_GateArr.GetLowerBound(0); i <= g_GateArr.GetUpperBound(0); i++)
                {
                    var Gate = g_GateArr[i];
                    if (Gate.boUsed)
                    {
                        continue;
                    }
                    Gate.GateIndex = i;
                    Gate.boUsed = true;
                    Gate.SocketId = e.ConnectionId;
                    Gate.Socket = e.Socket;
                    Gate.sAddr = GetGateAddr(e.RemoteIPaddr);
                    Gate.nPort = e.RemotePort;
                    Gate.UserList = new List<TGateUserInfo>();
                    Gate.nUserCount = 0;
                    Gate.Buffer = null;
                    Gate.nBuffLen = 0;
                    Gate.boSendKeepAlive = false;
                    Gate.nSendChecked = 0;
                    Gate.nSendBlockCount = 0;
                    GataSocket.TryAdd(e.ConnectionId, Gate);
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
            for (var GateIdx = g_GateArr.GetLowerBound(0); GateIdx <= g_GateArr.GetUpperBound(0); GateIdx++)
            {
                Gate = g_GateArr[GateIdx];
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

        private void CloseGate(AsyncUserToken e)
        {
            TGateUserInfo GateUser;
            IList<TGateUserInfo> UserList;
            TGateInfo Gate;
            const string sGateClose = "游戏网关[{0}]({1}:{2})已关闭...";
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                for (var GateIdx = g_GateArr.GetLowerBound(0); GateIdx <= g_GateArr.GetUpperBound(0); GateIdx++)
                {
                    Gate = g_GateArr[GateIdx];
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
                        Gate.Buffer = null;
                        Gate.nBuffLen = 0;
                        Gate.boUsed = false;
                        Gate.Socket = null;
                        M2Share.ErrorMessage(string.Format(sGateClose, GateIdx, e.EndPoint.Address, e.EndPoint.Port));
                        break;
                    }
                }
                GataSocket.TryRemove(e.ConnectionId, out Gate);
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
            if (data is not { Length: > 0 } || nMsgLen <= 0)
            {
                return;
            }
            try
            {
                if (data.Length > 0)
                {
                    var buffSize = GameGate.nBuffLen + nMsgLen;
                    if (GameGate.Buffer != null && buffSize > GameGate.nBuffLen)
                    {
                        var tempBuff = new byte[buffSize];
                        Array.Copy(GameGate.Buffer, 0, tempBuff, 0, GameGate.nBuffLen);
                        Array.Copy(data, 0, tempBuff, GameGate.nBuffLen, nMsgLen);
                        GameGate.Buffer = tempBuff;
                    }
                    else
                    {
                        GameGate.Buffer = new byte[buffSize];
                        Array.Copy(data, 0, GameGate.Buffer, 0, nMsgLen);
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
                nLen = GameGate.nBuffLen + nMsgLen;
                Buff = GameGate.Buffer;
                if (nLen >= MessageHeader.PacketSize)//sizeof(TMsgHeader)
                {
                    while (true)
                    {
                        var msgHeader = new MessageHeader(Buff);
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
                            if (msgHeader.nLength == 0)
                            {
                                ExecGateMsg(nGateIndex, GameGate, msgHeader, null, msgHeader.nLength);
                            }
                            else
                            {
                                byte[] msgBuff = Buff[MessageHeader.PacketSize..];//跳过消息头20字节
                                ExecGateMsg(nGateIndex, GameGate, msgHeader, msgBuff, msgHeader.nLength);
                            }
                            nLen -= (msgHeader.nLength + MessageHeader.PacketSize);
                            if (nLen < 20)
                            {
                                break;
                            }
                            var newLen = MessageHeader.PacketSize + msgHeader.nLength;
                            var tempBuff = new byte[Buff.Length - newLen];
                            Array.Copy(Buff, newLen, tempBuff, 0, tempBuff.Length);
                            Buff = tempBuff;
                            buffIndex = 0;
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
            try
            {
                if (nLen > 0)
                {
                    var tempBuff = new byte[nLen];
                    Array.Copy(Buff, 0, tempBuff, 0, nLen);
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
            if (GateIdx <= g_GateArr.GetUpperBound(0))
            {
                Gate = g_GateArr[GateIdx];
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

        private void SendCheck(Socket Socket, int nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var MsgHeader = new MessageHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = (ushort)nIdent,
                nLength = 0
            };
            if (Socket.Connected)
            {
                var data = MsgHeader.GetPacket();
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

        public GameGate()
        {
            TGateInfo Gate;
            m_RunAddrList = new StringList();
            m_RunSocketSection = new object();
            for (var i = g_GateArr.GetLowerBound(0); i <= g_GateArr.GetUpperBound(0); i++)
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
                    Queue = Channel.CreateUnbounded<byte[]>()
                };
                g_GateArr[i] = Gate;
            }
            LoadRunAddr();
            _gateSocket = new ISocketServer(ushort.MaxValue, 512);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            _gateSocket.Init();
        }

        public bool AddGateBuffer(int gateIdx, byte[] buffer)
        {
            var result = false;
            if (gateIdx < Grobal2.RUNGATEMAX)
            {
                var gameGate = g_GateArr[gateIdx];
                if (buffer != null && buffer.Length > 0)
                {
                    if (gameGate.boUsed && gameGate.Socket != null)
                    {
                        gameGate.AddToQueue(buffer);
                        result = true;
                    }
                }
            }
            return result;
        }

        public void SendOutConnectMsg(int nGateIdx, int nSocket, int nGsIdx)
        {
            var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new MessageHeader();
            msgHeader.dwCode = Grobal2.RUNGATECODE;
            msgHeader.nSocket = nSocket;
            msgHeader.wGSocketIdx = (ushort)nGsIdx;
            msgHeader.wIdent = Grobal2.GM_DATA;
            msgHeader.nLength = TDefaultMessage.PackSize;
            ClientOutMessage outMessage = new ClientOutMessage(msgHeader, defMsg);
            if (!AddGateBuffer(nGateIdx, outMessage.GetPacket()))
            {
                Console.WriteLine("发送玩家退出消息失败.");
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
            TDefaultMessage? sendDefMsg = DefMsg;
            if (sendDefMsg != null)
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
            if (!M2Share.RunSocket.AddGateBuffer(nGateIdx, Buff))
            {
                Buff = null;
            }
        }

        public void SetGateUserList(int nGateIdx, int nSocket, TPlayObject PlayObject)
        {
            if (nGateIdx > g_GateArr.GetUpperBound(0))
            {
                return;
            }
            var Gate = g_GateArr[nGateIdx];
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
            var msgHdr = new MessageHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = Grobal2.GM_TEST,
                nLength = 100
            };
            var nLen = msgHdr.nLength + Marshal.SizeOf(typeof(MessageHeader));
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(msgHdr.GetPacket());
            backingStream.Write(defMsg.GetPacket());
            memoryStream.Seek(0, SeekOrigin.Begin);
            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            if (!AddGateBuffer(nIndex, data))
            {
                data = null;
            }
        }

        public void KickUser(string sAccount, int nSessionID, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            try
            {
                for (var i = g_GateArr.GetLowerBound(0); i <= g_GateArr.GetUpperBound(0); i++)
                {
                    var Gate = g_GateArr[i];
                    if (Gate.boUsed && Gate.Socket != null && Gate.UserList != null)
                    {
                        HUtil32.EnterCriticalSection(m_RunSocketSection);
                        try
                        {
                            for (var j = 0; j < Gate.UserList.Count; j++)
                            {
                                var GateUserInfo = Gate.UserList[j];
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
                        if ((HUtil32.GetTickCount() - SendTestMsgTick) >= 100)
                        {
                            SendTestMsgTick = HUtil32.GetTickCount();
                            for (var i = g_GateArr.GetLowerBound(0); i <= g_GateArr.GetUpperBound(0); i++)
                            {
                                Gate = g_GateArr[i];
                                if (Gate.Queue != null)
                                {
                                    for (var nG = 0; nG < M2Share.g_Config.nGateLoad; nG++)
                                    {
                                        SendGateTestMsg(i);
                                    }
                                }
                            }
                        }
                    }
                    for (var i = g_GateArr.GetLowerBound(0); i <= g_GateArr.GetUpperBound(0); i++)
                    {
                        if (g_GateArr[i].Socket != null)
                        {
                            Gate = g_GateArr[i];
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
            M2Share.MainOutMessage($"游戏网关[{M2Share.g_Config.sGateAddr}:{M2Share.g_Config.nGatePort}]已启动...");
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
            CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            AddGate(e);
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            var nMsgLen = e.BytesReceived;
            if (nMsgLen <= 0)
            {
                return;
            }
            var data = new byte[nMsgLen];
            Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nMsgLen);
            if (GataSocket.TryGetValue(e.ConnectionId, out var gate))
            {
                ExecGateBuffers(gate.GateIndex, gate, data, nMsgLen);
            }
        }

        public async Task StartConsumer(CancellationToken cancellation)
        {
            var gTasks = new Task[g_GateArr.Length];
            for (var i = g_GateArr.GetLowerBound(0); i <= g_GateArr.GetUpperBound(0); i++)
            {
                var consumer = new GateConsumer(g_GateArr[i], i);
                var consumerTask = consumer.ProcessGateData(cancellation);
                gTasks[i] = consumerTask;
            }
            await Task.WhenAll(gTasks);
        }
    }
}
