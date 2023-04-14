using GameSrv.Maps;

namespace GameSrv.Monster.Monsters {
    /// <summary>
    /// 沙巴克城门
    /// </summary>
    public class CastleDoor : GuardUnit {
        public bool IsOpened;
        public bool HoldPlace;

        public CastleDoor() : base() {
            Animal = false;
            StickMode = true;
            IsOpened = false;
            AntiPoison = 200;
            CellType = CellType.Door;
        }

        private void SetMapXyFlag(int nFlag) {
            Envir.SetMapXyFlag(CurrX, CurrY - 2, true);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, true);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, true);
            bool boFlag = nFlag != 1;
            Envir.SetMapXyFlag(CurrX, CurrY, boFlag);
            Envir.SetMapXyFlag(CurrX, CurrY - 1, boFlag);
            Envir.SetMapXyFlag(CurrX, CurrY - 2, boFlag);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, boFlag);
            Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, boFlag);
            Envir.SetMapXyFlag(CurrX - 1, CurrY, boFlag);
            Envir.SetMapXyFlag(CurrX - 2, CurrY, boFlag);
            Envir.SetMapXyFlag(CurrX - 1, CurrY - 1, boFlag);
            Envir.SetMapXyFlag(CurrX - 1, CurrY + 1, boFlag);
            if (nFlag == 0) {
                Envir.SetMapXyFlag(CurrX, CurrY - 2, false);
                Envir.SetMapXyFlag(CurrX + 1, CurrY - 1, false);
                Envir.SetMapXyFlag(CurrX + 1, CurrY - 2, false);
            }
        }

        public void Open() {
            if (Death) {
                return;
            }
            Dir = 7;
            SendRefMsg(Messages.RM_DIGUP, Dir, CurrX, CurrY, 0, "");
            IsOpened = true;
            StoneMode = true;
            SetMapXyFlag(0);
            HoldPlace = false;
        }

        public void Close() {
            if (Death) {
                return;
            }
            Dir = (byte)(3 - HUtil32.Round(WAbil.HP / (double)WAbil.MaxHP * 3.0));
            if (Dir - 3 >= 0) {
                Dir = 0;
            }
            SendRefMsg(Messages.RM_DIGDOWN, Dir, CurrX, CurrY, 0, "");
            IsOpened = false;
            StoneMode = false;
            SetMapXyFlag(1);
            HoldPlace = true;
        }

        public override void Die() {
            base.Die();
            SetMapXyFlag(2);
        }

        public override void Run() {
            if (Death && Castle != null) {
                DeathTick = HUtil32.GetTickCount();
            }
            else {
                HealthTick = 0;
            }
            if (!IsOpened) {
                int n08 = 3 - HUtil32.Round(WAbil.HP / (double)WAbil.MaxHP * 3.0);
                if (Dir != n08 && n08 < 3) {
                    Dir = (byte)n08;
                    SendRefMsg(Messages.RM_TURN, Dir, CurrX, CurrY, 0, "");
                }
            }
            base.Run();
        }

        public void RefStatus() {
            int n08 = 3 - HUtil32.Round(WAbil.HP / (double)WAbil.MaxHP * 3.0);
            if (n08 - 3 >= 0) {
                n08 = 0;
            }
            Dir = (byte)n08;
            SendRefMsg(Messages.RM_ALIVE, Dir, CurrX, CurrY, 0, "");
        }
    }
}

