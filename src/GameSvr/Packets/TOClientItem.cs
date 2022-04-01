using System.IO;
using SystemModule;

namespace GameSvr
{
    public class TOClientItem : Packets
    {
        public TOStdItem Item;
        public int MakeIndex;
        public ushort Dura;
        public ushort DuraMax;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Item.GetBuffer());
            writer.Write(MakeIndex);
            writer.Write(Dura);
            writer.Write(DuraMax);
        }
    }
}