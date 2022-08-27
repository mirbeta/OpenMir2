using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class TOStdItem : Packets
    {
        public string Name;
        public byte StdMode;
        public byte Shape;
        public byte Weight;
        public byte AniCount;
        public byte Source;
        public byte Reserved;
        public byte NeedIdentify;
        public ushort Looks;
        public ushort DuraMax;
        public ushort AC;
        public ushort MAC;
        public ushort DC;
        public ushort MC;
        public ushort SC;
        public byte Need;
        public byte NeedLevel;
        public int Price;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name.ToByte(15));
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
        }
    }
}

