using System;
using System.Net;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    public class DSCClientConnectedEventArgs : EventArgs
    {
        private Socket socket;
        public string RemoteAddress;
        public int RemotePort;

        public DSCClientConnectedEventArgs(Socket soc)
        {
            if (soc.Connected == false)
            {
                return;
            }
            this.socket = soc;
            if (soc.RemoteEndPoint != null)
            {
                var endPoint = (IPEndPoint)soc.RemoteEndPoint;
                if (endPoint != null)
                {
                    this.RemoteAddress = endPoint.Address?.ToString();
                    this.RemotePort = endPoint.Port;
                }
            }
        }
    }
}