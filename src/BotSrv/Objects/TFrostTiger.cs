using BotSrv.Player;
using OpenMir2;
using SystemModule;

namespace BotSrv.Objects
{
    public class TFrostTiger : TSkeletonOma
    {
        public bool boActive;
        public bool boCasted;

        public TFrostTiger(RobotPlayer robotClient) : base(robotClient)
        {
            boActive = false;
            boCasted = false;
        }

        public override void Run()
        {
            if (m_nCurrentAction == Messages.SM_LIGHTING && boCasted)
                boCasted = false;
            //ClMain.g_PlayScene.NewMagic(this, 1, 39, this.CurrX, this.CurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, false, 30, ref bofly);
            base.Run();
        }

        public override int GetDefaultFrame(bool wmode)
        {
            var result = 0;
            if (boActive == false)
            {
                var pm = ActorConst.GetRaceByPM(Race, m_wAppearance);
                if (pm == null) return result;
                if (Death)
                {
                    base.GetDefaultFrame(wmode);
                    return result;
                }

                m_nDefFrameCount = pm.ActDeath.frame;
                int cf;
                if (m_nCurrentDefFrame < 0)
                    cf = 0;
                else if (m_nCurrentDefFrame >= pm.ActDeath.frame)
                    cf = 0;
                else
                    cf = m_nCurrentDefFrame;
                result = pm.ActDeath.start + m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip) + cf;
            }
            else
            {
                result = base.GetDefaultFrame(wmode);
            }
            return result;
        }
    }
}