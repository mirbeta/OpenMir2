using System;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    public class DSCClientDataInEventArgs : EventArgs
    {
        public int BuffLen => Buff == null ? 0 : Buff.Length;
        public byte[] Buff;
        public Socket socket;
        public string ReceiveText;
        public int SocketId => (int)socket.Handle;

        public DSCClientDataInEventArgs(Socket soc, byte[] dataIn)
        {
            this.socket = soc;
            this.Buff = dataIn;
            this.ReceiveText = System.Text.Encoding.GetEncoding("gb2312").GetString(dataIn, 0, dataIn.Length);
        }
    }
}