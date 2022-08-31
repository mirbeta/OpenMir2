using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Maps
{
    public class MapItem : EntityId
    {
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
        public object DropBaseObject;
        public object OfBaseObject;
        /// <summary>
        /// 可以拾取的时间
        /// </summary>
        public int CanPickUpTick;
        public TUserItem UserItem;
    }
}