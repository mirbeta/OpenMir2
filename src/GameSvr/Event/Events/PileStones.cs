using GameSvr.Maps;

namespace GameSvr.Event.Events
{
    public class PileStones : MirEvent
    {
        public PileStones(Envirnoment Envir, int nX, int nY, int nType, int nTime) : base(Envir, nX, nY, nType, nTime, true)
        {
            m_nEventParam = 1;
        }

        public void AddEventParam()
        {
            if (m_nEventParam < 5)
            {
                m_nEventParam++;
            }
        }
    }
}

