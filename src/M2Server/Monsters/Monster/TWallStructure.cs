using System;
using SystemModule;

namespace M2Server
{
    public class TWallStructure: TGuardUnit
    {
        public int n55C = 0;
        private int dw560 = 0;
        private bool boSetMapFlaged = false;

        public TWallStructure() : base()
        {
            this.m_boAnimal = false;
            this.m_boStickMode = true;
            boSetMapFlaged = false;
            this.m_btAntiPoison = 200;
        }

        public override void Initialize()
        {
            this.m_btDirection = 0;
            base.Initialize();
        }

        public void RefStatus()
        {
            byte n08;
            if (this.m_WAbil.HP > 0)
            {
                n08 = (byte)(3 - HUtil32.Round(this.m_WAbil.HP / this.m_WAbil.MaxHP * 3.0));
            }
            else
            {
                n08 = 4;
            }
            if (n08 >= 5)
            {
                n08 = 0;
            }
            this.m_btDirection = n08;
            this.SendRefMsg(Grobal2.RM_ALIVE, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
        }

        public override void Die()
        {
            base.Die();
            dw560 = HUtil32.GetTickCount();
        }

        public override void Run()
        {
            int n08;
            if (this.m_boDeath)
            {
                this.m_dwDeathTick =HUtil32.GetTickCount();
                if (boSetMapFlaged)
                {
                    this.m_PEnvir.SetMapXYFlag(this.m_nCurrX, this.m_nCurrY, true);
                    boSetMapFlaged = false;
                }
            }
            else
            {
                this.m_nHealthTick = 0;
                if (!boSetMapFlaged)
                {
                    this.m_PEnvir.SetMapXYFlag(this.m_nCurrX, this.m_nCurrY, false);
                    boSetMapFlaged = true;
                }
            }
            if (this.m_WAbil.HP > 0)
            {
                n08 = 3 - HUtil32.Round(this.m_WAbil.HP / this.m_WAbil.MaxHP * 3.0);
            }
            else
            {
                n08 = 4;
            }
            if (this.m_btDirection != n08 && n08 < 5)
            {
                this.m_btDirection = (byte)n08;
                this.SendRefMsg(Grobal2.RM_DIGUP, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
            }
            base.Run();
        }
    }
}

