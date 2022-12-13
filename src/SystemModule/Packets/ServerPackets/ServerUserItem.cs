using MemoryPack;
using SystemModule.Packets.ClientPackets;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class ServerUserItem 
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

        public ServerUserItem()
        {
            Desc = new byte[14];
            Prefix = new char[13];
        }

        public UserItem ToClientItem()
        {
            return new UserItem
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