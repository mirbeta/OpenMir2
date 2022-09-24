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

        protected override bool Operate(ProcessMessage processMsg)
        {
            return base.Operate(processMsg);
        }

        private bool CanAttckTarget(BaseObject baseObject)
        {
            //todo 最好加个字段直接判断是否能被攻击，减少判断
            return baseObject.Race == Grobal2.RC_ARCHERGUARD || baseObject.Race == Grobal2.RC_GUARD || baseObject.Race == Grobal2.RC_PEACENPC || baseObject.Race == Grobal2.RC_NPC;
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
                    var baseObject = this.VisibleActors[i].BaseObject;
                    if (baseObject.Death || CanAttckTarget(baseObject))
                    {
                        continue;
                    }
                    if (baseObject.PvpLevel() >= 2 || baseObject.Race >= Grobal2.RC_MONSTER && !baseObject.Mission)
                    {
                        if (AttackPet)
                        {
                            this.SetTargetCreat(baseObject);
                            break;
                        }
                        if (baseObject.Master == null)
                        {
                            this.SetTargetCreat(baseObject);
                            break;
                        }
                        if (baseObject.TargetCret == this)
                        {
                            this.SetTargetCreat(baseObject);
                            break;
                        }
                    }
                }
            }
            if (this.TargetCret != null)
            {
                if (TargetCret.Death || TargetCret.Ghost)
                {
                    DelTargetCreat();
                }
                else
                {
                    AttackTarget();
                }
            }
            base.Run();
        }
    }
}

