namespace M2Server
{
    public class TFireMonster : TMonster
    {
        public TFireMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            TFireBurnEvent FireBurnEvent;
            int nx;
            int ny;
            int ndamage;
            int ntime;
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[grobal2.POISON_STONE] == 0)
            {
                ntime = 20;
                ndamage = 10;
                nx = m_nCurrX;
                ny = m_nCurrY;
                if (m_PEnvir.GetEvent(nx, ny - 1) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx, ny - 1, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny - 2) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx, ny - 2, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx - 1, ny) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx - 1, ny, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx - 2, ny) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx - 2, ny, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx, ny, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx + 1, ny) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx + 1, ny, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx + 2, ny) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx + 2, ny, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny + 1) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx, ny + 1, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny + 2) == null)
                {
                    FireBurnEvent = new TFireBurnEvent(this, nx, ny + 2, grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (HUtil32.GetTickCount() - m_dwSearchEnemyTick > 8000 || HUtil32.GetTickCount() - m_dwSearchEnemyTick > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

