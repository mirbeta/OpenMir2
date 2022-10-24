using System.IO;

namespace SystemModule.Packet.ServerPackets
{
    public class LoginSvrPacket : Packets
    {
        public string ConnectionId { get; set; }
        public short PackLen { get; set; }
        public byte[] ClientPacket { get; set; }

        protected override void ReadPacket(BinaryReader reader)
        {
            ConnectionId = reader.ReadString();
            PackLen = reader.ReadInt16();
            ClientPacket = reader.ReadBytes(PackLen);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ConnectionId);
            if (ClientPacket.Length > 0)
            {
                writer.Write((short)ClientPacket.Length);
                writer.Write(ClientPacket);
            }
            else
            {
                writer.Write((ushort)0);
                writer.Write((byte)0);
            }
        }
    }
}