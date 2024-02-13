using BotSrv.Player;
using OpenMir2;

namespace BotSrv.Objects
{
    public class TNpcActor : Actor
    {
        private bool m_boDigUp;
        private long m_dwUseEffectTick;

        public TNpcActor(RobotPlayer robotClient) : base(robotClient)
        {
            m_boHitEffect = false;
            m_nHitEffectNumber = 0;
            m_boDigUp = false;
        }

        public override void Run()
        {
            int nEffectFrame;
            long dwEffectFrameTime;
            base.Run();
            nEffectFrame = m_nEffectFrame;
            if (m_boUseEffect)
            {
                if (m_boUseMagic)
                {
                    dwEffectFrameTime = HUtil32.Round(m_dwEffectFrameTime / 3);
                }
                else
                {
                    dwEffectFrameTime = m_dwEffectFrameTime;
                }

                if (MShare.GetTickCount() - m_dwEffectStartTime > dwEffectFrameTime)
                {
                    m_dwEffectStartTime = MShare.GetTickCount();
                    if (m_nEffectFrame < m_nEffectEnd)
                    {
                        m_nEffectFrame++;
                    }
                    else
                    {
                        if (m_boDigUp)
                        {
                            if (MShare.GetTickCount() > m_dwUseEffectTick)
                            {
                                m_boUseEffect = false;
                                m_boDigUp = false;
                                m_dwUseEffectTick = MShare.GetTickCount();
                            }

                            m_nEffectFrame = m_nEffectStart;
                        }
                        else
                        {
                            m_nEffectFrame = m_nEffectStart;
                        }

                        m_dwEffectStartTime = MShare.GetTickCount();
                    }
                }
            }
            if (nEffectFrame != m_nEffectFrame)
            {
                m_dwLoadSurfaceTime = MShare.GetTickCount();
            }
        }
    }
}