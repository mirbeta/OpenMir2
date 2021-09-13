using System.IO;

namespace GameSvr
{
    public class TOClientItem
    {
        public TOStdItem S;
        public int MakeIndex;
        public ushort Dura;
        public ushort DuraMax;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(S.ToByte());
                backingStream.Write(MakeIndex);
                backingStream.Write(Dura);
                backingStream.Write(DuraMax);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

