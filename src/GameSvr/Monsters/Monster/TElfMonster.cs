using SystemModule;

namespace GameSvr
{
    public class TElfMonster : TMonster
    {
        private bool boIsFirst;

        public void AppearNow()
        {
            boIsFirst = false;
            m_boFixedHideMode = false;
            RecalcAbilitys();
            m_dwWalkTick = m_dwWalkTick + 800;
        }

        public TElfMonster() : base()
        {
            m_nViewRange = 6;
            m_boFixedHideMode = true;
            m_boNoAttackMode = true;
            boIsFirst = true;
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            ResetElfMon();
        }

        private void ResetElfMon()
        {
            m_nWalkSpeed = 500 - m_btSlaveMakeLevel * 50;
            m_dwWalkTick = HUtil32.GetTickCount() + 2000;
        }

        public override void Run()
        {
            bool boChangeFace;
            TBaseObject ElfMon;
            if (boIsFirst)
            {
                boIsFirst = false;
                m_boFixedHideMode = false;
                SendRefMsg(Grobal2.RM_DIGUP, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                ResetElfMon();
            }
            if (m_boDeath)
            {
                if ((HUtil32.GetTickCount() - m_dwDeathTick) > (2 * 1000))
                {
                    MakeGhost();
                }
            }
            else
            {
                boChangeFace = false;
                if (m_TargetCret != null)
                {
                    boChangeFace = true;
                }
                if (m_Master != null && (m_Master.m_TargetCret != null || m_Master.m_LastHiter != null))
                {
                    boChangeFace = true;
                }
                if (boChangeFace)
                {
                    ElfMon = MakeClone(M2Share.g_Config.sDragon1, this);
                    if (ElfMon != null)
                    {
                        ElfMon.m_boAutoChangeColor = m_boAutoChangeColor;
                        if (ElfMon is TElfWarriorMonster)
                        {
                            (ElfMon as TElfWarriorMonster).AppearNow();
                        }
                        m_Master = null;
                        KickException();
                    }
                }
            }
            base.Run();
        }
    }
}

