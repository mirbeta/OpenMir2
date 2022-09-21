using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class ClientItem : Packets
    {
        public ClientStdItem Item;
        public int MakeIndex;
        public ushort Dura;
        public ushort DuraMax;
        public byte[] Desc;

        public ClientItem()
        {
            Item = new ClientStdItem();
            Desc = new byte[14];
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.WriteAsciiString(Item.Name, PacketConst.ItemNameLen);
            writer.Write(Item.StdMode);
            writer.Write(Item.Shape);
            writer.Write(Item.Weight);
            writer.Write(Item.AniCount);
            writer.Write(Item.SpecialPwr);
            writer.Write(Item.ItemDesc);
            writer.Write((byte)0);
            writer.Write(Item.Looks);
            writer.Write(DuraMax);
            writer.Write(Item.AC);
            writer.Write(Item.MAC);
            writer.Write(Item.DC);
            writer.Write(Item.MC);
            writer.Write(Item.SC);
            writer.Write(Item.Need);
            writer.Write(Item.NeedLevel);
            writer.Write(Item.NeedIdentify);
            writer.Write((byte)0);
            writer.Write(Item.Price);
            writer.Write(Item.Stock);
            writer.Write(Item.AtkSpd);
            writer.Write(Item.Agility);
            writer.Write(Item.Accurate);
            writer.Write(Item.MgAvoid);
            writer.Write(Item.Strong);
            writer.Write(Item.Undead);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write(Item.HpAdd);
            writer.Write(Item.MpAdd);
            writer.Write(Item.ExpAdd);
            writer.Write(Item.EffType1);
            writer.Write(Item.EffRate1);
            writer.Write(Item.EffValue1);
            writer.Write(Item.EffType2);
            writer.Write(Item.EffRate2);
            writer.Write(Item.EffValue2);
            writer.Write(Item.Slowdown);
            writer.Write(Item.Tox);
            writer.Write(Item.ToxAvoid);
            writer.Write(Item.UniqueItem);
            writer.Write(Item.OverlapItem);
            writer.Write(Item.Light);
            writer.Write(Item.ItemType);
            writer.Write((byte)0);
            writer.Write(Item.ItemSet);
            if (string.IsNullOrEmpty(Item.Reference))
            {
                writer.WriteAsciiString("", PacketConst.ItemNameLen);
            }
            else
            {
                writer.WriteAsciiString(Item.Reference, PacketConst.ItemNameLen);
            }
            writer.Write((byte)0);
            writer.Write(MakeIndex);
            writer.Write(Dura);
            writer.Write(DuraMax);
            writer.Write(Desc, 0, Desc.Length);
        }

        public class ClientStdItem
        {
            public string Name;
            public byte StdMode;
            public byte Shape;
            public byte Weight;
            public byte AniCount;
            public sbyte SpecialPwr;
            public byte ItemDesc;
            public ushort Looks;
            public ushort DuraMax;
            public ushort AC;
            public ushort MAC;
            public ushort DC;
            public ushort MC;
            public ushort SC;
            public byte Need;
            public byte NeedLevel;
            public byte NeedIdentify;
            public int Price;
            public int Stock;
            public byte AtkSpd;
            public byte Agility;
            public byte Accurate;
            public byte MgAvoid;
            public byte Strong;
            public byte Undead;
            public int HpAdd;
            public int MpAdd;
            public int ExpAdd;
            public byte EffType1;
            public byte EffRate1;
            public byte EffValue1;
            public byte EffType2;
            public byte EffRate2;
            public byte EffValue2;
            public byte Slowdown;
            public byte Tox;
            public byte ToxAvoid;
            public byte UniqueItem;
            public byte OverlapItem;
            public byte Light;
            public byte ItemType;
            public ushort ItemSet;
            public string Reference;
        }
    }
}