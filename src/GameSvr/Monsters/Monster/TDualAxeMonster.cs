using System;
using SystemModule;

namespace GameSvr
{
    public class TDualAxeMonster : TMonster
    {
        private int m_nAttackCount = 0;
        protected int m_nAttackMax = 0;

        private void FlyAxeAttack(TBaseObject Target)
        {
            if (m_PEnvir.CanFly(m_nCurrX, m_nCurrY, Target.m_nCurrX, Target.m_nCurrY))
            {
                m_btDirection = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, Target.m_nCurrX, Target.m_nCurrY);
                var WAbil = m_WAbil;
                var nDamage = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                if (nDamage > 0)
                {
                    nDamage = Target.GetHitStruckDamage(this, nDamage);
                }
                if (nDamage > 0)
                {
                    Target.StruckDamage(nDamage);
                    Target.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, Target.m_WAbil.HP, Target.m_WAbil.MaxHP, ObjectId, "", HUtil32._MAX(Math.Abs(m_nCurrX - Target.m_nCurrX), Math.Abs(m_nCurrY - Target.m_nCurrY)) * 50 + 600);
                }
                SendRefMsg(Grobal2.RM_FLYAXE, m_btDirection, m_nCurrX, m_nCurrY, Target.ObjectId, "");
            }
        }

        protected override bool AttackTarget()
        {
            var result = false;
            if (m_TargetCret == null)
            {
                return result;
            }
            if ((HUtil32.GetTickCount() - m_dwHitTick )> m_nNextHitTime)
            {
                m_dwHitTick = HUtil32.GetTickCount();
                if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 7 && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 7)
                {
                    if (m_nAttackMax - 1 > m_nAttackCount)
                    {
                        m_nAttackCount++;
                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                        FlyAxeAttack(m_TargetCret);
                    }
                    else
                    {
                        if (M2Share.RandomNumber.Random(5) == 0)
                        {
                            m_nAttackCount = 0;
                        }
                    }
                    result = true;
                    return result;
                }
                if (m_TargetCret.m_PEnvir == m_PEnvir)
                {
                    if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 11 && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 11)
                    {
                        SetTargetXY(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                    }
                }
                else
                {
                    DelTargetCreat();
                }
            }
            return result;
        }

        public TDualAxeMonster() : base()
        {
            m_nViewRange = 5;
            m_nRunTime = 250;
            m_dwSearchTime = 3000;
            m_nAttackCount = 0;
            m_nAttackMax = 2;
            m_dwSearchTick = HUtil32.GetTickCount();
        }

        public override void Run()
        {
            int nAbs;
            int nRage = 9999;
            TBaseObject BaseObject;
            TBaseObject TargetBaseObject = null;
            if (!m_boDeath && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - m_dwSearchEnemyTick) >= 5000)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    for (var i = 0; i < m_VisibleActors.Count; i++)
                    {
                        BaseObject = m_VisibleActors[i].BaseObject;
                        if (BaseObject.m_boDeath)
                        {
                            continue;
                        }
                        if (IsProperTarget(BaseObject))
                        {
                            if (!BaseObject.m_boHideMode || m_boCoolEye)
                            {
                                nAbs = Math.Abs(m_nCurrX - BaseObject.m_nCurrX) + Math.Abs(m_nCurrY - BaseObject.m_nCurrY);
                                if (nAbs < nRage)
                                {
                                    nRage = nAbs;
                                    TargetBaseObject = BaseObject;
                                }
                            }
                        }
                    }
                    if (TargetBaseObject != null)
                    {
                        SetTargetCreat(TargetBaseObject);
                    }
                }
                if ((HUtil32.GetTickCount() - m_dwWalkTick) > m_nWalkSpeed && m_TargetCret != null)
                {
                    if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4 && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4)
                    {
                        if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 2 && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 2)
                        {
                            if (M2Share.RandomNumber.Random(5) == 0)
                            {
                                GetBackPosition(ref m_nTargetX, ref m_nTargetY);
                            }
                        }
                        else
                        {
                            GetBackPosition(ref m_nTargetX, ref m_nTargetY);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

