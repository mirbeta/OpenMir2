using System.Net.Sockets;

namespace SelGate
{
    public class TSessionInfo
    {
        public Socket Socket;
        public int SocketId;
        public int dwReceiveTick;
        public string sAccount;
        public string sChrName;
        public string ClientIP;
    }
}