using System.IO;

namespace SystemModule.Packages
{
    /// <summary>
    /// 封包消息头
    /// </summary>
    public class PacketHeader : Packets
    {
        public uint PacketCode;
        public int Socket;
        public ushort SocketIdx;
        public ushort Ident;
        public int UserIndex;
        public int PackLength;

        public const int PacketSize = 20;

        public PacketHeader() { }

        protected override void ReadPacket(BinaryReader reader)
        {
            PacketCode = ReadUInt32();
            Socket = ReadInt32();
            SocketIdx = ReadUInt16();
            Ident = ReadUInt16();
            UserIndex = ReadInt32();
            PackLength = ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(PacketCode);
            writer.Write(Socket);
            writer.Write(SocketIdx);
            writer.Write(Ident);
            writer.Write(UserIndex);
            writer.Write(PackLength);
        }
    }

    public class ClientOutMessage : Packets
    {
        private PacketHeader MessageHeader;
        private ClientPacket DefaultMessage;

        public ClientOutMessage(PacketHeader messageHeader, ClientPacket defaultMessage)
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
            var nLen = MessageHeader.PackLength + 20;
            writer.Write(nLen);
            writer.Write(MessageHeader.GetPacket());
            writer.Write(DefaultMessage.GetPacket());
        }
    }
}