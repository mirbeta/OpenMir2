using System.IO;

namespace M2Server
{
    public struct TCharDesc
    {
        public int Feature;
        public int Status;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Feature);
                backingStream.Write(Status);
                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

