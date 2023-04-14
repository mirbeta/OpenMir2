using GameSrv.Actor;
using SystemModule.Data;

namespace GameSrv.Monster.Monsters {
    public class BeeQueen : AnimalObject {
        private readonly IList<BaseObject> BeeList;

        public BeeQueen() : base() {
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
            SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
            SendSelfDelayMsg(Messages.RM_ZEN_BEE, 0, 0, 0, 0, "", 500);
        }

        protected override bool Operate(ProcessMessage processMsg) {
            if (processMsg.wIdent == Messages.RM_ZEN_BEE) {
                BaseObject bb = M2Share.WorldEngine.RegenMonsterByName(Envir.MapName, CurrX, CurrY, M2Share.Config.Bee);
                if (bb != null) {
                    bb.SetTargetCreat(TargetCret);
                    BeeList.Add(bb);
                }
            }
            return base.Operate(processMsg);
        }

        public override void Run() {
            if (CanMove()) {
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed) {
                    WalkTick = HUtil32.GetTickCount();
                    if ((HUtil32.GetTickCount() - AttackTick) >= NextHitTime) {
                        AttackTick = HUtil32.GetTickCount();
                        SearchTarget();
                        if (TargetCret != null) {
                            MakeChildBee();
                        }
                    }
                    for (int i = BeeList.Count - 1; i >= 0; i--) {
                        BaseObject bb = BeeList[i];
                        if (bb.Death || bb.Ghost) {
                            BeeList.RemoveAt(i);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

