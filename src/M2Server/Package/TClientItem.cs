using System.IO;

namespace M2Server
{
    public class TClientItem
    {
        public TStdItem S;
        public int MakeIndex;
        public short Dura;
        public short DuraMax;

        public TClientItem()
        {
            S = new TStdItem();
        }

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

