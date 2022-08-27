using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class SandMobObject : StickMonster
    {
        private int m_dwAppearStart = 0;

        public SandMobObject() : base()
        {
            nComeOutValue = 8;
        }

        public override void Run()
        {
            if (!m_boDeath && !m_boGhost)
            {
                if ((HUtil32.GetTickCount() - m_dwWalkTick) > m_nWalkSpeed)
                {
                    m_dwWalkTick = HUtil32.GetTickCount();
                    if (m_boFixedHideMode)
                    {
                        if (m_WAbil.HP > m_WAbil.MaxHP / 20 && CheckComeOut())
                        {
                            m_dwAppearStart = HUtil32.GetTickCount();
                        }
                    }
                    else
                    {
                        if (m_WAbil.HP > 0 && m_WAbil.HP < m_WAbil.MaxHP / 20 && (HUtil32.GetTickCount() - m_dwAppearStart) > 3000)
                        {
                            ComeDown();
                        }
                        else if (m_TargetCret != null)
                        {
                            if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) > 15 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) > 15)
                            {
                                ComeDown();
                                return;
                            }
                        }
                        if ((HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
                        {
                            SearchTarget();
                        }
                        if (!m_boFixedHideMode)
                        {
                            if (AttackTarget())
                            {
                                base.Run();
                            }
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

