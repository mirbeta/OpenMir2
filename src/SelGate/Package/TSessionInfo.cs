using System.Net.Sockets;

namespace SelGate
{
    public class TSessionInfo
    {
        public Socket Socket;
        public int SocketId;
        public int nSckHandle;
        public ushort nUserListIndex;
        public int dwReceiveTick;
        public string sAccount;
        public string sChrName;
    }
}