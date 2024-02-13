using BotSrv.Player;
using OpenMir2;

namespace BotSrv.Objects
{
    public class TSkeletonArcherMon : TArcherMon
    {
        public bool m_boNowDeath;
        public int n264 = 0;
        public int n268 = 0;

        public TSkeletonArcherMon(RobotPlayer robotClient) : base(robotClient)
        {

        }

        public override void Run()
        {
            long m_dwFrameTimetime;
            if (m_boMsgMuch)
            {
                m_dwFrameTimetime = HUtil32.Round(m_dwFrameTime * 2 / 3);
            }
            else
            {
                m_dwFrameTimetime = m_dwFrameTime;
            }

            if (m_nCurrentAction != 0)
            {
                if (MShare.GetTickCount() - m_dwStartTime > m_dwFrameTimetime)
                {
                    if (m_nCurrentFrame < m_nEndFrame)
                    {
                    }
                    else
                    {
                        m_nCurrentAction = 0;
                        m_boNowDeath = false;
                    }
                }
            }

            base.Run();
        }
    }
}