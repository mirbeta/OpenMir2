using System.IO;

namespace GameSvr
{
    public class TNakedAbility : Package
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

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(DC);
                backingStream.Write(MC);
                backingStream.Write(SC);
                backingStream.Write(AC);
                backingStream.Write(MAC);
                backingStream.Write(HP);
                backingStream.Write(MP);
                backingStream.Write(Hit);
                backingStream.Write(Speed);
                backingStream.Write(X2);
                backingStream.Write(0);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

