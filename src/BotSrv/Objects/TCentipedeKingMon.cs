using BotSrv.Player;
using OpenMir2;
using SystemModule;

namespace BotSrv.Objects
{
    public class TCentipedeKingMon : TKillingHerb
    {
        public TCentipedeKingMon(RobotPlayer robotClient) : base(robotClient)
        {

        }

        public override void Run()
        {
            if (m_nCurrentAction == Messages.SM_WALK || m_nCurrentAction == Messages.SM_BACKSTEP ||
                m_nCurrentAction == Messages.SM_HORSERUN || m_nCurrentAction == Messages.SM_RUN)
            {
                return;
            }
            if (m_boUseEffect)
            {
                if (MShare.GetTickCount() - m_dwEffectStartTime > m_dwEffectFrameTime)
                {
                    m_dwEffectStartTime = MShare.GetTickCount();
                    if (m_nEffectFrame < m_nEffectEnd)
                        m_nEffectFrame++;
                    else
                        m_boUseEffect = false;
                }
            }
            base.Run();
        }
    }
}