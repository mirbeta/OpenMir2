namespace GameSrv.Monster.Monsters {
    public class CowMonster : AtMonster {
        public CowMonster() : base() {
            SearchTime = GameShare.RandomNumber.Random(1500) + 1500;
        }
    }
}

