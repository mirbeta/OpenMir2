using SystemModule;

public class TYimoogi: TGasKuDeGi
    {
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
                case Grobal2.SM_FLYAXE:
                    this.m_nStartFrame = pm.ActCritical.start + this.m_btDir * (pm.ActCritical.frame + pm.ActCritical.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                    this.m_dwFrameTime = pm.ActCritical.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boUseEffect = true;
                    this.m_nEffectStart = 0;
                    this.m_nEffectFrame = 0;
                    this.m_nEffectEnd = 6;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = pm.ActCritical.ftime;
                    this.m_nCurEffFrame = 0;
                    break;
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boUseEffect = true;
                    this.m_nEffectStart = 0;
                    this.m_nEffectFrame = 0;
                    this.m_nEffectEnd = 2;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = pm.ActAttack.ftime;
                    this.m_nCurEffFrame = 0;
                    break;
                case Grobal2.SM_HIT:
                    base.CalcActorFrame();
                    this.m_boUseEffect = false;
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boUseEffect = true;
                    this.m_nEffectFrame = 0;
                    this.m_nEffectStart = 0;
                    this.m_nEffectEnd = 10;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = 100;
                    break;
                default:
                    base.CalcActorFrame();
                    break;
            }
        }

        public override void LoadSurface()
        {
            bool bofly;
            TMagicEff meff;
            base.LoadSurface();
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_LIGHTING:
                    this.AttackEffectSurface = null;
                    if ((this.m_nEffectFrame == 1))
                    {
                        meff = new TObjectEffects(this, WMFile.Units.WMFile.g_WMagic2Images, 1330, 10, 100, true);
                        meff.ImgLib = WMFile.Units.WMFile.g_WMagic2Images;
                       ClMain.g_PlayScene.m_EffectList.Add(meff);
                    }
                    break;
                case Grobal2.SM_FLYAXE:
                    this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[23].GetCachedImage(2820 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                    if ((this.m_nEffectFrame == 3))
                    {
                       ClMain.g_PlayScene.NewMagic(this, 93, 93, this.m_nCurrX, this.m_nCurrY, this.m_nCurrX, this.m_nCurrY, 0, magiceff.TMagicType.mtExplosion, false, 30, ref bofly);
                    // PlaySound(8222);
                    }
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[23].GetCachedImage(2900 + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
                    break;
            }
        }

    } // end TYimoogi
