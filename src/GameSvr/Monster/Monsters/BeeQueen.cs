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
            MNRunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = HUtil32.GetTickCount();
            MBoStickMode = true;
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

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wIdent == Grobal2.RM_ZEN_BEE)
            {
                var BB = M2Share.UserEngine.RegenMonsterByName(Envir.MapName, CurrX, CurrY, M2Share.Config.sBee);
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
            if (!Ghost && !Death && MWStatusTimeArr[Grobal2.POISON_STONE] == 0)
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

