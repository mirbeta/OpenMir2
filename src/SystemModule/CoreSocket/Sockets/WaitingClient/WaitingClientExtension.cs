namespace TouchSocket.Sockets
{
    /// <summary>
    /// WaitingClientExtensions
    /// </summary>
    public static class WaitingClientExtension
    {
        /// <summary>
        /// 获取可等待的客户端。
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="waitingOptions"></param>
        /// <returns></returns>
        public static IWaitingClient<TClient> GetWaitingClient<TClient>(this TClient client,
            WaitingOptions waitingOptions) where TClient : IClient, IDefaultSender, ISender
        {
            WaitingClient<TClient> waitingClient = new WaitingClient<TClient>(client, waitingOptions);
            return waitingClient;
        }
    }
}