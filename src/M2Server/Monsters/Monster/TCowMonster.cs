namespace M2Server
{
    public class TCowMonster : TATMonster
    {
        public TCowMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }
    }
}

