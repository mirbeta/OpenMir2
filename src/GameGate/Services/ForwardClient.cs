using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace GameGate
{
    /// <summary>
    /// 网关客户端(GameGate-GameSvr)
    /// </summary>
    public class ForwardClient
    {
        private IClientScoket ClientSocket;
        private int nBufferOfM2Size = 0;
        private long dwProcessServerMsgTime = 0;
        /// <summary>
        /// 网关编号（初始化的时候进行分配）
        /// </summary>
        public int GateIdx = 0;
        /// <summary>
        /// 最大用户数
        /// </summary>
        public int MaxSession = 1000;
        /// <summary>
        /// 用户会话
        /// </summary>
        public TSessionInfo[] SessionArray;
        /// <summary>
        /// 心跳线程
        /// </summary>
        private Timer _heartTimer;
        /// <summary>
        ///  网关游戏服务器之间检测是否失败（超时）
        /// </summary>
        private bool boCheckServerFail = false;
        /// <summary>
        /// 独立Buffer分区
        /// </summary>
        private byte[] SocketBuffer = null;
        /// <summary>
        /// 上次剩下多少字节未处理
        /// </summary>
        private int nBuffLen = 0;

        public ForwardClient(string serverAddr, int serverPort)
        {
            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
            ClientSocket.Address = serverAddr;
            ClientSocket.Port = serverPort;
            SessionArray = new TSessionInfo[MaxSession];
        }

        public string GetSocketIp()
        {
            return $"{ClientSocket.Address}:{ClientSocket.Port}";
        }

        public void Start()
        {
            ClientSocket.Connect();
            Task.Factory.StartNew(async () =>
            {
                await ProcessMessage();
            });
            _heartTimer = new Timer(Heart, null, 5000, 10000);
        }

        public void Stop()
        {
            ClientSocket.Disconnect();
        }

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        /// <summary>
        /// 这里有问题 会导致多个监听到一个
        /// </summary>
        private async Task ProcessMessage()
        {
            while (await GateShare.ForwardMsgList.Reader.WaitToReadAsync())
            {
                if (GateShare.ForwardMsgList.Reader.TryRead(out var message))
                {
                    SendServerMsg(message);
                }
            }
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateShare.boGateReady = true;
            GateShare.dwCheckServerTick = HUtil32.GetTickCount();
            GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
            RestSessionArray();
            //GateShare.boServerReady = true;
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.ServerGateList.TryAdd(e.RemoteAddress, this);
            GateShare.AddMainLogMsg($"游戏引擎[{e.RemoteAddress}:{e.RemotePort}]链接成功.", 1);
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            TSessionInfo UserSession;
            for (var i = 0; i < MaxSession; i++)
            {
                UserSession = SessionArray[i];
                if (UserSession.Socket != null)
                {
                    UserSession.Socket.Close();
                    UserSession.Socket = null;
                    UserSession.nSckHandle = -1;
                }
            }
            RestSessionArray();
            SocketBuffer = null;
            GateShare.boGateReady = false;
            GateShare.ServerGateList.TryRemove(e.RemoteAddress, out var userClientService);
            GateShare.AddMainLogMsg($"游戏引擎[{e.RemoteAddress}:{e.RemotePort}]断开链接.", 1);
            //GateShare.boServerReady = false;
        }

        /// <summary>
        /// 收到M2发来的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            try
            {
                int dwTick14 = HUtil32.GetTickCount();
                var nMsgLen = e.Buff.Length;
                ProcReceiveBuffer(e.Buff, nMsgLen);
                nBufferOfM2Size += nMsgLen;
                var dwTime10 = HUtil32.GetTickCount() - dwTick14;
                if (dwProcessServerMsgTime < dwTime10)
                {
                    dwProcessServerMsgTime = dwTime10;
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ClientSocketRead", 1);
            }
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    GateShare.AddMainLogMsg("游戏引擎[" + ClientSocket.Address + ":" + ClientSocket.Port + "]拒绝链接...", 1);
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    GateShare.AddMainLogMsg("游戏引擎[" + ClientSocket.Address + ":" + ClientSocket.Port + "]关闭连接...", 1);
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    GateShare.AddMainLogMsg("游戏引擎[" + ClientSocket.Address + ":" + ClientSocket.Port + "]链接超时...", 1);
                    break;
            }
        }

        public void RestSessionArray()
        {
            TSessionInfo tSession;
            for (var i = 0; i < MaxSession; i++)
            {
                if (SessionArray[i] == null)
                {
                    SessionArray[i] = new TSessionInfo();
                }
                tSession = SessionArray[i];
                tSession.Socket = null;
                tSession.sSocData = "";
                tSession.sSendData = "";
                tSession.nUserListIndex = 0;
                tSession.nPacketIdx = -1;
                tSession.nPacketErrCount = 0;
                tSession.boStartLogon = true;
                tSession.boSendLock = false;
                tSession.boSendAvailable = true;
                tSession.boSendCheck = false;
                tSession.nCheckSendLength = 0;
                tSession.dwReceiveTick = HUtil32.GetTickCount();
                tSession.nSckHandle = 0;
                tSession.SocketId = string.Empty;
                tSession.dwSayMsgTick = HUtil32.GetTickCount();
            }
        }

        private void SendServerMsg(ForwardMessage message)
        {
            SendServerMsg(message.nIdent, message.wSocketIndex, message.nSocket, message.nUserListIndex, message.nLen, message.Data);
        }

        public void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, ushort nUserListIndex, int nLen, string Data)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var strBuff = HUtil32.GetBytes(Data);
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, strBuff);
            }
            else
            {
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, Array.Empty<byte>());
            }
        }

        private void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, ushort nUserListIndex, int nLen, byte[] Data)
        {
            var GateMsg = new TMsgHeader();
            GateMsg.dwCode = Grobal2.RUNGATECODE;
            GateMsg.nSocket = nSocket;
            GateMsg.wGSocketIdx = wSocketIndex;
            GateMsg.wIdent = nIdent;
            GateMsg.wUserListIndex = nUserListIndex;
            GateMsg.nLength = nLen;
            var sendBuffer = GateMsg.GetPacket();
            if (Data is { Length: > 0 })
            {
                var tempBuff = new byte[20 + Data.Length];
                Buffer.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
                Buffer.BlockCopy(Data, 0, tempBuff, sendBuffer.Length, Data.Length);
                SendSocket(tempBuff);
            }
            else
            {
                SendSocket(sendBuffer);
            }
        }

        private void ProcReceiveBuffer(byte[] tBuffer, int nMsgLen)
        {
            TMsgHeader pMsg;
            var BuffIndex = 0;
            const int HeaderMessageSize = 20;
            try
            {
                if (nBuffLen > 0) //有未处理完成的buff
                {
                    var tempBuff = new byte[nBuffLen + nMsgLen];
                    Buffer.BlockCopy(SocketBuffer, 0, tempBuff, 0, nBuffLen);
                    Buffer.BlockCopy(tBuffer, 0, tempBuff, nBuffLen, tBuffer.Length);
                    SocketBuffer = tempBuff;
                }
                else
                {
                    SocketBuffer = tBuffer;
                }
                var nLen = nBuffLen + nMsgLen;
                var Buff = SocketBuffer;
                if (nLen >= HeaderMessageSize)
                {
                    while (true)
                    {
                        pMsg = new TMsgHeader(Buff);
                        if (pMsg.dwCode == Grobal2.RUNGATECODE)
                        {
                            if ((Math.Abs(pMsg.nLength) + HeaderMessageSize) > nLen)
                            {
                                break;
                            }
                            switch (pMsg.wIdent)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    boCheckServerFail = false;
                                    GateShare.dwCheckServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    if (pMsg.wGSocketIdx < MaxSession && pMsg.nSocket == SessionArray[pMsg.wGSocketIdx].nSckHandle)
                                    {
                                        SessionArray[pMsg.wGSocketIdx].nUserListIndex = (ushort)pMsg.wUserListIndex;
                                    }
                                    break;
                                case Grobal2.GM_RECEIVE_OK:
                                    GateShare.dwCheckServerTimeMin = HUtil32.GetTickCount() - GateShare.dwCheckRecviceTick;
                                    if (GateShare.dwCheckServerTimeMin > GateShare.dwCheckServerTimeMax)
                                    {
                                        GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                                    }
                                    GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
                                    SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, 0, "");
                                    break;
                                case Grobal2.GM_DATA:
                                    if (pMsg.nLength > 0)
                                    {
                                        byte[] MsgBuff = new byte[pMsg.nLength];
                                        Buffer.BlockCopy(Buff, HeaderMessageSize, MsgBuff, 0, MsgBuff.Length);//跳过消息头20字节
                                        ProcessMakeSocketStr(pMsg.nSocket, pMsg.wGSocketIdx, MsgBuff, pMsg.nLength);
                                    }
                                    else
                                    {
                                        byte[] MsgBuff = new byte[Buff.Length - 20];
                                        Buffer.BlockCopy(Buff, HeaderMessageSize, MsgBuff, 0, MsgBuff.Length);//跳过消息头20字节
                                        ProcessMakeSocketStr(pMsg.nSocket, pMsg.wGSocketIdx, MsgBuff, pMsg.nLength);
                                    }
                                    break;
                                case Grobal2.GM_TEST:
                                    break;
                            }
                            var newLen = HeaderMessageSize + Math.Abs(pMsg.nLength);
                            var tempBuff = new byte[Buff.Length - newLen];
                            Buffer.BlockCopy(Buff, newLen, tempBuff, 0, tempBuff.Length);
                            Buff = tempBuff;
                            BuffIndex = 0;
                            nLen -= Math.Abs(pMsg.nLength) + HeaderMessageSize;
                        }
                        else
                        {
                            BuffIndex++;
                            var messageBuff = new byte[Buff.Length - 1];
                            Array.Copy(Buff, BuffIndex, messageBuff, 0, HeaderMessageSize);
                            Buff = messageBuff;
                            nLen -= 1;
                        }
                        if (nLen < HeaderMessageSize)
                        {
                            break;
                        }
                    }
                }
                if (nLen > 0)
                {
                    var tempBuff = new byte[nLen];
                    Buffer.BlockCopy(Buff, 0, tempBuff, 0, nLen);
                    SocketBuffer = tempBuff;
                    nBuffLen = nLen;
                }
                else
                {
                    SocketBuffer = null;
                    nBuffLen = 0;
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg($"[Exception] ProcReceiveBuffer BuffIndex:{BuffIndex}", 1);
            }
        }

        private void ProcessMakeSocketStr(int nSocket, int nSocketIndex, byte[] buffer, int nMsgLen)
        {
            string sSendMsg = string.Empty;
            TDefaultMessage pDefMsg;
            try
            {
                switch (nMsgLen)
                {
                    case < 0:
                        sSendMsg = "#" + HUtil32.GetString(buffer, 0, buffer.Length - 1) + "!";
                        break;
                    case >= 12:
                        {
                            pDefMsg = new TDefaultMessage(buffer);
                            if (nMsgLen > 12)
                            {
                                var nLen = nMsgLen - 12;
                                var sb = new System.Text.StringBuilder();
                                sb.Append('#');
                                sb.Append(EDcode.EncodeMessage(pDefMsg));
                                var strBuff = new byte[nLen];
                                Buffer.BlockCopy(buffer, 12, strBuff, 0, strBuff.Length);
                                sb.Append(HUtil32.StrPasTest(strBuff));
                                sb.Append('!');
                                sSendMsg = sb.ToString();
                            }
                            else
                            {
                                sSendMsg = "#" + EDcode.EncodeMessage(pDefMsg) + "!";
                            }
                            break;
                        }
                }
                if (nSocketIndex >= 0 && nSocketIndex < MaxSession && !string.IsNullOrEmpty(sSendMsg))
                {
                    var userData = new TSendUserData();
                    userData.nSocketHandle = nSocket;
                    userData.SocketIndex = nSocketIndex;
                    //userData.sMsg = sSendMsg;
                    GateShare.SendMsgList.Writer.TryWrite(userData);
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ProcessMakeSocketStr", 1);
            }
        }

        private void SendSocket(byte[] sendBuffer)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.Send(sendBuffer);
            }
        }

        /// <summary>
        /// 每二秒向游戏服务器发送一个检查信号
        /// </summary>
        /// <param name="obj"></param>
        private void Heart(object obj)
        {
            if (GateShare.boGateReady)
            {
                SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, 0, "");
            }
            if ((HUtil32.GetTickCount() - GateShare.dwCheckServerTick) > GateShare.dwCheckServerTimeOutTime)
            {
                boCheckServerFail = true;
                //ClientSocket.Disconnect();
                Console.WriteLine("与服务器断开连接.");
            }
            //if (dwLoopTime > 30)
            //{
            //    dwLoopTime -= 20;
            //}
            //if (dwProcessServerMsgTime > 1)
            //{
            //    dwProcessServerMsgTime -= 1;
            //}
            //if (_serverService.dwProcessClientMsgTime > 1)
            //{
            //    _serverService.dwProcessClientMsgTime -= 1;
            //}
            GateShare.boDecodeMsgLock = false;
        }
    }

    public class ForwardMessage
    {
        public ushort nIdent;
        public ushort wSocketIndex;
        public int nSocket;
        public ushort nUserListIndex;
        public int nLen;
        public byte[] Data;
    }
}