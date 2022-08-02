namespace GameSvr
{
    public class TCowMonster : AtMonster
    {
        public TCowMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }
    }
}

