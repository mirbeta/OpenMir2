using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class StickMonster : AnimalObject
    {
        protected int ComeOutValue;
        protected int AttackRange;

        public StickMonster() : base()
        {
            this.ViewRange = 7;
            this.RunTime = 250;
            this.SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            this.SearchTick = HUtil32.GetTickCount();
            ComeOutValue = 4;
            AttackRange = 4;
            this.FixedHideMode = true;
            this.StickMode = true;
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
                this.SetTargetXy(this.TargetCret.CurrX, this.TargetCret.CurrY);
            }
            else
            {
                this.DelTargetCreat();
            }
            return false;
        }

        protected virtual void FindAttackTarget()
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
            var result = false;
            for (var i = 0; i < this.VisibleActors.Count; i++)
            {
                BaseObject baseObject = this.VisibleActors[i].BaseObject;
                if (baseObject.Death)
                {
                    continue;
                }
                if (this.IsProperTarget(baseObject))
                {
                    if (!baseObject.HideMode || this.CoolEye)
                    {
                        if (Math.Abs(this.CurrX - baseObject.CurrX) < ComeOutValue && Math.Abs(this.CurrY - baseObject.CurrY) < ComeOutValue)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            return base.Operate(processMsg);
        }

        public override void Run()
        {
            if (CanMove())
            {
                if ((HUtil32.GetTickCount() - this.WalkTick) > this.WalkSpeed)
                {
                    this.WalkTick = HUtil32.GetTickCount();
                    if (this.FixedHideMode)
                    {
                        if (CheckComeOut())
                        {
                            FindAttackTarget();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - this.AttackTick) > this.NextHitTime)
                        {
                            this.SearchTarget();
                        }
                        var bo05 = false;
                        if (this.TargetCret != null)
                        {
                            if (Math.Abs(this.TargetCret.CurrX - this.CurrX) > AttackRange || Math.Abs(this.TargetCret.CurrY - this.CurrY) > AttackRange)
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

