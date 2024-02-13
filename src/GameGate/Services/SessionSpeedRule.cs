using OpenMir2;

namespace GameGate.Services
{
    public class SessionSpeedRule
    {
        /// <summary>
        /// 是否速度限制
        /// </summary>
        public bool SpeedLimit;
        /// <summary>
        /// 最高的人物身上所有装备+速度，默认6。
        /// </summary>
        public int ItemSpeed;
        /// <summary>
        /// 玩家加速度装备因数，数值越小，封加速越严厉，默认60。
        /// </summary>
        public int DefItemSpeed;
        /// <summary>
        /// 加速的累计值
        /// </summary>
        public int ErrorCount;
        /// <summary>
        /// 交易时间
        /// </summary>
        public int DealTick;
        /// <summary>
        /// 装备加速
        /// </summary>
        public int HitSpeed;
        /// <summary>
        /// 发言时间
        /// </summary>
        public int SayMsgTick;
        /// <summary>
        /// 移动时间
        /// </summary>
        public int MoveTick;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int AttackTick;
        /// <summary>
        /// 魔法时间
        /// </summary>
        public int SpellTick;
        /// <summary>
        /// 走路时间
        /// </summary>
        public int DwWalkTick;
        /// <summary>
        /// 跑步时间
        /// </summary>
        public int DwRunTick;
        /// <summary>
        /// 转身时间
        /// </summary>
        public int TurnTick;
        /// <summary>
        /// 挖肉时间
        /// </summary>
        public int ButchTick;
        /// <summary>
        /// 蹲下时间
        /// </summary>
        public int SitDownTick;
        /// <summary>
        /// 吃药时间
        /// </summary>
        public int EatTick;
        /// <summary>
        /// 捡起物品时间
        /// </summary>
        public int PickupTick;
        /// <summary>
        /// 移动时间
        /// </summary>
        public int DwRunWalkTick;
        /// <summary>
        /// 传送时间
        /// </summary>
        public int DwFeiDnItemsTick;
        /// <summary>
        /// 变速齿轮时间
        /// </summary>
        public int DwSupSpeederTick;
        /// <summary>
        /// 变速齿轮累计
        /// </summary>
        public int DwSupSpeederCount;
        /// <summary>
        /// 超级加速时间
        /// </summary>
        public int DwSuperNeverTick;
        /// <summary>
        /// 超级加速累计
        /// </summary>
        public int DwSuperNeverCount;
        /// <summary>
        /// 记录上一次操作
        /// </summary>
        public int DwUserDoTick;
        /// <summary>
        /// 保存停顿操作时间
        /// </summary>
        public long ContinueTick;
        /// <summary>
        /// 带有攻击并发累计
        /// </summary>
        public int ConHitMaxCount;
        /// <summary>
        /// 带有魔法并发累计
        /// </summary>
        public int ConSpellMaxCount;
        /// <summary>
        /// 记录上一次移动方向
        /// </summary>
        public int CombinationTick;
        /// <summary>
        /// 智能攻击累计
        /// </summary>
        public int CombinationCount;
        public long GameTick;
        public int WaringTick;

        public SessionSpeedRule()
        {
            var dwCurrentTick = HUtil32.GetTickCount();
            ErrorCount = dwCurrentTick;
            DealTick = dwCurrentTick;
            HitSpeed = dwCurrentTick;
            SayMsgTick = dwCurrentTick;
            MoveTick = dwCurrentTick;
            AttackTick = dwCurrentTick;
            SpellTick = dwCurrentTick;
            DwWalkTick = dwCurrentTick;
            DwRunTick = dwCurrentTick;
            TurnTick = dwCurrentTick;
            ButchTick = dwCurrentTick;
            SitDownTick = dwCurrentTick;
            EatTick = dwCurrentTick;
            PickupTick = dwCurrentTick;
            DwRunWalkTick = dwCurrentTick;
            DwFeiDnItemsTick = dwCurrentTick;
            DwSupSpeederTick = dwCurrentTick;
            DwSupSpeederCount = dwCurrentTick;
            DwSuperNeverTick = dwCurrentTick;
            DwSuperNeverCount = dwCurrentTick;
            DwUserDoTick = dwCurrentTick;
            ContinueTick = dwCurrentTick;
            ConHitMaxCount = dwCurrentTick;
            ConSpellMaxCount = dwCurrentTick;
            CombinationTick = dwCurrentTick;
            CombinationCount = dwCurrentTick;
            GameTick = dwCurrentTick;
            WaringTick = dwCurrentTick;
        }
    }
}