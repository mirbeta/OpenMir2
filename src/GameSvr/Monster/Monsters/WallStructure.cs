namespace GameSvr.Monster.Monsters
{
    public class WallStructure : GuardUnit
    {
        internal bool SetMapFlaged;

        public WallStructure() : base()
        {
            this.Animal = false;
            this.StickMode = true;
            SetMapFlaged = false;
            this.AntiPoison = 200;
        }

        public override void Initialize()
        {
            this.Direction = 0;
            base.Initialize();
        }

        public void RefStatus()
        {
            byte n08;
            if (this.WAbil.HP > 0)
            {
                n08 = (byte)(3 - HUtil32.Round(this.WAbil.HP / this.WAbil.MaxHP * 3));
            }
            else
            {
                n08 = 4;
            }
            if (n08 >= 5)
            {
                n08 = 0;
            }
            this.Direction = n08;
            this.SendRefMsg(Grobal2.RM_ALIVE, this.Direction, this.CurrX, this.CurrY, 0, "");
        }

        public override void Die()
        {
            base.Die();
        }

        public override void Run()
        {
            int n08;
            if (this.Death)
            {
                this.DeathTick = HUtil32.GetTickCount();
                if (SetMapFlaged)
                {
                    this.Envir.SetMapXyFlag(this.CurrX, this.CurrY, true);
                    SetMapFlaged = false;
                }
            }
            else
            {
                this.HealthTick = 0;
                if (!SetMapFlaged)
                {
                    this.Envir.SetMapXyFlag(this.CurrX, this.CurrY, false);
                    SetMapFlaged = true;
                }
            }
            if (this.WAbil.HP > 0)
            {
                n08 = 3 - HUtil32.Round(this.WAbil.HP / this.WAbil.MaxHP * 3);
            }
            else
            {
                n08 = 4;
            }
            if (this.Direction != n08 && n08 < 5)
            {
                this.Direction = (byte)n08;
                this.SendRefMsg(Grobal2.RM_DIGUP, this.Direction, this.CurrX, this.CurrY, 0, "");
            }
            base.Run();
        }
    }
}

