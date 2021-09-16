using System;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    internal class DSCClientDisconnectedEventArgs : EventArgs
    {
        public Socket socket;

        public DSCClientDisconnectedEventArgs(Socket soc)
        {
            this.socket = soc;
        }
    }
}