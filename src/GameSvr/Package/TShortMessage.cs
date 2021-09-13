using System.IO;

namespace GameSvr
{
    public class TShortMessage
    {
        public ushort Ident;
        public ushort wMsg;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Ident);
                backingStream.Write(wMsg);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

