using SystemModule;
using SystemModule.Consts;

namespace GameSvr.Monster.Monsters
{
    public class FrostTiger : MonsterObject
    {
        public FrostTiger() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (CanMove())
            {
                if (TargetCret == null)
                {
                    if (StatusArr[PoisonState.STATE_TRANSPARENT] == 0)
                    {
                        M2Share.MagicMgr.MagMakePrivateTransparent(this, 180);
                    }
                }
                else
                {
                    StatusArr[PoisonState.STATE_TRANSPARENT] = 0;
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

