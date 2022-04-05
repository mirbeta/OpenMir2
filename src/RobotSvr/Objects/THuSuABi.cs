namespace RobotSvr
{
    public class THuSuABi: TSkeletonOma
    {
        // ============================== THuSuABi =============================
        public override void LoadSurface()
        {
            base.LoadSurface();
            if (this.m_boUseEffect)
            {
                this.EffectSurface = WMFile.Units.WMFile.g_WMons[3].GetCachedImage(Units.AxeMon.DEATHFIREEFFECTBASE + this.m_nCurrentFrame - this.m_nStartFrame, ref this.ax, ref this.ay);
            }
        }

    }

    } // end TBanyaGuardMon

