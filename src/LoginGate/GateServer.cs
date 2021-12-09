using System;
using System.Threading;
using SystemModule;
using SystemModule.Sockets;

namespace LoginGate
{
    public class GateServer
    {
        private ISocketServer ServerSocket;
        private readonly GateClient gateClient;
        private long dwDecodeMsgTime = 0;
        private long dwSendKeepAliveTick = 0;
        private string sProcMsg = string.Empty;
        private Timer decodeTimer;

        public GateServer(GateClient gateClient)
        {
            this.gateClient = gateClient;
            ServerSocket = new ISocketServer(ushort.MaxValue, 1024);
            ServerSocket.OnClientConnect += ServerSocketClientConnect;
            ServerSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            ServerSocket.OnClientRead += ServerSocketClientRead;
            ServerSocket.OnClientError += ServerSocketClientError;
            ServerSocket.Init();
            dwDecodeMsgTime = 0;
        }

        public void Start()
        {
            dwSendKeepAliveTick = HUtil32.GetTickCount();
            ServerSocket.Start(GateShare.GateAddr, GateShare.GatePort);
            decodeTimer = new Timer(DecodeTimer, null, 0, 1);
        }

        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            TUserSession UserSession;
            string sLocalIPaddr;
            TSockaddr IPaddr;
            string sRemoteIPaddr = e.RemoteIPaddr;
            if (GateShare.g_boDynamicIPDisMode)
            {
                sLocalIPaddr = e.RemoteIPaddr;
            }
            else
            {
                sLocalIPaddr = e.RemoteIPaddr;
            }
            if (IsBlockIP(sRemoteIPaddr))
            {
                GateShare.MainOutMessage("过滤连接: " + sRemoteIPaddr, 1);
                e.Socket.Close();
                return;
            }
            if (IsConnLimited(sRemoteIPaddr))
            {
                switch (GateShare.BlockMethod)
                {
                    case TBlockIPMethod.mDisconnect:
                        e.Socket.Close();
                        break;
                    case TBlockIPMethod.mBlock:
                        IPaddr = new TSockaddr();
                        IPaddr.nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
                        GateShare.TempBlockIPList.Add(IPaddr);
                        CloseConnect(sRemoteIPaddr);
                        break;
                    case TBlockIPMethod.mBlockList:
                        IPaddr = new TSockaddr();
                        IPaddr.nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
                        GateShare.BlockIPList.Add(IPaddr);
                        CloseConnect(sRemoteIPaddr);
                        break;
                }
                GateShare.MainOutMessage("端口攻击: " + sRemoteIPaddr, 1);
                return;
            }
            if (GateShare.boGateReady)
            {
                for (var nSockIndex = 0; nSockIndex < GateShare.GATEMAXSESSION; nSockIndex++)
                {
                    UserSession = GateShare.g_SessionArray[nSockIndex];
                    if (UserSession.Socket == null)
                    {
                        UserSession.Socket = e.Socket;
                        UserSession.sRemoteIPaddr = sRemoteIPaddr;
                        UserSession.nSendMsgLen = 0;
                        UserSession.bo0C = false;
                        UserSession.dw10Tick = HUtil32.GetTickCount();
                        UserSession.dwConnctCheckTick = HUtil32.GetTickCount();
                        UserSession.boSendAvailable = true;
                        UserSession.boSendCheck = false;
                        UserSession.nCheckSendLength = 0;
                        UserSession.n20 = 0;
                        UserSession.dwUserTimeOutTick = HUtil32.GetTickCount();
                        UserSession.SocketHandle = (int)e.Socket.Handle;
                        UserSession.sIP = sRemoteIPaddr;
                        UserSession.MsgList.Clear();
                        GateShare.socketMap.TryAdd(e.ConnectionId, nSockIndex);
                        GateShare.nSessionCount++;
                        break;
                    }
                }
                if (e.ConnectionId > 0 && gateClient.ClientSocket.IsConnected)
                {
                    gateClient.ClientSocket.SendText("%O" + (int)e.Socket.Handle + "/" + sRemoteIPaddr + "/" + sLocalIPaddr + "$");
                    GateShare.MainOutMessage("客户端链接: " + sRemoteIPaddr, 5);
                }
                else
                {
                    e.Socket.Close();
                    GateShare.MainOutMessage("账号服务器未就绪，断开链接: " + sRemoteIPaddr, 1);
                }
            }
            else
            {
                e.Socket.Close();
                GateShare.MainOutMessage("服务器未准备就绪，断开链接: " + sRemoteIPaddr, 1);
            }
        }

