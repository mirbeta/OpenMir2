using GameSvr.Player;
using GameSvr.Services;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Channels;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace GameSvr.GateWay
{
    public class GameGateMgr
    {
        private static readonly GameGateMgr instance = new GameGateMgr();
        public static GameGateMgr Instance => instance;
        private readonly SocketServer _gateSocket = null;
        private readonly object m_RunSocketSection = null;
        private readonly Channel<ReceiveData> _receiveQueue;
        private readonly ConcurrentDictionary<int, GameGate> _gameGates;

        private GameGateMgr()
        {
            LoadRunAddr();
            _receiveQueue = Channel.CreateUnbounded<ReceiveData>();
            _gameGates = new ConcurrentDictionary<int, GameGate>();
            _gateSocket = new SocketServer(10, 512);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            m_RunSocketSection = new object();
        }

        public void Start(CancellationToken stoppingToken)
        {
            _gateSocket.Init();
            _gateSocket.Start(M2Share.g_Config.sGateAddr, M2Share.g_Config.nGatePort);
            StartMessageThread(stoppingToken);
            M2Share.MainOutMessage($"游戏网关[{M2Share.g_Config.sGateAddr}:{M2Share.g_Config.nGatePort}]已启动...");
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
                if (_gameGates.Count > 20)
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
                M2Share.MainOutMessage(string.Format(sGateOpen, e.EndPoint));
                _gameGates.TryAdd(e.SocHandle, new GameGate(e.SocHandle, gateInfo));
                _gameGates[e.SocHandle].StartGateQueue();
            }
            else
            {
                M2Share.ErrorMessage(string.Format(sKickGate, e.EndPoint));
                e.Socket.Close();
            }
        }

        public void CloseUser(int gateIdx, int nSocket)
        {
            if (_gameGates.ContainsKey(gateIdx))
            {
                _gameGates[gateIdx].CloseUser(nSocket);
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
                var keys = _gameGates.Keys.ToList();
                for (int i = 0; i < _gameGates.Count; i++)
                {
                    var gateInfo = _gameGates[keys[i]].GateInfo;
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
            var keys = _gameGates.Keys.ToList();
            for (int i = 0; i < _gameGates.Count; i++)
            {
                _gameGates[keys[i]].GateInfo.Socket.Close();
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
            const string sGateClose = "游戏网关({0}:{1})已关闭...";
            HUtil32.EnterCriticalSection(m_RunSocketSection);
            try
            {
                if (_gameGates[e.SocHandle] == null)
                {
                    Debug.WriteLine("非法请求");
                    return;
                }
                var gateInfo = _gameGates[e.SocHandle].GateInfo;
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
                                    IdSrvClient.Instance.SendHumanLogOutMsg(gateUser.sAccount, gateUser.nSessionID);
                                }
                            }
                            userList[i] = null;
                        }
                    }
                    gateInfo.UserList = null;
                    gateInfo.BoUsed = false;
                    gateInfo.Socket = null;
                    M2Share.ErrorMessage(string.Format(sGateClose, e.EndPoint.Address, e.EndPoint.Port));
                    if (_gameGates.Remove(e.SocHandle, out var gameGate))
                    {
                        gameGate.Stop();
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

        public void SendOutConnectMsg(int nGateIdx, int nSocket, ushort nGsIdx)
        {
            var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            var msgHeader = new PacketHeader();
            msgHeader.PacketCode = Grobal2.RUNGATECODE;
            msgHeader.Socket = nSocket;
            msgHeader.SessionId = nGsIdx;
            msgHeader.Ident = Grobal2.GM_DATA;
            msgHeader.PackLength = ClientPacket.PackSize;
            ClientOutMessage outMessage = new ClientOutMessage(msgHeader, defMsg);
            if (!AddGateBuffer(nGateIdx, outMessage.GetBuffer()))
            {
                Console.WriteLine("发送玩家退出消息失败.");
            }
        }

        /// <summary>
        /// 设置用户对应网关编号
        /// </summary>
        public void SetGateUserList(int nGateIdx, int nSocket, PlayObject PlayObject)
        {
            if (!_gameGates.ContainsKey(nGateIdx))
            {
                return;
            }
            var gameGate = _gameGates[nGateIdx];
            gameGate.SetGateUserList(nSocket, PlayObject);
        }

        public void Run()
        {
            var dwRunTick = HUtil32.GetTickCount();
            if (M2Share.boStartReady)
            {
                if (_gameGates.Count > 0)
                {
                    var gateServiceList = _gameGates.Values.ToList();
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
            var defMsg = new ClientPacket();
            var msgHdr = new PacketHeader
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = 0,
                Ident = Grobal2.GM_TEST,
                PackLength = 100
            };
            var nLen = msgHdr.PackLength + PacketHeader.PacketSize;
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(nLen);
            backingStream.Write(msgHdr.GetBuffer());
            backingStream.Write(defMsg.GetBuffer());
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
            var msgHeader = new PacketHeader
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
            if (_gameGates.ContainsKey(gateIdx))
            {
                _gameGates[gateIdx].HandleSendBuffer(buffer);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 收到GameGate发来的消息并添加到GameSvr消息队列
        /// </summary>
        public void AddGameGateQueue(int gateIdx, PacketHeader packet, byte[] data)
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
            }, cancellation);
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
            if (_gameGates.ContainsKey(e.SocHandle))
            {
                var nMsgLen = e.BytesReceived;
                var data = new byte[e.BytesReceived];
                Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nMsgLen);
                _gameGates[e.SocHandle].HandleReceiveBuffer(nMsgLen, data);
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
        public PacketHeader Packet;
        public byte[] Data;
        public int GateId;
    }
}