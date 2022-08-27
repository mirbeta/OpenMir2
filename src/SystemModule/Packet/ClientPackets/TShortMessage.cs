using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    public class TShortMessage : Packets
    {
        public ushort Ident;
        public ushort wMsg;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Ident);
            writer.Write(wMsg);
        }
    }
}
