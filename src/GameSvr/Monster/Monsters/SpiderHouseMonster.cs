using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class SpiderHouseMonster : AnimalObject
    {
        private readonly IList<TBaseObject> BBList;

        public SpiderHouseMonster() : base()
        {
            ViewRange = 9;
            m_nRunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = 0;
            m_boStickMode = true;
            BBList = new List<TBaseObject>();
        }

        private void GenBB()
        {
            if (BBList.Count < 15)
            {
                SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
                SendDelayMsg(this, Grobal2.RM_ZEN_BEE, 0, 0, 0, 0, "", 500);
            }
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            TBaseObject BB;
            short n08 = 0;
            short n0C = 0;
            if (ProcessMsg.wIdent == Grobal2.RM_ZEN_BEE)
            {
                n08 = CurrX;
                n0C = (short)(CurrY + 1);
                if (Envir.CanWalk(n08, n0C, true))
                {
                    BB = M2Share.UserEngine.RegenMonsterByName(Envir.MapName, n08, n0C, M2Share.Config.sSpider);
                    if (BB != null)
                    {
                        BB.SetTargetCreat(TargetCret);
                        BBList.Add(BB);
                    }
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
                            GenBB();
                        }
                    }
                    for (var i = BBList.Count - 1; i >= 0; i--)
                    {
                        var BB = BBList[i];
                        if (BB.Death || BB.Ghost)
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

