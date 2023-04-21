namespace GameSrv.Monster.Monsters {
    public class TeleMonster : MonsterObject {
        public TeleMonster() : base() {
            this.SearchTime = GameShare.RandomNumber.Random(1500) + 1500;
        }

        public override void Run() {
            if (CanMove()) {
                if (this.TargetCret != null) {
                    if (Math.Abs(this.CurrX - this.TargetX) > 5 || Math.Abs(this.CurrY - this.TargetY) > 5) {
                        this.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        this.SpaceMove(this.TargetCret.MapName, this.TargetCret.CurrX, this.TargetCret.CurrY, 0);
                    }
                }
                if ((HUtil32.GetTickCount() - this.SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - this.SearchEnemyTick) > 1000 && this.TargetCret == null) {
                    this.SearchEnemyTick = HUtil32.GetTickCount();
                    this.SearchTarget();
                }
            }
            base.Run();
        }
    }
}

