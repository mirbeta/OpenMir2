using System;
using System.Net.Sockets;

namespace SystemModule.Sockets.Event
{
    public class DSCClientDataInEventArgs : EventArgs
    {
        public int BuffLen;
        public readonly byte[] Buff;
        public readonly Socket Socket;
        public int SocketId => (int)Socket.Handle;

        public DSCClientDataInEventArgs(Socket soc, byte[] buff, int buffLen)
        {
            this.Socket = soc;
            this.Buff = buff;
            this.BuffLen = buffLen;
        }
    }

    public class ClientReceiveDataEventArgs : EventArgs
    {
        public int BuffLen;
        public readonly IntPtr Buff;
        public readonly Socket Socket;
        public int SocketId => (int)Socket.Handle;

        public ClientReceiveDataEventArgs(Socket soc, IntPtr buff, int buffLen)
        {
            this.Socket = soc;
            this.Buff = buff;
            this.BuffLen = buffLen;
        }
    }
}