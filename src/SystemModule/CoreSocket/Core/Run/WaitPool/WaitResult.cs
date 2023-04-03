using System;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 等待返回类
    /// </summary>
    [Serializable]
    public class WaitResult : IWaitResult
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 标记号
        /// </summary>
        public long Sign { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public byte Status { get; set; }
    }
}