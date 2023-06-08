using SystemModule;

namespace M2Server.Monster.Monsters {
    public class WallStructure : GuardUnit {
        internal bool SetMapFlaged;

        public WallStructure() : base() {
            this.Animal = false;
            this.StickMode = true;
            SetMapFlaged = false;
            this.AntiPoison = 200;
        }

        public override void Initialize() {
            this.Dir = 0;
            base.Initialize();
        }

        public void RefStatus() {
            byte n08;
            if (this.WAbil.HP > 0) {
                n08 = (byte)(3 - HUtil32.Round(this.WAbil.HP / this.WAbil.MaxHP * 3.0));
            }
            else {
                n08 = 4;
            }
            if (n08 >= 5) {
                n08 = 0;
            }
            this.Dir = n08;
            this.SendRefMsg(Messages.RM_ALIVE, this.Dir, this.CurrX, this.CurrY, 0, "");
        }

        public override void Die() {
            base.Die();
        }

        public override void Run() {
            int n08;
            if (this.Death) {
                this.DeathTick = HUtil32.GetTickCount();
                if (SetMapFlaged) {
                    this.Envir.SetMapXyFlag(this.CurrX, this.CurrY, true);
                    SetMapFlaged = false;
                }
            }
            else {
                this.HealthTick = 0;
                if (!SetMapFlaged) {
                    this.Envir.SetMapXyFlag(this.CurrX, this.CurrY, false);
                    SetMapFlaged = true;
                }
            }
            if (this.WAbil.HP > 0) {
                n08 = 3 - HUtil32.Round(this.WAbil.HP / this.WAbil.MaxHP * 3.0);
            }
            else {
                n08 = 4;
            }
            if (this.Dir != n08 && n08 < 5) {
                this.Dir = (byte)n08;
                this.SendRefMsg(Messages.RM_DIGUP, this.Dir, this.CurrX, this.CurrY, 0, "");
            }
            base.Run();
        }
    }
}

