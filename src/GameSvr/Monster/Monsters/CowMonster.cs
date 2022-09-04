namespace GameSvr.Monster.Monsters
{
    public class TCowMonster : AtMonster
    {
        public TCowMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }
    }
}

