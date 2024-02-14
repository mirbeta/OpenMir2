using BotSrv.Player;
using OpenMir2;

namespace BotSrv.Objects
{
    public class TSkeletonKingMon : TGasKuDeGi
    {
        public TSkeletonKingMon(RobotPlayer robotClient) : base(robotClient)
        {

        }

        public override void Run()
        {
            int prv;
            long m_dwEffectFrameTimetime;
            long m_dwFrameTimetime;
            if (m_nCurrentAction == Messages.SM_WALK || m_nCurrentAction == Messages.SM_BACKSTEP ||
                m_nCurrentAction == Messages.SM_RUN || m_nCurrentAction == Messages.SM_HORSERUN)
            {
                return;
            }

            m_boMsgMuch = false;
            if (m_MsgList.Count >= BotConst.MSGMUCH)
            {
                m_boMsgMuch = true;
            }

            RunFrameAction(m_nCurrentFrame - m_nStartFrame);
            if (m_boUseEffect)
            {
                if (m_boMsgMuch)
                {
                    m_dwEffectFrameTimetime = HUtil32.Round(m_dwEffectFrameTime * 2 / 3);
                }
                else
                {
                    m_dwEffectFrameTimetime = m_dwEffectFrameTime;
                }

                if (MShare.GetTickCount() - m_dwEffectStartTime > m_dwEffectFrameTimetime)
                {
                    m_dwEffectStartTime = MShare.GetTickCount();
                    if (m_nEffectFrame < m_nEffectEnd)
                    {
                        m_nEffectFrame++;
                    }
                    else
                    {
                        m_boUseEffect = false;
                    }
                }
            }

            prv = m_nCurrentFrame;
            if (m_nCurrentAction != 0)
            {
                if (m_nCurrentFrame < m_nStartFrame || m_nCurrentFrame > m_nEndFrame)
                {
                    m_nCurrentFrame = m_nStartFrame;
                }

                if (m_boMsgMuch)
                {
                    m_dwFrameTimetime = HUtil32.Round(m_dwFrameTime * 2 / 3);
                }
                else
                {
                    m_dwFrameTimetime = m_dwFrameTime;
                }

                if (MShare.GetTickCount() - m_dwStartTime > m_dwFrameTimetime)
                {
                    if (m_nCurrentFrame < m_nEndFrame)
                    {
                        m_nCurrentFrame++;
                        m_dwStartTime = MShare.GetTickCount();
                    }
                    else
                    {
                        m_nCurrentAction = 0;
                        m_boUseEffect = false;
                        BoUseDieEffect = false;
                    }

                    m_nCurrentDefFrame = 0;
                    m_dwDefFrameTime = MShare.GetTickCount();
                }
            }
            else
            {
                if (MShare.GetTickCount() - m_dwSmoothMoveTime > 200)
                {
                    if (MShare.GetTickCount() - m_dwDefFrameTime > 500)
                    {
                        m_dwDefFrameTime = MShare.GetTickCount();
                        m_nCurrentDefFrame++;
                        if (m_nCurrentDefFrame >= m_nDefFrameCount)
                        {
                            m_nCurrentDefFrame = 0;
                        }
                    }

                    DefaultMotion();
                }
            }

            if (prv != m_nCurrentFrame)
            {
                m_dwLoadSurfaceTime = MShare.GetTickCount();
            }
        }
    }
}