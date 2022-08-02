using System;
using System.Collections;
using SystemModule;

namespace RobotSvr
{
    public class TSkeletonOma: TActor
    {
        protected int ax = 0;
        protected int ay = 0;

        public TSkeletonOma() : base()
        {
            this.m_boUseEffect = false;
        }

        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_nCurrentFrame =  -1;
            this.m_boReverseFrame = false;
            this.m_boUseEffect = false;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    if (this.m_btRace >= 117 && this.m_btRace<= 119)
                    {
                        this.m_nStartFrame = pm.ActStand.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = pm.ActStand.frame;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    if (new ArrayList(new int[] {118, 119}).Contains(this.m_btRace))
                    {
                        this.m_boUseEffect = true;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                    }
                    break;
                case Grobal2.SM_WALK:
                case Grobal2.SM_BACKSTEP:
                    if (this.m_btRace >= 117 && this.m_btRace<= 119)
                    {
                        this.m_nStartFrame = pm.ActWalk.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActWalk.start + this.m_btDir * (pm.ActWalk.frame + pm.ActWalk.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActWalk.frame - 1;
                    this.m_dwFrameTime = pm.ActWalk.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = pm.ActWalk.usetick;
                    this.m_nCurTick = 0;
                    // WarMode := FALSE;
                    this.m_nMoveStep = 1;
                    if (new ArrayList(new int[] {118, 119}).Contains(this.m_btRace))
                    {
                        this.m_boUseEffect = true;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                    }
                    if (this.m_nCurrentAction == Grobal2.SM_WALK)
                    {
                        this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    else
                    {
                        // sm_backstep
                        this.Shift(ClFunc.GetBack(this.m_btDir), this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    break;
                case Grobal2.SM_DIGUP:
                    switch(this.m_btRace)
                    {
                        // Modify the A .. B: 23, 91 .. 93
                        case 23:
                        case 91:
                            this.m_nStartFrame = pm.ActDeath.start;
                            break;
                        default:
                            this.m_nStartFrame = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip);
                            break;
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                    this.m_dwFrameTime = pm.ActDeath.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_DIGDOWN:
                    if (this.m_btRace == 55)
                    {
                        this.m_nStartFrame = pm.ActCritical.start + this.m_btDir * (pm.ActCritical.frame + pm.ActCritical.skip);
                        this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                        this.m_dwFrameTime = pm.ActCritical.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_boReverseFrame = true;
                        this.Shift(this.m_btDir, 0, 0, 1);
                    }
                    break;
                case Grobal2.SM_HIT:
                case Grobal2.SM_FLYAXE:
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    // WarMode := TRUE;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    if ((this.m_btRace == 16) || (this.m_btRace == 54))
                    {
                        this.m_boUseEffect = true;
                    }
                    this.Shift(this.m_btDir, 0, 0, 1);
                    if (new ArrayList(new int[] {118, 119}).Contains(this.m_btRace))
                    {
                        this.m_boUseEffect = true;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                    }
                    break;
                case Grobal2.SM_STRUCK:
                    if (this.m_btRace >= 117 && this.m_btRace<= 119)
                    {
                        this.m_nStartFrame = pm.ActStruck.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActStruck.start + this.m_btDir * (pm.ActStruck.frame + pm.ActStruck.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStruck.frame - 1;
                    this.m_dwFrameTime = this.m_dwStruckFrameTime;
                    // pm.ActStruck.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DEATH:
                    if (this.m_btRace >= 117 && this.m_btRace<= 119)
                    {
                        this.m_nStartFrame = pm.ActDie.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_nStartFrame = this.m_nEndFrame;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_NOWDEATH:
                    if (this.m_btRace >= 117 && this.m_btRace<= 119)
                    {
                        this.m_nStartFrame = pm.ActDie.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    if (this.m_btRace != 22)
                    {
                        this.m_boUseEffect = true;
                    }
                    break;
                case Grobal2.SM_SKELETON:
                    this.m_nStartFrame = pm.ActDeath.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                    this.m_dwFrameTime = pm.ActDeath.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_ALIVE:
                    if (new ArrayList(new int[] {117}).Contains(this.m_btRace))
                    {
                        this.m_nStartFrame = pm.ActDeath.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                    this.m_dwFrameTime = pm.ActDeath.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
            }
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            int cf;
            TMonsterAction pm;
            result = 0;
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return result;
            }
            if (this.m_boDeath)
            {
                if (new ArrayList(new int[] {30, 151}).Contains(this.m_wAppearance))
                {
                    this.m_nDownDrawLevel = 1;
                }
                if (this.m_boSkeleton)
                {
                    result = pm.ActDeath.start;
                }
                else if (this.m_btRace == 120)
                {
                    result = 417;
                    this.m_boUseEffect = false;
                }
                else if (this.m_btRace >= 117 && this.m_btRace<= 119)
                {
                    result = pm.ActDie.start + (pm.ActDie.frame - 1);
                    this.m_boUseEffect = false;
                }
                else
                {
                    result = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip) + (pm.ActDie.frame - 1);
                }
            }
            else
            {
                this.m_nDefFrameCount = pm.ActStand.frame;
                if (this.m_nCurrentDefFrame < 0)
                {
                    cf = 0;
                }
                else if (this.m_nCurrentDefFrame >= pm.ActStand.frame)
                {
                    cf = 0;
                }
                else
                {
                    cf = this.m_nCurrentDefFrame;
                }
                if (this.m_btRace == 120)
                {
                    switch(this.m_nTempState)
                    {
                        case 1:
                            this.m_nStartFrame = 0;
                            break;
                        case 2:
                            this.m_nStartFrame = 80;
                            break;
                        case 3:
                            this.m_nStartFrame = 160;
                            break;
                        case 4:
                            this.m_nStartFrame = 240;
                            break;
                        case 5:
                            this.m_nStartFrame = 320;
                            break;
                    }
                    result = this.m_nStartFrame + cf;
                }
                else if (this.m_btRace >= 117 && this.m_btRace<= 119)
                {
                    result = pm.ActStand.start + cf;
                }
                else
                {
                    result = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip) + cf;
                }
                if (new ArrayList(new int[] {118, 119}).Contains(this.m_btRace))
                {
                    this.m_boUseEffect = true;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = pm.ActStand.ftime;
                }
                if (this.m_btRace == 118)
                {
                    this.m_nEffectFrame = 2730 + this.m_nCurrentFrame;
                }
                else if (this.m_btRace == 119)
                {
                    this.m_nEffectFrame = 2840 + this.m_nCurrentFrame;
                }
                else if (this.m_btRace == 120)
                {
                    this.m_boUseEffect = true;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = pm.ActStand.ftime;
                    this.m_nEffectFrame = 2940 + this.m_nCurrentFrame;
                }
            }
            return result;
        }

        public override void LoadSurface()
        {
            if ((this.m_btRace == 117) && this.m_boDeath)
            {
                this.m_BodySurface = null;
                return;
            }
            base.LoadSurface();
            switch(this.m_btRace)
            {
                case 14:
                case 15:
                case 17:
                case 22:
                case 53:
                    // 阁胶磐
                    if (this.m_boUseEffect)
                    {
                        EffectSurface = WMFile.Units.WMFile.g_WMons[3].GetCachedImage(Units.AxeMon.DEATHEFFECTBASE + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    break;
                case 23:
                    if (this.m_nCurrentAction == Grobal2.SM_DIGUP)
                    {
                        this.m_BodySurface = null;
                        EffectSurface = WMFile.Units.WMFile.g_WMons[4].GetCachedImage(this.m_nBodyOffset + this.m_nCurrentFrame, ref ax, ref ay);
                        this.m_boUseEffect = true;
                    }
                    else
                    {
                        this.m_boUseEffect = false;
                    }
                    break;
                // Modify the A .. B: 91 .. 93
                case 91:
                    if (this.m_nCurrentAction == Grobal2.SM_DIGUP)
                    {
                        this.m_BodySurface = null;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic7Images.GetCachedImage(this.m_nCurrentFrame, ref ax, ref ay);
                        this.m_boUseEffect = true;
                    }
                    else
                    {
                        this.m_boUseEffect = false;
                    }
                    break;
            }
            switch(this.m_wAppearance)
            {
                case 703:
                    if ((this.m_nCurrentAction == Grobal2.SM_DIGUP))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(220 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    else if ((this.m_nCurrentAction == Grobal2.SM_NOWDEATH))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(1970 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    break;
                case 705:
                    if ((this.m_nCurrentAction == Grobal2.SM_DIGUP))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(230 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    else if ((this.m_nCurrentAction == Grobal2.SM_NOWDEATH))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(1980 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    break;
                case 707:
                    if ((this.m_nCurrentAction == Grobal2.SM_DIGUP))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(240 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    else if ((this.m_nCurrentAction == Grobal2.SM_NOWDEATH))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(1990 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    break;
                case 704:
                    if ((this.m_nCurrentAction == Grobal2.SM_NOWDEATH))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(1970 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    break;
                case 706:
                    if ((this.m_nCurrentAction == Grobal2.SM_NOWDEATH))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(1980 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    break;
                case 708:
                    if ((this.m_nCurrentAction == Grobal2.SM_NOWDEATH))
                    {
                        this.m_boUseEffect = true;
                        EffectSurface = WMFile.Units.WMFile.g_WMagic8Images.GetCachedImage(1990 + this.m_nCurrentFrame - this.m_nStartFrame, ref ax, ref ay);
                    }
                    break;
            }
        }

        public override void Run()
        {
            int prv;
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
            // 荤款靛 瓤苞
            this.RunActSound(this.m_nCurrentFrame - this.m_nStartFrame);
            this.RunFrameAction(this.m_nCurrentFrame - this.m_nStartFrame);
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
                        // 悼累捞 场巢.
                        this.m_nCurrentAction = 0;
                        // 悼累 肯丰
                        this.m_boUseEffect = false;
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

        public override void DrawChr(TDirectDrawSurface dsurface, int dx, int dy, bool blend, bool boFlag, bool DrawOnSale)
        {
            TColorEffect ceff;
            if (!(this.m_btDir >= 0 && this.m_btDir<= 7))
            {
                return;
            }
                        if (MShare.GetTickCount() - this.m_dwLoadSurfaceTime > g_dwLoadSurfaceTime)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
            ceff = this.GetDrawEffectValue();
            if (this.m_BodySurface != null)
            {
                this.DrawEffSurface(dsurface, this.m_BodySurface, dx + this.m_nPx + this.m_nShiftX, dy + this.m_nPy + this.m_nShiftY, blend, ceff);
            }
            if (this.m_boUseEffect)
            {
                if (EffectSurface != null)
                {
                    cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, EffectSurface, 1);
                }
            }
        }

    } // end TSkeletonOma

    } // end TBanyaGuardMon

