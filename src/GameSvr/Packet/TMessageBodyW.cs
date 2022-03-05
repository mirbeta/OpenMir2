using System.IO;
using SystemModule;

namespace GameSvr
{
    public class TMessageBodyW : Packets
    {
        public ushort Param1;
        public ushort Param2;
        public ushort Tag1;
        public ushort Tag2;

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

