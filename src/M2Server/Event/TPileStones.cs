namespace M2Server
{
    public class TPileStones : TEvent
    {
        public TPileStones(TEnvirnoment Envir, int nX, int nY, int nType, int nTime) : base(Envir, nX, nY, nType, nTime, true)
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

