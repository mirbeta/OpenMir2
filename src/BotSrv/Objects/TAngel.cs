using BotSrv.Player;

namespace BotSrv.Objects
{
    public class TAngel : THumActor
    {
        protected int ax = 0;
        protected int ay = 0;

        public TAngel(RobotPlayer robotClient) : base(robotClient)
        {
            m_boUseEffect = false;
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int cf;
            int result = 0;
            TMonsterAction pm = ActorConst.GetRaceByPM(Race, m_wAppearance);
            if (pm == null) return result;
            if (Death)
            {
                result = pm.ActDie.start + m_btDir * (pm.ActDie.frame + pm.ActDie.skip) + (pm.ActDie.frame - 1);
            }
            else
            {
                m_nDefFrameCount = pm.ActStand.frame;
                if (m_nCurrentDefFrame < 0)
                    cf = 0;
                else if (m_nCurrentDefFrame >= pm.ActStand.frame)
                    cf = 0;
                else
                    cf = m_nCurrentDefFrame;
                result = pm.ActStand.start + m_btDir * (pm.ActStand.frame + pm.ActStand.skip) + cf;
            }

            return result;
        }
    }
}