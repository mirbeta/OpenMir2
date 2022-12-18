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

namespace GameSvr.GameGate
{
    public class GameGateMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SocketServer _gateSocket;
        private readonly object m_RunSocketSection;
        private readonly Channel<ReceiveData> _receiveQueue;
        private readonly ConcurrentDictionary<int, GameGate> GameGateMap;

        public GameGateMgr()
        {
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            GameGateMap = new ConcurrentDictionary<int, GameGate>();
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
                if (GameGateMap.Count > 20)
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
                gateInfo.GateBuff = new byte[1024 * 10];
                _logger.Info(string.Format(sGateOpen, e.EndPoint));
                GameGateMap.TryAdd(e.SocHandle, new GameGate(e.SocHandle, gateInfo));
                GameGateMap[e.SocHandle].StartGateQueue();
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
            if (GameGateMap.ContainsKey(gateIdx))
            {
                GameGateMap[gateIdx].CloseUser(nSocket);
            }
            else
            {
                M2Share.Log.Error("未找到用户对应Socket服务.");
            }
        }

        public void KickUser(string sAccount, int nSessionID, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            try
            {
                var keys = GameGateMap.Keys.ToList();
                for (int i = 0; i < GameGateMap.Count; i++)
                {
                    var gateInfo = GameGateMap[keys[i]].GateInfo;
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
                                if (string.Compare(gateUserInfo.Account, sAccount, StringComparison.OrdinalIgnoreCase) == 0 || (gateUserInfo.SessionID == nSessionID))
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
                                        gateUserInfo.PlayObject.BoEmergencyClose = true;
                                        gateUserInfo.PlayObject.BoSoftClose = true;
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
            var keys = GameGateMap.Keys.ToList();
            for (int i = 0; i < GameGateMap.Count; i++)
            {
                GameGateMap[keys[i]].GateInfo.Socket.Close();
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
                if (!GameGateMap.ContainsKey(e.SocHandle))
                {
                    return;
                }
                if (GameGateMap[e.SocHandle] == null)
                {
                    Debug.WriteLine("非法请求");
                    return;
                }
                var gateInfo = GameGateMap[e.SocHandle].GateInfo;
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
                                gateUser.PlayObject.BoEmergencyClose = true;
                                if (!gateUser.PlayObject.BoReconnection)
                                {
                                    IdSrvClient.Instance.SendHumanLogOutMsg(gateUser.Account, gateUser.SessionID);
                                }
                            }
                            userList[i] = null;
                        }
                    }
                    gateInfo.UserList = null;
                    gateInfo.BoUsed = false;
                    gateInfo.Socket = null;
                    _logger.Error(string.Format(sGateClose, e.EndPoint));
                    if (GameGateMap.Remove(e.SocHandle, out var gameGate))
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
            //todo   
            /*var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new GameServerPacket();
            msgHeader.PacketCode = Grobal2.RUNGATECODE;
            msgHeader.Socket = nSocket;
            msgHeader.SessionId = nGsIdx;
            msgHeader.Ident = Grobal2.GM_DATA;
            msgHeader.PackLength = ClientMesaagePacket.PackSize;
            ClientOutMessage outMessage = new ClientOutMessage(msgHeader, defMsg);
            if (!AddGateBuffer(nGateIdx, outMessage.GetBuffer()))
            {
                M2Share.Log.Error("发送玩家退出消息失败.");
            }*/
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nGateIdx, int nSocket, PlayObject PlayObject)
        {
            if (GameGateMap.TryGetValue(nGateIdx, out var runGate))
            {
                runGate.SetGateUserList(nSocket, PlayObject);
                return;
            }
            _logger.Debug("设置用户对应网关失败,网关ID不存在.");
        }

        public void Run()
        {
            var dwRunTick = HUtil32.GetTickCount();
            if (M2Share.StartReady)
            {
                if (!GameGateMap.IsEmpty)
                {
                    using IEnumerator<KeyValuePair<int, GameGate>> gameGates = GameGateMap.GetEnumerator();
                    while (gameGates.MoveNext())
                    {
                        var gameGate = gameGates.Current.Value;
                        var gateInfo = gameGate.GateInfo;
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
            var defMsg = new ClientCommandPacket();
            var msgHdr = new ServerMessagePacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = Grobal2.GM_TEST,
                PackLength = 100
            };
            /*var nLen = msgHdr.PackLength + GameServerPacket.PacketSize;
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(ServerPackSerializer.MemoryPackBrotli(msgHdr));
            backingStream.Write(defMsg.GetBuffer());
            memoryStream.Seek(0, SeekOrigin.Begin);
            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            if (!M2Share.GateMgr.AddGateBuffer(nIndex, data))
            {
                data = null;
            }*/
        }

        private void SendCheck(Socket Socket, int nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var msgHeader = new ServerMessagePacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = (ushort)nIdent,
                PackLength = 0
            };
            if (Socket.Connected)
            {
                var data = ServerPackSerializer.Serialize(msgHeader);
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr->GameGate
        /// </summary>
        /// <returns></returns>
        public bool AddGateBuffer(int gateIdx, int len, byte[] buffer, byte[] msgBuffer)
        {
            if (GameGateMap.TryGetValue(gateIdx, out GameGate value))
            {
                value.HandleSendBuffer(len, buffer, msgBuffer);
                return true;
            }
            _logger.Error("发送网关消息失败，用户对应网关ID不存在.");
            return false;
        }

        /// <summary>
        /// 收到GameGate发来的消息并添加到GameSvr消息队列
        /// </summary>
        public void AddGameGateQueue(int gateIdx, ServerMessagePacket packet, byte[] data)
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
            M2Share.Log.Error(e.Exception.StackTrace);
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
            if (GameGateMap.TryGetValue(e.SocHandle, out var runGate))
            {
                var nMsgLen = e.BytesReceived;
                if (nMsgLen <= 0)
                {
                    return;
                }
                Span<byte> data = stackalloc byte[e.BytesReceived];
                MemoryCopy.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nMsgLen);
                runGate.ProcessReceiveBuffer(nMsgLen, data);
            }
            else
            {
                M2Share.Log.Error("错误的网关数据");
            }
        }

        #endregion
    }

    public struct ReceiveData
    {
        public ServerMessagePacket Packet;
        public byte[] Data;
        public int GateId;
    }
}