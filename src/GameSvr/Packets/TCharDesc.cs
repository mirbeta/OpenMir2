using SystemModule;

namespace GameSvr
{
    public class TCharDesc : Packets
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

