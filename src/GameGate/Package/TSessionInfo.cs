using System.Net.Sockets;

namespace GameGate
{
    public class TSessionInfo
    {
        public Socket Socket;
        public int SocketId;
        public int nSckHandle;
        public ushort nUserListIndex;
        public int dwReceiveTick;
    }
}