using System;
using SystemModule;

namespace GameSvr
{
    public class TDigOutZombi : TMonster
    {
        public TDigOutZombi() : base()
        {
            bo554 = false;
            m_nViewRange = 7;
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            m_dwSearchTick = HUtil32.GetTickCount();
            m_boFixedHideMode = true;
        }

        private void sub_4AA8DC()
        {
            var digEvent = new TEvent(m_PEnvir, m_nCurrX, m_nCurrY, 1, 5 * 60 * 1000, true);
            M2Share.EventManager.AddEvent(digEvent);
            m_boFixedHideMode = false;
            SendRefMsg(Grobal2.RM_DIGUP, m_btDirection, m_nCurrX, m_nCurrY, digEvent.Id, "");
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (!m_boGhost && !m_boDeath && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - m_dwWalkTick) > m_nWalkSpeed)
            {
                if (m_boFixedHideMode)
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
                                if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) <= 3 && Math.Abs(m_nCurrY - BaseObject.m_nCurrY) <= 3)
                                {
                                    sub_4AA8DC();
                                    m_dwWalkTick = HUtil32.GetTickCount() + 1000;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 1000 && m_TargetCret == null)
                    {
                        m_dwSearchEnemyTick = HUtil32.GetTickCount();
                        SearchTarget();
                    }
                }
            }
            base.Run();
        }
    }
}

