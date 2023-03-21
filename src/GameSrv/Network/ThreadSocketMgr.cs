using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
using GameSrv.Player;
using GameSrv.Services;
using NLog;
using SystemModule.Common;
using SystemModule.Enums;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace GameSrv.Network
{
    public class ThreadSocketMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SocketServer _gateSocket;
        private readonly object runSocketSection;
        private readonly Channel<ReceiveData> _receiveQueue;//todo 一个网关一个队列
        private readonly ThreadSocket[] GameGates;
        private static int CurrentGateIdx = 0;
        private readonly HashSet<long> RunGatePermitMap = new HashSet<long>();
        private CancellationToken stoppingCancelReads;
        /// <summary>
        /// 数据接收缓冲区
        /// </summary>
        public byte[] ReceiveBuffer;
        public int ReceiveLen;

        public ThreadSocketMgr()
        {
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            GameGates = new ThreadSocket[20];
            _gateSocket = new SocketServer(100, 1024);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            runSocketSection = new object();
            stoppingCancelReads = new CancellationToken();
            ReceiveBuffer = new byte[4096];
        }

        public void Initialize()
        {
            _gateSocket.Init(50);
            _logger.Info("游戏网关初始化完成...");
        }

        public void Start()
        {
            _gateSocket.Start(M2Share.Config.sGateAddr, M2Share.Config.nGatePort);
            _logger.Info($"游戏网关[{M2Share.Config.sGateAddr}:{M2Share.Config.nGatePort}]已启动...");
        }

        public async Task StopAsync()
        {
            if (_receiveQueue.Reader.Count > 0)
                await _receiveQueue.Reader.Completion;
            stoppingCancelReads = new CancellationToken(true);
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
                for (var i = 0; i < runAddrList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(runAddrList[i]))
                    {
                        RunGatePermitMap.Add(HUtil32.IpToInt(runAddrList[i]));
                    }
                }
                var localEntity = Dns.GetHostEntry(Dns.GetHostName());
                for (var i = 0; i < localEntity.AddressList.Length; i++)
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
                var gateInfo = new ThreadGateInfo();
                gateInfo.nSendMsgCount = 0;
                gateInfo.nSendRemainCount = 0;
                gateInfo.dwSendTick = HUtil32.GetTickCount();
                gateInfo.nSendMsgBytes = 0;
                gateInfo.nSendedMsgCount = 0;
                gateInfo.BoUsed = true;
                gateInfo.SocketId = e.ConnectionId;
                gateInfo.Socket = e.Socket;
                gateInfo.UserList = new List<SessionUser>();
                gateInfo.nUserCount = 0;
                gateInfo.boSendKeepAlive = false;
                gateInfo.nSendChecked = 0;
                gateInfo.nSendBlockCount = 0;
                GameGates[CurrentGateIdx] = new ThreadSocket(gateInfo);
                Interlocked.Increment(ref CurrentGateIdx);
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

        public void KickUser(string sAccount, int sessionId, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            const string accountExpiredMsg = "账号付费时间已到,本机已被强行离线,请充值后再继续进行游戏!";
            try
            {
                for (var i = 0; i < GameGates.Length; i++)
                {
                    if (GameGates[i] == null)
                    {
                        continue;
                    }
                    var gateInfo = GameGates[i].GateInfo;
                    if (gateInfo.BoUsed && gateInfo.Socket != null && gateInfo.UserList != null)
                    {
                        HUtil32.EnterCriticalSection(runSocketSection);
                        try
                        {
                            for (var j = 0; j < gateInfo.UserList.Count; j++)
                            {
                                var gateUserInfo = gateInfo.UserList[j];
                                if (gateUserInfo == null)
                                {
                                    continue;
                                }
                                if (string.Compare(gateUserInfo.Account, sAccount, StringComparison.OrdinalIgnoreCase) == 0 || (gateUserInfo.SessionID == sessionId))
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
                                            gateUserInfo.PlayObject.SysMsg(accountExpiredMsg, MsgColor.Red, MsgType.Hint);
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
                            HUtil32.LeaveCriticalSection(runSocketSection);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
        }

        public void CloseAllGate()
        {
            for (var i = 0; i < GameGates.Length; i++)
            {
                GameGates[i].GateInfo.Socket.Close();
            }
        }

        public static void CloseErrGate(Socket Socket)
        {
            if (Socket.Connected)
            {
                Socket.Close();
            }
        }

        private void CloseGate(string connectionId, string endPoint)
        {
            const string sGateClose = "游戏网关[{0}]已关闭...";
            HUtil32.EnterCriticalSection(runSocketSection);
            try
            {
                for (int i = 0; i < GameGates.Length; i++)
                {
                    if (GameGates[i] == null)
                    {
                        continue;
                    }
                    if (GameGates[i].ConnectionId == connectionId)
                    {
                        var gateInfo = GameGates[i].GateInfo;
                        if (gateInfo.Socket == null)
                        {
                            _logger.Error("Scoket异常，无需关闭");
                            return;
                        }
                        for (var j = 0; j < gateInfo.UserList.Count; j++)
                        {
                            var gateUser = gateInfo.UserList[j];
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
                                gateInfo.UserList[j] = null;
                            }
                        }
                        gateInfo.UserList = null;
                        gateInfo.BoUsed = false;
                        gateInfo.Socket = null;
                        GameGates[i].Stop();
                        _logger.Error(string.Format(sGateClose, endPoint));
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(runSocketSection);
            }
        }

        public void SendServerStopMsg()
        {
            if (GameGates.Length > 0)
            {
                for (var i = 0; i < GameGates.Length; i++) //循环网关发送消息游戏引擎停止服务消息
                {
                    if (GameGates[i] == null)
                    {
                        continue;
                    }
                    var gateInfo = GameGates[i].GateInfo;
                    if (gateInfo.Socket != null && gateInfo.Socket.Connected)
                    {
                        GameGates[i].SendCheck(Grobal2.GM_STOP);
                    }
                }
            }
        }

        public void SendOutConnectMsg(int gateIdx, int nSocket, ushort nGsIdx)
        {
            var defMsg = Messages.MakeMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new ServerMessage();
            msgHeader.PacketCode = Grobal2.RunGateCode;
            msgHeader.Socket = nSocket;
            msgHeader.SessionId = nGsIdx;
            msgHeader.Ident = Grobal2.GM_DATA;
            msgHeader.PackLength = CommandMessage.Size;
            ClientOutMessage outMessage = new ClientOutMessage();
            outMessage.MessagePacket = msgHeader;
            outMessage.CommandPacket = defMsg;
            AddGateBuffer(gateIdx, SerializerUtil.Serialize(outMessage));
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int gateIdx, int nSocket, PlayObject playObject)
        {
            GameGates[gateIdx].SetGateUserList(nSocket, playObject);
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
        }

        private static void SendGateTestMsg(int nIndex)
        {
            var defMsg = new CommandMessage();
            var msgHdr = new ServerMessage
            {
                PacketCode = Grobal2.RunGateCode,
                Socket = 0,
                Ident = Messages.GM_TEST,
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
        /// GameSrv->GameGate
        /// </summary>
        /// <returns></returns>
        public void AddGateBuffer(int gateIdx, byte[] senData)
        {
            GameGates[gateIdx].ProcessBufferSend(senData);
        }

        internal void Send(string connectId, byte[] buff)
        {
            _gateSocket.Send(connectId, buff);
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
        public Task StartMessageThread()
        {
            return Task.Factory.StartNew(async () =>
             {
                 while (await _receiveQueue.Reader.WaitToReadAsync(stoppingCancelReads))
                 {
                     while (_receiveQueue.Reader.TryRead(out var message))
                     {
                         // ExecGateBuffers(message.Packet, message.Data);
                     }
                 }
             }, stoppingCancelReads);
        }

        #region Socket Events

        private void GateSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            //M2Share.RunSocket.CloseErrGate();
            M2Share.Logger.Error(e.Exception.StackTrace);
        }

        private void GateSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            M2Share.SocketMgr.CloseGate(e.ConnectionId, e.RemoteIPaddr);
            Interlocked.Decrement(ref CurrentGateIdx);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            if (RunGatePermitMap.Contains(HUtil32.IpToInt(e.RemoteIPaddr)))
            {
                M2Share.SocketMgr.AddGate(e);
            }
            else
            {
                e.Socket.Close();
                _logger.Warn($"关闭非白名单网关地址链接. IP:{e.RemoteIPaddr}");
            }
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            const string sExceptionMsg = "[Exception] GameGate::ProcessReceiveBuffer";
            var nLen = e.BytesReceived;
            if (nLen <= 0)
            {
                return;
            }
            var processLen = 0;
            MemoryCopy.BlockCopy(e.ReceiveBuffer, e.Offset, ReceiveBuffer, ReceiveLen, nLen);
            Span<byte> processBuff = ReceiveBuffer;
            try
            {
                while (nLen >= DataPacketMessage.PacketSize)
                {
                    var packetHeader = SerializerUtil.Deserialize<DataPacketMessage>(processBuff.Slice(processLen, DataPacketMessage.PacketSize));
                    if (packetHeader.PacketCode == Grobal2.RunGateCode)
                    {
                        var nCheckMsgLen = Math.Abs(packetHeader.PackLength) + DataPacketMessage.PacketSize;
                        if (nLen < nCheckMsgLen && nCheckMsgLen < 0x8000)
                        {
                            _logger.Warn("丢弃网关长度数据包.");
                            break;
                        }
                        if (packetHeader.PackLength > 0)
                        {
                            Span<byte> body = processBuff.Slice(DataPacketMessage.PacketSize, packetHeader.PackLength);
                            GameGates[packetHeader.GateIdx].ExecGateBuffers(packetHeader, packetHeader.PackLength, body);
                        }
                        else
                        {
                            GameGates[packetHeader.GateIdx].ExecGateBuffers(packetHeader, packetHeader.PackLength, null);
                        }
                        nLen -= nCheckMsgLen;
                        processLen += nCheckMsgLen;
                        if (nLen <= 0)
                        {
                            break;
                        }
                        ReceiveLen = nLen;
                    }
                    else
                    {
                        nLen -= 1;
                        _logger.Warn("丢弃整段网关异常数据包");
                    }
                    if (nLen < ServerMessage.PacketSize)
                    {
                        break;
                    }
                }
                if (nLen > 0)
                {
                    MemoryCopy.BlockCopy(processBuff, 0, ReceiveBuffer, 0, nLen);
                    ReceiveLen = nLen;
                }
                else
                {
                    ReceiveLen = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }

            /*for (int i = 0; i < GameGates.Length; i++)
            {
                if (GameGates[i].ConnectionId == e.ConnectionId)
                {
                    var gameGate = GameGates[i];
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
            }*/
            M2Share.NetworkMonitor.Receive(nLen);
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