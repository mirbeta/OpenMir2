using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class SandMobObject : StickMonster
    {
        private int m_dwAppearStart;

        public SandMobObject() : base()
        {
            nComeOutValue = 8;
        }

        public override void Run()
        {
            if (!Death && !Ghost)
            {
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    if (FixedHideMode)
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
                        else if (TargetCret != null)
                        {
                            if (Math.Abs(CurrX - TargetCret.CurrX) > 15 && Math.Abs(CurrY - TargetCret.CurrY) > 15)
                            {
                                ComeDown();
                                return;
                            }
                        }
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                        {
                            SearchTarget();
                        }
                        if (!FixedHideMode)
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

