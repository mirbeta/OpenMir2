using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace GameSvr
{
    public class GateManager
    {
        private static readonly GateManager instance = new GateManager();
        public static GateManager Instance => instance;

        private int SendTestMsgTick = 0;
        /// <summary>
        /// 最大6个游戏网关 要从配置文件读取网关数量
        /// </summary>
        private TGateInfo[] g_GateArr = new TGateInfo[1];
        private TIPaddr[] m_IPaddrArr;
        private object m_RunSocketSection = null;
        private StringList m_RunAddrList = null;
        private int n8 = 0;
        private Channel<ReceiveData> _receiveQueue;
        private readonly IList<GateService> gateList = new List<GateService>();
        private readonly ConcurrentDictionary<int, GateService> gataMap = new ConcurrentDictionary<int, GateService>();

        private GateManager()
        {
            m_RunAddrList = new StringList();
            m_RunSocketSection = new object();
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            for (var i = g_GateArr.GetLowerBound(0); i <= g_GateArr.GetUpperBound(0); i++)
            {
                TGateInfo Gate = new TGateInfo
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
                g_GateArr[i] = Gate;
            }
        }

        public void Initialization()
        {
            for (var i = 0; i < g_GateArr.Length; i++)
            {
                var gameGate = g_GateArr[i];
                gateList.Add(new GateService(i, M2Share.g_Config.sGateAddr, M2Share.g_Config.nGatePort, gameGate));
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

        public void OpenGate(AsyncUserToken e)
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
                    M2Share.MainOutMessage(string.Format(sGateOpen, i, e.EndPoint.Address, Gate.nPort));
                    gataMap.TryAdd(Gate.GateIndex, gateList[i]);
                    break;
                }
            }
            else
            {
                M2Share.ErrorMessage(string.Format(sKickGate, new object?[] { e.EndPoint.Address.ToString() }), MessageType.Error);
                e.Socket.Close();
            }
        }

        public void CloseUser(int GateIdx, int nSocket)
        {
            if (gataMap.ContainsKey(GateIdx))
            {
                gataMap[GateIdx].CloseUser(GateIdx, nSocket);
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

        public void CloseAllGate()
        {
            for (var GateIdx = g_GateArr.GetLowerBound(0); GateIdx <= g_GateArr.GetUpperBound(0); GateIdx++)
            {
                var Gate = g_GateArr[GateIdx];
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
                //gataMap.TryRemove(e.ConnectionId, out Gate);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
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

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
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
                                for (var nG = 0; nG < M2Share.g_Config.nGateLoad; nG++)
                                {
                                    SendGateTestMsg(i);
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
            var nLen = msgHdr.nLength + MessageHeader.PacketSize;
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(msgHdr.GetPacket());
            backingStream.Write(defMsg.GetPacket());
            memoryStream.Seek(0, SeekOrigin.Begin);
            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            if (!M2Share.GateManager.AddGateBuffer(nIndex, data))
            {
                data = null;
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

        public void Start()
        {
            var serverQueueTask = new Task[gateList.Count];
            for (int i = 0; i < gateList.Count; i++)
            {
                if (gateList[i] == null)
                {
                    continue;
                }
                serverQueueTask[i] = gateList[i].Start();
            }
            Task.WhenAll(serverQueueTask);
        }

        public void Stop()
        {
            for (int i = 0; i < gateList.Count; i++)
            {
                if (gateList[i] != null)
                {
                    gateList[i].Stop();
                }
            }
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr->GameGate
        /// </summary>
        /// <param name="gateIdx"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool AddGateBuffer(int gateIdx, byte[] buffer)
        {
            var result = false;
            if (gateIdx < Grobal2.RUNGATEMAX)
            {
                var gameGate = gataMap[gateIdx];
                if (buffer != null && buffer.Length > 0)
                {
                    gameGate.AddGateBuffer(buffer);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 收到GameGate发来的消息并添加到GameSvr消息队列
        /// </summary>
        public void AddToQueue(ReceiveData receiveData)
        {
            _receiveQueue.Writer.TryWrite(receiveData);
        }

        /// <summary>
        /// 处理GameGate发过来的消息
        /// </summary>
        public async Task StartMessageQueue(CancellationToken cancellation)
        {
            while (await _receiveQueue.Reader.WaitToReadAsync(cancellation))
            {
                if (_receiveQueue.Reader.TryRead(out var message))
                {
                    if (gataMap.TryGetValue(message.GateId, out var gameGate))
                    {
                        gameGate.ExecGateBuffers(message.GateId, message.Buffer, message.BuffLen);
                    }
                }
            }
        }
    }

    public struct ReceiveData
    {
        public byte[] Buffer;
        public int BuffLen;
        public int GateId;
    }
}