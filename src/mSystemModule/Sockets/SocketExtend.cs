using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace mSystemModule.Sockets
{
    public static class SocketExtend
    {
        public static bool Send(this Socket socket, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            if (socket.Connected)
            {
                var buff = System.Text.Encoding.Default.GetBytes(str);
                socket.Send(buff);
                return true;
            }
            return false;
        }

        public static bool SendText(this Socket socket,string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            if (socket.Connected)
            {
                var buff = System.Text.Encoding.Default.GetBytes(str);
                socket.Send(buff);
                return true;
            }
            return false;
        }

        public static string GetIPAddress(this EndPoint endPoint)
        {
            if (endPoint == null)
            {
                throw new Exception("endPoint is null");
            }
            var ipEndPoint = ((IPEndPoint)endPoint);
            return ipEndPoint.ToString();
        }

        public static int GetPort(this EndPoint endPoint)
        {
            if (endPoint == null)
            {
                throw new Exception("endPoint is null");
            }
            var ipEndPoint = ((IPEndPoint)endPoint);
            return ipEndPoint.Port;
        }
    }
}
