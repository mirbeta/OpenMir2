namespace M2Server
{
    public class TMinorNumaObject : TATMonster
    {
        public TMinorNumaObject() : base()
        {

        }

        public override void Run()
        {
            if (!m_boDeath)
            {
                if (((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 8000) || (((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 1000) && (m_TargetCret == null)))
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

