namespace GameSvr
{
    public class DefendMonster : Monster
    {
        public bool callguardrun = false;

        public DefendMonster()
            : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            base.Run();
        }
    }
}