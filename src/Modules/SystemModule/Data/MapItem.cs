using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public record struct MapItem
    {
        /// <summary>
        /// 物品唯一ID
        /// </summary>
        public int ItemId;
        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 物品外观
        /// </summary>
        public ushort Looks;
        public byte AniCount;
        public int Reserved;
        /// <summary>
        /// 数量
        /// </summary>
        public int Count;
        /// <summary>
        /// 谁掉落的
        /// </summary>
        public int DropBaseObject;
        /// <summary>
        /// 物品谁可以捡起
        /// </summary>
        public int OfBaseObject;
        /// <summary>
        /// 掉落的时间
        /// </summary>
        public int CanPickUpTick;
        public UserItem UserItem;

        public MapItem()
        {
            // this.ItemId = M2Share.ActorMgr.GetNextIdentity();
        }
    }
}