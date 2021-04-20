using System.IO;

namespace M2Server
{
    public class TMessageBodyW
    {
        public short Param1;
        public short Param2;
        public short Tag1;
        public short Tag2;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Param1);
                backingStream.Write(Param2);
                backingStream.Write(Tag1);
                backingStream.Write(Tag2);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

