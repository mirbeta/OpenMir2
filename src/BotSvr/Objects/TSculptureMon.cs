using SystemModule;

namespace BotSvr.Objects
{
    public class TSculptureMon : TSkeletonOma
    {
        private readonly int firedir;

        public TSculptureMon(RobotClient robotClient) : base(robotClient)
        {
        }

        public override void Run()
        {
            long m_dwEffectFrameTimetime;
            if (m_nCurrentAction == Grobal2.SM_WALK || m_nCurrentAction == Grobal2.SM_BACKSTEP ||
                m_nCurrentAction == Grobal2.SM_RUN || m_nCurrentAction == Grobal2.SM_HORSERUN) return;
            if (m_boUseEffect)
            {
                m_dwEffectFrameTimetime = m_dwEffectFrameTime;
                if (MShare.GetTickCount() - m_dwEffectStartTime > m_dwEffectFrameTimetime)
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