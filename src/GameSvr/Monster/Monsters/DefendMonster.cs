namespace GameSvr.Monster.Monsters
{
    public class DefendMonster : MonsterObject
    {
        public bool callguardrun = false;

        public DefendMonster()
            : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            base.Run();
        }
    }
}