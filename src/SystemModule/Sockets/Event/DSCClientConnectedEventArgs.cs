using System;
using System.Net;
using System.Net.Sockets;

namespace SystemModule.Sockets.Event
{
    public class DSCClientConnectedEventArgs : EventArgs
    {
        public readonly Socket Socket;
        public readonly int SocketHandle;
        public readonly IPEndPoint RemoteEndPoint;

        public DSCClientConnectedEventArgs(Socket soc)
        {
            this.Socket = soc;
            this.SocketHandle = (int)Socket.Handle;
            if (Socket.Connected)
            {
                if (soc?.RemoteEndPoint != null)
                {
                    RemoteEndPoint = (IPEndPoint)soc.RemoteEndPoint;
                }
            }
        }
    }
}