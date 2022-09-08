using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class BeeQueen : AnimalObject
    {
        private readonly IList<BaseObject> _beeList;

        public BeeQueen() : base()
        {
            ViewRange = 9;
            RunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = HUtil32.GetTickCount();
            StickMode = true;
            _beeList = new List<BaseObject>();
        }

        private void MakeChildBee()
        {
            if (_beeList.Count >= 15)
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
                var bb = M2Share.UserEngine.RegenMonsterByName(Envir.MapName, CurrX, CurrY, M2Share.Config.Bee);
                if (bb != null)
                {
                    bb.SetTargetCreat(TargetCret);
                    _beeList.Add(bb);
                }
            }
            return base.Operate(processMsg);
        }

        public override void Run()
        {
            if (!Ghost && !Death && StatusTimeArr[Grobal2.POISON_STONE] == 0)
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
                    for (var i = _beeList.Count - 1; i >= 0; i--)
                    {
                        var bb = _beeList[i];
                        if (bb.Death || bb.Ghost)
                        {
                            _beeList.RemoveAt(i);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

