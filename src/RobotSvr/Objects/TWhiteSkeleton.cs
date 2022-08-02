using SystemModule;

namespace RobotSvr
{
    public class TWhiteSkeleton: TSkeletonOma
    {
        public override void CalcActorFrame()
        {
            base.CalcActorFrame();
            this.m_boUseMagic = false;
            this.m_nCurrentFrame =  -1;
            this.m_nHitEffectNumber = 0;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            this.m_Action = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (this.m_Action == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_POWERHIT:
                    this.m_nStartFrame = this.m_Action.ActAttack.start + this.m_btDir * (this.m_Action.ActAttack.frame + this.m_Action.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + this.m_Action.ActAttack.frame - 1;
                    this.m_dwFrameTime = this.m_Action.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    if ((this.m_nCurrentAction == Grobal2.SM_POWERHIT))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 1;
                        switch(this.m_btRace)
                        {
                            case 91:
                                this.m_nHitEffectNumber += 101;
                                break;
                            case 92:
                                this.m_nHitEffectNumber += 201;
                                break;
                            case 93:
                                this.m_nHitEffectNumber += 301;
                                break;
                        }
                    }
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
            }
        }

        public override void DrawChr(TDirectDrawSurface dsurface, int dx, int dy, bool blend, bool boFlag, bool DrawOnSale)
        {
            int idx;
            int ax;
            int ay;
            TDirectDrawSurface d;
            TWMBaseImages wimg;
            if (!(this.m_btDir >= 0 && this.m_btDir<= 7))
            {
                return;
            }
            base.DrawChr(dsurface, dx, dy, blend, boFlag, DrawOnSale);
            d = null;
            if (this.m_boUseMagic && (this.m_CurMagic.EffectNumber > 0))
            {
                if (this.m_nCurEffFrame >= 0 && this.m_nCurEffFrame<= this.m_nSpellFrame - 1)
                {
                    magiceff.Units.magiceff.GetEffectBase(this.m_CurMagic.EffectNumber - 1, 0, ref wimg, ref idx);
                    idx = idx + this.m_nCurEffFrame;
                    d = null;
                    if (wimg != null)
                    {
                        d = wimg.GetCachedImage(idx, ref ax, ref ay);
                    }
                    if (d != null)
                    {
                        cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, d, 1);
                    }
                }
            }
            if (this.m_boHitEffect && (this.m_nHitEffectNumber > 0))
            {
                magiceff.Units.magiceff.GetEffectBase(this.m_nHitEffectNumber - 1, 1, ref wimg, ref idx);
                if (wimg != null)
                {
                    idx = idx + this.m_btDir * 10 + (this.m_nCurrentFrame - this.m_nStartFrame);
                    d = wimg.GetCachedImage(idx, ref ax, ref ay);
                }
                if (d != null)
                {
                    cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, d, 1);
                }
            }
        }

    } // end TWhiteSkeleton

    } // end TBanyaGuardMon

