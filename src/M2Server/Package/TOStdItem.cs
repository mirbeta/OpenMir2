using System.IO;
using SystemModule;

namespace M2Server
{
    public struct TOStdItem
    {
        public string Name;
        public byte StdMode;
        public byte Shape;
        public byte Weight;
        public byte AniCount;
        public byte Source;
        public byte Reserved;
        public byte NeedIdentify;
        public short Looks;
        public short DuraMax;
        public short AC;
        public short MAC;
        public short DC;
        public short MC;
        public short SC;
        public byte Need;
        public byte NeedLevel;
        public int Price;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Name.ToByte(15));
                backingStream.Write(StdMode);
                backingStream.Write(Shape);
                backingStream.Write(Weight);
                backingStream.Write(AniCount);
                backingStream.Write(Source);
                backingStream.Write(Reserved);
                backingStream.Write(NeedIdentify);
                backingStream.Write(Looks);
                backingStream.Write(DuraMax);
                backingStream.Write(AC);
                backingStream.Write(MAC);
                backingStream.Write(DC);
                backingStream.Write(MC);
                backingStream.Write(SC);
                backingStream.Write(Need);
                backingStream.Write(NeedLevel);
                backingStream.Write(Price);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }
}

