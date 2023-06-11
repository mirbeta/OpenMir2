namespace ChatSystem
{
    public interface IChatService
    {
        bool IsEnableChatServer { get; }

        Task Ping();
        void SendPubChannelMessage(string sendMsg);
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}