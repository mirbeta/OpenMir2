using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class MagicMonObject : MonsterObject
    {
        public bool MBoUseMagic;

        public MagicMonObject() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            MBoUseMagic = false;
        }

        private void LightingAttack(int nDir)
        {

        }

        public override void Run()
        {
            if (!Death && !Ghost && StatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (Abil.HP < Abil.MaxHP / 2)// 血量低于一半时开始用魔法攻击
                {
                    MBoUseMagic = true;
                }
                else
                {
                    MBoUseMagic = false;
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
                    if (MBoUseMagic || nX == 5 || nY == 5)
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

