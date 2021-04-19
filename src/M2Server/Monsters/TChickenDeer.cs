using System;
namespace M2Server
{
    public class TChickenDeer : TMonster
    {
        public TChickenDeer() : base()
        {
            m_nViewRange = 5;
        }

        public override void Run()
        {
            int nC;
            int n10;
            int n14;
            TBaseObject BaseObject1C;
            TBaseObject BaseObject;
            n10 = 9999;
            BaseObject = null;
            BaseObject1C = null;
            // 5 0x6A
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[grobal2.POISON_STONE] == 0)
            {

                if (HUtil32.GetTickCount() - m_dwWalkTick >= m_nWalkSpeed)
                {
                    for (var i = 0; i < m_VisibleActors.Count; i++)
                    {
                        BaseObject = m_VisibleActors[i].BaseObject;
                        if (BaseObject.m_boDeath)
                        {
                            continue;
                        }
                        if (IsProperTarget(BaseObject))
                        {
                            if (!BaseObject.m_boHideMode || m_boCoolEye)
                            {
                                nC = Math.Abs(m_nCurrX - BaseObject.m_nCurrX) + Math.Abs(m_nCurrY - BaseObject.m_nCurrY);
                                if (nC < n10)
                                {
                                    n10 = nC;
                                    BaseObject1C = BaseObject;
                                }
                            }
                        }
                    }
                    // for
                    if (BaseObject1C != null)
                    {
                        m_boRunAwayMode = true;
                        m_TargetCret = BaseObject1C;
                    }
                    else
                    {
                        m_boRunAwayMode = false;
                        m_TargetCret = null;
                    }
                }

                if (m_boRunAwayMode && m_TargetCret != null && HUtil32.GetTickCount() - m_dwWalkTick >= m_nWalkSpeed)
                {
                    if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) <= 6 && Math.Abs(m_nCurrX - BaseObject.m_nCurrX) <= 6)
                    {
                        n14 = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                        m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, n14, 5, ref m_nTargetX, ref m_nTargetY);
                    }
                }
            }
            base.Run();
        }
    }
}

