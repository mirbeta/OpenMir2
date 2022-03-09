using System.Net.Sockets;

namespace GameGate
{
    public class TSessionInfo
    {
        public Socket Socket;
        public int SessionId;
        public int SckHandle;
        public ushort nUserListIndex;
        public int dwReceiveTick;
        public string sAccount;
        public string sChrName;
    }
}