namespace RobotSvr
{
    public class TBossPigMon: TGasKuDeGi
    {
        // TBossPigMon
        public override void LoadSurface()
        {
            base.LoadSurface();
            if ((this.m_btRace == 61) && this.m_boUseEffect)
            {
                this.AttackEffectSurface = WMFile.Units.WMFile.g_WMons[19].GetCachedImage(860 + (this.firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref this.ax, ref this.ay);
            }
        }

    } // end TBossPigMon

    } // end TBanyaGuardMon
