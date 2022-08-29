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
            if (this.m_TargetCret.m_PEnvir == this.m_PEnvir)
            {
                if ((HUtil32.GetTickCount() - this.m_dwHitTick) > this.m_nNextHitTime)
                {
                    this.m_dwHitTick = HUtil32.GetTickCount();
                    this.m_dwTargetFocusTick = HUtil32.GetTickCount();
                    nOldX = this.m_nCurrX;
                    nOldY = this.m_nCurrY;
                    btOldDir = this.Direction;
                    this.m_TargetCret.GetBackPosition(ref this.m_nCurrX, ref this.m_nCurrY);
                    this.Direction = M2Share.GetNextDirection(this.m_nCurrX, this.m_nCurrY, this.m_TargetCret.m_nCurrX, this.m_TargetCret.m_nCurrY);
                    this.SendRefMsg(Grobal2.RM_HIT, this.Direction, this.m_nCurrX, this.m_nCurrY, 0, "");
                    wHitMode = 0;
                    this._Attack(ref wHitMode, this.m_TargetCret);
                    this.m_TargetCret.SetLastHiter(this);
                    this.m_TargetCret.m_ExpHitter = null;
                    this.m_nCurrX = nOldX;
                    this.m_nCurrY = nOldY;
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
            this.m_nViewRange = 7;
            this.m_nLight = 4;
            m_boAttackPet = true;
        }

        public override bool Operate(TProcessMessage ProcessMsg)
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
            if ((HUtil32.GetTickCount() - this.m_dwHitTick) > this.m_nNextHitTime)
            {
                for (var i = 0; i < this.m_VisibleActors.Count; i++)
                {
                    BaseObject = this.m_VisibleActors[i].BaseObject;
                    if (BaseObject.m_boDeath)
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
                                if (BaseObject.m_TargetCret == this)
                                {
                                    this.SetTargetCreat(BaseObject);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (this.m_TargetCret != null)
            {
                AttackTarget();
            }
            base.Run();
        }
    }
}

