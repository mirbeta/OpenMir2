using System.IO;
using ProtoBuf;

namespace SystemModule.Packet.ServerPackets
{
    [ProtoContract]
    public class TMagicRcd : Packets
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        [ProtoMember(1)]
        public ushort MagIdx;
        /// <summary>
        /// 等级
        /// </summary>
        [ProtoMember(2)]
        public byte Level;
        /// <summary>
        /// 技能快捷键
        /// </summary>
        [ProtoMember(3)]
        public char MagicKey;
        /// <summary>
        /// 当前修练值
        /// </summary>
        [ProtoMember(4)]
        public int TranPoint;

        protected override void ReadPacket(BinaryReader reader)
        {
            MagIdx = reader.ReadUInt16();
            Level = reader.ReadByte();
            MagicKey = reader.ReadChar();
            TranPoint = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MagIdx);
            writer.Write(Level);
            writer.Write(MagicKey);
            writer.Write(TranPoint);
        }
    }
}