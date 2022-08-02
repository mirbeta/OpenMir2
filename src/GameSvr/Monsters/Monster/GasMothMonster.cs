using SystemModule;

namespace GameSvr
{
    public class GasMothMonster : GasAttackMonster
    {
        public GasMothMonster() : base()
        {
            m_nViewRange = 7;
        }

        protected override TBaseObject sub_4A9C78(byte bt05)
        {
            TBaseObject result;
            var BaseObject = base.sub_4A9C78(bt05);
            if (BaseObject != null && M2Share.RandomNumber.Random(3) == 0 && BaseObject.m_boHideMode)
            {
                BaseObject.m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;
            }
            result = BaseObject;
            return result;
        }

        public override void Run()
        {
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - m_dwWalkTick) >= m_nWalkSpeed)
            {
                if ((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    sub_4C959C();
                }
            }
            base.Run();
        }
    }
}

