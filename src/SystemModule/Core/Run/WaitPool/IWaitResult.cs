namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 等待返回类
    /// </summary>
    public interface IWaitResult
    {
        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 标记
        /// </summary>
        long Sign { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        byte Status { get; set; }
    }
}