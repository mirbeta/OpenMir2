using SystemModule;

namespace RobotSvr
{
    public class TFlyingSpider: TSkeletonOma
    {
        // TFlyingSpider
        public override void CalcActorFrame()
        {
            TNormalDrawEffect Eff8;
            base.CalcActorFrame();
            if (this.m_nCurrentAction == Grobal2.SM_NOWDEATH)
            {
                Eff8 = new TNormalDrawEffect(this.m_nCurrX, this.m_nCurrY, WMFile.Units.WMFile.g_WMons[12], 1420, 20, this.m_dwFrameTime, true);
                if (Eff8 != null)
                {
                    Eff8.MagOwner = MShare.g_MySelf;
                   ClMain.g_PlayScene.m_EffectList.Add(Eff8);
                }
            }
        }

    } // end TFlyingSpider

    } // end TBanyaGuardMon

