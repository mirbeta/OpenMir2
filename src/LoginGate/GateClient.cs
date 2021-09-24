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

        public void CheckConnected()
        {
            if (ClientSocket.IsConnected)
            {
                return;
            }
            if (ClientSocket.IsBusy)
            {
                return;
            }
            ClientSocket.Connect(GateShare.ServerAddr, GateShare.ServerPort);
        }

        private void ClientSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            GateShare.boGateReady = true;
            GateShare.nSessionCount = 0;
            GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
            //ResUserSessionArray();
            GateShare.boServerReady = true;
            GateShare.MainOutMessage($"账号登陆服务器[{e.RemoteAddress}:{e.RemotePort}]链接成功.", 1);
        }

        private void ClientSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            TUserSession UserSession;
            for (var nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = GateShare.g_SessionArray[nIndex];
                UserSession.Socket?.Close();
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
            }
            ResUserSessionArray();
            //ClientSockeMsgList.Clear();
            GateShare.boGateReady = false;
            GateShare.nSessionCount = 0;
            GateShare.MainOutMessage($"账号登陆服务器[{e.RemoteAddress}:{e.RemotePort}]断开链接.", 1);
        }

        private void ClientSocketError(object sender, DSCClientErrorEventArgs e)
        {
            GateShare.boServerReady = false;
        }

        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
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