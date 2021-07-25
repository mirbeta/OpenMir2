using System;
using System.IO;

namespace M2Server
{
    public struct TMsgHeader
    {
        public uint dwCode;
        public int nSocket;
        public ushort wGSocketIdx;
        public ushort wIdent;
        public int wUserListIndex;
        public int nLength;

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