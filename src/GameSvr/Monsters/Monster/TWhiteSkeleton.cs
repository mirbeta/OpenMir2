using SystemModule;

namespace GameSvr
{
    public class TWhiteSkeleton: TATMonster
    {
        public bool m_boIsFirst = false;

        public TWhiteSkeleton() : base()
        {
            m_boIsFirst = true;
            this.m_boFixedHideMode = true;
            this.m_nViewRange = 6;
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            sub_4AAD54();
        }

        public override void Run()
        {
            if (m_boIsFirst)
            {
                m_boIsFirst = false;
                this.m_btDirection = 5;
                this.m_boFixedHideMode = false;
                this.SendRefMsg(Grobal2.RM_DIGUP, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
            }
            base.Run();
        }

        private void sub_4AAD54()
        {
            this.m_nNextHitTime = 3000 - this.m_btSlaveMakeLevel * 600;
            this.m_nWalkSpeed = 1200 - this.m_btSlaveMakeLevel * 250;
            this.m_dwWalkTick = HUtil32.GetTickCount() + 2000;
        }
    } 
}

