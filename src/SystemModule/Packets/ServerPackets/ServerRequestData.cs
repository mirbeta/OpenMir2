using MemoryPack;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class ServerRequestData
    {
        public int QueryId { get; set; }
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
    }

    [MemoryPackable]
    public partial class LoadPlayerDataPacket 
    {
        public string ChrName { get; set; }
        public PlayerDataInfo HumDataInfo { get; set; }
    }
}