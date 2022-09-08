﻿using SystemModule;

namespace GameSvr.Monster.Monsters
{
    /// <summary>
    /// 沙巴克城门
    /// </summary>
    public class CastleDoor : GuardUnit
    {
        public bool IsOpened;

        public CastleDoor() : base()
        {
            Animal = false;
            StickMode = true;
            IsOpened = false;
            AntiPoison = 200;
        }

        private void SetMapXyFlag(int nFlag)
        {
            bool bo06;
            Envir.SetMapXyFlag(CurrX, CurrY - 2, true);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, true);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, true);
            if (nFlag == 1)
            {
                bo06 = false;
            }
            else
            {
                bo06 = true;
            }
            Envir.SetMapXyFlag(CurrX, CurrY, bo06);
            Envir.SetMapXyFlag(CurrX, CurrY - 1, bo06);
            Envir.SetMapXyFlag(CurrX, CurrY - 2, bo06);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, bo06);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, bo06);
            Envir.SetMapXyFlag(CurrX - 1, CurrY, bo06);
            Envir.SetMapXyFlag(CurrX - 2, CurrY, bo06);
            Envir.SetMapXyFlag(CurrX - 1, CurrY - 1, bo06);
            Envir.SetMapXyFlag(CurrX - 1, CurrY + 1, bo06);
            if (nFlag == 0)
            {
                Envir.SetMapXyFlag(CurrX, CurrY - 2, false);
                Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, false);
                Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, false);
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
            IsOpened = true;
            StoneMode = true;
            SetMapXyFlag(0);
            Bo2B9 = false;
        }

        public void Close()
        {
            if (Death)
            {
                return;
            }
            Direction = (byte)(3 - HUtil32.Round(Abil.HP / Abil.MaxHP * 3));
            if (Direction - 3 >= 0)
            {
                Direction = 0;
            }
            SendRefMsg(Grobal2.RM_DIGDOWN, Direction, CurrX, CurrY, 0, "");
            IsOpened = false;
            StoneMode = false;
            SetMapXyFlag(1);
            Bo2B9 = true;
        }

        public override void Die()
        {
            base.Die();
            SetMapXyFlag(2);
        }

        public override void Run()
        {
            if (Death && Castle != null)
            {
                DeathTick = HUtil32.GetTickCount();
            }
            else
            {
                HealthTick = 0;
            }
            if (!IsOpened)
            {
                int n08 = 3 - HUtil32.Round(Abil.HP / Abil.MaxHP * 3);
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
            var n08 = 3 - HUtil32.Round(Abil.HP / Abil.MaxHP * 3);
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

