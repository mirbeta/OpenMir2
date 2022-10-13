using System;
using System.Net.Sockets;

namespace SystemModule.Sockets.Event
{
    public class DSCClientDataInEventArgs : EventArgs
    {
        public int BuffLen => Buff.Length == 0 ? 0 : Buff.Length;
        public readonly ReadOnlyMemory<byte> Buff;
        public readonly Socket Socket;
        public int SocketId => (int)Socket.Handle;

        public DSCClientDataInEventArgs(Socket soc, ReadOnlySpan<byte> dataIn)
        {
            this.Socket = soc;
            this.Buff = dataIn.ToArray();
        }
    }
}