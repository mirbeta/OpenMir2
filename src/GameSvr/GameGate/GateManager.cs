using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly ISocketServer _gateSocket = null;
        private object m_RunSocketSection = null;
        private Channel<ReceiveData> _receiveQueue;
        private readonly object runSocketSection;
        private ConcurrentDictionary<int, GateService> _gateDataService;
        private readonly IList<Task> _runTask;
        private int _runCount = 0;

        private GateManager()
        {
            _runTask = new List<Task>();
            m_RunSocketSection = new object();
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            _gateDataService = new ConcurrentDictionary<int, GateService>();
            _gateSocket = new ISocketServer(10, 4096);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            _gateSocket.Init();
        }

        public void Start()
        {
            _gateSocket.Start(M2Share.g_Config.sGateAddr, M2Share.g_Config.nGatePort);
            M2Share.MainOutMessage($"游戏网关[{ M2Share.g_Config.sGateAddr}:{M2Share.g_Config.nGatePort}]已启动...");
        }

        public void StartQueue()
        {
            Task.WhenAll(_runTask);
        }

        public void Stop()
        {
            _gateSocket.Shutdown();
        }

        private void LoadRunAddr()
        {
            var sFileName = ".\\RunAddr.txt";
            if (File.Exists(sFileName))
            {
                var runAddrList = new StringList();
                runAddrList.LoadFromFile(sFileName);
                M2Share.TrimStringList(runAddrList);
            }
        }

        private void AddGate(AsyncUserToken e)
        {
            const string sGateOpen = "游戏网关({0})已打开...";
            const string sKickGate = "服务器未就绪: {0}";
            if (M2Share.boStartReady)
            {
                if (_gateDataService.Count > 20)
                {
                    e.Socket.Close();
                }
                var gateInfo = new TGateInfo();
                gateInfo.nSendMsgCount = 0;
                gateInfo.nSendRemainCount = 0;
                gateInfo.dwSendTick = HUtil32.GetTickCount();
                gateInfo.nSendMsgBytes = 0;
                gateInfo.nSendedMsgCount = 0;
                gateInfo.boUsed = true;
                gateInfo.SocketId = e.ConnectionId;
                gateInfo.Socket = e.Socket;
                gateInfo.UserList = new List<TGateUserInfo>();
                gateInfo.nUserCount = 0;
                gateInfo.boSendKeepAlive = false;
                gateInfo.nSendChecked = 0;
                gateInfo.nSendBlockCount = 0;
                M2Share.MainOutMessage(string.Format(sGateOpen, e.EndPoint));
                _gateDataService.TryAdd(e.ConnectionId, new GateService(e.ConnectionId, gateInfo));
                _gateDataService[e.ConnectionId].StartQueueService();
            }
            else
            {
                M2Share.ErrorMessage(string.Format(sKickGate, new object?[] { e.EndPoint.Address.ToString() }), MessageType.Error);
                e.Socket.Close();
            }
        }

        public void CloseUser(int gateIdx, int nSocket)
        {
            if (_gateDataService.ContainsKey(gateIdx))
            {
                _gateDataService[gateIdx].CloseUser(nSocket);
            }
            else
            {
                Console.WriteLine("未找到用户对应Socket服务.");
            }
        }

        public void KickUser(string sAccount, int nSessionID, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            try
            {
                var keys = _gateDataService.Keys.ToList();
                for (int i = 0; i < _gateDataService.Count; i++)
                {
                    var gateInfo = _gateDataService[keys[i]].GateInfo;
                    if (gateInfo.boUsed && gateInfo.Socket != null && gateInfo.UserList != null)
                    {
                        HUtil32.EnterCriticalSection(m_RunSocketSection);
                        try
                        {
                            for (var j = 0; j < gateInfo.UserList.Count; j++)
                            {
                                var gateUserInfo = gateInfo.UserList[j];
                                if (gateUserInfo == null)
                                {
                                    continue;
                                }
                                if (gateUserInfo.sAccount == sAccount || gateUserInfo.nSessionID == nSessionID)
                                {
                                    if (gateUserInfo.FrontEngine != null)
                                    {
                                        gateUserInfo.FrontEngine.DeleteHuman(i, gateUserInfo.nSocket);
                                    }
                                    if (gateUserInfo.PlayObject != null)
                                    {
                                        if (payMode == 0)
                                        {
                                            gateUserInfo.PlayObject.SysMsg(sKickUserMsg, MsgColor.Red, MsgType.Hint);
                                        }
                                        else
                                        {
                                            gateUserInfo.PlayObject.SysMsg("账号付费时间已到,本机已被强行离线,请充值后再继续进行游戏!", MsgColor.Red, MsgType.Hint);
                                        }
                                        gateUserInfo.PlayObject.m_boEmergencyClose = true;
                                        gateUserInfo.PlayObject.m_boSoftClose = true;
                                    }
                                    gateUserInfo = null;
                                    gateInfo.UserList[j] = null;
                                    gateInfo.nUserCount -= 1;
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

        public void CloseAllGate()
        {
            var keys = _gateDataService.Keys.ToList();
            for (int i = 0; i < _gateDataService.Count; i++)
            {
                _gateDataService[keys[i]].GateInfo.Socket.Close();
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
            const string sGateClose = "游戏网关({0}:{1})已关闭...";
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                if (_gateDataService[e.ConnectionId] == null)
                {
                    Console.WriteLine("非法请求");
                    return;
                }
                Gate = _gateDataService[e.ConnectionId].GateInfo;
                if (Gate.Socket == null)
                {
                    Console.WriteLine("Scoket以关闭，无需关闭");
                    return;
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
                    Gate.boUsed = false;
                    Gate.Socket = null;
                    M2Share.ErrorMessage(string.Format(sGateClose, e.EndPoint.Address, e.EndPoint.Port));
                    if(_gateDataService.Remove(e.ConnectionId, out var gateService))
                    {
                        gateService.Stop();
                    }
                    else
                    {
                        Console.WriteLine("取消网关服务失败.");
                    }
                }
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
            if (!_gateDataService.ContainsKey(nGateIdx))
            {
                return;
            }
            var dataService = _gateDataService[nGateIdx];
            dataService?.SetGateUserList(nSocket, PlayObject);
        }

        public void Run()
        {
            var dwRunTick = HUtil32.GetTickCount();
            if (M2Share.boStartReady)
            {
                if (_gateDataService.Count > 0)
                {
                    var gateServiceList = _gateDataService.Values.ToList();
                    foreach (var gateService in gateServiceList)
                    {
                        var gateInfo = gateService.GateInfo;
                        if (gateInfo.Socket != null)
                        {
                            if (HUtil32.GetTickCount() - gateInfo.dwSendTick >= 1000)
                            {
                                gateInfo.dwSendTick = HUtil32.GetTickCount();
                                gateInfo.nSendMsgBytes = gateInfo.nSendBytesCount;
                                gateInfo.nSendedMsgCount = gateInfo.nSendCount;
                                gateInfo.nSendBytesCount = 0;
                                gateInfo.nSendCount = 0;
                            }
                            if (gateInfo.boSendKeepAlive)
                            {
                                gateInfo.boSendKeepAlive = false;
                                SendCheck(gateInfo.Socket, Grobal2.GM_CHECKSERVER);
                            }
                        }
                    }
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

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr->GameGate
        /// </summary>
        /// <returns></returns>
        public bool AddGateBuffer(int gateIdx, byte[] buffer)
        {
            var result = false;
            if (_gateDataService.ContainsKey(gateIdx))
            {
                _gateDataService[gateIdx].HandleSendBuffer(buffer);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 收到GameGate发来的消息并添加到GameSvr消息队列
        /// </summary>
        public void AddGameGateQueue(int gateIdx, MessageHeader packet, byte[] data)
        {
            _receiveQueue.Writer.TryWrite(new ReceiveData()
            {
                GateId = gateIdx,
                Packet = packet,
                Data = data
            });
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
                    //ExecGateBuffers(message.Packet, message.Data);
                }
            }
        }

        #region Socket Events

        private void GateSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            //M2Share.RunSocket.CloseErrGate();
            Console.WriteLine(e.Exception);
        }

        private void GateSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            M2Share.GateManager.CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            M2Share.GateManager.AddGate(e);
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            if (_gateDataService.ContainsKey(e.ConnectionId))
            {
                var data = new byte[e.BytesReceived];
                Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, e.BytesReceived);
                var nMsgLen = e.BytesReceived;
                _gateDataService[e.ConnectionId].HandleReceiveBuffer(nMsgLen, data);
            }
            else
            {
                Console.WriteLine("错误的网关数据");
            }
        }

        #endregion
    }

    public struct ReceiveData
    {
        public MessageHeader Packet;
        public byte[] Data;
        public int GateId;
    }
}