using System.IO;
using SystemModule.Packets.ServerPackets;

namespace SystemModule.Packets.ClientPackets
{
    public class ItemAttr
    {
        /// <summary>
        /// 武器升级
        /// </summary>
        public const int WeaponUpgrade = 10;
    }

    public class ClientUserItem : ClientPackage
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int MakeIndex { get; set; }
        /// <summary>
        /// 物品ID
        /// </summary>
        public ushort Index { get; set; }
        /// <summary>
        /// 当前持久值
        /// </summary>
        public ushort Dura { get; set; }
        /// <summary>
        /// 最大持久值
        /// </summary>
        public ushort DuraMax { get; set; }
        public byte[] Desc { get; set; }
        public byte ColorR { get; set; }
        public byte ColorG { get; set; }
        public byte ColorB { get; set; }
        public char[] Prefix { get; set; }

        public ClientUserItem()
        {
            Desc = new byte[14];
            Prefix = new char[13];
        }

        public ClientUserItem(ClientUserItem userItem)
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

        public ServerUserItem ToServerItem()
        {
            return new ServerUserItem
            {
                MakeIndex = MakeIndex,
                Index = Index,
                Dura = Dura,
                DuraMax = DuraMax,
                Desc = Desc,
                ColorR = ColorR,
                ColorG = ColorG,
                ColorB = ColorB,
                Prefix = Prefix
            };
        }
    }
}