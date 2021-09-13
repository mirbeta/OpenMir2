using SystemModule;

namespace GameSvr
{
    public class TGasAttackMonster : TATMonster
    {
        public TGasAttackMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            m_boAnimal = true;
        }

        public virtual TBaseObject sub_4A9C78(byte bt05)
        {
            TBaseObject BaseObject;
            TBaseObject result = null;
            m_btDirection = bt05;
            var WAbil = m_WAbil;
            var n10 = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
            if (n10 > 0)
            {
                SendRefMsg(Grobal2.RM_HIT, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                BaseObject = GetPoseCreate();
                if (BaseObject != null && IsProperTarget(BaseObject) && M2Share.RandomNumber.Random(BaseObject.m_btSpeedPoint) < m_btHitPoint)
                {
                    n10 = BaseObject.GetMagStruckDamage(this, n10);
                    if (n10 > 0)
                    {
                        BaseObject.StruckDamage(n10);
                        BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)n10, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ObjectId, "", 300);
                        if (M2Share.RandomNumber.Random(BaseObject.m_btAntiPoison + 20) == 0)
                        {
                            BaseObject.MakePosion(Grobal2.POISON_STONE, 5, 0);
                        }
                        result = BaseObject;
                    }
                }
            }
            return result;
        }

        protected override bool AttackTarget()
        {
            var result = false;
            byte btDir = 0;
            if (m_TargetCret == null)
            {
                return result;
            }
            if (GetAttackDir(m_TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
                {
                    m_dwHitTick = HUtil32.GetTickCount();
                    m_dwTargetFocusTick = HUtil32.GetTickCount();
                    sub_4A9C78(btDir);
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
            return result;
        }
    }
}

