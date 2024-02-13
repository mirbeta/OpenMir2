using BotSrv.Player;
using OpenMir2;
using SystemModule;

namespace BotSrv.Objects
{
    public class TRedThunderZuma : TGasKuDeGi
    {
        public bool boCasted;

        public TRedThunderZuma(RobotPlayer robotClient) : base(robotClient)
        {
            boCasted = false;
        }

        public override void Run()
        {
            if (m_nCurrentFrame - m_nStartFrame == 2)
            {
                if (m_nCurrentAction == Messages.SM_LIGHTING)
                {
                    if (boCasted)
                    {
                        boCasted = false;
                        //ClMain.g_PlayScene.NewMagic(this, 80, 80, CurrX, CurrY, m_nTargetX, m_nTargetY,m_nTargetRecog, magiceff.TMagicType.mtRedThunder, false, 30, ref bofly);
                    }
                }
            }

            base.Run();
        }
    }
}