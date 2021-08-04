using System;
using SystemModule;

namespace M2Server
{
    public class TFireballMonster : TMagicMonster
    {
        public TFireballMonster() : base()
        {
            m_dwSpellTick = HUtil32.GetTickCount();
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            TBaseObject baseobject;
            int nPower;
            if (!m_boDeath && !bo554 && !m_boGhost)
            {
                if (m_TargetCret != null)
                {
                    if (MagCanHitTarget(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_TargetCret))
                    {
                        if (IsProperTarget(m_TargetCret))
                        {
                            if (Math.Abs(m_nTargetX - m_nCurrX) <= 8 && Math.Abs(m_nTargetY - m_nCurrY) <= 8)
                            {
                                nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(m_WAbil.MC) - HUtil32.LoWord(m_WAbil.MC) + 1) + HUtil32.LoWord(m_WAbil.MC);
                                if (nPower > 0)
                                {
                                    baseobject = GetPoseCreate();
                                    if (baseobject != null && IsProperTarget(baseobject) && m_nAntiMagic >= 0)
                                    {
                                        nPower = baseobject.GetMagStruckDamage(this, nPower);
                                        if (nPower > 0)
                                        {
                                            baseobject.StruckDamage(nPower);
                                            if (HUtil32.GetTickCount() - m_dwSpellTick > m_nNextHitTime)
                                            {
                                                m_dwSpellTick = HUtil32.GetTickCount();
                                                SendRefMsg(grobal2.RM_SPELL, 48, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 48, "");
                                                SendRefMsg(grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(2, 48), HUtil32.MakeLong(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY), m_TargetCret.ObjectId, "");
                                                SendDelayMsg(grobal2.RM_STRUCK, grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY), 2, m_TargetCret.ObjectId, "", 600);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    BreakHolySeizeMode();
                }
                else
                {
                    m_TargetCret = null;
                }
                if (HUtil32.GetTickCount() - m_dwSearchEnemyTick > 8000 || HUtil32.GetTickCount() - m_dwSearchEnemyTick > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

