using SystemModule;

namespace GameSvr
{
    public class StickMonster : AnimalObject
    {
        public int n54C = 0;
        protected int nComeOutValue = 0;
        protected int nAttackRange = 0;

        public StickMonster() : base()
        {
            this.m_nViewRange = 7;
            this.m_nRunTime = 250;
            this.m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            this.m_dwSearchTick = HUtil32.GetTickCount();
            nComeOutValue = 4;
            nAttackRange = 4;
            this.m_boFixedHideMode = true;
            this.m_boStickMode = true;
            this.m_boAnimal = true;
        }

        protected virtual bool AttackTarget()
        {
            byte btDir = 0;
            if (this.m_TargetCret == null)
            {
                return false;
            }
            if (this.GetAttackDir(this.m_TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - this.m_dwHitTick) > this.m_nNextHitTime)
                {
                    this.m_dwHitTick = HUtil32.GetTickCount();
                    this.m_dwTargetFocusTick = HUtil32.GetTickCount();
                    this.Attack(this.m_TargetCret, btDir);
                }
                return true;
            }
            if (this.m_TargetCret.m_PEnvir == this.m_PEnvir)
            {
                this.SetTargetXY(this.m_TargetCret.m_nCurrX, this.m_TargetCret.m_nCurrY);
            }
            else
            {
                this.DelTargetCreat();
            }
            return false;
        }

        protected virtual void ComeOut()
        {
            this.m_boFixedHideMode = false;
            this.SendRefMsg(Grobal2.RM_DIGUP, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
        }

        protected virtual void ComeDown()
        {
            this.SendRefMsg(Grobal2.RM_DIGDOWN, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
            for (var i = 0; i < this.m_VisibleActors.Count; i++)
            {
                Dispose(m_VisibleActors[i]);
            }
            this.m_VisibleActors.Clear();
            this.m_boFixedHideMode = true;
        }

        protected virtual bool CheckComeOut()
        {
            TBaseObject BaseObject;
            var result = false;
            for (var i = 0; i < this.m_VisibleActors.Count; i++)
            {
                BaseObject = this.m_VisibleActors[i].BaseObject;
                if (BaseObject.m_boDeath)
                {
                    continue;
                }
                if (this.IsProperTarget(BaseObject))
                {
                    if (!BaseObject.m_boHideMode || this.m_boCoolEye)
                    {
                        if (Math.Abs(this.m_nCurrX - BaseObject.m_nCurrX) < nComeOutValue && Math.Abs(this.m_nCurrY - BaseObject.m_nCurrY) < nComeOutValue)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            bool bo05;
            if (!this.m_boGhost && !this.m_boDeath && this.m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - this.m_dwWalkTick) > this.m_nWalkSpeed)
                {
                    this.m_dwWalkTick = HUtil32.GetTickCount();
                    if (this.m_boFixedHideMode)
                    {
                        if (CheckComeOut())
                        {
                            ComeOut();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - this.m_dwHitTick) > this.m_nNextHitTime)
                        {
                            this.SearchTarget();
                        }
                        bo05 = false;
                        if (this.m_TargetCret != null)
                        {
                            if (Math.Abs(this.m_TargetCret.m_nCurrX - this.m_nCurrX) > nAttackRange || Math.Abs(this.m_TargetCret.m_nCurrY - this.m_nCurrY) > nAttackRange)
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

