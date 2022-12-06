using GameSvr.Player;
using GameSvr.Services;
using NLog;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Channels;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace GameSvr.GateWay
{
    public class GameGateMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SocketServer _gateSocket;
        private readonly object m_RunSocketSection;
        private readonly Channel<ReceiveData> _receiveQueue;
        private readonly ConcurrentDictionary<int, GameGate> _GateMap;

        public GameGateMgr()
        {
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            _GateMap = new ConcurrentDictionary<int, GameGate>();
            _gateSocket = new SocketServer(100, 1024);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            m_RunSocketSection = new object();
        }

        public void Start(CancellationToken stoppingToken)
        {
            _gateSocket.Init();
            _gateSocket.Start(M2Share.Config.sGateAddr, M2Share.Config.nGatePort);
            StartMessageThread(stoppingToken);
            _logger.Info($"游戏网关[{M2Share.Config.sGateAddr}:{M2Share.Config.nGatePort}]已启动...");
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
            const string sGateOpen = "游戏网关{0}已打开...";
            const string sKickGate = "服务器未就绪: {0}";
            if (M2Share.StartReady)
            {
                if (_GateMap.Count > 20)
                {
                    e.Socket.Close();
                    return;
                }
                var gateInfo = new GameGateInfo();
                gateInfo.nSendMsgCount = 0;
                gateInfo.nSendRemainCount = 0;
                gateInfo.dwSendTick = HUtil32.GetTickCount();
                gateInfo.nSendMsgBytes = 0;
                gateInfo.nSendedMsgCount = 0;
                gateInfo.BoUsed = true;
                gateInfo.SocketId = e.ConnectionId;
                gateInfo.Socket = e.Socket;
                gateInfo.UserList = new List<GateUserInfo>();
                gateInfo.nUserCount = 0;
                gateInfo.boSendKeepAlive = false;
                gateInfo.nSendChecked = 0;
                gateInfo.nSendBlockCount = 0;
                _logger.Info(string.Format(sGateOpen, e.EndPoint));
                _GateMap.TryAdd(e.SocHandle, new GameGate(e.SocHandle, gateInfo));
                _GateMap[e.SocHandle].StartGateQueue();
            }
            else
            {
                _logger.Error(string.Format(sKickGate, e.EndPoint));
                if (e.Socket != null)
                {
                    e.Socket.Close();
                }
            }
        }

        public void CloseUser(int gateIdx, int nSocket)
        {
            if (_GateMap.ContainsKey(gateIdx))
            {
                _GateMap[gateIdx].CloseUser(nSocket);
            }
            else
            {
                M2Share.Log.LogError("未找到用户对应Socket服务.");
            }
        }

        public void KickUser(string sAccount, int nSessionID, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            try
            {
                var keys = _GateMap.Keys.ToList();
                for (int i = 0; i < _GateMap.Count; i++)
                {
                    var gateInfo = _GateMap[keys[i]].GateInfo;
                    if (gateInfo.BoUsed && gateInfo.Socket != null && gateInfo.UserList != null)
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
                                if (gateUserInfo.Account == sAccount || gateUserInfo.nSessionID == nSessionID)
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
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message, MessageType.Error);
            }
        }

        public void CloseAllGate()
        {
            var keys = _GateMap.Keys.ToList();
            for (int i = 0; i < _GateMap.Count; i++)
            {
                _GateMap[keys[i]].GateInfo.Socket.Close();
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
            const string sGateClose = "游戏网关{0}已关闭...";
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                if (!_GateMap.ContainsKey(e.SocHandle))
                {
                    return;
                }
                if (_GateMap[e.SocHandle] == null)
                {
                    Debug.WriteLine("非法请求");
                    return;
                }
                var gateInfo = _GateMap[e.SocHandle].GateInfo;
                if (gateInfo.Socket == null)
                {
                    Debug.WriteLine("Scoket为空，无需关闭");
                    return;
                }
                if (gateInfo.SocketId.Equals(e.ConnectionId))
                {
                    IList<GateUserInfo> userList = gateInfo.UserList;
                    for (var i = 0; i < userList.Count; i++)
                    {
                        var gateUser = userList[i];
                        if (gateUser != null)
                        {
                            if (gateUser.PlayObject != null)
                            {
                                gateUser.PlayObject.m_boEmergencyClose = true;
                                if (!gateUser.PlayObject.m_boReconnection)
                                {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(gateUser.Account, gateUser.nSessionID);
                                }
                            }
                            userList[i] = null;
                        }
                    }
                    gateInfo.UserList = null;
                    gateInfo.BoUsed = false;
                    gateInfo.Socket = null;
                    _logger.Error(string.Format(sGateClose, e.EndPoint));
                    if (_GateMap.Remove(e.SocHandle, out var gameGate))
                    {
                        gameGate.Stop();
                    }
                    else
                    {
                        _logger.Error("取消网关服务失败.");
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_RunSocketSection);
            }
        }

        public void SendOutConnectMsg(int nGateIdx, int nSocket, ushort nGsIdx)
        {
            var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new GameServerPacket();
            msgHeader.PacketCode = Grobal2.RUNGATECODE;
            msgHeader.Socket = nSocket;
            msgHeader.SessionId = nGsIdx;
            msgHeader.Ident = Grobal2.GM_DATA;
            msgHeader.PackLength = ClientMesaagePacket.PackSize;
            ClientOutMessage outMessage = new ClientOutMessage(msgHeader, defMsg);
            if (!AddGateBuffer(nGateIdx, outMessage.GetBuffer()))
            {
                M2Share.Log.LogError("发送玩家退出消息失败.");
            }
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nGateIdx, int nSocket, PlayObject PlayObject)
        {
            if (!_GateMap.ContainsKey(nGateIdx))
            {
                return;
            }
            var gameGate = _GateMap[nGateIdx];
            gameGate.SetGateUserList(nSocket, PlayObject);
        }

        public void Run()
        {
            var dwRunTick = HUtil32.GetTickCount();
            if (M2Share.StartReady)
            {
                if (_GateMap.IsEmpty)
                {
                    var gateServiceList = _GateMap.Values.ToList();
                    foreach (var gateService in gateServiceList)
                    {
                        var gateInfo = gateService.GateInfo;
                        if (gateInfo.Socket != null && gateInfo.Socket.Connected)
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
            var defMsg = new ClientMesaagePacket();
            var msgHdr = new GameServerPacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = Grobal2.GM_TEST,
                PackLength = 100
            };
            var nLen = msgHdr.PackLength + GameServerPacket.PacketSize;
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(msgHdr.GetBuffer());
            backingStream.Write(defMsg.GetBuffer());
            memoryStream.Seek(0, SeekOrigin.Begin);
            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            if (!M2Share.GateMgr.AddGateBuffer(nIndex, data))
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
            var msgHeader = new GameServerPacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = (ushort)nIdent,
                PackLength = 0
            };
            if (Socket.Connected)
            {
                var data = msgHeader.GetBuffer();
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
            if (_GateMap.ContainsKey(gateIdx))
            {
                _GateMap[gateIdx].HandleSendBuffer(buffer);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 收到GameGate发来的消息并添加到GameSvr消息队列
        /// </summary>
        public void AddGameGateQueue(int gateIdx, GameServerPacket packet, byte[] data)
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
        private void StartMessageThread(CancellationToken cancellation)
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _receiveQueue.Reader.WaitToReadAsync(cancellation))
                {
                    while (_receiveQueue.Reader.TryRead(out var message))
                    {
                        // ExecGateBuffers(message.Packet, message.Data);
                    }
                }
            }, cancellation, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        #region Socket Events

        private void GateSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            //M2Share.RunSocket.CloseErrGate();
            M2Share.Log.LogError(e.Exception.StackTrace);
        }

        private void GateSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            M2Share.GateMgr.CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            M2Share.GateMgr.AddGate(e);
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            if (_GateMap.ContainsKey(e.SocHandle))
            {
                var nMsgLen = e.BytesReceived;
                if (nMsgLen <= 0)
                {
                    return;
                }
                Span<byte> data = stackalloc byte[e.BytesReceived];
                MemoryCopy.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nMsgLen);
                _GateMap[e.SocHandle].ProcessReceiveBuffer(nMsgLen, data);
            }
            else
            {
                M2Share.Log.LogError("错误的网关数据");
            }
        }

        #endregion
    }

    public struct ReceiveData
    {
        public GameServerPacket Packet;
        public byte[] Data;
        public int GateId;
    }
}