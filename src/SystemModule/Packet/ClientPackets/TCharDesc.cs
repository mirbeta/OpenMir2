using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    public class TCharDesc : Packets
    {
        public int Feature;
        public int Status;
        public int StatusEx;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Feature);
            writer.Write(Status);
            writer.Write(StatusEx);
        }
    }
}

