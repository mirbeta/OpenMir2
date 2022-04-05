namespace RobotSvr
{
    public class TFireCowFaceMon: TGasKuDeGi
    {
        // -----------------------------------------------------------
        public override int light()
        {
            int result;
            int L;
            L = this.m_nChrLight;
            if (L < 2)
            {
                if (this.m_boUseEffect)
                {
                    L = 2;
                }
            }
            result = L;
            return result;
        }

    } // end TFireCowFaceMon

    } // end TBanyaGuardMon

