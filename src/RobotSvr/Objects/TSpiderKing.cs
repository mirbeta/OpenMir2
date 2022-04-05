using SystemModule;

public class TSpiderKing: TBanyaGuardMon
    {
        public TDirectDrawSurface m_DrawEffect = null;
        public override void CalcActorFrame()
        {
            base.CalcActorFrame();
            return;
        }

        public override void LoadSurface()
        {
            base.LoadSurface();
            if ((this.m_btRace == 110) && this.m_boUseEffect && (this.m_nCurrentAction == Grobal2.SM_FLYAXE))
            {
                this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[24].GetCachedImage(1100 + (this.m_btDir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
            }
        }

    }

}
