using System.IO;

namespace SystemModule.Packets.ClientPackets
{
    public class CharDesc : ClientPacket
    {
        public int Feature;
        public int Status;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Feature);
            writer.Write(Status);
        }
    }
}

