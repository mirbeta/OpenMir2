﻿namespace M2Server
{
    public class TElfWarriorMonster : TSpitSpider
    {
        private bool boIsFirst = false;
        private int dwDigDownTick = 0;

        public void AppearNow()
        {
            boIsFirst = false;
            m_boFixedHideMode = false;
            SendRefMsg(grobal2.RM_DIGUP, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
            RecalcAbilitys();
            m_dwWalkTick = m_dwWalkTick + 800;
            dwDigDownTick = HUtil32.GetTickCount();
        }

        public TElfWarriorMonster()
            : base()
        {
            m_nViewRange = 6;
            m_boFixedHideMode = true;
            boIsFirst = true;
            m_boUsePoison = false;
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            ResetElfMon();
        }

        private void ResetElfMon()
        {
            m_nNextHitTime = 1500 - m_btSlaveMakeLevel * 100;
            m_nWalkSpeed = 500 - m_btSlaveMakeLevel * 50;
            m_dwWalkTick = HUtil32.GetTickCount() + 2000;
        }

        public override void Run()
        {
            bool boChangeFace;
            TBaseObject ElfMon = null;
            string ElfName;
            if (boIsFirst)
            {
                boIsFirst = false;
                m_boFixedHideMode = false;
                SendRefMsg(grobal2.RM_DIGUP, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                ResetElfMon();
            }
            if (m_boDeath)
            {
                if (HUtil32.GetTickCount() - m_dwDeathTick > 2 * 1000)
                {
                    MakeGhost();
                }
            }
            else
            {
                boChangeFace = true;
                if (m_TargetCret != null)
                {
                    boChangeFace = false;
                }
                if ((m_Master != null) && ((m_Master.m_TargetCret != null) || (m_Master.m_LastHiter != null)))
                {
                    boChangeFace = false;
                }
                if (boChangeFace)
                {
                    if ((HUtil32.GetTickCount() - dwDigDownTick) > 6 * 10 * 1000)
                    {
                        ElfName = m_sCharName;
                        if (ElfName[ElfName.Length - 1] == '1')
                        {
                            ElfName = ElfName.Substring(0, ElfName.Length - 1);
                            ElfMon = MakeClone(ElfName, this);
                        }
                        if (ElfMon != null)
                        {
                            SendRefMsg(grobal2.RM_DIGDOWN, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                            SendRefMsg(grobal2.RM_CHANGEFACE, 0, ObjectId, ElfMon.ObjectId, 0, "");
                            ElfMon.m_boAutoChangeColor = m_boAutoChangeColor;
                            if (ElfMon is TElfMonster)
                            {
                                (ElfMon as TElfMonster).AppearNow();
                            }
                            m_Master = null;
                            KickException();
                        }
                    }
                }
                else
                {
                    dwDigDownTick = HUtil32.GetTickCount();
                }
            }
            base.Run();
        }
    }
}