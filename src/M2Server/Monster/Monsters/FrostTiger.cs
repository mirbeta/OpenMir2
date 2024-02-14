using M2Server.Magic;
using OpenMir2;
using OpenMir2.Consts;

namespace M2Server.Monster.Monsters
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
                    if (StatusTimeArr[PoisonState.STATETRANSPARENT] == 0)
                    {
                        MagicManager.MagMakePrivateTransparent(this, 180);
                    }
                }
                else
                {
                    StatusTimeArr[PoisonState.STATETRANSPARENT] = 0;
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

