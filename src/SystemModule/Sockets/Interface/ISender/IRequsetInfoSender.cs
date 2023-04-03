using System;
using System.Threading.Tasks;
using SystemModule.Sockets.Exceptions;

namespace SystemModule.Sockets.Interface
{
    /// <summary>
    /// IRequsetInfoSend
    /// </summary>
    public interface IRequsetInfoSender
    {
        /// <summary>
        /// 同步发送数据。
        /// <para>内部已经封装Ssl和发送长度检测，即：调用完成即表示数据全部发送完毕。</para>
        /// <para>该发送会经过适配器封装，具体封装内容由适配器决定。</para>
        /// </summary>
        /// <param name="requestInfo">解析对象</param>
        /// <exception cref="NotConnectedException">客户端没有连接</exception>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        void Send(IRequestInfo requestInfo);

        /// <summary>
        /// 异步发送数据。
        /// <para>内部已经封装Ssl和发送长度检测，即：调用完成即表示数据全部发送完毕。</para>
        /// <para>该发送会经过适配器封装，具体封装内容由适配器决定。</para>
        /// </summary>
        /// <param name="requestInfo">解析对象</param>
        /// <exception cref="NotConnectedException">客户端没有连接</exception>
        /// <exception cref="OverlengthException">发送数据超长</exception>
        /// <exception cref="Exception">其他异常</exception>
        Task SendAsync(IRequestInfo requestInfo);
    }
}