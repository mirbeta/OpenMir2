using System.IO;

namespace SystemModule.Packages
{
    public struct TMsgHeader
    {
        public uint dwCode;
        public int nSocket;
        public ushort wGSocketIdx;
        public ushort wIdent;
        public int wUserListIndex;
        public int nLength;

        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(dwCode);
            backingStream.Write(nSocket);
            backingStream.Write(wGSocketIdx);
            backingStream.Write(wIdent);
            backingStream.Write(wUserListIndex);
            backingStream.Write(nLength);
            var stream = backingStream.BaseStream as MemoryStream;
            return stream?.ToArray();
        }

        public TMsgHeader(byte[] buff)
        {
            var binaryReader = new BinaryReader(new MemoryStream(buff));
            dwCode = binaryReader.ReadUInt32();
            nSocket = binaryReader.ReadInt32();
            wGSocketIdx = binaryReader.ReadUInt16();
            wIdent = binaryReader.ReadUInt16();
            wUserListIndex = binaryReader.ReadInt32();
            nLength = binaryReader.ReadInt32();
        }
    }
}