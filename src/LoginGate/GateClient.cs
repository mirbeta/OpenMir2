using SystemModule;
using SystemModule.Sockets;

namespace LoginGate
{
    public class GateClient
    {
        public IClientScoket ClientSocket;

        public GateClient()
        {
            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;
        }

        public void Start()
        {
            ClientSocket.Connect(GateShare.ServerAddr, GateShare.ServerPort);
            ResUserSessionArray();
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateShare.boGateReady = true;
            GateShare.nSessionCount = 0;
            GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
            ResUserSessionArray();
            GateShare.boServerReady = true;
        }

        public void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            TUserSession UserSession;
            for (var nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = GateShare.g_SessionArray[nIndex];
                if (UserSession.Socket != null)
                {
                    UserSession.Socket.Close();
                }
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
            }
            ResUserSessionArray();
            //ClientSockeMsgList.Clear();
            GateShare.boGateReady = false;
            GateShare.nSessionCount = 0;
        }

        public void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            GateShare.boServerReady = false;
        }

        public void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var sReviceMsg = HUtil32.GetString(e.Buff, 0, e.Buff.Length);
            GateShare.ClientSockeMsgList.Add(sReviceMsg);
        }

        private void ResUserSessionArray()
        {
            TUserSession UserSession;
            for (var nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = GateShare.g_SessionArray[nIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
                UserSession.MsgList.Clear();
            }
        }

    }
}