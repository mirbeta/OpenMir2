using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SelGate
{
    internal class UserSession
    {
        public string IPAddr;
        public int nIPAddr;
        public string LocalIPAddr;
        public Socket Socket;
        public int SocketId { get { return (int)Socket.Handle; } }

        public void SendBuffer(byte[] buffer)
        {
            Socket.Send(buffer);
        }
    }
}