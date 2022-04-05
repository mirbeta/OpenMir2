using SystemModule;

public class TCrystalSpider: TGasKuDeGi
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
                case Grobal2.SM_LIGHTING:
                    // DScreen.AddChatBoardString(IntToStr(m_nCurrentAction), clYellow, clRed);
                    this.m_nStartFrame = pm.ActCritical.start + this.m_btDir * (pm.ActCritical.frame + pm.ActCritical.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                    this.m_dwFrameTime = pm.ActCritical.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boUseEffect = true;
                    this.m_nEffectStart = 0;
                    this.m_nEffectFrame = 0;
                    this.m_nEffectEnd = 10;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = 50;
                    this.m_nCurEffFrame = 0;
                    break;
                case Grobal2.SM_HIT:
                    base.CalcActorFrame();
                    this.m_boUseEffect = false;
                    break;
                default:
                    base.CalcActorFrame();
                    break;
            }
        }

        public override void LoadSurface()
        {
            base.LoadSurface();
            if (this.m_boUseEffect && (this.m_nCurrentAction == Grobal2.SM_LIGHTING))
            {
                this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[23].GetCachedImage(2230 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
            }
            // case m_nCurrentAction of
            // SM_LIGHTING: begin
            // AttackEffectSurface := g_WMon23Img.GetCachedImage(2230 + (m_btDir * 10) + m_nEffectFrame - m_nEffectStart, ax, ay);
            // end;
            // end;

        }

    } // end TCrystalSpider
