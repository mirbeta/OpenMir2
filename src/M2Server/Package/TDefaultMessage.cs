using System.IO;

namespace M2Server
{
    public class TDefaultMessage : Package
    {
        public int Recog;
        public short Ident;
        public short Param;
        public short Tag;
        public short Series;

        public static byte PacketSize = 12;
        
        public TDefaultMessage()
        {
            
        }

        public TDefaultMessage(byte[] buffer) : base(buffer)
        {
            Recog = ReadInt32();
            Ident = ReadInt16();
            Param = ReadInt16();
            Tag = ReadInt16();
            Series = ReadInt16();
        }

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
