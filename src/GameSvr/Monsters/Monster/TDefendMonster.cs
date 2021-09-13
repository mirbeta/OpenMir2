namespace GameSvr
{
    public class TDefendMonster : TMonster
    {
        public bool callguardrun = false;

        public TDefendMonster()
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