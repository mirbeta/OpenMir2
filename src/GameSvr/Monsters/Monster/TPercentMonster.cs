using System;
using SystemModule;

namespace GameSvr
{
    public class TPercentMonster : TAnimalObject
    {
        public int n54C = 0;
        public int m_dwThinkTick = 0;
        public bool bo554 = false;
        public bool m_boDupMode = false;

        public TPercentMonster() : base()
        {
            m_boDupMode = false;
            bo554 = false;
            m_dwThinkTick = HUtil32.GetTickCount();
            m_nViewRange = 5;
            m_nRunTime = 250;
            m_dwSearchTime = 3000 + M2Share.RandomNumber.Random(2000);
            m_dwSearchTick = HUtil32.GetTickCount();
            m_btRaceServer = 80;
        }

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        private bool Think()
        {
            var result = false;
            if ((HUtil32.GetTickCount() - m_dwThinkTick) > 3 * 1000)
            {
                m_dwThinkTick = HUtil32.GetTickCount();
                if (m_PEnvir.GetXYObjCount(m_nCurrX, m_nCurrY) >= 2)
                {
                    m_boDupMode = true;
                }
                if (!IsProperTarget(m_TargetCret))
                {
                    m_TargetCret = null;
                }
            }
            if (m_boDupMode)
            {
                int nOldX = m_nCurrX;
                int nOldY = m_nCurrY;
                WalkTo((byte)M2Share.RandomNumber.Random(8), false);
                if (nOldX == m_nCurrX && nOldY == m_nCurrY)
                {
                    return result;
                }
                m_boDupMode = false;
                result = true;
            }
            return result;
        }

        protected virtual bool AttackTarget()
        {
            var result = false;
            byte btDir = 0;
            if (m_TargetCret != null)
            {
                if (GetAttackDir(m_TargetCret, ref btDir))
                {
                    if ((HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
                    {
                        m_dwHitTick = HUtil32.GetTickCount();
                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                        Attack(m_TargetCret, btDir);
                        BreakHolySeizeMode();
                    }
                    result = true;
                }
                else
                {
                    if (m_TargetCret.m_PEnvir == m_PEnvir)
                    {
                        SetTargetXY(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                    }
                    else
                    {
                        DelTargetCreat();
                    }
                }
            }
            return result;
        }

        public override void Run()
        {
            short nX = 0;
            short nY = 0;
            if (!m_boGhost && !m_boDeath && !m_boFixedHideMode && !m_boStoneMode &&
                m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (Think())
                {
                    base.Run();
                    return;
                }
                if (m_boWalkWaitLocked)
                {
                    if ((HUtil32.GetTickCount() - m_dwWalkWaitTick) > m_dwWalkWait)
                    {
                        m_boWalkWaitLocked = false;
                    }
                }
                if (!m_boWalkWaitLocked && (HUtil32.GetTickCount() - m_dwWalkTick) > m_nWalkSpeed)
                {
                    m_dwWalkTick = HUtil32.GetTickCount();
                    m_nWalkCount++;
                    if (m_nWalkCount > m_nWalkStep)
                    {
                        m_nWalkCount = 0;
                        m_boWalkWaitLocked = true;
                        m_dwWalkWaitTick = HUtil32.GetTickCount();
                    }
                    if (!m_boRunAwayMode)
                    {
                        if (!m_boNoAttackMode)
                        {
                            if (m_TargetCret != null)
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
                            if (m_TargetCret == null)
                            {
                                m_Master.GetBackPosition(ref nX, ref nY);
                                if (Math.Abs(m_nTargetX - nX) > 1 || Math.Abs(m_nTargetY - nY) > 1)
                                {
                                    m_nTargetX = nX;
                                    m_nTargetY = nY;
                                    if (Math.Abs(m_nCurrX - nX) <= 2 && Math.Abs(m_nCurrY - nY) <= 2)
                                    {
                                        if (m_PEnvir.GetMovingObject(nX, nY, true) != null)
                                        {
                                            m_nTargetX = m_nCurrX;
                                            m_nTargetY = m_nCurrY;
                                        }
                                    }
                                }
                            }
                            if (!m_Master.m_boSlaveRelax &&
                                (m_PEnvir != m_Master.m_PEnvir || Math.Abs(m_nCurrX - m_Master.m_nCurrX) > 20 ||
                                 Math.Abs(m_nCurrY - m_Master.m_nCurrY) > 20))
                            {
                                SpaceMove(m_Master.m_PEnvir.sMapName, m_nTargetX, m_nTargetY, 1);
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
                    if (m_Master != null && m_Master.m_boSlaveRelax)
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
                        if (m_TargetCret == null)
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