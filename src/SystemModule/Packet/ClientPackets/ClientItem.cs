
using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    public class ClientItem : Packets
    {
        public ClientStdItem Item;
        public int MakeIndex;
        public ushort Dura;
        public ushort DuraMax;

        public ClientItem()
        {
            Item = new ClientStdItem();
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
