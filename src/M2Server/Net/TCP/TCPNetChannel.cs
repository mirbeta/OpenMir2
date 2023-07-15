using NLog;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
using SystemModule;
using SystemModule.ByteManager;
using SystemModule.Common;
using SystemModule.Core.Config;
using SystemModule.DataHandlingAdapters;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets.Common;
using SystemModule.Sockets.Components.TCP;
using SystemModule.Sockets.Config;
using SystemModule.Sockets.Interface;
using SystemModule.Sockets.SocketEventArgs;

namespace M2Server.Net.TCP
{
    public class TCPNetChannel : INetChannel
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpService tcpService;
        private readonly object RunSocketSection;
        private readonly Channel<ReceiveData> _receiveQueue;
        private readonly ChannelMessageHandler[] _gameGates;
        /// <summary>
        /// 网关地址白名单
        /// </summary>
        private readonly HashSet<long> Whitelist = new HashSet<long>();
        private CancellationToken _stoppingCancelReads;

        public TCPNetChannel()
        {
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            _gameGates = new ChannelMessageHandler[20];
            tcpService = new TcpService();
            tcpService.Connected += Connecting;
            tcpService.Disconnected += Disconnected;
            tcpService.Received += Received;
            RunSocketSection = new object();
            _stoppingCancelReads = new CancellationToken();
        }

        private void Received(object sender, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (requestInfo is not DataMessageFixedHeaderRequestInfo gateMessage)
                return;
            var client = (SocketClient)sender;
            var gateId = int.Parse(client.ID) - 1;
            _receiveQueue.Writer.TryWrite(new ReceiveData()
            {
                Data = gateMessage.Message,
                Packet = gateMessage.Header,
                GateId = gateId
            });
        }

        private void Connecting(object sender, TouchSocketEventArgs e)
        {
            const string sKickGate = "服务器未就绪: {0}";
            const string sGateOpen = "游戏网关[{0}]已打开...";
            var client = (SocketClient)sender;
            if (M2Share.StartReady)
            {
                if (_gameGates.Length > 20)
                {
                    client.Close();
                    _logger.Error("超过网关最大链接数量.关闭链接");
                    return;
                }
                if (Whitelist.Contains(HUtil32.IpToInt(client.ServiceIP)))
                {
                    var gateInfo = new ChannelGate();
                    gateInfo.SendMsgCount = 0;
                    gateInfo.SendRemainCount = 0;
                    gateInfo.SendTick = HUtil32.GetTickCount();
                    gateInfo.SendMsgBytes = 0;
                    gateInfo.SendedMsgCount = 0;
                    gateInfo.BoUsed = true;
                    gateInfo.SocketId = client.ID;
                    gateInfo.Socket = client.MainSocket;
                    gateInfo.UserList = new List<SessionUser>();
                    gateInfo.UserCount = 0;
                    gateInfo.SendKeepAlive = false;
                    gateInfo.SendChecked = 0;
                    gateInfo.SendBlockCount = 0;
                    _gameGates[int.Parse(client.ID) - 1] = new ChannelMessageHandler(gateInfo);
                    _logger.Info(string.Format(sGateOpen, client.MainSocket.RemoteEndPoint));
                }
                else
                {
                    client.Close();
                    _logger.Warn($"关闭非白名单网关地址链接. IP:{client.MainSocket.RemoteEndPoint}");
                }
            }
            else
            {
                _logger.Error(string.Format(sKickGate, client.MainSocket.RemoteEndPoint));
                client.Close();
            }
        }

        private void Disconnected(object sender, DisconnectEventArgs e)
        {
            var client = (SocketClient)sender;
            M2Share.NetChannel.CloseGate(client.ID, client.ServiceIP);
            tcpService.NextId();
        }

        public void Initialize()
        {
            var touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts(new IPHost[1]
            {
                new IPHost(IPAddress.Parse(SystemShare.Config.sGateAddr), SystemShare.Config.nGatePort)
            }).SetDataHandlingAdapter(() => new PacketFixedHeaderDataHandlingAdapter());
            tcpService.Setup(touchSocketConfig);
            _logger.Info("游戏网关初始化完成...");
        }

        public Task Start(CancellationToken cancellationToken = default)
        {
            tcpService.Start();
            _logger.Info($"游戏网关[{SystemShare.Config.sGateAddr}:{SystemShare.Config.nGatePort}]已启动...");
            return StartMessageThread(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_receiveQueue.Reader.Count > 0)
                await _receiveQueue.Reader.Completion;
            _stoppingCancelReads = new CancellationToken(true);
            tcpService.Stop();
        }

        private void LoadRunAddr()
        {
            var sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "!runaddr.txt");
            if (File.Exists(sFileName))
            {
                var runAddrList = new StringList();
                runAddrList.LoadFromFile(sFileName);
                M2Share.TrimStringList(runAddrList);
                for (var i = 0; i < runAddrList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(runAddrList[i]))
                    {
                        Whitelist.Add(HUtil32.IpToInt(runAddrList[i]));
                    }
                }
                var localEntity = Dns.GetHostEntry(Dns.GetHostName());
                for (var i = 0; i < localEntity.AddressList.Length; i++)
                {
                    if (localEntity.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        Whitelist.Add(HUtil32.IpToInt(localEntity.AddressList[i].ToString()));
                    }
                }
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
                        HUtil32.EnterCriticalSection(RunSocketSection);
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
                                        gateUserInfo.FrontEngine.DeleteHuman(i, gateUserInfo.Socket);
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
                                    gateInfo.UserCount -= 1;
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            HUtil32.LeaveCriticalSection(RunSocketSection);
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

        public void CloseGate(string connectionId, string endPoint)
        {
            const string sGateClose = "游戏网关[{0}]已关闭...";
            HUtil32.EnterCriticalSection(RunSocketSection);
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
                            _logger.Error("Socket异常，无需关闭");
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
                                        M2Share.AccountSession.SendHumanLogOutMsg(gateUser.Account, gateUser.SessionID);
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
                HUtil32.LeaveCriticalSection(RunSocketSection);
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
        public void SetGateUserList(int gateIdx, int nSocket, IPlayerActor playObject)
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
                            if (HUtil32.GetTickCount() - gateInfo.SendTick >= 1000)
                            {
                                gateInfo.SendTick = HUtil32.GetTickCount();
                                gateInfo.SendMsgBytes = gateInfo.SendBytesCount;
                                gateInfo.SendedMsgCount = gateInfo.SendCount;
                                gateInfo.SendBytesCount = 0;
                                gateInfo.SendCount = 0;
                            }
                            if (gateInfo.SendKeepAlive)
                            {
                                gateInfo.SendKeepAlive = false;
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
            M2Share.NetChannel.AddGateBuffer(nIndex, SerializerUtil.Serialize(clientOutMessage));
        }

        /// <summary>
        /// 添加到网关发送队列
        /// M2Server.>GameGate
        /// </summary>
        /// <returns></returns>
        public void AddGateBuffer(int gateIdx, byte[] senData)
        {
            _gameGates[gateIdx].ProcessBufferSend(senData);
        }

        public void Send(string connectId, byte[] buff)
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
        private Task StartMessageThread(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (await _receiveQueue.Reader.WaitToReadAsync(_stoppingCancelReads))
                {
                    while (_receiveQueue.Reader.TryRead(out var message))
                    {
                        _gameGates[message.GateId].ProcessDataBuffer(message.Packet, message.Data);
                    }
                }
            }, cancellationToken);
        }
    }

    public struct ReceiveData
    {
        public ServerMessage Packet;
        public byte[] Data;
        public int GateId;
    }
}