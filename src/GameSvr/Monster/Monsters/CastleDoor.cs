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
            Animal = false;
            m_boStickMode = true;
            m_boOpened = false;
            m_btAntiPoison = 200;
        }

        private void SetMapXYFlag(int nFlag)
        {
            bool bo06;
            m_PEnvir.SetMapXyFlag(CurrX, CurrY - 2, true);
            m_PEnvir.SetMapXyFlag(CurrX + 1, CurrY - 1, true);
            m_PEnvir.SetMapXyFlag(CurrX + 1, CurrY - 2, true);
            if (nFlag == 1)
            {
                bo06 = false;
            }
            else
            {
                bo06 = true;
            }
            m_PEnvir.SetMapXyFlag(CurrX, CurrY, bo06);
            m_PEnvir.SetMapXyFlag(CurrX, CurrY - 1, bo06);
            m_PEnvir.SetMapXyFlag(CurrX, CurrY - 2, bo06);
            m_PEnvir.SetMapXyFlag(CurrX + 1, CurrY - 1, bo06);
            m_PEnvir.SetMapXyFlag(CurrX + 1, CurrY - 2, bo06);
            m_PEnvir.SetMapXyFlag(CurrX - 1, CurrY, bo06);
            m_PEnvir.SetMapXyFlag(CurrX - 2, CurrY, bo06);
            m_PEnvir.SetMapXyFlag(CurrX - 1, CurrY - 1, bo06);
            m_PEnvir.SetMapXyFlag(CurrX - 1, CurrY + 1, bo06);
            if (nFlag == 0)
            {
                m_PEnvir.SetMapXyFlag(CurrX, CurrY - 2, false);
                m_PEnvir.SetMapXyFlag(CurrX + 1, CurrY - 1, false);
                m_PEnvir.SetMapXyFlag(CurrX + 1, CurrY - 2, false);
            }
        }

        public void Open()
        {
            if (Death)
            {
                return;
            }
            Direction = 7;
            SendRefMsg(Grobal2.RM_DIGUP, Direction, CurrX, CurrY, 0, "");
            m_boOpened = true;
            StoneMode = true;
            SetMapXYFlag(0);
            bo2B9 = false;
        }

        public void Close()
        {
            if (Death)
            {
                return;
            }
            Direction = (byte)(3 - HUtil32.Round(m_WAbil.HP / m_WAbil.MaxHP * 3.0));
            if (Direction - 3 >= 0)
            {
                Direction = 0;
            }
            SendRefMsg(Grobal2.RM_DIGDOWN, Direction, CurrX, CurrY, 0, "");
            m_boOpened = false;
            StoneMode = false;
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
            if (Death && m_Castle != null)
            {
                DeathTick = HUtil32.GetTickCount();
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
                    SendRefMsg(Grobal2.RM_TURN, Direction, CurrX, CurrY, 0, "");
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
            SendRefMsg(Grobal2.RM_ALIVE, Direction, CurrX, CurrY, 0, "");
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}

