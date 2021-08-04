using SystemModule;

namespace M2Server
{
    public class TStoneMineEvent : TEvent
    {
        public int m_nMineCount = 0;
        private int m_nAddStoneCount = 0;
        public int m_dwAddStoneMineTick = 0;

        public TStoneMineEvent(TEnvirnoment Envir, int nX, int nY, int nType) : base(Envir, nX, nY, nType, 0, false)
        {
            m_Envir.AddToMapMineEvent(nX, nY, Grobal2.OS_EVENTOBJECT, this);
            m_boVisible = false;
            m_nMineCount = M2Share.RandomNumber.Random(200);
            m_dwAddStoneMineTick = HUtil32.GetTickCount();
            m_boActive = false;
            m_nAddStoneCount = M2Share.RandomNumber.Random(80);
        }

        public void AddStoneMine()
        {
            m_nMineCount = m_nAddStoneCount;
            m_dwAddStoneMineTick = HUtil32.GetTickCount();
        }
    }
}