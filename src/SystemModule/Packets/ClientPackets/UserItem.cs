using ProtoBuf;
using System.IO;

namespace SystemModule.Packets.ClientPackets
{
    public class ItemAttr
    {
        /// <summary>
        /// 武器升级
        /// </summary>
        public const int WeaponUpgrade = 10;
    }

    [ProtoContract]
    public class UserItem : Packets
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        [ProtoMember(1)]
        public int MakeIndex;
        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(2)]
        public ushort Index;
        /// <summary>
        /// 当前持久值
        /// </summary>
        [ProtoMember(3)]
        public ushort Dura;
        /// <summary>
        /// 最大持久值
        /// </summary>
        [ProtoMember(4)]
        public ushort DuraMax;
        [ProtoMember(5)]
        public byte[] Desc;
        [ProtoMember(6)]
        public byte ColorR;
        [ProtoMember(7)]
        public byte ColorG;
        [ProtoMember(8)]
        public byte ColorB;
        [ProtoMember(9)]
        public char[] Prefix;

        public UserItem()
        {
            Desc = new byte[14];
            Prefix = new char[13];
        }

        public UserItem(UserItem userItem)
        {
            MakeIndex = userItem.MakeIndex;
            Index = userItem.Index;
            Dura = userItem.Dura;
            DuraMax = userItem.DuraMax;
            Desc = userItem.Desc;
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            MakeIndex = reader.ReadInt32();
            Index = reader.ReadUInt16();
            Dura = reader.ReadUInt16();
            DuraMax = reader.ReadUInt16();
            Desc = reader.ReadBytes(14);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MakeIndex);
            writer.Write(Index);
            writer.Write(Dura);
            writer.Write(DuraMax);
            writer.Write(Desc);
            writer.Write(ColorR);
            writer.Write(ColorG);
            writer.Write(ColorB);
            writer.Write(Prefix);
        }
    }
}