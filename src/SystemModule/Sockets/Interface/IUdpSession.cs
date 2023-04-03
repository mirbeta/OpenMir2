using SystemModule.Sockets.DataAdapter.Udp;

namespace SystemModule.Sockets.Interface
{
    /// <summary>
    /// UDP会话
    /// </summary>
    public interface IUdpSession : IService, IClient, IClientSender, IUdpClientSender, IDefaultSender, IUdpDefaultSender
    {
        /// <summary>
        /// 缓存池大小
        /// </summary>
        int BufferLength { get; }

        /// <summary>
        /// 是否允许自由调用<see cref="SetDataHandlingAdapter"/>进行赋值。
        /// </summary>
        bool CanSetDataHandlingAdapter { get; }

        /// <summary>
        /// 数据处理适配器
        /// </summary>
        UdpDataHandlingAdapter DataHandlingAdapter { get; }

        /// <summary>
        /// 设置数据处理适配器
        /// </summary>
        /// <param name="adapter"></param>
        void SetDataHandlingAdapter(UdpDataHandlingAdapter adapter);
    }
}