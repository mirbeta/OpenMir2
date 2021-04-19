using System;
using System.Net;
using System.Net.Sockets;

namespace NetFramework
{
    public class DSCClientConnectedEventArgs : EventArgs
    {
        public Socket socket;
        public string RemoteAddress;
        public int RemotePort;


        public DSCClientConnectedEventArgs(Socket soc)
        {
            this.socket = soc;
            var endPoint = (IPEndPoint)soc.RemoteEndPoint;
            this.RemoteAddress = endPoint.Address.ToString();
            this.RemotePort = endPoint.Port;
        }
    }
}