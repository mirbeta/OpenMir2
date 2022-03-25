using System.IO;
using ProtoBuf;

namespace SystemModule
{
    [ProtoContract]
    public class RequestServerPacket : Packets
    {
        [ProtoMember(1)] public byte[] QueryId;
        [ProtoMember(2)] public byte[] PacketHead;
        [ProtoMember(3)] public byte[] PacketBody;

        public RequestServerPacket()
        {

        }
        
        public RequestServerPacket(byte[] data) : base(data)
        {

        }

        protected override void ReadPacket(BinaryReader reader)
        {
            var queryLen = reader.ReadUInt16();
            QueryId = reader.ReadBytes(queryLen);
            var packetLen = reader.ReadUInt16();
            if (packetLen > 0)
            {
                PacketHead = reader.ReadBytes(packetLen);
                PacketHead = EDcode.DecodeBuff(PacketHead);
            }
            var bodyLen = reader.ReadUInt16();
            if (bodyLen > 0)
            {
                PacketBody = reader.ReadBytes(bodyLen);
                PacketBody = EDcode.DecodeBuff(PacketBody);
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte) '#');
            writer.Write((ushort) QueryId.Length);
            writer.Write(QueryId, 0, QueryId.Length);
            writer.Write((ushort) PacketHead.Length);
            writer.Write(PacketHead, 0, PacketHead.Length);
            writer.Write((ushort) PacketBody.Length);
            writer.Write(PacketBody, 0, PacketBody.Length);
            writer.Write((byte) '!');
        }
    }

    [ProtoContract]
    public class ResponseServerPacket<T> : Packets
    {
        [ProtoMember(1)] public string QueryId;
        [ProtoMember(2)] public ServerMessagePacket PacketHead;
        [ProtoMember(3)] public T PacketBody;
        
        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }


    public class LoadHumanRcdResponsePacket
    {
        public string sChrName { get; set; }
        public THumDataInfo HumDataInfo { get; set; }
    }
}