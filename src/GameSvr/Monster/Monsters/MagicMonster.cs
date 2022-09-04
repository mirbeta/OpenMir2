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
            m_dwSearchTime = 3000 + M2Share.RandomNumber.Random(2000);
            m_dwSearchTick = HUtil32.GetTickCount();
            m_btRaceServer = 215;
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
                if (m_PEnvir.GetXyObjCount(CurrX, CurrY) >= 2)
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
                if (TargetCret == m_Master)
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
                        if (TargetCret.m_PEnvir == m_PEnvir)
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
                                m_nTargetX = -1;
                                if (m_boMission)
                                {
                                    m_nTargetX = m_nMissionX;
                                    m_nTargetY = m_nMissionY;
                                }
                            }
                        }
                        if (m_Master != null)
                        {
                            if (TargetCret == null)
                            {
                                m_Master.GetBackPosition(ref nX, ref nY);
                                if (Math.Abs(m_nTargetX - nX) > 1 || Math.Abs(m_nTargetY - nY) > 1)
                                {
                                    m_nTargetX = nX;
                                    m_nTargetY = nY;
                                    if (Math.Abs(CurrX - nX) <= 2 && Math.Abs(CurrY - nY) <= 2)
                                    {
                                        if (m_PEnvir.GetMovingObject(nX, nY, true) != null)
                                        {
                                            m_nTargetX = CurrX;
                                            m_nTargetY = CurrY;
                                        }
                                    }
                                }
                            }
                            if (!m_Master.SlaveRelax && (m_PEnvir != m_Master.m_PEnvir || Math.Abs(CurrX - m_Master.CurrX) > 20 || Math.Abs(CurrY - m_Master.CurrY) > 20))
                            {
                                SpaceMove(m_Master.m_PEnvir.MapName, m_nTargetX, m_nTargetY, 1);
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
                    if (m_Master != null && m_Master.SlaveRelax)
                    {
                        base.Run();
                        return;
                    }
                    if (m_nTargetX != -1)
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

