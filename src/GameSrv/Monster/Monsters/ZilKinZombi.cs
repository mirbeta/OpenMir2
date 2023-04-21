using SystemModule.Consts;

namespace GameSrv.Monster.Monsters {
    public class ZilKinZombi : AtMonster {
        //private int ReAliveTick;
        private int ZilKillCount;
        private int ZilkinDieTick;

        public ZilKinZombi() : base() {
            this.ViewRange = 6;
            this.SearchTime = GameShare.RandomNumber.Random(1500) + 2500;
            this.SearchTick = HUtil32.GetTickCount();
            this.ZilKillCount = 0;
            if (GameShare.RandomNumber.Random(3) == 0) {
                ZilKillCount = GameShare.RandomNumber.Random(3) + 1;
            }
        }

        public override void Die() {
            base.Die();
            if (ZilKillCount > 0) {
                ReAliveTick = HUtil32.GetTickCount();
                ZilkinDieTick = (GameShare.RandomNumber.Random(20) + 4) * 1000;
            }
            ZilKillCount -= 1;
        }

        public override void Run() {
            if (this.Death && !this.Ghost && this.StatusTimeArr[PoisonState.STONE] == 0 && ZilKillCount >= 0 && this.VisibleActors.Count > 0 && (HUtil32.GetTickCount() - ReAliveTick) >= ZilkinDieTick) {
                this.Abil.MaxHP = (ushort)(this.Abil.MaxHP >> 1);
                this.FightExp = this.FightExp / 2;
                this.Abil.HP = this.Abil.MaxHP;
                this.WAbil.HP = this.Abil.MaxHP;
                this.ReAlive();
                this.WalkTick = HUtil32.GetTickCount() + 1000;
            }
            base.Run();
        }
    }
}