namespace TouchSocket.Sockets
{
    /// <summary>
    /// 服务器辅助类接口
    /// </summary>
    public interface ISocketClient : ITcpClientBase, IClientSender, IIDSender, IIDRequsetInfoSender
    {
        /// <summary>
        /// 重新设置ID
        /// </summary>
        /// <param name="newID"></param>
        void ResetID(string newID);

        /// <summary>
        /// 用于索引的ID
        /// </summary>
        string ID { get; }

        /// <summary>
        /// 包含此辅助类的主服务器类
        /// </summary>
        TcpServiceBase Service { get; }

        /// <summary>
        /// 接收此客户端的服务器IP地址
        /// </summary>
        string ServiceIP { get; }

        /// <summary>
        /// 接收此客户端的服务器端口
        /// </summary>
        int ServicePort { get; }
    }
}