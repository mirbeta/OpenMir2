using SystemModule.Data;
using SystemModule.Packets.ServerPackets;

namespace MarketSystem
{
    public interface IMarketService
    {
        bool IsConnected { get; }

        void CheckConnected();
        bool RequestLoadPageUserMarket(int actorId, MarKetReqInfo marKetReqInfo);
        void SendFirstMessage();
        bool SendRequest<T>(int queryId, ServerRequestMessage message, T packet);
        bool SendUserMarketSellReady(int actorId, string chrName, int marketNpc);
        void Start();
        void Stop();
    }
}