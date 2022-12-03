using System.IO;
using MemoryPack;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    public partial class NakedAbility : ClientPackage
    {
        public ushort DC{ get; set; }
        public ushort MC{ get; set; }
        public ushort SC{ get; set; }
        public ushort AC{ get; set; }
        public ushort MAC{ get; set; }
        public ushort HP{ get; set; }
        public ushort MP{ get; set; }
        public byte Hit{ get; set; }
        public int Speed{ get; set; }
        public byte Reserved{ get; set; }

        protected override void ReadPacket(BinaryReader reader)
        {
            DC = reader.ReadUInt16();
            MC = reader.ReadUInt16();
            SC = reader.ReadUInt16();
            AC = reader.ReadUInt16();
            MAC = reader.ReadUInt16();
            HP = reader.ReadUInt16();
            MP = reader.ReadUInt16();
            Hit = reader.ReadByte();
            Speed = reader.ReadInt32();
            Reserved = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(DC);
            writer.Write(MC);
            writer.Write(SC);
            writer.Write(AC);
            writer.Write(MAC);
            writer.Write(HP);
            writer.Write(MP);
            writer.Write(Hit);
            writer.Write(Speed);
            writer.Write(Reserved);
            writer.Write(0);
        }
    }
}