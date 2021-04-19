﻿using System;
namespace M2Server
{
    public class TArcherGuard : TGuardUnit
    {

        public TArcherGuard() : base()
        {
            m_nViewRange = 12;
            m_boWantRefMsg = true;
            m_Castle = null;
            m_nDirection = 0;
        }

        private void sub_4A6B30(TBaseObject TargeTBaseObject)
        {
            int nPower;
            TAbility WAbil;
            m_btDirection = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, TargeTBaseObject.m_nCurrX, TargeTBaseObject.m_nCurrY);
            WAbil = m_WAbil;
            nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
            if (nPower > 0)
            {
                nPower = TargeTBaseObject.GetHitStruckDamage(this, nPower);
            }
            if (nPower > 0)
            {
                TargeTBaseObject.SetLastHiter(this);
                TargeTBaseObject.m_ExpHitter = null;
                TargeTBaseObject.StruckDamage(nPower);
                TargeTBaseObject.SendDelayMsg(grobal2.RM_STRUCK, grobal2.RM_10101, (short)nPower, TargeTBaseObject.m_WAbil.HP, TargeTBaseObject.m_WAbil.MaxHP, ObjectId, "", HUtil32._MAX(Math.Abs(m_nCurrX - TargeTBaseObject.m_nCurrX), Math.Abs(m_nCurrY - TargeTBaseObject.m_nCurrY)) * 50 + 600);
            }
            SendRefMsg(grobal2.RM_FLYAXE, m_btDirection, m_nCurrX, m_nCurrY, TargeTBaseObject.ObjectId, "");
        }

        public override void Run()
        {
            int nAbs;
            int nRage;
            TBaseObject BaseObject;
            TBaseObject TargetBaseObject;
            nRage = 9999;
            TargetBaseObject = null;
            if (!m_boDeath && !m_boGhost && (m_wStatusTimeArr[grobal2.POISON_STONE] == 0))
            {
                if ((HUtil32.GetTickCount() - m_dwWalkTick) >= m_nWalkSpeed)
                {
                    m_dwWalkTick = HUtil32.GetTickCount();
                    for (var i = 0; i < m_VisibleActors.Count; i++)
                    {
                        BaseObject = m_VisibleActors[i].BaseObject;
                        if (BaseObject.m_boDeath)
                        {
                            continue;
                        }
                        if (IsProperTarget(BaseObject))
                        {
                            nAbs = Math.Abs(m_nCurrX - BaseObject.m_nCurrX) + Math.Abs(m_nCurrY - BaseObject.m_nCurrY);
                            if (nAbs < nRage)
                            {
                                nRage = nAbs;
                                TargetBaseObject = BaseObject;
                            }
                        }
                    }
                    if (TargetBaseObject != null)
                    {
                        SetTargetCreat(TargetBaseObject);
                    }
                    else
                    {
                        DelTargetCreat();
                    }
                }
                if (m_TargetCret != null)
                {
                    if ((HUtil32.GetTickCount() - m_dwHitTick) >= m_nNextHitTime)
                    {
                        m_dwHitTick = HUtil32.GetTickCount();
                        sub_4A6B30(m_TargetCret);
                    }
                }
                else
                {
                    if ((m_nDirection >= 0) && (m_btDirection != m_nDirection))
                    {
                        TurnTo(m_nDirection);
                    }
                }
            }
            base.Run();
        }
    }
}

