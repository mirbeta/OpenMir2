using System;
using System.Net.Sockets;

namespace SystemModule.Sockets.Event
{
    public class DSCClientDataInEventArgs : EventArgs
    {
        public byte[] Buff;
        public Socket socket;
        public string Data;

        public DSCClientDataInEventArgs(Socket soc, byte[] datain)
        {
            this.socket = soc;
            this.Buff = datain;
            this.Data = System.Text.Encoding.GetEncoding("gb2312").GetString(datain, 0, datain.Length);
        }
    }
}