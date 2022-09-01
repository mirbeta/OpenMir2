using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class BeeQueen : AnimalObject
    {
        private readonly IList<TBaseObject> BBList;

        public BeeQueen() : base()
        {
            m_nViewRange = 9;
            m_nRunTime = 250;
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            m_dwSearchTick = HUtil32.GetTickCount();
            m_boStickMode = true;
            BBList = new List<TBaseObject>();
        }

        private void MakeChildBee()
        {
            if (BBList.Count >= 15)
            {
                return;
            }
            SendRefMsg(Grobal2.RM_HIT, Direction, m_nCurrX, m_nCurrY, 0, "");
            SendDelayMsg(this, Grobal2.RM_ZEN_BEE, 0, 0, 0, 0, "", 500);
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wIdent == Grobal2.RM_ZEN_BEE)
            {
                var BB = M2Share.UserEngine.RegenMonsterByName(m_PEnvir.MapName, m_nCurrX, m_nCurrY, M2Share.g_Config.sBee);
                if (BB != null)
                {
                    BB.SetTargetCreat(m_TargetCret);
                    BBList.Add(BB);
                }
            }
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
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
                            MakeChildBee();
                        }
                    }
                    for (var i = BBList.Count - 1; i >= 0; i--)
                    {
                        var BB = BBList[i];
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

