using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    /// <summary>
    /// 封包消息头
    /// </summary>
    public class PacketHeader : Packets
    {
        public uint PacketCode;
        /// <summary>
        /// SocketID
        /// </summary>
        public int Socket;
        /// <summary>
        /// 会话ID
        /// </summary>
        public ushort SessionId;
        public ushort Ident;
        public int ServerIndex;
        public int PackLength;

        public const int PacketSize = 20;

        protected override void ReadPacket(BinaryReader reader)
        {
            PacketCode = reader.ReadUInt32();
            Socket = reader.ReadInt32();
            SessionId = reader.ReadUInt16();
            Ident = reader.ReadUInt16();
            ServerIndex = reader.ReadInt32();
            PackLength = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(PacketCode);
            writer.Write(Socket);
            writer.Write(SessionId);
            writer.Write(Ident);
            writer.Write(ServerIndex);
            writer.Write(PackLength);
        }
    }

    public class ClientOutMessage : Packets
    {
        private PacketHeader MessageHeader;
        private ClientMesaagePacket DefaultMessage;

        public ClientOutMessage(PacketHeader messageHeader, ClientMesaagePacket defaultMessage)
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
            writer.Write(MessageHeader.GetBuffer());
            writer.Write(DefaultMessage.GetBuffer());
        }
    }
}