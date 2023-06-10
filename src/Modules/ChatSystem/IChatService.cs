namespace GameSrv.Services
{
    public interface IChatService
    {
        bool IsEnableChatServer { get; }

        Task Ping();
        void SendPubChannelMessage(string sendMsg);
        Task Start();
        Task Stop();
    }
}