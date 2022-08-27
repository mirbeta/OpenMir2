using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class SpiderHouseMonster : AnimalObject
    {
        public int n54C = 0;
        private readonly IList<TBaseObject> BBList = null;

        public SpiderHouseMonster() : base()
        {
            m_nViewRange = 9;
            m_nRunTime = 250;
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            m_dwSearchTick = 0;
            m_boStickMode = true;
            BBList = new List<TBaseObject>();
        }

        private void GenBB()
        {
            if (BBList.Count < 15)
            {
                SendRefMsg(Grobal2.RM_HIT, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                SendDelayMsg(this, Grobal2.RM_ZEN_BEE, 0, 0, 0, 0, "", 500);
            }
        }

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            TBaseObject BB;
            short n08 = 0;
            short n0C = 0;
            if (ProcessMsg.wIdent == Grobal2.RM_ZEN_BEE)
            {
                n08 = m_nCurrX;
                n0C = (short)(m_nCurrY + 1);
                if (m_PEnvir.CanWalk(n08, n0C, true))
                {
                    BB = M2Share.UserEngine.RegenMonsterByName(m_PEnvir.SMapName, n08, n0C, M2Share.g_Config.sSpider);
                    if (BB != null)
                    {
                        BB.SetTargetCreat(m_TargetCret);
                        BBList.Add(BB);
                    }
                }
            }
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            TBaseObject BB;
            if (!m_boGhost && !m_boDeath && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - m_dwWalkTick) >= m_nWalkSpeed)
                {
                    m_dwWalkTick = HUtil32.GetTickCount();
                    if ((HUtil32.GetTickCount() - m_dwHitTick) >= m_nNextHitTime)
                    {
                        m_dwHitTick = HUtil32.GetTickCount();
                        SearchTarget();
                        if (m_TargetCret != null)
                        {
                            GenBB();
                        }
                    }
                    for (var i = BBList.Count - 1; i >= 0; i--)
                    {
                        BB = BBList[i];
                        if (BB.m_boDeath || BB.m_boGhost)
                        {
                            BBList.RemoveAt(i);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

