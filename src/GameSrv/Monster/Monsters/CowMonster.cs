namespace GameSrv.Monster.Monsters {
    public class CowMonster : AtMonster {
        public CowMonster() : base() {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }
    }
}

