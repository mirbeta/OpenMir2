namespace SystemModule.Enums;

public enum AttackMode : byte
{
    /// <summary>
    /// 全体攻击模式
    /// </summary>
    HAM_ALL = 0,
    /// <summary>
    /// 和平攻击模式
    /// </summary>
    HAM_PEACE = 1,
    /// <summary>
    /// 夫妻攻击模式
    /// </summary>
    HAM_DEAR = 2,
    /// <summary>
    /// 师徒攻击模式
    /// </summary>
    HAM_MASTER = 3,
    /// <summary>
    /// 组队攻击模式
    /// </summary>
    HAM_GROUP = 4,
    /// <summary>
    /// 行会攻击模式
    /// </summary>
    HAM_GUILD = 5,
    /// <summary>
    /// 红名攻击模式
    /// </summary>
    HAM_PKATTACK = 6
}