using System;
using SystemModule;

public class TStoneMonster: TSkeletonArcherMon
    {
        // Size: 0x270 0x4d 0x4b
        public TDirectDrawSurface n26C = null;
        // TStoneMonster
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_boUseMagic = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            this.m_btDir = 0;
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    this.m_nStartFrame = pm.ActStand.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = pm.ActStand.frame;
                    if (!this.m_boUseEffect)
                    {
                        this.m_boUseEffect = true;
                        this.m_nEffectFrame = this.m_nStartFrame;
                        this.m_nEffectStart = this.m_nStartFrame;
                        this.m_nEffectEnd = this.m_nEndFrame;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = 300;
                    }
                    break;
                case Grobal2.SM_HIT:
                    this.m_nStartFrame = pm.ActAttack.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    if (!this.m_boUseEffect)
                    {
                        this.m_boUseEffect = true;
                        this.m_nEffectFrame = this.m_nStartFrame;
                        this.m_nEffectStart = this.m_nStartFrame;
                        this.m_nEffectEnd = this.m_nStartFrame + 25;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = 150;
                    }
                    break;
                case Grobal2.SM_STRUCK:
                    this.m_nStartFrame = pm.ActStruck.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStruck.frame - 1;
                    this.m_dwFrameTime = pm.ActStruck.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DEATH:
                    this.m_nStartFrame = pm.ActDie.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boNowDeath = true;
                    this.m_nEffectFrame = this.m_nStartFrame;
                    this.m_nEffectStart = this.m_nStartFrame;
                    this.m_nEffectEnd = this.m_nStartFrame + 19;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = 80;
                    break;
            }
        }

        //Constructor  Create()
        public TStoneMonster() : base()
        {
            n26C = null;
            this.m_boUseEffect = false;
            this.m_boNowDeath = false;
        }
        public override void DrawEff(TDirectDrawSurface dsurface, int dx, int dy)
        {
            base.DrawEff(dsurface, dx, dy);
            if (this.m_boUseEffect && (n26C != null))
            {
                cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + this.ax + this.m_nShiftX, dy + this.ay + this.m_nShiftY, n26C, 1);
            }
        }

        public override void LoadSurface()
        {
            base.LoadSurface();
            if (this.m_boNowDeath)
            {
                switch(this.m_btRace)
                {
                    case 75:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(2530 + this.m_nEffectFrame - this.m_nEffectStart, ref this.n264, ref this.n268);
                        break;
                    case 77:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(2660 + this.m_nEffectFrame - this.m_nEffectStart, ref this.n264, ref this.n268);
                        break;
                }
            }
            else
            {
                if (this.m_boUseEffect)
                {
                    switch(this.m_btRace)
                    {
                        case 75:
                            switch(this.m_nCurrentAction)
                            {
                                case Grobal2.SM_HIT:
                                    n26C = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(2500 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                    break;
                                case Grobal2.SM_TURN:
                                    n26C = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(2490 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                    break;
                            }
                            break;
                        case 77:
                            switch(this.m_nCurrentAction)
                            {
                                case Grobal2.SM_HIT:
                                    n26C = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(2630 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                    break;
                                case Grobal2.SM_TURN:
                                    n26C = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(2620 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        public override void Run()
        {
            int prv;
            long m_dwEffectFrameTimetime;
            long m_dwFrameTimetime;
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
            if (this.m_boUseEffect || this.m_boNowDeath)
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
                        this.m_nEffectFrame ++;
                    }
                    else
                    {
                        this.m_boUseEffect = false;
                        this.m_boNowDeath = false;
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
                    }
                    this.m_nCurrentDefFrame = 0;
                    this.m_dwDefFrameTime = MShare.GetTickCount();
                }
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
            if ((prv != this.m_nCurrentFrame) || (prv != this.m_nEffectFrame))
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
        }

    } // end TStoneMonster

