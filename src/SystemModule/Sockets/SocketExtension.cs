using System;
using System.Net;
using System.Net.Sockets;

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
                socket.Send(HUtil32.GetBytes(str));
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
                socket.Send( HUtil32.GetBytes(str));
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
            return ((IPEndPoint)endPoint).Address.ToString();
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
