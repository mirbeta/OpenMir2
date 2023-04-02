using System.Net;

namespace SystemModule.Extensions
{
    /// <summary>
    /// 其他扩展
    /// </summary>
    public static class SystemNetExtension
    {
        /// <summary>
        /// 从<see cref="EndPoint"/>中获得IP地址。
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static string GetIP(this EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Address.ToString();
        }

        /// <summary>
        /// 从<see cref="EndPoint"/>中获得Port。
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static int GetPort(this EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Port;
        }
    }
}