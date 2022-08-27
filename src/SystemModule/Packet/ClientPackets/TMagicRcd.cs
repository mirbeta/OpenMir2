using ProtoBuf;
using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    [ProtoContract]
    public class TMagicRcd : Packets
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        [ProtoMember(1)]
        public ushort wMagIdx;
        /// <summary>
        /// 等级
        /// </summary>
        [ProtoMember(2)]
        public byte btLevel;
        /// <summary>
        /// 技能快捷键
        /// </summary>
        [ProtoMember(3)]
        public byte btKey;
        /// <summary>
        /// 当前修练值
        /// </summary>
        [ProtoMember(4)]
        public int nTranPoint;

        protected override void ReadPacket(BinaryReader reader)
        {
            wMagIdx = reader.ReadUInt16();
            btLevel = reader.ReadByte();
            btKey = reader.ReadByte();
            nTranPoint = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(wMagIdx);
            writer.Write(btLevel);
            writer.Write(btKey);
            writer.Write(nTranPoint);
        }
    }
}