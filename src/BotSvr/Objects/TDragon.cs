using SystemModule;

namespace BotSvr.Objects
{
    public class TDragon : TActor
    {
        protected int ax = 0;
        protected int ax2 = 0;
        protected int ay = 0;
        protected int ay2 = 0;
        protected byte firedir;

        public TDragon(RobotClient robotClient) : base(robotClient)
        {
            m_boUseEffect = false;
        }

        private void AttackEff()
        {
            if (m_boDeath) return;
        }

        public override void Run()
        {
            int prv;
            long m_dwEffectframetimetime;
            long m_dwFrameTimetime;
            if (m_nCurrentAction == Grobal2.SM_WALK || m_nCurrentAction == Grobal2.SM_BACKSTEP ||
                m_nCurrentAction == Grobal2.SM_RUN || m_nCurrentAction == Grobal2.SM_HORSERUN) return;
            m_boMsgMuch = false;
            if (m_MsgList.Count >= MShare.MSGMUCH) m_boMsgMuch = true;
            if (m_boRunSound) m_boRunSound = false;
            if (m_boUseEffect)
            {
                if (m_boMsgMuch)
                    m_dwEffectframetimetime = m_dwEffectFrameTime * 2 / 3;
                else
                    m_dwEffectframetimetime = m_dwEffectFrameTime;
                if (MShare.GetTickCount() - m_dwEffectStartTime > m_dwEffectframetimetime)
                {
                    m_dwEffectStartTime = MShare.GetTickCount();
                    if (m_nEffectFrame < m_nEffectEnd)
                        m_nEffectFrame++;
                    else
                        m_boUseEffect = false;
                }
            }

            prv = m_nCurrentFrame;
            if (m_nCurrentAction != 0)
            {
                if (m_nCurrentFrame < m_nStartFrame || m_nCurrentFrame > m_nEndFrame) m_nCurrentFrame = m_nStartFrame;
                if (m_boMsgMuch)
                    m_dwFrameTimetime = m_dwFrameTime * 2 / 3;
                else
                    m_dwFrameTimetime = m_dwFrameTime;
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
                    }

                    if (m_nCurrentAction == Grobal2.SM_LIGHTING)
                    {
                        AttackEff();
                    }
                    else if (m_nCurrentAction == Grobal2.SM_HIT)
                    {
                        if (m_btDir <= 4)
                        {
                        }
                        else if (m_btDir == 5)
                        {
                        }
                        else if (m_btDir >= 6)
                        {
                        }

                        if (m_nCurrentFrame - m_nStartFrame == 4)
                        {
                            // ClMain.g_PlayScene.NewMagic(this, nDir, nDir, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, true, 30, ref bofly);
                        }
                    }
                }

                m_nCurrentDefFrame = 0;
                m_dwDefFrameTime = MShare.GetTickCount();
            }
            else
            {
                if (MShare.GetTickCount() - m_dwSmoothMoveTime > 200)
                {
                    if (MShare.GetTickCount() - m_dwDefFrameTime > 500)
                    {
                        m_dwDefFrameTime = MShare.GetTickCount();
                        m_nCurrentDefFrame++;
                        if (m_nCurrentDefFrame >= m_nDefFrameCount) m_nCurrentDefFrame = 0;
                    }

                    DefaultMotion();
                }
            }

            if (prv != m_nCurrentFrame) m_dwLoadSurfaceTime = MShare.GetTickCount();
        }
    }
}