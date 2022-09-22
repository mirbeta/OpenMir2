namespace SystemModule.Consts
{
    public class StatuStateConst
    {
        /// <summary>
        /// 中毒类型 - 绿毒
        /// </summary>
        public const byte POISON_DECHEALTH = 0;
        /// <summary>
        /// 中毒类型 - 红毒
        /// </summary>
        public const byte POISON_DAMAGEARMOR = 1;
        /// <summary>
        /// 中毒类型 - 不能使用技能
        /// </summary>
        public const byte POISON_LOCKSPELL = 2;
        /// <summary>
        /// 中毒类型 - 禁止移动
        /// </summary>
        public const byte POISON_DONTMOVE = 4;
        /// <summary>
        /// 中毒类型 - 防麻
        /// </summary>
        public const byte POISON_STONE = 5;
        /// <summary>
        /// 不能跑动(中蛛网)
        /// </summary>
        public const byte STATE_LOCKRUN = 3;
        public const byte POISON_68 = 68;
        public const byte STATE_FASTMOVE = 7;
        /// <summary>
        /// 隐身
        /// </summary>
        public const byte STATE_TRANSPARENT = 8;
        /// <summary>
        /// 神圣战甲术  防御力
        /// </summary>
        public const byte STATE_DEFENCEUP = 9;
        /// <summary>
        /// 幽灵盾  魔御力
        /// </summary>
        public const byte STATE_MAGDEFENCEUP = 10;
        /// <summary>
        /// 魔法盾
        /// </summary>
        public const byte STATE_BUBBLEDEFENCEUP = 11;
        /// <summary>
        /// 被石化
        /// </summary>
        public const int STATE_STONE_MODE = 0x00000001;
        public const int STATE_OPENHEATH = 0x00000002;
    }
}