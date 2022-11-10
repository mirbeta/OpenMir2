using ProtoBuf;
using System;
using System.IO;

namespace SystemModule.Packet.ServerPackets
{
    [ProtoContract]
    public class ServerRequestData : Packets
    {
        private readonly int packlen;

        [ProtoMember(1)]
        public int? PacketLen
        {
            get => (Message?.Length ?? 0) + (Packet?.Length ?? 0) + (Sgin?.Length ?? 0) + ByteSize;
            private set => value = packlen;
        }

        [ProtoMember(2)]
        public int QueryId { get; set; }
        /// <summary>
        /// 消息头
        /// </summary>
        [ProtoMember(3)]
        public byte[] Message { get; set; }
        /// <summary>
        /// 消息封包
        /// </summary>
        [ProtoMember(4)]
        public byte[] Packet { get; set; }
        /// <summary>
        /// 验签
        /// </summary>
        [ProtoMember(5)]
        public byte[] Sgin { get; set; }

        private const int ByteSize = 1 + 4 + 4 + 2 + 2 + 2 + 1;

        public ServerRequestData()
        {

        }

        protected override void ReadPacket(BinaryReader reader)
        {
            reader.ReadByte();//#
            PacketLen = reader.ReadInt32();
            QueryId = reader.ReadInt32();
            var msgLen = reader.ReadUInt16();
            if (msgLen > 0)
            {
                Message = reader.ReadBytes(msgLen);
                Message = Message;
            }
            var packLen = reader.ReadUInt16();
            if (packLen > 0)
            {
                Packet = reader.ReadBytes(packLen);
                Packet = Packet;
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
            reader.ReadByte();//!
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)'#');
            writer.Write(PacketLen.Value);
            writer.Write(QueryId);
            writer.Write((ushort)Message.Length);
            writer.Write(Message, 0, Message.Length);
            writer.Write((ushort)Packet.Length);
            writer.Write(Packet, 0, Packet.Length);
            writer.Write((ushort)Sgin.Length);
            writer.Write(Sgin, 0, Sgin.Length);
            writer.Write((byte)'!');
        }
    }

    [ProtoContract]
    public class LoadPlayerDataPacket
    {
        [ProtoMember(1)]
        public string ChrName { get; set; }
        [ProtoMember(2)]
        public PlayerDataInfo HumDataInfo { get; set; }
    }
}