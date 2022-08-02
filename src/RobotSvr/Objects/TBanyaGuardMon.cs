using System.Collections;
using SystemModule;

namespace RobotSvr
{
    public class TBanyaGuardMon: TSkeletonArcherMon
    {
        public TDirectDrawSurface m_DrawEffect = null;
        // TBanyaGuardMon
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_HIT:
                    if (this.m_btRace >= 117 && this.m_btRace<= 119)
                    {
                        this.m_nStartFrame = pm.ActAttack.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    if (!(new ArrayList(new int[] {27, 28, 111}).Contains(this.m_btRace)))
                    {
                        this.m_boUseEffect = true;
                        this.m_nEffectFrame = this.m_nStartFrame;
                        this.m_nEffectStart = this.m_nStartFrame;
                        this.m_nEffectEnd = this.m_nEndFrame;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    }
                    if (new ArrayList(new int[] {113, 114, 115}).Contains(this.m_btRace))
                    {
                        this.m_boUseEffect = false;
                    }
                    break;
                case Grobal2.SM_LIGHTING:
                case Grobal2.SM_LIGHTING_1:
                    if ((this.m_btRace >= 117 && this.m_btRace<= 119))
                    {
                        this.m_nStartFrame = pm.ActCritical.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActCritical.start + this.m_btDir * (pm.ActCritical.frame + pm.ActCritical.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                    this.m_dwFrameTime = pm.ActCritical.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nCurEffFrame = 0;
                    this.m_boUseMagic = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    if ((new ArrayList(new int[] {71, 72, 111}).Contains(this.m_btRace)))
                    {
                        this.m_boUseEffect = true;
                        this.m_nEffectFrame = this.m_nStartFrame;
                        this.m_nEffectStart = this.m_nStartFrame;
                        this.m_nEffectEnd = this.m_nEndFrame;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    }
                    else if ((new ArrayList(new int[] {27, 28}).Contains(this.m_btRace)))
                    {
                        this.m_boUseEffect = true;
                        this.m_nEffectFrame = this.m_nStartFrame;
                        this.m_nEffectStart = this.m_nStartFrame;
                        this.m_nEffectEnd = this.m_nEndFrame;
                        if (this.m_btRace == 28)
                        {
                            this.m_nEffectEnd = this.m_nEndFrame - 4;
                        }
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    }
                    else if ((new ArrayList(new int[] {113, 114}).Contains(this.m_btRace)))
                    {
                        this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                        this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                        this.m_dwFrameTime = pm.ActAttack.ftime;
                        if (this.m_btRace == 113)
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectFrame = 350 + this.m_btDir * 10;
                            this.m_nEffectStart = this.m_nEffectFrame;
                            this.m_nEffectEnd = this.m_nEffectFrame + 5;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = this.m_dwFrameTime;
                        }
                    }
                    else if ((this.m_btRace == 117) && (this.m_nCurrentAction == Grobal2.SM_LIGHTING_1))
                    {
                        this.m_boUseEffect = true;
                        this.m_nEffectFrame = this.m_nStartFrame + 15;
                        this.m_nEffectStart = this.m_nEffectFrame;
                        this.m_nEffectEnd = this.m_nEffectFrame + 3;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                                            }
                    else if (new ArrayList(new int[] {118, 119}).Contains(this.m_btRace))
                    {
                        this.m_boUseEffect = true;
                        this.m_boUseMagic = false;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                        if (this.m_btRace == 118)
                        {
                                                    }
                        this.m_nEffectFrame = 2750;
                        this.m_nEffectStart = 2750;
                        this.m_nEffectEnd = 2750 + 9;
                        if (this.m_btRace == 119)
                        {
                                                        this.m_nEffectFrame = 2860;
                            this.m_nEffectStart = 2860;
                            this.m_nEffectEnd = 2860 + 9;
                        }
                    }
                    break;
                case Grobal2.SM_LIGHTING_2:
                    if (this.m_btRace == 115)
                    {
                        this.m_nStartFrame = 420 + this.m_btDir * 10;
                        this.m_nEndFrame = this.m_nStartFrame + 9;
                        this.m_dwFrameTime = pm.ActCritical.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_dwWarModeTime = MShare.GetTickCount();
                        this.Shift(this.m_btDir, 0, 0, 1);
                        this.m_boUseEffect = true;
                        this.m_nEffectFrame = 0;
                        this.m_nEffectStart = 0;
                        this.m_nEffectEnd = 10;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                                            }
                    break;
                default:
                    base.CalcActorFrame();
                    break;
            }
        }

