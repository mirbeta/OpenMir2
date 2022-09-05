using GameSvr.Actor;
using GameSvr.Npc;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class SuperGuard : NormNpc
    {
        protected bool AttackPet;

        private bool AttackTarget()
        {
            var result = false;
            if (this.TargetCret.Envir == this.Envir)
            {
                if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime)
                {
                    this.AttackTick = HUtil32.GetTickCount();
                    this.TargetFocusTick = HUtil32.GetTickCount();
                    var nOldX = this.CurrX;
                    var nOldY = this.CurrY;
                    var btOldDir = this.Direction;
                    this.TargetCret.GetBackPosition(ref this.CurrX, ref this.CurrY);
                    this.Direction = M2Share.GetNextDirection(this.CurrX, this.CurrY, this.TargetCret.CurrX, this.TargetCret.CurrY);
                    this.SendRefMsg(Grobal2.RM_HIT, this.Direction, this.CurrX, this.CurrY, 0, "");
                    short wHitMode = 0;
                    this._Attack(ref wHitMode, this.TargetCret);
                    this.TargetCret.SetLastHiter(this);
                    this.TargetCret.ExpHitter = null;
                    this.CurrX = nOldX;
                    this.CurrY = nOldY;
                    this.Direction = btOldDir;
                    this.TurnTo(this.Direction);
                    this.BreakHolySeizeMode();
                }
                result = true;
            }
            else
            {
                this.DelTargetCreat();
            }
            return result;
        }

        public SuperGuard() : base()
        {
            this.ViewRange = 7;
            this.Light = 4;
            AttackPet = true;
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            if (this.Master != null)
            {
                this.Master = null;
            }
            // 不允许召唤为宝宝
            if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime)
            {
                for (var i = 0; i < this.VisibleActors.Count; i++)
                {
                    var BaseObject = this.VisibleActors[i].BaseObject;
                    if (BaseObject.Death)
                    {
                        continue;
                    }
                    if (BaseObject.PvpLevel() >= 2 || BaseObject.Race >= Grobal2.RC_MONSTER && !BaseObject.m_boMission)
                    {
                        if (AttackPet)
                        {
                            this.SetTargetCreat(BaseObject);
                            break;
                        }
                        if (BaseObject.Master == null)
                        {
                            this.SetTargetCreat(BaseObject);
                            break;
                        }
                        if (BaseObject.TargetCret == this)
                        {
                            this.SetTargetCreat(BaseObject);
                            break;
                        }
                    }
                }
            }
            if (this.TargetCret != null)
            {
                AttackTarget();
            }
            base.Run();
        }
    }
}

