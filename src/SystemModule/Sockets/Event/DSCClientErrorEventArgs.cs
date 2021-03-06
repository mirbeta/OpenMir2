using System;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    public class DSCClientErrorEventArgs : EventArgs
    {
        public SocketException exception;
        public string RemoteAddress;
        public int RemotePort;
        public SocketError ErrorCode;

        public DSCClientErrorEventArgs(string remoteAddress, int remotePort, int errorCode, SocketException e)
        {
            this.exception = e;
            this.RemoteAddress = remoteAddress;
            this.RemotePort = remotePort;
            this.ErrorCode = (SocketError)errorCode;
        }
    }
}