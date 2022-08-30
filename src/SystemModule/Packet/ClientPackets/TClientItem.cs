
using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    public class TClientItem : Packets
    {
        public TClientStdItem Item;
        public int MakeIndex;
        public ushort Dura;
        public ushort DuraMax;

        public TClientItem()
        {
            Item = new TClientStdItem();
        }

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
