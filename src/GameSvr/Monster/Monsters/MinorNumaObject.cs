namespace GameSvr.Monster.Monsters
{
    public class MinorNumaObject : AtMonster
    {
        public MinorNumaObject() : base()
        {

        }

        public override void Run()
        {
            if (!Death)
            {
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

