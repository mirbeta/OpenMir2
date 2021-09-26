using System.IO;

namespace SystemModule.Packages
{
    public struct TDefaultMessage
    {
        public int Recog;
        public ushort Ident;
        public ushort Param;
        public ushort Tag;
        public ushort Series;

        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(Recog);
            backingStream.Write(Ident);
            backingStream.Write(Param);
            backingStream.Write(Tag);
            backingStream.Write(Series);
            var stream = backingStream.BaseStream as MemoryStream;
            return stream?.ToArray();
        }
        
        public TDefaultMessage(byte[] buff)
        {
            var binaryReader = new BinaryReader(new MemoryStream(buff));
            Recog = binaryReader.ReadInt32();
            Ident = binaryReader.ReadUInt16();
            Param = binaryReader.ReadUInt16();
            Tag = binaryReader.ReadUInt16();
            Series = binaryReader.ReadUInt16();
        }
    }
}