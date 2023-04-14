namespace SystemModule.Data
{
    public struct AddAbility
    {
        public ushort HP;
        public ushort MP;
        public ushort HIT;
        public ushort SPEED;
        public ushort AC;
        public ushort MAC;
        public ushort DC;
        public ushort MC;
        public ushort SC;
        public ushort AntiPoison;
        public ushort PoisonRecover;
        public ushort HealthRecover;
        public ushort SpellRecover;
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