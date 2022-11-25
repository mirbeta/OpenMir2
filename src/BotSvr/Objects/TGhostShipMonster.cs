using SystemModule;

namespace BotSvr.Objects
{
    public class TGhostShipMonster : TActor
    {
        protected int ax = 0;
        protected int ax2 = 0;
        protected int ay = 0;
        protected int ay2 = 0;
        public bool FFireBall;
        protected byte firedir = 0;
        public bool FLighting;

        public TGhostShipMonster(RobotClient robotClient) : base(robotClient)
        {
            m_boUseEffect = false;
            FFireBall = false;
            FLighting = false;
        }

        public override void Run()
        {
            int prv;
            long dwFrameTimetime;
            if (m_nCurrentAction == Grobal2.SM_WALK || m_nCurrentAction == Grobal2.SM_BACKSTEP ||
                m_nCurrentAction == Grobal2.SM_RUN || m_nCurrentAction == Grobal2.SM_HORSERUN ||
                m_nCurrentAction == Grobal2.SM_RUSH || m_nCurrentAction == Grobal2.SM_RUSHKUNG)
            {
                return;
            }

            m_boMsgMuch = false;
            if (m_MsgList.Count >= 2) m_boMsgMuch = true;
            RunFrameAction(m_nCurrentFrame - m_nStartFrame);
            prv = m_nCurrentFrame;
            if (m_nCurrentAction != 0)
            {
                if (m_nCurrentFrame < m_nStartFrame || m_nCurrentFrame > m_nEndFrame) m_nCurrentFrame = m_nStartFrame;
                if (m_boMsgMuch)
                    dwFrameTimetime = HUtil32.Round(m_dwFrameTime * 2 / 3);
                else
                    dwFrameTimetime = m_dwFrameTime;
                if (MShare.GetTickCount() - m_dwStartTime > dwFrameTimetime)
                {
                    if (m_nCurrentFrame < m_nEndFrame)
                    {
                        if (m_boUseMagic)
                        {
                            if (m_nCurEffFrame == m_nSpellFrame - 2)
                            {
                                if (m_CurMagic.ServerMagicCode >= 0)
                                {
                                    m_nCurrentFrame++;
                                    m_nCurEffFrame++;
                                    m_dwStartTime = MShare.GetTickCount();
                                }
                            }
                            else
                            {
                                if (m_nCurrentFrame < m_nEndFrame - 1) m_nCurrentFrame++;
                                m_nCurEffFrame++;
                                m_dwStartTime = MShare.GetTickCount();
                            }
                        }
                        else
                        {
                            m_nCurrentFrame++;
                            m_dwStartTime = MShare.GetTickCount();
                        }
                    }
                    else
                    {
                        if (m_boDelActionAfterFinished) m_boDelActor = true;
                        ActionEnded();
                        m_nCurrentAction = 0;
                        m_boUseMagic = false;
                        m_boUseEffect = false;
                        m_boHitEffect = false;
                    }

                    if (m_boUseMagic)
                    {
                        if (m_nCurEffFrame == m_nSpellFrame - 1)
                        {
                            if (m_CurMagic.ServerMagicCode > 0)
                            {
                                var _wvar1 = m_CurMagic;
                                //robotClient.g_PlayScene.NewMagic(this, _wvar1.ServerMagicCode, _wvar1.EffectNumber, this.m_nCurrX, this.m_nCurrY, _wvar1.targx, _wvar1.targy, _wvar1.target, _wvar1.EffectType, _wvar1.Recusion, _wvar1.anitime, ref bofly, _wvar1.magfirelv);
                            }

                            m_CurMagic.ServerMagicCode = 0;
                        }
                    }
                }
                m_nCurrentDefFrame = 0;
                m_dwDefFrameTime = MShare.GetTickCount();
            }
            else if (MShare.GetTickCount() - m_dwSmoothMoveTime > 200)
            {
                if (MShare.GetTickCount() - m_dwDefFrameTime > 500)
                {
                    m_dwDefFrameTime = MShare.GetTickCount();
                    m_nCurrentDefFrame++;
                    if (m_nCurrentDefFrame >= m_nDefFrameCount) m_nCurrentDefFrame = 0;
                }

                DefaultMotion();
            }

            if (prv != m_nCurrentFrame) m_dwLoadSurfaceTime = MShare.GetTickCount();
        }
    }
}