using SystemModule;

namespace BotSvr.Objects
{
    public class TCentipedeKingMon : TKillingHerb
    {
        private readonly int ax = 0;
        private readonly int ay = 0;

        public TCentipedeKingMon(RobotClient robotClient) : base(robotClient)
        {

        }

        public override void Run()
        {
            if (m_nCurrentAction == Grobal2.SM_WALK || m_nCurrentAction == Grobal2.SM_BACKSTEP ||
                m_nCurrentAction == Grobal2.SM_HORSERUN || m_nCurrentAction == Grobal2.SM_RUN)
            {
                return;
            }
            if (m_boUseEffect)
            {
                if (MShare.GetTickCount() - m_dwEffectStartTime > m_dwEffectFrameTime)
                {
                    m_dwEffectStartTime = MShare.GetTickCount();
                    if (m_nEffectFrame < m_nEffectEnd)
                        m_nEffectFrame++;
                    else
                        m_boUseEffect = false;
                }
            }
            base.Run();
        }
    }
}