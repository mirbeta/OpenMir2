using System.IO;
using ProtoBuf;

namespace SystemModule
{
    [ProtoContract]
    public class TNakedAbility : Packets
    {
        [ProtoMember(1)]
        public ushort DC;
        [ProtoMember(2)]
        public ushort MC;
        [ProtoMember(3)]
        public ushort SC;
        [ProtoMember(4)]
        public ushort AC;
        [ProtoMember(5)]
        public ushort MAC;
        [ProtoMember(6)]
        public ushort HP;
        [ProtoMember(7)]
        public ushort MP;
        [ProtoMember(8)]
        public byte Hit;
        [ProtoMember(9)]
        public int Speed;
        [ProtoMember(10)]
        public byte X2;

        public TNakedAbility() { }

        public TNakedAbility(byte[] buff)
            : base(buff)
        {
            DC = ReadUInt16(); 
            MC = ReadUInt16();
            SC = ReadUInt16();
            AC = ReadUInt16();
            MAC = ReadUInt16();
            HP = ReadUInt16();
            MP = ReadUInt16();
            Hit = ReadByte();
            Speed = ReadInt32(); 
            X2 = ReadByte();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
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
            writer.Write(X2);
            writer.Write(0);
        }
    }
}