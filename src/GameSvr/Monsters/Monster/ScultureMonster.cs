using SystemModule;

namespace GameSvr
{
    public class ScultureMonster : Monster
    {
        public ScultureMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            m_nViewRange = 7;
            m_boStoneMode = true;
            m_nCharStatusEx = Grobal2.STATE_STONE_MODE;
        }

        private void MeltStone()
        {
            m_nCharStatusEx = 0;
            m_nCharStatus = GetCharStatus();
            SendRefMsg(Grobal2.RM_DIGUP, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
            m_boStoneMode = false;
        }

        private void MeltStoneAll()
        {
            TBaseObject BaseObject;
            MeltStone();
            IList<TBaseObject> List10 = new List<TBaseObject>();
            GetMapBaseObjects(m_PEnvir, m_nCurrX, m_nCurrY, 7, List10);
            for (var i = 0; i < List10.Count; i++)
            {
                BaseObject = List10[i];
                if (BaseObject.m_boStoneMode)
                {
                    if (BaseObject is ScultureMonster)
                    {
                        (BaseObject as ScultureMonster).MeltStone();
                    }
                }
            }
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (!m_boGhost && !m_boDeath && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - m_dwWalkTick) >= m_nWalkSpeed)
            {
                if (m_boStoneMode)
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
                                if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) <= 2 && Math.Abs(m_nCurrY - BaseObject.m_nCurrY) <= 2)
                                {
                                    MeltStoneAll();
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

