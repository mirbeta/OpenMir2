using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using SystemModule.CoreSocket;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 客户端扩展类
    /// </summary>
    public static class ClientExtension
    {
        /// <summary>
        /// 获取相关信息。格式：
        ///<para>IPPort=IP:Port,ID=id,Protocol=Protocol</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string GetInfo<T>(this T client) where T : ISocketClient
        {
            return $"IP&Port={client.IP}:{client.Port},ID={client.ID},Protocol={client.Protocol}";
        }

        /// <summary>
        /// 获取服务器中，除自身以外的所有客户端id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetOtherIDs<T>(this T client) where T : ISocketClient
        {
            return client.Service.GetIDs().Where(id => id != client.ID);
        }

        /// <summary>
        /// 获取最后活动时间。即<see cref="IClient.LastReceivedTime"/>与<see cref="IClient.LastSendTime"/>的最近值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <returns></returns>
        public static DateTime GetLastActiveTime<T>(this T client) where T : IClient
        {
            return client.LastSendTime > client.LastReceivedTime ? client.LastSendTime : client.LastReceivedTime;
        }

        /// <summary>
        /// 安全性发送关闭报文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="how"></param>
        public static bool TryShutdown<T>(this T client, SocketShutdown how = SocketShutdown.Both)
            where T : ITcpClientBase
        {
            try
            {
                if (!client.MainSocket.Connected)
                {
                    return false;
                }

                client?.MainSocket?.Shutdown(how);
                return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// 安全性关闭。不会抛出异常。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public static void SafeClose<T>(this T client, string msg) where T : ITcpClientBase
        {
            try
            {
                client.Close(msg);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取IP和端口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string GetIPPort<T>(this T client) where T : ITcpClientBase
        {
            return $"{client.IP}:{client.Port}";
        }

        #region 连接

        /// <summary>
        /// 尝试连接。不会抛出异常。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static Result TryConnect<TClient>(this TClient client, int timeout = 5000) where TClient : ITcpClient
        {
            try
            {
                client.Connect(timeout);
                return new Result(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return new Result(ResultCode.Exception, ex.Message);
            }
        }

        /// <summary>
        /// 尝试连接。不会抛出异常。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<Result> TryConnectAsync<TClient>(this TClient client, int timeout = 5000)
            where TClient : ITcpClient
        {
            try
            {
                await client.ConnectAsync(timeout);
                return new Result(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return new Result(ResultCode.Exception, ex.Message);
            }
        }

        #endregion 连接
    }
}