using SystemModule.Consts;

namespace GameSrv.Monster.Monsters {
    public class RedMonster : MonsterObject {
        public RedMonster() : base() {
            SearchTime = GameShare.RandomNumber.Random(1500) + 1500;
        }

        public override void Run() {
            if (CanMove()) {
                if (TargetCret != null) {
                    TargetX = TargetCret.CurrX;
                    TargetY = TargetCret.CurrY;
                    if (Math.Abs(TargetX - CurrX) == 1 && Math.Abs(TargetY - CurrY) == 1) {
                        if (GameShare.RandomNumber.Random(TargetCret.AntiPoison + 7) <= 6 && TargetCret.StatusTimeArr[PoisonState.DECHEALTH] == 0) {
                            TargetCret.MakePosion(PoisonState.DAMAGEARMOR, 30, 1);
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null) {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

