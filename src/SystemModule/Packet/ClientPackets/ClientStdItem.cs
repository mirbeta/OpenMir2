using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class ClientStdItem : Packets
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

        public ClientStdItem()
        {

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
            writer.Write(SpecialPwr);
            writer.Write(ItemDesc);
            writer.Write((byte)0);
            writer.Write(Looks);
            writer.Write(DuraMax);
            writer.Write(AC);
            writer.Write(MAC);
            writer.Write(DC);
            writer.Write(MC);
            writer.Write(SC);
            writer.Write(Need);
            writer.Write(NeedLevel);
            writer.Write(NeedIdentify);
            writer.Write((byte)0);
            writer.Write(Price);
            writer.Write(Stock);
            writer.Write(AtkSpd);
            writer.Write(Agility);
            writer.Write(Accurate);
            writer.Write(MgAvoid);
            writer.Write(Strong);
            writer.Write(Undead);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write(HpAdd);
            writer.Write(MpAdd);
            writer.Write(ExpAdd);
            writer.Write(EffType1);
            writer.Write(EffRate1);
            writer.Write(EffValue1);
            writer.Write(EffType2);
            writer.Write(EffRate2);
            writer.Write(EffValue2);
            writer.Write(Slowdown);
            writer.Write(Tox);
            writer.Write(ToxAvoid);
            writer.Write(UniqueItem);
            writer.Write(OverlapItem);
            writer.Write(Light);
            writer.Write(ItemType);
            writer.Write((byte)0);
            writer.Write(ItemSet);
            if (string.IsNullOrEmpty(Reference))
            {
                writer.WriteAsciiString("", PacketConst.ItemNameLen);
            }
            else
            {
                writer.WriteAsciiString(Reference, PacketConst.ItemNameLen);
            }
            writer.Write((byte)0);
        }
    }
}