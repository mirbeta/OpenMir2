using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SystemModule.Sockets.Exceptions;

namespace SystemModule.Sockets.Interface
{
    /// <summary>
    /// 具有发送功能的接口
    /// </summary>
    public interface ISender : ISenderBase
    {
        /// <summary>
        /// 同步发送数据。
        /// <para>内部已经封装Ssl和发送长度检测，即：调用完成即表示数据全部发送完毕。</para>
        /// <para>该发送会经过适配器封装，具体封装内容由适配器决定。</para>
        /// </summary>
        /// <param name="buffer">数据缓存区</param>
        /// <param name="offset">偏移量</param>
        /// <param name="length">数据长度</param>
        /// <exception cref="NotConnectedException">客户端没有连接</exception>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        void Send(byte[] buffer, int offset, int length);

        /// <summary>
        /// 同步发送数据。
        /// <para>内部已经封装Ssl和发送长度检测，即：调用完成即表示数据全部发送完毕。</para>
        /// <para>该发送会经过适配器封装，具体封装内容由适配器决定。</para>
        /// </summary>
        /// <param name="buffer">数据缓存区</param>
        /// <param name="offset">偏移量</param>
        /// <param name="length">数据长度</param>
        /// <exception cref="NotConnectedException">客户端没有连接</exception>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        void Send(ReadOnlyMemory<byte> buffer, int offset, int length);

        /// <summary>
        /// 异步发送数据。
        /// <para>在<see cref="ITcpClient"/>时，如果使用独立线程发送，则不会触发异常。</para>
        /// <para>在<see cref="ITcpClientBase"/>时，相当于<see cref="Socket.BeginSend(byte[], int, int, SocketFlags, out SocketError, AsyncCallback, object)"/>。</para>
        /// <para>该发送会经过适配器封装，具体封装内容由适配器决定。</para>
        /// </summary>
        /// <param name="buffer">数据缓存区</param>
        /// <param name="offset">偏移量</param>
        /// <param name="length">数据长度</param>
        /// <exception cref="NotConnectedException">客户端没有连接</exception>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        Task SendAsync(byte[] buffer, int offset, int length);
    }
}