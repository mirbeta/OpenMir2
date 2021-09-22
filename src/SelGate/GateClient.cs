using SystemModule;
using SystemModule.Sockets;

namespace SelGate
{
    public class GateClient
    {
        public readonly IClientScoket ClientSocket;

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
            ResUserSessionArray();
            ClientSocket.Address = GateShare.ServerAddr;
            ClientSocket.Port = GateShare.ServerPort;//游戏服务器(DB)的端口，此端口标准为 5000，如果使用的游戏服务器端修改过，则改为相应的端口。
            ClientSocket.Connect();
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateShare.boGateReady = true;
            GateShare.nSessionCount = 0;
            GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
            //ResUserSessionArray();
            GateShare.boServerReady = true;
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            TUserSession UserSession;
            for (int nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
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

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            GateShare.boServerReady = false;
        }

        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            string sRecvMsg = e.ReceiveText;
            GateShare.ClientSockeMsgList.Add(sRecvMsg);
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