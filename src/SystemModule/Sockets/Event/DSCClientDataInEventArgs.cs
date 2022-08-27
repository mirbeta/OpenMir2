using System;
using System.Net.Sockets;

namespace SystemModule.Sockets.Event
{
    public class DSCClientDataInEventArgs : EventArgs
    {
        public int BuffLen => Buff == null ? 0 : Buff.Length;
        public readonly byte[] Buff;
        public readonly Socket Socket;
        public int SocketId => (int)Socket.Handle;

        public DSCClientDataInEventArgs(Socket soc, byte[] dataIn)
        {
            this.Socket = soc;
            this.Buff = dataIn;
        }
    }
}