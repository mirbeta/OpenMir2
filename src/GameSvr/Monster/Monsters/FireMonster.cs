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
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!Death && !Ghost && StatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                int nx = CurrX;
                int ny = CurrY;
                FireBurnEvent FireBurnEvent;
                if (Envir.GetEvent(nx, ny - 1) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny - 1, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny - 2) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny - 2, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx - 1, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx - 1, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx - 2, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx - 2, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx + 1, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx + 1, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx + 2, ny) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx + 2, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny + 1) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny + 1, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny + 2) == null)
                {
                    FireBurnEvent = new FireBurnEvent(this, nx, ny + 2, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(FireBurnEvent);
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

