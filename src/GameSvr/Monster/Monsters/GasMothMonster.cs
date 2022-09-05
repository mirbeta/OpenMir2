using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class GasMothMonster : GasAttackMonster
    {
        public GasMothMonster() : base()
        {
            ViewRange = 7;
        }

        protected override TBaseObject sub_4A9C78(byte bt05)
        {
            var baseObject = base.sub_4A9C78(bt05);
            if (baseObject != null && M2Share.RandomNumber.Random(3) == 0 && baseObject.HideMode)
            {
                baseObject.m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;
            }
            return baseObject;
        }

        public override void Run()
        {
            if (!Death && !Ghost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    sub_4C959C();
                }
            }
            base.Run();
        }
    }
}