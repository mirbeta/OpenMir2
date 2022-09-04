using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster.Monsters
{
    public class MagicMonster : AnimalObject
    {
        public int m_dwThinkTick = 0;
        public int m_dwSpellTick = 0;
        public bool bo554 = false;
        public bool m_boDupMode = false;

        public MagicMonster() : base()
        {
            m_boDupMode = false;
            bo554 = false;
            m_dwThinkTick = HUtil32.GetTickCount();
            ViewRange = 8;
            m_nRunTime = 250;
            SearchTime = 3000 + M2Share.RandomNumber.Random(2000);
            SearchTick = HUtil32.GetTickCount();
            Race = 215;
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        private bool Think()
        {
            var result = false;
            if ((HUtil32.GetTickCount() - m_dwThinkTick) > (3 * 1000))
            {
                m_dwThinkTick = HUtil32.GetTickCount();
                if (Envir.GetXyObjCount(CurrX, CurrY) >= 2)
                {
                    m_boDupMode = true;
                }
                if (!IsProperTarget(TargetCret))
                {
                    TargetCret = null;
                }
            }
            if (m_boDupMode)
            {
                int nOldX = CurrX;
                int nOldY = CurrY;
                WalkTo(M2Share.RandomNumber.RandomByte(8), false);
                if (nOldX != CurrX || nOldY != CurrY)
                {
                    m_boDupMode = false;
                    result = true;
                }
            }
            return result;
        }

        protected virtual bool AttackTarget()
        {
            var result = false;
            byte bt06 = 0;
            if (TargetCret != null)
            {
                if (TargetCret == Master)
                {
                    TargetCret = null;
                }
                else
                {
                    if (GetAttackDir(TargetCret, ref bt06))
                    {
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                        {
                            AttackTick = HUtil32.GetTickCount();
                            TargetFocusTick = HUtil32.GetTickCount();
                        }
                        result = true;
                    }
                    else
                    {
                        if (TargetCret.Envir == Envir)
                        {
                            SetTargetXY(TargetCret.CurrX, TargetCret.CurrY);
                        }
                        else
                        {
                            DelTargetCreat();
                        }
                    }
                }
            }
            return result;
        }

        public override void Run()
        {
            short nX = 0;
            short nY = 0;
            if (!Ghost && !Death && !FixedHideMode && !StoneMode && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (Think())
                {
                    base.Run();
                    return;
                }
                if (WalkWaitLocked)
                {
                    if ((HUtil32.GetTickCount() - WalkWaitTick) > WalkWait)
                    {
                        WalkWaitLocked = false;
                    }
                }
                if (!WalkWaitLocked && (HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    WalkCount++;
                    if (WalkCount > WalkStep)
                    {
                        WalkCount = 0;
                        WalkWaitLocked = true;
                        WalkWaitTick = HUtil32.GetTickCount();
                    }
                    if (!m_boRunAwayMode)
                    {
                        if (!m_boNoAttackMode)
                        {
                            if (TargetCret != null)
                            {
                                if (AttackTarget())
                                {
                                    base.Run();
                                    return;
                                }
                            }
                            else
                            {
                                TargetX = -1;
                                if (m_boMission)
                                {
                                    TargetX = m_nMissionX;
                                    TargetY = m_nMissionY;
                                }
                            }
                        }
                        if (Master != null)
                        {
                            if (TargetCret == null)
                            {
                                Master.GetBackPosition(ref nX, ref nY);
                                if (Math.Abs(TargetX - nX) > 1 || Math.Abs(TargetY - nY) > 1)
                                {
                                    TargetX = nX;
                                    TargetY = nY;
                                    if (Math.Abs(CurrX - nX) <= 2 && Math.Abs(CurrY - nY) <= 2)
                                    {
                                        if (Envir.GetMovingObject(nX, nY, true) != null)
                                        {
                                            TargetX = CurrX;
                                            TargetY = CurrY;
                                        }
                                    }
                                }
                            }
                            if (!Master.SlaveRelax && (Envir != Master.Envir || Math.Abs(CurrX - Master.CurrX) > 20 || Math.Abs(CurrY - Master.CurrY) > 20))
                            {
                                SpaceMove(Master.Envir.MapName, TargetX, TargetY, 1);
                            }
                        }
                    }
                    else
                    {
                        if (m_dwRunAwayTime > 0 && (HUtil32.GetTickCount() - m_dwRunAwayStart) > m_dwRunAwayTime)
                        {
                            m_boRunAwayMode = false;
                            m_dwRunAwayTime = 0;
                        }
                    }
                    if (Master != null && Master.SlaveRelax)
                    {
                        base.Run();
                        return;
                    }
                    if (TargetX != -1)
                    {
                        GotoTargetXY();
                    }
                    else
                    {
                        if (TargetCret == null)
                        {
                            Wondering();
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

