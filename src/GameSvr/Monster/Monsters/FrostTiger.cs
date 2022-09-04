using SystemModule;

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
            if (!Death && !bo554 && !Ghost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (TargetCret == null)
                {
                    if (m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] == 0)
                    {
                        M2Share.MagicManager.MagMakePrivateTransparent(this, 180);
                    }
                }
                else
                {
                    m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 0;
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

