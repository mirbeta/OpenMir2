using GameSrv.Actor;
using SystemModule.Enums;

namespace GameSrv.Monster.Monsters {
    /// <summary>
    /// 神兽攻击形态
    /// </summary>
    public class ElfWarriorMonster : SpitSpider {
        public bool BoIsFirst;
        private int DigDownTick;

        public void AppearNow() {
            BoIsFirst = false;
            FixedHideMode = false;
            SendRefMsg(Messages.RM_DIGUP, Dir, CurrX, CurrY, 0, "");
            RecalcAbilitys();
            WalkTick = WalkTick + 800;
            DigDownTick = HUtil32.GetTickCount();
            Race = ActorRace.ElfWarriormon;
        }

        public ElfWarriorMonster()
            : base() {
            ViewRange = 6;
            FixedHideMode = true;
            BoIsFirst = true;
            UsePoison = false;
        }

        public override void RecalcAbilitys() {
            base.RecalcAbilitys();
            ResetElfMon();
        }

        private void ResetElfMon() {
            NextHitTime = 1500 - SlaveMakeLevel * 100;
            WalkSpeed = 500 - SlaveMakeLevel * 50;
            WalkTick = HUtil32.GetTickCount() + 2000;
        }

        public override void Run() {
            if (BoIsFirst) {
                BoIsFirst = false;
                FixedHideMode = false;
                SendRefMsg(Messages.RM_DIGUP, Dir, CurrX, CurrY, 0, "");
                ResetElfMon();
            }
            if (Death) {
                if ((HUtil32.GetTickCount() - DeathTick) > (2 * 1000)) {
                    MakeGhost();
                }
            }
            else {
                bool boChangeFace = TargetCret == null;
                if (Master != null && (Master.TargetCret != null || Master.LastHiter != null)) {
                    boChangeFace = false;
                }
                if (boChangeFace) {
                    if ((HUtil32.GetTickCount() - DigDownTick) > (6 * 10 * 1000)) {
                        BaseObject elfMon = null;
                        string elfName = ChrName;
                        if (elfName[^1] == '1') {
                            elfName = elfName[..^1];
                            elfMon = MakeClone(elfName, this);
                        }
                        if (elfMon != null) {
                            SendRefMsg(Messages.RM_DIGDOWN, Dir, CurrX, CurrY, 0, "");
                            SendRefMsg(Messages.RM_CHANGEFACE, 0, ActorId, elfMon.ActorId, 0, "");
                            elfMon.AutoChangeColor = AutoChangeColor;
                            if (elfMon is ElfMonster monster) {
                                monster.AppearNow();
                            }
                            Master = null;
                            KickException();
                        }
                    }
                }
                else {
                    DigDownTick = HUtil32.GetTickCount();
                }
            }
            base.Run();
        }
    }
}