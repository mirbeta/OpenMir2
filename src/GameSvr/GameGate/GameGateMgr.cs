using GameSvr.Player;
using GameSvr.Services;
using NLog;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.MemoryPool;
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
        private readonly GameGate[] GameGates;
        private static int CurrentGateIdx = 0;
        private readonly Dictionary<int, int> GameGateLinkMap = new Dictionary<int, int>();
        private readonly HashSet<long> RunGatePermitMap = new HashSet<long>();

        public GameGateMgr()
        {
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            GameGates = new GameGate[20];
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
            var sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "!Runaddr.txt");
            if (File.Exists(sFileName))
            {
                var runAddrList = new StringList();
                runAddrList.LoadFromFile(sFileName);
                M2Share.TrimStringList(runAddrList);
                for (int i = 0; i < runAddrList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(runAddrList[i]))
                    {
                        RunGatePermitMap.Add(HUtil32.IpToInt(runAddrList[i]));
                    }
                }
                var localEntity = Dns.GetHostEntry(Dns.GetHostName());
                for (int i = 0; i < localEntity.AddressList.Length; i++)
                {
                    if (localEntity.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        RunGatePermitMap.Add(HUtil32.IpToInt(localEntity.AddressList[i].ToString()));
                    }
                }
            }
        }

        private void AddGate(AsyncUserToken e)
        {
            const string sGateOpen = "游戏网关[{0}]已打开...";
            const string sKickGate = "服务器未就绪: {0}";
            if (M2Share.StartReady)
            {
                if (GameGates.Length > 20)
                {
                    e.Socket.Close();
                    _logger.Error("超过网关最大链接数量.关闭链接");
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
                GameGates[CurrentGateIdx] = new GameGate(CurrentGateIdx, gateInfo);
                GameGateLinkMap.Add(e.SocHandle, CurrentGateIdx);
                CurrentGateIdx++;
                _logger.Info(string.Format(sGateOpen, e.EndPoint));
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
            GameGates[gateIdx].CloseUser(nSocket);
        }

        public void KickUser(string sAccount, int nSessionID, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            try
            {
                for (int i = 0; i < GameGates.Length; i++)
                {
                    if (GameGates[i] == null)
                    {
                        continue;
                    }
                    var gateInfo = GameGates[i].GateInfo;
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
            for (int i = 0; i < GameGates.Length; i++)
            {
                GameGates[i].GateInfo.Socket.Close();
            }
        }

        public void CloseErrGate(Socket Socket)
        {
            if (Socket.Connected)
            {
                Socket.Close();
            }
        }

        private void CloseGate(int gateId, string connectionId,string endPoint)
        {
            const string sGateClose = "游戏网关[{0}]已关闭...";
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                var gameGate = GameGates[gateId];
                var gateInfo = gameGate.GateInfo;
                if (gateInfo.Socket == null)
                {
                    _logger.Warn("Scoket为空，无需关闭");
                    return;
                }
                if (gateInfo.SocketId.Equals(connectionId))
                {
                    for (var i = 0; i < gateInfo.UserList.Count; i++)
                    {
                        var gateUser = gateInfo.UserList[i];
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
                            gateInfo.UserList[i] = null;
                        }
                    }
                    gateInfo.UserList = null;
                    gateInfo.BoUsed = false;
                    gateInfo.Socket = null;
                    gameGate.Stop();
                    _logger.Error(string.Format(sGateClose, endPoint));
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
            //var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            //var msgHeader = new ServerMessagePacket();
            //msgHeader.PacketCode = Grobal2.RUNGATECODE;
            //msgHeader.Socket = nSocket;
            //msgHeader.SessionId = nGsIdx;
            //msgHeader.Ident = Grobal2.GM_DATA;
            //msgHeader.PackLength = ClientCommandPacket.PackSize;
            //ClientOutMessage outMessage = new ClientOutMessage();
            //outMessage.MessagePacket = msgHeader;
            //outMessage.CommandPacket = defMsg;
            //if (!AddGateBuffer(nGateIdx, 0, ServerPackSerializer.Serialize(outMessage), null))
            //{
            //    M2Share.Log.Error("发送玩家退出消息失败.");
            //}
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nGateIdx, int nSocket, PlayObject PlayObject)
        {
            GameGates[nGateIdx].SetGateUserList(nSocket, PlayObject);
        }

        public void Run()
        {
            var dwRunTick = HUtil32.GetTickCount();
            if (M2Share.StartReady)
            {
                if (GameGates.Length > 0)
                {
                    for (var i = 0; i < GameGates.Length; i++)
                    {
                        if (GameGates[i] == null)
                        {
                            continue;
                        }
                        var gateInfo = GameGates[i].GateInfo;
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
                                GameGates[i].SendCheck(Grobal2.GM_CHECKSERVER);
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
            var defMsg = new CommandPacket();
            var msgHdr = new ServerMessage
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = Grobal2.GM_TEST,
                PackLength = 100
            };
            var nLen = msgHdr.PackLength + ServerMessage.PacketSize;
            var clientOutMessage = new ClientOutMessage();
            clientOutMessage.CommandPacket = defMsg;
            clientOutMessage.MessagePacket = msgHdr;
            //M2Share.GateMgr.AddGateBuffer(nIndex, ServerPackSerializer.Serialize(clientOutMessage));
        }

        /// <summary>
        /// 添加到网关发送队列
        /// GameSvr->GameGate
        /// </summary>
        /// <returns></returns>
        public void AddGateBuffer(int gateIdx, byte[] senData)
        {
            GameGates[gateIdx].ProcessBufferSend(senData);
        }

        internal void Send(string connectId, byte[] buff)
        {
            _gateSocket.Send(connectId, buff);
            //((FixedLengthOwner<byte>)buff).Dispose();
        }

        /// <summary>
        /// 收到GameGate发来的消息并添加到GameSvr消息队列
        /// </summary>
        public void AddGameGateQueue(int gateIdx, ServerMessage packet, byte[] data)
        {
            _receiveQueue.Writer.TryWrite(new ReceiveData()
            {
                GateId = gateIdx,
                Packet = packet,
                Data = data
            });
        }

        /// <summary>
        /// 处理GameGate消息
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
            var gateId = GameGateLinkMap[e.SocHandle];
            M2Share.GateMgr.CloseGate(gateId, e.ConnectionId, e.RemoteIPaddr);
            CurrentGateIdx--;
            GameGateLinkMap.Remove(e.SocHandle);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            if (RunGatePermitMap.Contains(HUtil32.IpToInt(e.RemoteIPaddr)))
            {
                M2Share.GateMgr.AddGate(e);
            }
            else
            {
                e.Socket.Close();
                _logger.Warn($"关闭非白名单网关地址链接. IP:{e.RemoteIPaddr}");
            }
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            var nMsgLen = e.BytesReceived;
            if (nMsgLen <= 0)
            {
                return;
            }
            var gateId = GameGateLinkMap[e.SocHandle];
            var gameGate = GameGates[gateId];
            if (gameGate.ReceiveLen > 0)
            {
                MemoryCopy.BlockCopy(e.ReceiveBuffer, e.Offset, gameGate.ReceiveBuffer, gameGate.ReceiveLen, nMsgLen);
                gameGate.ProcessBufferReceive(gameGate.ReceiveBuffer, gameGate.ReceiveLen + nMsgLen);
            }
            else
            {
                MemoryCopy.BlockCopy(e.ReceiveBuffer, e.Offset, gameGate.ReceiveBuffer, 0, nMsgLen);
                gameGate.ProcessBufferReceive(gameGate.ReceiveBuffer, nMsgLen);
            }
        }

        #endregion
    }

    public struct ReceiveData
    {
        public ServerMessage Packet;
        public byte[] Data;
        public int GateId;
    }
}