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

    public class GragonConst
    {
        public const int DRAGON_RING_SHAPE = 198;
        public const int DRAGON_BRACELET_SHAPE = 199;
        public const int DRAGON_NECKLACE_SHAPE = 200;
        public const int DRAGON_DRESS_SHAPE = 10;
        public const int DRAGON_HELMET_SHAPE = 201;
        public const int DRAGON_WEAPON_SHAPE = 37;
        public const int DRAGON_BOOTS_SHAPE = 203;
        public const int DRAGON_BELT_SHAPE = 204;
    }

    public class ShapeConst
    {
        public const byte LOLLIPOP_SHAPE = 1;
        public const byte GOLDMEDAL_SHAPE = 2;
        public const byte SILVERMEDAL_SHAPE = 3;
        public const byte BRONZEMEDAL_SHAPE = 4;
        public const byte SHAPE_OF_LUCKYLADLE = 5;
    }

    public class AbilConst
    {
        public const byte EABIL_DCUP = 0;   //鉴埃利栏肺 颇鲍仿阑 棵覆 (老沥 矫埃)
        public const byte EABIL_MCUP = 1;
        public const byte EABIL_SCUP = 2;
        public const byte EABIL_HITSPEEDUP = 3;
        public const byte EABIL_HPUP = 4;
        public const byte EABIL_MPUP = 5;
        public const byte EABIL_PWRRATE = 6;   // 傍拜仿 饭捞飘 炼沥 
    }

    public class EfftypeConst
    {
        public const byte EFFTYPE_TWOHAND_WEHIGHT_ADD = 1;
        public const byte EFFTYPE_EQUIP_WHEIGHT_ADD = 2;
        public const byte EFFTYPE_LUCK_ADD = 3;
        public const byte EFFTYPE_BAG_WHIGHT_ADD = 4;
        public const byte EFFTYPE_HP_MP_ADD = 5;
        public const byte EFFTYPE2_EVENT_GRADE = 6;
    }
}