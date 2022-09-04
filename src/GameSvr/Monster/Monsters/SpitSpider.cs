﻿using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class SpitSpider : AtMonster
    {
        public bool m_boUsePoison;

        public SpitSpider() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            Animal = true;
            m_boUsePoison = true;
        }

        private void SpitAttack(byte btDir)
        {
            Direction = btDir;
            var WAbil = m_WAbil;
            var nDamage = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
            if (nDamage <= 0)
            {
                return;
            }
            SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
            for (var i = 0; i < 4; i++)
            {
                for (var k = 0; k < 4; k++)
                {
                    if (M2Share.g_Config.SpitMap[btDir, i, k] == 1)
                    {
                        var nX = (short)(CurrX - 2 + k);
                        var nY = (short)(CurrY - 2 + i);
                        var BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                        if (BaseObject != null && BaseObject != this && IsProperTarget(BaseObject) && M2Share.RandomNumber.Random(BaseObject.m_btSpeedPoint) < m_btHitPoint)
                        {
                            nDamage = BaseObject.GetMagStruckDamage(this, nDamage);
                            if (nDamage > 0)
                            {
                                BaseObject.StruckDamage(nDamage);
                                BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, m_WAbil.HP, m_WAbil.MaxHP, ObjectId, "", 300);
                                if (m_boUsePoison)
                                {
                                    if (M2Share.RandomNumber.Random(m_btAntiPoison + 20) == 0)
                                    {
                                        BaseObject.MakePosion(Grobal2.POISON_DECHEALTH, 30, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override bool AttackTarget()
        {
            byte btDir = 0;
            if (TargetCret == null)
            {
                return false;
            }
            if (TargetInSpitRange(TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    SpitAttack(btDir);
                    BreakHolySeizeMode();
                }
                return true;
            }
            if (TargetCret.m_PEnvir == m_PEnvir)
            {
                SetTargetXY(TargetCret.CurrX, TargetCret.CurrY);
            }
            else
            {
                DelTargetCreat();
            }
            return false;
        }
    }
}

