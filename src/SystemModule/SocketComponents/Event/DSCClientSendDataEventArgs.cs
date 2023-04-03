using System;
using System.Net.Sockets;

namespace SystemModule.SocketComponents.Event
{
    public class DSCClientSendDataEventArgs : EventArgs
    {
        public int BuffLen;
        public readonly Socket Socket;
        public int SocketId => (int)Socket.Handle;

        public DSCClientSendDataEventArgs(Socket soc, int buffLen)
        {
            this.Socket = soc;
            this.BuffLen = buffLen;
        }
    }
}