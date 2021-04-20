namespace M2Server
{
    public class TMapItem
    {
        public int Id;
        public string Name;
        public short Looks;
        public byte AniCount;
        public int Reserved;
        public int Count;
        public object DropBaseObject;
        public object OfBaseObject;
        public int dwCanPickUpTick;
        public TUserItem UserItem;
    }
}