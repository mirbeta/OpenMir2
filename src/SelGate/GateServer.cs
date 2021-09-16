using System;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Sockets;

namespace SelGate
{
    public class GateServer
    {
        private readonly ISocketServer ServerSocket;

        public GateServer()
        {
            ServerSocket.OnClientConnect += ServerSocketClientConnect;
            ServerSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            ServerSocket.OnClientRead += ServerSocketClientRead;
        }

        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            TUserSession UserSession;
            string sLocalIPaddr = string.Empty;
            int nSockIndex;
            TSockaddr IPaddr;
            var sRemoteIPaddr = e.RemoteIPaddr;
            if (GateShare.g_boDynamicIPDisMode)
            {
                sLocalIPaddr = ClientSocket.Socket.RemoteAddress;
            }
            else
            {
                sLocalIPaddr = Socket.LocalAddress;
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
                for (nSockIndex = 0; nSockIndex < GateShare.GATEMAXSESSION; nSockIndex++)
                {
                    UserSession = g_SessionArray[nSockIndex];
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
                        //Socket.nIndex = nSockIndex;
                        GateShare._sessionMap.TryAdd(e.ConnectionId, nSockIndex);
                        GateShare.nSessionCount++;
                        break;
                    }
                }
                if (nSockIndex >= 0)
                {
                    ClientSocket.SendText("%O" + (int)e.Socket.Handle + "/" + sRemoteIPaddr + "/" + sLocalIPaddr + "$");
                    GateShare.MainOutMessage("Connect: " + sRemoteIPaddr, 5);
                }
                else
                {
                    e.Socket.Close();
                    GateShare.MainOutMessage("Kick Off: " + sRemoteIPaddr, 1);
                }
            }
            else
            {
                e.Socket.Close();
                GateShare. MainOutMessage("Kick Off: " + sRemoteIPaddr, 1);
            }
        }

        public void ServerSocketClientDisconnect(object Sender, AsyncUserToken e)
        {
            TUserSession UserSession;
            int nSockIndex = 0;
            string sRemoteIPaddr = string.Empty;
            TSockaddr IPaddr = null;
            //sRemoteIPaddr = Socket.RemoteAddress;
            long nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
            //nSockIndex = Socket.nIndex;
            for (var i = 0; i < GateShare.CurrIPaddrList.Count; i++)
            {
                IPaddr = GateShare.CurrIPaddrList[i];
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
                UserSession = g_SessionArray[nSockIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
                UserSession.MsgList.Clear();
                GateShare.nSessionCount -= 1;
                if (GateShare.boGateReady)
                {
                    ClientSocket.SendText("%X" + (int)e.Socket.Handle + "$");
                    GateShare. MainOutMessage("DisConnect: " + sRemoteIPaddr, 5);
                }
            }
        }

        public void ServerSocketClientError(Object Sender, Socket Socket)
        {
 
        }

        public void ServerSocketClientRead(object Sender,AsyncUserToken e)
        {
            TUserSession UserSession;
            int nSockIndex = 0;
            string s10;
            string s1C;
            int nPos;
            int nMsgLen;
           // nSockIndex = Socket.nIndex;
            if ((nSockIndex >= 0) && (nSockIndex <GateShare. GATEMAXSESSION))
            {
                UserSession = g_SessionArray[nSockIndex];
                var nReviceLen = e.BytesReceived;
                var data = new byte[nReviceLen];
                Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                var sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                if ((sReviceMsg != "") && boServerReady)
                {
                    nPos = sReviceMsg.IndexOf("*");
                    if (nPos > 0)
                    {
                        UserSession.boSendAvailable = true;
                        UserSession.boSendCheck = false;
                        UserSession.nCheckSendLength = 0;
                        s10 = sReviceMsg.Substring(0, nPos - 1);
                        s1C = sReviceMsg.Substring(nPos + 1 - 1, sReviceMsg.Length - nPos);
                        sReviceMsg = s10 + s1C;
                    }
                    nMsgLen = sReviceMsg.Length;
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
                        ClientSocket.SendText("%A" + (int)e.Socket.Handle + "/" + sReviceMsg + "$");
                    }
                }
            }
        }
        
        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            TSockaddr IPaddr;
            long nIPaddr = HUtil32.IpToInt(sIPaddr);
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
            bool result;
            TSockaddr IPaddr;
            bool boDenyConnect;
            result = false;
            boDenyConnect = false;
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
    }
}