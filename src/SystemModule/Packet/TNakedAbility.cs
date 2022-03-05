using System.IO;

namespace SystemModule
{
    public class TNakedAbility : Packets
    {
        public ushort DC;
        public ushort MC;
        public ushort SC;
        public ushort AC;
        public ushort MAC;
        public ushort HP;
        public ushort MP;
        public byte Hit;
        public int Speed;
        public byte X2;

        public TNakedAbility() { }

        public TNakedAbility(byte[] buff)
            : base(buff)
        {
            DC = ReadUInt16(); //BitConverter.ToInt16(buff, 0);
            MC = ReadUInt16();//BitConverter.ToInt16(buff, 2);
            SC = ReadUInt16();//BitConverter.ToInt16(buff, 4);
            AC = ReadUInt16();//BitConverter.ToInt16(buff, 6);
            MAC = ReadUInt16();//BitConverter.ToInt16(buff, 8);
            HP = ReadUInt16();//BitConverter.ToInt16(buff, 10);
            MP = ReadUInt16();//BitConverter.ToInt16(buff, 12);
            Hit = ReadByte(); //buff[13];
            Speed = ReadInt32(); //BitConverter.ToInt32(buff, 14);
            X2 = ReadByte(); //buff[20];
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