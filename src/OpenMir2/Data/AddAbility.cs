namespace SystemModule.Data
{
    public class AddAbility
    {
        /// <summary>
        /// 生命值
        /// </summary>
        public ushort HP;
        /// <summary>
        /// 法力值
        /// </summary>
        public ushort MP;
        /// <summary>
        /// 准确值
        /// </summary>
        public ushort HIT;
        /// <summary>
        /// 敏捷
        /// </summary>
        public ushort SPEED;
        /// <summary>
        /// 防御力
        /// </summary>
        public ushort AC;
        /// <summary>
        /// 魔法防御力
        /// </summary>
        public ushort MAC;
        /// <summary>
        /// 物理攻击力
        /// </summary>
        public ushort DC;
        /// <summary>
        /// 魔法力
        /// </summary>
        public ushort MC;
        /// <summary>
        /// 道术
        /// </summary>
        public ushort SC;
        /// <summary>
        /// 中毒躲避
        /// </summary>
        public ushort AntiPoison;
        /// <summary>
        /// 中毒恢复
        /// </summary>
        public ushort PoisonRecover;
        /// <summary>
        /// 体力恢复(HP)
        /// </summary>
        public ushort HealthRecover;
        /// <summary>
        /// 魔法恢复(MP)
        /// </summary>
        public ushort SpellRecover;
        /// <summary>
        /// 魔法躲避
        /// </summary>
        public ushort AntiMagic;
        /// <summary>
        /// 幸运点
        /// </summary>
        public byte Luck;
        /// <summary>
        /// 诅咒点或神圣点
        /// </summary>
        public byte UnLuck;
        /// <summary>
        /// 攻击强度
        /// </summary>
        public byte WeaponStrong;
        public byte UndeadPower;
        /// <summary>
        /// 攻击速度
        /// </summary>
        public ushort HitSpeed;
        public byte Slowdown;
        public byte Poison;
    }
}