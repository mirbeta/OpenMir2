using System;
using System.Threading;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace RunGate
{
    /// <summary>
    /// 网关客户端(RunGate-M2Server)
    /// </summary>
    public class UserClientService
    {
        private IClientScoket ClientSocket;
        public int nBufferOfM2Size = 0;
        private long dwProcessServerMsgTime = 0;
        /// <summary>
        /// 网关编号（初始化的时候进行分配）
        /// </summary>
        public int GateIdx = 0;
        /// <summary>
        /// 最大用户数
        /// </summary>
        public const int GATEMAXSESSION = 1000;
        /// <summary>
        /// 用户会话
        /// </summary>
        public TSessionInfo[] SessionArray;
        /// <summary>
        /// 心跳线程
        /// </summary>
        private Timer _heartTimer;
        /// <summary>
        ///  网关 <->游戏服务器之间检测是否失败（超时）
        /// </summary>
        public bool boCheckServerFail = false;

        public UserClientService(string serverAddr, int serverPort)
        {
            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
            ClientSocket.Address = serverAddr;
            ClientSocket.Port = serverPort;
            SessionArray = new TSessionInfo[GATEMAXSESSION];
        }

        public int GetMaxSession()
        {
            return GATEMAXSESSION;
        }

        public void Start()
        {
            ClientSocket.Connect();
            _heartTimer = new Timer(Heart, null, 5000, 2000);
        }

        public void Stop()
        {
            ClientSocket.Disconnect();
        }

        public TSessionInfo[] GetSession()
        {
            return SessionArray;
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateShare.boGateReady = true;
            GateShare.dwCheckServerTick = HUtil32.GetTickCount();
            GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
            RestSessionArray();
            GateShare.boServerReady = true;
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.dwCheckServerTimeMax = 0;
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            TSessionInfo UserSession;
            for (var i = 0; i < GATEMAXSESSION; i++)
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
            GateShare.SocketBuffer = null;
            GateShare.List_45AA58.Clear();
            GateShare.boGateReady = false;
            GateShare.boServerReady = false;
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
                long dwTick14 = HUtil32.GetTickCount();
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
            Console.WriteLine(e.exception);
            GateShare.boServerReady = false;
        }

        public void RestSessionArray()
        {
            TSessionInfo tSession;
            for (var i = 0; i < GATEMAXSESSION; i++)
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
                tSession.dwSayMsgTick = HUtil32.GetTickCount();
            }
        }

        public void SendServerMsg(ushort nIdent, int wSocketIndex, int nSocket, int nUserListIndex, int nLen, string Data)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var strBuff = System.Text.Encoding.GetEncoding("gb2312").GetBytes(Data);
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, strBuff);
            }
            else
            {
                SendServerMsg(nIdent, wSocketIndex, nSocket, nUserListIndex, nLen, new byte[0]);
            }
        }

        public void SendServerMsg(ushort nIdent, int wSocketIndex, int nSocket, int nUserListIndex, int nLen, byte[] Data)
        {
            var GateMsg = new TMsgHeader();
            GateMsg.dwCode = Grobal2.RUNGATECODE;
            GateMsg.nSocket = nSocket;
            GateMsg.wGSocketIdx = (ushort)wSocketIndex;
            GateMsg.wIdent = nIdent;
            GateMsg.wUserListIndex = (ushort)nUserListIndex;
            GateMsg.nLength = nLen;
            var SendBuffer = GateMsg.ToByte();
            if (Data != null && Data.Length > 0)
            {
                var tempBuff = new byte[20 + Data.Length];
                Buffer.BlockCopy(SendBuffer, 0, tempBuff, 0, SendBuffer.Length);
                Buffer.BlockCopy(Data, 0, tempBuff, SendBuffer.Length, Data.Length);
                SendSocket(tempBuff);
            }
            else
            {
                SendSocket(SendBuffer);
            }
        }

        private void ProcReceiveBuffer(byte[] tBuffer, int nMsgLen)
        {
            TMsgHeader pMsg;
            var BuffIndex = 0;
            const int HeaderMessageSize = 20;
            try
            {
                if (GateShare.nBuffLen > 0) //有位处理完成的buff
                {
                    var tempBuff = new byte[GateShare.nBuffLen + nMsgLen];
                    Buffer.BlockCopy(GateShare.SocketBuffer, 0, tempBuff, 0, GateShare.nBuffLen);
                    Buffer.BlockCopy(tBuffer, 0, tempBuff, GateShare.nBuffLen, tBuffer.Length);
                    GateShare.SocketBuffer = tempBuff;
                }
                else
                {
                    GateShare.SocketBuffer = tBuffer;
                }
                var nLen = GateShare.nBuffLen + nMsgLen;
                var Buff = GateShare.SocketBuffer;
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
                                    if (pMsg.wGSocketIdx < GATEMAXSESSION && pMsg.nSocket == SessionArray[pMsg.wGSocketIdx].nSckHandle)
                                    {
                                        SessionArray[pMsg.wGSocketIdx].nUserListIndex = pMsg.wUserListIndex;
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
                                    //byte[] MsgBuff = new byte[Buff.Length - HeaderMessageSize];
                                    //Buffer.BlockCopy(Buff, HeaderMessageSize, MsgBuff, 0, MsgBuff.Length);//跳过消息头20字节
                                    //ProcessMakeSocketStr(pMsg.nSocket, pMsg.wGSocketIdx, MsgBuff, pMsg.nLength);
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
                    GateShare.SocketBuffer = tempBuff;
                    GateShare.nBuffLen = nLen;
                }
                else
                {
                    GateShare.SocketBuffer = null;
                    GateShare.nBuffLen = 0;
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
                        sSendMsg = "#" + HUtil32.GetString(buffer, 0, buffer.Length) + "!";
                        break;
                    case >= 12:
                        {
                            pDefMsg = new TDefaultMessage(buffer);
                            if (nMsgLen > 12)
                            {
                                var nLen = nMsgLen - 12;
                                var sb = new System.Text.StringBuilder();
                                sb.Append("#");
                                sb.Append(EDcode.EncodeMessage(pDefMsg));
                                var strBuff = new byte[nLen];
                                Buffer.BlockCopy(buffer, 12, strBuff, 0, strBuff.Length);
                                sb.Append(HUtil32.StrPasTest(strBuff));
                                sb.Append("!");
                                sSendMsg = sb.ToString();
                            }
                            else
                            {
                                sSendMsg = "#" + EDcode.EncodeMessage(pDefMsg) + "!";
                            }
                            break;
                        }
                }
                if (nSocketIndex >= 0 && nSocketIndex < GATEMAXSESSION && !string.IsNullOrEmpty(sSendMsg))
                {
                    var UserData = new TSendUserData();
                    UserData.nSocketIdx = nSocketIndex;
                    UserData.nSocketHandle = nSocket;
                    UserData.sMsg = sSendMsg;
                    UserData.UserClient = this;
                    GateShare.SendMsgList.Writer.TryWrite(UserData);
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ProcessMakeSocketStr", 1);
            }
        }

        private void SendSocket(byte[] SendBuffer)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.Send(SendBuffer);
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
            if (HUtil32.GetTickCount() - GateShare.dwCheckServerTick > GateShare.dwCheckServerTimeOutTime)
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
}