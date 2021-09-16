using System;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Sockets;

namespace SelGate
{
    public class GateClient
    {
        private readonly IClientScoket ClientSocket;
        private static TUserSession[] g_SessionArray;

        public void ClientSocketConnect(Object Sender, Socket Socket)
        {
            GateShare.boGateReady = true;
            nSessionCount = 0;
            GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
            ResUserSessionArray();
            boServerReady = true;
        }

        public void ClientSocketDisconnect(Object Sender, Socket Socket)
        {
            TUserSession UserSession;
            int nIndex;
            for (nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = g_SessionArray[nIndex];
                if (UserSession.Socket != null)
                {
                    UserSession.Socket.Close();
                }

                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
            }

            ResUserSessionArray();
            ClientSockeMsgList.Clear();
            GateShare.boGateReady = false;
            GateShare.nSessionCount = 0;
        }

        public void ClientSocketError(Object Sender, Socket Socket)
        {
            boServerReady = false;
        }

        public void ClientSocketRead(Object Sender, Socket Socket)
        {
            //string sRecvMsg;
            //sRecvMsg = Socket.ReceiveText;
            //ClientSockeMsgList.Add(sRecvMsg);
        }
        
        private void ResUserSessionArray()
        {
            TUserSession UserSession;
            int nIndex;
            for (nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = g_SessionArray[nIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
                UserSession.MsgList.Clear();
            }
        }

    }
}