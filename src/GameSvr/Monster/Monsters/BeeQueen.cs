using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class BeeQueen : AnimalObject
    {
        private readonly IList<BaseObject> BeeList;

        public BeeQueen() : base()
        {
            ViewRange = 9;
            RunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = HUtil32.GetTickCount();
            StickMode = true;
            BeeList = new List<BaseObject>();
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

        protected override bool Operate(ProcessMessage processMsg)
        {
            if (processMsg.wIdent == Grobal2.RM_ZEN_BEE)
            {
                var bb = M2Share.WorldEngine.RegenMonsterByName(Envir.MapName, CurrX, CurrY, M2Share.Config.Bee);
                if (bb != null)
                {
                    bb.SetTargetCreat(TargetCret);
                    BeeList.Add(bb);
                }
            }
            return base.Operate(processMsg);
        }

        public override void Run()
        {
            if (CanWalk())
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
                        var bb = BeeList[i];
                        if (bb.Death || bb.Ghost)
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

