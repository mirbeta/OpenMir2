using GameSrv.Actor;
using SystemModule.Enums;

namespace GameSrv.Monster.Monsters {
    /// <summary>
    /// 弓箭守卫
    /// </summary>
    public class ArcherGuard : GuardUnit {
        public ArcherGuard() : base() {
            ViewRange = 12;
            WantRefMsg = true;
            Castle = null;
            GuardDirection = -1;
            Race = ActorRace.ArcherGuard;
        }

        private void AttackTarger(BaseObject targetBaseObject) {
            Direction = M2Share.GetNextDirection(CurrX, CurrY, targetBaseObject.CurrX, targetBaseObject.CurrY);
            int nDamage = HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1);
            if (nDamage > 0) {
                nDamage = targetBaseObject.GetHitStruckDamage(this, nDamage);
            }
            if (nDamage > 0) {
                targetBaseObject.SetLastHiter(this);
                targetBaseObject.ExpHitter = null;
                targetBaseObject.StruckDamage((ushort)nDamage);
                targetBaseObject.SendStruckDelayMsg(Messages.RM_STRUCK, Messages.RM_REFMESSAGE, nDamage, targetBaseObject.WAbil.HP, targetBaseObject.WAbil.MaxHP, ActorId, "", HUtil32._MAX(Math.Abs(CurrX - targetBaseObject.CurrX), Math.Abs(CurrY - targetBaseObject.CurrY)) * 50 + 600);
            }
            SendRefMsg(Messages.RM_FLYAXE, Direction, CurrX, CurrY, targetBaseObject.ActorId, "");
        }

        public override void Run() {
            int nRage = 9999;
            BaseObject targetBaseObject = null;
            if (CanMove()) {
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed) {
                    WalkTick = HUtil32.GetTickCount();
                    for (int i = 0; i < VisibleActors.Count; i++) {
                        BaseObject baseObject = VisibleActors[i].BaseObject;
                        if (baseObject.Death) {
                            continue;
                        }
                        if (IsProperTarget(baseObject)) {
                            int nAbs = Math.Abs(CurrX - baseObject.CurrX) + Math.Abs(CurrY - baseObject.CurrY);
                            if (nAbs < nRage) {
                                nRage = nAbs;
                                targetBaseObject = baseObject;
                            }
                        }
                    }
                    if (targetBaseObject != null) {
                        SetTargetCreat(targetBaseObject);
                    }
                    else {
                        DelTargetCreat();
                    }
                }
                if (TargetCret != null) {
                    if ((HUtil32.GetTickCount() - AttackTick) >= NextHitTime) {
                        AttackTick = HUtil32.GetTickCount();
                        AttackTarger(TargetCret);
                    }
                }
                else {
                    if (GuardDirection > 0 && Direction != GuardDirection) {
                        TurnTo((byte)GuardDirection);
                    }
                }
            }
            base.Run();
        }
    }
}

