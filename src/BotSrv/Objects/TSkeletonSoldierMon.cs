using System.Collections;
using BotSrv.Player;
using SystemModule;

namespace BotSrv.Objects;

public class TSkeletonSoldierMon : TGasKuDeGi
{
    public TSkeletonSoldierMon(RobotPlayer robotClient) : base(robotClient)
    {
    }

    public override void Run()
    {
        int prv;
        long m_dwEffectFrameTimetime;
        long m_dwFrameTimetime;
        if (m_nCurrentAction == Messages.SM_WALK || m_nCurrentAction == Messages.SM_BACKSTEP ||
            m_nCurrentAction == Messages.SM_RUN || m_nCurrentAction == Messages.SM_HORSERUN) return;
        m_boMsgMuch = false;
        if (m_MsgList.Count >= BotConst.MSGMUCH) m_boMsgMuch = true;
        RunFrameAction(m_nCurrentFrame - m_nStartFrame);
        if (m_boUseEffect)
        {
            if (m_boMsgMuch)
                m_dwEffectFrameTimetime = HUtil32.Round(m_dwEffectFrameTime * 2 / 3);
            else
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

        prv = m_nCurrentFrame;
        if (m_nCurrentAction != 0)
        {
            if (m_nCurrentFrame < m_nStartFrame || m_nCurrentFrame > m_nEndFrame) m_nCurrentFrame = m_nStartFrame;
            if (m_boMsgMuch)
                m_dwFrameTimetime = HUtil32.Round(m_dwFrameTime * 2 / 3);
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
                    //this.m_boNowDeath = false;
                }

                if (m_nCurrentAction == Messages.SM_LIGHTING)
                {
                    //if ((this.m_btRace == 117) && (this.m_nCurrentFrame - this.m_nStartFrame == 1))
                    //{
                    //    robotClient.g_PlayScene.NewMagic(this, Grobal2.MAGIC_SIDESTONE_ATT1, Grobal2.MAGIC_SIDESTONE_ATT1, this.m_nCurrX, this.m_nCurrY, this.m_nCurrX, this.m_nCurrY, this.m_nRecogId, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                    //}
                    //if ((this.m_nCurrentFrame - this.m_nStartFrame) == 4)
                    //{
                    //    if (this.m_btRace == 111)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, 7, 33, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 101)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, 1, 1, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, true, 20, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 70)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, 7, 9, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 71)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, 11, 32, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, true, 30, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 72)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, 11, 32, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 78)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, 11, 37, this.m_nCurrX, this.m_nCurrY, this.m_nCurrX, this.m_nCurrY, this.m_nRecogId, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 81)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, 7, 9, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 113)
                    //    {
                    //    }
                    //    else if (this.m_btRace == 114)
                    //    {
                    //        // 11,
                    //        robotClient.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_THUNDER, Grobal2.MAGIC_FOX_THUNDER, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                    //    }
                    //    else if (this.m_btRace == 115)
                    //    {
                    //        robotClient.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_FIRE2, Grobal2.MAGIC_FOX_FIRE2, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtExploBujauk, false, 30, ref bofly);
                    //        this.m_nMagicStartSound = 10130;
                    //        this.m_nMagicFireSound = 10131;
                    //        this.m_nMagicExplosionSound = 3426;
                    //    }
                    //}
                }

                //else if (this.m_nCurrentAction == Messages.SM_LIGHTING_1)
                //{
                //    if (this.m_nCurrentFrame - this.m_nStartFrame == 4)
                //    {
                //        if (this.m_btRace == 114)
                //        {
                //            robotClient.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_FIRE1, Grobal2.MAGIC_FOX_FIRE1, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                //        }
                //        else if (this.m_btRace == 115)
                //        {
                //            robotClient.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_CURSE, Grobal2.MAGIC_FOX_CURSE, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtExploBujauk, false, 30, ref bofly);
                //            this.m_nMagicStartSound = 10130;
                //            this.m_nMagicFireSound = 10131;
                //            this.m_nMagicExplosionSound = 3427;
                //        }
                //    }
                //}
                m_nCurrentDefFrame = 0;
                m_dwDefFrameTime = MShare.GetTickCount();
            }
        }
        else
        {
            if (new ArrayList(new[] { 118, 119 }).Contains(Race))
            {
                if (MShare.GetTickCount() - m_dwDefFrameTime > 150)
                {
                    m_dwDefFrameTime = MShare.GetTickCount();
                    m_nCurrentDefFrame++;
                    if (m_nCurrentDefFrame >= m_nDefFrameCount) m_nCurrentDefFrame = 0;
                }

                DefaultMotion();
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
        }

        if (prv != m_nCurrentFrame) m_dwLoadSurfaceTime = MShare.GetTickCount();
    }
}