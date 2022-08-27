using System.Collections.Generic;

namespace SystemModule.Data;

public struct TMonInfo
{
    public IList<TMonItem> ItemList;
    /// <summary>
    /// 怪物名
    /// </summary>
    public string sName;
    /// <summary>
    /// 种族
    /// </summary>
    public byte btRace;
    /// <summary>
    /// 种族图像
    /// </summary>
    public byte btRaceImg;
    /// <summary>
    /// 形像代码
    /// </summary>
    public ushort wAppr;
    /// <summary>
    /// 怪物等级
    /// </summary>
    public ushort wLevel;
    /// <summary>
    /// 不死系
    /// </summary>
    public byte btLifeAttrib;
    /// <summary>
    /// 视线范围
    /// </summary>
    public short wCoolEye;
    /// <summary>
    /// 经验点数
    /// </summary>
    public int dwExp;
    public ushort wHP;
    public ushort wMP;
    public ushort wAC;
    public ushort wMAC;
    public ushort wDC;
    public ushort wMaxDC;
    public ushort wMC;
    public ushort wSC;
    public ushort wSpeed;
    /// <summary>
    /// 命中率
    /// </summary>
    public ushort wHitPoint;
    /// <summary>
    /// 行走速度
    /// </summary>
    public ushort wWalkSpeed;
    /// <summary>
    /// 行走步伐
    /// </summary>
    public ushort wWalkStep;
    /// <summary>
    /// 行走等待
    /// </summary>
    public ushort wWalkWait;
    /// <summary>
    /// 攻击速度
    /// </summary>
    public ushort wAttackSpeed;
    public ushort wAntiPush;
    public bool boAggro;
    public bool boTame;
}