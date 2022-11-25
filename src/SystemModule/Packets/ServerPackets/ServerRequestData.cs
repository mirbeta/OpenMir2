using MemoryPack;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class ServerRequestData : ServerPacket
    {
        public uint PacketCode { get; set; }
        public int QueryId { get; set; }
        public short PacketLen { get; set; }
        /// <summary>
        /// 消息头
        /// </summary>
        public byte[] Message { get; set; }
        /// <summary>
        /// 消息封包
        /// </summary>
        public byte[] Packet { get; set; }
        /// <summary>
        /// 验签
        /// </summary>
        public byte[] Sgin { get; set; }
        
        public const int HeaderMessageSize = 10;

        public override int GetPacketSize()
        {
            return HeaderMessageSize + Message.Length + Packet.Length + Sgin.Length;
        }
    }

    [MemoryPackable]
    public partial class LoadPlayerDataPacket : ServerPacket
    {
        public string ChrName { get; set; }
        public PlayerDataInfo HumDataInfo { get; set; }

        public override int GetPacketSize()
        {
            throw new System.NotImplementedException();
        }
    }
}