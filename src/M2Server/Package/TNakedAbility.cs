using System.IO;

namespace M2Server
{
    public class TNakedAbility : Package
    {
        public short DC;
        public short MC;
        public short SC;
        public short AC;
        public short MAC;
        public short HP;
        public short MP;
        public byte Hit;
        public int Speed;
        public byte X2;

        public TNakedAbility() { }

        public TNakedAbility(byte[] buff)
            : base(buff)
        {
            DC = ReadInt16(); //BitConverter.ToInt16(buff, 0);
            MC = ReadInt16();//BitConverter.ToInt16(buff, 2);
            SC = ReadInt16();//BitConverter.ToInt16(buff, 4);
            AC = ReadInt16();//BitConverter.ToInt16(buff, 6);
            MAC = ReadInt16();//BitConverter.ToInt16(buff, 8);
            HP = ReadInt16();//BitConverter.ToInt16(buff, 10);
            MP = ReadInt16();//BitConverter.ToInt16(buff, 12);
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

