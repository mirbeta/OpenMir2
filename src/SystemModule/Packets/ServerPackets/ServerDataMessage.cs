using MemoryPack;
using System.IO;

namespace SystemModule.Packets.ServerPackets
{
    public class ServerDataPacket : ServerPacket
    {
        /// <summary>
        /// 消息头固定大小
        /// </summary>
        public const int FixedHeaderLen = 6;

        /// <summary>
        /// 封包标识码
        /// </summary>
        public uint PacketCode { get; set; }
        /// <summary>
        /// 封包总长度
        /// </summary>
        public short PacketLen { get; set; }

        protected override void ReadPacket(BinaryReader reader)
        {
            PacketCode = reader.ReadUInt32();
            PacketLen = reader.ReadInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(PacketCode);
            writer.Write(PacketLen);
        }
    }

    [MemoryPackable]
    public partial class ServerDataMessage 
    {
        public ServerDataType Type { get; set; }
        public int SocketId { get; set; }
        public short DataLen { get; set; }
        public byte[] Data { get; set; }
    }

    public enum ServerDataType : byte
    {
        Enter = 0,
        Leave = 1,
        Data = 2,
        KeepAlive = 3
    }
}