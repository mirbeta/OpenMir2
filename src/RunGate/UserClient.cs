using System;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace RunGate
{
    public class UserClient
    {
        private IClientScoket ClientSocket;
        public int nBufferOfM2Size = 0;
        private long dwProcessServerMsgTime = 0;
        public bool boServerReady;

        public UserClient()
        {
            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.Address = GateShare.ServerAddr;
            ClientSocket.Port = GateShare.ServerPort;
        }

        public void Start()
        {
            ClientSocket.Connect();
        }

        public void Stop()
        {
            ClientSocket.Disconnect();
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateShare.boGateReady = true;
            GateShare.dwCheckServerTick = HUtil32.GetTickCount();
            GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
            RestSessionArray();
            boServerReady = true;
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.dwCheckServerTimeMax = 0;
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            TSessionInfo UserSession;
            for (var i = 0; i < GateShare.GATEMAXSESSION; i++)
            {
                UserSession = GateShare.SessionArray[i];
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
            boServerReady = false;
        }

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

        public void RestSessionArray()
        {
            TSessionInfo tSession;
            for (var i = 0; i < GateShare.GATEMAXSESSION; i ++ )
            {
                if (GateShare.SessionArray[i] == null)
                {
                    GateShare.SessionArray[i] = new TSessionInfo();
                }
                tSession = GateShare.SessionArray[i];
                tSession.Socket = null;
                tSession.sSocData = "";
                tSession.sSendData = "";
                tSession.nUserListIndex = 0;
                tSession.nPacketIdx =  -1;
                tSession.nPacketErrCount = 0;
                tSession.boStartLogon = true;
                tSession.boSendLock = false;
                tSession.boOverNomSize = false;
                tSession.nOverNomSizeCount = 0;
                tSession.dwSendLatestTime = HUtil32.GetTickCount();
                tSession.boSendAvailable = true;
                tSession.boSendCheck = false;
                tSession.nCheckSendLength = 0;
                tSession.nReceiveLength = 0;
                tSession.dwReceiveTick = HUtil32.GetTickCount();
                tSession.nSckHandle =  -1;
                tSession.dwSayMsgTick = HUtil32.GetTickCount();
            }
        }
        
        public void SendServerMsg(ushort nIdent, ushort wSocketIndex, int nSocket, short nUserListIndex, int nLen, string Data)
        {
            var GateMsg = new TMsgHeader();
            GateMsg.dwCode = Grobal2.RUNGATECODE;
            GateMsg.nSocket = nSocket;
            GateMsg.wGSocketIdx = wSocketIndex;
            GateMsg.wIdent = nIdent;
            GateMsg.wUserListIndex = nUserListIndex;
            GateMsg.nLength = nLen;
            SendSocket(GateMsg.ToByte());
        }
        
        private void ProcReceiveBuffer(byte[] tBuffer, int nMsgLen)
        {
            int nLen;
            byte[] Buff;
            TMsgHeader pMsg;
            byte[] MsgBuff;
            byte[] TempBuff;
            try
            {
                //ReallocMem(GateShare.SocketBuffer, GateShare.nBuffLen + nMsgLen);
                GateShare.SocketBuffer = new byte[GateShare.nBuffLen + nMsgLen];
                //Move(tBuffer, GateShare.SocketBuffer[GateShare.nBuffLen], nMsgLen);
                Buffer.BlockCopy(tBuffer, 0, GateShare.SocketBuffer, 0, nMsgLen);
                //FreeMem(tBuffer);
                nLen = GateShare.nBuffLen + nMsgLen;
                Buff = GateShare.SocketBuffer;
                if (nLen >= 20)
                {
                    while (true)
                    {
                        pMsg = new TMsgHeader(Buff);
                        if (pMsg.dwCode == Grobal2.RUNGATECODE)
                        {
                            if (Math.Abs(pMsg.nLength) + 20 > nLen)
                            {
                                break;
                            }
                            //MsgBuff = Ptr((long) Buff + 20);
                            MsgBuff = new byte[Buff.Length - 20];
                            Buffer.BlockCopy(Buff, 20, MsgBuff, 0, MsgBuff.Length);
                            switch (pMsg.wIdent)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    GateShare.boCheckServerFail = false;
                                    GateShare.dwCheckServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    if (pMsg.wGSocketIdx < GateShare.GATEMAXSESSION && pMsg.nSocket ==
                                        GateShare.SessionArray[pMsg.wGSocketIdx].nSckHandle)
                                    {
                                        GateShare.SessionArray[pMsg.wGSocketIdx].nUserListIndex = pMsg.wUserListIndex;
                                    }
                                    break;
                                case Grobal2.GM_RECEIVE_OK:
                                    GateShare.dwCheckServerTimeMin = HUtil32.GetTickCount() - GateShare.dwCheckRecviceTick;
                                    if (GateShare.dwCheckServerTimeMin > GateShare.dwCheckServerTimeMax)
                                    {
                                        GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                                    }
                                    GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
                                    SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, 0, null);
                                    break;
                                case Grobal2.GM_DATA:
                                    ProcessMakeSocketStr(pMsg.nSocket, pMsg.wGSocketIdx, MsgBuff, pMsg.nLength);
                                    break;
                                case Grobal2.GM_TEST:
                                    break;
                            }

                            //Buff = Buff[20 + Math.Abs(pMsg.nLength)];
                            var tempBuff = new byte[20 + Math.Abs(pMsg.nLength)];
                            Buffer.BlockCopy(Buff, 0, tempBuff, 0, tempBuff.Length);
                            nLen = nLen - (Math.Abs(pMsg.nLength) + 20);
                        }
                        else
                        {
                            //Buff++;
                            nLen -= 1;
                        }

                        if (nLen < 20)
                        {
                            break;
                        }
                    }
                }

                if (nLen > 0)
                {
                    //GetMem(TempBuff, nLen);
                    TempBuff = new byte[nLen];
                    Buffer.BlockCopy(Buff, 0, TempBuff, 0, nLen);
                    //Move(Buff, TempBuff, nLen);
                    //FreeMem(GateShare.SocketBuffer);
                    GateShare.SocketBuffer = TempBuff;
                    GateShare.nBuffLen = nLen;
                }
                else
                {
                    //FreeMem(GateShare.SocketBuffer);
                    GateShare.SocketBuffer = null;
                    GateShare.nBuffLen = 0;
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ProcReceiveBuffer", 1);
            }
        }

        private void ProcessMakeSocketStr(int nSocket, int nSocketIndex, byte[] buffer, int nMsgLen)
        {
            string sSendMsg;
            TDefaultMessage pDefMsg;
            TSendUserData UserData;
            try
            {
                sSendMsg = "";
                if (nMsgLen < 0)
                {
                    sSendMsg = "#" + buffer + "!";
                }
                else
                {
                    if (nMsgLen >= 12)
                    {
                        pDefMsg = new TDefaultMessage(buffer);//((TDefaultMessage) (Buffer));
                        if (nMsgLen > 12)
                        {
                            var buffStr = new byte[buffer.Length - 12];
                            Buffer.BlockCopy(buffer, 12, buffStr, 0, buffStr.Length);
                            sSendMsg = "#" + EDcode.EncodeMessage(pDefMsg) + HUtil32.GetString(buffStr,0,buffStr.Length) + "!";
                            //sSendMsg = "#" + EDcode.EncodeMessage(pDefMsg) + ((Buffer[12] as string) as string) + "!";
                        }
                        else
                        {
                            sSendMsg = "#" + EDcode.EncodeMessage(pDefMsg) + "!";
                        }
                    }
                }
                if (nSocketIndex >= 0 && nSocketIndex < GateShare.GATEMAXSESSION && sSendMsg != "")
                {
                    UserData = new TSendUserData();
                    UserData.nSocketIdx = nSocketIndex;
                    UserData.nSocketHandle = nSocket;
                    UserData.sMsg = sSendMsg;
                    GateShare.SendMsgList.Add(UserData);
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ProcessMakeSocketStr", 1);
            }
        }
        
        private void SendSocket(string SendBuffer, int nLen)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.SendText(SendBuffer);
            }
        }

        private void SendSocket(byte[] SendBuffer)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.Send(SendBuffer);
            }
        }
    }
}