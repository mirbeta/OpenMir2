using SystemModule;

namespace GameSvr.Monster.Monsters
{
    /// <summary>
    /// 沙巴克城门
    /// </summary>
    public class CastleDoor : GuardUnit
    {
        public int dw55C = 0;
        public bool m_boOpened = false;
        public bool bo565n = false;
        public bool bo566n = false;
        public bool bo567n = false;

        public CastleDoor() : base()
        {
            m_boAnimal = false;
            m_boStickMode = true;
            m_boOpened = false;
            m_btAntiPoison = 200;
        }

        private void SetMapXYFlag(int nFlag)
        {
            bool bo06;
            m_PEnvir.SetMapXyFlag(m_nCurrX, m_nCurrY - 2, true);
            m_PEnvir.SetMapXyFlag(m_nCurrX + 1, m_nCurrY - 1, true);
            m_PEnvir.SetMapXyFlag(m_nCurrX + 1, m_nCurrY - 2, true);
            if (nFlag == 1)
            {
                bo06 = false;
            }
            else
            {
                bo06 = true;
            }
            m_PEnvir.SetMapXyFlag(m_nCurrX, m_nCurrY, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX, m_nCurrY - 1, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX, m_nCurrY - 2, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX + 1, m_nCurrY - 1, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX + 1, m_nCurrY - 2, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX - 1, m_nCurrY, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX - 2, m_nCurrY, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX - 1, m_nCurrY - 1, bo06);
            m_PEnvir.SetMapXyFlag(m_nCurrX - 1, m_nCurrY + 1, bo06);
            if (nFlag == 0)
            {
                m_PEnvir.SetMapXyFlag(m_nCurrX, m_nCurrY - 2, false);
                m_PEnvir.SetMapXyFlag(m_nCurrX + 1, m_nCurrY - 1, false);
                m_PEnvir.SetMapXyFlag(m_nCurrX + 1, m_nCurrY - 2, false);
            }
        }

        public void Open()
        {
            if (m_boDeath)
            {
                return;
            }
            Direction = 7;
            SendRefMsg(Grobal2.RM_DIGUP, Direction, m_nCurrX, m_nCurrY, 0, "");
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
            Direction = (byte)(3 - HUtil32.Round(m_WAbil.HP / m_WAbil.MaxHP * 3.0));
            if (Direction - 3 >= 0)
            {
                Direction = 0;
            }
            SendRefMsg(Grobal2.RM_DIGDOWN, Direction, m_nCurrX, m_nCurrY, 0, "");
            m_boOpened = false;
            m_boStoneMode = false;
            SetMapXYFlag(1);
            bo2B9 = true;
        }

        public override void Die()
        {
            base.Die();
            SetMapXYFlag(2);
        }

        public override void Run()
        {
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
                int n08 = 3 - HUtil32.Round(m_WAbil.HP / m_WAbil.MaxHP * 3.0);
                if (Direction != n08 && n08 < 3)
                {
                    Direction = (byte)n08;
                    SendRefMsg(Grobal2.RM_TURN, Direction, m_nCurrX, m_nCurrY, 0, "");
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
            Direction = (byte)n08;
            SendRefMsg(Grobal2.RM_ALIVE, Direction, m_nCurrX, m_nCurrY, 0, "");
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}

