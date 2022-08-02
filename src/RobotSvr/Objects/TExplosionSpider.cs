using SystemModule;

namespace RobotSvr
{
    public class TExplosionSpider : TGasKuDeGi
    {
        public override void CalcActorFrame()
        {
            base.CalcActorFrame();
            switch (this.m_nCurrentAction)
            {
                case Grobal2.SM_HIT:
                    this.m_boUseEffect = false;
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nEffectStart = this.m_nStartFrame;
                    this.m_nEffectFrame = this.m_nStartFrame;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    this.m_nEffectEnd = this.m_nEndFrame;
                    this.m_boUseEffect = true;
                    break;
            }
        }
    }
}

