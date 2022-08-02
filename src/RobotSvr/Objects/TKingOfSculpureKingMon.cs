using SystemModule;

namespace RobotSvr
{
    public class TKingOfSculpureKingMon : TGasKuDeGi
    {
        // TKingOfSculpureKingMon
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_nCurrentFrame = -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch (this.m_nCurrentAction)
            {
                case Grobal2.SM_HIT:
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    if (this.m_btRace == 62)
                    {
                        this.m_boUseEffect = true;
                        this.firedir = this.m_btDir;
                        this.m_nEffectFrame = this.m_nStartFrame;
                        this.m_nEffectStart = this.m_nStartFrame;
                        this.m_nEffectEnd = this.m_nEndFrame;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    }
                    break;
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = pm.ActCritical.start + this.m_btDir * (pm.ActCritical.frame + pm.ActCritical.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                    this.m_dwFrameTime = pm.ActCritical.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boUseEffect = true;
                    this.firedir = this.m_btDir;
                    this.m_nEffectFrame = this.m_nStartFrame;
                    this.m_nEffectStart = this.m_nStartFrame;
                    this.m_nEffectEnd = this.m_nEndFrame;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nEffectFrame = pm.ActDie.start;
                    this.m_nEffectStart = pm.ActDie.start;
                    this.m_nEffectEnd = pm.ActDie.start + pm.ActDie.frame - 1;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    this.m_boUseEffect = true;
                    break;
                default:
                    base.CalcActorFrame();
                    break;
            }
        }

        public override void LoadSurface()
        {
            base.LoadSurface();
            if (this.m_boUseEffect)
            {
                switch (this.m_nCurrentAction)
                {
                    case Grobal2.SM_HIT:
                        this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[19].GetCachedImage(1490 + (this.firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        break;
                    case Grobal2.SM_LIGHTING:
                        switch (this.m_btRace)
                        {
                            case 25:
                                this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[25].GetCachedImage(426 + (this.firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                break;
                            case 26:
                                this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[25].GetCachedImage(932 + (this.firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                break;
                            case 62:
                                this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[19].GetCachedImage(1380 + (this.firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                                break;
                        }
                        break;
                    case Grobal2.SM_NOWDEATH:
                        if ((this.m_btRace == 62))
                        {
                            this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[19].GetCachedImage(1470 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                        }
                        break;
                }
            }
        }

    } // end TKingOfSculpureKingMon
}