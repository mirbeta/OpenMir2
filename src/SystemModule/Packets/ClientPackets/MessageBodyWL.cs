using System.IO;

namespace SystemModule.Packets.ClientPackets
{
    public class MessageBodyWL : Packets
    {
        public int Param1;
        public int Param2;
        public int Tag1;
        public int Tag2;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Param1);
            writer.Write(Param2);
            writer.Write(Tag1);
            writer.Write(Tag2);
        }
    }
}