using GameSvr.Event.Events;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class FireMonster : MonsterObject
    {
        public FireMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            FireBurnEvent FireBurnEvent;
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                var ntime = 20;
                var ndamage = 10;
                int nx = m_nCurrX;
                int ny = m_nCurrY;
                if (m_PEnvir.GetEvent(nx, ny - 1) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny - 1, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny - 2) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny - 2, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx - 1, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx - 1, ny, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx - 2, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx - 2, ny, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx + 1, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx + 1, ny, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx + 2, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx + 2, ny, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny + 1) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny + 1, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny + 2) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny + 2, Grobal2.ET_FIRE, ntime * 1000, ndamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if ((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

