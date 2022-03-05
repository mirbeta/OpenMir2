using System.IO;

namespace SystemModule.Packages
{
    /// <summary>
    /// 封包消息头
    /// </summary>
    public class MessageHeader : Packets
    {
        public uint dwCode;
        public int nSocket;
        public ushort wGSocketIdx;
        public ushort wIdent;
        public int wUserListIndex;
        public int nLength;

        public const int PacketSize = 20;

        public MessageHeader() { }

        public MessageHeader(byte[] buffer)
        {
            using var binaryReader = new BinaryReader(new MemoryStream(buffer));
            dwCode = binaryReader.ReadUInt32();
            nSocket = binaryReader.ReadInt32();
            wGSocketIdx = binaryReader.ReadUInt16();
            wIdent = binaryReader.ReadUInt16();
            wUserListIndex = binaryReader.ReadInt32();
            nLength = binaryReader.ReadInt32();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(dwCode);
            writer.Write(nSocket);
            writer.Write(wGSocketIdx);
            writer.Write(wIdent);
            writer.Write(wUserListIndex);
            writer.Write(nLength);
        }
    }
}