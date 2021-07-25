using System.IO;

namespace M2Server
{
    public struct TDefaultMessage
    {
        public int Recog;
        public ushort Ident;
        public ushort Param;
        public ushort Tag;
        public ushort Series;

        public byte[] ToByte()
        {
            //todo  需要验证delphi下int转ushort的问题
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