        public void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            if (!GateShare.socketMap.ContainsKey(e.ConnectionId))
            {
                return;
            }
            TUserSession UserSession;
            string sRemoteIPaddr = e.RemoteIPaddr;
            long nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
            int nSockIndex = GateShare.socketMap[e.ConnectionId];
            for (var i = 0; i < GateShare.CurrIPaddrList.Count; i++)
            {
                TSockaddr IPaddr = GateShare.CurrIPaddrList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    IPaddr.nCount -= 1;
                    if (IPaddr.nCount <= 0)
                    {
                        IPaddr = null;
                        GateShare.CurrIPaddrList.RemoveAt(i);
                    }
                    break;
                }
            }
            if ((nSockIndex >= 0) && (nSockIndex < GateShare.GATEMAXSESSION))
            {
                UserSession = GateShare.g_SessionArray[nSockIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
                UserSession.MsgList.Clear();
                GateShare.nSessionCount -= 1;
                if (GateShare.boGateReady && gateClient.ClientSocket.IsConnected)
                {
                    gateClient.ClientSocket.SendText("%X" + e.Socket.Handle + "$");
                    GateShare.MainOutMessage("DisConnect: " + sRemoteIPaddr, 5);
                }
            }
        }

        public void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        public void ServerSocketClientRead(object sender, AsyncUserToken e)
        {
            if (!GateShare.socketMap.TryGetValue(e.ConnectionId, out var nSockIndex))
            {
                return;
            }
            if ((nSockIndex >= 0) && (nSockIndex < GateShare.GATEMAXSESSION))
            {
                TUserSession UserSession = GateShare.g_SessionArray[nSockIndex];
                var nReviceLen = e.BytesReceived;
                var data = new byte[nReviceLen];
                Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                string sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                if ((sReviceMsg != "") && GateShare.boServerReady)
                {
                    int nPos = sReviceMsg.IndexOf("*");
                    if (nPos > 0)
                    {
                        UserSession.boSendAvailable = true;
                        UserSession.boSendCheck = false;
                        UserSession.nCheckSendLength = 0;
                        string s10 = sReviceMsg.Substring(0, nPos - 1);
                        string s1C = sReviceMsg.Substring(nPos, sReviceMsg.Length - nPos);
                        sReviceMsg = s10 + s1C;
                    }
                    int nMsgLen = sReviceMsg.Length;
                    if ((sReviceMsg != "") && (GateShare.boGateReady) && (!GateShare.boKeepAliveTimcOut))
                    {
                        UserSession.dwConnctCheckTick = HUtil32.GetTickCount();
                        if ((HUtil32.GetTickCount() - UserSession.dwUserTimeOutTick) < 1000)
                        {
                            UserSession.n20 += nMsgLen;
                        }
                        else
                        {
                            UserSession.n20 = nMsgLen;
                        }
                        gateClient.ClientSocket.SendText("%A" + (int)e.Socket.Handle + "/" + sReviceMsg + "$");
                    }
                }
            }
        }

        public void DecodeTimer(object obj)
        {
            string sProcessMsg = string.Empty;
            string sSocketMsg = string.Empty;
            string sSocketHandle = string.Empty;
            int dwDecodeTick = 0;
            TUserSession UserSession;
            TSockaddr IPaddr;
            if (GateShare.boDecodeLock || (!GateShare.boGateReady))
            {
                return;
            }
            try
            {
                dwDecodeTick = HUtil32.GetTickCount();
                GateShare.boDecodeLock = true;
                int nSocketIndex;
                while (true)
                {
                    if (GateShare.ClientSockeMsgList.Count <= 0)
                    {
                        break;
                    }
                    sProcessMsg = sProcMsg + GateShare.ClientSockeMsgList[0];
                    sProcMsg = "";
                    GateShare.ClientSockeMsgList.RemoveAt(0);
                    while (true)
                    {
                        if (HUtil32.TagCount(sProcessMsg, '$') < 1)
                        {
                            break;
                        }
                        sProcessMsg = HUtil32.ArrestStringEx(sProcessMsg, "%", "$", ref sSocketMsg);
                        if (string.IsNullOrEmpty(sSocketMsg))
                        {
                            break;
                        }
                        if (sSocketMsg[0] == '+')
                        {
                            if (sSocketMsg[1] == '-')
                            {
                                CloseSocket(HUtil32.Str_ToInt(sSocketMsg.Substring(2, sSocketMsg.Length - 2), 0));
                                continue;
                            }
                            else
                            {
                                GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
                                GateShare.boKeepAliveTimcOut = false;
                                continue;
                            }
                        }
                        sSocketMsg = HUtil32.GetValidStr3(sSocketMsg, ref sSocketHandle, new string[] { "/" });
                        int nSocketHandle = HUtil32.Str_ToInt(sSocketHandle, -1);
                        if (nSocketHandle < 0)
                        {
                            continue;
                        }
                        for (nSocketIndex = 0; nSocketIndex < GateShare.GATEMAXSESSION; nSocketIndex++)
                        {
                            if (GateShare.g_SessionArray[nSocketIndex].SocketHandle == nSocketHandle)
                            {
                                GateShare.g_SessionArray[nSocketIndex].MsgList.Add(sSocketMsg);
                                break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sProcessMsg))
                {
                    sProcMsg = sProcessMsg;
                }
                GateShare.nSendMsgCount = 0;
                for (nSocketIndex = 0; nSocketIndex < GateShare.GATEMAXSESSION; nSocketIndex++)
                {
                    if (GateShare.g_SessionArray[nSocketIndex].SocketHandle <= -1)
                    {
                        continue;
                    }
                    if ((HUtil32.GetTickCount() - GateShare.g_SessionArray[nSocketIndex].dwConnctCheckTick) > GateShare.dwKeepConnectTimeOut)// 踢除超时无数据传输连接
                    {
                        string sRemoteIPaddr = GateShare.g_SessionArray[nSocketIndex].sRemoteIPaddr;
                        switch (GateShare.BlockMethod)
                        {
                            case TBlockIPMethod.mDisconnect:
                                GateShare.g_SessionArray[nSocketIndex].Socket.Close();
                                break;
                            case TBlockIPMethod.mBlock:
                                IPaddr = new TSockaddr();
                                IPaddr.nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
                                GateShare.TempBlockIPList.Add(IPaddr);
                                CloseConnect(sRemoteIPaddr);
                                break;
                            case TBlockIPMethod.mBlockList:
                                IPaddr = new TSockaddr();
                                IPaddr.nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
                                GateShare.BlockIPList.Add(IPaddr);
                                CloseConnect(sRemoteIPaddr);
                                break;
                        }
                        GateShare.MainOutMessage("端口空连接攻击: " + sRemoteIPaddr, 1);
                        continue;
                    }
                    while (true)
                    {
                        if (GateShare.g_SessionArray[nSocketIndex].MsgList.Count <= 0)
                        {
                            break;
                        }
                        UserSession = GateShare.g_SessionArray[nSocketIndex];
                        int nSendRetCode = SendUserMsg(UserSession, UserSession.MsgList[0]);
                        if ((nSendRetCode >= 0))
                        {
                            if (nSendRetCode == 1)
                            {
                                UserSession.dwConnctCheckTick = HUtil32.GetTickCount();
                                UserSession.MsgList.RemoveAt(0);
                                continue;
                            }
                            if (UserSession.MsgList.Count > 100)
                            {
                                int nMsgCount = 0;
                                while (nMsgCount != 51)
                                {
                                    UserSession.MsgList.RemoveAt(0);
                                    nMsgCount++;
                                }
                            }
                            GateShare.MainOutMessage(UserSession.sIP + " : " + UserSession.MsgList.Count, 5);
                            GateShare.nSendMsgCount++;
                        }
                        else
                        {
                            UserSession.SocketHandle = -1;
                            UserSession.Socket = null;
                            UserSession.MsgList.Clear();
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - dwSendKeepAliveTick) > 2 * 1000)
                {
                    dwSendKeepAliveTick = HUtil32.GetTickCount();
                    if (GateShare.boGateReady && gateClient.ClientSocket.IsConnected)
                    {
                        gateClient.ClientSocket.SendText("%--$");
                    }
                }
                if ((HUtil32.GetTickCount() - GateShare.dwKeepAliveTick) > 10 * 1000)
                {
                    GateShare.boKeepAliveTimcOut = true;
                    gateClient.ClientSocket.Disconnect();
                }
            }
            finally
            {
                GateShare.boDecodeLock = false;
            }
            int dwDecodeTime = HUtil32.GetTickCount() - dwDecodeTick;
            if (dwDecodeMsgTime < dwDecodeTime)
            {
                dwDecodeMsgTime = dwDecodeTime;
            }
            if (dwDecodeMsgTime > 50)
            {
                dwDecodeMsgTime -= 50;
            }
        }

        private int SendUserMsg(TUserSession UserSession, string sSendMsg)
        {
            var result = -1;
            if (UserSession.Socket != null)
            {
                if (!UserSession.bo0C)
                {
                    if (!UserSession.boSendAvailable && (HUtil32.GetTickCount() > UserSession.dwSendLockTimeOut))
                    {
                        UserSession.boSendAvailable = true;
                        UserSession.nCheckSendLength = 0;
                        GateShare.boSendHoldTimeOut = true;
                        GateShare.dwSendHoldTick = HUtil32.GetTickCount();
                    }
                    if (UserSession.boSendAvailable)
                    {
                        if (UserSession.nCheckSendLength >= 250)
                        {
                            if (!UserSession.boSendCheck)
                            {
                                UserSession.boSendCheck = true;
                                sSendMsg = "*" + sSendMsg;
                            }
                            if (UserSession.nCheckSendLength >= 512)
                            {
                                UserSession.boSendAvailable = false;
                                UserSession.dwSendLockTimeOut = HUtil32.GetTickCount() + 3 * 1000;
                            }
                        }
                        UserSession.Socket.SendText(sSendMsg);
                        UserSession.nSendMsgLen += sSendMsg.Length;
                        UserSession.nCheckSendLength += sSendMsg.Length;
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            var nIPaddr = HUtil32.IpToInt(sIPaddr);
            TSockaddr IPaddr = null;
            for (var i = 0; i < GateShare.TempBlockIPList.Count; i++)
            {
                IPaddr = GateShare.TempBlockIPList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    result = true;
                    return result;
                }
            }
            for (var i = 0; i < GateShare.BlockIPList.Count; i++)
            {
                IPaddr = GateShare.BlockIPList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }

        private bool IsConnLimited(string sIPaddr)
        {
            bool result = false;
            TSockaddr IPaddr;
            bool boDenyConnect = false;
            long nIPaddr = HUtil32.IpToInt(sIPaddr);
            for (var i = 0; i < GateShare.CurrIPaddrList.Count; i++)
            {
                IPaddr = GateShare.CurrIPaddrList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    IPaddr.nCount++;
                    if (HUtil32.GetTickCount() - IPaddr.dwIPCountTick1 < 1000)
                    {
                        IPaddr.nIPCount1++;
                        if (IPaddr.nIPCount1 >= GateShare.nIPCountLimit1)
                        {
                            boDenyConnect = true;
                        }
                    }
                    else
                    {
                        IPaddr.dwIPCountTick1 = HUtil32.GetTickCount();
                        IPaddr.nIPCount1 = 0;
                    }
                    if (HUtil32.GetTickCount() - IPaddr.dwIPCountTick2 < 3000)
                    {
                        IPaddr.nIPCount2++;
                        if (IPaddr.nIPCount2 >= GateShare.nIPCountLimit2)
                        {
                            boDenyConnect = true;
                        }
                    }
                    else
                    {
                        IPaddr.dwIPCountTick2 = HUtil32.GetTickCount();
                        IPaddr.nIPCount2 = 0;
                    }
                    if (IPaddr.nCount > GateShare.nMaxConnOfIPaddr)
                    {
                        boDenyConnect = true;
                    }
                    result = boDenyConnect;
                    return result;
                }
            }
            IPaddr = new TSockaddr();
            IPaddr.nIPaddr = nIPaddr;
            IPaddr.nCount = 1;
            GateShare.CurrIPaddrList.Add(IPaddr);
            return result;
        }

        private void CloseConnect(string sIPaddr)
        {
            bool boCheck;
            if (ServerSocket.Active)
            {
                while (true)
                {
                    boCheck = false;
                    var socketList = ServerSocket.GetSockets();
                    for (var i = 0; i < socketList.Count; i++)
                    {
                        if (sIPaddr == socketList[i].RemoteIPaddr)
                        {
                            socketList[i].Socket.Close();
                            boCheck = true;
                            break;
                        }
                    }
                    if (!boCheck)
                    {
                        break;
                    }
                }
            }
        }

        private void CloseSocket(int nSocketHandle)
        {
            TUserSession UserSession;
            for (var nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = GateShare.g_SessionArray[nIndex];
                if ((UserSession.Socket != null) && (UserSession.SocketHandle == nSocketHandle))
                {
                    UserSession.Socket.Close();
                    break;
                }
            }
        }
    }
}