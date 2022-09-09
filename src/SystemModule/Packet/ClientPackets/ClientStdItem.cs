using System.IO;
using SystemModule.Data;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class ClientStdItem : Packets
    {
        public string Name;
        public byte StdMode;
        public byte Shape;
        public byte Weight;
        public ushort AniCount;
        public sbyte Source;
        public byte Reserved;
        public byte NeedIdentify;
        public ushort Looks;
        public ushort DuraMax;
        public int AC;
        public int MAC;
        public int DC;
        public int MC;
        public int SC;
        public int Need;
        public int NeedLevel;
        public int Price;
        public byte UniqueItem;
        public byte Overlap;
        public byte ItemType;
        public ushort ItemSet;
        public byte Binded;
        public byte[] Reserve;
        public byte[] AddOn;
        public TEvaluation Eva;

        public ClientStdItem()
        {
            Reserve = new byte[9];
            AddOn = new byte[10];
            Eva = new TEvaluation();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.WriteAsciiString(Name, PacketConst.ItemNameLen);
            writer.Write(StdMode);
            writer.Write(Shape);
            writer.Write(Weight);
            writer.Write(AniCount);
            writer.Write(Source);
            writer.Write(Reserved);
            writer.Write(NeedIdentify);
            writer.Write(Looks);
            writer.Write(DuraMax);
            writer.Write(AC);
            writer.Write(MAC);
            writer.Write(DC);
            writer.Write(MC);
            writer.Write(SC);
            writer.Write(Need);
            writer.Write(NeedLevel);
            writer.Write(Price);
            writer.Write(UniqueItem);
            writer.Write(Overlap);
            writer.Write(ItemType);
            writer.Write(ItemSet);
            writer.Write(Binded);
            writer.Write(Reserve);
            writer.Write(AddOn);
            writer.Write(Eva.GetBuffer());
        }
    }
}