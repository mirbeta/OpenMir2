using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class RedMonster : MonsterObject
    {
        public RedMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!Death && !bo554 && !Ghost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (TargetCret != null)
                {
                    m_nTargetX = TargetCret.CurrX;
                    m_nTargetY = TargetCret.CurrY;
                    if (Math.Abs(m_nTargetX - CurrX) == 1 && Math.Abs(m_nTargetY - CurrY) == 1)
                    {
                        if (M2Share.RandomNumber.Random(TargetCret.m_btAntiPoison + 7) <= 6 && TargetCret.m_wStatusTimeArr[Grobal2.POISON_DECHEALTH] == 0)
                        {
                            TargetCret.MakePosion(Grobal2.POISON_DAMAGEARMOR, 30, 1);
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

