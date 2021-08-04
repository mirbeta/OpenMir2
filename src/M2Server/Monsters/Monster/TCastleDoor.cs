using SystemModule;

namespace M2Server
{
    /// <summary>
    /// 沙巴克城门
    /// </summary>
    public class TCastleDoor : TGuardUnit
    {
        public int dw55C = 0;
        private int dw560 = 0;
        public bool m_boOpened = false;
        public bool bo565n = false;
        public bool bo566n = false;
        public bool bo567n = false;

        public TCastleDoor() : base()
        {
            m_boAnimal = false;
            m_boStickMode = true;
            m_boOpened = false;
            m_btAntiPoison = 200;
        }

        private void SetMapXYFlag(int nFlag)
        {
            bool bo06;
            m_PEnvir.SetMapXYFlag(m_nCurrX, m_nCurrY - 2, true);
            m_PEnvir.SetMapXYFlag(m_nCurrX + 1, m_nCurrY - 1, true);
            m_PEnvir.SetMapXYFlag(m_nCurrX + 1, m_nCurrY - 2, true);
            if (nFlag == 1)
            {
                bo06 = false;
            }
            else
            {
                bo06 = true;
            }
            m_PEnvir.SetMapXYFlag(m_nCurrX, m_nCurrY, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX, m_nCurrY - 1, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX, m_nCurrY - 2, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX + 1, m_nCurrY - 1, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX + 1, m_nCurrY - 2, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX - 1, m_nCurrY, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX - 2, m_nCurrY, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX - 1, m_nCurrY - 1, bo06);
            m_PEnvir.SetMapXYFlag(m_nCurrX - 1, m_nCurrY + 1, bo06);
            if (nFlag == 0)
            {
                m_PEnvir.SetMapXYFlag(m_nCurrX, m_nCurrY - 2, false);
                m_PEnvir.SetMapXYFlag(m_nCurrX + 1, m_nCurrY - 1, false);
                m_PEnvir.SetMapXYFlag(m_nCurrX + 1, m_nCurrY - 2, false);
            }
        }

        public void Open()
        {
            if (m_boDeath)
            {
                return;
            }
            m_btDirection = 7;
            SendRefMsg(Grobal2.RM_DIGUP, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
            m_boOpened = true;
            m_boStoneMode = true;
            SetMapXYFlag(0);
            bo2B9 = false;
        }

        public void Close()
        {
            if (m_boDeath)
            {
                return;
            }
            m_btDirection = (byte)(3 - HUtil32.Round(m_WAbil.HP / m_WAbil.MaxHP * 3.0));
            if (m_btDirection - 3 >= 0)
            {
                m_btDirection = 0;
            }
            SendRefMsg(Grobal2.RM_DIGDOWN, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
            m_boOpened = false;
            m_boStoneMode = false;
            SetMapXYFlag(1);
            bo2B9 = true;
        }

        public override void Die()
        {
            base.Die();
            dw560 = HUtil32.GetTickCount();
            SetMapXYFlag(2);
        }

        public override void Run()
        {
            int n08;
            if (m_boDeath && m_Castle != null)
            {
                m_dwDeathTick = HUtil32.GetTickCount();
            }
            else
            {
                m_nHealthTick = 0;
            }
            if (!m_boOpened)
            {
                n08 = 3 - HUtil32.Round(m_WAbil.HP / m_WAbil.MaxHP * 3.0);
                if (m_btDirection != n08 && n08 < 3)
                {
                    m_btDirection = (byte)n08;
                    SendRefMsg(Grobal2.RM_TURN, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                }
            }
            base.Run();
        }

        public void RefStatus()
        {
            var n08 = 3 - HUtil32.Round(m_WAbil.HP / m_WAbil.MaxHP * 3.0);
            if (n08 - 3 >= 0)
            {
                n08 = 0;
            }
            m_btDirection = (byte)n08;
            SendRefMsg(Grobal2.RM_ALIVE, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}

