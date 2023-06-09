using M2Server.Event.Events;
using SystemModule;

namespace M2Server.Monster.Monsters
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
            if (CanMove())
            {
                short nx = CurrX;
                short ny = CurrY;
                FireBurnEvent fireBurnEvent;
                if (Envir.GetEvent(nx, ny - 1) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, nx, (short)(ny - 1), Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny - 2) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, nx, (short)(ny - 2), Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx - 1, ny) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, (short)(nx - 1), ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx - 2, ny) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, (short)(nx - 2), ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, nx, ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx + 1, ny) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, (short)(nx + 1), ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx + 2, ny) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, (short)(nx + 2), ny, Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny + 1) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, nx, (short)(ny + 1), Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
                }
                if (Envir.GetEvent(nx, ny + 2) == null)
                {
                    fireBurnEvent = new FireBurnEvent(this, nx, (short)(ny + 2), Grobal2.ET_FIRE, FireTime, FireDamage);
                    M2Share.EventMgr.AddEvent(fireBurnEvent);
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

