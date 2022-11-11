using System;
using System.IO;

namespace SystemModule.Data
{
    public class StdItemInfo
    {
        public string Name;
        public byte StdMode;
        public byte Shape;
        public byte Weight;
        public byte AniCount;
        public short Source;
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
        public short ItemSet;
        public byte Binded;
        public byte[] Reserve;
        public byte[] AddOn;

        public StdItemInfo()
        {
            Reserve = new byte[9];
            AddOn = new byte[10];
        }
    }

    public class TEvaAbil : Packets.Packets
    {
        public byte btType;
        public byte btValue;

        public TEvaAbil()
        {
            btType = 0;
            btValue = 0;
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(btType);
            writer.Write(btValue);
        }
    }
}