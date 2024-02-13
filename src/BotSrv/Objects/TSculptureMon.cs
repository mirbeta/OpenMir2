using BotSrv.Player;
using OpenMir2;
using SystemModule;

namespace BotSrv.Objects
{
    public class TSculptureMon : TSkeletonOma
    {
        public TSculptureMon(RobotPlayer robotClient) : base(robotClient)
        {
        }

        public override void Run()
        {
            long m_dwEffectFrameTimetime;
            if (m_nCurrentAction == Messages.SM_WALK || m_nCurrentAction == Messages.SM_BACKSTEP ||
                m_nCurrentAction == Messages.SM_RUN || m_nCurrentAction == Messages.SM_HORSERUN) return;
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