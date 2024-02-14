using System.Collections.Generic;

namespace OpenMir2.Data
{
    public struct MonsterInfo
    {
        /// <summary>
        /// 怪物名
        /// </summary>
        public string Name;
        /// <summary>
        /// 种族
        /// </summary>
        public byte Race;
        /// <summary>
        /// 种族图像
        /// </summary>
        public byte RaceImg;
        /// <summary>
        /// 形像代码
        /// </summary>
        public ushort Appr;
        /// <summary>
        /// 怪物等级
        /// </summary>
        public byte Level;
        /// <summary>
        /// 不死系
        /// </summary>
        public byte btLifeAttrib;
        /// <summary>
        /// 视线范围
        /// </summary>
        public byte CoolEye;
        /// <summary>
        /// 经验点数
        /// </summary>
        public int Exp;
        public ushort HP;
        public ushort MP;
        public ushort AC;
        public ushort MAC;
        public ushort DC;
        public ushort MaxDC;
        public ushort MC;
        public ushort SC;
        public byte Speed;
        /// <summary>
        /// 命中率
        /// </summary>
        public byte HitPoint;
        /// <summary>
        /// 行走速度
        /// </summary>
        public ushort WalkSpeed;
        /// <summary>
        /// 行走步伐
        /// </summary>
        public ushort WalkStep;
        /// <summary>
        /// 行走等待
        /// </summary>
        public ushort WalkWait;
        /// <summary>
        /// 攻击速度
        /// </summary>
        public ushort AttackSpeed;
        public ushort AntiPush;
        public bool boAggro;
        public bool boTame;
        /// <summary>
        /// 掉落物品列表
        /// </summary>
        public IList<MonsterDropItem> ItemList;
    }
}