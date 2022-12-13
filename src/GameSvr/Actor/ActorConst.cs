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

    public class DragonConst
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
        public const byte EABIL_DCUP = 0;   
        public const byte EABIL_MCUP = 1;
        public const byte EABIL_SCUP = 2;
        public const byte EABIL_HITSPEEDUP = 3;
        public const byte EABIL_HPUP = 4;
        public const byte EABIL_MPUP = 5;
        public const byte EABIL_PWRRATE = 6;
    }

    public class EfftypeConst
    {
        public const byte EFFTYPE_TWOHAND_WEHIGHT_ADD = 1;
        public const byte EFFTYPE_EQUIP_WHEIGHT_ADD = 2;
        public const byte EFFTYPE_LUCK_ADD = 3;
        public const byte EFFTYPE_BAG_WHIGHT_ADD = 4;
        public const byte EFFTYPE_HP_MP_ADD = 5;
        public const byte EFFTYPE2_EVENT_GRADE = 6;
        public const byte EFFECTIVE_HIGHLEVEL = 50;
    }

    public class ItemShapeConst
    {
        public const int SHAPE_OF_LUCKYLADLE = 5;
        /// <summary>
        /// 隐身属性
        /// </summary>
        public const int RING_TRANSPARENT_ITEM = 111;
        /// <summary>
        /// 传送属性
        /// </summary>
        public const int RING_SPACEMOVE_ITEM = 112;
        /// <summary>
        /// 麻痹属性
        /// </summary>
        public const int RING_MAKESTONE_ITEM = 113;
        /// <summary>
        /// 复活属性
        /// </summary>
        public const int RING_REVIVAL_ITEM = 114;
        /// <summary>
        /// 火焰属性
        /// </summary>
        public const int RING_FIREBALL_ITEM = 115;
        /// <summary>
        /// 治愈属性
        /// </summary>
        public const int RING_HEALING_ITEM = 116;
        /// <summary>
        /// 未知
        /// </summary>
        public const int RING_ANGERENERGY_ITEM = 117;
        /// <summary>
        /// 护身属性
        /// </summary>
        public const int RING_MAGICSHIELD_ITEM = 118;
        /// <summary>
        /// 活力属性
        /// </summary>
        public const int RING_SUPERSTRENGTH_ITEM = 119;
        /// <summary>
        /// 技巧项链
        /// </summary>
        public const int NECTLACE_FASTTRAINING_ITEM = 120;
        /// <summary>
        /// 探测项链
        /// </summary>
        public const int NECTLACE_SEARCH_ITEM = 121;
        /// <summary>
        /// 
        /// </summary>
        public const int RING_CHUN_ITEM = 122;
        public const int NECKLACE_GI_ITEM = 123;
        public const int ARMRING_HAP_ITEM = 124;
        public const int HELMET_IL_ITEM = 125;
        public const int RING_OF_UNKNOWN = 130;
        public const int BRACELET_OF_UNKNOWN = 131;
        public const int HELMET_OF_UNKNOWN = 132;
        public const int RING_OF_MANATOHEALTH = 133;
        public const int BRACELET_OF_MANATOHEALTH = 134;
        /// <summary>
        /// 魔血项链
        /// </summary>
        public const int NECKLACE_OF_MANATOHEALTH = 135;
        public const int RING_OF_SUCKHEALTH = 136;
        public const int BRACELET_OF_SUCKHEALTH = 137;
        /// <summary>
        /// 虹膜项链
        /// </summary>
        public const int NECKLACE_OF_SUCKHEALTH = 138;
        public const int RING_OF_HPUP = 140;
        public const int BRACELET_OF_HPUP = 141;
        public const int RING_OF_MPUP = 142;
        public const int BRACELET_OF_MPUP = 143;
        public const int RING_OF_HPMPUP = 144;
        public const int BRACELET_OF_HPMPUP = 145;
        public const int NECKLACE_OF_HPPUP = 146;
        public const int BRACELET_OF_HPPUP = 147;
        public const int RING_OH_HPPUP = 148;
        public const int CCHO_WEAPON = 23;
        public const int CCHO_NECKLACE = 149;
        public const int CCHO_RING = 150;
        public const int CCHO_HELMET = 151;
        public const int CCHO_BRACELET = 152;
        public const int PSET_NECKLACE_SHAPE = 153;
        public const int PSET_BRACELET_SHAPE = 154;
        public const int PSET_RING_SHAPE = 155;
        public const int HSET_NECKLACE_SHAPE = 156;
        public const int HSET_BRACELET_SHAPE = 157;
        public const int HSET_RING_SHAPE = 158;
        public const int YSET_NECKLACE_SHAPE = 159;
        public const int YSET_BRACELET_SHAPE = 160;
        public const int YSET_RING_SHAPE = 161;
        public const int BONESET_WEAPON_SHAPE = 4;
        public const int BONESET_HELMET_SHAPE = 162;
        public const int BONESET_DRESS_SHAPE = 2;
        public const int BUGSET_NECKLACE_SHAPE = 163;
        public const int BUGSET_RING_SHAPE = 164;
        public const int BUGSET_BRACELET_SHAPE = 165;
        public const int PTSET_BELT_SHAPE = 166;
        public const int PTSET_BOOTS_SHAPE = 167;
        public const int PTSET_NECKLACE_SHAPE = 168;
        public const int PTSET_BRACELET_SHAPE = 169;
        public const int PTSET_RING_SHAPE = 170;
        public const int KSSET_BELT_SHAPE = 176;
        public const int KSSET_BOOTS_SHAPE = 177;
        public const int KSSET_NECKLACE_SHAPE = 178;
        public const int KSSET_BRACELET_SHAPE = 179;
        public const int KSSET_RING_SHAPE = 180;
        public const int RUBYSET_BELT_SHAPE = 171;
        public const int RUBYSET_BOOTS_SHAPE = 172;
        public const int RUBYSET_NECKLACE_SHAPE = 173;
        public const int RUBYSET_BRACELET_SHAPE = 174;
        public const int RUBYSET_RING_SHAPE = 175;
        public const int STRONG_PTSET_BELT_SHAPE = 181;
        public const int STRONG_PTSET_BOOTS_SHAPE = 182;
        public const int STRONG_PTSET_NECKLACE_SHAPE = 183;
        public const int STRONG_PTSET_BRACELET_SHAPE = 184;
        public const int STRONG_PTSET_RING_SHAPE = 185;
        public const int STRONG_KSSET_BELT_SHAPE = 191;
        public const int STRONG_KSSET_BOOTS_SHAPE = 192;
        public const int STRONG_KSSET_NECKLACE_SHAPE = 193;
        public const int STRONG_KSSET_BRACELET_SHAPE = 194;
        public const int STRONG_KSSET_RING_SHAPE = 195;
        public const int STRONG_RUBYSET_BELT_SHAPE = 186;
        public const int STRONG_RUBYSET_BOOTS_SHAPE = 187;
        public const int STRONG_RUBYSET_NECKLACE_SHAPE = 188;
        public const int STRONG_RUBYSET_BRACELET_SHAPE = 189;
        public const int STRONG_RUBYSET_RING_SHAPE = 190;
        public const int DRESS_SHAPE_WING = 9;
        public const int DRESS_SHAPE_PBKING = 11;
        public const int DRESS_STDMODE_MAN = 10;
        public const int DRESS_STDMODE_WOMAN = 11;
    }
}