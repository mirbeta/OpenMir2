using SystemModule;

namespace GameSvr
{
    public class TMapItem
    {
        public int Id;
        public string Name;
        public ushort Looks;
        public byte AniCount;
        public int Reserved;
        public int Count;
        public object DropBaseObject;
        public object OfBaseObject;
        public int dwCanPickUpTick;
        public TUserItem UserItem;

        public TMapItem()
        {
            Id = HUtil32.Sequence();
        }
    }
}