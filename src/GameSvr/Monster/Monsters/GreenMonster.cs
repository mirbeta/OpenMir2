using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class GreenMonster : MonsterObject
    {
        public GreenMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (m_TargetCret != null)
                {
                    m_nTargetX = m_TargetCret.m_nCurrX;
                    m_nTargetY = m_TargetCret.m_nCurrY;
                    if (Math.Abs(m_nTargetX - m_nCurrX) == 1 && Math.Abs(m_nTargetY - m_nCurrY) == 1)
                    {
                        if (M2Share.RandomNumber.Random(m_TargetCret.m_btAntiPoison + 7) <= 6 && m_TargetCret.m_wStatusTimeArr[Grobal2.POISON_DECHEALTH] == 0)
                        {
                            m_TargetCret.MakePosion(Grobal2.POISON_DECHEALTH, 30, 1);
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}