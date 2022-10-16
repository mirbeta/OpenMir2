using System;
using System.Net;

namespace SystemModule.Sockets.Event
{
    public class DSCClientErrorEventArgs : EventArgs
    {
        public Exception exception;
        public IPEndPoint EndPoint;
        public int ErrorCode;

        public DSCClientErrorEventArgs(EndPoint endPoint, int errorCode, Exception e)
        {
            this.exception = e;
            this.EndPoint = (IPEndPoint)endPoint;
            this.ErrorCode = errorCode;
        }
    }
}