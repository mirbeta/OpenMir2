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

        public MessageHeader(byte[] buffer) : base(buffer)
        {
            dwCode = ReadUInt32();
            nSocket = ReadInt32();
            wGSocketIdx = ReadUInt16();
            wIdent = ReadUInt16();
            wUserListIndex = ReadInt32();
            nLength = ReadInt32();
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

    public class ClientOutMessage : Packets
    {
        private MessageHeader MessageHeader;
        private TDefaultMessage DefaultMessage;
        
        public ClientOutMessage(MessageHeader messageHeader,TDefaultMessage defaultMessage)
        {
            MessageHeader = messageHeader;
            DefaultMessage = defaultMessage;
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            var nLen = MessageHeader.nLength + 20;
            writer.Write(nLen);
            writer.Write(MessageHeader.GetPacket());
            writer.Write(DefaultMessage.GetPacket());
        }
    }
}