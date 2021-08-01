using System;
using System.Net.Sockets;

namespace SystemModule.Sockets.Event
{
    public class DSCClientDataInEventArgs : EventArgs
    {
        public byte[] Buff;
        public Socket socket;
        public string Data;

        public DSCClientDataInEventArgs(Socket soc, byte[] dataIn)
        {
            this.socket = soc;
            this.Buff = dataIn;
            this.Data = System.Text.Encoding.GetEncoding("gb2312").GetString(dataIn, 0, dataIn.Length);
        }
    }
}