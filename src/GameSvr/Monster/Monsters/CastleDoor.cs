using SystemModule;

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
            Envir.SetMapXyFlag(CurrX, CurrY - 2, true);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, true);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, true);
            var boFlag = nFlag != 1;
            Envir.SetMapXyFlag(CurrX, CurrY, boFlag);
            Envir.SetMapXyFlag(CurrX, CurrY - 1, boFlag);
            Envir.SetMapXyFlag(CurrX, CurrY - 2, boFlag);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, boFlag);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, boFlag);
            Envir.SetMapXyFlag(CurrX - 1, CurrY, boFlag);
            Envir.SetMapXyFlag(CurrX - 2, CurrY, boFlag);
            Envir.SetMapXyFlag(CurrX - 1, CurrY - 1, boFlag);
            Envir.SetMapXyFlag(CurrX - 1, CurrY + 1, boFlag);
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
            HoldPlace = false;
        }

        public void Close()
        {
            if (Death)
            {
                return;
            }
            Direction = (byte)(3 - HUtil32.Round(WAbil.HP / WAbil.MaxHP * 3));
            if (Direction - 3 >= 0)
            {
                Direction = 0;
            }
            SendRefMsg(Grobal2.RM_DIGDOWN, Direction, CurrX, CurrY, 0, "");
            IsOpened = false;
            StoneMode = false;
            SetMapXyFlag(1);
            HoldPlace = true;
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
                int n08 = 3 - HUtil32.Round(WAbil.HP / WAbil.MaxHP * 3);
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
            var n08 = 3 - HUtil32.Round(WAbil.HP / WAbil.MaxHP * 3);
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

