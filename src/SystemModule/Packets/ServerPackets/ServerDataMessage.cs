using MemoryPack;
using System;
using System.IO;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class ServerDataMessage : Packets
    {
        public uint PacketCode { get; set; }
        public int PacketLen { get; set; }
        public ServerDataType Type { get; set; }
        public int SocketId { get; set; }
        public short DataLen { get; set; }
        public byte[] Data { get; set; }

        /// <summary>
        /// 消息头固定大小
        /// </summary>
        public const int HeaderPacketSize = 8;

        protected override void ReadPacket(BinaryReader reader)
        {
            PacketCode = reader.ReadUInt32();
            PacketLen = reader.ReadInt32();
            Type = (ServerDataType)reader.ReadByte();
            SocketId = reader.ReadInt32();
            DataLen = reader.ReadInt16();
            Data = reader.ReadBytes(DataLen);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(PacketCode);
            writer.Write(PacketLen);
            writer.Write((byte)Type);
            writer.Write(SocketId);
            if (Data == null || Data.Length <= 0)
            {
                writer.Write((short)0);
                writer.Write(Array.Empty<byte>());
            }
            else
            {
                writer.Write(DataLen);
                writer.Write(Data);
            }
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