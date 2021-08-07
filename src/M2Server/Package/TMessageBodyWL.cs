using System.IO;

namespace M2Server
{
    public class TMessageBodyWL
    {
        public int lParam1;
        public long lParam2;
        public int lTag1;
        public int lTag2;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(lParam1);
                backingStream.Write(lParam2);
                backingStream.Write(lTag1);
                backingStream.Write(lTag2);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

