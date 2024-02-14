using OpenMir2.Packets.ServerPackets;
using SystemModule.Actors;

namespace M2Server.Net
{
    public interface INetChannel
    {
        void AddGameGateQueue(int gateIdx, ServerMessage packet, byte[] data);

        void AddGateBuffer(int gateIdx, byte[] senData);

        void CloseAllGate();

        void CloseUser(int gateIdx, int nSocket);

        void Initialize();

        void KickUser(string account, int sessionId, int payMode);

        void Run();

        void SendOutConnectMsg(int gateIdx, int nSocket, ushort nGsIdx);

        void SendServerStopMsg();

        void SetGateUserList(int gateIdx, int nSocket, IPlayerActor playObject);

        Task Start(CancellationToken cancellationToken = default);

        void Send(string connectId, byte[] buff);

        void CloseGate(string connectionId, string endPoint);

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}