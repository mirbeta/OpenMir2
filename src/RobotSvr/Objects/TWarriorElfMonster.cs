using SystemModule;

namespace RobotSvr
{
    public class TWarriorElfMonster: TSkeletonOma
    {
        private int oldframe = 0;
        public override void RunFrameAction(int frame)
        {
            TMapEffect meff;
            if (this.m_nCurrentAction == Grobal2.SM_HIT)
            {
                if ((frame == 5) && (oldframe != frame))
                {
                    if (this.m_wAppearance == 704)
                    {
                        meff = new TMapEffect(250 + 10 * this.m_btDir + 1, 6, this.m_nCurrX, this.m_nCurrY);
                        meff.ImgLib = WMFile.Units.WMFile.g_WMagic8Images;
                        meff.NextFrameTime = 100;
                       ClMain.g_PlayScene.m_EffectList.Add(meff);
                    }
                    else if (this.m_wAppearance == 706)
                    {
                        meff = new TMapEffect(330 + 10 * this.m_btDir + 1, 6, this.m_nCurrX, this.m_nCurrY);
                        meff.ImgLib = WMFile.Units.WMFile.g_WMagic8Images;
                        meff.NextFrameTime = 100;
                       ClMain.g_PlayScene.m_EffectList.Add(meff);
                    }
                    else if (this.m_wAppearance == 708)
                    {
                        meff = new TMapEffect(410 + 10 * this.m_btDir + 1, 6, this.m_nCurrX, this.m_nCurrY);
                        meff.ImgLib = WMFile.Units.WMFile.g_WMagic8Images;
                        meff.NextFrameTime = 100;
                       ClMain.g_PlayScene.m_EffectList.Add(meff);
                    }
                    else
                    {
                        meff = new TMapEffect(Units.AxeMon.WARRIORELFFIREBASE + 10 * this.m_btDir + 1, 5, this.m_nCurrX, this.m_nCurrY);
                        meff.ImgLib = WMFile.Units.WMFile.g_WMons[18];
                        meff.NextFrameTime = 100;
                       ClMain.g_PlayScene.m_EffectList.Add(meff);
                    }
                }
                oldframe = frame;
            }
        }

    } // end TWarriorElfMonster

    } // end TBanyaGuardMon

