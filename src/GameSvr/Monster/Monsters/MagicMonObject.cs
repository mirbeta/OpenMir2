namespace GameSvr.Monster.Monsters
{
    public class MagicMonObject : MonsterObject
    {
        /// <summary>
        /// 是否使用魔法攻击
        /// </summary>
        public bool UseMagic;

        public MagicMonObject() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            UseMagic = false;
        }

        private void LightingAttack(int nDir)
        {

        }

        public override void Run()
        {
            if (CanMove())
            {
                if (WAbil.HP < WAbil.MaxHP / 2)// 血量低于一半时开始用魔法攻击
                {
                    UseMagic = true;
                }
                else
                {
                    UseMagic = false;
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if (Master == null)
                {
                    return;
                }
                var nX = Math.Abs(CurrX - Master.CurrX);
                var nY = Math.Abs(CurrY - Master.CurrY);
                if (nX <= 5 && nY <= 5)
                {
                    if (UseMagic || nX == 5 || nY == 5)
                    {
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                        {
                            AttackTick = HUtil32.GetTickCount();
                            int nAttackDir = M2Share.GetNextDirection(CurrX, CurrY, Master.CurrX, Master.CurrY);
                            LightingAttack(nAttackDir);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

