namespace SystemModule;

public class Grobal2
{
    public const int ClientVersionNumber = 1200409180;
    public const uint PacketCode = 0xAA55AA55 + 0x00450045;
    /// <summary>
    /// 最大魔法技能数
    /// </summary>
    public const byte MaxMagicCount = 54;
    public const string StringGoldName = "金币";
    /// <summary>
    /// 最高等级
    /// </summary>
    public const byte MaxLevel = byte.MaxValue;
    /// <summary>
    /// 最高经验等级
    /// </summary>
    public const ushort MaxChangeLevel = 1000;
    /// <summary>
    /// 属下最高等级
    /// </summary>
    public const byte SlaveMaxLevel = 50;
    /// <summary>
    /// 记录游戏金币日志
    /// </summary>
    public const byte LogGameGold = 1;
    /// <summary>
    /// 记录声望日志
    /// </summary>
    public const byte LogGamePoint = 2;
    /// <summary>
    /// 组队最大人数
    /// </summary>
    public const byte GroupMax = 11;
    /// <summary>
    /// 物品类型(物品属性读取)
    /// </summary>
    public const byte MAX_STATUS_ATTRIBUTE = 12;

    public const byte ET_DIGOUTZOMBI = 1;
    public const byte ET_MINE = 2;
    public const byte ET_PILESTONES = 3;
    public const byte ET_HOLYCURTAIN = 4;
    /// <summary>
    /// 火墙事件
    /// </summary>
    public const byte ET_FIRE = 5;
    public const byte ET_SCULPEICE = 6;

    public const byte GM_OPEN = 1;
    public const byte GM_CLOSE = 2;
    public const byte GM_CHECKSERVER = 3;
    public const byte GM_CHECKCLIENT = 4;
    public const byte GM_DATA = 5;
    public const byte GM_SERVERUSERINDEX = 6;
    public const byte GM_RECEIVE_OK = 7;
    public const byte GM_STOP = 8;
    /// <summary>
    /// 最大包裹数
    /// </summary>
    public const byte MaxBagItem = 46;
    public const byte LA_UNDEAD = 1;
}