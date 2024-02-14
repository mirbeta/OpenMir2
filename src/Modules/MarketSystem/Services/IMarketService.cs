using OpenMir2.Data;
using OpenMir2.Packets.ServerPackets;
using SystemModule.Actors;

namespace MarketSystem.Services
{
    public interface IMarketService
    {
        bool IsConnected { get; }

        void Start();

        void Stop();

        void CheckConnected();

        bool RequestLoadPageUserMarket(int actorId, MarKetReqInfo marKetReqInfo);

        void SendFirstMessage();

        bool SendRequest<T>(int queryId, ServerRequestMessage message, T packet);

        bool SendUserMarketSellReady(int actorId, string chrName, int marketNpc);

        void SendUserMarket(INormNpc normNpc, IPlayerActor user, short ItemType, byte UserMode);
    }
}