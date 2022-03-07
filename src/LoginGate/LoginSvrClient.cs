using SystemModule;
using SystemModule.Sockets;

namespace LoginGate
{
    public class LoginSvrClient
    {
        public IClientScoket ClientSocket;

        public LoginSvrClient()
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
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    GateShare.MainOutMessage("账号登陆服务器[" + GateShare.ServerAddr + ":" + GateShare.ServerPort + "]拒绝链接...", 1);
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    GateShare.MainOutMessage("账号登陆服务器[" + GateShare.ServerAddr + ":" + GateShare.ServerPort + "]关闭连接...", 1);
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    GateShare.MainOutMessage("账号登陆服务器[" + GateShare.ServerAddr + ":" + GateShare.ServerPort + "]链接超时...", 1);
                    break;
            }
        }

        private void ClientSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            GateShare.ClientSockeMsgList.Add(e.ReceiveText);
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