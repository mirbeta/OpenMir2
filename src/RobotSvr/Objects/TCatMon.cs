namespace RobotSvr
{
    public class TCatMon: TSkeletonOma
    {
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
                this.LoadSurface();
            }
            ceff = this.GetDrawEffectValue();
            if (this.m_BodySurface != null)
            {
                this.DrawEffSurface(dsurface, this.m_BodySurface, dx + this.m_nPx + this.m_nShiftX, dy + this.m_nPy + this.m_nShiftY, blend, ceff);
            }
        }

    } // end TCatMon

    } // end TBanyaGuardMon

