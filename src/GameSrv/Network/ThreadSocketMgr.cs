using GameSrv.Network.DataHandlingAdapters;
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
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace GameSrv.Network
{
    public class ThreadSocketMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpService tcpService;
        private readonly object _runSocketSection;
        private readonly Channel<ReceiveData> _receiveQueue;//todo 一个网关一个队列
        private readonly ThreadSocket[] _gameGates;
        private static int _currentGateIdx = 0;
        private readonly HashSet<long> _gatePermitMap = new HashSet<long>();
        private CancellationToken _stoppingCancelReads;
        /// <summary>
        /// 数据接收缓冲区
        /// </summary>
        private byte[] ReceiveBuffer;
        private int ReceiveLen;

        public ThreadSocketMgr()
        {
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            _gameGates = new ThreadSocket[20];
            tcpService = new TcpService();
            tcpService.Connected += Connecting;
            tcpService.Disconnected += Disconnected;
            tcpService.Received += Received;
            _runSocketSection = new object();
            _stoppingCancelReads = new CancellationToken();
            ReceiveBuffer = new byte[4096 * 10];
        }

        private void Received(object sender, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (requestInfo is not DataMessageFixedHeaderRequestInfo fixedHeader)
                return;
            var client = (SocketClient)sender;
            if (int.TryParse(client.ID, out var clientId))
            {
                _gameGates[clientId - 1].ProcessBuffer(fixedHeader.Header, fixedHeader.Message);
            }
            else
            {
                _logger.Info("未知客户端...");
            }
        }

        private void Connecting(object sender, TouchSocketEventArgs e)
        {
            var client = (SocketClient)sender;
            if (_gatePermitMap.Contains(HUtil32.IpToInt(client.ServiceIP)))
            {
                M2Share.SocketMgr.AddGate(client);
            }
            else
            {
                client.Close();
                _logger.Warn($"关闭非白名单网关地址链接. IP:{client.MainSocket.RemoteEndPoint}");
            }
        }

        public void Disconnected(object sender, DisconnectEventArgs e)
        {
            var client = (SocketClient)sender;
            M2Share.SocketMgr.CloseGate(client.ID, client.ServiceIP);
            Interlocked.Decrement(ref _currentGateIdx);
        }

        public void Initialize()
        {
            _logger.Info("游戏网关初始化完成...");
        }

        public void Start()
        {
            TouchSocketConfig touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts(new IPHost[1]
            {
                new IPHost(IPAddress.Parse(M2Share.Config.sGateAddr), M2Share.Config.nGatePort)
            }).SetDataHandlingAdapter(() => new MyFixedHeaderCustomDataHandlingAdapter());
            tcpService.Setup(touchSocketConfig).Start();
            _logger.Info($"游戏网关[{M2Share.Config.sGateAddr}:{M2Share.Config.nGatePort}]已启动...");
        }

        public async Task StopAsync()
        {
            if (_receiveQueue.Reader.Count > 0)
                await _receiveQueue.Reader.Completion;
            _stoppingCancelReads = new CancellationToken(true);
            tcpService.Stop();
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
                        _gatePermitMap.Add(HUtil32.IpToInt(runAddrList[i]));
                    }
                }
                var localEntity = Dns.GetHostEntry(Dns.GetHostName());
                for (var i = 0; i < localEntity.AddressList.Length; i++)
                {
                    if (localEntity.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        _gatePermitMap.Add(HUtil32.IpToInt(localEntity.AddressList[i].ToString()));
                    }
                }
            }
        }

        private void AddGate(SocketClient e)
        {
            const string sGateOpen = "游戏网关[{0}]已打开...";
            const string sKickGate = "服务器未就绪: {0}";
            if (M2Share.StartReady)
            {
                if (_gameGates.Length > 20)
                {
                    e.Close();
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
                gateInfo.SocketId = e.ID;
                gateInfo.Socket = e.MainSocket;
                gateInfo.UserList = new List<SessionUser>();
                gateInfo.nUserCount = 0;
                gateInfo.boSendKeepAlive = false;
                gateInfo.nSendChecked = 0;
                gateInfo.nSendBlockCount = 0;
                _gameGates[_currentGateIdx] = new ThreadSocket(gateInfo);
                Interlocked.Increment(ref _currentGateIdx);
                _logger.Info(string.Format(sGateOpen, e.MainSocket.RemoteEndPoint));
            }
            else
            {
                _logger.Error(string.Format(sKickGate, e.MainSocket.RemoteEndPoint));
                e.Close();
            }
        }

        public void CloseUser(int gateIdx, int nSocket)
        {
            _gameGates[gateIdx].CloseUser(nSocket);
        }

        public void KickUser(string sAccount, int sessionId, int payMode)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::KickUser";
            const string sKickUserMsg = "当前登录帐号正在其它位置登录，本机已被强行离线!!!";
            const string accountExpiredMsg = "账号付费时间已到,本机已被强行离线,请充值后再继续进行游戏!";
            try
            {
                for (var i = 0; i < _gameGates.Length; i++)
                {
                    if (_gameGates[i] == null)
                    {
                        continue;
                    }
                    var gateInfo = _gameGates[i].GateInfo;
                    if (gateInfo.BoUsed && gateInfo.Socket != null && gateInfo.UserList != null)
                    {
                        HUtil32.EnterCriticalSection(_runSocketSection);
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
                            HUtil32.LeaveCriticalSection(_runSocketSection);
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
            for (var i = 0; i < _gameGates.Length; i++)
            {
                _gameGates[i].GateInfo.Socket.Close();
            }
        }

        public static void CloseErrGate(Socket socket)
        {
            if (socket.Connected)
            {
                socket.Close();
            }
        }

        private void CloseGate(string connectionId, string endPoint)
        {
            const string sGateClose = "游戏网关[{0}]已关闭...";
            HUtil32.EnterCriticalSection(_runSocketSection);
            try
            {
                for (int i = 0; i < _gameGates.Length; i++)
                {
                    if (_gameGates[i] == null)
                    {
                        continue;
                    }
                    if (_gameGates[i].ConnectionId == connectionId)
                    {
                        var gateInfo = _gameGates[i].GateInfo;
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
                        _gameGates[i].Stop();
                        _logger.Error(string.Format(sGateClose, endPoint));
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(_runSocketSection);
            }
        }

        public void SendServerStopMsg()
        {
            if (_gameGates.Length > 0)
            {
                for (var i = 0; i < _gameGates.Length; i++) //循环网关发送消息游戏引擎停止服务消息
                {
                    if (_gameGates[i] == null)
                    {
                        continue;
                    }
                    var gateInfo = _gameGates[i].GateInfo;
                    if (gateInfo.Socket != null && gateInfo.Socket.Connected)
                    {
                        _gameGates[i].SendCheck(Grobal2.GM_STOP);
                    }
                }
            }
        }

        public void SendOutConnectMsg(int gateIdx, int nSocket, ushort nGsIdx)
        {
            var defMsg = Messages.MakeMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new ServerMessage();
            msgHeader.PacketCode = Grobal2.PacketCode;
            msgHeader.Socket = nSocket;
            msgHeader.SessionId = nGsIdx;
            msgHeader.Ident = Grobal2.GM_DATA;
            msgHeader.PackLength = CommandMessage.Size;
            ClientOutMessage outMessage = new ClientOutMessage();
            outMessage.MessagePacket = msgHeader;
            outMessage.CommandPacket = defMsg;
            _logger.Info("待实现");
            //AddGateBuffer(gateIdx, SerializerUtil.Serialize(outMessage));
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int gateIdx, int nSocket, PlayObject playObject)
        {
            _gameGates[gateIdx].SetGateUserList(nSocket, playObject);
        }

        public void Run()
        {
            if (M2Share.StartReady)
            {
                if (_gameGates.Length > 0)
                {
                    for (var i = 0; i < _gameGates.Length; i++)
                    {
                        if (_gameGates[i] == null)
                        {
                            continue;
                        }
                        var gateInfo = _gameGates[i].GateInfo;
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
                                _gameGates[i].SendCheck(Grobal2.GM_CHECKSERVER);
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
                PacketCode = Grobal2.PacketCode,
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
            _gameGates[gateIdx].ProcessBufferSend(senData);
        }

        internal void Send(string connectId, byte[] buff)
        {
            tcpService.Send(connectId, buff);
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
                 while (await _receiveQueue.Reader.WaitToReadAsync(_stoppingCancelReads))
                 {
                     while (_receiveQueue.Reader.TryRead(out var message))
                     {
                         // ExecGateBuffers(message.Packet, message.Data);
                     }
                 }
             }, _stoppingCancelReads);
        }
    }

    public struct ReceiveData
    {
        public ServerMessage Packet;
        public byte[] Data;
        public int GateId;
    }
}