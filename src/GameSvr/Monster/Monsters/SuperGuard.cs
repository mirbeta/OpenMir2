using GameSvr.Actor;
using GameSvr.Npc;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class SuperGuard : NormNpc
    {
        public int n564 = 0;
        protected bool m_boAttackPet = false;

        private bool AttackTarget()
        {
            var result = false;
            short nOldX;
            short nOldY;
            byte btOldDir;
            short wHitMode;
            if (this.TargetCret.m_PEnvir == this.m_PEnvir)
            {
                if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime)
                {
                    this.AttackTick = HUtil32.GetTickCount();
                    this.TargetFocusTick = HUtil32.GetTickCount();
                    nOldX = this.CurrX;
                    nOldY = this.CurrY;
                    btOldDir = this.Direction;
                    this.TargetCret.GetBackPosition(ref this.CurrX, ref this.CurrY);
                    this.Direction = M2Share.GetNextDirection(this.CurrX, this.CurrY, this.TargetCret.CurrX, this.TargetCret.CurrY);
                    this.SendRefMsg(Grobal2.RM_HIT, this.Direction, this.CurrX, this.CurrY, 0, "");
                    wHitMode = 0;
                    this._Attack(ref wHitMode, this.TargetCret);
                    this.TargetCret.SetLastHiter(this);
                    this.TargetCret.m_ExpHitter = null;
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
            this.m_nLight = 4;
            m_boAttackPet = true;
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (this.m_Master != null)
            {
                this.m_Master = null;
            }
            // 不允许召唤为宝宝
            if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime)
            {
                for (var i = 0; i < this.VisibleActors.Count; i++)
                {
                    BaseObject = this.VisibleActors[i].BaseObject;
                    if (BaseObject.Death)
                    {
                        continue;
                    }
                    if (BaseObject.PKLevel() >= 2 || BaseObject.m_btRaceServer >= Grobal2.RC_MONSTER && !BaseObject.m_boMission)
                    {
                        if (m_boAttackPet)
                        {
                            this.SetTargetCreat(BaseObject);
                            break;
                        }
                        else
                        {
                            if (BaseObject.m_Master == null)
                            {
                                this.SetTargetCreat(BaseObject);
                                break;
                            }
                            else
                            {
                                if (BaseObject.TargetCret == this)
                                {
                                    this.SetTargetCreat(BaseObject);
                                    break;
                                }
                            }
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

