namespace SystemModule.Sockets.SocketEventArgs
{
    /// <summary>
    /// Client消息操作事件
    /// </summary>
    public class OperationEventArgs : TouchSocketEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationEventArgs()
        {
            IsPermitOperation = true;
        }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ID { get; set; }
    }
}