using GameSvr.Event.Events;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class FireMonster : MonsterObject
    {
        /// <summary>
        /// 火墙持续时间
        /// </summary>
        private const int FireTime = 20 * 1000;
        /// <summary>
        /// 火墙伤害值
        /// </summary>
        private const int FireDamage = 10;

        public FireMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                int nx = m_nCurrX;
                int ny = m_nCurrY;
                FireBurnEvent FireBurnEvent;
                if (m_PEnvir.GetEvent(nx, ny - 1) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny - 1, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny - 2) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny - 2, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx - 1, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx - 1, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx - 2, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx - 2, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx + 1, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx + 1, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx + 2, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx + 2, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny + 1) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny + 1, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventManager.AddEvent(FireBurnEvent);
                }
                if (m_PEnvir.GetEvent(nx, ny + 2) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny + 2, Grobal2.ET_FIRE, FireTime, FireDamage);
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

