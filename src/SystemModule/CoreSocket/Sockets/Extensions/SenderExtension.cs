using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// SenderExtension
    /// </summary>
    public static class SenderExtension
    {
        #region ISend

        /// <summary>
        /// 同步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        public static void Send<TClient>(this TClient client, byte[] buffer) where TClient : ISender
        {
            client.Send(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 同步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="byteBlock"></param>
        public static void Send<TClient>(this TClient client, ByteBlock byteBlock) where TClient : ISender
        {
            client.Send(byteBlock.Buffer, 0, byteBlock.Len);
        }

        /// <summary>
        /// 以UTF-8的编码同步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="value"></param>
        public static void Send<TClient>(this TClient client, string value) where TClient : ISender
        {
            client.Send(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 异步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        public static Task SendAsync<TClient>(this TClient client, byte[] buffer) where TClient : ISender
        {
            return client.SendAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 以UTF-8的编码异步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="value"></param>
        public static Task SendAsync<TClient>(this TClient client, string value) where TClient : ISender
        {
            return client.SendAsync(Encoding.UTF8.GetBytes(value));
        }

        #endregion ISend

        #region IDefaultSender

        /// <summary>
        /// 以UTF-8的编码同步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="value"></param>
        public static void DefaultSend<TClient>(this TClient client, string value) where TClient : IDefaultSender
        {
            client.DefaultSend(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 同步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        public static void DefaultSend<TClient>(this TClient client, byte[] buffer) where TClient : IDefaultSender
        {
            client.DefaultSend(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 同步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="byteBlock"></param>
        public static void DefaultSend<TClient>(this TClient client, ByteBlock byteBlock) where TClient : IDefaultSender
        {
            client.DefaultSend(byteBlock.Buffer, 0, byteBlock.Len);
        }

        /// <summary>
        /// 以UTF-8的编码异步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="value"></param>
        public static Task DefaultSendAsync<TClient>(this TClient client, string value) where TClient : IDefaultSender
        {
            return client.DefaultSendAsync(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 异步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        public static Task DefaultSendAsync<TClient>(this TClient client, byte[] buffer) where TClient : IDefaultSender
        {
            return client.DefaultSendAsync(buffer, 0, buffer.Length);
        }

        #endregion IDefaultSender

        #region IIDSender

        /// <summary>
        /// 以UTF-8的编码同步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void Send<TClient>(this TClient client, string id, string value) where TClient : IIDSender
        {
            client.Send(id, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 同步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="buffer"></param>
        public static void Send<TClient>(this TClient client, string id, byte[] buffer) where TClient : IIDSender
        {
            client.Send(id, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 同步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="buffer"></param>
        public static void Send<TClient>(this TClient client, string id, Memory<byte> buffer) where TClient : IIDSender
        {
            client.Send(id, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 同步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="byteBlock"></param>
        public static void Send<TClient>(this TClient client, string id, ByteBlock byteBlock) where TClient : IIDSender
        {
            client.Send(id, byteBlock.Buffer, 0, byteBlock.Len);
        }

        /// <summary>
        /// 以UTF-8的编码异步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static Task SendAsync<TClient>(this TClient client, string id, string value) where TClient : IIDSender
        {
            return client.SendAsync(id, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 异步发送数据。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="buffer"></param>
        public static Task SendAsync<TClient>(this TClient client, string id, byte[] buffer) where TClient : IIDSender
        {
            return client.SendAsync(id, buffer, 0, buffer.Length);
        }

        #endregion IIDSender

        #region IUdpDefaultSender

        /// <summary>
        /// 以UTF-8的编码同步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="endPoint"></param>
        /// <param name="value"></param>
        public static void DefaultSend<TClient>(this TClient client, EndPoint endPoint, string value) where TClient : IUdpDefaultSender
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            client.DefaultSend(endPoint, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 绕过适配器，直接发送字节流
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endPoint">目的终结点</param>
        /// <param name="buffer">数据区</param>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        public static void DefaultSend<TClient>(this TClient client, EndPoint endPoint, byte[] buffer)
            where TClient : IUdpDefaultSender
        {
            client.DefaultSend(endPoint, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 以UTF-8的编码异步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="endPoint"></param>
        /// <param name="value"></param>
        public static Task DefaultSendAsync<TClient>(this TClient client, EndPoint endPoint, string value) where TClient : IUdpDefaultSender
        {
            return client.DefaultSendAsync(endPoint, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 绕过适配器，直接发送字节流
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endPoint">目的终结点</param>
        /// <param name="buffer">数据缓存区</param>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        public static Task DefaultSendAsync<TClient>(this TClient client, EndPoint endPoint, byte[] buffer)
            where TClient : IUdpDefaultSender
        {
            return client.DefaultSendAsync(endPoint, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 绕过适配器，直接发送字节流
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endPoint">目的终结点</param>
        /// <param name="byteBlock"></param>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        public static Task DefaultSendAsync<TClient>(this TClient client, EndPoint endPoint, ByteBlock byteBlock)
            where TClient : IUdpDefaultSender
        {
            return client.DefaultSendAsync(endPoint, byteBlock.Buffer, 0, byteBlock.Len);
        }

        #endregion IUdpDefaultSender

        #region IUdpClientSender

        /// <summary>
        /// 以UTF-8的编码同步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="endPoint"></param>
        /// <param name="value"></param>
        public static void Send<TClient>(this TClient client, EndPoint endPoint, string value) where TClient : IUdpClientSender
        {
            client.Send(endPoint, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 发送字节流
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endPoint">目的终结点</param>
        /// <param name="buffer">数据区</param>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        public static void Send<TClient>(this TClient client, EndPoint endPoint, byte[] buffer)
            where TClient : IUdpClientSender
        {
            client.Send(endPoint, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 发送字节流
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endPoint">目的终结点</param>
        /// <param name="byteBlock">数据区</param>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        public static void Send<TClient>(this TClient client, EndPoint endPoint, ByteBlock byteBlock)
            where TClient : IUdpClientSender
        {
            client.Send(endPoint, byteBlock.Buffer, 0, byteBlock.Len);
        }

        /// <summary>
        /// 以UTF-8的编码异步发送字符串。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="endPoint"></param>
        /// <param name="value"></param>
        public static Task SendAsync<TClient>(this TClient client, EndPoint endPoint, string value) where TClient : IUdpClientSender
        {
            return client.SendAsync(endPoint, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 发送字节流
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endPoint">目的终结点</param>
        /// <param name="buffer">数据缓存区</param>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        public static Task SendAsync<TClient>(this TClient client, EndPoint endPoint, byte[] buffer)
            where TClient : IUdpClientSender
        {
            return client.SendAsync(endPoint, buffer, 0, buffer.Length);
        }

        #endregion IUdpClientSender
    }
}