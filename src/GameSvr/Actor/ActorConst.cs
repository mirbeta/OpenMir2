namespace GameSvr.Actor
{
    public struct SendMessage
    {
        public int wIdent;
        public int wParam;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        /// <summary>
        /// 延时时间
        /// </summary>
        public int DeliveryTime;
        public int BaseObject;
        public bool LateDelivery;
        public string Buff;
    }

    /// <summary>
    /// 可见的精灵
    /// </summary>
    public class VisibleBaseObject
    {
        public BaseObject BaseObject;
        public VisibleFlag VisibleFlag;
    }

    public enum VisibleFlag : byte
    {
        /// <summary>
        /// 可见
        /// </summary>
        Visible = 0,
        /// <summary>
        /// 不可见
        /// </summary>
        Invisible = 1,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hidden = 2
    }

    public enum PlayGender : byte
    {
        Man = 0,
        WoMan = 1
    }
}