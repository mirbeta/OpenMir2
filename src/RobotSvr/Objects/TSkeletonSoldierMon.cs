using System;
using System.Collections;
using SystemModule;

namespace RobotSvr
{
    public class TSkeletonSoldierMon : TGasKuDeGi
    {
        public override void Run()
        {
            int prv;
            long m_dwEffectFrameTimetime;
            long m_dwFrameTimetime;
            bool bofly;
            if ((this.m_nCurrentAction == Grobal2.SM_WALK) || (this.m_nCurrentAction == Grobal2.SM_BACKSTEP) || (this.m_nCurrentAction == Grobal2.SM_RUN) || (this.m_nCurrentAction == Grobal2.SM_HORSERUN))
            {
                return;
            }
            this.m_boMsgMuch = false;
            if (this.m_MsgList.Count >= MShare.MSGMUCH)
            {
                this.m_boMsgMuch = true;
            }
            this.RunActSound(this.m_nCurrentFrame - this.m_nStartFrame);
            this.RunFrameAction(this.m_nCurrentFrame - this.m_nStartFrame);
            if (this.m_boUseEffect)
            {
                if (this.m_boMsgMuch)
                {
                    m_dwEffectFrameTimetime = Math.Round(this.m_dwEffectFrameTime * 2 / 3);
                }
                else
                {
                    m_dwEffectFrameTimetime = this.m_dwEffectFrameTime;
                }
                if (MShare.GetTickCount() - this.m_dwEffectStartTime > m_dwEffectFrameTimetime)
                {
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    if (this.m_nEffectFrame < this.m_nEffectEnd)
                    {
                        this.m_nEffectFrame++;
                    }
                    else
                    {
                        this.m_boUseEffect = false;
                    }
                }
            }
            prv = this.m_nCurrentFrame;
            if (this.m_nCurrentAction != 0)
            {
                if ((this.m_nCurrentFrame < this.m_nStartFrame) || (this.m_nCurrentFrame > this.m_nEndFrame))
                {
                    this.m_nCurrentFrame = this.m_nStartFrame;
                }
                if (this.m_boMsgMuch)
                {
                    m_dwFrameTimetime = Math.Round(this.m_dwFrameTime * 2 / 3);
                }
                else
                {
                    m_dwFrameTimetime = this.m_dwFrameTime;
                }
                if (MShare.GetTickCount() - this.m_dwStartTime > m_dwFrameTimetime)
                {
                    if (this.m_nCurrentFrame < this.m_nEndFrame)
                    {
                        this.m_nCurrentFrame++;
                        this.m_dwStartTime = MShare.GetTickCount();
                    }
                    else
                    {
                        this.m_nCurrentAction = 0;
                        this.m_boUseEffect = false;
                        this.m_boNowDeath = false;
                    }
                    if (this.m_nCurrentAction == Grobal2.SM_LIGHTING)
                    {
                        if ((this.m_btRace == 117) && (this.m_nCurrentFrame - this.m_nStartFrame == 1))
                        {
                            ClMain.g_PlayScene.NewMagic(this, Grobal2.MAGIC_SIDESTONE_ATT1, Grobal2.MAGIC_SIDESTONE_ATT1, this.m_nCurrX, this.m_nCurrY, this.m_nCurrX, this.m_nCurrY, this.m_nRecogId, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                        }
                        if ((this.m_nCurrentFrame - this.m_nStartFrame) == 4)
                        {
                            if ((this.m_btRace == 111))
                            {
                                // RightGuard
                                ClMain.g_PlayScene.NewMagic(this, 7, 33, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 101))
                            {
                                // RightGuard
                                ClMain.g_PlayScene.NewMagic(this, 1, 1, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, true, 20, ref bofly);
                            }
                            // or (m_btRace = 81)
                            else if ((this.m_btRace == 70))
                            {
                                // m_nMagicNum
                                // 8
                                ClMain.g_PlayScene.NewMagic(this, 7, 9, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 71))
                            {
                                // 1
                                // 1
                                ClMain.g_PlayScene.NewMagic(this, 11, 32, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, true, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 72))
                            {
                                ClMain.g_PlayScene.NewMagic(this, 11, 32, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 78))
                            {
                                ClMain.g_PlayScene.NewMagic(this, 11, 37, this.m_nCurrX, this.m_nCurrY, this.m_nCurrX, this.m_nCurrY, this.m_nRecogId, magiceff.TMagicType.mtGroundEffect, false, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 81))
                            {
                                ClMain.g_PlayScene.NewMagic(this, 7, 9, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 113))
                            {
                            }
                            else if ((this.m_btRace == 114))
                            {
                                // 11,
                                ClMain.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_THUNDER, Grobal2.MAGIC_FOX_THUNDER, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 115))
                            {
                                ClMain.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_FIRE2, Grobal2.MAGIC_FOX_FIRE2, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtExploBujauk, false, 30, ref bofly);
                                this.m_nMagicStartSound = 10130;
                                this.m_nMagicFireSound = 10131;
                                this.m_nMagicExplosionSound = 3426;
                            }
                        }
                    }
                    else if (this.m_nCurrentAction == Grobal2.SM_LIGHTING_1)
                    {
                        if ((this.m_nCurrentFrame - this.m_nStartFrame == 4))
                        {
                            if ((this.m_btRace == 114))
                            {
                                ClMain.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_FIRE1, Grobal2.MAGIC_FOX_FIRE1, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtThunder, false, 30, ref bofly);
                            }
                            else if ((this.m_btRace == 115))
                            {
                                ClMain.g_PlayScene.NewMagic(this, Grobal2.MAGIC_FOX_CURSE, Grobal2.MAGIC_FOX_CURSE, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtExploBujauk, false, 30, ref bofly);
                                this.m_nMagicStartSound = 10130;
                                this.m_nMagicFireSound = 10131;
                                this.m_nMagicExplosionSound = 3427;
                            }
                        }
                    }
                    this.m_nCurrentDefFrame = 0;
                    this.m_dwDefFrameTime = MShare.GetTickCount();
                }
            }
            else
            {
                if (new ArrayList(new int[] { 118, 119 }).Contains(this.m_btRace))
                {
                    if (MShare.GetTickCount() - this.m_dwDefFrameTime > 150)
                    {
                        this.m_dwDefFrameTime = MShare.GetTickCount();
                        this.m_nCurrentDefFrame++;
                        if (this.m_nCurrentDefFrame >= this.m_nDefFrameCount)
                        {
                            this.m_nCurrentDefFrame = 0;
                        }
                    }
                    this.DefaultMotion();
                }
                else if (((int)MShare.GetTickCount() - this.m_dwSmoothMoveTime) > 200)
                {
                    if (MShare.GetTickCount() - this.m_dwDefFrameTime > 500)
                    {
                        this.m_dwDefFrameTime = MShare.GetTickCount();
                        this.m_nCurrentDefFrame++;
                        if (this.m_nCurrentDefFrame >= this.m_nDefFrameCount)
                        {
                            this.m_nCurrentDefFrame = 0;
                        }
                    }
                    this.DefaultMotion();
                }
            }
            if (prv != this.m_nCurrentFrame)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
        }

    }
}