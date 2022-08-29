namespace GameSvr.Actor
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

    public enum PlayGender : byte
    {
        Man = 0,
        WoMan = 1
    }

    public enum Job
    {
        
    }
}