using MemoryPack;
using System.IO;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class MagicRcd : Packets
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public ushort MagIdx;
        /// <summary>
        /// 等级
        /// </summary>
        public byte Level;
        /// <summary>
        /// 技能快捷键
        /// </summary>
        public char MagicKey;
        /// <summary>
        /// 当前修练值
        /// </summary>
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