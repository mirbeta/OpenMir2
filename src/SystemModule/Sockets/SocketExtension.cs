using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SystemModule.Sockets
{
    public static class SocketExtension
    {
        public static bool Send(this Socket socket, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            if (socket.Connected)
            {
                var buff = Encoding.GetEncoding("gb2312").GetBytes(str);
                socket.Send(buff);
                return true;
            }
            return false;
        }

        public static bool SendText(this Socket socket, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            if (socket.Connected)
            {
                var buff = Encoding.GetEncoding("gb2312").GetBytes(str);
                socket.Send(buff);
                return true;
            }
            return false;
        }

        public static bool SendBuffer(this Socket socket, byte[] buffer)
        {
            if (socket.Connected)
            {
                socket.Send(buffer);
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
            return ipEndPoint.Address.ToString();
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
