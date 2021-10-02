using System.IO;

namespace GameSvr
{
    public class TMessageBodyW
    {
        public ushort Param1;
        public ushort Param2;
        public ushort Tag1;
        public ushort Tag2;

        public byte[] GetPacket()
        {
            using MemoryStream memoryStream = new MemoryStream();
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

