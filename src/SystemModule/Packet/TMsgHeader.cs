using System.IO;

namespace SystemModule.Packages
{
    /// <summary>
    /// 封包消息头
    /// </summary>
    public class MessageHeader : Packets
    {
        public uint PacketCode;
        public int Socket;
        public ushort SocketIdx;
        public ushort wIdent;
        public int wUserListIndex;
        public int nLength;

        public const int PacketSize = 20;

        public MessageHeader() { }

        public MessageHeader(byte[] buffer) : base(buffer)
        {
            PacketCode = ReadUInt32();
            Socket = ReadInt32();
            SocketIdx = ReadUInt16();
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
            writer.Write(PacketCode);
            writer.Write(Socket);
            writer.Write(SocketIdx);
            writer.Write(wIdent);
            writer.Write(wUserListIndex);
            writer.Write(nLength);
        }
    }

    public class ClientOutMessage : Packets
    {
        private MessageHeader MessageHeader;
        private ClientPacket DefaultMessage;

        public ClientOutMessage(MessageHeader messageHeader, ClientPacket defaultMessage)
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