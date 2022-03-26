using ProtoBuf;
using System.IO;

namespace SystemModule
{
    [ProtoContract]
    public class RequestServerPacket : Packets
    {
        private int _packLen = 0;
        
        [ProtoMember(1)]
        public int PacketLen
        {
            get
            {
                var len = QueryId + Message?.Length + Packet?.Length + CheckBody?.Length;
                return len > 0 ? len.Value + 2 : 0;
            }
            private set => value = _packLen;
        }

        [ProtoMember(2)] 
        public int QueryId { get; set; }
        /// <summary>
        /// 消息头,需要自行解密
        /// </summary>
        [ProtoMember(3)] 
        public byte[] Message { get; set; }
        /// <summary>
        /// 消息封包，需要自行调用解密
        /// </summary>
        [ProtoMember(4)]
        public byte[] Packet { get; set; }
        /// <summary>
        /// 已经解密，无需在解密
        /// </summary>
        [ProtoMember(5)]
        public byte[] CheckBody { get; set; }

        public RequestServerPacket()
        {

        }

        protected override void ReadPacket(BinaryReader reader)
        {
            _packLen = reader.ReadInt32();
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
            var checkLen = reader.ReadUInt16();
            if (checkLen > 0)
            {
                CheckBody = reader.ReadBytes(checkLen);
                CheckBody = EDcode.DecodeBuff(CheckBody);
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte) '#');
            writer.Write(PacketLen);
            writer.Write(QueryId);
            writer.Write((ushort) Message.Length);
            writer.Write(Message, 0, Message.Length);
            writer.Write((ushort) Packet.Length);
            writer.Write(Packet, 0, Packet.Length);
            writer.Write((ushort) CheckBody.Length);
            writer.Write(CheckBody, 0, CheckBody.Length);
            writer.Write((byte) '!');
        }
    }

    [ProtoContract]
    public class LoadHumanRcdResponsePacket
    {
        [ProtoMember(1)]
        public string sChrName { get; set; }
        [ProtoMember(2)]
        public THumDataInfo HumDataInfo { get; set; }
    }
}