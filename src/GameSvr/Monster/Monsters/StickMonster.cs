using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class StickMonster : AnimalObject
    {
        public int n54C = 0;
        protected int nComeOutValue = 0;
        protected int nAttackRange = 0;

        public StickMonster() : base()
        {
            this.ViewRange = 7;
            this.m_nRunTime = 250;
            this.SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            this.SearchTick = HUtil32.GetTickCount();
            nComeOutValue = 4;
            nAttackRange = 4;
            this.FixedHideMode = true;
            this.m_boStickMode = true;
            this.Animal = true;
        }

        protected virtual bool AttackTarget()
        {
            byte btDir = 0;
            if (this.TargetCret == null)
            {
                return false;
            }
            if (this.GetAttackDir(this.TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime)
                {
                    this.AttackTick = HUtil32.GetTickCount();
                    this.TargetFocusTick = HUtil32.GetTickCount();
                    this.Attack(this.TargetCret, btDir);
                }
                return true;
            }
            if (this.TargetCret.Envir == this.Envir)
            {
                this.SetTargetXY(this.TargetCret.CurrX, this.TargetCret.CurrY);
            }
            else
            {
                this.DelTargetCreat();
            }
            return false;
        }

        protected virtual void ComeOut()
        {
            this.FixedHideMode = false;
            this.SendRefMsg(Grobal2.RM_DIGUP, this.Direction, this.CurrX, this.CurrY, 0, "");
        }

        protected virtual void ComeDown()
        {
            this.SendRefMsg(Grobal2.RM_DIGDOWN, this.Direction, this.CurrX, this.CurrY, 0, "");
            for (var i = 0; i < this.VisibleActors.Count; i++)
            {
                Dispose(VisibleActors[i]);
            }
            this.VisibleActors.Clear();
            this.FixedHideMode = true;
        }

        protected virtual bool CheckComeOut()
        {
            TBaseObject BaseObject;
            var result = false;
            for (var i = 0; i < this.VisibleActors.Count; i++)
            {
                BaseObject = this.VisibleActors[i].BaseObject;
                if (BaseObject.Death)
                {
                    continue;
                }
                if (this.IsProperTarget(BaseObject))
                {
                    if (!BaseObject.HideMode || this.CoolEye)
                    {
                        if (Math.Abs(this.CurrX - BaseObject.CurrX) < nComeOutValue && Math.Abs(this.CurrY - BaseObject.CurrY) < nComeOutValue)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            bool bo05;
            if (!this.Ghost && !this.Death && this.m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - this.WalkTick) > this.WalkSpeed)
                {
                    this.WalkTick = HUtil32.GetTickCount();
                    if (this.FixedHideMode)
                    {
                        if (CheckComeOut())
                        {
                            ComeOut();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime)
                        {
                            this.SearchTarget();
                        }
                        bo05 = false;
                        if (this.TargetCret != null)
                        {
                            if (Math.Abs(this.TargetCret.CurrX - this.CurrX) > nAttackRange || Math.Abs(this.TargetCret.CurrY - this.CurrY) > nAttackRange)
                            {
                                bo05 = true;
                            }
                        }
                        else
                        {
                            bo05 = true;
                        }
                        if (bo05)
                        {
                            ComeDown();
                        }
                        else
                        {
                            if (AttackTarget())
                            {
                                base.Run();
                                return;
                            }
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

