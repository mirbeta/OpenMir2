using SystemModule;

namespace GameSvr
{
    public class TStoneMineEvent : TEvent
    {
        public int m_nMineCount = 0;
        private int m_nAddStoneCount = 0;
        public int m_dwAddStoneMineTick = 0;
        public bool m_boAddToMap = false;

        public TStoneMineEvent(TEnvirnoment Envir, int nX, int nY, int nType) : base(Envir, nX, nY, nType, 0, false)
        {
            m_boAddToMap = true;
            if (nType == 55 || nType == 56 || nType == 57)
            {
                if (m_Envir.AddToMapItemEvent(nX, nY, Grobal2.OS_EVENTOBJECT, this) == null)
                {
                    m_boAddToMap = false;
                }
                else
                {
                    m_boVisible = false;
                    m_nMineCount = M2Share.RandomNumber.Random(2000) + 300;
                    m_dwAddStoneMineTick = HUtil32.GetTickCount();
                    m_boActive = false;
                    m_nAddStoneCount = M2Share.RandomNumber.Random(800) + 100;
                }
            }
            else
            {
                if (m_Envir.AddToMapItemEvent(nX, nY, Grobal2.OS_EVENTOBJECT, this) == null)
                {
                    m_boAddToMap = false;
                }
                else
                {
                    m_boVisible = false;
                    m_nMineCount = M2Share.RandomNumber.Random(200) + 1;
                    m_dwAddStoneMineTick = HUtil32.GetTickCount();
                    m_boActive = false;
                    m_nAddStoneCount = M2Share.RandomNumber.Random(80) + 1;
                }
            }
        }

        public void AddStoneMine()
        {
            m_nMineCount = m_nAddStoneCount;
            m_dwAddStoneMineTick = HUtil32.GetTickCount();
        }
    }
}