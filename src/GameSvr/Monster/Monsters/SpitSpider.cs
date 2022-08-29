using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class SpitSpider : AtMonster
    {
        public bool m_boUsePoison;

        public SpitSpider() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            m_boAnimal = true;
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
            SendRefMsg(Grobal2.RM_HIT, Direction, m_nCurrX, m_nCurrY, 0, "");
            for (var i = 0; i < 4; i++)
            {
                for (var k = 0; k < 4; k++)
                {
                    if (M2Share.g_Config.SpitMap[btDir, i, k] == 1)
                    {
                        var nX = (short)(m_nCurrX - 2 + k);
                        var nY = (short)(m_nCurrY - 2 + i);
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
            if (m_TargetCret == null)
            {
                return false;
            }
            if (TargetInSpitRange(m_TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
                {
                    m_dwHitTick = HUtil32.GetTickCount();
                    m_dwTargetFocusTick = HUtil32.GetTickCount();
                    SpitAttack(btDir);
                    BreakHolySeizeMode();
                }
                return true;
            }
            if (m_TargetCret.m_PEnvir == m_PEnvir)
            {
                SetTargetXY(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
            }
            else
            {
                DelTargetCreat();
            }
            return false;
        }
    }
}

