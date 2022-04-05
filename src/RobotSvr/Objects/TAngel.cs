using System.Collections;
using SystemModule;

namespace RobotSvr
{
    public class TAngel: THumActor
    {
        protected TDirectDrawSurface EffectSurface = null;
        protected int ax = 0;
        protected int ay = 0;
        //Constructor  Create()
        public TAngel() : base()
        {
            EffectSurface = null;
            this.m_boUseEffect = false;
        }
        public override void SetSound()
        {
            base.SetSound();
            if (this.m_boUseMagic && (this.m_CurMagic.MagicSerial > 0))
            {
                this.m_nMagicStartSound = 10000 + this.m_CurMagic.MagicSerial * 10;
                this.m_nMagicFireSound = this.m_nMagicStartSound + 1;
                this.m_nMagicExplosionSound = this.m_nMagicStartSound + 2;
            }
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
                    this.m_nStartFrame = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = pm.ActStand.frame;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_WALK:
                    this.m_nStartFrame = pm.ActWalk.start + this.m_btDir * (pm.ActWalk.frame + pm.ActWalk.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActWalk.frame - 1;
                    this.m_dwFrameTime = pm.ActWalk.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = pm.ActWalk.usetick;
                    this.m_nCurTick = 0;
                    this.m_nMoveStep = 1;
                    this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    break;
                case Grobal2.SM_DIGUP:
                    if ((this.m_wAppearance == 330) || (this.m_wAppearance == 336))
                    {
                        this.m_nStartFrame = 4;
                        this.m_nEndFrame = this.m_nStartFrame + 6 - 1;
                        this.m_dwFrameTime = 120;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.Shift(this.m_btDir, 0, 0, 1);
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActDeath.start;
                        this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                        this.m_dwFrameTime = pm.ActDeath.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.Shift(this.m_btDir, 0, 0, 1);
                    }
                    break;
                case Grobal2.SM_SPELL:
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nCurEffFrame = 0;
                    this.m_boUseMagic = true;
                    this.m_nMagLight = 2;
                    this.m_nSpellFrame = pm.ActCritical.frame;
                    this.m_dwWaitMagicRequest = MShare.GetTickCount();
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_HIT:
                case Grobal2.SM_FLYAXE:
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseEffect = true;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_STRUCK:
                    this.m_nStartFrame = pm.ActStruck.start + this.m_btDir * (pm.ActStruck.frame + pm.ActStruck.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStruck.frame - 1;
                    this.m_dwFrameTime = this.m_dwStruckFrameTime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DEATH:
                    this.m_nStartFrame = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_nStartFrame = this.m_nEndFrame;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    if (this.m_btRace != 22)
                    {
                        this.m_boUseEffect = true;
                    }
                    break;
                case Grobal2.SM_ALIVE:
                    this.m_nStartFrame = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip);
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
                result = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip) + (pm.ActDie.frame - 1);
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
                result = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip) + cf;
            }
            return result;
        }

        public override void LoadSurface()
        {
            int nBody;
            int nEffect;
            int nDigUp;
            int nBodyOffset;
            // inherited LoadSurface;
            if (this.m_wAppearance == 330)
            {
                if (this.m_boUseEffect)
                {
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(20 + this.m_nEffectFrame, ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(360 + this.m_nEffectFrame, ref ax, ref ay);
                }
                else if (!this.m_boReverseFrame)
                {
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(20 + this.m_nCurrentFrame, ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(360 + this.m_nCurrentFrame, ref ax, ref ay);
                }
                else
                {
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(20 + this.m_nEndFrame - (this.m_nCurrentFrame - this.m_nStartFrame), ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(360 + this.m_nEndFrame - (this.m_nCurrentFrame - this.m_nStartFrame), ref ax, ref ay);
                }
                if (this.m_nCurrentAction == Grobal2.SM_DIGUP)
                {
                    // g_Screen.AddChatBoardString(Format('%d', [m_nCurrentFrame]), GetRGB(5), clWhite);
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(this.m_nCurrentFrame, ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(10 + this.m_nCurrentFrame, ref ax, ref ay);
                    this.m_boUseEffect = true;
                }
                else
                {
                    this.m_boUseEffect = false;
                }
                return;
            }
            if (this.m_wAppearance == 336)
            {
                nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
                // 1840
                nBody = nBodyOffset;
                nDigUp = nBodyOffset - 10;
                nEffect = 360 + nBodyOffset - 20;
                if (this.m_boUseEffect)
                {
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(nBody + this.m_nEffectFrame, ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(nEffect + this.m_nEffectFrame, ref ax, ref ay);
                }
                else if (!this.m_boReverseFrame)
                {
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(nBody + this.m_nCurrentFrame, ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(nEffect + this.m_nCurrentFrame, ref ax, ref ay);
                }
                else
                {
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(nBody + this.m_nEndFrame - (this.m_nCurrentFrame - this.m_nStartFrame), ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(nEffect + this.m_nEndFrame - (this.m_nCurrentFrame - this.m_nStartFrame), ref ax, ref ay);
                }
                if (this.m_nCurrentAction == Grobal2.SM_DIGUP)
                {
                    // g_Screen.AddChatBoardString(Format('%d', [FCurrentFrame]), GetRGB(5), clWhite);
                    this.m_BodySurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(this.m_nCurrentFrame, ref this.m_nPx, ref this.m_nPy);
                    EffectSurface = WMFile.Units.WMFile.g_WMons[34].GetCachedImage(nDigUp + this.m_nCurrentFrame, ref ax, ref ay);
                    this.m_boUseEffect = true;
                }
                else
                {
                    this.m_boUseEffect = false;
                }
                return;
            }
            if (this.m_boUseEffect)
            {
                this.m_BodySurface = WMFile.Units.WMFile.g_WMons[18].GetCachedImage(1280 + this.m_nEffectFrame, ref this.m_nPx, ref this.m_nPy);
                EffectSurface = WMFile.Units.WMFile.g_WMons[18].GetCachedImage(920 + this.m_nEffectFrame, ref ax, ref ay);
            }
            else if (!this.m_boReverseFrame)
            {
                this.m_BodySurface = WMFile.Units.WMFile.g_WMons[18].GetCachedImage(1280 + this.m_nCurrentFrame, ref this.m_nPx, ref this.m_nPy);
                EffectSurface = WMFile.Units.WMFile.g_WMons[18].GetCachedImage(920 + this.m_nCurrentFrame, ref ax, ref ay);
            }
            else
            {
                this.m_BodySurface = WMFile.Units.WMFile.g_WMons[18].GetCachedImage(1280 + this.m_nEndFrame - (this.m_nCurrentFrame - this.m_nStartFrame), ref this.m_nPx, ref this.m_nPy);
                EffectSurface = WMFile.Units.WMFile.g_WMons[18].GetCachedImage(920 + this.m_nEndFrame - (this.m_nCurrentFrame - this.m_nStartFrame), ref ax, ref ay);
            }
            if (this.m_nCurrentAction == Grobal2.SM_DIGUP)
            {
                this.m_BodySurface = null;
                EffectSurface = WMFile.Units.WMFile.g_WMons[18].GetCachedImage(920 + this.m_nCurrentFrame, ref ax, ref ay);
                this.m_boUseEffect = true;
            }
            else
            {
                this.m_boUseEffect = false;
            }
        }

        public override void DrawChr(TDirectDrawSurface dsurface, int dx, int dy, bool blend, bool boFlag, bool DrawOnSale)
        {
            int idx;
            TDirectDrawSurface d;
            TWMBaseImages wimg;
            bool bWin;
            int ShiftX;
            int ShiftY;
            TColorEffect ceff;
            int ax2;
            int ay2;
            if (!(this.m_btDir >= 0 && this.m_btDir<= 7))
            {
                return;
            }
                        if (MShare.GetTickCount() - this.m_dwLoadSurfaceTime > g_dwLoadSurfaceTime)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
            ShiftX = dx + this.m_nShiftX;
            ShiftY = dy + this.m_nShiftY;
            ceff = this.GetDrawEffectValue();
            if ((this.m_wAppearance == 330) || (this.m_wAppearance == 336))
            {
                bWin = (new ArrayList(new int[] {0, 6, 7}).Contains(this.m_btDir));
                if (EffectSurface != null)
                {
                    if (!bWin)
                    {
                        cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, EffectSurface, 1);
                    }
                }
                if (this.m_BodySurface != null)
                {
                    this.DrawEffSurface(dsurface, this.m_BodySurface, ShiftX + this.m_nPx, ShiftY + this.m_nPy, blend, ceff);
                }
                if (EffectSurface != null)
                {
                    if (bWin)
                    {
                        cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, EffectSurface, 1);
                    }
                }
            }
            else
            {
                if (EffectSurface != null)
                {
                    cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, EffectSurface, 1);
                }
                if (this.m_BodySurface != null)
                {
                    this.DrawEffSurface(dsurface, this.m_BodySurface, ShiftX + this.m_nPx, ShiftY + this.m_nPy, blend, ceff);
                }
            }
            if (this.m_boUseMagic && (this.m_CurMagic.EffectNumber > 0))
            {
                // sm_spell
                if (this.m_nCurEffFrame >= 0 && this.m_nCurEffFrame<= this.m_nSpellFrame - 1)
                {
                    wimg = null;
                    magiceff.Units.magiceff.GetEffectBase(this.m_CurMagic.EffectNumber - 1, 0, ref wimg, ref idx);
                    if (wimg != null)
                    {
                        idx = idx + this.m_nCurEffFrame;
                        d = wimg.GetCachedImage(idx, ref ax2, ref ay2);
                        if (d != null)
                        {
                            cliUtil.Units.cliUtil.DrawBlend(dsurface, ShiftX + ax2, ShiftY + ay2, d, 1);
                        }
                    }
                }
            }
            // 显示攻击效果
            if (this.m_boHitEffect)
            {
                if ((this.m_nHitEffectNumber > 0))
                {
                    magiceff.Units.magiceff.GetEffectBase(this.m_nHitEffectNumber - 1, 1, ref wimg, ref idx);
                    if (wimg != null)
                    {
                        idx = idx + this.m_btDir * 10 + (this.m_nCurrentFrame - this.m_nStartFrame);
                        d = wimg.GetCachedImage(idx, ref ax2, ref ay2);
                        if (d != null)
                        {
                            cliUtil.Units.cliUtil.DrawBlend(dsurface, ShiftX + ax2, ShiftY + ay2, d, 1);
                        }
                    }
                }
            }
        }

    } // end TAngel

    } // end TBanyaGuardMon