        //Constructor  Create()
        public TBanyaGuardMon() : base()
        {
            m_DrawEffect = null;
        }
        public override void DrawEff(TDirectDrawSurface dsurface, int dx, int dy)
        {
            base.DrawEff(dsurface, dx, dy);
            if (this.m_boUseEffect && (m_DrawEffect != null))
            {
                cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + this.ax + this.m_nShiftX, dy + this.ay + this.m_nShiftY, m_DrawEffect, 1);
            }
        }

        public override void LoadSurface()
        {
            if ((this.m_btRace == 117) && this.m_boDeath)
            {
                this.m_BodySurface = null;
                return;
            }
            base.LoadSurface();
            if (this.m_boNowDeath)
            {
                switch(this.m_btRace)
                {
                    case 070:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[21].GetCachedImage(2320 + this.m_nCurrentFrame - this.m_nStartFrame, ref this.n264, ref this.n268);
                        break;
                    case 071:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[21].GetCachedImage(2870 + (this.m_btDir * 10) + this.m_nCurrentFrame - this.m_nStartFrame, ref this.n264, ref this.n268);
                        break;
                    case 078:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(3120 + (this.m_btDir * 4) + this.m_nCurrentFrame - this.m_nStartFrame, ref this.n264, ref this.n268);
                        break;
                    case 111:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[24].GetCachedImage(3710 + this.m_nCurrentFrame - this.m_nStartFrame, ref this.n264, ref this.n268);
                        break;
                    case 113:
                    case 114:
                    case 115:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[33].GetCachedImage(340 + this.m_nCurrentFrame - this.m_nStartFrame, ref this.n264, ref this.n268);
                        if ((this.m_nCurrentFrame - this.m_nStartFrame) == 0)
                        {
                                                    }
                        break;
                    case 118:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[33].GetCachedImage(2770 + this.m_nCurrentFrame - this.m_nStartFrame, ref this.n264, ref this.n268);
                        break;
                    case 119:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[33].GetCachedImage(2880 + this.m_nCurrentFrame - this.m_nStartFrame, ref this.n264, ref this.n268);
                        break;
                }
            }
            else if (this.m_boUseEffect)
            {
                switch(this.m_btRace)
                {
                    case 70:
                        if (this.m_nCurrentAction == Grobal2.SM_HIT)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[21].GetCachedImage(2230 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 71:
                        switch(this.m_nCurrentAction)
                        {
                            case Grobal2.SM_HIT:
                                m_DrawEffect = WMFile.Units.WMFile.g_WMons[21].GetCachedImage(2780 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                break;
                            // Modify the A .. B: Grobal2.SM_FLYAXE .. Grobal2.SM_LIGHTING
                            case Grobal2.SM_FLYAXE:
                                m_DrawEffect = WMFile.Units.WMFile.g_WMons[21].GetCachedImage(2960 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                break;
                        }
                        break;
                    case 72:
                        if (this.m_nCurrentAction == Grobal2.SM_HIT)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[21].GetCachedImage(3490 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        if (this.m_nCurrentAction == Grobal2.SM_LIGHTING)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[21].GetCachedImage(3580 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 78:
                        if (this.m_nCurrentAction == Grobal2.SM_HIT)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[22].GetCachedImage(3440 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 111:
                        if (this.m_nCurrentAction == Grobal2.SM_LIGHTING)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[24].GetCachedImage(3720 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 027:
                        if (this.m_nCurrentAction == Grobal2.SM_LIGHTING)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[26].GetCachedImage(420 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 028:
                        if (this.m_nCurrentAction == Grobal2.SM_LIGHTING)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[26].GetCachedImage(930 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 113:
                        if (this.m_nCurrentAction == Grobal2.SM_LIGHTING)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[33].GetCachedImage(350 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 115:
                        if (this.m_nCurrentAction == Grobal2.SM_LIGHTING_2)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[2].GetCachedImage(this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 117:
                        if (this.m_nCurrentAction == Grobal2.SM_LIGHTING_1)
                        {
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[33].GetCachedImage(2665 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                    case 118:
                        if (!this.m_boNowDeath)
                        {
                            this.m_dwEffectFrameTime = this.m_dwFrameTime;
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[33].GetCachedImage(2730 + this.m_nCurrentFrame, ref this.ax, ref this.ay);
                        }
                        break;
                    case 119:
                        if (!this.m_boNowDeath)
                        {
                            this.m_dwEffectFrameTime = this.m_dwFrameTime;
                            m_DrawEffect = WMFile.Units.WMFile.g_WMons[33].GetCachedImage(2840 + this.m_nCurrentFrame, ref this.ax, ref this.ay);
                        }
                        break;
                }
            }
        }
        }

    } // end TBanyaGuardMon

