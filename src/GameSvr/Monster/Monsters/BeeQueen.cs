using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class BeeQueen : AnimalObject
    {
        private readonly IList<TBaseObject> BeeList;

        public BeeQueen() : base()
        {
            ViewRange = 9;
            m_nRunTime = 250;
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            m_dwSearchTick = HUtil32.GetTickCount();
            m_boStickMode = true;
            BeeList = new List<TBaseObject>();
        }

        private void MakeChildBee()
        {
            if (BeeList.Count >= 15)
            {
                return;
            }
            SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
            SendDelayMsg(this, Grobal2.RM_ZEN_BEE, 0, 0, 0, 0, "", 500);
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wIdent == Grobal2.RM_ZEN_BEE)
            {
                var BB = M2Share.UserEngine.RegenMonsterByName(m_PEnvir.MapName, CurrX, CurrY, M2Share.g_Config.sBee);
                if (BB != null)
                {
                    BB.SetTargetCreat(TargetCret);
                    BeeList.Add(BB);
                }
            }
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            if (!Ghost && !Death && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    if ((HUtil32.GetTickCount() - AttackTick) >= NextHitTime)
                    {
                        AttackTick = HUtil32.GetTickCount();
                        SearchTarget();
                        if (TargetCret != null)
                        {
                            MakeChildBee();
                        }
                    }
                    for (var i = BeeList.Count - 1; i >= 0; i--)
                    {
                        var BB = BeeList[i];
                        if (BB.Death || BB.Ghost)
                        {
                            BeeList.RemoveAt(i);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

