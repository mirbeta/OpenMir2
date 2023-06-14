using SystemModule;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets.Components.TCP;

namespace M2Server
{
    public interface IThreadSocket
    {
        void AddGameGateQueue(int gateIdx, ServerMessage packet, byte[] data);
        void AddGateBuffer(int gateIdx, byte[] senData);
        void CloseAllGate();
        void CloseUser(int gateIdx, int nSocket);
        void Initialize();
        void KickUser(string sAccount, int sessionId, int payMode);
        void Run();
        void SendOutConnectMsg(int gateIdx, int nSocket, ushort nGsIdx);
        void SendServerStopMsg();
        void SetGateUserList(int gateIdx, int nSocket, IPlayerActor playObject);
        void Start();
        void Send(string connectId, byte[] buff);
        void CloseGate(string connectionId, string endPoint);
        void AddGate(SocketClient e);
        Task StartMessageThread(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}