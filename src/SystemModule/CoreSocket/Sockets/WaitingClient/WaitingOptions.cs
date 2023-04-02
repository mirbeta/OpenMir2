using SystemModule.CoreSocket.Common;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 适配器筛选
    /// </summary>
    public enum AdapterFilter
    {
        /// <summary>
        /// 发送和接收都经过适配器
        /// </summary>
        AllAdapter,

        /// <summary>
        /// 发送经过适配器，接收不经过
        /// </summary>
        SendAdapter,

        /// <summary>
        /// 发送不经过适配器，接收经过
        /// </summary>
        WaitAdapter,

        /// <summary>
        /// 全都不经过适配器。
        /// </summary>
        NoneAll
    }

    /// <summary>
    /// 等待设置
    /// </summary>
    public class WaitingOptions
    {
        /// <summary>
        /// 适配器筛选
        /// </summary>
        public AdapterFilter AdapterFilter { get; set; } = AdapterFilter.AllAdapter;

        /// <summary>
        /// 当Client为Tcp系时。是否在断开连接时立即触发结果。默认会返回null。当<see cref="ThrowBreakException"/>为<see langword="true"/>时，会触发异常。
        /// </summary>
        public bool BreakTrigger { get; set; }

        /// <summary>
        /// 远程地址(仅在Udp模式下生效)
        /// </summary>
        public IPHost RemoteIPHost { get; set; }

        /// <summary>
        /// 当Client为Tcp系时。是否在断开连接时以异常返回结果。
        /// </summary>
        public bool ThrowBreakException { get; set; } = true;
    }
}