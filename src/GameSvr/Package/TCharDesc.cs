using System.IO;

namespace GameSvr
{
    public struct TCharDesc
    {
        public int Feature;
        public long Status;

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

