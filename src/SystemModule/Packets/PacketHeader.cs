using MemoryPack;
using SystemModule.Packets.ClientPackets;

namespace SystemModule.Packets
{
    /// <summary>
    /// 封包消息头
    /// </summary>
    [MemoryPackable]
    public partial struct ServerMessagePacket
    {
        public uint PacketCode { get; set; }
        /// <summary>
        /// SocketID
        /// </summary>
        public int Socket { get; set; }
        /// <summary>
        /// 会话ID
        /// </summary>
        public ushort SessionId { get; set; }
        public ushort Ident { get; set; }
        public int ServerIndex { get; set; }
        public int PackLength { get; set; }

        public const int PacketSize = 20;
    }

    [MemoryPackable]
    public partial struct ClientOutMessage
    {
        public ServerMessagePacket MessagePacket;
        public ClientCommandPacket CommandPacket;
        public byte[] Data;
    }

    /// <summary>
    /// 动作消息包
    /// </summary>
    [MemoryPackable]
    public partial struct ActionOutPacket
    {
        public ServerMessagePacket MessagePacket;
        public byte[] Data;
    }
}