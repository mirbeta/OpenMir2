using System;
using SystemModule;

public class TDragonStatue: TSkeletonArcherMon
    {
        // Size: 0x270 0x54
        public TDirectDrawSurface n26C = null;
        // TDragonStatue
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_btDir = 0;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_DIGUP:
                    this.Shift(0, 0, 0, 1);
                    this.m_nStartFrame = 0;
                    this.m_nEndFrame = 0;
                    // blue
                    this.m_dwFrameTime = 100;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = 0;
                    this.m_nEndFrame = 9;
                    this.m_dwFrameTime = 100;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boUseEffect = true;
                    this.m_nEffectStart = 0;
                    this.m_nEffectFrame = 0;
                    this.m_nEffectEnd = 9;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = 100;
                    break;
            }
        }

        public TDragonStatue() : base()
        {
            n26C = null;
        }

        public override void LoadSurface()
        {
            TWMBaseImages mimg;
            mimg = WMFile.Units.WMFile.g_WDragonImg;
            if (mimg != null)
            {
                // + m_nCurrentFrame
                this.m_BodySurface = mimg.GetCachedImage(Actor.Units.Actor.GetOffset(this.m_wAppearance), ref this.m_nPx, ref this.m_nPy);
            }
            if (this.m_boUseEffect)
            {
                switch(this.m_btRace)
                {
                    // Modify the A .. B: 84 .. 86
                    case 84:
                        this.EffectSurface = mimg.GetCachedImage(310 + this.m_nEffectFrame, ref this.ax, ref this.ay);
                        break;
                    // Modify the A .. B: 87 .. 89
                    case 87:
                        this.EffectSurface = mimg.GetCachedImage(330 + this.m_nEffectFrame, ref this.ax, ref this.ay);
                        break;
                }
            }
        }

        public override void Run()
        {
            int prv;
            long dwEffectFrameTime;
            long m_dwFrameTimetime;
            bool bofly;
            this.m_btDir = 0;
            if ((this.m_nCurrentAction == Grobal2.SM_WALK) || (this.m_nCurrentAction == Grobal2.SM_BACKSTEP) || (this.m_nCurrentAction == Grobal2.SM_RUN) || (this.m_nCurrentAction == Grobal2.SM_HORSERUN))
            {
                return;
            }
            this.m_boMsgMuch = false;
            if (this.m_MsgList.Count >= MShare.MSGMUCH)
            {
                this.m_boMsgMuch = true;
            }
            if (this.m_boUseEffect)
            {
                if (this.m_boMsgMuch)
                {
                    dwEffectFrameTime = Math.Round(this.m_dwEffectFrameTime * 2 / 3);
                }
                else
                {
                    dwEffectFrameTime = this.m_dwEffectFrameTime;
                }
                if (MShare.GetTickCount() - this.m_dwEffectStartTime > dwEffectFrameTime)
                {
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    if (this.m_nEffectFrame < this.m_nEffectEnd)
                    {
                        this.m_nEffectFrame ++;
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
                        this.m_nCurrentFrame ++;
                        this.m_dwStartTime = MShare.GetTickCount();
                    }
                    else
                    {
                        this.m_nCurrentAction = 0;
                        this.m_boUseEffect = false;
                        this.m_boNowDeath = false;
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_LIGHTING) && (this.m_nCurrentFrame == 4))
                    {
                        // 74
                        // 74
                        // mtThunder
                       ClMain.g_PlayScene.NewMagic(this, 90, 90, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, 0, magiceff.TMagicType.mtExplosion, false, 30, ref bofly);
                                            }
                }
                this.m_nCurrentDefFrame = 0;
                this.m_dwDefFrameTime = MShare.GetTickCount();
            }
            else
            {
                if (((int)MShare.GetTickCount() - this.m_dwSmoothMoveTime) > 200)
                {
                    if (MShare.GetTickCount() - this.m_dwDefFrameTime > 500)
                    {
                        this.m_dwDefFrameTime = MShare.GetTickCount();
                        this.m_nCurrentDefFrame ++;
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

    } // end TDragonStatue

