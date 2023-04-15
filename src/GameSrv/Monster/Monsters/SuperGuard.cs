using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.Monster.Monsters {
    /// <summary>
    /// 守卫
    /// </summary>
    public class SuperGuard : GuardUnit {
        protected bool AttackPet;

        private bool AttackTarget() {
            bool result = false;
            if (this.TargetCret.Envir == this.Envir) {
                if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime) {
                    this.AttackTick = HUtil32.GetTickCount();
                    this.TargetFocusTick = HUtil32.GetTickCount();
                    short nOldX = this.CurrX;
                    short nOldY = this.CurrY;
                    byte btOldDir = this.Dir;
                    this.TargetCret.GetBackPosition(ref this.CurrX, ref this.CurrY);
                    this.Dir = M2Share.GetNextDirection(this.CurrX, this.CurrY, this.TargetCret.CurrX, this.TargetCret.CurrY);
                    this.SendRefMsg(Messages.RM_HIT, this.Dir, this.CurrX, this.CurrY, 0, "");
                    this._Attack(GetBaseAttackPoewr(), this.TargetCret);
                    this.TargetCret.SetLastHiter(this);
                    this.TargetCret.ExpHitter = null;
                    this.CurrX = nOldX;
                    this.CurrY = nOldY;
                    this.Dir = btOldDir;
                    this.TurnTo(this.Dir);
                    //this.BreakHolySeizeMode(); //守卫能被困住？
                }
                result = true;
            }
            else {
                this.DelTargetCreat();
            }
            return result;
        }

        public SuperGuard() {
            this.ViewRange = 7;
            this.Light = 4;
            AttackPet = true;
        }

        protected override bool Operate(ProcessMessage processMsg) {
            return base.Operate(processMsg);
        }

        private static bool CanAttckTarget(int monsterType) {
            return monsterType is ActorRace.Guard or ActorRace.ArcherGuard or ActorRace.PeaceNpc or ActorRace.NPC;
        }

        public override void Run() {
            if (Master != null)// 不允许召唤为宝宝
            {
                Master = null;
            }
            if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime) {
                for (int i = 0; i < this.VisibleActors.Count; i++) {
                    BaseObject attackObject = this.VisibleActors[i].BaseObject;
                    if (attackObject == null) {
                        continue;
                    }
                    if (attackObject.Death || attackObject.Ghost || CanAttckTarget(attackObject.Race)) {
                        VisibleActors.RemoveAt(i);
                        continue;
                    }
                    if (attackObject.Race == ActorRace.Play && !attackObject.Mission) {
                        if (((PlayObject)attackObject).PvpLevel() >= 2) {
                            SetAttackTarget(attackObject);
                            break;
                        }
                    }
                    else if (attackObject.Race >= ActorRace.Monster && !attackObject.Mission) {
                        SetAttackTarget(attackObject);
                        break;
                    }
                }
            }
            if (this.TargetCret != null) {
                if (TargetCret.Death || TargetCret.Ghost) {
                    DelTargetCreat();
                }
                else {
                    AttackTarget();
                }
            }
            base.Run();
        }

        private void SetAttackTarget(BaseObject attackObject) {
            if (AttackPet) {
                this.SetTargetCreat(attackObject);
                return;
            }
            if (attackObject.Master == null) {
                this.SetTargetCreat(attackObject);
                return;
            }
            if (attackObject.TargetCret == this) {
                this.SetTargetCreat(attackObject);
                return;
            }
        }
    }
}

