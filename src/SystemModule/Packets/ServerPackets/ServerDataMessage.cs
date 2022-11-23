using MemoryPack;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial struct ServerDataMessage 
    {
        /// <summary>
        /// 封包标识码
        /// </summary>
        public uint PacketCode { get; set; }
        /// <summary>
        /// 封包总长度
        /// </summary>
        public short PacketLen { get; set; }
        public ServerDataType Type { get; set; }
        public int SocketId { get; set; }
        public short DataLen { get; set; }
        public byte[] Data { get; set; }

        /// <summary>
        /// 消息头固定大小
        /// </summary>
        public const int FixedHeaderLen = 6;

        public short GetPacketSize()
        {
            return (short)(FixedHeaderLen + 7 + (Data?.Length ?? 5));
        }
    }

    public enum ServerDataType : byte
    {
        Enter = 0,
        Leave = 1,
        Data = 2,
        KeepAlive = 3
    }
}