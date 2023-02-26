using GameSrv.Actor;

namespace GameSrv.Monster.Monsters {
    public class BigHeartMonster : AnimalObject {
        public BigHeartMonster() : base() {
            ViewRange = 16;
            Animal = false;
        }

        protected virtual bool AttackTarget() {
            bool result = false;
            if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime) {
                AttackTick = HUtil32.GetTickCount();
                SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                int nPower = HUtil32._MAX(0, HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1));
                for (int i = 0; i < VisibleActors.Count; i++) {
                    BaseObject baseObject = VisibleActors[i].BaseObject;
                    if (baseObject.Death) {
                        continue;
                    }
                    if (IsProperTarget(baseObject)) {
                        if (Math.Abs(CurrX - baseObject.CurrX) <= ViewRange && Math.Abs(CurrY - baseObject.CurrY) <= ViewRange) {
                            SendDelayMsg(this, Messages.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(baseObject.CurrX, baseObject.CurrY), 1, baseObject.ActorId, "", 200);
                            SendRefMsg(Messages.RM_10205, 0, baseObject.CurrX, baseObject.CurrY, 1, "");
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        public override void Run() {
            if (CanMove()) {
                if (VisibleActors.Count > 0) {
                    AttackTarget();
                }
            }
            base.Run();
        }
    }
}

