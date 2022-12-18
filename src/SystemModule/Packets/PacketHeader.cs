using MemoryPack;
using System.IO;
using SystemModule.Packets.ClientPackets;

namespace SystemModule.Packets
{
    /// <summary>
    /// 封包消息头
    /// </summary>
    [MemoryPackable]
    public partial struct GameServerPacket 
    {
        public uint PacketCode;
        /// <summary>
        /// SocketID
        /// </summary>
        public int Socket;
        /// <summary>
        /// 会话ID
        /// </summary>
        public ushort SessionId;
        public ushort Ident;
        public int ServerIndex;
        public int PackLength;
        public string nMsg;

        public const int PacketSize = 20;
    }

    public struct ClientOutMessage 
    {
        private readonly GameServerPacket MessageHeader;
        private readonly ClientMesaagePacket clientMesaage;

        public ClientOutMessage(GameServerPacket messageHeader, ClientMesaagePacket clientMesaage)
        {
            MessageHeader = messageHeader;
            clientMesaage = clientMesaage;
        }
    }
}