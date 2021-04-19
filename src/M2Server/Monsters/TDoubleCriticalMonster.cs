namespace M2Server
{
    public class TDoubleCriticalMonster : TATMonster
    {
        public TDoubleCriticalMonster() : base()
        {
            m_boAnimal = false;
        }

        public override bool AttackTarget()
        {
            var result = false;
            byte btDir = 0;
            if (m_TargetCret == null)
            {
                return result;
            }
            if (TargetInSpitRange(m_TargetCret, ref btDir))
            {
                if (HUtil32.GetTickCount() - m_dwHitTick > m_nNextHitTime)
                {
                    m_dwHitTick = HUtil32.GetTickCount();
                    m_dwTargetFocusTick = HUtil32.GetTickCount();
                    DoubleAttack(btDir);
                    BreakHolySeizeMode();
                }
                result = true;
                return result;
            }
            if (m_TargetCret.m_PEnvir == m_PEnvir)
            {
                SetTargetXY(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
            }
            else
            {
                DelTargetCreat();
            }
            return result;
        }

        private void DoubleAttack(byte btDir)
        {
            short nX;
            short nY;
            TBaseObject BaseObject;
            m_btDirection = btDir;
            var WAbil = m_WAbil;
            var nDamage = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
            if (nDamage <= 0)
            {
                return;
            }
            SendRefMsg(grobal2.RM_HIT, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
            for (var i = 0; i <= 4; i++)
            {
                for (var k = 0; k <= 4; k++)
                {
                    if (M2Share.g_Config.SpitMap[btDir, i, k] == 1)
                    {
                        nX = (short)(m_nCurrX - 2 + k);
                        nY = (short)(m_nCurrY - 2 + i);
                        BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                        if (BaseObject != null && BaseObject != this && IsProperTarget(BaseObject) && M2Share.RandomNumber.Random(BaseObject.m_btSpeedPoint) < m_btHitPoint)
                        {
                            nDamage = BaseObject.GetHitStruckDamage(this, nDamage);
                            if (nDamage > 0)
                            {
                                BaseObject.StruckDamage(nDamage);
                                BaseObject.SendDelayMsg(grobal2.RM_STRUCK, grobal2.RM_10101, (short)nDamage, m_WAbil.HP, m_WAbil.MaxHP, ObjectId, "", 300);
                            }
                        }
                    }
                }
            }
        }
    }
}

