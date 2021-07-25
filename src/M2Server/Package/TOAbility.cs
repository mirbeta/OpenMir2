using System.IO;

namespace M2Server
{
    public class TOAbility
    {
        public ushort Level;
        public ushort AC;
        public ushort MAC;
        public ushort DC;
        public ushort MC;
        public ushort SC;
        public ushort HP;
        public ushort MP;
        public ushort MaxHP;
        public ushort MaxMP;
        public int dw1AC;
        public int Exp;
        public int MaxExp;
        public ushort Weight;
        public ushort MaxWeight;
        public byte WearWeight;
        public byte MaxWearWeight;
        public byte HandWeight;
        public byte MaxHandWeight;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Level);
                backingStream.Write(AC);
                backingStream.Write(MAC);
                backingStream.Write(DC);
                backingStream.Write(MC);
                backingStream.Write(SC);
                backingStream.Write(HP);
                backingStream.Write(MP);
                backingStream.Write(MaxHP);
                backingStream.Write(MaxMP);
                backingStream.Write(Exp);
                backingStream.Write(MaxExp);
                backingStream.Write(Weight);
                backingStream.Write(MaxWeight);
                backingStream.Write(WearWeight);
                backingStream.Write(MaxWearWeight);
                backingStream.Write(HandWeight);
                backingStream.Write(MaxHandWeight);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }
}

