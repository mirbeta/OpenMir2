using System;
using System.Net;
using System.Net.Sockets;

namespace SystemModule.SocketComponents.Event
{
    public class DSCClientErrorEventArgs : EventArgs
    {
        public Exception exception;
        public IPEndPoint EndPoint;
        public SocketError ErrorCode;

        public DSCClientErrorEventArgs(EndPoint endPoint, SocketError errorCode, Exception e)
        {
            this.exception = e;
            this.EndPoint = (IPEndPoint)endPoint;
            this.ErrorCode = errorCode;
        }
    }
}