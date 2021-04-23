using System;
using System.IO;

namespace M2Server
{
    public class TMsgHeader : Package
    {
        public uint dwCode;
        public int nSocket;
        public short wGSocketIdx;
        public short wIdent;
        public int wUserListIndex;
        public int nLength;

        public static byte PackageSize = 20;

        public TMsgHeader()
        { }

        public TMsgHeader(byte[] buff)
            : base(buff)
        {
            dwCode = ReadUInt32();
            nSocket = ReadInt32();
            wGSocketIdx = ReadInt16();
            wIdent = ReadInt16();
            wUserListIndex = ReadInt32();
            nLength = ReadInt32();
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(dwCode);
                backingStream.Write(nSocket);
                backingStream.Write(wGSocketIdx);
                backingStream.Write(wIdent);
                backingStream.Write(wUserListIndex);
                backingStream.Write(nLength);
                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}