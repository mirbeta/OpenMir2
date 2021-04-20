using System.IO;

namespace M2Server
{
    public struct TDefaultMessage
    {
        public int Recog;
        public short Ident;
        public short Param;
        public short Tag;
        public short Series;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Recog);
                backingStream.Write(Ident);
                backingStream.Write(Param);
                backingStream.Write(Tag);
                backingStream.Write(Series);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }
}

