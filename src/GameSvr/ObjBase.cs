namespace GameSvr
{
    public struct SendMessage
    {
        public int wIdent;
        public int wParam;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        public int dwDeliveryTime;
        public TBaseObject BaseObject;
        public int ObjectId;
        public bool boLateDelivery;
        public string Buff;
    }

    /// <summary>
    /// 可见的精灵
    /// </summary>
    public class TVisibleBaseObject
    {
        public TBaseObject BaseObject;
        public int nVisibleFlag;
    }

    /// <summary>
    /// 可见的地图物品
    /// </summary>
    public class TVisibleMapItem
    {
        public int nX;
        public int nY;
        public TMapItem MapItem;
        public string sName;
        public ushort wLooks;
        public int nVisibleFlag;
    } 
    
    public class ObjBase
    {
        public const int gMan = 0;
        public const int gWoMan = 1;
    }
}
