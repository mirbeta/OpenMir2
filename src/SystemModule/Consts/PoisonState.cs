namespace SystemModule.Consts
{
    /// <summary>
    /// 中毒状态定义
    /// </summary>
    public static class PoisonState
    {
        /// <summary>
        /// 中毒类型 - 绿毒
        /// </summary>
        public const byte DECHEALTH = 0;
        /// <summary>
        /// 中毒类型 - 红毒
        /// </summary>
        public const byte DAMAGEARMOR = 1;
        /// <summary>
        /// 中毒类型 - 不能使用技能
        /// </summary>
        public const byte LOCKSPELL = 2;
        /// <summary>
        /// 中毒类型 - 禁止移动
        /// </summary>
        public const byte DONTMOVE = 4;
        /// <summary>
        /// 中毒类型 - 防麻
        /// </summary>
        public const byte STONE = 5;
        /// <summary>
        /// 不能跑动(中蛛网)
        /// </summary>
        public const byte LOCKRUN = 3;
        public const byte POISON_68 = 68;
        public const byte FASTMOVE = 7;
        /// <summary>
        /// 隐身
        /// </summary>
        public const byte STATE_TRANSPARENT = 8;
        /// <summary>
        /// 神圣战甲术  防御力
        /// </summary>
        public const byte DEFENCEUP = 9;
        /// <summary>
        /// 幽灵盾  魔御力
        /// </summary>
        public const byte MAGDEFENCEUP = 10;
        /// <summary>
        /// 魔法盾
        /// </summary>
        public const byte BUBBLEDEFENCEUP = 11;
        /// <summary>
        /// 被石化
        /// </summary>
        public const int STONEMODE = 0x00000001;
        public const int OPENHEATH = 0x00000002;
    }
}