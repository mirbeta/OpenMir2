using MemoryPack;
using System;
using System.IO;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class ServerRequestData : Packets
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

        private const int ByteSize = 1 + 4 + 4 + 2 + 2 + 2 + 1;
        
        public const int HeaderMessageSize = 10;
        
        public ServerRequestData()
        {

        }

        protected override void ReadPacket(BinaryReader reader)
        {
            PacketCode = reader.ReadUInt32();
            QueryId = reader.ReadInt32();
            PacketLen = reader.ReadInt16();
            var msgLen = reader.ReadUInt16();
            if (msgLen > 0)
            {
                Message = reader.ReadBytes(msgLen);
            }
            var packLen = reader.ReadUInt16();
            if (packLen > 0)
            {
                Packet = reader.ReadBytes(packLen);
            }
            else
            {
                Packet = Array.Empty<byte>();
            }
            var checkLen = reader.ReadUInt16();
            if (checkLen > 0)
            {
                Sgin = reader.ReadBytes(checkLen);
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            PacketLen = (short)(Message.Length + Packet.Length + Sgin.Length + ByteSize);
            writer.Write(PacketCode);
            writer.Write(QueryId);
            writer.Write(PacketLen);
            writer.Write((ushort)Message.Length);
            writer.Write(Message, 0, Message.Length);
            writer.Write((ushort)Packet.Length);
            writer.Write(Packet, 0, Packet.Length);
            writer.Write((ushort)Sgin.Length);
            writer.Write(Sgin, 0, Sgin.Length);
        }
    }

    [MemoryPackable]
    public partial class LoadPlayerDataPacket
    {
        public string ChrName { get; set; }
        public PlayerDataInfo HumDataInfo { get; set; }
    }
}