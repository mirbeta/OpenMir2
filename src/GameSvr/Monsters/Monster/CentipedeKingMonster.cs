using System;
using SystemModule;

namespace GameSvr
{
    public class CentipedeKingMonster : StickMonster
    {
        public int m_dwAttickTick = 0;

        public CentipedeKingMonster() : base()
        {
            m_nViewRange = 6;
            nComeOutValue = 4;
            nAttackRange = 6;
            m_boAnimal = false;
            m_dwAttickTick = HUtil32.GetTickCount();
        }

        private bool sub_4A5B0C()
        {
            var result = false;
            TBaseObject BaseObject;
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                BaseObject = m_VisibleActors[i].BaseObject;
                if (BaseObject.m_boDeath)
                {
                    continue;
                }
                if (IsProperTarget(BaseObject))
                {
                    if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) <= m_nViewRange && Math.Abs(m_nCurrY - BaseObject.m_nCurrY) <= m_nViewRange)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        protected override bool AttackTarget()
        {
            var result = false;
            TAbility WAbil;
            int nPower;
            TBaseObject BaseObject;
            if (!sub_4A5B0C())
            {
                return result;
            }
            if ((HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
            {
                m_dwHitTick = HUtil32.GetTickCount();
                SendAttackMsg(Grobal2.RM_HIT, m_btDirection, m_nCurrX, m_nCurrY);
                WAbil = m_WAbil;
                nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    BaseObject = m_VisibleActors[i].BaseObject;
                    if (BaseObject.m_boDeath)
                    {
                        continue;
                    }
                    if (IsProperTarget(BaseObject))
                    {
                        if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) < m_nViewRange && Math.Abs(m_nCurrY - BaseObject.m_nCurrY) < m_nViewRange)
                        {
                            m_dwTargetFocusTick = HUtil32.GetTickCount();
                            SendDelayMsg(this, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(BaseObject.m_nCurrX, BaseObject.m_nCurrY), 2, BaseObject.ObjectId, "", 600);
                            if (M2Share.RandomNumber.Random(4) == 0)
                            {
                                if (M2Share.RandomNumber.Random(3) != 0)
                                {
                                    BaseObject.MakePosion(Grobal2.POISON_DECHEALTH, 60, 3);
                                }
                                else
                                {
                                    BaseObject.MakePosion(Grobal2.POISON_STONE, 5, 0);
                                }
                                m_TargetCret = BaseObject;
                            }
                        }
                    }
                }
            }
            result = true;
            return result;
        }

        protected override void ComeOut()
        {
            base.ComeOut();
            m_WAbil.HP = m_WAbil.MaxHP;
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (!m_boGhost && !m_boDeath && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - m_dwWalkTick) > m_nWalkSpeed)
                {
                    m_dwWalkTick = HUtil32.GetTickCount();
                    if (m_boFixedHideMode)
                    {
                        if ((HUtil32.GetTickCount() - m_dwAttickTick) > 10000)
                        {
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
                                        if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) < nComeOutValue && Math.Abs(m_nCurrY - BaseObject.m_nCurrY) < nComeOutValue)
                                        {
                                            ComeOut();
                                            m_dwAttickTick = HUtil32.GetTickCount();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - m_dwAttickTick) > 3000)
                        {
                            if (AttackTarget())
                            {
                                base.Run();
                                return;
                            }
                            if ((HUtil32.GetTickCount() - m_dwAttickTick) > 10000)
                            {
                                ComeDown();
                                m_dwAttickTick = HUtil32.GetTickCount();
                            }
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

