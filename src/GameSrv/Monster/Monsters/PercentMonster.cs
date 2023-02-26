using GameSrv.Actor;
using SystemModule.Data;

namespace GameSrv.Monster.Monsters {
    public class PercentMonster : AnimalObject {
        public int MDwThinkTick;
        public bool MBoDupMode;

        public PercentMonster() : base() {
            MBoDupMode = false;
            MDwThinkTick = HUtil32.GetTickCount();
            ViewRange = 5;
            RunTime = 250;
            SearchTime = 3000 + M2Share.RandomNumber.Random(2000);
            SearchTick = HUtil32.GetTickCount();
            Race = 80;
        }

        protected override bool Operate(ProcessMessage processMsg) {
            return base.Operate(processMsg);
        }

        private bool Think() {
            if ((HUtil32.GetTickCount() - MDwThinkTick) > 3 * 1000) {
                MDwThinkTick = HUtil32.GetTickCount();
                if (Envir.GetXyObjCount(CurrX, CurrY) >= 2) {
                    MBoDupMode = true;
                }
                if (!IsProperTarget(TargetCret)) {
                    TargetCret = null;
                }
            }
            if (MBoDupMode) {
                int nOldX = CurrX;
                int nOldY = CurrY;
                WalkTo(M2Share.RandomNumber.RandomByte(8), false);
                if (nOldX == CurrX && nOldY == CurrY) {
                    return false;
                }
                MBoDupMode = false;
                return true;
            }
            return false;
        }

        protected virtual bool AttackTarget() {
            bool result = false;
            byte btDir = 0;
            if (TargetCret != null) {
                if (GetAttackDir(TargetCret, ref btDir)) {
                    if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime) {
                        AttackTick = HUtil32.GetTickCount();
                        TargetFocusTick = HUtil32.GetTickCount();
                        Attack(TargetCret, btDir);
                        BreakHolySeizeMode();
                    }
                    result = true;
                }
                else {
                    if (TargetCret.Envir == Envir) {
                        SetTargetXy(TargetCret.CurrX, TargetCret.CurrY);
                    }
                    else {
                        DelTargetCreat();
                    }
                }
            }
            return result;
        }

        public override void Run() {
            short nX = 0;
            short nY = 0;
            if (CanMove() && !FixedHideMode && !StoneMode) {
                if (Think()) {
                    base.Run();
                    return;
                }
                if (WalkWaitLocked) {
                    if ((HUtil32.GetTickCount() - WalkWaitTick) > WalkWait) {
                        WalkWaitLocked = false;
                    }
                }
                if (!WalkWaitLocked && (HUtil32.GetTickCount() - WalkTick) > WalkSpeed) {
                    WalkTick = HUtil32.GetTickCount();
                    WalkCount++;
                    if (WalkCount > WalkStep) {
                        WalkCount = 0;
                        WalkWaitLocked = true;
                        WalkWaitTick = HUtil32.GetTickCount();
                    }
                    if (!RunAwayMode) {
                        if (!NoAttackMode) {
                            if (TargetCret != null) {
                                if (AttackTarget()) {
                                    base.Run();
                                    return;
                                }
                            }
                            else {
                                TargetX = -1;
                                if (Mission) {
                                    TargetX = MissionX;
                                    TargetY = MissionY;
                                }
                            }
                        }
                        if (Master != null) {
                            if (TargetCret == null) {
                                Master.GetBackPosition(ref nX, ref nY);
                                if (Math.Abs(TargetX - nX) > 1 || Math.Abs(TargetY - nY) > 1) {
                                    TargetX = nX;
                                    TargetY = nY;
                                    if (Math.Abs(CurrX - nX) <= 2 && Math.Abs(CurrY - nY) <= 2) {
                                        if (Envir.GetMovingObject(nX, nY, true) != null) {
                                            TargetX = CurrX;
                                            TargetY = CurrY;
                                        }
                                    }
                                }
                            }
                            if (!Master.SlaveRelax &&
                                (Envir != Master.Envir || Math.Abs(CurrX - Master.CurrX) > 20 ||
                                 Math.Abs(CurrY - Master.CurrY) > 20)) {
                                SpaceMove(Master.Envir.MapName, TargetX, TargetY, 1);
                            }
                        }
                    }
                    else {
                        if (RunAwayTime > 0 && (HUtil32.GetTickCount() - RunAwayStart) > RunAwayTime) {
                            RunAwayMode = false;
                            RunAwayTime = 0;
                        }
                    }
                    if (Master != null && Master.SlaveRelax) {
                        base.Run();
                        return;
                    }
                    if (TargetX != -1) {
                        GotoTargetXy();
                    }
                    else {
                        if (TargetCret == null) {
                            Wondering();
                        }
                    }
                }
            }
            base.Run();
        }
    }
}